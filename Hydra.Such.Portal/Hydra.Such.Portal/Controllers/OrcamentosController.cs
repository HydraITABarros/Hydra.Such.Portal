using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Hydra.Such.Data;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.Approvals;
using Hydra.Such.Data.Logic.Contracts;
using Hydra.Such.Data.Logic.OrcamentoL;
using Hydra.Such.Data.Logic.Project;
using Hydra.Such.Data.NAV;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.OrcamentoVM;
using Hydra.Such.Data.ViewModel.ProjectView;
using Hydra.Such.Portal.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Hydra.Such.Portal.Controllers
{
    public class OrcamentosController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private readonly GeneralConfigurations _generalConfig;
        private readonly IHostingEnvironment _hostingEnvironment;

        public OrcamentosController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IOptions<GeneralConfigurations> appSettingsGeneral, IHostingEnvironment _hostingEnvironment)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            _generalConfig = appSettingsGeneral.Value;
            this._hostingEnvironment = _hostingEnvironment;
        }

        public IActionResult Orcamentos_List()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Orcamentos);
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

        public IActionResult Orcamentos_Details(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Orcamentos);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.NoOrcamento = id ?? "";
                ViewBag.reportServerURL = _config.ReportServerURL;
                ViewBag.UPermissions = UPerm;

                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        #region List
        [HttpPost]
        public JsonResult GetList()
        {
            List<OrcamentosViewModel> result = new List<OrcamentosViewModel>();
            result = DBOrcamentos.GetAll().ParseToViewModel().ToList();

            List<EnumData> AllEstados = EnumerablesFixed.OrcamentosEstados;
            List<UnidadePrestação> AllUnidadesPrestacao = DBFetcUnit.GetAll();
            List<EnumData> AllTipoFaturacao = EnumerablesFixed.ContractBillingTypes;
            List<NAVClientsViewModel> AllClients = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "").ToList();
            List<Contactos> AllContacts = DBContacts.GetAll();
            List<NAVDimValueViewModel> AllRegions = DBNAV2017DimensionValues.GetByDimTypeAndUserId(_config.NAVDatabaseName, _config.NAVCompanyName, 1, User.Identity.Name);
            List<ConfigUtilizadores> AllUsers = DBUserConfigurations.GetAll();

            result.ForEach(x => {
                x.EstadoText = x.IDEstado != null ? AllEstados.Where(y => y.Id == x.IDEstado).FirstOrDefault() != null ? AllEstados.Where(y => y.Id == x.IDEstado).FirstOrDefault().Value : "" : "";
                x.UnidadePrestacaoText = x.UnidadePrestacao != null ? AllUnidadesPrestacao.Where(y => y.Código == x.UnidadePrestacao).FirstOrDefault() != null ? AllUnidadesPrestacao.Where(y => y.Código == x.UnidadePrestacao).FirstOrDefault().Descrição : "" : "";
                x.TipoFaturacaoText = x.TipoFaturacao != null ? AllTipoFaturacao.Where(y => y.Id == x.UnidadePrestacao).FirstOrDefault() != null ? AllTipoFaturacao.Where(y => y.Id == x.UnidadePrestacao).FirstOrDefault().Value : "" : "";
                x.ClienteText = !string.IsNullOrEmpty(x.NoCliente) ? AllClients.Where(y => y.No_ == x.NoCliente).FirstOrDefault() != null ? AllClients.Where(y => y.No_ == x.NoCliente).FirstOrDefault().Name : "" : "";
                x.ContactoText = !string.IsNullOrEmpty(x.NoContacto) ? AllContacts.Where(y => y.No == x.NoContacto).FirstOrDefault() != null ? AllContacts.Where(y => y.No == x.NoContacto).FirstOrDefault().Nome : "" : "";
                x.RegiaoText = !string.IsNullOrEmpty(x.CodRegiao) ? AllRegions.Where(y => y.Code == x.CodRegiao).FirstOrDefault() != null ? AllRegions.Where(y => y.Code == x.CodRegiao).FirstOrDefault().Name : "" : "";

                x.EmailUtilizadorEnvioText = !string.IsNullOrEmpty(x.EmailUtilizadorEnvio) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.EmailUtilizadorEnvio.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.EmailUtilizadorEnvio.ToLower()).FirstOrDefault().Nome : "" : "";
                x.UtilizadorCriacaoText = !string.IsNullOrEmpty(x.UtilizadorCriacao) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorCriacao.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorCriacao.ToLower()).FirstOrDefault().Nome : "" : "";
                x.UtilizadorAceiteText = !string.IsNullOrEmpty(x.UtilizadorAceite) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorAceite.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorAceite.ToLower()).FirstOrDefault().Nome : "" : "";
                x.UtilizadorConcluidoText = !string.IsNullOrEmpty(x.UtilizadorConcluido) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorConcluido.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorConcluido.ToLower()).FirstOrDefault().Nome : "" : "";
                x.UtilizadorModificacaoText = !string.IsNullOrEmpty(x.UtilizadorModificacao) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorModificacao.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorModificacao.ToLower()).FirstOrDefault().Nome : "" : "";
            });

            return Json(result);
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_Orcamentos([FromBody] List<OrcamentosViewModel> Lista)
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
                ISheet excelSheet = workbook.CreateSheet("Orçamentos");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["no"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº do Orçamento"); Col = Col + 1; }
                if (dp["clienteText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cliente"); Col = Col + 1; }
                if (dp["contactoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Contacto"); Col = Col + 1; }
                if (dp["dataValidadeText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Orçamento Válido até"); Col = Col + 1; }
                if (dp["estadoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Estado do Orçamento"); Col = Col + 1; }
                if (dp["descricao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Descrição"); Col = Col + 1; }
                if (dp["regiaoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Região"); Col = Col + 1; }
                if (dp["unidadePrestacaoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Unidade de Prestação"); Col = Col + 1; }
                if (dp["tipoFaturacaoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Tipo Faturação"); Col = Col + 1; }
                if (dp["totalSemIVA"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Total sem IVA"); Col = Col + 1; }
                if (dp["totalComIVA"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Total com IVA"); Col = Col + 1; }
                if (dp["noProposta"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº da Proposta Associada"); Col = Col + 1; }
                if (dp["email"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("E-mail"); Col = Col + 1; }
                if (dp["emailAssunto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Assunto"); Col = Col + 1; }
                if (dp["emailCorpo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Corpo"); Col = Col + 1; }
                if (dp["emailDataEnvioText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data de Envio"); Col = Col + 1; }
                if (dp["emailUtilizadorEnvioText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Utilizador de Envio"); Col = Col + 1; }
                if (dp["dataCriacaoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data de Criação"); Col = Col + 1; }
                if (dp["utilizadorCriacaoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Utilizador de Criação"); Col = Col + 1; }
                if (dp["dataAceiteText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Aceite / Não Aceite"); Col = Col + 1; }
                if (dp["utilizadorAceiteText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Utilizador que Aceitou / Não Aceitou"); Col = Col + 1; }
                if (dp["dataConcluidoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Concluído"); Col = Col + 1; }
                if (dp["utilizadorConcluidoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Utilizador que Concluio"); Col = Col + 1; }
                if (dp["dataModificacaoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Última Alteração"); Col = Col + 1; }
                if (dp["utilizadorModificacaoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Utilizador da Última Modificação"); Col = Col + 1; }

                if (dp != null)
                {
                    int count = 1;
                    foreach (OrcamentosViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["no"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.No); Col = Col + 1; }
                        if (dp["clienteText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ClienteText); Col = Col + 1; }
                        if (dp["contactoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ContactoText); Col = Col + 1; }
                        if (dp["dataValidadeText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DataValidadeText); Col = Col + 1; }
                        if (dp["estadoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.EstadoText); Col = Col + 1; }
                        if (dp["descricao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Descricao); Col = Col + 1; }
                        if (dp["regiaoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.RegiaoText); Col = Col + 1; }
                        if (dp["unidadePrestacaoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.UnidadePrestacaoText); Col = Col + 1; }
                        if (dp["tipoFaturacaoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.TipoFaturacaoText); Col = Col + 1; }
                        if (dp["totalSemIVA"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.TotalSemIVA.ToString()); Col = Col + 1; }
                        if (dp["totalComIVA"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.TotalComIVA.ToString()); Col = Col + 1; }
                        if (dp["noProposta"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NoProposta); Col = Col + 1; }
                        if (dp["email"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Email); Col = Col + 1; }
                        if (dp["emailAssunto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.EmailAssunto); Col = Col + 1; }
                        if (dp["emailCorpo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.EmailCorpo); Col = Col + 1; }
                        if (dp["emailDataEnvioText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.EmailDataEnvioText); Col = Col + 1; }
                        if (dp["emailUtilizadorEnvioText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.EmailUtilizadorEnvioText); Col = Col + 1; }
                        if (dp["dataCriacaoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DataCriacaoText); Col = Col + 1; }
                        if (dp["utilizadorCriacaoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.UtilizadorCriacaoText); Col = Col + 1; }
                        if (dp["dataAceiteText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DataAceiteText); Col = Col + 1; }
                        if (dp["utilizadorAceiteText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.UtilizadorAceiteText); Col = Col + 1; }
                        if (dp["dataConcluidoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DataConcluidoText); Col = Col + 1; }
                        if (dp["utilizadorConcluidoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.UtilizadorConcluidoText); Col = Col + 1; }
                        if (dp["dataModificacaoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DataModificacaoText); Col = Col + 1; }
                        if (dp["utilizadorModificacaoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.UtilizadorModificacaoText); Col = Col + 1; }
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
        public IActionResult ExportToExcelDownload_Orcamentos(string sFileName)
        {
            sFileName = @"/Upload/temp/" + sFileName;
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Orçamentos.xlsx");
        }

        #endregion

        #region details
        [HttpPost]
        public JsonResult GetDetails([FromBody] OrcamentosViewModel ORC)
        {
            ORC.eReasonCode = 3;
            ORC.eMessage = "Ocorreu um erro a obter o Orcamento.";

            try
            {
                if (ORC != null && !string.IsNullOrEmpty(ORC.No))
                {
                    ORC = DBOrcamentos.GetById(ORC.No).ParseToViewModel();

                    ORC.LinhasOrcamentos = DBLinhasOrcamentos.GetAllByOrcamento(ORC.No).ParseToViewModel();
                    ORC.AnexosOrcamentos = DBAttachments.ParseToViewModel(DBAttachments.GetById(ORC.No));

                    ORC.TotalSemIVA = ORC.LinhasOrcamentos.Sum(item => item.Quantidade * item.ValorUnitario);
                    ORC.TotalComIVA = ORC.LinhasOrcamentos.Sum(item => item.TotalLinha);

                    List<EnumData> AllEstados = EnumerablesFixed.OrcamentosEstados;
                    List<UnidadePrestação> AllUnidadesPrestacao = DBFetcUnit.GetAll();
                    List<EnumData> AllTipoFaturacao = EnumerablesFixed.ContractBillingTypes;
                    List<NAVClientsViewModel> AllClients = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "").ToList();
                    List<Contactos> AllContacts = DBContacts.GetAll();
                    List<NAVDimValueViewModel> AllRegions = DBNAV2017DimensionValues.GetByDimTypeAndUserId(_config.NAVDatabaseName, _config.NAVCompanyName, 1, User.Identity.Name);
                    List<ConfigUtilizadores> AllUsers = DBUserConfigurations.GetAll();

                    ORC.EstadoText = ORC.IDEstado != null ? AllEstados.Where(y => y.Id == ORC.IDEstado).FirstOrDefault() != null ? AllEstados.Where(y => y.Id == ORC.IDEstado).FirstOrDefault().Value : "" : "";
                    ORC.UnidadePrestacaoText = ORC.UnidadePrestacao != null ? AllUnidadesPrestacao.Where(y => y.Código == ORC.UnidadePrestacao).FirstOrDefault() != null ? AllUnidadesPrestacao.Where(y => y.Código == ORC.UnidadePrestacao).FirstOrDefault().Descrição : "" : "";
                    ORC.TipoFaturacaoText = ORC.TipoFaturacao != null ? AllTipoFaturacao.Where(y => y.Id == ORC.UnidadePrestacao).FirstOrDefault() != null ? AllTipoFaturacao.Where(y => y.Id == ORC.UnidadePrestacao).FirstOrDefault().Value : "" : "";
                    ORC.ClienteText = !string.IsNullOrEmpty(ORC.NoCliente) ? AllClients.Where(y => y.No_ == ORC.NoCliente).FirstOrDefault() != null ? AllClients.Where(y => y.No_ == ORC.NoCliente).FirstOrDefault().Name : "" : "";
                    ORC.ContactoText = !string.IsNullOrEmpty(ORC.NoContacto) ? AllContacts.Where(y => y.No == ORC.NoContacto).FirstOrDefault() != null ? AllContacts.Where(y => y.No == ORC.NoContacto).FirstOrDefault().Nome : "" : "";
                    ORC.RegiaoText = !string.IsNullOrEmpty(ORC.CodRegiao) ? AllRegions.Where(y => y.Code == ORC.CodRegiao).FirstOrDefault() != null ? AllRegions.Where(y => y.Code == ORC.CodRegiao).FirstOrDefault().Name : "" : "";

                    ORC.EmailUtilizadorEnvioText = !string.IsNullOrEmpty(ORC.EmailUtilizadorEnvio) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == ORC.EmailUtilizadorEnvio.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == ORC.EmailUtilizadorEnvio.ToLower()).FirstOrDefault().Nome : "" : "";
                    ORC.UtilizadorCriacaoText = !string.IsNullOrEmpty(ORC.UtilizadorCriacao) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == ORC.UtilizadorCriacao.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == ORC.UtilizadorCriacao.ToLower()).FirstOrDefault().Nome : "" : "";
                    ORC.UtilizadorAceiteText = !string.IsNullOrEmpty(ORC.UtilizadorAceite) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == ORC.UtilizadorAceite.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == ORC.UtilizadorAceite.ToLower()).FirstOrDefault().Nome : "" : "";
                    ORC.UtilizadorConcluidoText = !string.IsNullOrEmpty(ORC.UtilizadorConcluido) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == ORC.UtilizadorConcluido.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == ORC.UtilizadorConcluido.ToLower()).FirstOrDefault().Nome : "" : "";
                    ORC.UtilizadorModificacaoText = !string.IsNullOrEmpty(ORC.UtilizadorModificacao) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == ORC.UtilizadorModificacao.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == ORC.UtilizadorModificacao.ToLower()).FirstOrDefault().Nome : "" : "";

                    using (var ctx = new SuchDBContext())
                    {
                        ctx.Orcamentos.Update(ORC.ParseToDB());
                        ctx.SaveChanges();
                    }

                    ORC.eReasonCode = 1;
                }
            }
            catch (Exception ex)
            {
                ORC.eReasonCode = 3;
                ORC.eMessage = "Ocorreu um erro a obter o Orcamento.";
                return Json(ORC);
            }

            return Json(ORC);
        }

        [HttpPost]
        public JsonResult UpdateOrcamento([FromBody] OrcamentosViewModel ORCAMENTO)
        {
            ORCAMENTO.eReasonCode = 3;
            ORCAMENTO.eMessage = "Ocorreu um erro ao atualizar o Orçamento.";

            try
            {
                if (ORCAMENTO != null && !string.IsNullOrEmpty(ORCAMENTO.No))
                {
                    ORCAMENTO.UtilizadorModificacao = User.Identity.Name;
                    ORCAMENTO.DataModificacao = DateTime.Now;

                    if (DBOrcamentos.Update(ORCAMENTO.ParseToDB()) != null)
                    {
                        ORCAMENTO.eReasonCode = 1;
                        return Json(ORCAMENTO);
                    }
                    else
                    {
                        ORCAMENTO.eReasonCode = 3;
                        ORCAMENTO.eMessage = "Ocorreu um erro ao atualizar o Orçamento.";
                        return Json(ORCAMENTO);
                    }
                }
            }
            catch (Exception ex)
            {
                ORCAMENTO.eReasonCode = 3;
                ORCAMENTO.eMessage = "Ocorreu um erro ao atualizar o Orçamento.";
                return Json(ORCAMENTO);
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult CreateLinha([FromBody] LinhasOrcamentosViewModel LINHA)
        {
            LINHA.eReasonCode = 3;
            LINHA.eMessage = "Ocorreu um erro ao criar a Linha.";

            try
            {
                if (LINHA != null && !string.IsNullOrEmpty(LINHA.NoOrcamento) && !string.IsNullOrEmpty(LINHA.Descricao) && LINHA.Quantidade != null && LINHA.ValorUnitario != null && LINHA.TaxaIVA != null && LINHA.TotalLinha != null)
                {
                    LINHA.UtilizadorCriacao = User.Identity.Name;
                    LINHA.DataCriacao = DateTime.Now;

                    if (DBLinhasOrcamentos.Create(LINHA.ParseToDB()) != null)
                    {
                        LINHA.eReasonCode = 1;
                        return Json(LINHA);
                    }
                    else
                    {
                        LINHA.eReasonCode = 3;
                        LINHA.eMessage = "Ocorreu um erro ao criar a Linha.";
                        return Json(LINHA);
                    }
                }
                else
                {
                    LINHA.eReasonCode = 3;
                    LINHA.eMessage = "Existem campos por preencher.";
                    return Json(LINHA);
                }
            }
            catch (Exception ex)
            {
                LINHA.eReasonCode = 3;
                LINHA.eMessage = "Ocorreu um erro ao criar a Linha.";
                return Json(LINHA);
            }
        }

        [HttpPost]
        public JsonResult DeleteLinha([FromBody] LinhasOrcamentosViewModel LINHA)
        {
            LINHA.eReasonCode = 3;
            LINHA.eMessage = "Ocorreu um erro ao eliminar a Linha.";

            try
            {
                if (LINHA != null && LINHA.NoLinha > 0)
                {
                    if (DBLinhasOrcamentos.Delete(LINHA.NoLinha) == true)
                    {
                        LINHA.eReasonCode = 1;
                        return Json(LINHA);
                    }
                    else
                    {
                        LINHA.eReasonCode = 3;
                        LINHA.eMessage = "Ocorreu um erro ao eliminar a Linha.";
                        return Json(LINHA);
                    }
                }
                else
                {
                    LINHA.eReasonCode = 3;
                    LINHA.eMessage = "A linha tem que existir.";
                    return Json(LINHA);
                }
            }
            catch (Exception ex)
            {
                LINHA.eReasonCode = 3;
                LINHA.eMessage = "Ocorreu um erro ao eliminar a Linha.";
                return Json(LINHA);
            }
        }

        [HttpPost]
        public JsonResult LoadAttachments([FromBody] JObject requestParams)
        {
            string id = requestParams["no"].ToString();

            List<Anexos> list = DBAttachments.GetById(id);
            List<AttachmentsViewModel> attach = new List<AttachmentsViewModel>();
            list.ForEach(x => attach.Add(DBAttachments.ParseToViewModel(x)));
            return Json(attach);
        }

        [HttpGet]
        public FileStreamResult DownloadFile(string id)
        {
            if (_generalConfig.Conn == "eSUCH_Prod" || _generalConfig.Conn == "PlataformaOperacionalSUCH_TST")
                return new FileStreamResult(new FileStream("E:\\Data\\eSUCH\\Orcamentos\\" + id, FileMode.Open), "application/xlsx");
            else
                return new FileStreamResult(new FileStream("C:\\Data\\eSUCH\\Orcamentos\\" + id, FileMode.Open), "application/xlsx");
        }

        [HttpPost]
        [Route("Orcamentos/FileUpload")]
        [Route("Orcamentos/FileUpload/{id}")]
        public JsonResult FileUpload(string id)
        {
            try
            {
                var files = Request.Form.Files;
                string full_filename;
                foreach (var file in files)
                {
                    try
                    {
                        string extension = Path.GetExtension(file.FileName);
                        if (extension.ToLower() == ".msg" ||
                            extension.ToLower() == ".txt" || extension.ToLower() == ".text" ||
                            extension.ToLower() == ".pdf" ||
                            extension.ToLower() == ".xls" || extension.ToLower() == ".xlsx" ||
                            extension.ToLower() == ".doc" || extension.ToLower() == ".docx" || extension.ToLower() == ".dotx" ||
                            extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".pjpeg" || extension.ToLower() == ".jfif" || extension.ToLower() == ".pjp" ||
                            extension.ToLower() == ".png" || extension.ToLower() == ".gif")
                        {
                            string filename = Path.GetFileName(file.FileName);

                            full_filename = id + "_" + filename;
                            var path = "";
                            if (_generalConfig.Conn == "eSUCH_Prod" || _generalConfig.Conn == "PlataformaOperacionalSUCH_TST")
                                path = Path.Combine("E:\\Data\\eSUCH\\Orcamentos\\", full_filename);
                            else
                                path = Path.Combine("C:\\Data\\eSUCH\\Orcamentos\\", full_filename);

                            using (FileStream dd = new FileStream(path, FileMode.CreateNew))
                            {
                                file.CopyTo(dd);
                                dd.Dispose();

                                Anexos newfile = new Anexos();
                                newfile.NºOrigem = id;
                                newfile.UrlAnexo = full_filename;
                                newfile.TipoOrigem = TipoOrigemAnexos.Orcamentos;
                                newfile.DataHoraCriação = DateTime.Now;
                                newfile.UtilizadorCriação = User.Identity.Name;

                                DBAttachments.Create(newfile);
                                if (newfile.NºLinha == 0)
                                {
                                    System.IO.File.Delete(path);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return Json(true);
        }

        [HttpPost]
        public JsonResult DeleteAnexo([FromBody] AttachmentsViewModel requestParams)
        {
            try
            {
                if (_generalConfig.Conn == "eSUCH_Prod" || _generalConfig.Conn == "PlataformaOperacionalSUCH_TST")
                    System.IO.File.Delete("E:\\Data\\eSUCH\\Orcamentos\\" + requestParams.Url);
                else
                    System.IO.File.Delete("C:\\Data\\eSUCH\\Orcamentos\\" + requestParams.Url);

                DBAttachments.Delete(DBAttachments.ParseToDB(requestParams));
                requestParams.eReasonCode = 1;
            }
            catch (Exception ex)
            {
                requestParams.eReasonCode = 2;
                return Json(requestParams);
            }
            return Json(requestParams);
        }

        [HttpPost]
        public JsonResult SendEmail([FromBody] OrcamentosViewModel ORCAMENTO)
        {
            ORCAMENTO.eReasonCode = 3;
            ORCAMENTO.eMessage = "Ocorreu um erro ao enviar o email.";

            try
            {
                if (ORCAMENTO != null && !string.IsNullOrEmpty(ORCAMENTO.Email) && !string.IsNullOrEmpty(ORCAMENTO.EmailAssunto) && !string.IsNullOrEmpty(ORCAMENTO.EmailCorpo))
                {
                    //ENVIO DE EMAIL
                    SendEmailApprovals Email = new SendEmailApprovals();

                    Email.Subject = ORCAMENTO.EmailAssunto;
                    Email.From = User.Identity.Name;
                    Email.To.Add(ORCAMENTO.Email);
                    Email.Body = ORCAMENTO.EmailCorpo;
                    Email.IsBodyHtml = true;

                    Email.SendEmail_Simple();

                    Orcamentos ORC_DB = ORCAMENTO.ParseToDB();
                    ORC_DB.EmailDataEnvio = DateTime.Now;
                    ORC_DB.EmailUtilizadorEnvio = User.Identity.Name;
                    ORC_DB.DataModificacao = DateTime.Now;
                    ORC_DB.UtilizadorModificacao = User.Identity.Name;

                    if (DBOrcamentos.Update(ORC_DB) != null)
                    {
                        ORCAMENTO.eReasonCode = 1;
                        return Json(ORCAMENTO);
                    }
                    else
                    {
                        ORCAMENTO.eReasonCode = 3;
                        ORCAMENTO.eMessage = "Ocorreu um erro ao atualizar o Orçamento.";
                        return Json(ORCAMENTO);
                    }
                }
                else
                {
                    ORCAMENTO.eReasonCode = 3;
                    ORCAMENTO.eMessage = "Existem campos obrigatórios por preencher.";
                    return Json(ORCAMENTO);
                }
            }
            catch (Exception ex)
            {
                ORCAMENTO.eReasonCode = 3;
                ORCAMENTO.eMessage = "Ocorreu um erro ao atualizar o Orçamento.";
                return Json(ORCAMENTO);
            }
        }









        #endregion












    }
}