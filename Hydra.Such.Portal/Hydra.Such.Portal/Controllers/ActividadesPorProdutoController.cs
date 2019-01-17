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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Portal.Controllers
{
    public class ActividadesPorProdutoController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ActividadesPorProdutoController(IHostingEnvironment _hostingEnvironment)
        {
            this._hostingEnvironment = _hostingEnvironment;
        }

        public IActionResult ActividadesPorProduto()
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
        public JsonResult GetAllActividadesPorProduto()
        {
            List<ActividadesPorProduto> result = DBConsultaMercado.GetAllActividadesPorProdutoToList();
            List<ActividadesPorProdutoView> list = new List<ActividadesPorProdutoView>();

            foreach (ActividadesPorProduto actporforn in result)
            {
                list.Add(DBConsultaMercado.CastActividadesPorProdutoToView(actporforn));
            }

            //return Json(result);
            return Json(list);
        }


        public IActionResult DetalheActividadePorProduto(string id)
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
        public JsonResult DetalheActividadePorProduto([FromBody] ActividadesPorProdutoView data)
        {
            if (data != null)
            {
                ActividadesPorProduto actividadesPorProduto = DBConsultaMercado.GetDetalheActividadesPorProduto(data.Id.ToString());

                if (actividadesPorProduto != null)
                {
                    ActividadesPorProdutoView result = DBConsultaMercado.CastActividadesPorProdutoToView(actividadesPorProduto);

                    return Json(result);
                }

                return Json(new ActividadesView());
            }
            return Json(false);
        }


        [HttpPost]
        public JsonResult CreateActividadePorProduto([FromBody] ActividadesPorProdutoView item)
        {
            if (item != null)
            {
                ActividadesPorProduto actividadesPorProduto = DBConsultaMercado.GetDetalheActividadesPorProduto(item.Id.ToString());

                if (actividadesPorProduto != null)
                {
                    item.eReasonCode = -1;
                    item.eMessage = string.Format("Já existe uma Actividade por Produto com o mesmo ID!");
                }
                else
                {
                    ActividadesPorProduto novo = new ActividadesPorProduto()
                    {
                        CodProduto = item.CodProduto,
                        CodActividade = item.CodActividade
                    };

                    try
                    {
                        DBConsultaMercado.Create(novo);
                    }
                    catch
                    {
                        item.eReasonCode = -1;
                        item.eMessage = "Ocorreu um erro ao criar a Actividade por Produto!";
                        return Json(item);
                    }

                    actividadesPorProduto = DBConsultaMercado.GetDetalheActividadesPorProduto(novo.Id.ToString());
                    item = DBConsultaMercado.CastActividadesPorProdutoToView(actividadesPorProduto);

                    item.eReasonCode = 1;
                    item.eMessage = "Actividade por Produto criada com sucesso!";
                }
            }

            return Json(item);
        }

        [HttpPost]
        public JsonResult UpdateActividadePorProduto([FromBody] ActividadesPorProdutoView item)
        {
            if (item != null)
            {
                ActividadesPorProduto actividadesPorProduto = DBConsultaMercado.GetDetalheActividadesPorProduto(item.Id.ToString());

                if (actividadesPorProduto != null)
                {
                    actividadesPorProduto.CodActividade = item.CodActividade;
                    actividadesPorProduto.CodProduto = item.CodProduto;

                    try
                    {
                        DBConsultaMercado.Update(actividadesPorProduto);

                        actividadesPorProduto = DBConsultaMercado.GetDetalheActividadesPorProduto(item.Id.ToString());
                        item = DBConsultaMercado.CastActividadesPorProdutoToView(actividadesPorProduto);

                        item.eReasonCode = 1;
                        item.eMessage = "Actividade por Produto actualizada com sucesso!";
                    }
                    catch
                    {
                        item.eReasonCode = -1;
                        item.eMessage = "Ocorreu um erro ao gravar a Actividade por Produto!";
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
        public JsonResult DeleteActividadePorProduto([FromBody] ActividadesPorProdutoView item)
        {
            if (item != null)
            {
                ActividadesPorProduto actividadesPorProduto = DBConsultaMercado.GetDetalheActividadesPorProduto(item.Id.ToString());

                if (actividadesPorProduto != null)
                {
                    try
                    {
                        DBConsultaMercado.Delete(actividadesPorProduto);

                        TabelaLog TabLog = new TabelaLog
                        {
                            Tabela = "[dbo].[Actividades_por_Produto]",
                            Descricao = "Delete - [ID]: " + actividadesPorProduto.Id.ToString(),
                            Utilizador = User.Identity.Name,
                            DataHora = DateTime.Now
                        };
                        DBTabelaLog.Create(TabLog);

                        item.eReasonCode = 1;
                        item.eMessage = "Actividade por Produto eliminada com sucesso!";
                    }
                    catch
                    {
                        item.eReasonCode = -1;
                        item.eMessage = "Ocorreu um erro ao eliminar a Actividade por Produto!";
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
        public async Task<JsonResult> ExportToExcel_ActividadesPorProduto([FromBody] List<ActividadesPorProdutoView> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

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
                ISheet excelSheet = workbook.CreateSheet("Actividades Por Produto");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["id"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("ID"); Col = Col + 1; }
                if (dp["codProduto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Produto"); Col = Col + 1; }
                if (dp["codActividade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Actividade"); Col = Col + 1; }

                if (dp != null)
                {
                    int count = 1;
                    foreach (ActividadesPorProdutoView item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["id"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Id); Col = Col + 1; }
                        if (dp["codProduto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodProduto); Col = Col + 1; }
                        if (dp["codActividade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodActividade); Col = Col + 1; }
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
        public IActionResult ExportToExcelDownload_ActividadesPorProduto(string sFileName)
        {
            sFileName = @"/Upload/temp/" + sFileName;
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Actividades Por Produto.xlsx");
        }

        #endregion

    }
}