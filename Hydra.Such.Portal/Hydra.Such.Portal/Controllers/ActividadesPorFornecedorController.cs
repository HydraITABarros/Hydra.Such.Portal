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
    public class ActividadesPorFornecedorController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ActividadesPorFornecedorController(IHostingEnvironment _hostingEnvironment)
        {
            this._hostingEnvironment = _hostingEnvironment;
        }

        public IActionResult ActividadesPorFornecedor()
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
        public JsonResult GetAllActividadesPorFornecedor()
        {
            List<ActividadesPorFornecedor> result = DBConsultaMercado.GetAllActividadesPorFornecedorToList();
            List<ActividadesPorFornecedorView> list = new List<ActividadesPorFornecedorView>();

            foreach (ActividadesPorFornecedor actporforn in result)
            {
                list.Add(DBConsultaMercado.CastActividadesPorFornecedorToView(actporforn));
            }

            //return Json(result);
            return Json(list);
        }


        public IActionResult DetalheActividadePorFornecedor(string id)
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
        public JsonResult DetalheActividadePorFornecedor([FromBody] ActividadesPorFornecedorView data)
        {
            if (data != null)
            {
                ActividadesPorFornecedor actividadesPorFornecedor = DBConsultaMercado.GetDetalheActividadesPorFornecedor(data.Id.ToString());

                if (actividadesPorFornecedor != null)
                {
                    ActividadesPorFornecedorView result = DBConsultaMercado.CastActividadesPorFornecedorToView(actividadesPorFornecedor);

                    return Json(result);
                }

                return Json(new ActividadesView());
            }
            return Json(false);
        }


        [HttpPost]
        public JsonResult CreateActividadePorFornecedor([FromBody] ActividadesPorFornecedorView item)
        {
            if (item != null)
            {
                ActividadesPorFornecedor actividadesPorFornecedor = DBConsultaMercado.GetDetalheActividadesPorFornecedor(item.Id.ToString());

                if (actividadesPorFornecedor != null)
                {
                    item.eReasonCode = -1;
                    item.eMessage = string.Format("Já existe uma Actividade por Fornecedor com o mesmo ID!");
                }
                else
                {
                    ActividadesPorFornecedor novo = new ActividadesPorFornecedor()
                    {
                        CodFornecedor = item.CodFornecedor,
                        CodActividade = item.CodActividade
                    };

                    try
                    {
                        DBConsultaMercado.Create(novo);
                    }
                    catch
                    {
                        item.eReasonCode = -1;
                        item.eMessage = "Ocorreu um erro ao criar a Actividade por Fornecedor!";
                        return Json(item);
                    }

                    actividadesPorFornecedor = DBConsultaMercado.GetDetalheActividadesPorFornecedor(novo.Id.ToString());
                    item = DBConsultaMercado.CastActividadesPorFornecedorToView(actividadesPorFornecedor);

                    item.eReasonCode = 1;
                    item.eMessage = "Actividade por Fornecedor criada com sucesso!";
                }
            }

            return Json(item);
        }

        [HttpPost]
        public JsonResult UpdateActividadePorFornecedor([FromBody] ActividadesPorFornecedorView item)
        {
            if (item != null)
            {
                ActividadesPorFornecedor actividadesPorFornecedor = DBConsultaMercado.GetDetalheActividadesPorFornecedor(item.Id.ToString());

                if (actividadesPorFornecedor != null)
                {
                    actividadesPorFornecedor.CodActividade = item.CodActividade;
                    actividadesPorFornecedor.CodFornecedor = item.CodFornecedor;

                    try
                    {
                        DBConsultaMercado.Update(actividadesPorFornecedor);

                        actividadesPorFornecedor = DBConsultaMercado.GetDetalheActividadesPorFornecedor(item.Id.ToString());
                        item = DBConsultaMercado.CastActividadesPorFornecedorToView(actividadesPorFornecedor);

                        item.eReasonCode = 1;
                        item.eMessage = "Actividade por Fornecedor actualizada com sucesso!";
                    }
                    catch
                    {
                        item.eReasonCode = -1;
                        item.eMessage = "Ocorreu um erro ao gravar a Actividade por Fornecedor!";
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
        public JsonResult DeleteActividadePorFornecedor([FromBody] ActividadesPorFornecedorView item)
        {
            if (item != null)
            {
                ActividadesPorFornecedor actividadesPorFornecedor = DBConsultaMercado.GetDetalheActividadesPorFornecedor(item.Id.ToString());

                if (actividadesPorFornecedor != null)
                {
                    try
                    {
                        DBConsultaMercado.Delete(actividadesPorFornecedor);

                        item.eReasonCode = 1;
                        item.eMessage = "Actividade por Fornecedor eliminada com sucesso!";
                    }
                    catch
                    {
                        item.eReasonCode = -1;
                        item.eMessage = "Ocorreu um erro ao eliminar a Actividade por Fornecedor!";
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
        public async Task<JsonResult> ExportToExcel_ActividadesPorFornecedor([FromBody] List<ActividadesPorFornecedorView> Lista)
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
                ISheet excelSheet = workbook.CreateSheet("Actividades Por Fornecedor");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["id"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("ID"); Col = Col + 1; }
                if (dp["codFornecedor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Fornecedor"); Col = Col + 1; }
                if (dp["codActividade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Actividade"); Col = Col + 1; }

                if (dp != null)
                {
                    int count = 1;
                    foreach (ActividadesPorFornecedorView item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["id"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Id); Col = Col + 1; }
                        if (dp["codFornecedor"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodFornecedor); Col = Col + 1; }
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
        public IActionResult ExportToExcelDownload_ActividadesPorFornecedor(string sFileName)
        {
            sFileName = @"/Upload/temp/" + sFileName;
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Actividades Por Fornecedor.xlsx");
        }

        #endregion

    }
}