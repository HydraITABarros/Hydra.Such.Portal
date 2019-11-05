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
using Hydra.Such.Data.Logic.PedidoCotacao;
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

            if (UPerm != null && UPerm.Create.Value == true && string.IsNullOrEmpty(id))
            {
                //Get Orcamentos Numeration
                Configuração Configs = DBConfigurations.GetById(1);
                int OrcamentosNumerationConfigurationId = Configs.NumeracaoOrcamentos.Value;
                id = DBNumerationConfigurations.GetNextNumeration(OrcamentosNumerationConfigurationId, true, false);

                //Update Last Numeration Used
                ConfiguraçãoNumerações ConfigNumerations = DBNumerationConfigurations.GetById(OrcamentosNumerationConfigurationId);
                ConfigNumerations.ÚltimoNºUsado = id;
                DBNumerationConfigurations.Update(ConfigNumerations);

                Orcamentos ORC = new Orcamentos();
                ORC.No = id;
                ORC.IDEstado = 1;
                ORC.TotalSemIVA = 0;
                ORC.TotalComIVA = 0;
                ORC.DataCriacao = DateTime.Now;
                ORC.UtilizadorCriacao = User.Identity.Name;
                if (DBOrcamentos.Create(ORC) != null)
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
            List<NAVContactsViewModel> AllContacts = DBNAV2017Contacts.GetContacts(_config.NAVDatabaseName, _config.NAVCompanyName, "").ToList();
            List<NAVDimValueViewModel> AllRegions = DBNAV2017DimensionValues.GetByDimTypeAndUserId(_config.NAVDatabaseName, _config.NAVCompanyName, 1, User.Identity.Name);
            List<Projetos> AllProjetos = DBProjects.GetAll();
            List<ConfigUtilizadores> AllUsers = DBUserConfigurations.GetAll();

            result.ForEach(x => {
                x.EstadoText = x.IDEstado != null ? AllEstados.Where(y => y.Id == x.IDEstado).FirstOrDefault() != null ? AllEstados.Where(y => y.Id == x.IDEstado).FirstOrDefault().Value : "" : "";
                x.UnidadePrestacaoText = x.UnidadePrestacao != null ? AllUnidadesPrestacao.Where(y => y.Código == x.UnidadePrestacao).FirstOrDefault() != null ? AllUnidadesPrestacao.Where(y => y.Código == x.UnidadePrestacao).FirstOrDefault().Descrição : "" : "";
                x.TipoFaturacaoText = x.TipoFaturacao != null ? AllTipoFaturacao.Where(y => y.Id == x.TipoFaturacao).FirstOrDefault() != null ? AllTipoFaturacao.Where(y => y.Id == x.TipoFaturacao).FirstOrDefault().Value : "" : "";
                x.ClienteText = !string.IsNullOrEmpty(x.NoCliente) ? AllClients.Where(y => y.No_ == x.NoCliente).FirstOrDefault() != null ? AllClients.Where(y => y.No_ == x.NoCliente).FirstOrDefault().Name : "" : "";
                x.ContactoText = !string.IsNullOrEmpty(x.NoContacto) ? AllContacts.Where(y => y.No_ == x.NoContacto).FirstOrDefault() != null ? AllContacts.Where(y => y.No_ == x.NoContacto).FirstOrDefault().Name : "" : "";
                x.RegiaoText = !string.IsNullOrEmpty(x.CodRegiao) ? AllRegions.Where(y => y.Code == x.CodRegiao).FirstOrDefault() != null ? AllRegions.Where(y => y.Code == x.CodRegiao).FirstOrDefault().Name : "" : "";
                x.ProjetoAssociadoText = !string.IsNullOrEmpty(x.ProjetoAssociado) ? AllProjetos.Where(y => y.NºProjeto == x.ProjetoAssociado).FirstOrDefault() != null ? AllProjetos.Where(y => y.NºProjeto == x.ProjetoAssociado).FirstOrDefault().Descrição : "" : "";

                x.EmailUtilizadorEnvioText = !string.IsNullOrEmpty(x.EmailUtilizadorEnvio) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.EmailUtilizadorEnvio.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.EmailUtilizadorEnvio.ToLower()).FirstOrDefault().Nome : "" : "";
                x.UtilizadorCriacaoText = !string.IsNullOrEmpty(x.UtilizadorCriacao) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorCriacao.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorCriacao.ToLower()).FirstOrDefault().Nome : "" : "";
                x.UtilizadorAceiteText = !string.IsNullOrEmpty(x.UtilizadorAceite) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorAceite.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorAceite.ToLower()).FirstOrDefault().Nome : "" : "";
                x.UtilizadorNaoAceiteText = !string.IsNullOrEmpty(x.UtilizadorNaoAceite) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorNaoAceite.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorNaoAceite.ToLower()).FirstOrDefault().Nome : "" : "";
                x.UtilizadorConcluidoText = !string.IsNullOrEmpty(x.UtilizadorConcluido) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorConcluido.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorConcluido.ToLower()).FirstOrDefault().Nome : "" : "";
                x.UtilizadorModificacaoText = !string.IsNullOrEmpty(x.UtilizadorModificacao) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorModificacao.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorModificacao.ToLower()).FirstOrDefault().Nome : "" : "";
            });

            return Json(result.OrderByDescending(x => x.No));
        }

        [HttpPost]
        public JsonResult GetListByEstado([FromBody] OrcamentosViewModel ORC)
        {
            
            List<OrcamentosViewModel> result = new List<OrcamentosViewModel>();
            if (ORC == null || ORC.IDEstado == null || ORC.IDEstado == 0)
                result = DBOrcamentos.GetAll().ParseToViewModel().ToList();
            else
                result = DBOrcamentos.GetAllByEstado((int)ORC.IDEstado).ParseToViewModel().ToList();

            List<EnumData> AllEstados = EnumerablesFixed.OrcamentosEstados;
            List<UnidadePrestação> AllUnidadesPrestacao = DBFetcUnit.GetAll();
            List<EnumData> AllTipoFaturacao = EnumerablesFixed.ContractBillingTypes;
            List<NAVClientsViewModel> AllClients = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "").ToList();
            List<NAVContactsViewModel> AllContacts = DBNAV2017Contacts.GetContacts(_config.NAVDatabaseName, _config.NAVCompanyName, "").ToList();
            List<NAVDimValueViewModel> AllRegions = DBNAV2017DimensionValues.GetByDimTypeAndUserId(_config.NAVDatabaseName, _config.NAVCompanyName, 1, User.Identity.Name);
            List<Projetos> AllProjetos = DBProjects.GetAll();
            List<ConfigUtilizadores> AllUsers = DBUserConfigurations.GetAll();

            result.ForEach(x => {
                x.EstadoText = x.IDEstado != null ? AllEstados.Where(y => y.Id == x.IDEstado).FirstOrDefault() != null ? AllEstados.Where(y => y.Id == x.IDEstado).FirstOrDefault().Value : "" : "";
                x.UnidadePrestacaoText = x.UnidadePrestacao != null ? AllUnidadesPrestacao.Where(y => y.Código == x.UnidadePrestacao).FirstOrDefault() != null ? AllUnidadesPrestacao.Where(y => y.Código == x.UnidadePrestacao).FirstOrDefault().Descrição : "" : "";
                x.TipoFaturacaoText = x.TipoFaturacao != null ? AllTipoFaturacao.Where(y => y.Id == x.TipoFaturacao).FirstOrDefault() != null ? AllTipoFaturacao.Where(y => y.Id == x.TipoFaturacao).FirstOrDefault().Value : "" : "";
                x.ClienteText = !string.IsNullOrEmpty(x.NoCliente) ? AllClients.Where(y => y.No_ == x.NoCliente).FirstOrDefault() != null ? AllClients.Where(y => y.No_ == x.NoCliente).FirstOrDefault().Name : "" : "";
                x.ContactoText = !string.IsNullOrEmpty(x.NoContacto) ? AllContacts.Where(y => y.No_ == x.NoContacto).FirstOrDefault() != null ? AllContacts.Where(y => y.No_ == x.NoContacto).FirstOrDefault().Name : "" : "";
                x.RegiaoText = !string.IsNullOrEmpty(x.CodRegiao) ? AllRegions.Where(y => y.Code == x.CodRegiao).FirstOrDefault() != null ? AllRegions.Where(y => y.Code == x.CodRegiao).FirstOrDefault().Name : "" : "";
                x.ProjetoAssociadoText = !string.IsNullOrEmpty(x.ProjetoAssociado) ? AllProjetos.Where(y => y.NºProjeto == x.ProjetoAssociado).FirstOrDefault() != null ? AllProjetos.Where(y => y.NºProjeto == x.ProjetoAssociado).FirstOrDefault().Descrição : "" : "";

                x.EmailUtilizadorEnvioText = !string.IsNullOrEmpty(x.EmailUtilizadorEnvio) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.EmailUtilizadorEnvio.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.EmailUtilizadorEnvio.ToLower()).FirstOrDefault().Nome : "" : "";
                x.UtilizadorCriacaoText = !string.IsNullOrEmpty(x.UtilizadorCriacao) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorCriacao.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorCriacao.ToLower()).FirstOrDefault().Nome : "" : "";
                x.UtilizadorAceiteText = !string.IsNullOrEmpty(x.UtilizadorAceite) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorAceite.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorAceite.ToLower()).FirstOrDefault().Nome : "" : "";
                x.UtilizadorNaoAceiteText = !string.IsNullOrEmpty(x.UtilizadorNaoAceite) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorNaoAceite.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorNaoAceite.ToLower()).FirstOrDefault().Nome : "" : "";
                x.UtilizadorConcluidoText = !string.IsNullOrEmpty(x.UtilizadorConcluido) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorConcluido.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorConcluido.ToLower()).FirstOrDefault().Nome : "" : "";
                x.UtilizadorModificacaoText = !string.IsNullOrEmpty(x.UtilizadorModificacao) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorModificacao.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorModificacao.ToLower()).FirstOrDefault().Nome : "" : "";
            });

            return Json(result.OrderByDescending(x => x.No));
        }

        [HttpPost]
        public JsonResult GetListMeusOrcamentos()
        {

            List<OrcamentosViewModel> result = new List<OrcamentosViewModel>();
            result = DBOrcamentos.GetAllByMeusOrcamentos(User.Identity.Name).ParseToViewModel().ToList();

            List<EnumData> AllEstados = EnumerablesFixed.OrcamentosEstados;
            List<UnidadePrestação> AllUnidadesPrestacao = DBFetcUnit.GetAll();
            List<EnumData> AllTipoFaturacao = EnumerablesFixed.ContractBillingTypes;
            List<NAVClientsViewModel> AllClients = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "").ToList();
            List<NAVContactsViewModel> AllContacts = DBNAV2017Contacts.GetContacts(_config.NAVDatabaseName, _config.NAVCompanyName, "").ToList();
            List<NAVDimValueViewModel> AllRegions = DBNAV2017DimensionValues.GetByDimTypeAndUserId(_config.NAVDatabaseName, _config.NAVCompanyName, 1, User.Identity.Name);
            List<Projetos> AllProjetos = DBProjects.GetAll();
            List<ConfigUtilizadores> AllUsers = DBUserConfigurations.GetAll();

            result.ForEach(x => {
                x.EstadoText = x.IDEstado != null ? AllEstados.Where(y => y.Id == x.IDEstado).FirstOrDefault() != null ? AllEstados.Where(y => y.Id == x.IDEstado).FirstOrDefault().Value : "" : "";
                x.UnidadePrestacaoText = x.UnidadePrestacao != null ? AllUnidadesPrestacao.Where(y => y.Código == x.UnidadePrestacao).FirstOrDefault() != null ? AllUnidadesPrestacao.Where(y => y.Código == x.UnidadePrestacao).FirstOrDefault().Descrição : "" : "";
                x.TipoFaturacaoText = x.TipoFaturacao != null ? AllTipoFaturacao.Where(y => y.Id == x.TipoFaturacao).FirstOrDefault() != null ? AllTipoFaturacao.Where(y => y.Id == x.TipoFaturacao).FirstOrDefault().Value : "" : "";
                x.ClienteText = !string.IsNullOrEmpty(x.NoCliente) ? AllClients.Where(y => y.No_ == x.NoCliente).FirstOrDefault() != null ? AllClients.Where(y => y.No_ == x.NoCliente).FirstOrDefault().Name : "" : "";
                x.ContactoText = !string.IsNullOrEmpty(x.NoContacto) ? AllContacts.Where(y => y.No_ == x.NoContacto).FirstOrDefault() != null ? AllContacts.Where(y => y.No_ == x.NoContacto).FirstOrDefault().Name : "" : "";
                x.RegiaoText = !string.IsNullOrEmpty(x.CodRegiao) ? AllRegions.Where(y => y.Code == x.CodRegiao).FirstOrDefault() != null ? AllRegions.Where(y => y.Code == x.CodRegiao).FirstOrDefault().Name : "" : "";
                x.ProjetoAssociadoText = !string.IsNullOrEmpty(x.ProjetoAssociado) ? AllProjetos.Where(y => y.NºProjeto == x.ProjetoAssociado).FirstOrDefault() != null ? AllProjetos.Where(y => y.NºProjeto == x.ProjetoAssociado).FirstOrDefault().Descrição : "" : "";

                x.EmailUtilizadorEnvioText = !string.IsNullOrEmpty(x.EmailUtilizadorEnvio) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.EmailUtilizadorEnvio.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.EmailUtilizadorEnvio.ToLower()).FirstOrDefault().Nome : "" : "";
                x.UtilizadorCriacaoText = !string.IsNullOrEmpty(x.UtilizadorCriacao) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorCriacao.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorCriacao.ToLower()).FirstOrDefault().Nome : "" : "";
                x.UtilizadorAceiteText = !string.IsNullOrEmpty(x.UtilizadorAceite) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorAceite.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorAceite.ToLower()).FirstOrDefault().Nome : "" : "";
                x.UtilizadorNaoAceiteText = !string.IsNullOrEmpty(x.UtilizadorNaoAceite) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorNaoAceite.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorNaoAceite.ToLower()).FirstOrDefault().Nome : "" : "";
                x.UtilizadorConcluidoText = !string.IsNullOrEmpty(x.UtilizadorConcluido) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorConcluido.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorConcluido.ToLower()).FirstOrDefault().Nome : "" : "";
                x.UtilizadorModificacaoText = !string.IsNullOrEmpty(x.UtilizadorModificacao) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorModificacao.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == x.UtilizadorModificacao.ToLower()).FirstOrDefault().Nome : "" : "";
            });

            return Json(result.OrderByDescending(x => x.No));
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_Orcamentos([FromBody] List<OrcamentosViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "Orcamentos\\" + "tmp\\";
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
                if (dp["projetoAssociadoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Projeto Associado"); Col = Col + 1; }
                if (dp["noProposta"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº da Proposta Associada"); Col = Col + 1; }
                if (dp["email"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("E-mail"); Col = Col + 1; }
                if (dp["emailAssunto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Assunto"); Col = Col + 1; }
                if (dp["emailCorpo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Corpo"); Col = Col + 1; }
                if (dp["emailDataEnvioText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data de Envio"); Col = Col + 1; }
                if (dp["emailUtilizadorEnvioText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Utilizador de Envio"); Col = Col + 1; }
                if (dp["dataCriacaoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data de Criação"); Col = Col + 1; }
                if (dp["utilizadorCriacaoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Utilizador de Criação"); Col = Col + 1; }
                if (dp["dataAceiteText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Aceite"); Col = Col + 1; }
                if (dp["utilizadorAceiteText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Utilizador que Aceitou"); Col = Col + 1; }
                if (dp["dataNaoAceiteText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Não Aceite"); Col = Col + 1; }
                if (dp["utilizadorNaoAceiteText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Utilizador que Não Aceitou"); Col = Col + 1; }
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
                        if (dp["projetoAssociadoText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ProjetoAssociadoText); Col = Col + 1; }
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
                        if (dp["dataNaoAceiteText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DataNaoAceiteText); Col = Col + 1; }
                        if (dp["utilizadorNaoAceiteText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.UtilizadorNaoAceiteText); Col = Col + 1; }
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
            sFileName = _generalConfig.FileUploadFolder + "Orcamentos\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Orçamentos.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
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
                    ORC.UtilizadorNaoAceiteText = !string.IsNullOrEmpty(ORC.UtilizadorNaoAceite) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == ORC.UtilizadorNaoAceite.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == ORC.UtilizadorNaoAceite.ToLower()).FirstOrDefault().Nome : "" : "";
                    ORC.UtilizadorConcluidoText = !string.IsNullOrEmpty(ORC.UtilizadorConcluido) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == ORC.UtilizadorConcluido.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == ORC.UtilizadorConcluido.ToLower()).FirstOrDefault().Nome : "" : "";
                    ORC.UtilizadorModificacaoText = !string.IsNullOrEmpty(ORC.UtilizadorModificacao) ? AllUsers.Where(y => y.IdUtilizador.ToLower() == ORC.UtilizadorModificacao.ToLower()).FirstOrDefault() != null ? AllUsers.Where(y => y.IdUtilizador.ToLower() == ORC.UtilizadorModificacao.ToLower()).FirstOrDefault().Nome : "" : "";

                    if (ORC.IDEstado == 4)
                    {
                        ORC.LinhasOrcamentos.ForEach(x => x.Visivel = false);
                        ORC.AnexosOrcamentos.ForEach(x => x.Visivel = false);
                    }
                    else
                    {
                        ORC.LinhasOrcamentos.ForEach(x => x.Visivel = true);
                        ORC.AnexosOrcamentos.ForEach(x => x.Visivel = true);
                    }

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
                    if (ORCAMENTO.LinhasOrcamentos != null && ORCAMENTO.LinhasOrcamentos.Count > 0)
                    {
                        foreach (LinhasOrcamentosViewModel linha in ORCAMENTO.LinhasOrcamentos)
                        {
                            if (linha.Quantidade != null || linha.ValorUnitario != null || linha.TaxaIVA != null || linha.TotalLinha != null)
                            {
                                if (linha.Quantidade == null)
                                    linha.Quantidade = 0;
                                if (linha.ValorUnitario == null)
                                    linha.ValorUnitario = 0;
                                if (linha.TaxaIVA == null)
                                    linha.TaxaIVA = 0;
                                if (linha.TotalLinha == null)
                                    linha.TotalLinha = 0;

                                decimal Custo = (decimal)(linha.Quantidade * linha.ValorUnitario);
                                decimal ValorIVA = (decimal)((linha.TaxaIVA * Custo) / 100);

                                linha.TotalLinha = Custo + ValorIVA;
                            }

                            DBLinhasOrcamentos.Update(linha.ParseToDB());
                        }

                        ORCAMENTO.TotalSemIVA = ORCAMENTO.LinhasOrcamentos.Sum(item => item.Quantidade * item.ValorUnitario);
                        ORCAMENTO.TotalComIVA = ORCAMENTO.LinhasOrcamentos.Sum(item => item.TotalLinha);
                    }

                    Orcamentos OrcOriginal = DBOrcamentos.GetById(ORCAMENTO.No);
                    if (OrcOriginal != null)
                    {
                        //2 = Aceite
                        if (ORCAMENTO.IDEstado == 2 && ORCAMENTO.IDEstado != OrcOriginal.IDEstado)
                        {
                            ORCAMENTO.DataAceiteText = DateTime.Now.ToString();
                            ORCAMENTO.UtilizadorAceite = User.Identity.Name;
                        }

                        //3 = Não Aceite
                        if (ORCAMENTO.IDEstado == 3 && ORCAMENTO.IDEstado != OrcOriginal.IDEstado)
                        {
                            ORCAMENTO.DataNaoAceiteText = DateTime.Now.ToString();
                            ORCAMENTO.UtilizadorNaoAceite = User.Identity.Name;
                        }

                        //4 = Concluído
                        if (ORCAMENTO.IDEstado == 4 && ORCAMENTO.IDEstado != OrcOriginal.IDEstado)
                        {
                            ORCAMENTO.DataConcluidoText = DateTime.Now.ToString();
                            ORCAMENTO.UtilizadorConcluido = User.Identity.Name;
                        }
                    }

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
        public JsonResult DeleteOrcamento([FromBody] OrcamentosViewModel ORCAMENTO)
        {
            ORCAMENTO.eReasonCode = 3;
            ORCAMENTO.eMessage = "Ocorreu um erro ao eliminar o Orçamento.";

            try
            {
                UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Orcamentos);

                if (UPerm != null && UPerm.Delete.Value == true)
                {
                    if (ORCAMENTO != null && !string.IsNullOrEmpty(ORCAMENTO.No))
                    {
                        List<LinhasOrcamentos> LINHAS = DBLinhasOrcamentos.GetAllByOrcamento(ORCAMENTO.No);
                        if (LINHAS != null && LINHAS.Count > 0)
                        {
                            if (DBLinhasOrcamentos.Delete(LINHAS) != true)
                            {
                                ORCAMENTO.eReasonCode = 3;
                                ORCAMENTO.eMessage = "Ocorreu um erro ao eliminar as Linhas do Orçamento.";
                                return Json(ORCAMENTO);
                            }
                        }

                        List<Anexos> ANEXOS = DBAttachments.GetAll().Where(x => x.TipoOrigem == TipoOrigemAnexos.Orcamentos).ToList();
                        if (ANEXOS != null && ANEXOS.Count > 0)
                        {
                            if (DBAttachments.Delete(ANEXOS) != true)
                            {
                                ORCAMENTO.eReasonCode = 3;
                                ORCAMENTO.eMessage = "Ocorreu um erro ao eliminar os Anexos do Orçamento.";
                                return Json(ORCAMENTO);
                            }
                        }

                        Orcamentos ORC_DB = DBOrcamentos.GetById(ORCAMENTO.No);
                        if (ORC_DB != null)
                        {
                            if (DBOrcamentos.Delete(ORC_DB) == true)
                            {
                                ORCAMENTO.eReasonCode = 1;
                                return Json(ORCAMENTO);
                            }
                            else
                            {
                                ORCAMENTO.eReasonCode = 3;
                                ORCAMENTO.eMessage = "Ocorreu um erro ao eliminar o Orçamento.";
                                return Json(ORCAMENTO);
                            }
                        }
                        else
                        {
                            ORCAMENTO.eReasonCode = 3;
                            ORCAMENTO.eMessage = "Ocorreu um erro ao obter o Orçamento da Base de Dados.";
                            return Json(ORCAMENTO);
                        }
                    }
                    else
                    {
                        ORCAMENTO.eReasonCode = 3;
                        ORCAMENTO.eMessage = "O Orçamento tem que existir.";
                        return Json(ORCAMENTO);
                    }
                }
                else
                {
                    ORCAMENTO.eReasonCode = 3;
                    ORCAMENTO.eMessage = "Não tem permissões para Eliminar Orçamentos.";
                    return Json(ORCAMENTO);
                }
            }
            catch (Exception ex)
            {
                ORCAMENTO.eReasonCode = 3;
                ORCAMENTO.eMessage = "Ocorreu um erro ao eliminar o Orçamento.";
                return Json(ORCAMENTO);
            }
        }

        [HttpPost]
        public JsonResult CreateLinha([FromBody] LinhasOrcamentosViewModel LINHA)
        {
            LINHA.eReasonCode = 3;
            LINHA.eMessage = "Ocorreu um erro ao criar a Linha.";

            try
            {
                if (LINHA != null && !string.IsNullOrEmpty(LINHA.NoOrcamento) && !string.IsNullOrEmpty(LINHA.Descricao))
                {
                    if (LINHA.Ordem == null)
                    {
                        LINHA.Ordem = DBLinhasOrcamentos.GetMaxOrdemByOrcamento(LINHA.NoOrcamento) + 1;
                    }

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
        public async Task<JsonResult> SendEmail([FromBody] OrcamentosViewModel ORCAMENTO)
        {
            ORCAMENTO.eReasonCode = 3;
            ORCAMENTO.eMessage = "Ocorreu um erro ao enviar o email.";

            try
            {
                if (ORCAMENTO != null && !string.IsNullOrEmpty(ORCAMENTO.Email) && !string.IsNullOrEmpty(ORCAMENTO.EmailAssunto) && !string.IsNullOrEmpty(ORCAMENTO.EmailCorpo))
                {
                    string sWebRootFolder = "E:\\Data\\eSUCH\\tmp";
                    string sFileName = "Orcamento" + "_" + ORCAMENTO.No + ".pdf";

                    var theURL = (_config.ReportServerURL_PDF + "Orcamentos&OrcamentosNo=" + ORCAMENTO.No + "&ClienteNo=" + ORCAMENTO.NoCliente + "&ContatoNo=" + ORCAMENTO.NoContacto + "&rs:Command=Render&rs:format=PDF");

                    //OBTER CREDENCIAIS PARA O SERVIDOR DE REPORTS
                    Configuração config = DBConfigurations.GetById(1);

                    WebClient Client = new WebClient
                    {
                        Credentials = new NetworkCredential(config.ReportUsername, config.ReportPassword)
                    };


                    byte[] myDataBuffer = Client.DownloadData(theURL);

                    using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                    {
                        await fs.WriteAsync(myDataBuffer, 0, myDataBuffer.Length);
                    }

                    Stream _my_stream = new MemoryStream(myDataBuffer);

                    using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
                    {
                        await stream.CopyToAsync(_my_stream);
                    }

                    SendEmailsPedidoCotacao Email = new SendEmailsPedidoCotacao
                    {
                        DisplayName = "e-SUCH",
                        From = User.Identity.Name,
                        Subject = ORCAMENTO.EmailAssunto,
                        Anexo = Path.Combine(sWebRootFolder, sFileName),
                        Body = MakeEmailBodyContent(ORCAMENTO.EmailCorpo, User.Identity.Name),
                        IsBodyHtml = true
                    };

                    Email.To.Add(ORCAMENTO.Email);
                    Email.BCC.Add(User.Identity.Name);
                    Email.SendEmail();

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
                ORCAMENTO.eMessage = "Ocorreu um erro ao enviar o E-mail.";
                return Json(ORCAMENTO);
            }
        }

        public static string MakeEmailBodyContent(string BodyText, string SenderName)
        {
            string Body = @"<html>" +
                                "<head>" +
                                    "<style>" +
                                        "table{border:0;} " +
                                        "td{width:600px; vertical-align: top;}" +
                                    "</style>" +
                                "</head>" +
                                "<body>" +
                                    "<table>" +
                                        "<tr><td>&nbsp;</td></tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "Exmos (as) Senhores (as)," +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr><td>&nbsp;</td></tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                BodyText +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "&nbsp;" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "Com os melhores cumprimentos," +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                SenderName +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "&nbsp;" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<i>SUCH - Serviço de Utilização Comum dos Hospitais</i>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                            "</html>";

            return Body;
        }

        [HttpPost]
        public JsonResult CriarProposta([FromBody] OrcamentosViewModel ORCAMENTO)
        {
            ORCAMENTO.eReasonCode = 3;
            ORCAMENTO.eMessage = "Ocorreu um erro ao Criar Proposta.";

            try
            {
                if (ORCAMENTO != null && !string.IsNullOrEmpty(ORCAMENTO.No))
                {
                    if (ORCAMENTO.IDEstado == 2)
                    {
                        if (string.IsNullOrEmpty(ORCAMENTO.NoProposta))
                        {
                            if (!string.IsNullOrEmpty(ORCAMENTO.NoCliente) && !string.IsNullOrEmpty(ORCAMENTO.CodRegiao) &&
                                ORCAMENTO.TipoFaturacao != null && ORCAMENTO.UnidadePrestacao != null)
                            {
                                Configuração Configs = DBConfigurations.GetById(1);
                                string newNumeration = DBNumerationConfigurations.GetNextNumeration(Configs.NumeraçãoPropostas.Value, true, false);
                                Contratos NewProposta = new Contratos
                                {
                                    TipoContrato = 2,
                                    NºDeContrato = newNumeration,
                                    NºVersão = 1,
                                    Estado = 1,
                                    NºCliente = ORCAMENTO.NoCliente,
                                    CódigoRegião = ORCAMENTO.CodRegiao,
                                    TipoFaturação = ORCAMENTO.TipoFaturacao,
                                    UnidadePrestação = ORCAMENTO.UnidadePrestacao,
                                    DataHoraCriação = DateTime.Now,
                                    UtilizadorCriação = User.Identity.Name,
                                    Arquivado = false,
                                    Historico = false,
                                    Tipo = 1
                                };

                                if (DBContracts.Create(NewProposta) != null)
                                {
                                    Orcamentos OrcOriginal = DBOrcamentos.GetById(ORCAMENTO.No);
                                    if (OrcOriginal != null)
                                    {
                                        //2 = Aceite
                                        if (ORCAMENTO.IDEstado == 2 && ORCAMENTO.IDEstado != OrcOriginal.IDEstado)
                                        {
                                            ORCAMENTO.DataAceiteText = DateTime.Now.ToString();
                                            ORCAMENTO.UtilizadorAceite = User.Identity.Name;
                                        }
                                    }

                                    ORCAMENTO.NoProposta = newNumeration;
                                    ORCAMENTO.DataModificacaoText = DateTime.Now.ToString();
                                    ORCAMENTO.UtilizadorModificacao = User.Identity.Name;

                                    if (DBOrcamentos.Update(DBOrcamentos.ParseToDB(ORCAMENTO)) != null)
                                    {
                                        ORCAMENTO.eReasonCode = 1;
                                        ORCAMENTO.eMessage = "A Proposta Nº " + newNumeration + " foi criada com sucesso.";
                                        return Json(ORCAMENTO);
                                    }
                                }
                                else
                                {
                                    ORCAMENTO.eReasonCode = 3;
                                    ORCAMENTO.eMessage = "Ocorreu um erro ao criar a Proposta.";
                                    return Json(ORCAMENTO);
                                }
                            }
                            else
                            {
                                ORCAMENTO.eReasonCode = 3;
                                ORCAMENTO.eMessage = "Os campos Cliente, Região, Unidade de Prestação e Tipo Faturação são de preenchimento obrigatório.";
                                return Json(ORCAMENTO);
                            }
                        }
                        else
                        {
                            ORCAMENTO.eReasonCode = 3;
                            ORCAMENTO.eMessage = "Não é possivel criar Proposta, por existir uma Proposta Nº " + ORCAMENTO.NoProposta + " associada a este Orçamento.";
                            return Json(ORCAMENTO);
                        }
                    }
                    else
                    {
                        ORCAMENTO.eReasonCode = 3;
                        ORCAMENTO.eMessage = "Para criar Proposta, o Orçamento têm que estar no estado Aceite.";
                        return Json(ORCAMENTO);
                    }
                }
                else
                {
                    ORCAMENTO.eReasonCode = 3;
                    ORCAMENTO.eMessage = "Não foi possível obter o Orçamento";
                    return Json(ORCAMENTO);
                }
            }
            catch (Exception ex)
            {
                ORCAMENTO.eReasonCode = 3;
                ORCAMENTO.eMessage = "Ocorreu um erro ao criar a Proposta.";
                return Json(ORCAMENTO);
            }

            return Json(ORCAMENTO);
        }








        #endregion












    }
}