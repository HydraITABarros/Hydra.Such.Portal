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


        public IActionResult Index(int? archived, string contractNo)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.ParseToViewModel(DBUserAccesses.GetByUserId(User.Identity.Name).Where(x => x.Área == 1 && x.Funcionalidade == 2).FirstOrDefault());

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.Archived = archived == null ? 0 : 1;
                ViewBag.ContractNo = contractNo ?? "";
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

        public IActionResult AvencaFixa()
        {
            return View();
        }

        public JsonResult GetListContractsByArea([FromBody] JObject requestParams)
        {
            int AreaId = int.Parse(requestParams["AreaId"].ToString());
            int Archived = int.Parse(requestParams["Archived"].ToString());
            string ContractNo = requestParams["ContractNo"].ToString();

            List<Contratos> ContractsList = null;

            if (Archived == 0 || ContractNo == "")
            {
                if (AreaId == 4)
                {
                    ContractsList = DBContracts.GetAllByContractType(3);
                    ContractsList.RemoveAll(x => x.Arquivado.HasValue && x.Arquivado.Value);
                }
                else
                {
                    ContractsList = DBContracts.GetAllByAreaIdAndType(AreaId, 3);
                    ContractsList.RemoveAll(x => x.Arquivado.HasValue && x.Arquivado.Value);
                }
            }
            else
            {
                ContractsList = DBContracts.GetByNo(ContractNo, true);
            }


            //Apply User Dimensions Validations
            List<AcessosDimensões> CUserDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //Regions
            if  (CUserDimensions.Where(x => x.Dimensão == 1).Count() >0  )
                ContractsList.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == 1 && y.ValorDimensão == x.CódigoRegião));
            //FunctionalAreas
            if (CUserDimensions.Where(x => x.Dimensão == 2).Count() > 0)
                ContractsList.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == 2 && y.ValorDimensão == x.CódigoÁreaFuncional));
            //ResponsabilityCenter
            if (CUserDimensions.Where(x => x.Dimensão == 3).Count() > 0)
                ContractsList.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == 3 && y.ValorDimensão == x.CódigoCentroResponsabilidade));


            List<ContractViewModel> result = new List<ContractViewModel>();

            ContractsList.ForEach(x => result.Add(DBContracts.ParseToViewModel(x, _config.NAVDatabaseName, _config.NAVCompanyName)));

            List<EnumData> status = EnumerablesFixed.ContractStatus;
            result.ForEach(x => { x.StatusDescription = status.Where(y => y.Id == x.Status).Select(y => y.Value).FirstOrDefault(); });

            return Json(result);
        }

        #region Details
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
                            #region old
                            ////Update Fields
                            //ContratoDB.UnidadePrestação = data.ProvisionUnit;
                            //ContratoDB.CódigoCentroResponsabilidade = data.CodeResponsabilityCenter;
                            //ContratoDB.CódTermosPagamento = data.CodePaymentTerms;
                            //ContratoDB.Descrição = data.Description;
                            //ContratoDB.NºCliente = data.ClientNo;
                            //ContratoDB.CódigoRegião = data.CodeRegion;
                            //ContratoDB.CódigoÁreaFuncional = data.CodeFunctionalArea;
                            //ContratoDB.Notas = data.Notes;
                            //ContratoDB.ObjetoServiço = data.ServiceObject;
                            //ContratoDB.DataInício1ºContrato = data.StartDateFirstContract == ""
                            //    ? null
                            //    : (DateTime?)DateTime.Parse(data.StartDateFirstContract);
                            //ContratoDB.NºRequisiçãoDoCliente = data.ClientRequisitionNo;
                            //ContratoDB.NºCompromisso = data.PromiseNo;
                            //ContratoDB.DataInicial = data.StartData == ""
                            //    ? null
                            //    : (DateTime?)DateTime.Parse(data.StartData);
                            //ContratoDB.Estado = data.Status;
                            //ContratoDB.DataReceçãoRequisição = data.ReceiptDateRequisition == ""
                            //    ? null
                            //    : (DateTime?)DateTime.Parse(data.ReceiptDateRequisition);
                            //ContratoDB.NºVersão = data.VersionNo;
                            //ContratoDB.DataExpiração = data.DueDate == ""
                            //    ? null
                            //    : (DateTime?)DateTime.Parse(data.DueDate);
                            //ContratoDB.EstadoAlteração = data.ChangeStatus;
                            //ContratoDB.CódEndereçoEnvio = data.CodeShippingAddress;
                            //ContratoDB.EnvioAEndereço = data.ShippingAddress;
                            //ContratoDB.EnvioALocalidade = data.ShippingLocality;
                            //ContratoDB.EnvioACódPostal = data.ShippingZipCode;
                            //ContratoDB.EnvioANome = data.ShippingName;
                            //ContratoDB.TipoFaturação = data.BillingType;
                            //ContratoDB.Mc = data.Mc;
                            //ContratoDB.ContratoAvençaFixa = data.FixedVowsAgreement;
                            //ContratoDB.JuntarFaturas = data.BatchInvoices;
                            //ContratoDB.TipoContratoManut = data.MaintenanceContractType;
                            //ContratoDB.TaxaDeslocação = data.DisplacementFee;
                            //ContratoDB.ContratoAvençaVariável = data.VariableAvengeAgrement;
                            //ContratoDB.LinhasContratoEmFact = data.ContractLinesInBilling;
                            //ContratoDB.TaxaAprovisionamento = data.ProvisioningFee;
                            //ContratoDB.PeríodoFatura = data.InvocePeriod;
                            //ContratoDB.UtilizadorModificação = User.Identity.Name;
                            //ContratoDB.OrigemDoPedido = data.OrderOrigin;
                            //ContratoDB.DescOrigemDoPedido = data.OrdOrderSource;
                            //ContratoDB.DataEnvioCliente = data.CustomerShipmentDate == ""
                            //    ? null
                            //    : (DateTime?)DateTime.Parse(data.CustomerShipmentDate);
                            //ContratoDB.DataAlteraçãoProposta = data.ProposalChangeDate == ""
                            //    ? null
                            //    : (DateTime?)DateTime.Parse(data.ProposalChangeDate);
                            //ContratoDB.NumeraçãoInterna = data.InternalNumeration;
                            //ContratoDB.ValorTotalProposta = data.TotalProposalValue;
                            //ContratoDB.DataHoraLimiteEsclarecimentos = data.LimitClarificationDate == ""
                            //    ? null
                            //    : (DateTime?)DateTime.Parse(data.LimitClarificationDate);

                            //ContratoDB.DataHoraErrosEOmissões = data.ErrorsOmissionsDate == ""
                            //    ? null
                            //    : (DateTime?)DateTime.Parse(data.ErrorsOmissionsDate);

                            //ContratoDB.DataHoraRelatórioFinal = data.FinalReportDate == ""
                            //    ? null
                            //    : (DateTime?)DateTime.Parse(data.FinalReportDate);
                            //ContratoDB.DataHoraHabilitaçãoDocumental = data.DocumentationHabilitationDate == ""
                            //    ? null
                            //    : (DateTime?)DateTime.Parse(data.DocumentationHabilitationDate);
                            //ContratoDB.PróximaDataFatura = data.NextInvoiceDate == ""
                            //    ? null
                            //    : (DateTime?)DateTime.Parse(data.NextInvoiceDate);
                            //ContratoDB.PróximoPeríodoFact = data.NextBillingPeriod;
                            //ContratoDB.NºContato = data.ContactNo;
                            //ContratoDB.ValorBaseProcedimento = data.BaseValueProcedure;
                            //ContratoDB.AudiênciaPrévia = data.PreviousHearing == ""
                            //    ? null
                            //    : (DateTime?)DateTime.Parse(data.PreviousHearing);
                            //ContratoDB.DataHoraEntregaProposta = data.ProposalDelivery == ""
                            //    ? null
                            //    : (DateTime?)DateTime.Parse(data.ProposalDelivery);

                            //if (ContratoDB.AudiênciaPrévia != null)
                            //{
                            //    ContratoDB.AudiênciaPrévia = ContratoDB.AudiênciaPrévia.Value.Date;
                            //    if (data.PreviousHearingTime != null)
                            //    {
                            //        ContratoDB.AudiênciaPrévia = ContratoDB.AudiênciaPrévia.Value.Add(TimeSpan.Parse(data.PreviousHearingTime));
                            //    }
                            //}

                            //if (ContratoDB.DataHoraLimiteEsclarecimentos != null)
                            //{
                            //    ContratoDB.DataHoraLimiteEsclarecimentos = ContratoDB.DataHoraLimiteEsclarecimentos.Value.Date;
                            //    if (data.LimitClarificationTime != null)
                            //    {
                            //        ContratoDB.DataHoraLimiteEsclarecimentos = ContratoDB.DataHoraLimiteEsclarecimentos.Value.Add(TimeSpan.Parse(data.LimitClarificationTime));
                            //    }
                            //}

                            //if (ContratoDB.DataHoraErrosEOmissões != null)
                            //{
                            //    ContratoDB.DataHoraErrosEOmissões = ContratoDB.DataHoraErrosEOmissões.Value.Date;
                            //    if (data.ErrorsOmissionsTime != null)
                            //    {
                            //        ContratoDB.DataHoraErrosEOmissões = ContratoDB.DataHoraErrosEOmissões.Value.Add(TimeSpan.Parse(data.ErrorsOmissionsTime));
                            //    }
                            //}

                            //if (ContratoDB.DataHoraRelatórioFinal != null)
                            //{
                            //    ContratoDB.DataHoraRelatórioFinal = ContratoDB.DataHoraRelatórioFinal.Value.Date;
                            //    if (data.FinalReportTime != null)
                            //    {
                            //        ContratoDB.DataHoraRelatórioFinal = ContratoDB.DataHoraRelatórioFinal.Value.Add(TimeSpan.Parse(data.FinalReportTime));
                            //    }
                            //}

                            //if (ContratoDB.DataHoraHabilitaçãoDocumental != null)
                            //{
                            //    ContratoDB.DataHoraHabilitaçãoDocumental = ContratoDB.DataHoraHabilitaçãoDocumental.Value.Date;
                            //    if (data.DocumentationHabilitationTime != null)
                            //    {
                            //        ContratoDB.DataHoraHabilitaçãoDocumental = ContratoDB.DataHoraHabilitaçãoDocumental.Value.Add(TimeSpan.Parse(data.DocumentationHabilitationTime));
                            //    }
                            //}

                            //if (ContratoDB.DataHoraEntregaProposta != null)
                            //{
                            //    ContratoDB.DataHoraEntregaProposta = ContratoDB.DataHoraEntregaProposta.Value.Date;
                            //    if (data.ProposalDeliveryTime != null)
                            //    {
                            //        ContratoDB.DataHoraEntregaProposta = ContratoDB.DataHoraEntregaProposta.Value.Add(TimeSpan.Parse(data.ProposalDeliveryTime));
                            //    }
                            //}

                            //ContratoDB.ReferênciaContrato = data.ContractReference;
                            //ContratoDB.DataInícioContrato = data.ContractStartDate != "" && data.ContractStartDate != null ? DateTime.Parse(data.ContractStartDate) : (DateTime?)null;
                            //ContratoDB.DataFimContrato = data.ContractEndDate != "" && data.ContractEndDate != null ? DateTime.Parse(data.ContractEndDate) : (DateTime?)null;
                            //ContratoDB.DescriçãoDuraçãoContrato = data.ContractDurationDescription;
                            //ContratoDB.RescisãoPrazoAviso = data.TerminationTermNotice;
                            //ContratoDB.CondiçõesParaRenovação = data.RenovationConditions;
                            //ContratoDB.CondiçõesRenovaçãoOutra = data.RenovationConditionsAnother;
                            //ContratoDB.CondiçõesPagamento = data.PaymentTerms;
                            //ContratoDB.CondiçõesPagamentoOutra = data.PaymentTermsAnother;
                            //ContratoDB.AssinadoPeloCliente = data.CustomerSigned;
                            //ContratoDB.Juros = data.Interests;
                            //ContratoDB.DataDaAssinatura = data.SignatureDate != "" && data.SignatureDate != null ? DateTime.Parse(data.SignatureDate) : (DateTime?)null;
                            //ContratoDB.DataEnvioCliente = data.CustomerShipmentDate !=  "" && data.CustomerShipmentDate != null ? DateTime.Parse(data.CustomerShipmentDate) : (DateTime?)null;
                            #endregion
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
                Contratos cContract = DBContracts.GetByIdAndVersion(data.ContractNo, data.VersionNo);

                if (cContract != null)
                {
                    try
                    {
                        //Create new contract and update old
                        cContract.UtilizadorModificação = User.Identity.Name;
                        cContract.Arquivado = true;
                        DBContracts.Update(cContract);

                        cContract.NºVersão = cContract.NºVersão + 1;
                        cContract.UtilizadorCriação = User.Identity.Name;
                        cContract.UtilizadorModificação = "";
                        if(cContract.TipoContrato == 1)
                        {
                            cContract.NºProposta = "";
                        }else if(cContract.TipoContrato == 2 )
                        {
                            cContract.NºContrato = "";
                        }
                        
                        cContract.DataHoraModificação = null;
                        cContract.Arquivado = false;

                        if (data.ActionCode.HasValue && data.ActionCode.Value == 2)
                        {
                            cContract.Estado = 1;
                            cContract.DataHoraModificação = DateTime.Now;
                            cContract.UtilizadorModificação = User.Identity.Name;
                        }

                        DBContracts.Create(cContract);

                        //Duplicate Contract Lines
                        List<LinhasContratos> ContractLines = DBContractLines.GetAllByActiveContract(data.ContactNo, data.VersionNo);

                        ContractLines.ForEach(x =>
                        {
                            x.NºVersão = cContract.NºVersão;
                            DBContractLines.Create(x);
                        });

                        data.VersionNo = cContract.NºVersão;
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

        //[HttpPost]
        //public JsonResult UpdateProposalContract([FromBody] ContractViewModel data)
        //{

        //    if (data != null)
        //    {
        //        Contratos cContract = DBContracts.GetByIdAndVersion(data.ContractNo, data.VersionNo);

        //        if (cContract != null)
        //        {
        //            try
        //            {

        //                //Create new contract and update old
        //                cContract.UtilizadorModificação = User.Identity.Name;
        //                cContract.Arquivado = true;
        //                DBContracts.Update(cContract);

        //                cContract.NºVersão = cContract.NºVersão + 1;
        //                cContract.UtilizadorCriação = User.Identity.Name;
        //                cContract.UtilizadorModificação = "";
        //                if (cContract.TipoContrato == 1)
        //                {
        //                    cContract.NºProposta = "";
        //                }
        //                else if (cContract.TipoContrato == 2)
        //                {
        //                    cContract.NºContrato = "";
        //                }

        //                cContract.DataHoraModificação = null;
        //                cContract.Arquivado = false;
        //                DBContracts.Create(cContract);

        //                //Duplicate Contract Lines
        //                List<LinhasContratos> ContractLines = DBContractLines.GetAllByActiveContract(data.ContactNo, data.VersionNo);
        //                ContractLines.RemoveAll(x => !x.CriaContrato.HasValue || !x.CriaContrato.Value);
        //                ContractLines.ForEach(x =>
        //                {
        //                    x.NºVersão = cContract.NºVersão;
        //                    DBContractLines.Create(x);
        //                });

        //                data.VersionNo = cContract.NºVersão;
        //                data.eReasonCode = 1;
        //                data.eMessage = "Contrato atualizado com sucesso.";
        //                return Json(data);
        //            }
        //            catch (Exception)
        //            {
        //                data.eReasonCode = 2;
        //                data.eMessage = "Ocorreu um erro ao atualizar o contrato.";
        //            }
        //        }
        //    }
        //    else
        //    {
        //        data.eReasonCode = 2;
        //        data.eMessage = "Ocorreu um erro ao atualizar o contrato.";
        //    }
        //    return Json(data);
        //}

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
                            CLine.CódServiçoCliente = x.ServiceClientNo == 0 ? null : x.ServiceClientNo;
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
        #endregion

        #region Oportunidades

        public JsonResult GetListOportunitiesByArea([FromBody] JObject requestParams)
        {
            int AreaId = int.Parse(requestParams["AreaId"].ToString());
            int Archived = int.Parse(requestParams["Archived"].ToString());
            string ContractNo = requestParams["ContractNo"].ToString();

            List<Contratos> ContractsList = null;

            if (Archived == 0 )
            {
                if (AreaId == 4)
                {
                    ContractsList = DBContracts.GetAllByContractType(1);
                    ContractsList.RemoveAll(x => x.Arquivado.HasValue && x.Arquivado.Value);
                    //ContractsList.RemoveAll(x => x.Estado == 9);
                }
                else
                {
                    ContractsList = DBContracts.GetAllByAreaIdAndType(AreaId, 1);
                    ContractsList.RemoveAll(x => x.Arquivado.HasValue && x.Arquivado.Value);
                    //ContractsList.RemoveAll(x => x.Estado == 9);
                }
            }
            else
            {
                if (AreaId == 4)
                {
                    ContractsList = DBContracts.GetAllByContractType(1);
                    //ContractsList.RemoveAll(x => x.Estado != 9);
                }
                else
                {
                    ContractsList = DBContracts.GetAllByAreaIdAndType(AreaId, 1);
                    //ContractsList.RemoveAll(x => x.Estado != 9);
                }
            }

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
            List<AutorizarFaturaçãoContratos> contractList = DBContractInvoices.GetAll();
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

        public JsonResult GenerateInvoice([FromBody] List<FaturacaoContratosViewModel> data)
        {
            // Delete All lines From "Autorizar Faturação Contratos" & "Linhas Faturação Contrato"
            DBAuthorizeInvoiceContracts.DeleteAllAllowedInvoiceAndLines();

            List<Contratos> contractList = DBContracts.GetAllAvencaFixa();
            foreach (var item in contractList)
            {
                List<LinhasContratos> contractLinesList = DBContractLines.GetAllByNoTypeVersion(item.NºDeContrato, item.TipoContrato, item.NºVersão, true);
                contractLinesList.OrderBy(x => x.NºContrato).ThenBy(y => y.GrupoFatura);

                String ContractNoDuplicate = "";
                int InvoiceGroupDuplicate = -1;
                DateTime current = DateTime.Now;
                DateTime lastDay = (new DateTime(current.Year, current.Month, 1)).AddMonths(1).AddDays(-1);

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
                                nextInvoice = item.ÚltimaDataFatura.Value;
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
            // Delete Lines

            return Json(true);
        }
        #endregion

        #region Propostas

        public JsonResult GetListContractsProposals([FromBody] JObject requestParams)
        {
            int AreaId = int.Parse(requestParams["AreaId"].ToString());
            int Archived = int.Parse(requestParams["Archived"].ToString());
            string ContractNo = requestParams["ContractNo"].ToString();
            int showLevel = int.Parse(requestParams["showLevel"].ToString());

            List<Contratos> ContractsList = null;

            if (Archived == 0 || ContractNo == "")
            {
                if (AreaId == 4)
                {
                    ContractsList = DBContracts.GetAllByContractType(2);
                    ContractsList.RemoveAll(x => x.Arquivado.HasValue && x.Arquivado.Value);
                }
                else
                {
                    ContractsList = DBContracts.GetAllByAreaIdAndType(AreaId, 2);
                    ContractsList.RemoveAll(x => x.Arquivado.HasValue && x.Arquivado.Value);
                }
            }
            else
            {
                ContractsList = DBContracts.GetByNo(ContractNo, true);
            }

            List<ContractViewModel> result = new List<ContractViewModel>();

            if (showLevel == 1)
            {
                ContractsList.RemoveAll(x => !x.Estado.HasValue || x.Estado.Value == 4 || x.Estado.Value == 5 || x.Estado.Value == 9);
            }
            else if (showLevel == 2)
            {
                ContractsList.RemoveAll(x => !(x.Estado.HasValue && x.Estado.Value == 5) || x.Estado.Value == 9);
            }
            else if (showLevel == 4)
            {
                ContractsList.RemoveAll(x => !x.Estado.HasValue || x.Estado.Value != 9);
            }
            else
            {
                ContractsList.RemoveAll(x => !(x.Estado.HasValue && x.Estado.Value == 4) || x.Estado.Value == 9);
            }

            ContractsList.ForEach(x => result.Add(DBContracts.ParseToViewModel(x, _config.NAVDatabaseName, _config.NAVCompanyName)));

            List<EnumData> status = EnumerablesFixed.ProposalsStatus;
            result.ForEach(x => { x.StatusDescription = status.Where(y => y.Id == x.Status).Select(y => y.Value).FirstOrDefault(); });
            return Json(result);
        }

        public JsonResult GetListContractsAllProposals([FromBody] JObject requestParams)
        {
            int AreaId = int.Parse(requestParams["AreaId"].ToString());

            List<Contratos> ContractsList = DBContracts.GetAllByAreaIdAndType(AreaId, 2);
            
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
                                thisHeader.Estado = 1;
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
    }
}