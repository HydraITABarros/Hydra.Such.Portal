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
using Hydra.Such.Data.NAV;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class UnidadeArmazenamentoController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private readonly IHostingEnvironment _hostingEnvironment;
        public UnidadeArmazenamentoController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IHostingEnvironment _hostingEnvironment)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            this._hostingEnvironment = _hostingEnvironment;
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

        //1
        [HttpPost]
        public async Task<JsonResult> ExportToExcel_UnidadesArmazenamento([FromBody] List<StockkeepingUnitViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\Upload\\temp";
            string user = User.Identity.Name;
            user = user.Replace("@", "_");
            user = user.Replace(".", "_");
            string sFileName = @"" + user + ".xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Unidades Armazenamento");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["productNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº de Produto");
                    Col = Col + 1;
                }
                if (dp["code"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Localização");
                    Col = Col + 1;
                }
                if (dp["description"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Descrição");
                    Col = Col + 1;
                }
                if (dp["inventory"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Inventário");
                    Col = Col + 1;
                }
                if (dp["blocked"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Bloqueado");
                    Col = Col + 1;
                }
                if (dp["codeUnitMeasure"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Unidade Medida Produto");
                    Col = Col + 1;
                }
                if (dp["unitCost"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Custo Unitário");
                    Col = Col + 1;
                }
                if (dp["wareHouseValue"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Valor em Armazem");
                    Col = Col + 1;
                }
                if (dp["shelfNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Prateleiras");
                    Col = Col + 1;
                }
                if (dp["vendorNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Fornecedor");
                    Col = Col + 1;
                }
                if (dp["vendorItemNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Produto Fornecedor");
                    Col = Col + 1;
                }

                if (dp != null)
                {
                    int count = 1;
                    foreach (StockkeepingUnitViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["productNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ProductNo);
                            Col = Col + 1;
                        }
                        if (dp["code"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Code);
                            Col = Col + 1;
                        }
                        if (dp["description"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Description);
                            Col = Col + 1;
                        }
                        if (dp["inventory"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Inventory.ToString());
                            Col = Col + 1;
                        }
                        if (dp["blocked"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Blocked.ToString());
                            Col = Col + 1;
                        }
                        if (dp["codeUnitMeasure"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodeUnitMeasure);
                            Col = Col + 1;
                        }
                        if (dp["unitCost"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.UnitCost.ToString());
                            Col = Col + 1;
                        }
                        if (dp["wareHouseValue"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.WareHouseValue.ToString());
                            Col = Col + 1;
                        }
                        if (dp["shelfNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ShelfNo);
                            Col = Col + 1;
                        }
                        if (dp["vendorNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.VendorNo);
                            Col = Col + 1;
                        }
                        if (dp["vendorItemNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.VendorItemNo);
                            Col = Col + 1;
                        }
                        count++;
                    }
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return Json(sFileName);
        }
        //2
        public IActionResult ExportToExcelDownload_UnidadesArmazenamento(string sFileName)
        {
            sFileName = @"/Upload/temp/" + sFileName;
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Unidades Armazenamento.xlsx");
        }

    }
}