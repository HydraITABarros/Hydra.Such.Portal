using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.Logic.Telemoveis;
using Microsoft.AspNetCore.Mvc;
using static Hydra.Such.Data.Enumerations;
using Newtonsoft.Json.Linq;
using Hydra.Such.Data.ViewModel.Telemoveis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;

namespace Hydra.Such.Portal.Controllers
{
    public class TelemoveisEquipamentosController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public TelemoveisEquipamentosController(IHostingEnvironment _hostingEnvironment)
        {
            this._hostingEnvironment = _hostingEnvironment;
        }

        public IActionResult TelemoveisEquipamentos()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Features.Telemoveis);

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

        public IActionResult DetalheTelemoveisEquipamentos([FromQuery] string tipo, string imei)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Features.Telemoveis);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                ViewBag.tipo = tipo;
                ViewBag.imei = imei;
                ViewBag.tipo_desc = int.Parse(tipo != null ? tipo : "0") == 0 ? "Equipamento" : "Placa de Rede";
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetAllEquipamentos()
        {
            List<TelemoveisEquipamentos> result = DBTelemoveis.GetAllTelemoveisEquipamentosToList();
            List<TelemoveisEquipamentosView> list = new List<TelemoveisEquipamentosView>();

            foreach (TelemoveisEquipamentos tel in result)
            {
                list.Add(DBTelemoveis.CastTelemoveisEquipamentosToView(tel));
            }

            //return Json(result);
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetEquipamentosPorTipo([FromBody] JObject requestParams)
        {
            int tipo = int.Parse(requestParams["tipo"].ToString());

            List<TelemoveisEquipamentos> result = DBTelemoveis.GetAllTelemoveisEquipamentosTypeToList(tipo);

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetEquipamentoDetails([FromBody] TelemoveisEquipamentosView data)
        {
            try
            {
                if (data != null)
                {
                    TelemoveisEquipamentos telemoveisEquipamentos = DBTelemoveis.GetTelemoveisEquipamentos(data.Tipo, data.Imei);
                    
                    if (telemoveisEquipamentos != null)
                    {
                        TelemoveisEquipamentosView telemoveisEquipamentosView = DBTelemoveis.CastTelemoveisEquipamentosToView(telemoveisEquipamentos);
                        
                        return Json(telemoveisEquipamentosView);
                    }

                    return Json(new TelemoveisEquipamentosView());
                }
            }
            catch (Exception e)
            {
                return null;
            }

            return Json(false);
        }


        [HttpPost]
        public JsonResult CreateTelemoveisEquipamentos([FromBody] TelemoveisEquipamentosView item)
        {
            if (item != null)
            {
                //Verificar se existe chave única tipo + imei
                TelemoveisEquipamentos telemoveisEquipamentos = DBTelemoveis.GetTelemoveisEquipamentos(item.Tipo, item.Imei);

                if (telemoveisEquipamentos != null)
                {
                    string Tipo_Desc = item.Tipo == 0 ? "Equipamento" : "Placa de Rede";
                    item.eReasonCode = -1;
                    item.eMessage = string.Format("Já existe um equipamento do tipo '{0}' com o IMEI/Nº Série '{1}'", Tipo_Desc, item.Imei);
                }
                else
                {
                    TelemoveisEquipamentos novo = new TelemoveisEquipamentos()
                    {
                        Tipo = item.Tipo,
                        Imei = item.Imei,
                        Marca = item.Marca,
                        Modelo = item.Modelo,
                        Estado = item.Estado,
                        Cor = item.Cor,
                        Observacoes = item.Observacoes,
                        DataRecepcao = item.DataRecepcao,
                        Documento = item.Documento,
                        DocumentoRecepcao = item.DocumentoRecepcao,
                        Utilizador = item.Utilizador,
                        DataAlteracao = item.DataAlteracao,
                        DevolvidoBk = item.DevolvidoBk,
                        NumEmpregadoComprador = item.NumEmpregadoComprador,
                        NomeComprador = item.NomeComprador,
                        Devolvido = item.Devolvido,
                        UtilizadorCriacao = User.Identity.Name,
                        DataHoraCriacao = DateTime.Now
                    };

                    try
                    {
                        DBTelemoveis.Create(novo);
                    }
                    catch
                    {
                        item.eReasonCode = -1;
                        item.eMessage = "Ocorreu um erro ao criar o Equipamento!";
                        return Json(item);
                    }

                    telemoveisEquipamentos = DBTelemoveis.GetTelemoveisEquipamentos(item.Tipo, item.Imei);
                    item = DBTelemoveis.CastTelemoveisEquipamentosToView(telemoveisEquipamentos);

                    item.eReasonCode = 1;
                    item.eMessage = "Equipamento criado com sucesso!";
                }
            }

            return Json(item);
        }


        [HttpPost]
        public JsonResult UpdateTelemoveisEquipamentos([FromBody] TelemoveisEquipamentosView item)
        {
            if (item != null)
            {
                //Verificar se existe chave única tipo + imei
                TelemoveisEquipamentos telemoveisEquipamentos = DBTelemoveis.GetTelemoveisEquipamentos(item.Tipo, item.Imei);

                if (telemoveisEquipamentos != null)
                {
                    telemoveisEquipamentos.Marca = item.Marca;
                    telemoveisEquipamentos.Modelo = item.Modelo;
                    telemoveisEquipamentos.Estado = item.Estado;
                    telemoveisEquipamentos.Cor = item.Cor;
                    telemoveisEquipamentos.Observacoes = item.Observacoes;
                    telemoveisEquipamentos.DataRecepcao = item.DataRecepcao;
                    telemoveisEquipamentos.Documento = item.Documento;
                    telemoveisEquipamentos.DocumentoRecepcao = item.DocumentoRecepcao;
                    telemoveisEquipamentos.Utilizador = User.Identity.Name;
                    telemoveisEquipamentos.DataAlteracao = DateTime.Now;
                    telemoveisEquipamentos.DevolvidoBk = item.DevolvidoBk;
                    telemoveisEquipamentos.NumEmpregadoComprador = item.NumEmpregadoComprador;
                    telemoveisEquipamentos.NomeComprador = item.NomeComprador;
                    telemoveisEquipamentos.Devolvido = item.Devolvido;
                    telemoveisEquipamentos.UtilizadorModificacao = User.Identity.Name;
                    telemoveisEquipamentos.DataHoraModificacao = DateTime.Now;

                    try
                    {
                        DBTelemoveis.Update(telemoveisEquipamentos);

                        telemoveisEquipamentos = DBTelemoveis.GetTelemoveisEquipamentos(item.Tipo, item.Imei);
                        item = DBTelemoveis.CastTelemoveisEquipamentosToView(telemoveisEquipamentos);

                        item.eReasonCode = 1;
                        item.eMessage = "Equipamento actualizado com sucesso!";
                    }
                    catch
                    {
                        item.eReasonCode = -1;
                        item.eMessage = "Ocorreu um erro ao gravar o Equipamento!";
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
        public JsonResult DeleteTelemoveisEquipamentos([FromBody] TelemoveisEquipamentosView item)
        {
            if (item != null)
            {
                //Verificar se existe chave única tipo + imei
                TelemoveisEquipamentos telemoveisEquipamentos = DBTelemoveis.GetTelemoveisEquipamentos(item.Tipo, item.Imei);

                if (telemoveisEquipamentos != null)
                {
                    try
                    {
                        DBTelemoveis.Delete(telemoveisEquipamentos);
                        
                        item.eReasonCode = 1;
                        item.eMessage = "Equipamento eliminado com sucesso!";
                    }
                    catch
                    {
                        item.eReasonCode = -1;
                        item.eMessage = "Ocorreu um erro ao eliminar o Equipamento!";
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

        //1
        [HttpPost]
        public async Task<JsonResult> ExportToExcel_TelemoveisEquipamentos([FromBody] List<TelemoveisEquipamentosView> dp)
        {
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
                ISheet excelSheet = workbook.CreateSheet("Telemóveis Equipamentos");
                IRow row = excelSheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("IMEI / Nº Série");
                row.CreateCell(1).SetCellValue("Tipo");
                row.CreateCell(2).SetCellValue("Marca");
                row.CreateCell(3).SetCellValue("Modelo");
                row.CreateCell(4).SetCellValue("Estado");
                row.CreateCell(5).SetCellValue("Cor");
                row.CreateCell(6).SetCellValue("Observações");
                row.CreateCell(7).SetCellValue("Devolvido");
                row.CreateCell(8).SetCellValue("Utilizador");
                row.CreateCell(9).SetCellValue("Data Alteração");

                if (dp != null)
                {
                    int count = 1;
                    foreach (TelemoveisEquipamentosView item in dp)
                    {
                        row = excelSheet.CreateRow(count);
                        row.CreateCell(0).SetCellValue(item.Imei);
                        row.CreateCell(1).SetCellValue(item.Tipo_Show);
                        row.CreateCell(2).SetCellValue(item.Marca);
                        row.CreateCell(3).SetCellValue(item.Modelo);
                        row.CreateCell(4).SetCellValue(item.Estado_Show);
                        row.CreateCell(5).SetCellValue(item.Cor);
                        row.CreateCell(6).SetCellValue(item.Observacoes);
                        row.CreateCell(7).SetCellValue(item.Devolvido_Show);
                        row.CreateCell(8).SetCellValue(item.Utilizador);
                        row.CreateCell(9).SetCellValue(item.DataAlteracao_Show);
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
        public IActionResult ExportToExcelDownload_TelemoveisEquipamentos(string sFileName)
        {
            sFileName = @"/Upload/temp/" + sFileName;
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Telemóveis Equipamentos.xlsx");
        }

    }
}