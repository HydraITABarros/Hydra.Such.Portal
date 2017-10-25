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
            ViewBag.Archived = archived == null ? 0 : 1;
            ViewBag.ContractNo = contractNo ?? "";
            return View();
        }

        public IActionResult Details(string id, string version)
        {
            ViewBag.ContractNo = id ?? "";
            ViewBag.VersionNo = version ?? "";
            return View();
        }

        public JsonResult GetListContractsByArea([FromBody] JObject requestParams )
        {
            int AreaId = int.Parse(requestParams["AreaId"].ToString());
            int Archived = int.Parse(requestParams["Archived"].ToString());
            string ContractNo = requestParams["ContractNo"].ToString();

            List<Contratos> ContractsList = null;

            if (Archived == 0 || ContractNo == "")
            {
                ContractsList = DBContracts.GetAllByAreaIdAndType(AreaId, 3);
                ContractsList.RemoveAll(x => x.Arquivado.HasValue && x.Arquivado.Value);
            }
            else
            {
                ContractsList = DBContracts.GetByNo(ContractNo, true);
            }
            


            List<ContractViewModel> result = new List<ContractViewModel>();

            ContractsList.ForEach(x => result.Add(DBContracts.ParseToViewModel(x,_config.NAVDatabaseName,_config.NAVCompanyName)));

            return Json(result);
        }


        #region Details
        [HttpPost]
        public JsonResult ValidateNumeration([FromBody] ContractViewModel data)
        {
            //Get Project Numeration
            Configuração Cfg = DBConfigurations.GetById(1);
            int ProjectNumerationConfigurationId = Cfg.NumeraçãoContratos.Value;

            ConfiguraçãoNumerações CfgNumeration = DBNumerationConfigurations.GetById(ProjectNumerationConfigurationId);

            //Validate if ProjectNo is valid
            if (!(data.ContactNo == "" || data.ContactNo == null) && !CfgNumeration.Manual.Value)
            {
                return Json("A numeração configurada para contratos não permite inserção manual.");
            }
            else if (data.ContactNo == "" && !CfgNumeration.Automático.Value)
            {
                return Json("É obrigatório inserir o Nº de Contratos.");
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
                else {
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
                    Configuração Configs = DBConfigurations.GetById(1);
                    int ProjectNumerationConfigurationId = Configs.NumeraçãoContratos.Value;
                    data.ContractNo = DBNumerationConfigurations.GetNextNumeration(ProjectNumerationConfigurationId, (data.ContractNo == "" || data.ContractNo == null));

                    if (data.ContractNo != null)
                    {


                        Contratos cContract = DBContracts.ParseToDB(data);
                        cContract.TipoContrato = 3;
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
                                r.ContractNo = cContract.NºContrato;
                                r.CreateUser = User.Identity.Name;
                                DBContractClientRequisition.Create(DBContractClientRequisition.ParseToDB(r));
                            });

                            //Create Contract Invoice Texts
                            data.InvoiceTexts.ForEach(r =>
                            {
                                r.ContractNo = cContract.NºContrato;
                                r.CreateUser = User.Identity.Name;
                                DBContractInvoiceText.Create(DBContractInvoiceText.ParseToDB(r));
                            });
                            
                            //Update Last Numeration Used
                            ConfiguraçãoNumerações ConfigNumerations = DBNumerationConfigurations.GetById(ProjectNumerationConfigurationId);
                            ConfigNumerations.ÚltimoNºUsado = data.ContractNo;
                            ConfigNumerations.UtilizadorModificação = User.Identity.Name;
                            DBNumerationConfigurations.Update(ConfigNumerations);

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
                        Contratos ContratoDB = DBContracts.GetByIdAndVersion(data.ContractNo, data.VersionNo);


                        if (ContratoDB != null)
                        {
                            //Update Fields
                            ContratoDB.UnidadePrestação = data.ProvisionUnit;
                            ContratoDB.CódigoCentroResponsabilidade = data.CodeResponsabilityCenter;
                            ContratoDB.CódTermosPagamento = data.CodePaymentTerms;
                            ContratoDB.Descrição = data.Description;
                            ContratoDB.CódigoRegião = data.CodeRegion;
                            ContratoDB.Notas = data.Notes;
                            ContratoDB.DataInício1ºContrato = data.StartDateFirstContract == "" ? null : (DateTime?)DateTime.Parse(data.StartDateFirstContract);
                            ContratoDB.NºRequisiçãoDoCliente = data.ClientRequisitionNo;
                            ContratoDB.NºCompromisso = data.PromiseNo;
                            ContratoDB.DataInicial = data.StartData == "" ?  null : (DateTime?)DateTime.Parse(data.StartData);
                            ContratoDB.Estado = data.Status - 1;
                            ContratoDB.DataReceçãoRequisição = data.ReceiptDateRequisition == "" ? null : (DateTime?)DateTime.Parse(data.ReceiptDateRequisition);
                            ContratoDB.NºVersão = data.VersionNo;
                            ContratoDB.DataExpiração = data.DueDate == "" ? null : (DateTime?)DateTime.Parse(data.DueDate);
                            ContratoDB.EstadoAlteração = data.ChangeStatus - 1;
                            ContratoDB.CódEndereçoEnvio = data.CodeShippingAddress;
                            ContratoDB.EnvioAEndereço = data.ShippingAddress;
                            ContratoDB.EnvioALocalidade = data.ShippingLocality;
                            ContratoDB.EnvioACódPostal = data.ShippingZipCode;
                            ContratoDB.EnvioANome = data.ShippingName;
                            ContratoDB.TipoFaturação = data.BillingType - 1;
                            ContratoDB.Mc = data.Mc;
                            ContratoDB.ContratoAvençaFixa = data.FixedVowsAgreement;
                            ContratoDB.JuntarFaturas = data.BatchInvoices;
                            ContratoDB.TipoContratoManut = data.MaintenanceContractType - 1;
                            ContratoDB.TaxaDeslocação = data.DisplacementFee;
                            ContratoDB.ContratoAvençaVariável = data.VariableAvengeAgrement;
                            ContratoDB.LinhasContratoEmFact = data.ContractLinesInBilling;
                            ContratoDB.TaxaAprovisionamento = data.ProvisioningFee;
                            ContratoDB.PeríodoFatura = data.InvocePeriod - 1;
                            ContratoDB.UtilizadorModificação = User.Identity.Name;
                            ContratoDB = DBContracts.Update(ContratoDB);
                            
                            //Create/Update Contract Client Requests
                            List<RequisiçõesClienteContrato> RCC = DBContractClientRequisition.GetByContract(ContratoDB.NºContrato);
                            List<RequisiçõesClienteContrato> RCCToDelete = RCC.Where(x => !data.ClientRequisitions.Any(y => x.NºRequisiçãoCliente == x.NºRequisiçãoCliente && x.GrupoFatura == y.InvoiceGroup && x.NºProjeto == y.ProjectNo && x.DataInícioCompromisso == DateTime.Parse(y.StartDate))).ToList();

                            data.ClientRequisitions.ForEach(y =>
                            {
                                RequisiçõesClienteContrato RCCO = RCC.Where(x => x.NºRequisiçãoCliente == x.NºRequisiçãoCliente && x.GrupoFatura == y.InvoiceGroup && x.NºProjeto == y.ProjectNo && x.DataInícioCompromisso == DateTime.Parse(y.StartDate)).FirstOrDefault();
                                if (RCCO != null)
                                {
                                    RCCO.NºContrato = y.ContractNo;
                                    RCCO.GrupoFatura = y.InvoiceGroup;
                                    RCCO.NºProjeto = y.ProjectNo;
                                    RCCO.DataInícioCompromisso = DateTime.Parse(y.StartDate);
                                    RCCO.DataFimCompromisso = DateTime.Parse(y.EndDate);
                                    RCCO.NºRequisiçãoCliente = y.ClientRequisitionNo;
                                    RCCO.DataRequisição = DateTime.Parse(y.RequisitionDate);
                                    RCCO.NºCompromisso = y.PromiseNo;
                                    RCCO.DataÚltimaFatura = DateTime.Parse(y.LastInvoiceDate);
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
                            List<TextoFaturaContrato> CITToDelete = CIT.Where(x => !data.InvoiceTexts.Any(y => x.GrupoFatura == y.InvoiceGroup && x.NºProjeto == y.ProjectNo && x.NºContrato == y.ContractNo)).ToList();

                            data.InvoiceTexts.ForEach(y =>
                            {
                                TextoFaturaContrato CITO = CIT.Where(x => x.GrupoFatura == y.InvoiceGroup && x.NºProjeto == y.ProjectNo && x.NºContrato == y.ContractNo).FirstOrDefault();
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
                Contratos cContract = DBContracts.GetByIdAndVersion(data.ContractNo,data.VersionNo);

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
                        cContract.DataHoraModificação = null;
                        cContract.Arquivado = false;
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
                        data.eMessage = "Contrato arquivado com sucesso.";
                        return Json(data);
                    }
                    catch (Exception)
                    {
                        data.eReasonCode = 2;
                        data.eMessage = "Ocorreu um erro ao arquivar o contrato.";
                    }
                }
            }
            else
            {
                data.eReasonCode = 2;
                data.eMessage = "Ocorreu um erro ao arquivar o contrato.";
            }
            return Json(data);
        }
        #endregion



    }
}