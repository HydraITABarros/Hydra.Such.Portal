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
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Hydra.Such.Portal.Controllers
{
    public class TelemoveisCartoesController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public TelemoveisCartoesController(IHostingEnvironment _hostingEnvironment)
        {
            this._hostingEnvironment = _hostingEnvironment;
        }

        public IActionResult TelemoveisCartoes()
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

        public IActionResult DetalheTelemoveisCartoes(string numCartao)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Features.Telemoveis);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                ViewBag.No = numCartao == null ? "" : numCartao;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetAllCartoes()
        {
            List<TelemoveisCartoes> result = DBTelemoveis.GetAllTelemoveisCartoesToList();
            List<TelemoveisCartoesView> list = new List<TelemoveisCartoesView>();

            foreach (TelemoveisCartoes tel in result)
            {
                list.Add(DBTelemoveis.CastTelemoveisCartoesToView(tel));
            }

            return Json(list);
        }


        [HttpPost]
        public JsonResult GetCartoesDetails([FromBody] TelemoveisCartoesView data)
        {
            try
            {
                if (data != null)
                {
                    TelemoveisCartoes telemoveisCartoes = DBTelemoveis.GetTelemoveisCartoes(data.NumCartao);

                    if (telemoveisCartoes != null)
                    {
                        TelemoveisCartoesView telemoveisCartoesView = DBTelemoveis.CastTelemoveisCartoesToView(telemoveisCartoes);
                        
                        return Json(telemoveisCartoesView);
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
        public JsonResult CreateTelemoveisCartoes([FromBody] TelemoveisCartoesView item)
        {
            if (item != null)
            {
                //Verificar se existe
                TelemoveisCartoes telemoveisCartoes = DBTelemoveis.GetTelemoveisCartoes(item.NumCartao);

                if (telemoveisCartoes != null)
                {
                    item.eReasonCode = -1;
                    item.eMessage = string.Format("Já existe um cartão com o nº '{0}'", item.NumCartao);
                }
                else
                {
                    TelemoveisCartoes novo = new TelemoveisCartoes()
                    {
                        NumCartao = item.NumCartao,
                        TipoServico = item.TipoServico,
                        ContaSuch = item.ContaSuch,
                        ContaUtilizador = item.ContaUtilizador,
                        Barramentos = item.Barramentos,
                        TarifarioVoz = item.TarifarioVoz,
                        TarifarioDados = item.TarifarioDados,
                        ExtensaoVpn = item.ExtensaoVpn,
                        PlafondFr = item.PlafondFr,
                        PlafondExtra = item.PlafondExtra,
                        FimFidelizacao = item.FimFidelizacao,
                        Gprs = item.Gprs,
                        Estado = item.Estado,
                        DataEstado = DateTime.Now,
                        Observacoes = item.Observacoes,
                        NumFuncionario = item.NumFuncionario,
                        Nome = item.Nome,
                        CodRegiao = item.CodRegiao,
                        CodAreaFuncional = item.CodAreaFuncional,
                        CodCentroResponsabilidade = item.CodCentroResponsabilidade,
                        Grupo = item.Grupo,
                        Imei = item.Imei,
                        DataAtribuicao = item.DataAtribuicao,
                        ChamadasInternacionais = item.ChamadasInternacionais,
                        Roaming = item.Roaming,
                        Internet = item.Internet,
                        Declaracao = item.Declaracao,
                        Utilizador = item.Utilizador,
                        DataAlteracao = item.DataAlteracao,
                        Plafond100percUtilizador = item.Plafond100percUtilizador,
                        WhiteList = item.WhiteList,
                        ValorMensalidadeDados = item.ValorMensalidadeDados,
                        PlafondDados = item.PlafondDados,
                        EquipamentoNaoDevolvido = item.EquipamentoNaoDevolvido
                    };

                    try
                    {
                        DBTelemoveis.Create(novo);
                    }
                    catch
                    {
                        item.eReasonCode = -1;
                        item.eMessage = "Ocorreu um erro ao criar o Cartão!";
                        return Json(item);
                    }

                    telemoveisCartoes = DBTelemoveis.GetTelemoveisCartoes(item.NumCartao);
                    item = DBTelemoveis.CastTelemoveisCartoesToView(telemoveisCartoes);

                    item.eReasonCode = 1;
                    item.eMessage = "Cartão criado com sucesso!";
                }
            }

            return Json(item);
        }

        [HttpPost]
        public JsonResult UpdateTelemoveisCartoes([FromBody] TelemoveisCartoesView item)
        {
            if (item != null)
            {
                //Verificar se existe
                TelemoveisCartoes telemoveisCartoes = DBTelemoveis.GetTelemoveisCartoes(item.NumCartao);

                if (telemoveisCartoes != null)
                {
                    //Verificar se o estado é diferente, para alterar a data do estado
                    if (telemoveisCartoes.Estado != item.Estado)
                        telemoveisCartoes.DataEstado = DateTime.Now;

                    telemoveisCartoes.NumCartao = item.NumCartao;
                    telemoveisCartoes.TipoServico = item.TipoServico;
                    telemoveisCartoes.ContaSuch = item.ContaSuch;
                    telemoveisCartoes.ContaUtilizador = item.ContaUtilizador;
                    telemoveisCartoes.Barramentos = item.Barramentos;
                    telemoveisCartoes.TarifarioVoz = item.TarifarioVoz;
                    telemoveisCartoes.TarifarioDados = item.TarifarioDados;
                    telemoveisCartoes.ExtensaoVpn = item.ExtensaoVpn;
                    telemoveisCartoes.PlafondFr = item.PlafondFr;
                    telemoveisCartoes.PlafondExtra = item.PlafondExtra;
                    telemoveisCartoes.FimFidelizacao = item.FimFidelizacao;
                    telemoveisCartoes.Gprs = item.Gprs;
                    telemoveisCartoes.Estado = item.Estado;
                    //telemoveisCartoes.DataEstado = item.DataEstado;
                    telemoveisCartoes.Observacoes = item.Observacoes;
                    telemoveisCartoes.NumFuncionario = item.NumFuncionario;
                    telemoveisCartoes.Nome = item.Nome;
                    telemoveisCartoes.CodRegiao = item.CodRegiao;
                    telemoveisCartoes.CodAreaFuncional = item.CodAreaFuncional;
                    telemoveisCartoes.CodCentroResponsabilidade = item.CodCentroResponsabilidade;
                    telemoveisCartoes.Grupo = item.Grupo;
                    telemoveisCartoes.Imei = item.Imei;
                    telemoveisCartoes.DataAtribuicao = item.DataAtribuicao;
                    telemoveisCartoes.ChamadasInternacionais = Convert.ToByte(item.ChamadasInternacionais_Show);
                    telemoveisCartoes.Roaming = Convert.ToByte(item.Roaming_Show);
                    telemoveisCartoes.Internet = item.Internet;
                    telemoveisCartoes.Declaracao = item.Declaracao;
                    telemoveisCartoes.Utilizador = item.Utilizador;
                    telemoveisCartoes.DataAlteracao = DateTime.Now;
                    telemoveisCartoes.Plafond100percUtilizador = Convert.ToByte(item.Plafond100percUtilizador_Show);
                    telemoveisCartoes.WhiteList = item.WhiteList;
                    telemoveisCartoes.ValorMensalidadeDados = item.ValorMensalidadeDados;
                    telemoveisCartoes.PlafondDados = item.PlafondDados;
                    telemoveisCartoes.EquipamentoNaoDevolvido = Convert.ToByte(item.EquipamentoNaoDevolvido_Show);

                    try
                    {
                        DBTelemoveis.Update(telemoveisCartoes);

                        telemoveisCartoes = DBTelemoveis.GetTelemoveisCartoes(item.NumCartao);
                        item = DBTelemoveis.CastTelemoveisCartoesToView(telemoveisCartoes);

                        item.eReasonCode = 1;
                        item.eMessage = "Cartão actualizado com sucesso!";
                    }
                    catch
                    {
                        item.eReasonCode = -1;
                        item.eMessage = "Ocorreu um erro ao gravar o Cartão!";
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
        public JsonResult DeleteTelemoveisCartoes([FromBody] TelemoveisCartoesView item)
        {
            if (item != null)
            {
                //Verificar se existe
                TelemoveisCartoes telemoveisCartoes = DBTelemoveis.GetTelemoveisCartoes(item.NumCartao);

                if (telemoveisCartoes != null)
                {
                    try
                    {
                        DBTelemoveis.Delete(telemoveisCartoes);

                        item.eReasonCode = 1;
                        item.eMessage = "Cartão eliminado com sucesso!";
                    }
                    catch
                    {
                        item.eReasonCode = -1;
                        item.eMessage = "Ocorreu um erro ao eliminar o Cartão!";
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
        public async Task<JsonResult> ExportToExcel_TelemoveisCartoes([FromBody] List<TelemoveisCartoesView> dp)
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
                ISheet excelSheet = workbook.CreateSheet("Telemóveis Cartões");
                IRow row = excelSheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("Nº Cartão");
                row.CreateCell(1).SetCellValue("Tipo Serviço");
                row.CreateCell(2).SetCellValue("Estado");
                row.CreateCell(3).SetCellValue("Conta SUCH");
                row.CreateCell(4).SetCellValue("Código Região");
                row.CreateCell(5).SetCellValue("Código Área Funcional");
                row.CreateCell(6).SetCellValue("Código Centro Responsabilidade");

                if (dp != null)
                {
                    int count = 1;
                    foreach (TelemoveisCartoesView item in dp)
                    {
                        row = excelSheet.CreateRow(count);
                        row.CreateCell(0).SetCellValue(item.NumCartao);
                        row.CreateCell(1).SetCellValue(item.TipoServico_Show);
                        row.CreateCell(2).SetCellValue(item.Estado_Show);
                        row.CreateCell(3).SetCellValue(item.ContaSuch);
                        row.CreateCell(4).SetCellValue(item.CodRegiao);
                        row.CreateCell(5).SetCellValue(item.CodAreaFuncional);
                        row.CreateCell(6).SetCellValue(item.CodCentroResponsabilidade);
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
        public IActionResult ExportToExcelDownload_TelemoveisCartoes(string sFileName)
        {
            sFileName = @"/Upload/temp/" + sFileName;
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Telemóveis Cartões.xlsx");
        }

    }
}