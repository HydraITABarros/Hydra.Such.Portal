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

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class FaturacaoController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private BillingReceptionService billingRecService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public FaturacaoController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IHostingEnvironment _hostingEnvironment)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            billingRecService = new BillingReceptionService();
            this._hostingEnvironment = _hostingEnvironment;
        }

        public IActionResult RececaoFaturas()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.ReceçãoFaturação);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UserPermissions = UPerm;
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
                ViewBag.Id = id;
                ViewBag.UserPermissions = UPerm;
                ViewBag.BillingReceptionStates = EnumHelper.GetItemsAsDictionary(typeof(BillingReceptionStates));
                ViewBag.RFPerfil = userConfig.RFPerfil;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
      

        public JsonResult GetBillingReceptions()
        {
            var billingReceptions = billingRecService.GetAllForUser(User.Identity.Name);
            return Json(billingReceptions);
        }
        public JsonResult GetBillingReceptionsHistory()
        {

            UserConfigurationsViewModel userConfig = DBUserConfigurations.GetById(User.Identity.Name).ParseToViewModel();
            BillingReceptionAreas areaPendente= userConfig.RFPerfil ?? BillingReceptionAreas.Aprovisionamento;
            var billingReceptions = billingRecService.GetAllForUserHistPending(User.Identity.Name,0, areaPendente);
            return Json(billingReceptions);
        }
        public JsonResult GetBillingReceptionsPending()
        {

            UserConfigurationsViewModel userConfig = DBUserConfigurations.GetById(User.Identity.Name).ParseToViewModel();
            BillingReceptionAreas areaPendente = userConfig.RFPerfil ?? BillingReceptionAreas.Aprovisionamento;
            var billingReceptions = billingRecService.GetAllForUserHistPending(User.Identity.Name, 1, areaPendente);
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
                    createdItem.eReasonCode = 1;
                    createdItem.eMessage = "Registo criado com sucesso";
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
                item.eMessage = "O registo não pode ser nulo";
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
                updatedItem.eMessage = "O registo não pode ser nulo";
            }
            return Json(updatedItem);
        }

        [HttpPost]
        public JsonResult ValidateNumeration([FromBody] BillingReceptionModel data)
        {
            //Get Project Numeration
            int Cfg = (int)DBUserConfigurations.GetById(User.Identity.Name).PerfilNumeraçãoRecDocCompras;

            ConfiguraçãoNumerações CfgNumeration = DBNumerationConfigurations.GetById(Cfg);

            //Validate if ProjectNo is valid
            if (!(data.Id == "" || data.Id == null) && !CfgNumeration.Manual.Value)
            {
                return Json("A numeração configurada para contratos não permite inserção manual.");
            }
            else if (data.Id == "" && !CfgNumeration.Automático.Value)
            {
                return Json("É obrigatório inserir o Nº de Contrato.");
            }

            return Json("");
        }

        [HttpPost]
        public JsonResult SendBillingReception([FromBody] BillingReceptionModel item)
        {

            BillingReceptionModel updatedItem = null;
            if (item != null)
            {
                item.ModificadoPor = User.Identity.Name;
                BillingRecWorkflowModel workflow = item.WorkflowItems.LastOrDefault();                
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
                updatedItem.eMessage = "O registo não pode ser nulo";
            }
            return Json(updatedItem);
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
                item.eMessage = "O registo não pode ser nulo";
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
                postedDocument = billingRecService.PostDocument(item, User.Identity.Name, _config, _configws);
                item = postedDocument;            
            }
            else
            {
                item = new BillingReceptionModel();
                item.eReasonCode = 2;
                item.eMessage = "O registo não pode ser nulo";
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
                item.eMessage = "O registo não pode ser nulo";
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
                item.eMessage = "O registo não pode ser nulo";
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
                item.eMessage = "O registo não pode ser nulo";
            }
            return Json(false);
        }

        [HttpGet]
        public JsonResult GetProblems()
        {

            List<DDMessageRelated> result = billingRecService.GetProblem().Select(x => new DDMessageRelated()
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

            if (data.AreaPendente2 == "Aprovisionamento")
            {
                if (userPendingProfile == 1 && userDestinyProfile == 0)
                {
                    AnswerType = "RF1R";
                    result = billingRecService.GetProblemAnswer(AnswerType).ToList();
                } 
            }

            if(data.AreaPendente2 == "UnidadesProdutivas" || data.AreaPendente2 == "UnidadesProdutivas")
            {
                if ((userPendingProfile == 2 || userPendingProfile == 3) && userDestinyProfile == 1)
                {
                    AnswerType = "RF5R";
                    result = billingRecService.GetProblemAnswer(AnswerType).ToList();
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
        public async Task<JsonResult> ExportToExcel_RececaoFaturas([FromBody] List<BillingReceptionModel> dp)
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
                ISheet excelSheet = workbook.CreateSheet("Receção de Faturas");
                IRow row = excelSheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("Nº");
                row.CreateCell(1).SetCellValue("Tipo de Documento");
                row.CreateCell(2).SetCellValue("Estado");
                row.CreateCell(3).SetCellValue("Data de Receção");
                row.CreateCell(4).SetCellValue("Fornecedor");
                row.CreateCell(5).SetCellValue("Núm. Doc. Fornecedor");
                row.CreateCell(6).SetCellValue("Data Doc. Fornecedor");
                row.CreateCell(7).SetCellValue("Núm. Encomenda");
                row.CreateCell(8).SetCellValue("Núm. Encomenda Manual");
                row.CreateCell(9).SetCellValue("Valor Encomenda Original");
                row.CreateCell(10).SetCellValue("Quantidade Encomenda");
                row.CreateCell(11).SetCellValue("Quantidade Recebida");
                row.CreateCell(12).SetCellValue("Valor Recebido não Contabilizado");
                row.CreateCell(13).SetCellValue("Valor");
                row.CreateCell(14).SetCellValue("Cód. Região");
                row.CreateCell(15).SetCellValue("Cód. Área Funcional");
                row.CreateCell(16).SetCellValue("Cód. Centro Responsabilidade");
                row.CreateCell(17).SetCellValue("Cód. Localização");
                row.CreateCell(18).SetCellValue("Local");
                row.CreateCell(19).SetCellValue("Núm. Acordo Fornecedor");
                row.CreateCell(20).SetCellValue("Destinatario");
                row.CreateCell(21).SetCellValue("Àrea Pendente");
                row.CreateCell(22).SetCellValue("Data Última Interação");

                if (dp != null)
                {
                    int count = 1;
                    foreach (BillingReceptionModel item in dp)
                    {
                        row = excelSheet.CreateRow(count);
                        row.CreateCell(0).SetCellValue(item.Id);
                        row.CreateCell(1).SetCellValue(item.TipoDocumento.ToString());
                        row.CreateCell(2).SetCellValue(item.Estado.ToString());
                        row.CreateCell(3).SetCellValue(item.DataRececao);
                        row.CreateCell(4).SetCellValue(item.CodFornecedor);
                        row.CreateCell(5).SetCellValue(item.NumDocFornecedor);
                        row.CreateCell(6).SetCellValue(item.DataDocFornecedor);
                        row.CreateCell(7).SetCellValue(item.NumEncomenda);
                        row.CreateCell(8).SetCellValue(item.NumEncomendaManual);
                        row.CreateCell(9).SetCellValue(item.ValorEncomendaOriginal.ToString());
                        row.CreateCell(10).SetCellValue(item.QuantidadeEncomenda.ToString());
                        row.CreateCell(11).SetCellValue(item.QuantidadeRecebida.ToString());
                        row.CreateCell(12).SetCellValue(item.ValorRecebidoNaoContabilizado.ToString());
                        row.CreateCell(13).SetCellValue(item.Valor.ToString());
                        row.CreateCell(14).SetCellValue(item.CodRegiao);
                        row.CreateCell(15).SetCellValue(item.CodAreaFuncional);
                        row.CreateCell(16).SetCellValue(item.CodCentroResponsabilidade);
                        row.CreateCell(17).SetCellValue(item.CodLocalizacao);
                        row.CreateCell(18).SetCellValue(item.Local);
                        row.CreateCell(19).SetCellValue(item.NumAcordoFornecedor);
                        row.CreateCell(20).SetCellValue(item.Destinatario);
                        row.CreateCell(21).SetCellValue(item.AreaPendente);
                        row.CreateCell(22).SetCellValue(item.DataUltimaInteracao);
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
            BillingRecWorkflowModel workflow = item.WorkflowItems.LastOrDefault();
            item.WorkflowItems.RemoveAt(item.WorkflowItems.Count - 1);

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
                }
                else if(item.Estado == BillingReceptionStates.Resolvido || item.Estado == BillingReceptionStates.Contabilizado || item.Estado == BillingReceptionStates.SemEfeito)
                {
                    item.AreaPendente = "";
                    item.AreaPendente2 = "";
                    item.Destinatario = "";
                    item.TipoProblema = "";
                    item.Descricao = "";
                    item.DescricaoProblema = "";
                    item.AreaPendente = "Contabilidade";
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
                item.eMessage = "O registo não pode ser nulo";
            }
            return Json(item);

        }
    }
}