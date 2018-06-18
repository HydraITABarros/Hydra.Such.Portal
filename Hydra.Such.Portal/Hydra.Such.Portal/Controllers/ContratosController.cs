using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.ViewModel.Contracts;
using Hydra.Such.Data.Logic.Contracts;
using Hydra.Such.Data.Database;
using Microsoft.Extensions.Options;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.NAV;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.ViewModel;
using Newtonsoft.Json.Linq;
using Hydra.Such.Portal.Services;
using static Hydra.Such.Data.Enumerations;
using Hydra.Such.Data.Logic.Project;
using Hydra.Such.Data;
using Newtonsoft.Json;
using Hydra.Such.Data.ViewModel.Projects;
using Hydra.Such.Data.ViewModel.Clients;

namespace Hydra.Such.Portal.Controllers
{
    public class ContratosController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;

        public ContratosController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
        }

        #region Contratos
        public IActionResult Index(int? archived, string contractNo)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Contratos);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.Archived = archived == null ? 0 : 1;
                ViewBag.ContractNo = contractNo ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult Details(string id, string version)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.ParseToViewModel(DBUserAccesses.GetByUserId(User.Identity.Name).Where(x => x.Área == 1 && x.Funcionalidade == 2).FirstOrDefault());
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.ContractNo = id ?? "";
                ViewBag.VersionNo = version ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }



        }

        public IActionResult DetalhesContrato(string id, string version = "")
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Contratos);
            if (UPerm != null && UPerm.Read.Value)
            {
                Contratos cContract = null;
                if (version != "")
                    cContract = DBContracts.GetByIdAndVersion(id, int.Parse(version));
                else
                    cContract = DBContracts.GetByIdLastVersion(id);

                //if (cContract != null && (cContract.Arquivado == true || cContract.EstadoAlteração == 2))
                if (cContract != null && cContract.Arquivado == true)
                {
                    UPerm.Update = false;
                }

                ViewBag.ContractNo = id ?? "";
                ViewBag.VersionNo = version ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult AvencaFixa()
        {
            return View();
        }

        public JsonResult GetListContractsByArea([FromBody] JObject requestParams)
        {
            int Archived = int.Parse(requestParams["Archived"].ToString());
            string ContractNo = requestParams["ContractNo"].ToString();
            int Historic = 0;
            if (requestParams["Historic"] != null)
                Historic = int.Parse(requestParams["Historic"].ToString());

            List<Contratos> ContractsList = null;

            if ((Archived == 0 || ContractNo == "") && (Historic == 0))
            {
                ContractsList = DBContracts.GetAllByContractType(ContractType.Contract);
                ContractsList.RemoveAll(x => x.Arquivado.HasValue && x.Arquivado.Value);
            }
            else if (Historic == 1)
            {
                ContractsList = DBContracts.GetAllHistoric((int)ContractType.Contract);
            }
            else
            {
                ContractsList = DBContracts.GetByNo(ContractNo, true);
            }

            //Apply User Dimensions Validations
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //Regions
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.Region).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.CódigoRegião));
            //FunctionalAreas
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.CódigoÁreaFuncional));
            //ResponsabilityCenter
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CódigoCentroResponsabilidade));


            List<ContractViewModel> result = new List<ContractViewModel>();

            ContractsList.ForEach(x => result.Add(DBContracts.ParseToViewModel(x, _config.NAVDatabaseName, _config.NAVCompanyName)));

            List<EnumData> status = EnumerablesFixed.ContractStatus;
            result.ForEach(x => { x.StatusDescription = status.Where(y => y.Id == x.Status).Select(y => y.Value).FirstOrDefault(); });

            return Json(result);
        }

        [HttpPost]
        public JsonResult ValidateNumeration([FromBody] ContractViewModel data)
        {
            //Get Project Numeration
            Configuração Cfg = DBConfigurations.GetById(1);
            int ProjectNumerationConfigurationId = 0;

            switch (data.ContractType)
            {
                case 1:
                    ProjectNumerationConfigurationId = Cfg.NumeraçãoOportunidades.Value;
                    break;
                case 2:
                    ProjectNumerationConfigurationId = Cfg.NumeraçãoPropostas.Value;
                    break;
                case 3:
                    ProjectNumerationConfigurationId = Cfg.NumeraçãoContratos.Value;
                    break;
                default:
                    break;
            }

            ConfiguraçãoNumerações CfgNumeration = DBNumerationConfigurations.GetById(ProjectNumerationConfigurationId);

            //Validate if ProjectNo is valid
            if (!(data.ContractNo == "" || data.ContractNo == null) && !CfgNumeration.Manual.Value)
            {
                return Json("A numeração configurada para contratos não permite inserção manual.");
            }
            else if (data.ContractNo == "" && !CfgNumeration.Automático.Value)
            {
                return Json("É obrigatório inserir o Nº de Contrato.");
            }

            return Json("");
        }

        [HttpPost]
        public JsonResult GetContractDetails([FromBody] ContractViewModel data)
        {
            if (data != null)
            {
                Contratos cContract = null;
                if (data.VersionNo != 0)
                {
                    cContract = DBContracts.GetByIdAndVersion(data.ContractNo, data.VersionNo);
                }
                else
                {
                    cContract = DBContracts.GetByIdLastVersion(data.ContractNo);
                }

                ContractViewModel result = new ContractViewModel();


                if (cContract != null)
                {
                    result = DBContracts.ParseToViewModel(cContract, _config.NAVDatabaseName, _config.NAVCompanyName);


                    //GET CLIENT REQUISITIONS
                    List<RequisiçõesClienteContrato> ClientRequisition = DBContractClientRequisition.GetByContract(result.ContractNo);
                    result.ClientRequisitions = new List<ContractClientRequisitionViewModel>();

                    if (ClientRequisition != null)
                    {
                        ClientRequisition.ForEach(x => result.ClientRequisitions.Add(DBContractClientRequisition.ParseToViewModel(x)));
                    }

                    //GET INVOICE TEXTS
                    List<TextoFaturaContrato> InvoicesTexts = DBContractInvoiceText.GetByContract(result.ContractNo);
                    result.InvoiceTexts = new List<ContractInvoiceTextViewModel>();

                    if (InvoicesTexts != null)
                    {
                        InvoicesTexts.ForEach(x => result.InvoiceTexts.Add(DBContractInvoiceText.ParseToViewModel(x)));
                    }
                }
                else
                {
                    if (data.ContractType == 1)
                        result.Status = 1;
                    result.ClientRequisitions = new List<ContractClientRequisitionViewModel>();
                    result.InvoiceTexts = new List<ContractInvoiceTextViewModel>();
                }

                return Json(result);
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult CreateContract([FromBody] ContractViewModel data)
        {
            try
            {
                if (data != null)
                {
                    //Get Contract Numeration
                    bool autoGenId = false;
                    Configuração Configs = DBConfigurations.GetById(1);
                    int ProjectNumerationConfigurationId = 0;

                    switch (data.ContractType)
                    {
                        case 1:
                            ProjectNumerationConfigurationId = Configs.NumeraçãoOportunidades.Value;
                            break;
                        case 2:
                            ProjectNumerationConfigurationId = Configs.NumeraçãoPropostas.Value;
                            break;
                        case 3:
                            ProjectNumerationConfigurationId = Configs.NumeraçãoContratos.Value;
                            break;
                        default:
                            break;
                    }
                    if (data.ContractNo == "" || data.ContractNo == null)
                    {
                        autoGenId = true;
                        data.ContractNo = DBNumerationConfigurations.GetNextNumeration(ProjectNumerationConfigurationId, autoGenId);
                    }

                    if (data.ContractNo != null)
                    {
                        data.Filed = false;
                        data.History = false;
                        Contratos cContract = DBContracts.ParseToDB(data);
                        cContract.TipoContrato = data.ContractType;
                        cContract.UtilizadorCriação = User.Identity.Name;
                        //Create Contract On Database
                        cContract = DBContracts.Create(cContract);

                        if (cContract == null)
                        {
                            data.eReasonCode = 3;
                            data.eMessage = "Ocorreu um erro ao criar o contrato.";
                        }
                        else
                        {
                            //Create Client Contract Requisitions
                            data.ClientRequisitions.ForEach(r =>
                            {
                                r.ContractNo = cContract.NºDeContrato;
                                r.CreateUser = User.Identity.Name;
                                DBContractClientRequisition.Create(DBContractClientRequisition.ParseToDB(r));
                            });

                            //Create Contract Invoice Texts
                            data.InvoiceTexts.ForEach(r =>
                            {
                                r.ContractNo = cContract.NºDeContrato;
                                r.CreateUser = User.Identity.Name;
                                DBContractInvoiceText.Create(DBContractInvoiceText.ParseToDB(r));
                            });
                            if (autoGenId)
                            {
                                //Update Last Numeration Used
                                ConfiguraçãoNumerações ConfigNumerations = DBNumerationConfigurations.GetById(ProjectNumerationConfigurationId);
                                ConfigNumerations.ÚltimoNºUsado = data.ContractNo;
                                ConfigNumerations.UtilizadorModificação = User.Identity.Name;
                                DBNumerationConfigurations.Update(ConfigNumerations);
                            }
                            data.eReasonCode = 1;
                        }
                    }
                    else
                    {
                        data.eReasonCode = 5;
                        data.eMessage = "A numeração configurada não é compativel com a inserida.";
                    }
                }
            }
            catch (Exception ex)
            {
                data.eReasonCode = 4;
                data.eMessage = "Ocorreu um erro ao criar o contrato";
            }
            return Json(data);

        }

        [HttpPost]
        public JsonResult UpdateContract([FromBody] ContractViewModel data)
        {
            try
            {
                if (data != null)
                {
                    if (data.ContractNo != null)
                    {
                        //Contratos cContract = DBContracts.ParseToDB(data);
                        Contratos ContratoDB = DBContracts.GetByIdAndVersion(data.ContractNo, data.VersionNo);


                        if (ContratoDB != null)
                        {
                            ContratoDB = DBContracts.ParseToDB(data);
                            ContratoDB = DBContracts.Update(ContratoDB);

                            //Create/Update Contract Client Requests
                            List<RequisiçõesClienteContrato> RCC =
                                DBContractClientRequisition.GetByContract(ContratoDB.NºContrato);
                            List<RequisiçõesClienteContrato> RCCToDelete = RCC
                                .Where(x => !data.ClientRequisitions.Any(
                                    y => x.NºRequisiçãoCliente == x.NºRequisiçãoCliente &&
                                         x.GrupoFatura == y.InvoiceGroup && x.NºProjeto == y.ProjectNo &&
                                         x.DataInícioCompromisso == DateTime.Parse(y.StartDate))).ToList();

                            data.ClientRequisitions.ForEach(y =>
                            {
                                RequisiçõesClienteContrato RCCO =
                                    RCC.Where(x => x.NºRequisiçãoCliente == x.NºRequisiçãoCliente &&
                                                   x.GrupoFatura == y.InvoiceGroup && x.NºProjeto == y.ProjectNo &&
                                                   x.DataInícioCompromisso == DateTime.Parse(y.StartDate))
                                        .FirstOrDefault();
                                if (RCCO != null)
                                {
                                    RCCO.NºContrato = y.ContractNo;
                                    RCCO.GrupoFatura = y.InvoiceGroup;
                                    RCCO.NºProjeto = y.ProjectNo;
                                    RCCO.DataInícioCompromisso = DateTime.Parse(y.StartDate);
                                    RCCO.DataFimCompromisso = y.EndDate != "" ? DateTime.Parse(y.EndDate) : (DateTime?)null;
                                    RCCO.NºRequisiçãoCliente = y.ClientRequisitionNo;
                                    RCCO.DataRequisição = y.RequisitionDate != "" ? DateTime.Parse(y.RequisitionDate) : (DateTime?)null;
                                    RCCO.NºCompromisso = y.PromiseNo;
                                    RCCO.DataÚltimaFatura = y.LastInvoiceDate != "" ? DateTime.Parse(y.LastInvoiceDate) : (DateTime?)null;
                                    RCCO.NºFatura = y.InvoiceNo;
                                    RCCO.ValorFatura = y.InvoiceValue;
                                    RCCO.UtilizadorModificação = User.Identity.Name;
                                    DBContractClientRequisition.Update(RCCO);
                                }
                                else
                                {
                                    y.CreateUser = User.Identity.Name;
                                    DBContractClientRequisition.Create(DBContractClientRequisition.ParseToDB(y));
                                }
                            });

                            //Delete Contract Client Requests
                            RCCToDelete.ForEach(x => DBContractClientRequisition.Delete(x));

                            //Create/Update Contract Invoice Texts
                            List<TextoFaturaContrato> CIT = DBContractInvoiceText.GetByContract(ContratoDB.NºContrato);
                            List<TextoFaturaContrato> CITToDelete =
                                CIT.Where(x => !data.InvoiceTexts.Any(
                                    y => x.GrupoFatura == y.InvoiceGroup && x.NºProjeto == y.ProjectNo &&
                                         x.NºContrato == y.ContractNo)).ToList();

                            data.InvoiceTexts.ForEach(y =>
                            {
                                TextoFaturaContrato CITO = CIT
                                    .Where(x => x.GrupoFatura == y.InvoiceGroup && x.NºProjeto == y.ProjectNo &&
                                                x.NºContrato == y.ContractNo).FirstOrDefault();
                                if (CITO != null)
                                {
                                    CITO.NºContrato = y.ContractNo;
                                    CITO.GrupoFatura = y.InvoiceGroup;
                                    CITO.NºProjeto = y.ProjectNo;
                                    CITO.TextoFatura = y.InvoiceText;
                                    CITO.UtilizadorModificação = User.Identity.Name;
                                    DBContractInvoiceText.Update(CITO);
                                }
                                else
                                {
                                    y.CreateUser = User.Identity.Name;
                                    DBContractInvoiceText.Create(DBContractInvoiceText.ParseToDB(y));
                                }
                            });

                            //Delete Contract Invoice Texts
                            CITToDelete.ForEach(x => DBContractInvoiceText.Delete(x));
                        }
                        data.eReasonCode = 1;
                        data.eMessage = "Contrato atualizado com sucesso.";
                    }
                }
            }
            catch (Exception ex)
            {
                data.eReasonCode = 2;
                data.eMessage = "Ocorreu um erro ao atualizar o contrato.";
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult DeleteContract([FromBody] ContractViewModel data)
        {
            ErrorHandler result = new ErrorHandler();
            try
            {
                if (data != null)
                {
                    //Verify if contract have Invoices Or Projects
                    bool haveContracts = DBContracts.GetAllByContractNo(data.ContractNo).Count > 0;
                    bool haveInvoices = DBContractInvoices.GetByContractNo(data.ContractNo).Count > 0;

                    if (haveContracts || haveInvoices)
                    {
                        result.eReasonCode = 2;
                        result.eMessage = "Não é possivel remover o contrato pois possui faturas e/ou projetos associados.";
                    }
                    else
                    {
                        // Delete Contract Lines
                        DBContractLines.DeleteAllFromContract(data.ContractNo);

                        // Delete Contract Invoice Texts
                        DBContractInvoiceText.DeleteAllFromContract(data.ContractNo);

                        // Delete Contract Client Requisitions
                        DBContractClientRequisition.DeleteAllFromContract(data.ContractNo);

                        // Delete Contract 
                        DBContracts.DeleteByContractNo(data.ContractNo);


                        result.eReasonCode = 1;
                        result.eMessage = "Contrato eliminado com sucesso.";
                    }

                }
            }
            catch (Exception ex)
            {
                result.eReasonCode = 2;
                result.eMessage = "Ocorreu um erro ao eliminar o contrato.";
            }
            return Json(result);

        }

        [HttpPost]
        public JsonResult ArchiveContract([FromBody] ContractViewModel data)
        {

            if (data != null)
            {
                ContractsService serv = new ContractsService(User.Identity.Name);
                data = serv.ArchiveContract(data);
            }
            else
            {
                data.eReasonCode = 2;
                data.eMessage = "Ocorreu um erro ao arquivar.";
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult SendContractToHistory([FromBody] ContractViewModel data)
        {

            if (data != null)
            {
                ContractsService serv = new ContractsService(User.Identity.Name);
                data = serv.ArchiveContract(data);
                Contratos cContract = DBContracts.GetByIdAndVersion(data.ContractNo, data.VersionNo);

                if (cContract != null)
                {
                    try
                    {
                        //Create new contract and update old
                        cContract.UtilizadorModificação = User.Identity.Name;
                        cContract.Arquivado = true;
                        cContract.Historico = true;
                        DBContracts.Update(cContract);

                        data.eReasonCode = 1;
                        data.eMessage = "Arquivado com sucesso.";
                        return Json(data);
                    }
                    catch (Exception)
                    {
                        data.eReasonCode = 2;
                        data.eMessage = "Ocorreu um erro ao arquivar.";
                    }
                }
            }
            else
            {
                data.eReasonCode = 2;
                data.eMessage = "Ocorreu um erro ao arquivar.";
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult UpdateProposalContract([FromBody] JObject requestParams)
        {
            ContractViewModel proposal = null;
            bool partialUpdate = false;
            if (requestParams != null)
            {
                proposal = requestParams["proposal"].ToObject<ContractViewModel>();
                partialUpdate = requestParams["partialUpdate"].ToObject<bool>();
            }

            if (proposal != null)
            {
                Contratos contract = DBContracts.GetByIdLastVersion(proposal.RelatedContract);

                if (contract != null)
                {
                    try
                    {
                        /*
                        •	Se o estado da proposta estiver aberta, perguntar se quer mudar para Enviada ou para Revista (tem de mudar para um destes estados obrigatoriamente);
                        •	Arquiva a proposta;
                        •	Pergunta que linhas vai passar para o contrato, tal como faz no tornar em contrato parcial;
                        •	Atualiza o contrato com os dados da proposta (as linhas do contrato atuais são eliminadas e criadas novamente com as linhas da proposta, passa apenas as linhas marcadas como sendo para passar);
                        •	Muda o estado da proposta para Renovada se for total, ou para Parcialmente Aceite se for parcial (se selecionou todas as linhas ou não);
                        */

                        Contratos proposalToUpdate = DBContracts.GetByIdAndVersion(proposal.ContractNo, proposal.VersionNo);
                        proposalToUpdate.Estado = proposal.Status;
                        proposalToUpdate.DataExpiração = DateTime.Parse(proposal.DueDate);
                        proposalToUpdate.UtilizadorModificação = User.Identity.Name;
                        proposalToUpdate.Arquivado = true;
                        proposalToUpdate = DBContracts.Update(proposalToUpdate);
                        if (proposalToUpdate != null)
                        {
                            proposalToUpdate.NºVersão = proposal.VersionNo + 1;
                            proposalToUpdate.UtilizadorCriação = User.Identity.Name;
                            proposalToUpdate.UtilizadorModificação = "";

                            proposalToUpdate.DataHoraModificação = null;
                            proposalToUpdate.Arquivado = false;
                            DBContracts.Create(proposalToUpdate);

                            //Duplicate proposal Lines
                            List<LinhasContratos> proposalLines = DBContractLines.GetAllByActiveContract(proposal.ContractNo, proposal.VersionNo);
                            proposalLines.ForEach(x =>
                            {
                                x.NºVersão = proposalToUpdate.NºVersão;
                                DBContractLines.Create(x);
                            });

                            if (DBContractLines.DeleteAllFromContract(proposal.RelatedContract))
                            {
                                if (partialUpdate)
                                {
                                    proposalLines.RemoveAll(x => !x.CriaContrato.HasValue || !x.CriaContrato.Value);
                                    proposalToUpdate.Estado = 8;
                                }
                                else
                                {
                                    proposalToUpdate.Estado = 6;
                                }
                                DBContracts.Update(proposalToUpdate);
                                proposalLines.ForEach(x =>
                                {
                                    LinhasContratos newline = ParseToNewModel(x);
                                    newline.TipoContrato = contract.TipoContrato;
                                    newline.NºContrato = contract.NºContrato;
                                    newline.NºVersão = contract.NºVersão;
                                    DBContractLines.Create(newline);
                                });

                                proposal = DBContracts.ParseToViewModel(proposalToUpdate, _config.NAVDatabaseName, _config.NAVCompanyName);

                                proposal.eReasonCode = 1;
                                proposal.eMessage = "Contrato atualizado com sucesso.";
                                return Json(proposal);
                            }
                            else
                            {
                                proposal.eReasonCode = 2;
                                proposal.eMessage = "Ocorreu um erro ao atualizar as linhas do contrato.";
                            }
                        }
                        else
                        {
                            proposal.eReasonCode = 2;
                            proposal.eMessage = "Ocorreu um erro ao atualizar o contrato.";
                        }

                        return Json(proposal);
                    }
                    catch (Exception)
                    {
                        proposal.eReasonCode = 2;
                        proposal.eMessage = "Ocorreu um erro ao atualizar o contrato.";
                    }
                }
            }
            else
            {
                proposal.eReasonCode = 2;
                proposal.eMessage = "Ocorreu um erro ao atualizar o contrato.";
            }
            return Json(proposal);
        }

        [HttpPost]
        public JsonResult GetContractNavData([FromBody] ContractViewModel data)
        {
            if (data != null)
            {
                NAVContractDetailsViewModel result = DBNAV2017ContractDetails.GetContractByNo(data.ContractNo, _config.NAVDatabaseName, _config.NAVCompanyName);

                return Json(result);
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult GetContractLines([FromBody] ContractViewModel data)
        {
            if (data != null)
            {
                List<LinhasContratos> ContractLines = DBContractLines.GetAllByActiveContract(data.ContractNo, data.VersionNo);

                ContractLineHelperViewModel result = new ContractLineHelperViewModel
                {
                    ContractNo = data.ContractNo,
                    VersionNo = data.VersionNo,
                    Lines = new List<ContractLineViewModel>()
                };

                if (ContractLines != null)
                {
                    ContractLines.ForEach(x => result.Lines.Add(DBContractLines.ParseToViewModel(x)));
                }
                return Json(result);
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult UpdateContractLines([FromBody] ContractLineHelperViewModel data)
        {
            try
            {
                if (data != null)
                {
                    List<LinhasContratos> ContractLines = DBContractLines.GetAllByActiveContract(data.ContractNo, data.VersionNo);
                    List<LinhasContratos> CLToDelete = ContractLines.Where(y => !data.Lines.Any(x => x.ContractType == y.TipoContrato && x.ContractNo == y.NºContrato && x.VersionNo == y.NºVersão && x.LineNo == y.NºLinha)).ToList();

                    CLToDelete.ForEach(x => DBContractLines.Delete(x));

                    data.Lines.ForEach(x =>
                    {
                        LinhasContratos CLine = ContractLines.Where(y => x.ContractType == y.TipoContrato && x.ContractNo == y.NºContrato && x.VersionNo == y.NºVersão && x.LineNo == y.NºLinha).FirstOrDefault();

                        if (CLine != null)
                        {
                            CLine.TipoContrato = x.ContractType;
                            CLine.NºContrato = x.ContractNo;
                            CLine.NºVersão = x.VersionNo;
                            CLine.NºLinha = x.LineNo;
                            CLine.Tipo = x.Type;
                            CLine.Código = x.Code;
                            CLine.Descrição = x.Description;
                            CLine.Quantidade = x.Quantity;
                            CLine.CódUnidadeMedida = x.CodeMeasureUnit;
                            CLine.PreçoUnitário = x.UnitPrice;
                            CLine.DescontoLinha = x.LineDiscount;
                            CLine.Faturável = x.Billable;
                            CLine.CódigoRegião = x.CodeRegion;
                            CLine.CódigoÁreaFuncional = x.CodeFunctionalArea;
                            CLine.CódigoCentroResponsabilidade = x.CodeResponsabilityCenter;
                            CLine.Periodicidade = x.Frequency;
                            CLine.NºHorasIntervenção = x.InterventionHours;
                            CLine.NºTécnicos = x.TotalTechinicians;
                            CLine.TipoProposta = x.ProposalType;
                            CLine.DataInícioVersão = x.VersionStartDate != null && x.VersionStartDate != "" ? DateTime.Parse(x.VersionStartDate) : (DateTime?)null;
                            CLine.DataFimVersão = x.VersionEndDate != null && x.VersionStartDate != "" ? DateTime.Parse(x.VersionEndDate) : (DateTime?)null;
                            CLine.NºResponsável = x.ResponsibleNo;
                            CLine.CódServiçoCliente = x.ServiceClientNo;
                            CLine.GrupoFatura = x.InvoiceGroup;
                            CLine.CriaContrato = x.CreateContract;
                            CLine.UtilizadorModificação = User.Identity.Name;
                            DBContractLines.Update(CLine);
                        }
                        else
                        {
                            x = DBContractLines.ParseToViewModel(DBContractLines.Create(DBContractLines.ParseToDB(x)));
                        }
                    });

                    data.eReasonCode = 1;
                    data.eMessage = "Linhas de contrato atualizadas com sucesso.";
                }
            }
            catch (Exception ex)
            {
                data.eReasonCode = 2;
                data.eMessage = "Ocorreu um erro ao atualizar as linhas de contrato.";
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult UpdateContractPrices([FromBody] UpdateContractPricesRequest updatePricesRequest)
        {
            ContractViewModel updatedContract;
            try
            {
                ContractsService serv = new ContractsService(User.Identity.Name);
                updatedContract = serv.UpdatePrices(updatePricesRequest);
            }
            catch
            {
                updatedContract = new ContractViewModel
                {
                    eReasonCode = 2,
                    eMessage = "Ocorreu um erro ao criar a proposta"
                };
            }
            return Json(updatedContract);
        }

        [HttpPost]
        public JsonResult RemoveFromHistoric([FromBody] ContractViewModel data)
        {
            try
            {
                if (data != null)
                {
                    if (data.ContractNo != null)
                    {
                        //Contratos cContract = DBContracts.ParseToDB(data);
                        Contratos ContratoDB = DBContracts.GetByIdAndVersion(data.ContractNo, data.VersionNo);


                        if (ContratoDB != null)
                        {
                            ContratoDB.Historico = false;
                            ContratoDB.Arquivado = false;
                            ContratoDB = DBContracts.Update(ContratoDB);
                        }
                        data.eReasonCode = 1;
                        data.eMessage = "Contrato atualizado com sucesso.";
                    }
                }
            }
            catch (Exception ex)
            {
                data.eReasonCode = 2;
                data.eMessage = "Ocorreu um erro ao atualizar o contrato.";
            }
            return Json(data);
        }


        public IActionResult CreateProjectContract(string id, string versionNo = "")
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Projetos);
            if (UPerm != null && UPerm.Read.Value)
            {
                Contratos ContratoDB = DBContracts.GetByIdAndVersion(id, int.Parse(versionNo));

                ViewBag.UPermissions = UPerm;
                ViewBag.CodeRegion = ContratoDB.CódigoRegião;
                ViewBag.FuncArea = ContratoDB.CódigoÁreaFuncional;
                ViewBag.RespCode = ContratoDB.CódigoCentroResponsabilidade;
                ViewBag.CodClient = ContratoDB.NºCliente;
                ViewBag.ContractNo = id;

                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        #endregion

        #region Oportunidades

        public IActionResult Oportunidades(int? archived, string contractNo)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Oportunidades);

            if (UPerm != null && UPerm.Read.Value)
            {

                ViewBag.Archived = archived == null ? 0 : 1;
                ViewBag.ContractNo = contractNo ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult DetalhesOportunidade(string id, string version = "")
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Oportunidades);
            if (UPerm != null && UPerm.Read.Value)
            {
                Contratos cContract = null;
                if (version != "")
                    cContract = DBContracts.GetByIdAndVersion(id, int.Parse(version));
                else
                    cContract = DBContracts.GetByIdLastVersion(id);

                if (cContract != null && cContract.Arquivado == true)
                {
                    UPerm.Update = false;
                }

                ViewBag.ContractNo = id ?? "";
                ViewBag.VersionNo = version ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public JsonResult GetListOportunities([FromBody] JObject requestParams)
        {
            int Archived = int.Parse(requestParams["Archived"].ToString());
            string ContractNo = requestParams["ContractNo"].ToString();

            List<Contratos> ContractsList = null;

            if (Archived == 0)
            {
                ContractsList = DBContracts.GetAllByContractType(ContractType.Oportunity);
                ContractsList.RemoveAll(x => x.Arquivado.HasValue && x.Arquivado.Value);
            }
            else
            {
                ContractsList = DBContracts.GetAllByContractType(ContractType.Oportunity);
            }
            //Apply User Dimensions Validations
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //Regions
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.Region).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.CódigoRegião));
            //FunctionalAreas
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.CódigoÁreaFuncional));
            //ResponsabilityCenter
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CódigoCentroResponsabilidade));

            List<ContractViewModel> result = new List<ContractViewModel>();

            ContractsList.ForEach(x => result.Add(DBContracts.ParseToViewModel(x, _config.NAVDatabaseName, _config.NAVCompanyName)));

            List<EnumData> status = EnumerablesFixed.ProposalsStatus;
            result.ForEach(x => { x.StatusDescription = status.Where(y => y.Id == x.Status).Select(y => y.Value).FirstOrDefault(); });
            return Json(result);
        }

        #endregion

        #region Avenca Fixa

        public JsonResult GetAllAvencaFixa()
        {
            List<AutorizarFaturaçãoContratos> contractList = DBContractInvoices.GetAllInvoice();
            List<FaturacaoContratosViewModel> result = new List<FaturacaoContratosViewModel>();

            foreach (var item in contractList)
            {
                //Client Name -> NAV
                String cliName = DBNAV2017Clients.GetClientNameByNo(item.NºCliente, _config.NAVDatabaseName, _config.NAVCompanyName);

                // Valor Fatura
                List<LinhasFaturaçãoContrato> contractInvoiceLines = DBInvoiceContractLines.GetById(item.NºContrato);
                Decimal sum = contractInvoiceLines.Sum(x => x.ValorVenda).Value;

                result.Add(new FaturacaoContratosViewModel
                {
                    ContractNo = item.NºContrato,
                    Description = item.Descrição,
                    ClientNo = item.NºCliente,
                    ClientName = cliName,
                    InvoiceValue = sum,
                    NumberOfInvoices = item.NºDeFaturasAEmitir,
                    InvoiceTotal = item.TotalAFaturar,
                    ContractValue = item.ValorDoContrato,
                    ValueToInvoice = item.ValorPorFaturar,
                    BilledValue = item.ValorFaturado,
                    RegionCode = item.CódigoRegião,
                    FunctionalAreaCode = item.CódigoÁreaFuncional,
                    ResponsabilityCenterCode = item.CódigoCentroResponsabilidade,
                    RegisterDate = item.DataPróximaFatura.HasValue ? item.DataPróximaFatura.Value.ToString("yyyy-MM-dd") : ""
                });
            }

            return Json(result);
        }

        public JsonResult GetPedingAvencaFixa()
        {
            List<AutorizarFaturaçãoContratos> contractList = DBAuthorizeInvoiceContracts.GetPedding();
            List<FaturacaoContratosViewModel> result = new List<FaturacaoContratosViewModel>();

            foreach (var item in contractList)
            {
                //Estado Pendente
                
                    String cliName = DBNAV2017Clients.GetClientNameByNo(item.NºCliente, _config.NAVDatabaseName, _config.NAVCompanyName);

                    // Valor Fatura
                    List<LinhasFaturaçãoContrato> contractInvoiceLines = DBInvoiceContractLines.GetById(item.NºContrato);
                    Decimal sum = contractInvoiceLines.Sum(x => x.ValorVenda).Value;

                    result.Add(new FaturacaoContratosViewModel
                    {
                        ContractNo = item.NºContrato,
                        Description = item.Descrição,
                        ClientNo = item.NºCliente,
                        ClientName = cliName,
                        InvoiceValue = sum,
                        NumberOfInvoices = item.NºDeFaturasAEmitir,
                        InvoiceTotal = item.TotalAFaturar,
                        ContractValue = item.ValorDoContrato,
                        ValueToInvoice = item.ValorPorFaturar,
                        BilledValue = item.ValorFaturado,
                        RegionCode = item.CódigoRegião,
                        Situation= item.Situação,
                        FunctionalAreaCode = item.CódigoÁreaFuncional,
                        ResponsabilityCenterCode = item.CódigoCentroResponsabilidade,
                        RegisterDate = item.DataPróximaFatura.HasValue ? item.DataPróximaFatura.Value.ToString("yyyy-MM-dd") : ""
                    });
                
            }
            return Json(result);
        }

        public JsonResult GenerateInvoice([FromBody] List<FaturacaoContratosViewModel> data)
        {
            // Delete All lines From "Autorizar Faturação Contratos" & "Linhas Faturação Contrato"
            DBAuthorizeInvoiceContracts.DeleteAllAllowedInvoiceAndLines();

            List<Contratos> contractList = DBContracts.GetAllAvencaFixa2();
            foreach (var item in contractList)
            {
                List<LinhasContratos> contractLinesList = DBContractLines.GetAllByNoTypeVersion(item.NºDeContrato, item.TipoContrato, item.NºVersão, true);
                contractLinesList.OrderBy(x => x.NºContrato).ThenBy(y => y.GrupoFatura);

                String ContractNoDuplicate = "";
                int InvoiceGroupDuplicate = -1;
                DateTime current = DateTime.Now;
                DateTime lastDay = (new DateTime(current.Year, current.Month, 1)).AddMonths(1).AddDays(-1);
                string Problema;
                foreach (var line in contractLinesList)
                {
                  
                    Decimal lineQuantity = 1;

                    if (ContractNoDuplicate != line.NºContrato || InvoiceGroupDuplicate != line.GrupoFatura)
                    {
                        ContractNoDuplicate = line.NºContrato;
                        InvoiceGroupDuplicate = line.GrupoFatura == null ? 0 : line.GrupoFatura.Value;

                        Decimal contractVal = 0;
                        if (item.TipoContrato == 1 || item.TipoContrato == 4)
                        {
                            int NumMeses = 0;

                            if (item.DataExpiração.Value != null && item.DataExpiração.ToString() != "" && item.DataInicial.Value != null && item.DataInicial.ToString() != "")
                            {
                                NumMeses = ((item.DataExpiração.Value.Year - item.DataInicial.Value.Year) * 12) + item.DataExpiração.Value.Month - item.DataInicial.Value.Month;
                            }
                            contractVal = Math.Round((NumMeses * contractLinesList.Sum(x => x.PreçoUnitário.Value)), 2);
                        }

                        List<NAVSalesInvoiceLinesViewModel> salesList = null;
                        List<NAVSalesCrMemoLinesViewModel> crMemo = null;
                        if (item.DataInicial != null && item.DataExpiração != null)
                        {
                            salesList = DBNAV2017SalesInvoiceLine.GetSalesInvoiceLines(_config.NAVDatabaseName, _config.NAVCompanyName, item.NºDeContrato, item.DataInicial.Value, item.DataExpiração.Value);
                            crMemo = DBNAV2017SalesCrMemo.GetSalesCrMemoLines(_config.NAVDatabaseName, _config.NAVCompanyName, item.NºDeContrato, item.DataInicial.Value, item.DataExpiração.Value);
                        }
                        Decimal invoicePeriod = salesList != null ? salesList.Sum(x => x.Amount) : 0;
                        Decimal creditPeriod = crMemo != null ? crMemo.Sum(x => x.Amount) : 0;

                        DateTime nextInvoice = lastDay;
                        int lastInvoice = 0;
                        int invoiceNumber = 0;

                        if (line.Quantidade != 0)
                        {
                            lineQuantity = line.Quantidade == null ? 0 : line.Quantidade.Value;
                        }

                        if (item.ÚltimaDataFatura == null)
                        {
                            if (item.DataInicial != null)
                            {
                                //nextInvoice = item.ÚltimaDataFatura.Value;
                                lastInvoice = item.DataInicial.Value.Month;
                            }
                        }
                        else
                        {
                            nextInvoice = item.ÚltimaDataFatura.Value;
                            lastInvoice = item.ÚltimaDataFatura.Value.Month;
                        }



                        if (item.PeríodoFatura != null || item.PeríodoFatura != 0)
                        {
                            switch (item.PeríodoFatura)
                            {
                                case 1:
                                    nextInvoice = nextInvoice.AddMonths(1);
                                    invoiceNumber = (GetMonthDiff(nextInvoice, current)) / 1;
                                    lineQuantity = lineQuantity * 1;
                                    break;
                                case 2:
                                    nextInvoice = nextInvoice.AddMonths(2);
                                    invoiceNumber = (GetMonthDiff(nextInvoice, current)) / 2;
                                    lineQuantity = lineQuantity * 2;
                                    break;
                                case 3:
                                    nextInvoice = nextInvoice.AddMonths(3);
                                    invoiceNumber = (GetMonthDiff(nextInvoice, current)) / 3;
                                    lineQuantity = lineQuantity * 3;
                                    break;
                                case 4:
                                    nextInvoice = nextInvoice.AddMonths(6);
                                    invoiceNumber = (GetMonthDiff(nextInvoice, current)) / 6;
                                    lineQuantity = lineQuantity * 6;
                                    break;
                                case 5:
                                    nextInvoice = nextInvoice.AddMonths(12);
                                    invoiceNumber = (GetMonthDiff(nextInvoice, current)) / 12;
                                    lineQuantity = lineQuantity * 12;
                                    break;
                                case 6:
                                    //
                                    break;
                                default:
                                    break;
                            }
                        }

                        //Validações para registar situações
                        Problema = "";
                        if (item.TipoFaturação != 1 && item.TipoFaturação != 4)
                        {
                            Problema += "Tipo de fatura mal defenido!";
                        }

                        if (item.Estado != 4)
                        {
                            Problema += "Contrato Não Assinado!";
                        }
                        if (item.EstadoAlteração ==1)
                        {
                            Problema += "Contrato Aberto!";
                        }
                        
                        if (item.PróximaDataFatura < item.DataInicial || item.PróximaDataFatura >item.DataExpiração)
                        {
                            Problema += "Contrato Não Vigente!";
                        }
                        if (item.CódigoRegião =="" || item.CódigoÁreaFuncional == "" || item.CódigoCentroResponsabilidade =="")
                        {
                            Problema += "Dimensões Bloqueadas!";
                        }
                        if (item.CódTermosPagamento == "" || item.CondiçõesPagamento == 0 || item.EnvioAEndereço == "" )
                        {
                            Problema += "Falta Código Termos Pagamento!";
                        }
                        if (item.CondiçõesPagamento == 0 )
                        {
                            Problema += "Falta Codições Pagamento!";
                        }
                        if (item.EnvioAEndereço == "")
                        {
                            Problema += "Falta Morada!";
                        }
                        
                        bool verifica = false;
                        if (item.NºComprimissoObrigatório == false || item.NºComprimissoObrigatório== null)
                        {
                           foreach(RequisiçõesClienteContrato req in DBContractClientRequisition.GetByContract(item.NºContrato))
                            {
                                if(req.GrupoFatura==line.GrupoFatura && req.DataInícioCompromisso==item.DataInícioContrato && req.DataFimCompromisso == item.DataFimContrato)
                                {
                                    if (req.NºCompromisso == "" || req.NºCompromisso == null)
                                        verifica = true;                                      
                                }
                            }
                        }
                        if(verifica==true)
                            Problema += "Falta Nº Compromisso";

                        NAVSalesHeaderViewModel result= DBNAV2017SalesHeader.GetSalesHeader(_config.NAVDatabaseName, _config.NAVCompanyName, item.NºDeContrato, 2);
                        if (result != null)
                        {
                           Problema += "Fatura no Pre-Registo";
                        }

                        Task<ClientDetailsViewModel> postNAV = WSCustomerService.GetByNoAsync(item.NºCliente, _configws);
                        postNAV.Wait();
                        if (postNAV.IsCompletedSuccessfully == true && postNAV.Result!=null)
                        {
                           if(postNAV.Result.Blocked == WSClientNAV.Blocked.Invoice || postNAV.Result.Blocked == WSClientNAV.Blocked.All)
                            {
                                Problema += "Cliente Bloqueado";
                            }
                        }

                        AutorizarFaturaçãoContratos newInvoiceContract = new AutorizarFaturaçãoContratos
                        {
                            NºContrato = item.NºDeContrato,
                            GrupoFatura = line.GrupoFatura == null ? 0 : line.GrupoFatura.Value,
                            Descrição = item.Descrição,
                            NºCliente = item.NºCliente,
                            CódigoRegião = item.CódigoRegião,
                            CódigoÁreaFuncional = item.CódigoÁreaFuncional,
                            CódigoCentroResponsabilidade = item.CódigoCentroResponsabilidade,
                            ValorDoContrato = contractVal,
                            ValorFaturado = (invoicePeriod - creditPeriod),
                            ValorPorFaturar = (contractVal - (invoicePeriod - creditPeriod)),
                            NºDeFaturasAEmitir = invoiceNumber,
                            DataPróximaFatura = nextInvoice,
                            DataDeRegisto = lastDay,
                            Estado = item.Estado,
                            Situação=Problema,
                            DataHoraCriação = DateTime.Now,
                            UtilizadorCriação = User.Identity.Name
                        };
                        try
                        {
                            DBAuthorizeInvoiceContracts.Create(newInvoiceContract);
                        }
                        catch (Exception ex)
                        {
                            return Json(false);
                        }
                    }

                    if (lineQuantity == 0)
                    {
                        Problema = "Sem Valor!";
                    }

                    //Create Contract Lines
                    LinhasFaturaçãoContrato newInvoiceLine = new LinhasFaturaçãoContrato
                    {
                        NºContrato = line.NºContrato,
                        GrupoFatura = line.GrupoFatura == null ? -1 : line.GrupoFatura.Value,
                        NºLinha = line.NºLinha,
                        Tipo = line.Tipo.ToString(),
                        Código = line.Código,
                        Descrição = line.Descrição,
                        Quantidade = lineQuantity,
                        CódUnidadeMedida = line.CódUnidadeMedida,
                        PreçoUnitário = line.PreçoUnitário,
                        ValorVenda = (lineQuantity * line.PreçoUnitário),
                        CódigoRegião = line.CódigoRegião,
                        CódigoÁreaFuncional = line.CódigoÁreaFuncional,
                        CódigoCentroResponsabilidade = line.CódigoCentroResponsabilidade,
                        CódigoServiço = line.CódServiçoCliente,
                        DataHoraCriação = DateTime.Now,
                        UtilizadorCriação = User.Identity.Name
                    };
                    try
                    {
                        DBInvoiceContractLines.Create(newInvoiceLine);
                    }
                    catch (Exception ex)
                    {
                        return Json(false);
                    }
                }
            }
            return Json(true);
        }

        public JsonResult CountInvoice([FromBody] List<FaturacaoContratosViewModel> data)
        {
            List<AutorizarFaturaçãoContratos> contractList = DBAuthorizeInvoiceContracts.GetAll();
            List<LinhasFaturaçãoContrato> lineList = DBInvoiceContractLines.GetAll();

            foreach (var item in contractList)
            {
                if (item.Situação == "" || item.Situação == null)
                {
                    Task<WSCreatePreInvoice.Create_Result> InvoiceHeader = WSPreInvoice.CreateContractInvoice(item, _configws);
                    InvoiceHeader.Wait();

                    if (InvoiceHeader.IsCompletedSuccessfully && InvoiceHeader != null)
                    {
                        String InvoiceHeaderNo = InvoiceHeader.Result.WSPreInvoice.No;
                        List<LinhasFaturaçãoContrato> itemList = lineList.Where(x => x.NºContrato == item.NºContrato && x.GrupoFatura == item.GrupoFatura).ToList();

                        if (itemList.Count > 0)
                        {
                            Task<WSCreatePreInvoiceLine.CreateMultiple_Result> InvoiceLines = WSPreInvoiceLine.CreatePreInvoiceLineList(itemList, InvoiceHeaderNo, _configws);
                            InvoiceLines.Wait();

                            if (InvoiceLines.IsCompletedSuccessfully && InvoiceLines != null)
                            {
                                Task<WSGenericCodeUnit.FxPostInvoice_Result> postNAV = WSGeneric.CreatePreInvoiceLineList(InvoiceHeaderNo, _configws);
                                postNAV.Wait();

                                if (!postNAV.IsCompletedSuccessfully || postNAV == null)
                                {
                                    return Json(false);
                                }
                            }
                            else
                            {
                                return Json(false);
                            }
                        }
                    }
                    else
                    {
                        return Json(false);
                    }
                }
            }
            // Delete Lines

            return Json(true);
        }
        #endregion

        #region Propostas
        public IActionResult Propostas(int? archived, string contractNo)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Propostas);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.Archived = archived == null ? 0 : 1;
                ViewBag.ContractNo = contractNo ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult DetalhesProposta(string id, string version = "")
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Propostas);
            if (UPerm != null && UPerm.Read.Value)
            {
                Contratos cContract = null;
                if (version != "")
                    cContract = DBContracts.GetByIdAndVersion(id, int.Parse(version));
                else
                    cContract = DBContracts.GetByIdLastVersion(id);

                if (cContract != null && cContract.Arquivado == true)
                {
                    UPerm.Update = false;
                }

                ViewBag.ContractNo = id ?? "";
                ViewBag.VersionNo = version ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public JsonResult GetListContractsProposals([FromBody] JObject requestParams)
        {
            //int AreaId = int.Parse(requestParams["AreaId"].ToString());
            int Archived = int.Parse(requestParams["Archived"].ToString());
            string ContractNo = requestParams["ContractNo"].ToString();
            int showLevel = int.Parse(requestParams["showLevel"].ToString());

            List<Contratos> ContractsList = null;

            if (ContractNo == "")
            {
                ContractsList = DBContracts.GetAllByContractType(ContractType.Proposal);

                if (Archived == 0)
                    ContractsList.RemoveAll(x => x.Arquivado.HasValue && x.Arquivado.Value);
                else
                    ContractsList.RemoveAll(x => x.Arquivado.HasValue && !x.Arquivado.Value);
            }
            else
            {
                ContractsList = DBContracts.GetByNo(ContractNo, true);
            }

            //Apply User Dimensions Validations
            List<AcessosDimensões> userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //Regions
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.Region).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.CódigoRegião));
            //FunctionalAreas
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.CódigoÁreaFuncional));
            //ResponsabilityCenter
            if (userDimensions.Where(x => x.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                ContractsList.RemoveAll(x => !userDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CódigoCentroResponsabilidade));

            List<ContractViewModel> result = new List<ContractViewModel>();

            if (showLevel == 2) //Canceladas
            {
                ContractsList.RemoveAll(x => !x.Estado.HasValue || x.Estado.Value != 5);
            }
            else if (showLevel == 3) //Perdidas
            {
                ContractsList.RemoveAll(x => !x.Estado.HasValue || x.Estado.Value != 4);
            }

            ContractsList.ForEach(x => result.Add(DBContracts.ParseToViewModel(x, _config.NAVDatabaseName, _config.NAVCompanyName)));

            List<EnumData> status = EnumerablesFixed.ProposalsStatus;
            result.ForEach(x => {
                x.StatusDescription = status.Where(y => y.Id == x.Status).Select(y => y.Value).FirstOrDefault();
                //x.CodeRegion = DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 1, User.Identity.Name, x.CodeRegion).FirstOrDefault().Name ?? "";
                //x.CodeFunctionalArea = DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 2, User.Identity.Name, x.CodeFunctionalArea).FirstOrDefault().Name ?? "";
                //x.CodeResponsabilityCenter = DBNAV2017DimensionValues.GetById(_config.NAVDatabaseName, _config.NAVCompanyName, 3, User.Identity.Name, x.CodeResponsabilityCenter).FirstOrDefault().Name ?? "";

            });

            return Json(result);
        }

        public JsonResult GetListContractsProposalsById([FromBody] JObject requestParams)
        {
            string ContractNo = requestParams["ContractNo"].ToString();

            List<Contratos> ContractsList = null;
            List<ContractViewModel> result = new List<ContractViewModel>();

            ContractsList = DBContracts.GetAllByContractProposalsNo(ContractNo);
            ContractsList.ForEach(x => result.Add(DBContracts.ParseToViewModel(x, _config.NAVDatabaseName, _config.NAVCompanyName)));
            return Json(result);
        }

        public JsonResult GetContractsProposalById([FromBody] string ContractNo)
        {
            Contratos ContractsList = null;
            bool haveProposals = false;

            ContractsList = DBContracts.GetContractProposalsNo(ContractNo);

            if (ContractsList != null)
            {
                haveProposals = true;
            }

            return Json(haveProposals);
        }

        public JsonResult GetListContractsAllProposals()
        {
            List<Contratos> ContractsList = DBContracts.GetAllByContractType(ContractType.Proposal);

            List<ContractViewModel> result = new List<ContractViewModel>();

            ContractsList.ForEach(x => result.Add(DBContracts.ParseToViewModel(x, _config.NAVDatabaseName, _config.NAVCompanyName)));

            List<EnumData> status = EnumerablesFixed.ProposalsStatus;
            result.ForEach(x => { x.StatusDescription = status.Where(y => y.Id == x.Status).Select(y => y.Value).FirstOrDefault(); });
            return Json(result);
        }

        public JsonResult GetProposalDetails([FromBody] JObject requestParams)
        {
            int contractVersion = int.Parse(requestParams["Version"].ToString());
            string contractNo = requestParams["ContractNo"].ToString();


            ContractViewModel result = DBContracts.ParseToViewModel(DBContracts.GetByIdAndVersion(contractNo, contractVersion), _config.NAVDatabaseName, _config.NAVCompanyName);

            List<EnumData> status = EnumerablesFixed.ProposalsStatus;
            result.StatusDescription = status.Where(y => y.Id == result.Status).Select(y => y.Value).FirstOrDefault();

            result.ContractNo = "";
            result.ContractReference = "";
            result.VersionNo = 1;
            result.OportunityNo = "";
            return Json(result);
        }

        [HttpPost]
        public JsonResult SetProposalStatus([FromBody] ContractViewModel item)
        {
            if (item != null)
            {
                ProposalsService serv = new ProposalsService(User.Identity.Name);
                item = serv.SetStatus(item);
            }
            return Json(item);
        }

        [HttpPost]
        public JsonResult CreateProposalFromContract([FromBody] JObject requestParams)
        {
            string contractId = requestParams["contractId"].ToString();
            int version = int.Parse(requestParams["versionNo"].ToString());
            string percentage = requestParams["percentageToApllyInLines"].ToString();
            decimal percentageToApllyInLines = decimal.MinValue;

            if (!string.IsNullOrEmpty(percentage))
                decimal.TryParse(requestParams["percentageToApllyInLines"].ToString(), out percentageToApllyInLines);

            ErrorHandler result = new ErrorHandler();
            try
            {
                ProposalsService serv = new ProposalsService(User.Identity.Name);
                result = serv.CreateProposalFromContract(contractId, version, percentageToApllyInLines);
            }
            catch
            {
                result = new ErrorHandler()
                {
                    eReasonCode = 2,
                    eMessage = "Ocorreu um erro ao criar a proposta",
                };
            }
            return Json(result);
        }

        public IActionResult PropostasContrato(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Projetos);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                ViewBag.ContractNo = string.IsNullOrEmpty(id) ? string.Empty : id;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        #endregion

        #region Invoice
        [HttpPost]
        public JsonResult CreateInvoiceHeaderFromContract([FromBody] JObject requestParams)
        {
            bool registado = false;
            if (requestParams["Contrato"].ToString() != null && requestParams["LinhasContrato"].ToString() != null)
            {
                string obs = "";
                ContractViewModel Contract = JsonConvert.DeserializeObject<ContractViewModel>(requestParams["Contrato"].ToString());
                List<ContractLineViewModel> ContractLines = JsonConvert.DeserializeObject<List<ContractLineViewModel>>(requestParams["LinhasContrato"].ToString());
                string groupInvoice = requestParams["GrupoFatura"].ToString();
                List<int> groups = new List<int>();

                if (groupInvoice != null && groupInvoice != "")
                {
                    
                    foreach(ContractInvoiceTextViewModel texts in Contract.InvoiceTexts)
                    {
                        if (texts.InvoiceGroup == Convert.ToInt32(groupInvoice))
                        {
                            obs += texts.InvoiceText;
                        }
                    }
                    //CREATE SALES HEADER
                    NAVSalesHeaderViewModel PreInvoiceToCreate = new NAVSalesHeaderViewModel();
                    PreInvoiceToCreate.Sell_toCustomerNo = Contract.ClientNo;
                    PreInvoiceToCreate.DocumentDate = DateTime.Parse(Contract.CreateDate);
                    if (Contract.SentData != null && Contract.SentData != "")
                        PreInvoiceToCreate.ShipmentDate = DateTime.Parse(Contract.SentData);
                    if (Contract.ContractStartDate != "" && Contract.ContractEndDate != "")
                        PreInvoiceToCreate.PeriododeFact_Contrato = Contract.ContractStartDate + " a " + Contract.ContractEndDate;
                    PreInvoiceToCreate.ValorContrato = Contract.TotalValue ?? 0;
                    PreInvoiceToCreate.Ship_toAddress = Contract.ShippingAddress;
                    PreInvoiceToCreate.Ship_toPostCode = Contract.ShippingZipCode;
                    if (Contract.DueDate != null && Contract.DueDate != "")
                        PreInvoiceToCreate.DueDate = DateTime.Parse(Contract.DueDate);
                    PreInvoiceToCreate.PaymentTermsCode = Contract.CodePaymentTerms;
                    //PreInvoiceToCreate.ResponsibilityCenter= Contract.CodeResponsabilityCenter;
                    PreInvoiceToCreate.No_Compromisso = Contract.PromiseNo;
                    PreInvoiceToCreate.CodigoPedido = Contract.ClientRequisitionNo;
                    if (Contract.ReceiptDateRequisition != null && Contract.ReceiptDateRequisition != "")
                        PreInvoiceToCreate.DataEncomenda = DateTime.Parse(Contract.ReceiptDateRequisition);
                    string mes = DateTime.Now.ToString("MMMM");
                    PreInvoiceToCreate.DataServ_Prestado = String.Format("{0}/{1}", mes.ToUpper(), DateTime.Now.Year);
                    PreInvoiceToCreate.Observacoes = obs;
                    PreInvoiceToCreate.ContractNo = Contract.ContractNo;
                    PreInvoiceToCreate.FacturaCAF = true;
                    PreInvoiceToCreate.Userpreregisto2009 = User.Identity.Name;
                    if (Contract.StartDateFirstContract != null && Contract.StartDateFirstContract != "")
                        PreInvoiceToCreate.PostingDate = DateTime.Parse(Contract.StartDateFirstContract);

                    PreInvoiceToCreate.ResponsabilityCenterCode20 = Contract.CodeResponsabilityCenter;
                    PreInvoiceToCreate.FunctionAreaCode20 = Contract.CodeFunctionalArea;
                    PreInvoiceToCreate.RegionCode20 = Contract.CodeRegion;
                    obs = "";
                    Task<WSCreatePreInvoice.Create_Result> InvoiceHeader = WSPreInvoice.CreatePreInvoiceHeader(PreInvoiceToCreate, _configws);
                    InvoiceHeader.Wait();
                    if (InvoiceHeader.IsCompletedSuccessfully && InvoiceHeader.Result!=null)
                    { 
                        string cod = InvoiceHeader.Result.WSPreInvoice.No;
                        List<LinhasFaturaçãoContrato> LinhasFaturacao = new List<LinhasFaturaçãoContrato>();
                        foreach (ContractLineViewModel line in ContractLines)
                        {
                            //CREATE SALES LINES
                            if (line.Billable == true)
                            {
                                LinhasFaturaçãoContrato PreInvoiceLinesToCreate = new LinhasFaturaçãoContrato();
                                PreInvoiceLinesToCreate.Tipo = "1";
                                PreInvoiceLinesToCreate.Descrição = line.Description;
                                PreInvoiceLinesToCreate.CódUnidadeMedida = line.CodeMeasureUnit;
                                PreInvoiceLinesToCreate.CódigoÁreaFuncional = line.CodeFunctionalArea;
                                PreInvoiceLinesToCreate.CódigoRegião = line.CodeRegion;
                                PreInvoiceLinesToCreate.CódigoCentroResponsabilidade = line.CodeResponsabilityCenter;
                                PreInvoiceLinesToCreate.NºContrato = Contract.ContractNo;
                                PreInvoiceLinesToCreate.CódigoServiço = line.ServiceClientNo;
                                PreInvoiceLinesToCreate.Quantidade = line.Quantity * Contract.InvocePeriod;
                                PreInvoiceLinesToCreate.PreçoUnitário = line.UnitPrice;
                                LinhasFaturacao.Add(PreInvoiceLinesToCreate);
                            }
                        }
                        Task<WSCreatePreInvoiceLine.CreateMultiple_Result> InvoiceLines = WSPreInvoiceLine.CreatePreInvoiceLineList(LinhasFaturacao, cod, _configws);
                        InvoiceLines.Wait();
                        if (InvoiceLines.IsCompletedSuccessfully)
                        {
                            registado = true;
                        }
                    }

                }
                else
                {
                    foreach (ContractLineViewModel line in ContractLines)
                    {
                        if (groups.Find(x => x == line.InvoiceGroup) == 0)
                        {
                            groups.Add(line.InvoiceGroup ?? 0);
                        }
                    }

                    foreach (int group in groups)
                    {
                        //CREATE SALES HEADER
                        foreach (ContractInvoiceTextViewModel texts in Contract.InvoiceTexts)
                        {
                            if (texts.InvoiceGroup == Convert.ToInt32(group))
                            {
                                obs += texts.InvoiceText;
                            }
                        }
                        NAVSalesHeaderViewModel PreInvoiceToCreate = new NAVSalesHeaderViewModel();
                        PreInvoiceToCreate.Sell_toCustomerNo = Contract.ClientNo;
                        PreInvoiceToCreate.DocumentDate = DateTime.Parse(Contract.CreateDate);
                        if (Contract.SentData != null && Contract.SentData != "")
                            PreInvoiceToCreate.ShipmentDate = DateTime.Parse(Contract.SentData);
                        if(Contract.ContractStartDate!="" && Contract.ContractEndDate !="")
                           PreInvoiceToCreate.PeriododeFact_Contrato = Contract.ContractStartDate + " a " + Contract.ContractEndDate;
                        PreInvoiceToCreate.ValorContrato = Contract.TotalValue ?? 0;
                        PreInvoiceToCreate.Ship_toAddress = Contract.ShippingAddress;
                        PreInvoiceToCreate.Ship_toPostCode = Contract.ShippingZipCode;
                        if (Contract.DueDate != null && Contract.DueDate != "")
                            PreInvoiceToCreate.DueDate = DateTime.Parse(Contract.DueDate);
                        PreInvoiceToCreate.PaymentTermsCode = Contract.CodePaymentTerms;
                        //PreInvoiceToCreate.ResponsibilityCenter= Contract.CodeResponsabilityCenter;
                        PreInvoiceToCreate.No_Compromisso = Contract.PromiseNo;
                        PreInvoiceToCreate.CodigoPedido = Contract.ClientRequisitionNo;
                        if (Contract.ReceiptDateRequisition != null && Contract.ReceiptDateRequisition != "")
                            PreInvoiceToCreate.DataEncomenda = DateTime.Parse(Contract.ReceiptDateRequisition);
                        string mes = DateTime.Now.ToString("MMMM");
                        PreInvoiceToCreate.DataServ_Prestado = String.Format("{0}/{1}", mes.ToUpper(), DateTime.Now.Year);
                        PreInvoiceToCreate.Observacoes = obs;
                        PreInvoiceToCreate.ContractNo = Contract.ContractNo;
                        PreInvoiceToCreate.FacturaCAF = true;
                        PreInvoiceToCreate.Userpreregisto2009 = User.Identity.Name;
                        if (Contract.StartDateFirstContract != null && Contract.StartDateFirstContract != "")
                            PreInvoiceToCreate.PostingDate = DateTime.Parse(Contract.StartDateFirstContract);
                       
                        PreInvoiceToCreate.ResponsabilityCenterCode20 = Contract.CodeResponsabilityCenter;
                        PreInvoiceToCreate.FunctionAreaCode20 = Contract.CodeFunctionalArea;
                        PreInvoiceToCreate.RegionCode20 = Contract.CodeRegion;
                        obs = "";
                        Task<WSCreatePreInvoice.Create_Result> InvoiceHeader = WSPreInvoice.CreatePreInvoiceHeader(PreInvoiceToCreate, _configws);
                        InvoiceHeader.Wait();
                        if (InvoiceHeader.IsCompletedSuccessfully && InvoiceHeader.Result != null)
                        {
                            string cod = InvoiceHeader.Result.WSPreInvoice.No;
                            List<LinhasFaturaçãoContrato> LinhasFaturacao = new List<LinhasFaturaçãoContrato>();
                            foreach (ContractLineViewModel line in ContractLines)
                            {
                                //CREATE SALES LINES
                                if (line.Billable == true && group == line.InvoiceGroup)
                                {
                                    LinhasFaturaçãoContrato PreInvoiceLinesToCreate = new LinhasFaturaçãoContrato();
                                    PreInvoiceLinesToCreate.Tipo = "1";
                                    PreInvoiceLinesToCreate.Descrição = line.Description;
                                    PreInvoiceLinesToCreate.CódUnidadeMedida = line.CodeMeasureUnit;
                                    PreInvoiceLinesToCreate.CódigoÁreaFuncional = line.CodeFunctionalArea;
                                    PreInvoiceLinesToCreate.CódigoRegião = line.CodeRegion;
                                    PreInvoiceLinesToCreate.CódigoCentroResponsabilidade = line.CodeResponsabilityCenter;
                                    PreInvoiceLinesToCreate.NºContrato = Contract.ContractNo;
                                    PreInvoiceLinesToCreate.CódigoServiço = line.ServiceClientNo;
                                    PreInvoiceLinesToCreate.Quantidade = line.Quantity * Contract.InvocePeriod;
                                    PreInvoiceLinesToCreate.PreçoUnitário = line.UnitPrice;
                                    PreInvoiceLinesToCreate.GrupoFatura = line.InvoiceGroup ?? 0;
                                    LinhasFaturacao.Add(PreInvoiceLinesToCreate);
                                }
                            }
                            Task<WSCreatePreInvoiceLine.CreateMultiple_Result> InvoiceLines = WSPreInvoiceLine.CreatePreInvoiceLineList(LinhasFaturacao, cod, _configws);
                            InvoiceLines.Wait();
                            if (InvoiceLines.IsCompletedSuccessfully)
                            {
                                registado = true;
                            }
                        }
                        
                    }
                }
            }

            return Json(registado);
        }

        public JsonResult ExitSalesHeader([FromBody] JObject requestParams)
        {

            string Contract = requestParams["Contrato"].ToString();
            NAVSalesHeaderViewModel result = new NAVSalesHeaderViewModel();
            result.eReasonCode = 0;
            result.eMessage = "";
            try
            {
                result = DBNAV2017SalesHeader.GetSalesHeader(_config.NAVDatabaseName, _config.NAVCompanyName, Contract, 2);

            }
            catch
            {

                result.eReasonCode = 2;
                result.eMessage = "Ocorreu um erro ao pesquisar Vendas";

            }

            if (result != null)
            {
                result.eReasonCode = 1;
                result.eMessage = "Existem facturas não registadas ligadas a este contrato. Deseja continuar?";
            }
            else if (result == null)
            {
                result = new NAVSalesHeaderViewModel();
                result.eReasonCode = 0;
                result.eMessage = "";
            }
            return Json(result);
        }

        #endregion

        public JsonResult ParseContractType([FromBody] JObject requestParams)
        {
            // Parse Header
            String contractNo = requestParams["HeaderNo"].ToString();
            String versionNo = requestParams["VersionNo"].ToString();

            int originType = int.Parse(requestParams["OriginType"].ToString());
            int contractType = int.Parse(requestParams["HeaderType"].ToString());

            Boolean isParcial = Boolean.Parse(requestParams["Parcial"].ToString());


            String newNumeration = "";

            if (contractNo != null && contractNo != "" &&
                versionNo != null && versionNo != "" &&
                originType != 0 && contractType != 0)
            {
                Contratos thisHeader = DBContracts.GetByIdAndVersion(contractNo, int.Parse(versionNo));

                if (thisHeader != null)
                {
                    newNumeration = DBNumerationConfigurations.GetNextNumeration(GetNumeration(contractType), true);
                    try
                    {
                        thisHeader.Arquivado = false;

                        if (originType == 2)
                        {
                            List<LinhasContratos> ContractLines = DBContractLines.GetAllByActiveContract(contractNo, int.Parse(versionNo)).OrderBy(x => x.NºLinha).ToList();
                            try
                            {
                                if (isParcial)
                                {
                                    thisHeader.Estado = 8;
                                }
                                else
                                {
                                    thisHeader.Estado = 7;
                                }
                                thisHeader.TipoContrato = originType;
                                thisHeader.NºDeContrato = contractNo;
                                thisHeader.NºContrato = newNumeration;
                                thisHeader.NºProposta = contractNo;
                                thisHeader.Arquivado = true;
                                DBContracts.Update(thisHeader);


                                thisHeader.Arquivado = false;
                                thisHeader.TipoContrato = contractType;
                                thisHeader.NºProposta = contractNo;
                                thisHeader.NºDeContrato = newNumeration;
                                thisHeader.EstadoAlteração = 2;
                                //thisHeader.Estado = ????? ABARROS
                                string create = DBContracts.Create(thisHeader).NºDeContrato;

                                if (create != null)
                                {
                                    if (isParcial)
                                    {
                                        ContractLines.RemoveAll(x => !x.CriaContrato.HasValue || !x.CriaContrato.Value);
                                    }
                                    foreach (var contractlinestocreate in ContractLines)
                                    {
                                        LinhasContratos newline = ParseToNewModel(contractlinestocreate);

                                        newline.TipoContrato = contractType;
                                        newline.NºContrato = newNumeration;
                                        newline.NºVersão = int.Parse(versionNo);
                                        newline.NºLinha = 0;
                                        DBContractLines.Create(newline);
                                    }
                                    if (string.IsNullOrEmpty(thisHeader.NºCliente) && !string.IsNullOrEmpty(thisHeader.NºContato))
                                    {
                                        //convert contact to custumer
                                        Task<WSGenericCodeUnit.FxContact2Customer_Result> convertToCustumerTask = WSGeneric.ConvertToCustomer(thisHeader.NºContato, _configws);
                                        convertToCustumerTask.Wait();
                                        if (convertToCustumerTask.IsCompletedSuccessfully)
                                        {
                                            thisHeader.NºCliente = convertToCustumerTask.Result.return_value;
                                            DBContracts.Update(thisHeader);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                return null;
                            }

                            //Update Last Numeration Used
                            ConfiguraçãoNumerações ConfigNumerations = DBNumerationConfigurations.GetById(GetNumeration(contractType));
                            ConfigNumerations.ÚltimoNºUsado = newNumeration;
                            ConfigNumerations.UtilizadorModificação = User.Identity.Name;
                            DBNumerationConfigurations.Update(ConfigNumerations);

                        }
                        else if (originType == 1)
                        {
                            try
                            {
                                List<LinhasContratos> ContractLines = DBContractLines.GetAllByActiveContract(contractNo, int.Parse(versionNo)).OrderBy(x => x.NºLinha).ToList();


                                thisHeader.TipoContrato = originType;
                                thisHeader.NºOportunidade = contractNo;
                                thisHeader.NºProposta = newNumeration;
                                thisHeader.NºDeContrato = contractNo;
                                thisHeader.Arquivado = true;
                                DBContracts.Update(thisHeader);

                                thisHeader.Arquivado = false;
                                thisHeader.TipoContrato = contractType;
                                thisHeader.NºOportunidade = contractNo;
                                thisHeader.NºProposta = newNumeration;
                                thisHeader.NºDeContrato = newNumeration;
                                thisHeader.Estado = 1;
                                var create = DBContracts.Create(thisHeader).NºDeContrato;

                                if(create != null)
                                {
                                    foreach (var contractlinestocreate in ContractLines)
                                    {
                                        LinhasContratos newline = ParseToNewModel(contractlinestocreate);

                                        newline.NºLinha = 0;
                                        newline.TipoContrato = contractType;
                                        newline.NºContrato = newNumeration;
                                        newline.NºVersão = int.Parse(versionNo);
                                        DBContractLines.Create(newline);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                return null;
                            }

                            //Update Last Numeration Used
                            ConfiguraçãoNumerações ConfigNumerations = DBNumerationConfigurations.GetById(GetNumeration(contractType));
                            ConfigNumerations.ÚltimoNºUsado = newNumeration;
                            ConfigNumerations.UtilizadorModificação = User.Identity.Name;
                            DBNumerationConfigurations.Update(ConfigNumerations);
                        }
                    }
                    catch (Exception ex)
                    {
                        return Json("Erro ao criar cabeçalho do contrato.");
                    }

                    return Json(newNumeration);
                }
                else
                {
                    return Json("Erro ao tentar aceder à informação do presente proposta.");
                }
            }
            return Json("Informação em falta para converter para contrato.");
        }

        private static LinhasContratos ParseToNewModel(LinhasContratos contractlinestocreate)
        {
            LinhasContratos newline = new LinhasContratos();
            newline.Contratos = contractlinestocreate.Contratos;
            newline.CriaContrato = contractlinestocreate.CriaContrato;
            newline.Código = contractlinestocreate.Código;
            newline.CódigoCentroResponsabilidade = contractlinestocreate.CódigoCentroResponsabilidade;
            newline.CódigoRegião = contractlinestocreate.CódigoRegião;
            newline.CódigoÁreaFuncional = contractlinestocreate.CódigoÁreaFuncional;
            newline.CódServiçoCliente = contractlinestocreate.CódServiçoCliente;
            newline.CódUnidadeMedida = contractlinestocreate.CódUnidadeMedida;
            newline.DataFimVersão = contractlinestocreate.DataFimVersão;
            newline.DataInícioVersão = contractlinestocreate.DataInícioVersão;
            newline.DescontoLinha = contractlinestocreate.DescontoLinha;
            newline.Descrição = contractlinestocreate.Descrição;
            newline.Faturável = contractlinestocreate.Faturável;
            newline.GrupoFatura = contractlinestocreate.GrupoFatura;
            newline.NºHorasIntervenção = contractlinestocreate.NºHorasIntervenção;
            newline.NºResponsável = contractlinestocreate.NºResponsável;
            newline.NºTécnicos = contractlinestocreate.NºTécnicos;
            newline.NºVersão = contractlinestocreate.NºVersão;
            newline.Periodicidade = contractlinestocreate.Periodicidade;
            newline.PreçoUnitário = contractlinestocreate.PreçoUnitário;
            newline.Quantidade = contractlinestocreate.Quantidade;
            newline.Tipo = contractlinestocreate.Tipo;
            newline.TipoProposta = contractlinestocreate.TipoProposta;

            return newline;
        }
        private static int GetNumeration(int type)
        {
            //Get Contract Numeration
            Configuração Configs = DBConfigurations.GetById(1);
            int ProjectNumerationConfigurationId = 0;

            switch (type)
            {
                case 1:
                    ProjectNumerationConfigurationId = Configs.NumeraçãoOportunidades.Value;
                    break;
                case 2:
                    ProjectNumerationConfigurationId = Configs.NumeraçãoPropostas.Value;
                    break;
                case 3:
                    ProjectNumerationConfigurationId = Configs.NumeraçãoContratos.Value;
                    break;
                default:
                    ProjectNumerationConfigurationId = 0;
                    break;
            }
            return ProjectNumerationConfigurationId;
        }

        private static int GetMonthDiff(DateTime NewDate, DateTime OldDate)
        {
            return ((NewDate.Year - OldDate.Year) * 12) + NewDate.Month - OldDate.Month;
        }


        public IActionResult ProjetosContrato(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Projetos);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                ViewBag.ContractId = string.IsNullOrEmpty(id) ? string.Empty : id;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
    }
}