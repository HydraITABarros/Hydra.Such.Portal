using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.NAV;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Portal.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Hydra.Such.Portal.Controllers
{
    public class ContabilidadeController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private readonly GeneralConfigurations _generalConfig;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ContabilidadeController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IOptions<GeneralConfigurations> appSettingsGeneral, IHostingEnvironment _hostingEnvironment)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            _generalConfig = appSettingsGeneral.Value;
            this._hostingEnvironment = _hostingEnvironment;
        }

        public IActionResult ContasReceberACSS()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.ContabilidadeMapas);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                ViewBag.reportServerURL = _config.ReportServerURL;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult AntiguidadeSaldos()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.ContabilidadeMapasClientes);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                ViewBag.reportServerURL = _config.ReportServerURL;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult VendasAnuais()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.ContabilidadeMapasClientes);

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

        [HttpPost]
        public JsonResult GetListVendasAnuais([FromBody] VendasAnuais data)
        {
            List<VendasAnuais> result = DBVendasAnuais.GetAllByFilterToList((int)data.Ano, data.Regiao);

            if (result != null && result.Count > 0)
            {
                result.ForEach(x =>
                {
                    x.Total = x.Jan + x.Fev + x.Mar + x.Abr + x.Mai + x.Jun + x.Jul + x.Ago + x.Set + x.Out + x.Nov + x.Dez;
                });
            }

            return Json(result);
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_VendasAnuais([FromBody] List<VendasAnuais> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "Contabilidade\\" + "tmp\\";
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
                ISheet excelSheet = workbook.CreateSheet("Vendas Anuais");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                var columns = dp.AsJEnumerable().ToList();
                for (int i = 0; i < columns.Count; i++)
                {
                    var column = columns[i];
                    var isHidden = true;
                    var label = "";
                    try
                    {
                        isHidden = (bool)column.First()["hidden"];
                        label = (string)column.First()["label"];
                    }
                    catch { }

                    if (!isHidden)
                    {
                        row.CreateCell(i).SetCellValue(label);
                    }
                }

                //if (dp["ano"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Ano"); Col = Col + 1; }
                //if (dp["regiao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Região"); Col = Col + 1; }
                //if (dp["noAssociado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº de Associado"); Col = Col + 1; }
                //if (dp["nomeAssociado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nome do Associado"); Col = Col + 1; }
                //if (dp["jan"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Janeiro"); Col = Col + 1; }
                //if (dp["fev"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Fevereiro"); Col = Col + 1; }
                //if (dp["mar"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Março"); Col = Col + 1; }
                //if (dp["abr"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Abril"); Col = Col + 1; }
                //if (dp["mai"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Maio"); Col = Col + 1; }
                //if (dp["jun"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Junho"); Col = Col + 1; }
                //if (dp["jul"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Julho"); Col = Col + 1; }
                //if (dp["ago"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Agosto"); Col = Col + 1; }
                //if (dp["set"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Setembro"); Col = Col + 1; }
                //if (dp["out"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Outubro"); Col = Col + 1; }
                //if (dp["nov"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Novembro"); Col = Col + 1; }
                //if (dp["dez"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Dezembro"); Col = Col + 1; }
                //if (dp["total"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Total"); Col = Col + 1; }

                if (dp != null)
                {
                    int count = 1;
                    foreach (VendasAnuais item in Lista)
                    {
                        row = excelSheet.CreateRow(count);

                        for (int i = 0; i < columns.Count; i++)
                        {
                            var column = columns[i];
                            var isHidden = true;
                            try { isHidden = (bool)column.First()["hidden"]; } catch { }

                            if (!isHidden)
                            {
                                var columnPath = column.Path.ToString();
                                columnPath = columnPath.First().ToString().ToUpper() + String.Join("", columnPath.Skip(1));
                                object value = null;
                                try { value = item.GetType().GetProperty(columnPath).GetValue(item, null); } catch { }
                                if (value == null) try { value = item.GetType().GetProperty(columnPath.ToUpper()).GetValue(item, null); } catch { }

                                if ((new[] { "jan", "fev", "mar", "abr", "mai", "jun", "jul", "ago", "set", "out", "nov", "dez", "total" }).Contains(columnPath))
                                {
                                    row.CreateCell(i).SetCellValue((double)(value != null ? (decimal)value : 0));
                                }
                                else
                                {
                                    row.CreateCell(i).SetCellValue(value?.ToString());
                                }
                            }
                        }

                        count++;
                    }
                }

                //if (dp != null)
                //{
                //    int count = 1;
                //    foreach (VendasAnuais item in Lista)
                //    {
                //        Col = 0;
                //        row = excelSheet.CreateRow(count);

                //        if (dp["ano"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Ano.ToString()); Col = Col + 1; }
                //        if (dp["regiao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Regiao); Col = Col + 1; }
                //        if (dp["noAssociado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NoAssociado); Col = Col + 1; }
                //        if (dp["nomeAssociado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NomeAssociado); Col = Col + 1; }
                //        if (dp["jan"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Jan.ToString()); Col = Col + 1; }
                //        if (dp["fev"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Fev.ToString()); Col = Col + 1; }
                //        if (dp["mar"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Mar.ToString()); Col = Col + 1; }
                //        if (dp["abr"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Abr.ToString()); Col = Col + 1; }
                //        if (dp["mai"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Mai.ToString()); Col = Col + 1; }
                //        if (dp["jun"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Jun.ToString()); Col = Col + 1; }
                //        if (dp["jul"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Jul.ToString()); Col = Col + 1; }
                //        if (dp["ago"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Ago.ToString()); Col = Col + 1; }
                //        if (dp["set"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Set.ToString()); Col = Col + 1; }
                //        if (dp["out"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Out.ToString()); Col = Col + 1; }
                //        if (dp["nov"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Nov.ToString()); Col = Col + 1; }
                //        if (dp["dez"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Dez.ToString()); Col = Col + 1; }
                //        if (dp["total"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Total.ToString()); Col = Col + 1; }

                //        count++;
                //    }
                //}
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
        public IActionResult ExportToExcelDownload_VendasAnuais(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Contabilidade\\" + "tmp\\" + sFileName;
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }















    }
}