using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.ComprasML;
using Hydra.Such.Data.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Compras;
using Hydra.Such.Data.Logic.Request;
using Hydra.Such.Data;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.NAV;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Hydra.Such.Data.ViewModel.Encomendas;
using Newtonsoft.Json;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class EncomendasController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private readonly IHostingEnvironment _hostingEnvironment;

        public EncomendasController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IOptions<GeneralConfigurations> appSettingsGeneral, IHostingEnvironment _hostingEnvironment)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            this._hostingEnvironment = _hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Encomendas);

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

        [HttpGet]
        public IActionResult DetalhesEncomenda(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Encomendas);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.No = id ?? "";
                ViewBag.reportServerURL = _config.ReportServerURL;
                ViewBag.userLogin = User.Identity.Name.ToString();
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetList([FromBody] JObject requestParams)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Encomendas);

            if (UPerm != null && UPerm.Read.Value)
            {
                List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
                var result = DBNAV2017Encomendas.ListByDimListAndNoFilter(_config.NAVDatabaseName, _config.NAVCompanyName, userDimensions, "C%");
                return Json(result);
            }
            else
            {
                return Json(null);
            }
        }

        [HttpPost]
        public JsonResult GetDetails([FromBody] EncomendasViewModel encomenda)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Encomendas);

            if (UPerm != null && UPerm.Read.Value)
            {
                List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
                var details = DBNAV2017Encomendas.GetDetailsByNo(_config.NAVDatabaseName, _config.NAVCompanyName, encomenda.No, "C%");
                var lines = DBNAV2017Encomendas.ListLinesByNo(_config.NAVDatabaseName, _config.NAVCompanyName, encomenda.No, "C%");
                return Json(new {
                    details,
                    lines
                });
            }
            else
            {
                return Json(null);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ExportToExcel([FromBody] dynamic form)
       {
            JObject dp = (JObject)form.GetValue("columns");

            var list = (dynamic)form.GetValue("list");

            string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\Upload\\temp";
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
                ISheet excelSheet = workbook.CreateSheet("Encomendas");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["no"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº");
                    Col = Col + 1;
                }
                if (dp["payToVendorNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Código Fornecedor");
                    Col = Col + 1;
                }
                if (dp["payToName"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nome Fornecedor");
                    Col = Col + 1;
                }
                if (dp["yourReference"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Sua Referência");
                    Col = Col + 1;
                }
                if (dp["noConsulta"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Consulta Mercado");
                    Col = Col + 1;
                }
                if (dp["expectedReceiptDate"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data da Encomenda");
                    Col = Col + 1;
                }
                if (dp["requisitionNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Pedido Interno");
                    Col = Col + 1;
                }
                if (dp["regionId"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Região");
                    Col = Col + 1;
                }
                if (dp["functionalAreaId"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Área Funcional");
                    Col = Col + 1;
                }
                if (dp["respCenterId"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Centro de Responsabilidade");
                    Col = Col + 1;
                }

                if (dp != null)
                {
                    int count = 1;

                    foreach (JObject item in list)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["no"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item["no"].ToString());
                            Col = Col + 1;
                        }
                        if (dp["payToVendorNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item["payToVendorNo"].ToString());
                            Col = Col + 1;
                        }
                        if (dp["payToName"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item["payToName"].ToString());
                            Col = Col + 1;
                        }
                        if (dp["yourReference"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item["yourReference"].ToString());
                            Col = Col + 1;
                        }
                        if (dp["noConsulta"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item["noConsulta"].ToString());
                            Col = Col + 1;
                        }
                        if (dp["expectedReceiptDate"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item["expectedReceiptDate"].ToString());
                            Col = Col + 1;
                        }
                        if (dp["requisitionNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item["requisitionNo"].ToString());
                            Col = Col + 1;
                        }
                        if (dp["regionId"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item["regionId"].ToString());
                            Col = Col + 1;
                        }
                        if (dp["functionalAreaId"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item["functionalAreaId"].ToString());
                            Col = Col + 1;
                        }
                        if (dp["respCenterId"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item["respCenterId"].ToString());
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

        public IActionResult ExportToExcel(string fileName)        
        {            
            fileName = @"/Upload/temp/" + fileName;
            return File(fileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Encomendas.xlsx");
        }

    }
}