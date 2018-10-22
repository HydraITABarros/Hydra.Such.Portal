using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Compras;
using Hydra.Such.Data.Logic.Request;
using Hydra.Such.Data;
using static Hydra.Such.Data.Enumerations;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.NAV;
using Microsoft.Extensions.Options;
using Hydra.Such.Portal.Services;
using Hydra.Such.Data.Logic.Approvals;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Hydra.Such.Data.Logic.ComprasML;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class FaturacaoController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private BillingReceptionService billingRecService;
        private readonly GeneralConfigurations _generalConfig;
        private readonly IHostingEnvironment _hostingEnvironment;

        public FaturacaoController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IOptions<GeneralConfigurations> appSettingsGeneral, IHostingEnvironment _hostingEnvironment)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            _generalConfig = appSettingsGeneral.Value;
            billingRecService = new BillingReceptionService();
            this._hostingEnvironment = _hostingEnvironment;
        }

        public IActionResult RececaoFaturas()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.ReceçãoFaturação);
            UserConfigurationsViewModel userConfig = DBUserConfigurations.GetById(User.Identity.Name).ParseToViewModel();
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UserPermissions = UPerm;
                ViewBag.RFPerfil = userConfig.RFPerfil;
                ViewBag.RFPerfilVisualizacao = userConfig.RFPerfilVisualizacao;
                ViewBag.UserCanChangeDestination = userConfig.RFAlterarDestinatarios.HasValue ? userConfig.RFAlterarDestinatarios.Value : false;

                bool userCanSeePending = false;
                if (userConfig.RFPerfilVisualizacao.HasValue)
                    userCanSeePending = (userConfig.RFPerfilVisualizacao.Value == BillingReceptionUserProfiles.Perfil) | (userConfig.RFPerfilVisualizacao.Value == BillingReceptionUserProfiles.Tudo);
                ViewBag.UserCanSeePending = userCanSeePending;

                bool userBelongsToProvisioning = false;
                if (userConfig.RFPerfilVisualizacao.HasValue)
                    userBelongsToProvisioning = userConfig.RFPerfil.Value == BillingReceptionAreas.Aprovisionamento;
                ViewBag.UserBelongsToProvisioning = userBelongsToProvisioning;

                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult DetalhesRecFatura(string id)
        {

            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.ReceçãoFaturação);
            UserConfigurationsViewModel userConfig = DBUserConfigurations.GetById(User.Identity.Name).ParseToViewModel();
            
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UploadURL = _generalConfig.FileUploadFolder;
                ViewBag.Id = id;
                ViewBag.UserPermissions = UPerm;
                ViewBag.BillingReceptionStates = EnumHelper.GetItemsAsDictionary(typeof(BillingReceptionStates));
                ViewBag.RFPerfil = userConfig.RFPerfil;
                ViewBag.RFPerfilVisualizacao = userConfig.RFPerfilVisualizacao;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public JsonResult GetBillingReceptions()
        {
            UserConfigurationsViewModel userConfig = DBUserConfigurations.GetById(User.Identity.Name).ParseToViewModel();
            var billingReceptions = billingRecService.GetAllForUser(userConfig);
            return Json(billingReceptions);
        }

        public JsonResult GetBillingReceptionsHistory()
        {
            UserConfigurationsViewModel userConfig = DBUserConfigurations.GetById(User.Identity.Name).ParseToViewModel();
            var billingReceptions = billingRecService.GetAllForUserHist(User.Identity.Name, userConfig.RFPerfilVisualizacao);

            return Json(billingReceptions);
        }

        public JsonResult GetBillingReceptionsPending()
        {
            UserConfigurationsViewModel userConfig = DBUserConfigurations.GetById(User.Identity.Name).ParseToViewModel();
            var billingReceptions = billingRecService.GetPendingForUser(userConfig.RFPerfil, User.Identity.Name);
            return Json(billingReceptions);
            //UserConfigurationsViewModel userConfig = DBUserConfigurations.GetById(User.Identity.Name).ParseToViewModel();
            //BillingReceptionAreas perfil = userConfig.RFPerfil ?? BillingReceptionAreas.Contabilidade;
            //BillingReceptionUserProfiles perfilVisulalizacao = userConfig.RFPerfilVisualizacao ?? BillingReceptionUserProfiles.Tudo;

            //var billingReceptions = billingRecService.GetAllForUserPendingExcept(User.Identity.Name, perfil, perfilVisulalizacao);
            //return Json(billingReceptions);
        }

        public JsonResult GetBillingReceptionsPendingOnAreas()
        {

            //UserConfigurationsViewModel userConfig = DBUserConfigurations.GetById(User.Identity.Name).ParseToViewModel();
            //BillingReceptionAreas areaPendente = userConfig.RFPerfil ?? BillingReceptionAreas.Aprovisionamento;
            //var billingReceptions = billingRecService.GetAllForUserPending();
            var billingReceptions = billingRecService.GetPendingOnAreas(User.Identity.Name);
            return Json(billingReceptions);
        }

        public JsonResult GetChangeableDestination()
        {

            //UserConfigurationsViewModel userConfig = DBUserConfigurations.GetById(User.Identity.Name).ParseToViewModel();
            //BillingReceptionAreas areaPendente = userConfig.RFPerfil ?? BillingReceptionAreas.Aprovisionamento;
            //var billingReceptions = billingRecService.GetAllForUserPending();
            var billingReceptions = billingRecService.GetChangeableDestination(User.Identity.Name);
            return Json(billingReceptions);
        }

        [HttpGet]
        public JsonResult GetBillingReception(string id)
        {
            var billingReception = billingRecService.GetById(id);
            return Json(billingReception);
        }

        [HttpPost]
        public JsonResult CreateBillingReception([FromBody] BillingReceptionModel item)
        {
            BillingReceptionModel createdItem = null;
            if (item != null)
            {
                item.CriadoPor = User.Identity.Name;
                createdItem = billingRecService.Create(item);
                if (createdItem != null)
                {
                    if (createdItem.Id == null || createdItem.Id == "")
                    {
                        createdItem.eReasonCode = 2;
                        createdItem.eMessage = "Ocorreu um erro ao criar o registo, Id vazio!";
                    }
                    else
                    {
                        createdItem.eReasonCode = 1;
                        createdItem.eMessage = "Registo criado com sucesso";
                    }
                    item = createdItem;
                }
                else
                {
                    item.eReasonCode = 2;
                    item.eMessage = "Ocorreu um erro ao criar o registo";
                }
            }
            else
            {
                item.eReasonCode = 2;
                //item.eMessage = "O registo não pode ser nulo";
                item.eMessage = "Ocorreu um Erro: O registo encontra-se vazio";
            }
            return Json(item);
        }

        [HttpPost]
        public JsonResult UpdateBillingReception([FromBody] BillingReceptionModel item)
        {
            BillingReceptionModel updatedItem = null;
            if (item != null)
            {
                item.ModificadoPor = User.Identity.Name;
                updatedItem = billingRecService.Update(item);
                if (updatedItem != null)
                {
                    updatedItem.eReasonCode = 1;
                    updatedItem.eMessage = "Registo atualizado com sucesso";
                }
                else
                {
                    item.eReasonCode = 2;
                    updatedItem = item;
                }
            }
            else
            {
                updatedItem = new BillingReceptionModel();
                updatedItem.eReasonCode = 2;
                //updatedItem.eMessage = "O registo não pode ser nulo";
                item.eMessage = "Ocorreu um Erro: O registo encontra-se vazio";
            }
            return Json(updatedItem);
        }

        [HttpPost]
        public JsonResult ValidateNumeration([FromBody] BillingReceptionModel data)
        {
            ConfigUtilizadores user = DBUserConfigurations.GetById(User.Identity.Name);
            //Get Project Numeration
            if (user.PerfilNumeraçãoRecDocCompras != null)
            {
                int Cfg = (int)user.PerfilNumeraçãoRecDocCompras;
                ConfiguraçãoNumerações CfgNumeration = DBNumerationConfigurations.GetById(Cfg);

                //Validate if ProjectNo is valid
                if (!(data.Id == "" || data.Id == null) && !CfgNumeration.Manual.Value)
                {
                    return Json("A numeração configurada para contratos não permite inserção manual.");
                }
                else if ((data.Id == "" || data.Id == null) && !CfgNumeration.Automático.Value)
                {
                    return Json("É obrigatório inserir o Nº de Contrato.");
                }
                return Json("");
            }
            else {
                return Json("É obrigatório a configuração Perfil de Numeração no utilizador.");
            }

              
        }
        [HttpPost]
        public JsonResult UpdateBillingList([FromBody] List<BillingReceptionModel> items)
        {

            BillingReceptionModel updatedItem = null;
            if (items != null)
            {
                string NovoDestinatario=items[0].Destinatario;
                foreach (BillingReceptionModel item in items)
                {

                    item.ModificadoPor = User.Identity.Name;
                    BillingRecWorkflowModel workflow = new BillingRecWorkflowModel();
                    item.WorkflowItems.Add(workflow);
                    item.AreaUltimaInteracao = workflow.AreaWorkflow;
                    item.UserUltimaInteracao = workflow.CriadoPor;
                    workflow.DataCriacao = DateTime.Now;
                    workflow.Destinatario = NovoDestinatario;
                    workflow.CodTipoProblema = item.TipoProblema;
                    workflow.Area = item.AreaPendente2;
                    workflow.AreaWorkflow= item.AreaPendente;  
                    workflow.Comentario = item.Descricao;
                    workflow.Descricao= item.DescricaoProblema;
                    workflow.Comentario = "Alteração Destinatário";
                    workflow.Estado = BillingReceptionStates.Pendente;
                    item.AreaPendente = "Aprovisionamentos";
                    updatedItem = billingRecService.CreateWorkFlowSend(item, workflow, User.Identity.Name);
                    if (updatedItem == null)
                    {

                        item.eReasonCode = 2;
                        updatedItem = item;

                    }


                }
            }
            else
            {
                updatedItem = new BillingReceptionModel();
                updatedItem.eReasonCode = 2;
                //updatedItem.eMessage = "O registo não pode ser nulo";
                updatedItem.eMessage = "Ocorreu um Erro: O registo encontra-se vazio";
            }
            return Json(updatedItem);
        }

        [HttpPost]
        public JsonResult SendBillingReception([FromBody] BillingReceptionModel item)
        {

            BillingReceptionModel updatedItem = null;
            if (item != null)
            {

                BillingRecWorkflowModel workflow = item.WorkflowItems.LastOrDefault();
                UploadFile(workflow);
                item.WorkflowItems.RemoveAt(item.WorkflowItems.Count - 1);
                workflow.DataCriacao = DateTime.Now;
                item.WorkflowItems.Add(workflow);
                
                updatedItem = billingRecService.CreateWorkFlowSend(item, workflow, User.Identity.Name);
                if (updatedItem != null)
                {
                    updatedItem.eReasonCode = 1;
                    updatedItem.eMessage = "Registo atualizado com sucesso";
                    
                }
                else
                {
                    item.eReasonCode = 2;
                    updatedItem = item;
                }
            }
            else
            {
                updatedItem = new BillingReceptionModel();
                updatedItem.eReasonCode = 2;
                //updatedItem.eMessage = "O registo não pode ser nulo";
                updatedItem.eMessage = "Ocorreu um Erro: O registo encontra-se vazio";
            }
            return Json(updatedItem);
        }

        [HttpPost]
        public JsonResult GetWorkflowAttached([FromBody] BillingRecWorkflowModel item)
        {
            List<BillingRecWorkflowModelAttached> items = DBBillingReceptionWFAttach.ParseToViewModel(DBBillingReceptionWFAttach.GetById(item.Id));
            return Json(items);
        }

        [HttpPost]
        public JsonResult UpdateWorkFlow([FromBody] BillingReceptionModel item)
        {

            BillingReceptionModel updatedItem = null;
            if (item != null)
            {               
                item.ModificadoPor = User.Identity.Name;
                BillingRecWorkflowModel workflow = item.WorkflowItems.LastOrDefault();
                item.WorkflowItems.RemoveAt(item.WorkflowItems.Count - 1);
                item.DescricaoProblema = "";
                item.TipoProblema = "";
                workflow.DataCriacao = DateTime.Now;
                workflow.IdRecFaturacao = item.Id;
                item.WorkflowItems.Add(workflow);
                
                updatedItem = billingRecService.UpdateWorkFlow(item, workflow, User.Identity.Name);
                if (updatedItem != null)
                {
                    item.eReasonCode = 1;
                    item.eMessage = "Registo atualizado com sucesso";
                    item = updatedItem;

                }
                else
                {
                    item.eReasonCode = 2;

                }
            }
            else
            {
                item = new BillingReceptionModel();
                item.eReasonCode = 2;
               // item.eMessage = "O registo não pode ser nulo";
                item.eMessage = "Ocorreu um Erro: O registo encontra-se vazio";
            }
            return Json(item);
        }
        //CF ou CP ou CC opc
        [HttpPost]
        public JsonResult PostDocument([FromBody] BillingReceptionModel item)
        {
            BillingReceptionModel postedDocument;
            if (item != null)
            {
                try
                {
                    UserConfigurationsViewModel userConfig = DBUserConfigurations.GetById(User.Identity.Name).ParseToViewModel();
                    postedDocument = billingRecService.PostDocument(item, User.Identity.Name, userConfig.NumSeriePreFaturasCompra, _config, _configws);
                    item = postedDocument;
                }
                catch (Exception ex)
                {
                    item.eReasonCode = 2;
                    item.eMessage = "Ocorreu um erro ao criar fatura: " + ex.Message;
                }
            }
            else
            {
                item = new BillingReceptionModel();
                item.eReasonCode = 2;
                //item.eMessage = "O registo não pode ser nulo";
                item.eMessage = "Ocorreu um Erro: O registo encontra-se vazio";
            }
            return Json(item);
        }

        [HttpPost]
        public JsonResult OpenOrderNav([FromBody] BillingReceptionModel item)
        {
            BillingReceptionModel postedDocument;
            if (item != null)
            {
                postedDocument = billingRecService.OpenOrder(item, User.Identity.Name, _config, _configws);
                item = postedDocument;
            }
            else
            {
                item = new BillingReceptionModel();
                item.eReasonCode = 2;
                //item.eMessage = "O registo não pode ser nulo";
                item.eMessage = "Ocorreu um Erro: O registo encontra-se vazio";
            }
            return Json(item);
        }
        [HttpPost]
        public JsonResult OpenOrderBillingNav([FromBody] BillingReceptionModel item)
        {
            BillingReceptionModel postedDocument;
            if (item != null)
            {
                postedDocument = billingRecService.OpenOrderByBilling(item, User.Identity.Name, _config, _configws);
                item = postedDocument;
            }
            else
            {
                item = new BillingReceptionModel();
                item.eReasonCode = 2;
                //item.eMessage = "O registo não pode ser nulo";
                item.eMessage = "Ocorreu um Erro: O registo encontra-se vazio";
            }
            return Json(item);
        }
        [HttpPost]
        public ActionResult DocumentIsDigitized([FromBody] BillingReceptionModel item)
        {
            if (item != null)
            {
              
            }
            else
            {
                item.eReasonCode = 2;
                //item.eMessage = "O registo não pode ser nulo";
                item.eMessage = "Ocorreu um Erro: O registo encontra-se vazio";
            }
            return Json(false);
        }

        [HttpGet]
        public JsonResult GetProblems()
        {

            List<DDMessageRelated> result = billingRecService.GetProblem("RF1P").Select(x => new DDMessageRelated()
            {
                id = x.Tipo,
                value = x.Descricao,
                extra = x.EnvioAreas
            }).ToList();

            return Json(result);
        }

        [HttpGet]
        public JsonResult GetUAProblems()
        {
            List<DDMessageRelated> result = billingRecService.GetProblem("RF5P").Select(x => new DDMessageRelated()
            {
                id = x.Tipo,
                value = x.Descricao,
                extra = x.EnvioAreas
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetUserProfileById([FromBody] string user)
        {
            int userPendingProfile = (int)DBUserConfigurations.GetById(user).Rfperfil;
            return Json(userPendingProfile);
        }


        [HttpPost]
        public JsonResult GetAnswers([FromBody] BillingReceptionModel data)
        {
            int userPendingProfile = (int)DBUserConfigurations.GetById(User.Identity.Name).Rfperfil;
            //int userDestinyProfile = (int)DBUserConfigurations.GetById(data.CriadoPor).Rfperfil;
            //string QuestionArea = billingRecService.GetQuestionIDByDesc(data.TipoProblema, data.Descricao).EnvioAreas;
            int userDestinyProfile = 0;
            List<RecFacturasProblemas> result = new List<RecFacturasProblemas>();
            string AnswerType = "";

            if (data.AreaPendente == "Aprovisionamento")
            {
                if (userPendingProfile == 1 && userDestinyProfile == 0)
                {
                    AnswerType = "RF1R";
                    result = billingRecService.GetProblem(AnswerType).ToList();
                } 
            }

            if(data.AreaPendente == "UnidadesProdutivas" || data.AreaPendente == "UnidadesApoioESuporte")
            {
                if ((userPendingProfile == 2 || userPendingProfile == 3) && userDestinyProfile == 1)
                {
                    AnswerType = "RF5R";
                    result = billingRecService.GetProblem(AnswerType).ToList();
                }
            }
            
            //if(QuestionArea == "")
            //{

            //}

            List<DDMessageRelated> answers = result
            .Select(x => new DDMessageRelated()
            {
                id = x.Tipo,
                value = x.Descricao,
                extra = x.Codigo
            }).ToList();

            return Json(answers);
        }

        [HttpGet]
        public JsonResult GetReasons()
        {
            List<DDMessageString> result = billingRecService.GetReason().Select(x => new DDMessageString()
            {
                id = x.Tipo,
                value = x.Descricao
            }).ToList();

            return Json(result);
        }

        [HttpGet]
        public JsonResult GetAreas()
        {
            List<DDMessageRelated> result = billingRecService.GetAreas().Select(x => new DDMessageRelated()
            {
                id = x.Codigo,
                value = x.CodArea,
                extra = x.Destinatario
            }).ToList();

            return Json(result);
        }
        private static string ExtractAreaFromConfigId(string @configId)
        {
            int startindex = @configId.IndexOf('-');
            int endindex = @configId.IndexOf('.');
            if (endindex == -1)
                endindex = @configId.Length;

            return @configId.Substring(startindex + 1, endindex - startindex - 1);
        }
        [HttpGet]
        public JsonResult GetAreasUPUAS()
        {
            List<DDMessageRelated> result = billingRecService.GetAreasUPUAS(string.Empty).Select(x => new DDMessageRelated()
            {
                id = ExtractAreaFromConfigId(x.Codigo),
                value = x.CodArea,
                extra = x.Destinatario
            })
            .Distinct()
            .ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetDimensionsForArea([FromBody] string areaId)
        {
            List<DDMessageRelated> result = billingRecService.GetDimensionsForArea(areaId).Select(x => new DDMessageRelated()
            {
                id = x.CodCentroResponsabilidade,
                value = x.CodCentroResponsabilidade,
                extra = x.Destinatario
            })
            .Distinct()
            .ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetUsersToResend([FromBody] JObject requestParams)
        { 
            string area = string.Empty;
            bool byNumber = false;
            string respCenter = string.Empty;

            if (requestParams != null)
            {
                area = requestParams["area"].ToString();
                bool.TryParse(requestParams["byNumber"].ToString(), out byNumber);
                if(requestParams["respCenter"] != null)
                    respCenter = requestParams["respCenter"].ToString();
            }

            List<DDMessageRelated> result = null;
            if (byNumber)
            {
                result = billingRecService.GetUsersToResendByAreaNumber(area).Select(x => new DDMessageRelated()
                {
                    id = x.Destinatario,
                    value = x.Destinatario
                })
                .GroupBy(x => x.value).Select(x => x.FirstOrDefault())
                .ToList();
            }
            else
            {
                result = billingRecService.GetUsersToResendByAreaName(area, respCenter).Select(x => new DDMessageRelated()
                {
                    id = x.Destinatario,
                    value = x.Destinatario
                })
                .GroupBy(x => x.value).Select(x => x.FirstOrDefault())
                .ToList();
            }
            return Json(result);
        }

        [HttpGet]
        public JsonResult GetDestino()
        {
            List<DDMessageRelated> result = billingRecService.GetDestination().Select(x => new DDMessageRelated()
            {
                id = x.Codigo,
                value = x.CodArea,
                extra = x.Destinatario
            }).ToList();

            return Json(result);
        }

        //1
        [HttpPost]
        public async Task<JsonResult> ExportToExcel_RececaoFaturas([FromBody] List<BillingReceptionModel> Lista)
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
                ISheet excelSheet = workbook.CreateSheet("Receção de Faturas");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["id"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº");
                    Col = Col + 1;
                }
                if (dp["tipoDocumento"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Tipo de Documento");
                    Col = Col + 1;
                }
                if (dp["estado"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Estado");
                    Col = Col + 1;
                }
                if (dp["dataRececao"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data de Receção");
                    Col = Col + 1;
                }
                if (dp["codFornecedor"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Fornecedor");
                    Col = Col + 1;
                }
                if (dp["numDocFornecedor"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Núm. Doc. Fornecedor");
                    Col = Col + 1;
                }
                if (dp["dataDocFornecedor"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data Doc. Fornecedor");
                    Col = Col + 1;
                }
                if (dp["numEncomenda"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Núm. Encomenda");
                    Col = Col + 1;
                }
                if (dp["numEncomendaManual"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Núm. Encomenda Manual");
                    Col = Col + 1;
                }
                if (dp["valorEncomendaOriginal"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Valor Encomenda Original");
                    Col = Col + 1;
                }
                if (dp["quantidadeEncomenda"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Quantidade Encomenda");
                    Col = Col + 1;
                }
                if (dp["quantidadeRecebida"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Quantidade Recebida");
                    Col = Col + 1;
                }
                if (dp["valorRecebidoNaoContabilizado"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Valor Recebido não Contabilizado");
                    Col = Col + 1;
                }
                if (dp["valor"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Valor");
                    Col = Col + 1;
                }
                if (dp["codRegiao"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Região");
                    Col = Col + 1;
                }
                if (dp["codAreaFuncional"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Área Funcional");
                    Col = Col + 1;
                }
                if (dp["codCentroResponsabilidade"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Centro Responsabilidade");
                    Col = Col + 1;
                }
                if (dp["codLocalizacao"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Localização");
                    Col = Col + 1;
                }
                if (dp["local"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Local");
                    Col = Col + 1;
                }
                if (dp["numAcordoFornecedor"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Núm. Acordo Fornecedor");
                    Col = Col + 1;
                }
                if (dp["destinatario"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Destinatario");
                    Col = Col + 1;
                }
                if (dp["areaPendente"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Àrea Pendente");
                    Col = Col + 1;
                }
                if (dp["dataUltimaInteracao"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data Última Interação");
                    Col = Col + 1;
                }

                if (dp != null)
                {
                    int count = 1;
                    foreach (BillingReceptionModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["id"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Id);
                            Col = Col + 1;
                        }
                        if (dp["tipoDocumento"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.TipoDocumento.ToString());
                            Col = Col + 1;
                        }
                        if (dp["estado"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Estado.ToString());
                            Col = Col + 1;
                        }
                        if (dp["dataRececao"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.DataRececao);
                            Col = Col + 1;
                        }
                        if (dp["codFornecedor"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodFornecedor);
                            Col = Col + 1;
                        }
                        if (dp["numDocFornecedor"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.NumDocFornecedor);
                            Col = Col + 1;
                        }
                        if (dp["dataDocFornecedor"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.DataDocFornecedor);
                            Col = Col + 1;
                        }
                        if (dp["numEncomenda"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.NumEncomenda);
                            Col = Col + 1;
                        }
                        if (dp["numEncomendaManual"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.NumEncomendaManual);
                            Col = Col + 1;
                        }
                        if (dp["valorEncomendaOriginal"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ValorEncomendaOriginal.ToString());
                            Col = Col + 1;
                        }
                        if (dp["quantidadeEncomenda"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.QuantidadeEncomenda.ToString());
                            Col = Col + 1;
                        }
                        if (dp["quantidadeRecebida"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.QuantidadeRecebida.ToString());
                            Col = Col + 1;
                        }
                        if (dp["valorRecebidoNaoContabilizado"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ValorRecebidoNaoContabilizado.ToString());
                            Col = Col + 1;
                        }
                        if (dp["valor"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Valor.ToString());
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
                        if (dp["codLocalizacao"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodLocalizacao);
                            Col = Col + 1;
                        }
                        if (dp["local"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Local);
                            Col = Col + 1;
                        }
                        if (dp["numAcordoFornecedor"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.NumAcordoFornecedor);
                            Col = Col + 1;
                        }
                        if (dp["destinatario"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Destinatario);
                            Col = Col + 1;
                        }
                        if (dp["areaPendente"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.AreaPendente);
                            Col = Col + 1;
                        }
                        if (dp["dataUltimaInteracao"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.DataUltimaInteracao);
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
        public IActionResult ExportToExcelDownload_RececaoFaturas(string sFileName)
        {
            sFileName = @"/Upload/temp/" + sFileName;
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Receção de Faturas.xlsx");
        }
        [HttpPost]
        public JsonResult Existlink([FromBody] string sFileName)
        {
            string name = sFileName.Remove(0, 5);
            name=name.Replace('/', '\\');
            string link = Directory.GetCurrentDirectory()+"\\wwwroot" + name;
            if (System.IO.File.Exists(link))
            {
                return Json("1");
            }
            return Json("0");


        }

        [HttpGet]
        public FileStreamResult OpenLinkAttached(string id)
        {

            return new FileStreamResult(new FileStream(_generalConfig.FileUploadFolder + id, FileMode.Open), "application/xlsx");
        }

     
        [HttpPost]
        public JsonResult CheckIfDocumentExists([FromBody] BillingReceptionModel item)
        {
            Services.BillingReceptionService billingReceptionService = new Services.BillingReceptionService();
            bool exists = billingReceptionService.CheckIfDocumentExistsFor(item);

            ErrorHandler result = new ErrorHandler();
            if (exists)
            {
                result.eMessage = "Já foi criada receção de fatura para o documento.";
                result.eReasonCode = 2;
            }
            else
            {
                result.eMessage = "O documento pode ser rececionado.";
                result.eReasonCode = 1;
            }
            return Json(result);
        }

        //[HttpGet]
        //public JsonResult GetUserProfileById([FromBody] string user)
        //{
        //    int userProfile = (int)DBUserConfigurations.GetById(user).Rfperfil;
        //    return Json(userProfile);
        //}
        [HttpPost]
        public JsonResult SetState([FromBody] BillingReceptionModel item)
        {
            BillingRecWorkflowModel workflow = new BillingRecWorkflowModel();
          

            BillingReceptionModel updatedItem = null;
            if (item != null)
            {
                
                if (item.Estado==BillingReceptionStates.Pendente)
                {
                    item.DataResolucao = null;
                    item.AreaPendente2 = "";
                    item.Destinatario = "";
                    item.AreaPendente = "Contabilidade";
                    item.IdAreaPendente = BillingReceptionAreas.Contabilidade;
                    item.AreaUltimaInteracao = "Contabilidade";
                    item.UserUltimaInteracao = workflow.CriadoPor;
                }
                else if(item.Estado == BillingReceptionStates.Resolvido || item.Estado == BillingReceptionStates.Contabilizado || item.Estado == BillingReceptionStates.SemEfeito)
                {
                    item.AreaPendente = "";
                    item.AreaPendente2 = "";
                    item.Destinatario = "";
                    item.TipoProblema = "";
                    item.Descricao = "";
                    item.DescricaoProblema = "";
                    item.AreaUltimaInteracao = "Contabilidade";
                    item.UserUltimaInteracao = workflow.CriadoPor;
                    item.IdAreaPendente = BillingReceptionAreas.Contabilidade;
                    item.DataResolucao = DateTime.Now.ToString("dd/MM/yyyy");
                }
               
                item.ModificadoPor = User.Identity.Name;
                if(item.Estado == BillingReceptionStates.Resolvido)
                    workflow.Descricao = BillingReceptionStates.Resolvido.ToString();
                else
                   workflow.Descricao = "Alteração MANUAL para o estado " + item.Estado;


                workflow.Estado = item.Estado;
                workflow.DataCriacao = DateTime.Now;
                workflow.IdRecFaturacao = item.Id;
                item.DescricaoProblema = workflow.Descricao;
                item.WorkflowItems.Add(workflow);

                updatedItem = billingRecService.UpdateWorkFlow(item, workflow, User.Identity.Name);
                if (updatedItem != null)
                {
                    updatedItem.eReasonCode = 1;
                    updatedItem.eMessage = "Registo atualizado para "+  item.Estado + " com sucesso";
                    item = updatedItem;

                }
                else
                {
                    item.eReasonCode = 2;

                }
            }
            else
            {
                item = new BillingReceptionModel();
                item.eReasonCode = 2;
                //item.eMessage = "O registo não pode ser nulo";
                item.eMessage = "Ocorreu um Erro: O registo encontra-se vazio";
            }
            return Json(item);
        }

        #region ANEXOS

        [HttpPost]
        [Route("Faturacao/ExistFile")]
        public JsonResult ExistFile()
        {
            ErrorHandler result = new ErrorHandler();
            try
            {
                result.eReasonCode = 1;
                var files = Request.Form.Files;
                foreach (var file in files)
                {
                    string filename = Path.GetFileName(file.FileName);
                    result.eMessage = filename;
                    var path = Path.Combine(_generalConfig.FileUploadFolder, filename);
                    if (System.IO.File.Exists(path))
                    {
                        result.eReasonCode = 2;
                    }
                    else
                    {
                        using (FileStream dd = new FileStream(path, FileMode.CreateNew))
                        {
                            file.CopyTo(dd);
                            dd.Dispose();
                        }
                    }
                } 
            }
            catch (Exception ex)
            {
                result.eReasonCode = 2;
                result.eMessage = ex.Message;
            }
            return Json(result);
        }

        public JsonResult UploadFile(BillingRecWorkflowModel workflow)
        {
            if (workflow.Attached != null)
            {
                try
                {

                    foreach (var file in workflow.Attached)
                    {
                        var path = Path.Combine(_generalConfig.FileUploadFolder, file.File);
                        if (!System.IO.File.Exists(path))
                        {
                            try
                            {
                                using (FileStream dd = new FileStream(path, FileMode.CreateNew))
                                {

                                    dd.Dispose();
                                }
                            }
                            catch (Exception ex)
                            {
                                throw;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return Json("");
        }



        #endregion

    }
}