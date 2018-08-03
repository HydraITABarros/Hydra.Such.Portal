using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.Logic.Nutrition;
using Hydra.Such.Data.ViewModel.Nutrition;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Portal.Configurations;
using Microsoft.Extensions.Options;
using static Hydra.Such.Data.Enumerations;
using Hydra.Such.Data;

namespace Hydra.Such.Portal.Areas.Nutricao.Controllers
{
    public class CafetariasRefeitoriosController : Controller
    {
        private readonly NAVConfigurations config;

        public CafetariasRefeitoriosController(IOptions<NAVConfigurations> appSettings)
        {
            config = appSettings.Value;
        }

      
        public IActionResult Index()
        {
            UserAccessesViewModel userPermissions = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Cafetarias_Refeitórios);

            if (userPermissions != null && userPermissions.Read.Value)
            {
                ViewBag.UserPermissions = userPermissions;
                return View();
            }
            else
            {
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }

        [HttpPost]
       
        public JsonResult GetCoffeeShops()
        {
            var items = DBCoffeeShops.GetAll().ParseToViewModel(config.NAVDatabaseName, config.NAVCompanyName);
            if (items != null)
            {
                //Apply User Dimensions Validations
                List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
                if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.Region).Count() > 0)
                    items.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && (y.ValorDimensão == x.CodeRegion || string.IsNullOrEmpty(x.CodeRegion))));
                if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                    items.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && (y.ValorDimensão == x.CodeFunctionalArea || string.IsNullOrEmpty(x.CodeFunctionalArea))));
                if (userDimensions.Where(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                    items.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && (y.ValorDimensão == x.CodeResponsabilityCenter || string.IsNullOrEmpty(x.CodeResponsabilityCenter))));
            }
            return Json(items);
        }

       
        public IActionResult Detalhes(int productivityUnitNo, int type, int code, string explorationStartDate)
        {
            UserAccessesViewModel userPermissions = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Cafetarias_Refeitórios);

            if (userPermissions != null && userPermissions.Read.Value)
            {
                ViewBag.UPermissions = userPermissions;

                ViewBag.ProductivityUnityNo = productivityUnitNo;
                ViewBag.Type = type;
                ViewBag.Code = code;
                ViewBag.ExplorationStartDate = explorationStartDate;
                ViewBag.ReportServerURL = config.ReportServerURL;

                return View();
            }
            else
            {
                return Redirect(Url.Content("~/Error/AccessDenied"));
            }
        }

        [HttpPost]
      
        public JsonResult GetCoffeeShop([FromBody] Newtonsoft.Json.Linq.JObject requestParams)
        {
            CoffeeShopViewModel item;
            if (requestParams != null)
            {
                int productivityUnitNo = int.Parse(requestParams["productivityUnitNo"].ToString());
                int type = int.Parse(requestParams["type"].ToString());
                int code = int.Parse(requestParams["code"].ToString());
                string explorationStartDate = requestParams["explorationStartDate"].ToString();

                DateTime date;
                if (DateTime.TryParse(explorationStartDate, out date))
                {
                    var coffeeShop = DBCoffeeShops.GetById(productivityUnitNo, type, code, date);
                    item = DBCoffeeShops.ParseToViewModel(coffeeShop, config.NAVDatabaseName, config.NAVCompanyName);
                }
                else
                {
                    item = new CoffeeShopViewModel();
                }
            }
            else
            {
                item = new CoffeeShopViewModel();
            }
            return Json(item);
        }

        [HttpPost]
       
        public JsonResult CreateCoffeeShop([FromBody] CoffeeShopViewModel data)
        {
            try
            {
                if (data != null)
                {
                    data.CreateUser = User.Identity.Name;
                    CafetariasRefeitórios itemToCreate = DBCoffeeShops.ParseToDB(data);

                    itemToCreate = DBCoffeeShops.Create(itemToCreate);

                    if (itemToCreate != null)
                    {
                        data = DBCoffeeShops.ParseToViewModel(itemToCreate, config.NAVDatabaseName, config.NAVCompanyName);
                        data.eReasonCode = 1;
                        data.eMessage = "Registo criado com sucesso.";
                    }
                    else
                    {
                        data.eReasonCode = 3;
                        data.eMessage = "Ocorreu um erro ao inserir os dados na base de dados.";
                    }
                }
            }
            catch (Exception)
            {
                data.eReasonCode = 2;
                data.eMessage = "Ocorreu um erro ao criar a cafetaria / refeitório.";
            }
            return Json(data);
        }

        [HttpPost]
      
        public JsonResult UpdateCoffeeShop([FromBody] CoffeeShopViewModel item)
        {
            if (item != null)
            {
                item.UpdateUser = User.Identity.Name;
                CafetariasRefeitórios updatedItem = DBCoffeeShops.ParseToDB(item);
                updatedItem = DBCoffeeShops.Update(updatedItem);

                if (updatedItem != null)
                {
                    item = DBCoffeeShops.ParseToViewModel(updatedItem, config.NAVDatabaseName, config.NAVCompanyName);
                    item.eReasonCode = 1;
                    item.eMessage = "Cafetaria / refeitório atualizado com sucesso.";
                }
                else
                {
                    item.eReasonCode = 2;
                    item.eMessage = "Ocorreu um erro ao atualizar a cafetaria / refeitório.";
                }
            }
            else
            {
                item = new CoffeeShopViewModel()
                {
                    eReasonCode = 3,
                    eMessage = "Ocorreu um erro ao atualizar. A cafetaria / refeitório não pode ser nulo."
                };
            }
            return Json(item);
        }

        [HttpPost]
      
        public JsonResult DeleteCoffeeShop([FromBody] CoffeeShopViewModel item)
        {
            ErrorHandler errorHandler = new ErrorHandler()
            {
                eReasonCode = 2,
                eMessage = "Ocorreu um erro ao eliminar o registo."
            };

            try
            {
                if (item != null)
                {
                    bool sucess = DBCoffeeShops.Delete(DBCoffeeShops.ParseToDB(item));
                    if (sucess)
                    {
                        item.eReasonCode = 1;
                        item.eMessage = "Registo eliminado com sucesso.";
                    }
                }
                else
                {
                    item = new CoffeeShopViewModel();
                }
            }
            catch
            {
                item.eReasonCode = errorHandler.eReasonCode;
                item.eMessage = errorHandler.eMessage;
            }
            return Json(item);
        }
    }
}