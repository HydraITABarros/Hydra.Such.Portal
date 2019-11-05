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
using System.Text;
using NPOI.HSSF.UserModel;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class UnidadeArmazenamentoController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly GeneralConfigurations _generalConfig;

        public UnidadeArmazenamentoController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IHostingEnvironment _hostingEnvironment, IOptions<GeneralConfigurations> appSettingsGeneral)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            this._hostingEnvironment = _hostingEnvironment;
            _generalConfig = appSettingsGeneral.Value;
        }

        public IActionResult Index()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Localizações);
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
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Localizações);
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
            FichaProdutoViewModel product = DBFichaProduto.ParseToViewModel(DBFichaProduto.GetById(idProduct));
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

        public JsonResult UpdateUnitStockeeping([FromBody] StockkeepingUnitViewModel data)
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
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Localizações);
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
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_UnidadesArmazenamento([FromBody] List<StockkeepingUnitViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "UnidadesArmazenamento\\" + "tmp\\";
            string user = User.Identity.Name;
            user = user.Replace("@", "_");
            user = user.Replace(".", "_");
            string sFileName = @"" + user + "_ExportEXCEL.xlsx";
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
                if (dp["description"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Descrição");
                    Col = Col + 1;
                }
                if (dp["code"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Localização");
                    Col = Col + 1;
                }
                if (dp["inventory"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Inventário");
                    Col = Col + 1;
                }
                if (dp["shelfNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Prateleiras");
                    Col = Col + 1;
                }
                if (dp["blocked"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Bloqueado");
                    Col = Col + 1;
                }
                if (dp["codeWareHouse"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Armazém Principal");
                    Col = Col + 1;
                }
                if (dp["wareHouseValue"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Valor em Armazem");
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
                if (dp["lastCostDirect"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Último Custo Directo");
                    Col = Col + 1;
                }
                if (dp["priceSale"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Preço de Venda");
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
                        if (dp["description"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Description);
                            Col = Col + 1;
                        }
                        if (dp["code"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Code);
                            Col = Col + 1;
                        }
                        if (dp["inventory"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Inventory.ToString());
                            Col = Col + 1;
                        }
                        if (dp["shelfNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ShelfNo);
                            Col = Col + 1;
                        }
                        if (dp["blocked"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Blocked.ToString());
                            Col = Col + 1;
                        }
                        if (dp["codeWareHouse"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodeWareHouse.ToString());
                            Col = Col + 1;
                        }
                        if (dp["wareHouseValue"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.WareHouseValue.ToString());
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
                        if (dp["lastCostDirect"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.LastCostDirect.ToString());
                            Col = Col + 1;
                        }
                        if (dp["priceSale"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.PriceSale.ToString());
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
            sFileName = _generalConfig.FileUploadFolder + "UnidadesArmazenamento\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Unidades Armazenamento.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_MovimentosProdutos([FromBody] List<ProductMovementViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "UnidadesArmazenamento\\" + "tmp\\";
            string user = User.Identity.Name;
            user = user.Replace("@", "_");
            user = user.Replace(".", "_");
            string sFileName = @"" + user + "_ExportEXCEL.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Movimentos de Produtos");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["movementNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº de Movimento");
                    Col = Col + 1;
                }
                if (dp["dateRegister"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data do Registo");
                    Col = Col + 1;
                }
                if (dp["movementType"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Tipo de Movimento");
                    Col = Col + 1;
                }
                if (dp["documentNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº de Documento");
                    Col = Col + 1;
                }
                if (dp["productNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº do Produto");
                    Col = Col + 1;
                }
                if (dp["description"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Descrição");
                    Col = Col + 1;
                }
                if (dp["codLocation"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Código de Localização");
                    Col = Col + 1;
                }
                if (dp["quantity"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Quantidade");
                    Col = Col + 1;
                }
                if (dp["unitCost"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Custo Unitário");
                    Col = Col + 1;
                }
                if (dp["val"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Valor");
                    Col = Col + 1;
                }
                if (dp["projectNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº de Projeto");
                    Col = Col + 1;
                }
                if (dp["codeRegion"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Código de Região");
                    Col = Col + 1;
                }
                if (dp["codeFunctionalArea"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Código de Área");
                    Col = Col + 1;
                }
                if (dp["codeResponsabilityCenter"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Código Centro de Responsabilidade");
                    Col = Col + 1;
                }

                if (dp != null)
                {
                    int count = 1;
                    foreach (ProductMovementViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["movementNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.MovementNo);
                            Col = Col + 1;
                        }
                        if (dp["dateRegister"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.DateRegister);
                            Col = Col + 1;
                        }
                        if (dp["movementType"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.MovementType.ToString());
                            Col = Col + 1;
                        }
                        if (dp["documentNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.DocumentNo.ToString());
                            Col = Col + 1;
                        }
                        if (dp["productNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ProductNo);
                            Col = Col + 1;
                        }
                        if (dp["description"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Description);
                            Col = Col + 1;
                        }
                        if (dp["codLocation"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodLocation);
                            Col = Col + 1;
                        }
                        if (dp["quantity"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Quantity.ToString());
                            Col = Col + 1;
                        }
                        if (dp["unitCost"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.UnitCost.ToString());
                            Col = Col + 1;
                        }
                        if (dp["val"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Val.ToString());
                            Col = Col + 1;
                        }
                        if (dp["projectNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ProjectNo.ToString());
                            Col = Col + 1;
                        }
                        if (dp["codeRegion"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodeRegion);
                            Col = Col + 1;
                        }
                        if (dp["codeFunctionalArea"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodeFunctionalArea);
                            Col = Col + 1;
                        }
                        if (dp["codeResponsabilityCenter"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodeResponsabilityCenter);
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
        public IActionResult ExportToExcelDownload_MovimentosProdutos(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "UnidadesArmazenamento\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Movimentos de Produtos.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        //3
        [HttpPost]
        public JsonResult OnPostImport_UnidadesArmazenamento()
        {
            var files = Request.Form.Files;
            List<StockkeepingUnitViewModel> ListToCreate = DBStockkeepingUnit.ParseToViewModel(DBStockkeepingUnit.GetAll());
            StockkeepingUnitViewModel nrow = new StockkeepingUnitViewModel();
            for (int i = 0; i < files.Count; i++)
            {
                IFormFile file = files[i];
                string folderName = "Upload";
                string webRootPath = _generalConfig.FileUploadFolder + "UnidadesArmazenamento\\" + "tmp\\";
                string newPath = Path.Combine(webRootPath, folderName);
                StringBuilder sb = new StringBuilder();
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                    ISheet sheet;
                    string fullPath = Path.Combine(newPath, file.FileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        stream.Position = 0;
                        if (sFileExtension == ".xls")
                        {
                            HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                        }
                        else
                        {
                            XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                        }
                        for (int j = (sheet.FirstRowNum + 1); j <= sheet.LastRowNum; j++)
                        {
                            IRow row = sheet.GetRow(j);
                            if (row != null)
                            {
                                nrow = new StockkeepingUnitViewModel();

                                nrow.ProductNo = row.GetCell(0) != null ? row.GetCell(0).ToString() : "";
                                nrow.Description = row.GetCell(1) != null ? row.GetCell(1).ToString() : "";
                                nrow.Code = row.GetCell(2) != null ? row.GetCell(2).ToString() : "";
                                nrow.InventoryText = row.GetCell(3) != null ? row.GetCell(3).ToString() : "";
                                nrow.ShelfNo = row.GetCell(4) != null ? row.GetCell(4).ToString() : "";
                                nrow.BlockedText = row.GetCell(5) != null ? row.GetCell(5).ToString() : "";
                                nrow.CodeWareHouseText = row.GetCell(6) != null ? row.GetCell(6).ToString() : "";
                                nrow.WareHouseValueText = row.GetCell(7) != null ? row.GetCell(7).ToString() : "";
                                nrow.VendorNo = row.GetCell(8) != null ? row.GetCell(8).ToString() : "";
                                nrow.VendorItemNo = row.GetCell(9) != null ? row.GetCell(9).ToString() : "";
                                nrow.CodeUnitMeasure = row.GetCell(10) != null ? row.GetCell(10).ToString() : "";
                                nrow.UnitCostText = row.GetCell(11) != null ? row.GetCell(11).ToString() : "";
                                nrow.LastCostDirectText = row.GetCell(12) != null ? row.GetCell(12).ToString() : "";
                                nrow.PriceSaleText = row.GetCell(13) != null ? row.GetCell(13).ToString() : "";

                                ListToCreate.Add(nrow);
                            }
                        }
                    }
                }
                if (ListToCreate.Count > 0)
                {
                    foreach (StockkeepingUnitViewModel item in ListToCreate)
                    {
                        if (!string.IsNullOrEmpty(item.InventoryText))
                        {
                            item.Inventory = Convert.ToDecimal(item.InventoryText);
                            item.InventoryText = "";
                        }
                        if (!string.IsNullOrEmpty(item.BlockedText))
                        {
                            item.Blocked = item.BlockedText.ToLower() == "sim" ? true : false;
                            item.BlockedText = "";
                        }
                        if (!string.IsNullOrEmpty(item.CodeWareHouseText))
                        {
                            item.CodeWareHouse = item.CodeWareHouseText.ToLower() == "sim" ? true : false;
                            item.CodeWareHouseText = "";
                        }
                        if (!string.IsNullOrEmpty(item.WareHouseValueText))
                        {
                            item.WareHouseValue = Convert.ToDecimal(item.WareHouseValueText);
                            item.WareHouseValueText = "";
                        }
                        if (!string.IsNullOrEmpty(item.UnitCostText))
                        {
                            item.UnitCost = Convert.ToDecimal(item.UnitCostText);
                            item.UnitCostText = "";
                        }
                        if (!string.IsNullOrEmpty(item.LastCostDirectText))
                        {
                            item.LastCostDirect = Convert.ToDecimal(item.LastCostDirectText);
                            item.LastCostDirectText = "";
                        }
                        if (!string.IsNullOrEmpty(item.PriceSaleText))
                        {
                            item.PriceSale = Convert.ToDecimal(item.PriceSaleText);
                            item.PriceSaleText = "";
                        }
                    }
                }
            }
            return Json(ListToCreate);
        }
        //4
        [HttpPost]
        public JsonResult UpdateCreate_UnidadesArmazenamento([FromBody] List<StockkeepingUnitViewModel> data)
        {
            List<StockkeepingUnitViewModel> results = DBStockkeepingUnit.ParseToViewModel(DBStockkeepingUnit.GetAll());

            data.RemoveAll(x => results.Any(
                u =>
                    u.ProductNo == x.ProductNo &&
                    u.Description == x.Description &&
                    u.Code == x.Code &&
                    u.Inventory == x.Inventory &&
                    u.ShelfNo == x.ShelfNo &&
                    u.Blocked == x.Blocked &&
                    u.CodeWareHouse == x.CodeWareHouse &&
                    u.WareHouseValue == x.WareHouseValue &&
                    u.VendorNo == x.VendorNo &&
                    u.VendorItemNo == x.VendorItemNo &&
                    u.CodeUnitMeasure == x.CodeUnitMeasure &&
                    u.UnitCost == x.UnitCost &&
                    u.LastCostDirect == x.LastCostDirect &&
                    u.PriceSale == x.PriceSale
            ));

            data.ForEach(x =>
            {
                if (!string.IsNullOrEmpty(x.ProductNo) && DBFichaProduto.GetById(x.ProductNo) != null)
                {
                    UnidadeDeArmazenamento toCreate = DBStockkeepingUnit.ParseToDb(x);
                    UnidadeDeArmazenamento toUpdate = DBStockkeepingUnit.ParseToDb(x);
                    UnidadeDeArmazenamento toSearch = DBStockkeepingUnit.GetById(x.ProductNo);

                    FichaProdutoViewModel product = DBFichaProduto.ParseToViewModel(DBFichaProduto.GetById(x.ProductNo));
                    LocationViewModel localizacao = DBLocations.ParseToViewModel(DBLocations.GetById(x.Code));
                    NAVVendorViewModel fornecedor = DBNAV2017Vendor.GetVendor(_config.NAVDatabaseName, _config.NAVCompanyName).Where(y => y.No_ == x.VendorNo).FirstOrDefault();
                    UnidadeMedidaViewModel unidadeMedida = DBUnidadeMedida.ParseToViewModel(DBUnidadeMedida.GetById(x.CodeUnitMeasure));

                    if (toSearch == null)
                    {
                        toCreate.NºProduto = x.ProductNo;
                        toCreate.Descrição = product.Descricao;
                        if (localizacao != null)
                            toCreate.CódLocalização = x.Code;
                        else
                            toCreate.CódLocalização = null;
                        toCreate.Inventário = x.Inventory;
                        toCreate.NºPrateleira = x.ShelfNo;
                        toCreate.Bloqueado = x.Blocked.HasValue ? x.Blocked.ToString().ToLower() == "sim" ? true : false : false;
                        toCreate.ArmazémPrincipal = x.CodeWareHouse.HasValue ? x.CodeWareHouse.ToString().ToLower() == "sim" ? true : false : false;
                        toCreate.ValorEmArmazem = x.WareHouseValue;
                        if (fornecedor != null)
                            toCreate.NºFornecedor = x.VendorNo;
                        else
                            toCreate.NºFornecedor = null;
                        toCreate.CódProdForn = x.VendorItemNo;
                        if (unidadeMedida != null)
                            toCreate.CódUnidadeMedidaProduto = x.CodeUnitMeasure;
                        else
                            toCreate.CódUnidadeMedidaProduto = null;
                        toCreate.CustoUnitário = x.UnitCost;
                        toCreate.UltimoCustoDirecto = x.LastCostDirect;
                        toCreate.PreçoDeVenda = x.PriceSale;
                        toCreate.UtilizadorCriação = User.Identity.Name;
                        toCreate.DataHoraCriação = DateTime.Now;

                        DBStockkeepingUnit.Create(toCreate);
                    }
                    else
                    {
                        toUpdate.NºProduto = x.ProductNo;
                        toUpdate.Descrição = product.Descricao;
                        if (localizacao != null)
                            toUpdate.CódLocalização = x.Code;
                        else
                            toUpdate.CódLocalização = null;
                        toUpdate.Inventário = x.Inventory;
                        toUpdate.NºPrateleira = x.ShelfNo;
                        toUpdate.Bloqueado = x.Blocked.HasValue ? x.Blocked.ToString().ToLower() == "sim" ? true : false : false;
                        toUpdate.ArmazémPrincipal = x.CodeWareHouse.HasValue ? x.CodeWareHouse.ToString().ToLower() == "sim" ? true : false : false;
                        toUpdate.ValorEmArmazem = x.WareHouseValue;
                        if (fornecedor != null)
                            toUpdate.NºFornecedor = x.VendorNo;
                        else
                            toUpdate.NºFornecedor = null;
                        toUpdate.CódProdForn = x.VendorItemNo;
                        if (unidadeMedida != null)
                            toUpdate.CódUnidadeMedidaProduto = x.CodeUnitMeasure;
                        else
                            toUpdate.CódUnidadeMedidaProduto = null;
                        toUpdate.CustoUnitário = x.UnitCost;
                        toUpdate.UltimoCustoDirecto = x.LastCostDirect;
                        toUpdate.PreçoDeVenda = x.PriceSale;
                        toUpdate.UtilizadorModificação = User.Identity.Name;
                        toUpdate.DataHoraModificação = DateTime.Now;

                        DBStockkeepingUnit.Update(toUpdate);
                    }
                }
            });
            return Json(data);
        }

    }
}