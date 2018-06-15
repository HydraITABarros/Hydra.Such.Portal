using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.Logic;
using Newtonsoft.Json.Linq;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.Nutrition;
using Hydra.Such.Data.ViewModel.Nutrition;
using Hydra.Such.Data.Logic.Contracts;
using Hydra.Such.Portal.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Hydra.Such.Data;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class UnidadeArmazenamentoController : Controller
    {
        private readonly NAVConfigurations _config;
        public UnidadeArmazenamentoController(IOptions<NAVConfigurations> appSettings)
        {
            _config = appSettings.Value;
        }

        public IActionResult Index()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.DiárioProjeto);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult Detalhes(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.DiárioProjeto);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.ProductNo = id ?? "";
                if (ViewBag.ProductNo != "")
                {
                    ViewBag.NoProductDisable = true;
                    ViewBag.ButtonHide = 0;
                }
                else
                {
                    ViewBag.NoProductDisable = false;
                    ViewBag.ButtonHide = 1;
                }

                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public JsonResult GetUnitStockeeping()
        {
            List<StockkeepingUnitViewModel> result = DBStockkeepingUnit.ParseToViewModel(DBStockkeepingUnit.GetAll());
            return Json(result);
        }

        public JsonResult GetUnitStockeepingId([FromBody]string idStock)
        {
            StockkeepingUnitViewModel result = DBStockkeepingUnit.ParseToViewModel(DBStockkeepingUnit.GetById(idStock));
            return Json(result);
        }

        [HttpPost]

        public JsonResult GetProductId([FromBody]string idProduct)
        {
            List<NAVProductsViewModel> product = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, idProduct);
            return Json(product);
        }
        
        public JsonResult CreateUnitStockeeping([FromBody] StockkeepingUnitViewModel data)
        {
            string eReasonCode = "";
            //Update 
            eReasonCode = DBStockkeepingUnit.Create(DBStockkeepingUnit.ParseToDb(data)) == null ? "101" : "";

            if (String.IsNullOrEmpty(eReasonCode))
            {
                return Json(data);
            }
            else
            {
                return Json(eReasonCode);
            }
        }

        public JsonResult DeleteUnitStockeeping([FromBody] StockkeepingUnitViewModel data)
        {
           
            string eReasonCode = "";
            //Create new 
            eReasonCode = DBStockkeepingUnit.Delete(DBStockkeepingUnit.ParseToDb(data)) == true ? "103" : "";
            
            if (String.IsNullOrEmpty(eReasonCode))
            {
                return Json(null);
            }
            else
            {
                return Json(eReasonCode);
            }
        }

        public JsonResult  UpdateUnitStockeeping([FromBody] StockkeepingUnitViewModel data)
        {
            string eReasonCode = "";
            //Create new 
            eReasonCode = DBStockkeepingUnit.Update(DBStockkeepingUnit.ParseToDb(data)) == null ? "102" : "";

            if (String.IsNullOrEmpty(eReasonCode))
            {
                return Json(data);
            }
            else
            {
                return Json(eReasonCode);
            }
        }

        #region Movimento Produtos

        public IActionResult MovimentoProdutos(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.DiárioProjeto);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.ProjectNo = id ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public JsonResult GetMovementProduct()
        {
            List<ProductMovementViewModel> result;
            if (HttpContext.Session.GetString("productNo") == null)
            {
                result = DBProductMovement.ParseToViewModel(DBProductMovement.GetAll());
            }
            else
            {
                string nProduct = HttpContext.Session.GetString("productNo");
                string codeLocation = HttpContext.Session.GetString("codLocation");
                result = DBProductMovement.ParseToViewModel(DBProductMovement.GetByNoprodLocation(nProduct, codeLocation));
                HttpContext.Session.Remove("productNo");
                HttpContext.Session.Remove("codLocation");
            }
            if (result != null)
            {
                //Apply User Dimensions Validations
                List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
                //Regions
                if (userDimensions.Where(y => y.Dimensão == (int)Enumerations.Dimensions.Region).Count() > 0)
                    result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Enumerations.Dimensions.Region && (y.ValorDimensão == x.CodeRegion || string.IsNullOrEmpty(x.CodeRegion))));
                //FunctionalAreas
                if (userDimensions.Where(y => y.Dimensão == (int)Enumerations.Dimensions.FunctionalArea).Count() > 0)
                    result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Enumerations.Dimensions.FunctionalArea && (y.ValorDimensão == x.CodeFunctionalArea || string.IsNullOrEmpty(x.CodeFunctionalArea))));
                //ResponsabilityCenter
                if (userDimensions.Where(y => y.Dimensão == (int)Enumerations.Dimensions.ResponsabilityCenter).Count() > 0)
                    result.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Enumerations.Dimensions.ResponsabilityCenter && (y.ValorDimensão == x.CodeResponsabilityCenter || string.IsNullOrEmpty(x.CodeResponsabilityCenter))));
            }
            return Json(result);
        }


        public bool SetSessionMovimentoProduto([FromBody] StockkeepingUnitViewModel data)
        {
            if (data.ProductNo != null)
            {
                HttpContext.Session.SetString("productNo", data.ProductNo);
                HttpContext.Session.SetString("codLocation", data.Code);
                return true;
            }
            return false;
        }
        #endregion
    }
}