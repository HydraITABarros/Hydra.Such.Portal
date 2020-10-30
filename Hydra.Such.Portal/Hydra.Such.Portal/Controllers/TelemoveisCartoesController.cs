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
using Hydra.Such.Portal.Configurations;
using Microsoft.Extensions.Options;

namespace Hydra.Such.Portal.Controllers
{
    public class TelemoveisCartoesController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly GeneralConfigurations _generalConfig;

        public TelemoveisCartoesController(IHostingEnvironment _hostingEnvironment, IOptions<GeneralConfigurations> appSettingsGeneral)
        {
            this._hostingEnvironment = _hostingEnvironment;
            _generalConfig = appSettingsGeneral.Value;
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
                        item.eMessage = "Ocorreu um erro ao Guardar o Cartão!";
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
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_TelemoveisCartoes([FromBody] List<TelemoveisCartoesView> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "TelemoveisCartoes\\" + "tmp\\";
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
                ISheet excelSheet = workbook.CreateSheet("Telemóveis Cartões");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["numCartao"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Cartão");
                    Col = Col + 1;
                }
                if (dp["tipoServico_Show"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Tipo Serviço");
                    Col = Col + 1;
                }
                if (dp["estado_Show"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Estado");
                    Col = Col + 1;
                }
                if (dp["contaSuch"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Conta SUCH");
                    Col = Col + 1;
                }
                if (dp["codRegiao"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Código Região");
                    Col = Col + 1;
                }
                if (dp["codAreaFuncional"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Código Área Funcional");
                    Col = Col + 1;
                }
                if (dp["codCentroResponsabilidade"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Código Centro Responsabilidade");
                    Col = Col + 1;
                }

                if (dp != null)
                {
                    int count = 1;
                    foreach (TelemoveisCartoesView item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["numCartao"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.NumCartao);
                            Col = Col + 1;
                        }
                        if (dp["tipoServico_Show"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.TipoServico_Show);
                            Col = Col + 1;
                        }
                        if (dp["estado_Show"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Estado_Show);
                            Col = Col + 1;
                        }
                        if (dp["contaSuch"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ContaSuch);
                            Col = Col + 1;
                        }
                        if (dp["codRegiao"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodRegiao);
                            Col = Col + 1;
                        }
                        if (dp["codAreaFuncional"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodAreaFuncional);
                            Col = Col + 1;
                        }
                        if (dp["codCentroResponsabilidade"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodCentroResponsabilidade);
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
        public IActionResult ExportToExcelDownload_TelemoveisCartoes(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "TelemoveisCartoes\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Telemóveis Cartões.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

    }
}