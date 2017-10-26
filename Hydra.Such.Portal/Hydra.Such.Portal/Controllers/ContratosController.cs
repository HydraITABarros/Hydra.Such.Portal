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

        public IActionResult AvencaFixa()
        {
            return View();
        }

        public JsonResult GetListContractsByArea([FromBody] int AreaId)
        {
            List<Contratos> AllContracts = DBContracts.GetAllByAreaIdAndType(AreaId, 3);


            List<ContractViewModel> result = new List<ContractViewModel>();

            AllContracts.ForEach(x => result.Add(DBContracts.ParseToViewModel(x, _config.NAVDatabaseName, _config.NAVCompanyName)));

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


        #region Avenca Fixa

        public JsonResult GetAllAvencaFixa()
        {
            List<AutorizarFaturaçãoContratos> contractList = DBContractInvoices.GetAll();
            List<FaturacaoContratosViewModel> result = null;

            foreach (var item in contractList)
            {
                //Client Name -> NAV
                String cliName = DBNAV2017Clients.GetClientNameByNo(_config.NAVDatabaseName, _config.NAVCompanyName, item.NºContrato);

                // Valor Fatura
                List<LinhasFaturaçãoContrato> contractInvoiceLines = DBInvoiceContractLines.GetById(item.NºContrato);
                Decimal sum = contractInvoiceLines.Sum(x => x.ValorVenda).Value;

                result.Add(new FaturacaoContratosViewModel
                {
                    ContractNo = item.NºContrato,
                    Description = item.Descrição,
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
                    RegisterDate = item.DataPróximaFatura
                });
            }

            return Json(result);
        }

        public JsonResult GenerateInvoice([FromBody] List<FaturacaoContratosViewModel> data)
        {
            DateTime current = DateTime.Now;
            DateTime lastDay = (new DateTime(current.Year, current.Month, 1)).AddMonths(1).AddDays(-1);

            // Delete All lines From "Autorizar Faturação Contratos" & "Linhas Faturação Contrato"
            DBAuthorizeInvoiceContracts.DeleteAll();
            DBInvoiceContractLines.DeleteAll();

            // Cycle for "Contratos" filtered by "Avença Fixa" = SIM && "Arquivado" = NAO
            List<Contratos> contractList = DBContracts.GetAllFixedAndArquived(true, false);
            foreach (var item in contractList)
            {
                // Cycle for "Linha Contratos" filtered by "Tipo Contrato", "Nº Contrato", "Versão" = Cycle Item, "Faturavel" = SIM, ordered by "Nº Contrato", "Grupo Fatura"
                List<LinhasContratos> contractLinesList = DBContractLines.GetAllByNoTypeVersion(item.NºContrato, item.TipoContrato, item.NºVersão);
                contractLinesList.OrderBy(x => x.NºContrato).ThenBy(y => y.GrupoFatura);

                String ContractNoDuplicate = "";
                int InvoiceGroupDuplicate = -1;

                foreach (var line in contractLinesList)
                {
                    if (ContractNoDuplicate != line.NºContrato || InvoiceGroupDuplicate != line.GrupoFatura)
                    {
                        ContractNoDuplicate = line.NºContrato;
                        InvoiceGroupDuplicate = line.GrupoFatura.Value;

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

                        List<NAVSalesInvoiceLinesViewModel> salesList = DBNAV2017SalesInvoiceLine.GetSalesInvoiceLines(_config.NAVDatabaseName, _config.NAVCompanyName, item.NºContrato, item.DataInicial.ToString(), item.DataExpiração.Value.ToString());
                        Decimal invoicePeriod = salesList.Sum(x => x.Amount);

                        List<NAVSalesCrMemoLinesViewModel> crMemo = DBNAV2017SalesCrMemo.GetSalesCrMemoLines(_config.NAVDatabaseName, _config.NAVCompanyName, item.NºContrato, item.DataInicial.ToString(), item.DataExpiração.Value.ToString());
                        Decimal creditPeriod = crMemo.Sum(x => x.Amount);

                        AutorizarFaturaçãoContratos newInvoiceContract = new AutorizarFaturaçãoContratos
                        {
                            NºContrato = item.NºContrato,
                            GrupoFatura = line.GrupoFatura.Value,
                            Descrição = item.Descrição,
                            NºCliente = item.NºCliente,
                            CódigoRegião = item.CódigoRegião,
                            CódigoÁreaFuncional = item.CódigoÁreaFuncional,
                            CódigoCentroResponsabilidade = item.CódigoCentroResponsabilidade,
                            ValorDoContrato = contractVal,
                            ValorFaturado = (invoicePeriod - creditPeriod),
                            ValorPorFaturar = (contractVal - (invoicePeriod - creditPeriod)),
                            DataPróximaFatura = item.PróximaDataFatura,
                            DataDeRegisto = lastDay,
                            Estado = item.Estado,
                            DataHoraCriação = DateTime.Now,
                            UtilizadorCriação = User.Identity.Name
                        };
                        //Create
                        DBAuthorizeInvoiceContracts.Create(newInvoiceContract);
                    }

                    //Create Contract Lines
                    Decimal lineQuantity = 1;
                    if (line.Quantidade != 0)
                    {
                        lineQuantity = line.Quantidade.Value;
                    }
                    switch (item.PeríodoFatura)
                    {
                        case 1:
                            lineQuantity = lineQuantity * 1;
                            break;
                        case 2:
                            lineQuantity = lineQuantity * 2;
                            break;
                        case 3:
                            lineQuantity = lineQuantity * 3;
                            break;
                        case 4:
                            lineQuantity = lineQuantity * 6;
                            break;
                        case 5:
                            lineQuantity = lineQuantity * 12;
                            break;
                        default:
                            break;
                    }

                    LinhasFaturaçãoContrato newInvoiceLine = new LinhasFaturaçãoContrato
                    {
                        NºContrato = line.NºContrato,
                        GrupoFatura = line.GrupoFatura.Value,
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
                    //Create
                    DBInvoiceContractLines.Create(newInvoiceLine);
                }
            }
            return Json(true);
        }

        public JsonResult CountInvoice([FromBody] List<FaturacaoContratosViewModel> data)
        {
            List<AutorizarFaturaçãoContratos> contractList = DBAuthorizeInvoiceContracts.GetAll();
            List<LinhasFaturaçãoContrato> lineList = DBInvoiceContractLines.GetAll();

            //NAV WsPreInvoice
            

            //WsPreInvoiceLine

            //WsGeneric.fxPostInvoice

            return Json(true);
        }

        #endregion

    }
}