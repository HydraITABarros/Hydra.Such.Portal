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


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(string id)
        {
            ViewBag.ContractNo = id == null ? "" : id;
            return View();
        }

        public JsonResult GetListContractsByArea([FromBody] int AreaId)
        {
            List<Contratos> AllContracts = DBContracts.GetAllByAreaIdAndType(AreaId, 3);
            

            List<ContractViewModel> result = new List<ContractViewModel>();

            AllContracts.ForEach(x => result.Add(DBContracts.ParseToViewModel(x,_config.NAVDatabaseName,_config.NAVCompanyName)));

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
                Contratos cContract = DBContracts.GetByIdLastVersion(data.ContractNo);
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
                    if (data.ContractNo != null)
                    {
                        //Contratos cContract = DBContracts.ParseToDB(data);


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
                            ContratoDB.DataInício1ºContrato = DateTime.Parse(data.StartDateFirstContract);
                            ContratoDB.NºRequisiçãoDoCliente = data.ClientRequisitionNo;
                            ContratoDB.NºCompromisso = data.PromiseNo;
                            ContratoDB.DataInícioContrato = DateTime.Parse(data.StartData);
                            ContratoDB.Estado = data.Status;
                            ContratoDB.DataReceçãoRequisição = DateTime.Parse(data.ReceiptDateRequisition);
                            ContratoDB.NºVersão = data.VersionNo;
                            ContratoDB.DataExpiração = DateTime.Parse(data.DueDate);
                            ContratoDB.EstadoAlteração = data.ChangeStatus;
                            ContratoDB.CódEndereçoEnvio = data.CodeShippingAddress;
                            ContratoDB.EnvioAEndereço = data.ShippingAddress;
                            ContratoDB.EnvioALocalidade = data.ShippingLocality;
                            ContratoDB.EnvioACódPostal = data.ShippingZipCode;
                            ContratoDB.EnvioANome = data.ShippingName;
                            ContratoDB.TipoFaturação = data.BillingType;
                            ContratoDB.Mc = data.Mc;
                            ContratoDB.ContratoAvençaFixa = data.FixedVowsAgreement;
                            ContratoDB.JuntarFaturas = data.BatchInvoices;
                            ContratoDB.TipoContratoManut = data.MaintenanceContractType;
                            ContratoDB.TaxaDeslocação = data.DisplacementFee;
                            ContratoDB.ContratoAvençaVariável = data.VariableAvengeAgrement;
                            ContratoDB.LinhasContratoEmFact = data.ContractLinesInBilling;
                            ContratoDB.TaxaAprovisionamento = data.ProvisioningFee;
                            ContratoDB.PeríodoFatura = data.InvocePeriod;
                            ContratoDB.UtilizadorModificação = User.Identity.Name;
                            ContratoDB = DBContracts.Update(ContratoDB);



                            //Create/Update Contract Client Requests
                            List<RequisiçõesClienteContrato> RCC = DBContractClientRequisition.GetByContract(ContratoDB.NºContrato);
                            List<RequisiçõesClienteContrato> RCCToDelete = RCC.Where(x => !data.ClientRequisitions.Any(y => x.NºRequisiçãoCliente == x.NºRequisiçãoCliente)).ToList();

                            RCC.ForEach(X =>
                            {
                                //if (RCCToDelete)
                                //{

                                //}
                            });

                            //Delete Contract Client Requests
                            RCCToDelete.ForEach(x => DBContractClientRequisition.Delete(x));

                            //Update Contract Invoice Texts
                        }












                        //Create Contract On Database
                        //cContract = DBContracts.Create(cContract);

                        //if (cContract == null)
                        //{
                        //    data.eReasonCode = 3;
                        //    data.eMessage = "Ocorreu um erro ao criar o contrato.";
                        //}
                        //else
                        //{
                        //    //Create Client Contract Requisitions
                        //    data.ClientRequisitions.ForEach(r =>
                        //    {
                        //        r.ContractNo = cContract.NºContrato;
                        //        r.CreateUser = User.Identity.Name;
                        //        DBContractClientRequisition.Create(DBContractClientRequisition.ParseToDB(r));
                        //    });

                        //    //Create Contract Invoice Texts
                        //    data.InvoiceTexts.ForEach(r =>
                        //    {
                        //        r.ContractNo = cContract.NºContrato;
                        //        r.CreateUser = User.Identity.Name;
                        //        DBContractInvoiceText.Create(DBContractInvoiceText.ParseToDB(r));
                        //    });

                        //    //Update Last Numeration Used
                        //    ConfiguraçãoNumerações ConfigNumerations = DBNumerationConfigurations.GetById(ProjectNumerationConfigurationId);
                        //    ConfigNumerations.ÚltimoNºUsado = data.ContractNo;
                        //    ConfigNumerations.UtilizadorModificação = User.Identity.Name;
                        //    DBNumerationConfigurations.Update(ConfigNumerations);

                        //    data.eReasonCode = 1;
                        //}
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

        #endregion



    }
}