using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.PedidoCotacao;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.PedidoCotacao;
using Hydra.Such.Portal.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Portal.Controllers
{
    public class ActividadesController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly GeneralConfigurations _generalConfig;


        public ActividadesController(IHostingEnvironment _hostingEnvironment, IOptions<GeneralConfigurations> appSettingsGeneral)
        {
            this._hostingEnvironment = _hostingEnvironment;
            _generalConfig = appSettingsGeneral.Value;
        }

        public IActionResult Actividades()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Features.PedidoCotacao);

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
        public JsonResult GetAllActividades()
        {
            List<Actividades> result = DBConsultaMercado.GetAllActividadesToList();
            List<ActividadesView> list = new List<ActividadesView>();

            foreach (Actividades act in result)
            {
                list.Add(DBConsultaMercado.CastActividadesToView(act));
            }

            //return Json(result);
            return Json(list.OrderByDescending(x => x.CodActividade));
        }


        public IActionResult DetalheActividade(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Features.PedidoCotacao);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                ViewBag.No = id == null ? "" : id;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }


        [HttpPost]
        public JsonResult DetalheActividade([FromBody] ActividadesView data)
        {
            if (data != null)
            {
                Actividades actividades = DBConsultaMercado.GetDetalheActividades(data.CodActividade);

                if (actividades != null)
                {
                    ActividadesView result = DBConsultaMercado.CastActividadesToView(actividades);

                    return Json(result);
                }

                return Json(new ActividadesView());
            }
            return Json(false);
        }


        [HttpPost]
        public JsonResult CreateActividade([FromBody] ActividadesView item)
        {
            if (item != null)
            {
                Actividades actividades = DBConsultaMercado.GetDetalheActividades(item.CodActividade);

                if (actividades != null)
                {
                    item.eReasonCode = -1;
                    item.eMessage = string.Format("Já existe uma Actividade com o mesmo Código!");
                }
                else
                {
                    Actividades novo = new Actividades()
                    {
                        CodActividade = item.CodActividade,
                        Descricao = item.Descricao
                    };

                    try
                    {
                        DBConsultaMercado.Create(novo);
                    }
                    catch
                    {
                        item.eReasonCode = -1;
                        item.eMessage = "Ocorreu um erro ao criar a Actividade!";
                        return Json(item);
                    }

                    actividades = DBConsultaMercado.GetDetalheActividades(item.CodActividade);
                    item = DBConsultaMercado.CastActividadesToView(actividades);

                    item.eReasonCode = 1;
                    item.eMessage = "Actividade criada com sucesso!";
                }
            }

            return Json(item);
        }

        [HttpPost]
        public JsonResult UpdateActividade([FromBody] ActividadesView item)
        {
            if (item != null)
            {
                Actividades actividades = DBConsultaMercado.GetDetalheActividades(item.CodActividade);

                if (actividades != null)
                {
                    actividades.CodActividade = item.CodActividade;
                    actividades.Descricao = item.Descricao;

                    try
                    {
                        DBConsultaMercado.Update(actividades);

                        actividades = DBConsultaMercado.GetDetalheActividades(item.CodActividade);
                        item = DBConsultaMercado.CastActividadesToView(actividades);

                        item.eReasonCode = 1;
                        item.eMessage = "Actividade actualizada com sucesso!";
                    }
                    catch
                    {
                        item.eReasonCode = -1;
                        item.eMessage = "Ocorreu um erro ao Guardar a Actividade!";
                        return Json(item);
                    }
                }
                else
                {
                    item.eReasonCode = -1;
                    item.eMessage = "Ocorreu um erro!";
                    return Json(item);
                }
            }

            return Json(item);
        }


        [HttpPost]
        public JsonResult DeleteActividade([FromBody] ActividadesView item)
        {
            if (item != null)
            {
                Actividades actividades = DBConsultaMercado.GetDetalheActividades(item.CodActividade);

                if (actividades != null)
                {
                    try
                    {
                        DBConsultaMercado.Delete(actividades);

                        TabelaLog TabLog = new TabelaLog
                        {
                            Tabela = "[Consulta_Mercado]",
                            Descricao = "Delete - [Num_Consulta_Mercado]: " + actividades.CodActividade.ToString(),
                            Utilizador = User.Identity.Name,
                            DataHora = DateTime.Now
                        };
                        DBTabelaLog.Create(TabLog);

                        item.eReasonCode = 1;
                        item.eMessage = "Actividade eliminada com sucesso!";
                    }
                    catch
                    {
                        item.eReasonCode = -1;
                        item.eMessage = "Ocorreu um erro ao eliminar a Actividade!";
                        return Json(item);
                    }
                }
                else
                {
                    item.eReasonCode = -1;
                    item.eMessage = "Ocorreu um erro!";
                    return Json(item);
                }
            }

            return Json(item);
        }


        #region EXCEL

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_Actividades([FromBody] List<ActividadesView> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "Atividades\\" + "tmp\\";
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
                ISheet excelSheet = workbook.CreateSheet("Actividades");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["codActividade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Actividade"); Col = Col + 1; }
                if (dp["descricao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Descrição"); Col = Col + 1; }

                if (dp != null)
                {
                    int count = 1;
                    foreach (ActividadesView item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["codActividade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodActividade); Col = Col + 1; }
                        if (dp["descricao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Descricao); Col = Col + 1; }
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
        public IActionResult ExportToExcelDownload_Actividades(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Atividades\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Actividades.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        #endregion

    }
}