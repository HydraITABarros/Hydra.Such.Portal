using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
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

        public IActionResult Orcamentos_Details(string NoOrcamento)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Orcamentos);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.NoOrcamento = NoOrcamento ?? "";
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














    }
}