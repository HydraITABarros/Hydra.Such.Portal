using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.ViewModel.Projects;
using Hydra.Such.Data.Logic;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.Contracts;
using Hydra.Such.Data.Logic.Project;
using Microsoft.Extensions.Options;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.NAV;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using Hydra.Such.Data;
using static Hydra.Such.Data.Enumerations;
using System.Net;
using Hydra.Such.Data.Extensions;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Microsoft.AspNetCore.Http;
using System.Text;
using NPOI.HSSF.UserModel;
using Hydra.Such.Data.ViewModel.ProjectDiary;
using Hydra.Such.Data.Logic.ProjectDiary;
using Hydra.Such.Data.Logic.ProjectMovements;
using System.Globalization;
using Hydra.Such.Data.ViewModel.Clients;
using Hydra.Such.Portal.Extensions;
using Hydra.Such.Data.Logic.Approvals;
using Hydra.Such.Data.ViewModel.ProjectView;
using System.ServiceModel;
using Hydra.Such.Data.ViewModel.Contracts;
using Hydra.Such.Data.ViewModel.Encomendas;
using System.Threading;

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class ProjetosController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly GeneralConfigurations _generalConfig;
        public ProjetosController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IHostingEnvironment _hostingEnvironment, IOptions<GeneralConfigurations> appSettingsGeneral)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            this._hostingEnvironment = _hostingEnvironment;
            _generalConfig = appSettingsGeneral.Value;
        }

        public IActionResult Index()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Projetos);
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

        public IActionResult Detalhes(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Projetos);
            Projetos PROJ = DBProjects.GetById(id);

            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.ProjectNo = id == null ? "" : id;
                ViewBag.UPermissions = UPerm;
                ViewBag.reportServerURL = _config.ReportServerURL;
                if (PROJ != null && PROJ.Estado == (EstadoProjecto)2)
                    ViewBag.projectClosed = true;
                else
                {
                    if (UPerm.Update == true)
                        ViewBag.projectClosed = false;
                    else
                        ViewBag.projectClosed = true;
                }

                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult DetalhesProjeto(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Projetos);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.ProjectNo = id == null ? "" : id;
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        public IActionResult Movimentos_List()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.ProjetosListaMovimentos);
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

        public IActionResult FaturasNotas_List(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Projetos);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.ProjectNo = id ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        #region Home
        [HttpPost]
        public JsonResult GetListProjectsByArea([FromBody] JObject requestParams)
        {
            try
            {
                //int AreaId = int.Parse(requestParams["areaid"].ToString());
                Boolean Ended = Boolean.Parse(requestParams["ended"].ToString());

                List<ProjectListItemViewModel> result = new List<ProjectListItemViewModel>();

                if (!Ended)
                {
                    result = DBProjects.GetAllByAreaToList();
                    result.RemoveAll(x => x.Status == EstadoProjecto.Terminado);
                }
                else
                {
                    result = DBProjects.GetAllByEstado((EstadoProjecto)2); //Terminado

                }

                //Apply User Dimensions Validations
                List<AcessosDimensões> CUserDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
                //Regions
                if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.Region).Count() > 0)
                    result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.RegionCode));
                //FunctionalAreas
                if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                    result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.FunctionalAreaCode));
                //ResponsabilityCenter
                if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                    result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.ResponsabilityCenterCode));

                List<NAVClientsViewModel> AllClients = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "");

                result.ForEach(x =>
                {
                    x.StatusDescription = x.Status.HasValue ? x.Status.Value.GetDescription() : "";
                    x.ClientName = !string.IsNullOrEmpty(x.ClientNo) ? AllClients.Where(y => y.No_ == x.ClientNo).FirstOrDefault() != null ? AllClients.Where(y => y.No_ == x.ClientNo).FirstOrDefault().Name : "" : "";
                    x.ClientRegionCode = !string.IsNullOrEmpty(x.ClientNo) ? AllClients.Where(y => y.No_ == x.ClientNo).FirstOrDefault() != null ? AllClients.Where(y => y.No_ == x.ClientNo).FirstOrDefault().RegionCode : "" : "";
                    //x.ClientName = DBNAV2017Clients.GetClientNameByNo(x.ClientNo, _config.NAVDatabaseName, _config.NAVCompanyName);
                });

                return Json(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult ValidatedCreateNewProject()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Projetos);
            if (UPerm != null && UPerm.Create.Value)
                return Json(true);
            else
                return Json(false);
        }

        [HttpPost]
        public JsonResult GetByContract([FromBody] JObject requestParams)
        {
            string contractId = requestParams["contractId"].ToString();

            List<ProjectListItemViewModel> result = string.IsNullOrEmpty(contractId) ? new List<ProjectListItemViewModel>() : DBProjects.GetByContract(contractId);

            result.ForEach(x =>
            {
                if (x.Status.HasValue)
                {
                    x.StatusDescription = x.Status.Value.GetDescription();
                }
                x.ClientName = DBNAV2017Clients.GetClientNameByNo(x.ClientNo, _config.NAVDatabaseName, _config.NAVCompanyName);
            });


            //Apply User Dimensions Validations
            List<AcessosDimensões> CUserDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
            //Regions
            if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.Region).Count() > 0)
                result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.RegionCode));
            //FunctionalAreas
            if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.FunctionalAreaCode));
            //ResponsabilityCenter
            if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.ResponsabilityCenterCode));

            return Json(result);
        }


        //[HttpPost]
        //public JsonResult TESTE([FromBody] JObject requestParams)
        //{
        //    decimal result = decimal.Zero;

        //    result = Math.Round((decimal)1.002, 2, MidpointRounding.AwayFromZero);
        //    result = Math.Round((decimal)1.005, 2, MidpointRounding.AwayFromZero);
        //    result = Math.Round((decimal)1.003, 2, MidpointRounding.AwayFromZero);
        //    result = Math.Round((decimal)1.055, 2, MidpointRounding.AwayFromZero);
        //    result = Math.Round((decimal)1.0352, 2, MidpointRounding.AwayFromZero);

        //    return Json(null);
        //}
        #endregion

        #region Details


        [HttpPost]
        public JsonResult GetProjectDetails([FromBody] ProjectDetailsViewModel data)
        {

            if (data != null)
            {
                Projetos cProject = DBProjects.GetById(data.ProjectNo);

                if (cProject != null)
                {
                    bool EnviarParaAprovacao = false;
                    if (cProject.Estado == 0) //PENDENTE
                    {
                        MovimentosDeAprovação MovAprovacao = DBApprovalMovements.GetAll().Where(x => x.Número == cProject.NºProjeto && x.Tipo == 5).LastOrDefault();

                        if (MovAprovacao != null)
                            if (MovAprovacao.Estado == 3) //REJEITADO
                                EnviarParaAprovacao = true;
                    }

                    bool VerMenu = false;
                    if (cProject.Estado == (EstadoProjecto)1) //ENCOMENDA
                        VerMenu = true;

                    DateTime servDate = DateTime.Now;
                    string monthName = servDate.ToString("MMMM", System.Globalization.CultureInfo.CreateSpecificCulture("pt-PT"));

                    NAVClientsViewModel cli = new NAVClientsViewModel();
                    if (!string.IsNullOrEmpty(cProject.NºCliente))
                        cli = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, cProject.NºCliente);

                    ProjectDetailsViewModel result = new ProjectDetailsViewModel()
                    {
                        ProjectNo = cProject.NºProjeto,
                        Area = cProject.Área,
                        Description = cProject.Descrição,
                        ClientNo = cProject.NºCliente,
                        ClientRegionCode = cli != null ? !string.IsNullOrEmpty(cli.RegionCode) ? cli.RegionCode : "" : "",
                        Date = cProject.Data.HasValue ? cProject.Data.Value.ToString("yyyy-MM-dd") : "",
                        Status = cProject.Estado,
                        RegionCode = cProject.CódigoRegião,
                        FunctionalAreaCode = cProject.CódigoÁreaFuncional,
                        ResponsabilityCenterCode = cProject.CódigoCentroResponsabilidade,
                        Billable = cProject.Faturável,
                        ContractNo = cProject.NºContrato,
                        ShippingAddressCode = cProject.CódEndereçoEnvio,
                        ShippingName = cProject.EnvioANome,
                        ShippingAddress = cProject.EnvioAEndereço,
                        ShippingPostalCode = cProject.EnvioACódPostal,
                        ShippingLocality = cProject.EnvioALocalidade,
                        ShippingContact = cProject.EnvioAContato,
                        ProjectTypeCode = cProject.CódTipoProjeto,
                        OurProposal = cProject.NossaProposta,
                        ServiceObjectCode = cProject.CódObjetoServiço,
                        CommitmentCode = cProject.NºCompromisso,
                        AccountWorkGroup = cProject.GrupoContabObra,
                        GroupContabProjectType = cProject.TipoGrupoContabProjeto,
                        GroupContabOMProjectType = cProject.TipoGrupoContabOmProjeto,
                        ClientRequest = cProject.PedidoDoCliente,
                        RequestDate = cProject.DataDoPedido.HasValue ? cProject.DataDoPedido.Value.ToString("yyyy-MM-dd") : "",
                        RequestValidity = cProject.ValidadeDoPedido,
                        DetailedDescription = cProject.DescriçãoDetalhada,
                        ProjectCategory = cProject.CategoriaProjeto,
                        BudgetContractNo = cProject.NºContratoOrçamento,
                        InternalProject = cProject.ProjetoInterno,
                        ProjectLeader = cProject.ChefeProjeto,
                        ProjectResponsible = cProject.ResponsávelProjeto,
                        EnviarParaAprovacao = EnviarParaAprovacao,
                        VerMenu = VerMenu,
                        Utilizador = User.Identity.Name,
                        NameDB = _config.NAVDatabaseName,
                        CompanyName = _config.NAVCompanyName,
                        ObservacoesAutorizarFaturacao = "",
                        FaturaPrecosIvaIncluido = cProject.FaturaPrecosIvaIncluido,
                        FechoAutomatico = cProject.FechoAutomatico,
                        CreateUser = cProject.UtilizadorCriação,
                        CreateDate = cProject.DataHoraCriação,
                        UpdateUser = cProject.UtilizadorModificação,
                        UpdateDate = cProject.DataHoraModificação,
                        KWPotenciaInstalada = cProject.KWPotenciaInstalada
                    };

                    string TextoFatura = "";
                    if (!string.IsNullOrEmpty(cProject.NºProjeto) && !string.IsNullOrEmpty(cProject.NºContrato))
                    {
                        TextoFatura = DBContractInvoiceText.GetByContractAndProject(cProject.NºContrato, cProject.NºProjeto).FirstOrDefault() != null ? DBContractInvoiceText.GetByContractAndProject(cProject.NºContrato, cProject.NºProjeto).FirstOrDefault().TextoFatura : "";

                        if (!string.IsNullOrEmpty(TextoFatura))
                        {
                            result.ObservacoesAutorizarFaturacao = TextoFatura;
                        }
                    }

                    if (string.IsNullOrEmpty(TextoFatura) && !string.IsNullOrEmpty(cProject.NºContrato))
                    {
                        TextoFatura = DBContracts.GetByIdLastVersion(cProject.NºContrato) != null ? DBContracts.GetByIdLastVersion(cProject.NºContrato).TextoFatura : "";

                        if (!string.IsNullOrEmpty(TextoFatura))
                        {
                            result.ObservacoesAutorizarFaturacao = TextoFatura;
                        }
                    }

                    if (result != null && !string.IsNullOrEmpty(result.ClientNo) && !string.IsNullOrEmpty(result.ShippingAddressCode))
                    {
                        NAVAddressesViewModel SHIP = DBNAV2017ShippingAddresses.GetByClientAndCode(result.ClientNo, result.ShippingAddressCode, _config.NAVDatabaseName, _config.NAVCompanyName);
                        if (SHIP != null)
                        {
                            result.ShippingName = SHIP.Name;
                            result.ShippingAddress = SHIP.Address;
                            result.ShippingPostalCode = SHIP.ZipCode;
                            result.ShippingLocality = SHIP.City;
                            result.ShippingContact = SHIP.Contact;
                        }
                    }
                    else
                    {
                        if (cli != null)
                        {
                            result.ShippingName = !string.IsNullOrEmpty(cli.Name) ? cli.Name : "";
                            result.ShippingAddress = !string.IsNullOrEmpty(cli.Address) ? cli.Address : "";
                            result.ShippingPostalCode = !string.IsNullOrEmpty(cli.PostCode) ? cli.PostCode : "";
                            result.ShippingLocality = !string.IsNullOrEmpty(cli.City) ? cli.City : "";
                            result.ShippingContact = !string.IsNullOrEmpty(cli.PhoneNo) ? cli.PhoneNo : "";
                        }
                    }

                    //Valores Fixos de Contrato
                    result.ValoresFixosContratos = false;
                    if (cProject != null && !string.IsNullOrEmpty(cProject.NºContrato))
                    {
                        Contratos Contract = DBContracts.GetByIdLastVersion(cProject.NºContrato);
                        if (Contract != null && Contract.TipoFaturação.HasValue && Contract.TipoFaturação == 4) //"Mensal+Consumo"
                        //if (Contract != null && Contract.TipoFaturação.HasValue && Contract.TipoFaturação == 2) //"Consumo"
                        {
                            List<LinhasContratos> ContractLines = DBContractLines.GetAllByActiveContract(Contract.NºDeContrato, Contract.NºVersão).Where(x => x.Faturável == true && x.Quantidade > 0).ToList();
                            if (ContractLines != null && ContractLines.Count > 0)
                            {
                                result.ValoresFixosContratos = true;
                            }
                        }
                    }

                    return Json(result);
                }

                ConfigUtilizadores user = DBUserConfigurations.GetById(User.Identity.Name);
                ProjectDetailsViewModel finalr = new ProjectDetailsViewModel();
                if (user.CriarProjetoSemAprovacao == true)
                {
                    finalr.Status = (EstadoProjecto)1; //ENCOMENDA
                    finalr.ProjectLeader = user.EmployeeNo;
                    finalr.ProjectResponsible = user.EmployeeNo;
                }
                else
                {
                    finalr.Status = (EstadoProjecto)0; //PENDENTE
                    finalr.ProjectLeader = user.EmployeeNo;
                }

                return Json(finalr);
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult GetListContractsLines([FromBody] ProjectDetailsViewModel data)
        {
            if (data != null && !string.IsNullOrEmpty(data.ProjectNo))
            {
                Projetos Project = DBProjects.GetById(data.ProjectNo);

                if (Project != null && !string.IsNullOrEmpty(Project.NºContrato))
                {
                    Contratos Contract = DBContracts.GetByIdLastVersion(Project.NºContrato);
                    if (Contract != null && Contract.TipoFaturação.HasValue && Contract.TipoFaturação == 4) //"Mensal+Consumo"
                    {
                        List<LinhasContratos> ContractLines = DBContractLines.GetAllByActiveContract(Contract.NºDeContrato, Contract.NºVersão).Where(x => x.Faturável == true && x.Quantidade > 0).ToList();
                        if (ContractLines != null && ContractLines.Count > 0)
                        {
                            List<ContractLineViewModel> result = new List<ContractLineViewModel>();

                            ContractLines.ForEach(x => result.Add(DBContractLines.ParseToViewModel(x)));

                            List<NAVClientsViewModel> AllClients = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "");
                            List<EnumData> AllStatus = EnumerablesFixed.ContractALLStatus;
                            List<EnumData> AllContractBillingTypes = EnumerablesFixed.ContractBillingTypes;

                            result.ForEach(x =>
                            {
                                x.ContratoClienteCode = !string.IsNullOrEmpty(Contract.NºCliente) ? Contract.NºCliente : "";
                                x.ContratoClienteNome = !string.IsNullOrEmpty(Contract.NºCliente) ? AllClients.Where(y => y.No_ == Contract.NºCliente) != null ? AllClients.Where(y => y.No_ == Contract.NºCliente).FirstOrDefault().Name : "" : "";

                                x.ContractoEstado = Contract.Estado != null ? AllStatus.Where(y => y.Id == Contract.Estado) != null ? AllStatus.Where(y => y.Id == Contract.Estado).FirstOrDefault().Value : "" : "";
                                x.ContractEndereco = !string.IsNullOrEmpty(Contract.EnvioAEndereço) ? Contract.EnvioAEndereço : "";
                                x.ContratoCodigoPostal = !string.IsNullOrEmpty(Contract.EnvioACódPostal) ? Contract.EnvioACódPostal : "";
                                x.ContratoTipo = Contract.TipoContrato != null ? Contract.TipoContrato.ToString() : "";
                                x.ContratoAvencaFixa = Contract.ContratoAvençaFixa.HasValue ? Contract.ContratoAvençaFixa == true ? "Sim" : "Não" : "Não";
                                x.ContratoDataExpiracao = Contract.DataExpiração.HasValue ? Convert.ToDateTime(Contract.DataExpiração).ToShortDateString() : "";
                                x.ContratoTipoFaturacao = Contract.TipoFaturação != null ? AllContractBillingTypes.Where(y => y.Id == Contract.TipoFaturação) != null ? AllContractBillingTypes.Where(y => y.Id == Contract.TipoFaturação).FirstOrDefault().Value : "" : "";

                                x.ContratoTipo = !string.IsNullOrEmpty(x.ContratoTipo) ? x.ContratoTipo == "1" ? "Oportunidade" : x.ContratoTipo == "2" ? "Proposta" : x.ContratoTipo == "3" ? "Contrato" : "" : "";
                            });

                            return Json(result.OrderBy(x => x.ContractNo).ThenBy(y => y.VersionNo).ThenBy(z => z.LineNo));
                        }
                    }
                }
            }
            return null;
        }

        [HttpPost]
        public JsonResult FaturarValoresFixosContratos([FromBody] List<ContractLineViewModel> Linhas)
        {
            ErrorHandler result = new ErrorHandler();
            result.eReasonCode = 1;
            result.eMessage = "Linhas do Contrato Faturadas com sucesso.";
            string message = string.Empty;

            try
            {
                if (Linhas != null && Linhas.Count > 0)
                {
                    //List<NAVResourcesViewModel> AllResources = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, "", "", 0, "").ToList();
                    List<ProjectDiaryViewModel> dp = new List<ProjectDiaryViewModel>();

                    Linhas.ForEach(line =>
                    {
                        //NAVResourcesViewModel Resource = AllResources.Where(x => x.Code == line.Code).FirstOrDefault();
                        ProjectDiaryViewModel dpLine = new ProjectDiaryViewModel();

                        dpLine.ProjectNo = line.ProjectNo;
                        dpLine.Date = DateTime.Now.ToShortDateString();
                        dpLine.MovementType = 1;
                        dpLine.Type = line.Type;
                        dpLine.Code = line.Code;
                        dpLine.Description = line.Description;
                        dpLine.Quantity = line.Quantity.HasValue ? Math.Round((decimal)line.Quantity, 2, MidpointRounding.AwayFromZero) : decimal.Zero;
                        dpLine.MeasurementUnitCode = line.CodeMeasureUnit;
                        dpLine.LocationCode = "";
                        dpLine.ProjectContabGroup = "PROJETO";
                        dpLine.RegionCode = line.CodeRegion;
                        dpLine.FunctionalAreaCode = line.CodeFunctionalArea;
                        dpLine.ResponsabilityCenterCode = line.CodeResponsabilityCenter;
                        dpLine.User = User.Identity.Name;
                        //dpLine.UnitCost = Resource.UnitCost; //???
                        //dpLine.TotalCost = line.Quantity * Resource.UnitCost; //???
                        dpLine.UnitPrice = line.UnitPrice.HasValue ? Math.Round((decimal)line.UnitPrice, 4, MidpointRounding.AwayFromZero) : decimal.Zero;
                        dpLine.TotalPrice = line.Quantity.HasValue && line.UnitPrice.HasValue ? Math.Round((decimal)(line.Quantity * line.UnitPrice), 2, MidpointRounding.AwayFromZero) : decimal.Zero;
                        dpLine.Billable = line.Billable;
                        dpLine.ResidueGuideNo = "";
                        dpLine.ExternalGuideNo = "";
                        dpLine.InvoiceToClientNo = ""; //???
                        //dpLine.MealType = 0; //???
                        dpLine.ServiceGroupCode = "";
                        dpLine.ConsumptionDate = DateTime.Now.ToShortDateString();
                        dpLine.CreateDate = DateTime.Now;
                        dpLine.CreateUser = User.Identity.Name;
                        dpLine.Registered = false;
                        dpLine.Billed = false;
                        dpLine.ServiceClientCode = line.ServiceClientNo;
                        dpLine.PreRegistered = false;
                        dpLine.Coin = ""; // ???

                        dp.Add(dpLine);
                    });

                    //SET INTEGRATED IN DB
                    if (dp != null)
                    {
                        dp.RemoveAll(x => x.Quantity == null || x.Quantity == 0);

                        bool hasItemsWithoutMealType = dp.Any(x => x.FunctionalAreaCode.StartsWith("5") && (!x.MealType.HasValue || x.MealType == 0));

                        bool hasItemsWithoutDimensions = dp.Any(x => string.IsNullOrEmpty(x.RegionCode) ||
                                                                    string.IsNullOrEmpty(x.FunctionalAreaCode) ||
                                                                    string.IsNullOrEmpty(x.ResponsabilityCenterCode));

                        if (hasItemsWithoutMealType)
                        {
                            result.eReasonCode = 2;
                            result.eMessage = "Existem linhas inválidas: o campo Tipo de Refeição é de preenchimento obrigatório para a área da Alimentação.";
                        }
                        else
                        {
                            if (hasItemsWithoutDimensions)
                            {
                                result.eReasonCode = 2;
                                result.eMessage = "Existem linhas inválidas: a Região, Área Funcional e Centro de Responsabilidade são de preenchimento obrigatórios.";
                            }
                            else
                            {
                                ConfiguracaoParametros Parametro = DBConfiguracaoParametros.GetById(13);
                                dp.ForEach(x =>
                                {
                                    if (x != null && Parametro != null && Convert.ToDateTime(Parametro.Valor) > Convert.ToDateTime(x.Date))
                                    {
                                        result.eReasonCode = 6;
                                        result.eMessage = "Não é possivel Faturar, por existir pelo menos uma linha onde o campo Data é a inferior á data " + Convert.ToDateTime(Parametro.Valor).ToShortDateString();
                                    }
                                });
                                if (result.eReasonCode == 6)
                                    return Json(result);

                                Guid transactID = Guid.NewGuid();
                                try
                                {
                                    //Create Lines in NAV
                                    Task<WSCreateProjectDiaryLine.CreateMultiple_Result> TCreateNavDiaryLine = WSProjectDiaryLine.CreateNavDiaryLines(dp, transactID, _configws);
                                    TCreateNavDiaryLine.Wait();
                                    try
                                    {
                                        Task<WSGenericCodeUnit.FxPostJobJrnlLines_Result> TRegisterNavDiaryLine = WSProjectDiaryLine.RegsiterNavDiaryLines(transactID, _configws);
                                        TRegisterNavDiaryLine.Wait();
                                    }
                                    catch (Exception e)
                                    {
                                        WSProjectDiaryLine.DeleteNavDiaryLines(transactID, _configws);
                                        result.eReasonCode = 2;
                                        result.eMessage = "Não foi possivel Faturar: " + e.Message;
                                        return Json(result);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    result.eReasonCode = 2;
                                    result.eMessage = "Não foi possivel Faturar: " + ex.Message;
                                    return Json(result);
                                }

                                dp.ForEach(x =>
                                {
                                    if (x.Code != null)
                                    {
                                        MovimentosDeProjeto ProjectMovement = new MovimentosDeProjeto()
                                        {
                                            //NºLinha = x.NºLinha,
                                            NºProjeto = x.ProjectNo,
                                            Data = !string.IsNullOrEmpty(x.Date) ? Convert.ToDateTime(x.Date) : DateTime.Now,
                                            TipoMovimento = 1, //CONSUMO
                                            Tipo = x.Type,
                                            Código = x.Code,
                                            Descrição = x.Description,
                                            Quantidade = x.Quantity.HasValue ? Math.Round((decimal)x.Quantity, 2, MidpointRounding.AwayFromZero) : decimal.Zero,
                                            CódUnidadeMedida = x.MeasurementUnitCode,
                                            CódLocalização = x.LocationCode,
                                            GrupoContabProjeto = x.ProjectContabGroup,
                                            CódigoRegião = x.RegionCode,
                                            CódigoÁreaFuncional = x.FunctionalAreaCode,
                                            CódigoCentroResponsabilidade = x.ResponsabilityCenterCode,
                                            Utilizador = User.Identity.Name,
                                            CustoUnitário = x.UnitCost.HasValue ? Math.Round((decimal)x.UnitCost, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                                            CustoTotal = x.Quantity.HasValue && x.UnitCost.HasValue ? Math.Round((decimal)(x.Quantity * x.UnitCost), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //x.CustoTotal,
                                            PreçoUnitário = x.UnitPrice.HasValue ? Math.Round((decimal)x.UnitPrice, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                                            PreçoTotal = x.Quantity.HasValue && x.UnitPrice.HasValue ? Math.Round((decimal)(x.Quantity * x.UnitPrice), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //x.PreçoTotal,
                                            Faturável = x.Billable,
                                            Registado = true,
                                            Faturada = false,
                                            FaturaANºCliente = x.InvoiceToClientNo,
                                            Moeda = x.Coin,
                                            ValorUnitárioAFaturar = x.UnitValueToInvoice,
                                            TipoRefeição = x.MealType,
                                            CódGrupoServiço = x.ServiceGroupCode,
                                            NºGuiaResíduos = x.ResidueGuideNo,
                                            NºGuiaExterna = x.ExternalGuideNo,
                                            DataConsumo = !string.IsNullOrEmpty(x.ConsumptionDate) ? Convert.ToDateTime(x.ConsumptionDate) : DateTime.Now,
                                            CódServiçoCliente = x.ServiceClientCode,
                                            UtilizadorCriação = User.Identity.Name,
                                            DataHoraCriação = DateTime.Now,
                                            FaturaçãoAutorizada = false,
                                            FaturaçãoAutorizada2 = false,
                                            NºDocumento = "ES_" + x.ProjectNo,
                                            CriarMovNav2017 = false
                                        };

                                        DBProjectMovements.Create(ProjectMovement);
                                    }
                                });
                            }
                        }
                    }
                    else
                    {
                        result.eReasonCode = 2;
                        result.eMessage = "Não existem linhas para Faturar.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.eReasonCode = 222;
                result.eMessage = "Ocorreu um erro.";
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult ValidateNumeration([FromBody] ProjectDetailsViewModel data)
        {
            //Get Project Numeration
            Configuração Cfg = DBConfigurations.GetById(1);
            int ProjectNumerationConfigurationId = Cfg.NumeraçãoProjetos.Value;
            string result = string.Empty;
            bool ok = false;

            ConfiguraçãoNumerações CfgNumeration = DBNumerationConfigurations.GetById(ProjectNumerationConfigurationId);

            //Validate if ProjectNo is valid
            if (!(data.ProjectNo == "" || data.ProjectNo == null) && !CfgNumeration.Manual.Value)
            {
                //return Json("A numeração configurada para projetos não permite inserção manual.");
                result = "A numeração configurada para projetos não permite inserção manual.";
            }
            else if (data.ProjectNo == "" && !CfgNumeration.Automático.Value)
            {
                //return Json("É obrigatório inserir o Nº de Projeto.");
                result = "É obrigatório inserir o Nº de Projeto.";
            }

            //Se o estado atual for deferente da Base Dados não grava
            if (data != null && !string.IsNullOrEmpty(data.ProjectNo))
            {
                Projetos DBProj = DBProjects.GetById(data.ProjectNo);
                if (DBProj != null && DBProj.Estado != data.Status)
                {
                    result = "Não é possivel guardar o Projeto, atualize a página para oter a última versão.";
                }
            }

            //Verificar se existem validadores para o projeto
            if (string.IsNullOrEmpty(result))
            {
                if (data.Status != (EstadoProjecto)1) //ENCOMENDA
                {
                    List<ConfiguraçãoAprovações> ApprovalConfigurations = DBApprovalConfigurations.GetByTypeAreaValueDateAndDimensions(5, data.FunctionalAreaCode, data.ResponsabilityCenterCode, data.RegionCode, 0, DateTime.Now);

                    int lowLevel = ApprovalConfigurations.Where(x => x.NívelAprovação.HasValue).OrderBy(x => x.NívelAprovação.Value).Select(x => x.NívelAprovação.Value).FirstOrDefault();
                    ApprovalConfigurations.RemoveAll(x => x.NívelAprovação != lowLevel);

                    if (ApprovalConfigurations != null && ApprovalConfigurations.Count > 0)
                    {
                        List<string> UsersToNotify = new List<string>();
                        var approvalConfiguration = ApprovalConfigurations[0];
                        if (approvalConfiguration.UtilizadorAprovação != "" && approvalConfiguration.UtilizadorAprovação != null)
                        {
                            if (approvalConfiguration.UtilizadorAprovação.ToLower() == User.Identity.Name.ToLower())
                                approvalConfiguration.UtilizadorAprovação = DBUserConfigurations.GetById(User.Identity.Name).SuperiorHierarquico;

                            if (approvalConfiguration.UtilizadorAprovação != "" && approvalConfiguration.UtilizadorAprovação != null && approvalConfiguration.UtilizadorAprovação.ToLower() != User.Identity.Name.ToLower())
                            {
                                ok = true;
                            }
                        }
                        else if (approvalConfiguration.GrupoAprovação.HasValue)
                        {
                            List<string> GUsers = DBApprovalUserGroup.GetAllFromGroup(approvalConfiguration.GrupoAprovação.Value);
                            if (GUsers.Exists(x => x.ToLower() == User.Identity.Name.ToLower()))
                            {
                                GUsers.RemoveAll(x => x.ToLower() == User.Identity.Name.ToLower());

                                string SH = DBUserConfigurations.GetById(User.Identity.Name).SuperiorHierarquico;
                                if (!string.IsNullOrEmpty(SH))
                                    GUsers.Add(SH);
                            }
                            GUsers = GUsers.Distinct().ToList();

                            if (GUsers.Count > 0)
                                ok = true;
                        }
                    }

                    if (ok == false)
                        result = "Não é possivel criar o projeto, por não existirem validadores para dimensões pretendidas.";
                }
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetAddressData([FromBody] string AddressCode)
        {
            NAVAddressesViewModel result = DBNAV2017ShippingAddresses.GetByCode(AddressCode, _config.NAVDatabaseName, _config.NAVCompanyName);

            return Json(result);
        }

        public IActionResult GoFaturasNotas([FromBody] string ProjectNo)
        {
            return RedirectToAction("FaturasNotas_List", "Projetos", new { id = ProjectNo });
        }

        [HttpPost]
        public JsonResult VerificarPDF([FromBody] string pdf)
        {
            string pdfPath = _generalConfig.FileUploadFolder + "FaturasClientes\\" + pdf.Replace("@", "\\");

            if (System.IO.File.Exists(pdfPath))
                return Json(true);
            else
                return Json(false);
        }

        [Route("Projetos/LoadPDF/{pdf}")]
        public ActionResult LoadPDF(string pdf)
        {
            string pdfPath = _generalConfig.FileUploadFolder + "FaturasClientes\\" + pdf.Replace("@", "\\");

            if (System.IO.File.Exists(pdfPath))
            {
                var stream = new FileStream(pdfPath, FileMode.Open, FileAccess.Read);
                var result = new FileStreamResult(stream, "application/pdf");

                return result;
            }
            else
                return null;
        }


        //eReason = 1 -> Sucess
        //eReason = 2 -> Error creating Project on Databse 
        //eReason = 3 -> Error creating Project on NAV 
        //eReason = 4 -> Unknow Error 
        //eReason = 5 -> Error getting Numeration 
        [HttpPost]
        public JsonResult CreateProject([FromBody] ProjectDetailsViewModel data)
        {
            try
            {
                if (data != null)
                {
                    //Get Project Numeration
                    bool autoGenId = false;
                    Configuração Configs = DBConfigurations.GetById(1);
                    int ProjectNumerationConfigurationId = Configs.NumeraçãoProjetos.Value;
                    string projNoAuto = "";
                    if (data.ProjectNo == "" || data.ProjectNo == null)
                    {
                        autoGenId = true;
                        projNoAuto = DBNumerationConfigurations.GetNextNumeration(ProjectNumerationConfigurationId, autoGenId, false);
                        data.ProjectNo = projNoAuto;
                    }

                    if (data.Status == (EstadoProjecto)1) //ENCOMENDA
                    {
                        if (data.ProjectNo != null)
                        {
                            Projetos cProject = new Projetos()
                            {
                                NºProjeto = data.ProjectNo,
                                Área = data.Area,
                                Descrição = data.Description,
                                NºCliente = data.ClientNo,
                                Data = data.Date != "" && data.Date != null ? DateTime.Parse(data.Date) : (DateTime?)null,
                                Estado = data.Status,
                                //MovimentosVenda = data
                                CódigoRegião = data.RegionCode,
                                CódigoÁreaFuncional = data.FunctionalAreaCode,
                                CódigoCentroResponsabilidade = data.ResponsabilityCenterCode,
                                Faturável = data.Billable,
                                NºContrato = data.ContractNo,
                                CódEndereçoEnvio = data.ShippingAddressCode,
                                EnvioANome = data.ShippingName,
                                EnvioAEndereço = data.ShippingAddress,
                                EnvioACódPostal = data.ShippingPostalCode,
                                EnvioALocalidade = data.ShippingLocality,
                                EnvioAContato = data.ShippingContact,
                                CódTipoProjeto = data.ProjectTypeCode,
                                NossaProposta = data.OurProposal,
                                CódObjetoServiço = data.ServiceObjectCode,
                                NºCompromisso = data.CommitmentCode,
                                GrupoContabObra = "PROJETO",
                                TipoGrupoContabProjeto = data.GroupContabProjectType,
                                TipoGrupoContabOmProjeto = data.GroupContabOMProjectType,
                                PedidoDoCliente = data.ClientRequest,
                                DataDoPedido = data.RequestDate != "" && data.RequestDate != null ? DateTime.Parse(data.RequestDate) : (DateTime?)null,
                                ValidadeDoPedido = data.RequestValidity,
                                DescriçãoDetalhada = data.DetailedDescription,
                                CategoriaProjeto = data.ProjectCategory,
                                NºContratoOrçamento = data.BudgetContractNo,
                                ProjetoInterno = data.InternalProject,
                                ChefeProjeto = data.ProjectLeader,
                                ResponsávelProjeto = data.ProjectResponsible,
                                DataHoraCriação = DateTime.Now,
                                UtilizadorCriação = User.Identity.Name,
                                DataHoraModificação = DateTime.Now,
                                UtilizadorModificação = User.Identity.Name,
                                FaturaPrecosIvaIncluido = data.FaturaPrecosIvaIncluido,
                                FechoAutomatico = data.FechoAutomatico,
                                KWPotenciaInstalada = data.KWPotenciaInstalada
                            };

                            //Create Project on NAV
                            data.Visivel = true;
                            Task<WSCreateNAVProject.Create_Result> TCreateNavProj = WSProject.CreateNavProject(cProject.ParseToViewModel(), _configws);
                            try
                            {
                                TCreateNavProj.Wait();
                            }
                            catch (Exception ex)
                            {
                                data.eReasonCode = 3;
                                data.eMessage = "Ocorreu um erro ao criar o projeto no NAV.";
                            }

                            if (!TCreateNavProj.IsCompletedSuccessfully)
                            {
                                data.eReasonCode = 3;
                                data.eMessage = "Ocorreu um erro ao criar o projeto no NAV.";

                                if (TCreateNavProj.Exception != null)
                                    data.eMessages.Add(new TraceInformation(TraceType.Exception, TCreateNavProj.Exception.Message));

                                if (TCreateNavProj.Exception.InnerException != null)
                                    data.eMessages.Add(new TraceInformation(TraceType.Exception, TCreateNavProj.Exception.InnerException.ToString()));
                            }
                            else
                            {
                                //Create Project On Database eSUCH
                                if (DBProjects.Create(cProject) != null)
                                {
                                    //Update Last Numeration Used
                                    if (autoGenId)
                                    {
                                        ConfiguraçãoNumerações ConfigNumerations = DBNumerationConfigurations.GetById(ProjectNumerationConfigurationId);
                                        ConfigNumerations.ÚltimoNºUsado = data.ProjectNo;
                                        ConfigNumerations.UtilizadorModificação = User.Identity.Name;
                                        DBNumerationConfigurations.Update(ConfigNumerations);
                                    }
                                    data.eReasonCode = 1;
                                }
                                else
                                {
                                    data.eReasonCode = 3;
                                    data.eMessage = "Ocorreu um erro ao criar o projeto no eSUCH.";
                                }
                            }
                        }
                        else
                        {
                            data.eReasonCode = 5;
                            data.eMessage = "A numeração configurada não é compativel com a inserida.";
                        }

                        if (data.eReasonCode != 1 && projNoAuto != "")
                        {
                            data.ProjectNo = "";
                        }
                    }
                    else
                    {
                        if (data.Status == (EstadoProjecto)0) //PENDENTE
                        {
                            if (data.ProjectNo != null)
                            {
                                ErrorHandler resultApprovalMovement = new ErrorHandler();
                                Projetos cProject = new Projetos()
                                {
                                    NºProjeto = data.ProjectNo,
                                    Área = data.Area,
                                    Descrição = data.Description,
                                    NºCliente = data.ClientNo,
                                    Data = data.Date != "" && data.Date != null ? DateTime.Parse(data.Date) : (DateTime?)null,
                                    Estado = data.Status,
                                    //MovimentosVenda = data
                                    CódigoRegião = data.RegionCode,
                                    CódigoÁreaFuncional = data.FunctionalAreaCode,
                                    CódigoCentroResponsabilidade = data.ResponsabilityCenterCode,
                                    Faturável = data.Billable,
                                    NºContrato = data.ContractNo,
                                    CódEndereçoEnvio = data.ShippingAddressCode,
                                    EnvioANome = data.ShippingName,
                                    EnvioAEndereço = data.ShippingAddress,
                                    EnvioACódPostal = data.ShippingPostalCode,
                                    EnvioALocalidade = data.ShippingLocality,
                                    EnvioAContato = data.ShippingContact,
                                    CódTipoProjeto = data.ProjectTypeCode,
                                    NossaProposta = data.OurProposal,
                                    CódObjetoServiço = data.ServiceObjectCode,
                                    NºCompromisso = data.CommitmentCode,
                                    GrupoContabObra = "PROJETO",
                                    TipoGrupoContabProjeto = data.GroupContabProjectType,
                                    TipoGrupoContabOmProjeto = data.GroupContabOMProjectType,
                                    PedidoDoCliente = data.ClientRequest,
                                    DataDoPedido = data.RequestDate != "" && data.RequestDate != null ? DateTime.Parse(data.RequestDate) : (DateTime?)null,
                                    ValidadeDoPedido = data.RequestValidity,
                                    DescriçãoDetalhada = data.DetailedDescription,
                                    CategoriaProjeto = data.ProjectCategory,
                                    NºContratoOrçamento = data.BudgetContractNo,
                                    ProjetoInterno = data.InternalProject,
                                    ChefeProjeto = data.ProjectLeader,
                                    ResponsávelProjeto = data.ProjectResponsible,
                                    DataHoraCriação = DateTime.Now,
                                    UtilizadorCriação = User.Identity.Name,
                                    DataHoraModificação = DateTime.Now,
                                    UtilizadorModificação = User.Identity.Name,
                                    FaturaPrecosIvaIncluido = data.FaturaPrecosIvaIncluido,
                                    FechoAutomatico = data.FechoAutomatico,
                                    KWPotenciaInstalada = data.KWPotenciaInstalada
                                };

                                //Create Project On Database
                                cProject = DBProjects.Create(cProject);

                                if (cProject == null)
                                {
                                    data.eReasonCode = 3;
                                    data.eMessage = "Ocorreu um erro ao criar o projeto no portal.";
                                }
                                else
                                {

                                    resultApprovalMovement = ApprovalMovementsManager.StartApprovalMovement_Projetos(5, data.FunctionalAreaCode, data.ResponsabilityCenterCode, data.RegionCode, 0, data.ProjectNo, User.Identity.Name);

                                    if (autoGenId)
                                    {
                                        ConfiguraçãoNumerações ConfigNumerations = DBNumerationConfigurations.GetById(ProjectNumerationConfigurationId);
                                        ConfigNumerations.ÚltimoNºUsado = data.ProjectNo;
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
                            if (data.eReasonCode != 1 && projNoAuto != "")
                            {
                                data.ProjectNo = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.eReasonCode = 4;
                data.eMessage = "Ocorreu um erro ao criar o projeto";
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult UpdateProject([FromBody] ProjectDetailsViewModel data)
        {
            try
            {
                if (data != null)
                {
                    if (string.IsNullOrEmpty(data.CreateUser))
                    {
                        data.CreateUser = User.Identity.Name;
                        data.CreateDate = DateTime.Now;
                        data.AccountWorkGroup = "PROJETO";
                    };

                    if (data.Status == (EstadoProjecto)1) //ENCOMENDA
                    {
                        //Read NAV Project Key
                        Task<WSCreateNAVProject.Read_Result> TReadNavProj = WSProject.GetNavProject(data.ProjectNo, _configws);
                        try
                        {
                            TReadNavProj.Wait();
                        }
                        catch (Exception ex)
                        {
                            data.eReasonCode = 3;
                            data.eMessage = "Erro ao atualizar: Não foi possivel obter o Projeto a partir do NAV.";
                            return Json(data);
                        }

                        if (TReadNavProj.IsCompletedSuccessfully)
                        {
                            if (TReadNavProj.Result.WSJob == null)
                            {
                                data.eReasonCode = 3;
                                data.eMessage = "Erro ao atualizar: O projeto não existe no NAV";
                                return Json(data);
                            }
                            else
                            {
                                Projetos OLD_Proj = DBProjects.GetById(data.ProjectNo);

                                if (OLD_Proj.Descrição != data.Description || OLD_Proj.NºCliente != data.ClientNo || OLD_Proj.Estado != data.Status ||
                                    OLD_Proj.CódigoRegião != data.RegionCode || OLD_Proj.CódigoÁreaFuncional != data.FunctionalAreaCode || OLD_Proj.CódigoCentroResponsabilidade != data.ResponsabilityCenterCode)
                                {
                                    //Update Project on NAV
                                    Task<WSCreateNAVProject.Update_Result> TUpdateNavProj = WSProject.UpdateNavProject(TReadNavProj.Result.WSJob.Key, data, _configws);
                                    bool statusL = true;
                                    try
                                    {
                                        TUpdateNavProj.Wait();
                                    }
                                    catch (Exception ex)
                                    {
                                        data.eReasonCode = 3;
                                        if (ex.InnerException.Message == "You cannot change Bill-to Customer No. because one or more entries are associated with this Job.")
                                            data.eMessage = "Não é possivel alterar o Cliente, pois o Projeto já contêm Movimentos inseridos.";
                                        else
                                            data.eMessage = ex.InnerException.Message;
                                        statusL = false;
                                        return Json(data);
                                    }

                                    if (!TUpdateNavProj.IsCompletedSuccessfully || statusL == false)
                                    {
                                        if (OLD_Proj != null && OLD_Proj.NºCliente != data.ClientNo)
                                        {
                                            data.eReasonCode = 3;
                                            data.eMessage = "Não é possível alterar o cliente deste projeto.";
                                            return Json(data);
                                        }
                                        else
                                        {
                                            data.eReasonCode = 3;
                                            data.eMessage = "Ocorreu um erro ao atualizar o projeto no NAV.";
                                            return Json(data);
                                        }
                                    }
                                }

                                Projetos cProject = DBProjects.ParseToDB(data);
                                cProject.UtilizadorModificação = User.Identity.Name;
                                cProject.DataHoraModificação = DateTime.Now;

                                if (DBProjects.Update(cProject) != null)
                                {
                                    data.eReasonCode = 1;
                                }
                                else
                                {
                                    data.eReasonCode = 3;
                                    data.eMessage = "Ocorreu um erro ao atualizar o projeto no eSUCH.";
                                    return Json(data);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (data.Status == (EstadoProjecto)0) //PENDENTE
                        {
                            Projetos ProjectDB = DBProjects.GetById(data.ProjectNo);
                            if (ProjectDB.Estado != data.Status)
                            {
                                data.eReasonCode = 3;
                                data.eMessage = "Não é possível Guardar o Projeto pois o mesmo encontra-se num estado diferente na Base de Dados.";
                                return Json(data);
                            }

                            Projetos cProject = DBProjects.ParseToDB(data);
                            cProject.UtilizadorModificação = User.Identity.Name;
                            cProject.DataHoraModificação = DateTime.Now;

                            if (DBProjects.Update(cProject) != null)
                            {
                                data.eReasonCode = 1;
                            }
                            else
                            {
                                data.eReasonCode = 3;
                                data.eMessage = "Ocorreu um erro ao atualizar o projeto no eSUCH.";
                                return Json(data);
                            }
                        }
                        else
                        {
                            data.eReasonCode = 3;
                            data.eMessage = "Não é possivel atualizar o projeto pois o mesmo está terminado.";
                            return Json(data);
                        }
                    }
                }
                else
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Ocorreu um erro: Não foi possivel obter os dados.";
                    return Json(data);
                }
            }
            catch (Exception ex)
            {
                data.eReasonCode = 4;
                data.eMessage = "Ocorreu um erro ao atualizar o projeto";
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult DeleteProject([FromBody] ProjectDetailsViewModel data)
        {

            if (data != null)
            {
                ErrorHandler result = new ErrorHandler();

                List<DiárioDeProjeto> Movements = DBProjectDiary.GetByProjectNo(data.ProjectNo, User.Identity.Name);
                Movements.RemoveAll(x => !x.Registado.Value);

                if (Movements.Count() > 0)
                {
                    result = new ErrorHandler()
                    {
                        eReasonCode = 6,
                        eMessage = "Já existem movimentos de projeto."
                    };
                    return Json(result);
                }
                else
                {
                    //Update Project on NAV
                    //Read NAV Project Key
                    Task<WSCreateNAVProject.Read_Result> TReadNavProj = WSProject.GetNavProject(data.ProjectNo, _configws);
                    try
                    {
                        TReadNavProj.Wait();
                    }
                    catch (Exception ex)
                    {
                        result = new ErrorHandler()
                        {
                            eReasonCode = 5,
                            eMessage = "Ocorreu um erro ao ler o projeto do NAV."
                        };
                        return Json(result);
                    }

                    if (TReadNavProj.Result.WSJob != null)
                    {
                        if (TReadNavProj.IsCompletedSuccessfully)
                        {
                            Task<WSCreateNAVProject.Delete_Result> TDeleteNavProj = WSProject.DeleteNavProject(TReadNavProj.Result.WSJob.Key, _configws);
                            try
                            {
                                TDeleteNavProj.Wait();

                                if (!TDeleteNavProj.IsCompletedSuccessfully)
                                {
                                    result = new ErrorHandler()
                                    {
                                        eReasonCode = 4,
                                        eMessage = "Não é possivel remover o projeto no nav."
                                    };
                                    return Json(result);
                                }
                                else
                                {
                                    List<MovimentosDeAprovação> AllMovAprovacao = DBApprovalMovements.GetAll().Where(x => x.Número == data.ProjectNo && x.Tipo == 5).ToList();

                                    if (AllMovAprovacao != null & AllMovAprovacao.Count > 0)
                                    {
                                        foreach (MovimentosDeAprovação MovAprovacao in AllMovAprovacao)
                                        {
                                            if (MovAprovacao != null)
                                            {
                                                List<UtilizadoresMovimentosDeAprovação> AllUserMov = DBUserApprovalMovements.GetById(MovAprovacao.NºMovimento);

                                                if (AllUserMov != null && AllUserMov.Count > 0)
                                                {
                                                    foreach (UtilizadoresMovimentosDeAprovação UserMov in AllUserMov)
                                                    {
                                                        if (DBUserApprovalMovements.Delete(UserMov) == false)
                                                        {
                                                            result = new ErrorHandler()
                                                            {
                                                                eReasonCode = 10,
                                                                eMessage = "Ocorreu um erro ao eliminar o Movimento de Aprovação dos Utilizadores."
                                                            };
                                                            return Json(result);
                                                        }
                                                    }
                                                }

                                                if (DBApprovalMovements.Delete(MovAprovacao) == false)
                                                {
                                                    result = new ErrorHandler()
                                                    {
                                                        eReasonCode = 11,
                                                        eMessage = "Ocorreu um erro ao eliminar o Movimento de Aprovação."
                                                    };
                                                    return Json(result);
                                                }
                                            }
                                        }
                                    }

                                    if (DBProjects.Delete(data.ProjectNo) == true)
                                    {
                                        result = new ErrorHandler()
                                        {
                                            eReasonCode = 0,
                                            eMessage = "Projeto removido com sucesso."
                                        };
                                        return Json(result);
                                    }
                                    else
                                    {
                                        result = new ErrorHandler()
                                        {
                                            eReasonCode = 8,
                                            eMessage = "Ocorreu um erro ao eliminar o Projecto do eSUCH."
                                        };
                                        return Json(result);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                result = new ErrorHandler()
                                {
                                    eReasonCode = 3,
                                    eMessage = "Não é possivel remover o projeto no nav."
                                };
                                return Json(result);
                            }
                        }
                        else
                        {
                            result = new ErrorHandler()
                            {
                                eReasonCode = 2,
                                eMessage = "Ocorreu um erro ao obter o Projeto do Navision."
                            };
                            return Json(result);
                        }
                    }
                    else
                    {
                        List<MovimentosDeAprovação> AllMovAprovacao = DBApprovalMovements.GetAll().Where(x => x.Número == data.ProjectNo && x.Tipo == 5).ToList();

                        if (AllMovAprovacao != null & AllMovAprovacao.Count > 0)
                        {
                            foreach (MovimentosDeAprovação MovAprovacao in AllMovAprovacao)
                            {
                                if (MovAprovacao != null)
                                {
                                    List<UtilizadoresMovimentosDeAprovação> AllUserMov = DBUserApprovalMovements.GetById(MovAprovacao.NºMovimento);

                                    if (AllUserMov != null && AllUserMov.Count > 0)
                                    {
                                        foreach (UtilizadoresMovimentosDeAprovação UserMov in AllUserMov)
                                        {
                                            if (DBUserApprovalMovements.Delete(UserMov) == false)
                                            {
                                                result = new ErrorHandler()
                                                {
                                                    eReasonCode = 10,
                                                    eMessage = "Ocorreu um erro ao eliminar o Movimento de Aprovação dos Utilizadores."
                                                };
                                                return Json(result);
                                            }
                                        }
                                    }

                                    if (DBApprovalMovements.Delete(MovAprovacao) == false)
                                    {
                                        result = new ErrorHandler()
                                        {
                                            eReasonCode = 11,
                                            eMessage = "Ocorreu um erro ao eliminar o Movimento de Aprovação."
                                        };
                                        return Json(result);
                                    }
                                }
                            }
                        }

                        if (DBProjects.Delete(data.ProjectNo) == true)
                        {
                            result = new ErrorHandler()
                            {
                                eReasonCode = 0,
                                eMessage = "Projeto removido com sucesso."
                            };
                            return Json(result);
                        }
                        else
                        {
                            result = new ErrorHandler()
                            {
                                eReasonCode = 8,
                                eMessage = "Ocorreu um erro ao eliminar o Projecto do eSUCH."
                            };
                            return Json(result);
                        }
                    }

                }
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult TerminarProject([FromBody] ProjectDetailsViewModel data)
        {

            if (data != null)
            {
                ErrorHandler result = new ErrorHandler();

                UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Projetos);
                if (UPerm != null && UPerm.Update == true)
                {
                    //string NoMecanografico = DBUserConfigurations.GetById(User.Identity.Name).EmployeeNo;
                    //if (data.ProjectResponsible == NoMecanografico)
                    //{
                    MovimentosDeAprovação MovAprovacao = DBApprovalMovements.GetAll().Where(x => x.Número == data.ProjectNo && x.Tipo == 5).LastOrDefault();
                    if (MovAprovacao != null && MovAprovacao.Estado == 1)
                    {
                        result = new ErrorHandler()
                        {
                            eReasonCode = 2,
                            eMessage = "O Projeto não pode ser Terminado pois existe movimento de aprovação por aprovar."
                        };
                        return Json(result);
                    }
                    else
                    {
                        try
                        {
                            //Read NAV Project Key
                            Task<WSCreateNAVProject.Read_Result> TReadNavProj = WSProject.GetNavProject(data.ProjectNo, _configws);
                            try
                            {
                                TReadNavProj.Wait();
                            }
                            catch (Exception ex)
                            {
                                result = new ErrorHandler()
                                {
                                    eReasonCode = 3,
                                    eMessage = "Ocorreu um erro ao ler o projeto do NAV."
                                };
                                return Json(result);
                            }

                            if (TReadNavProj.IsCompletedSuccessfully)
                            {
                                if (TReadNavProj.Result.WSJob == null)
                                {
                                    result = new ErrorHandler()
                                    {
                                        eReasonCode = 4,
                                        eMessage = "Erro ao atualizar: O projeto não existe no NAV."
                                    };
                                    return Json(result);
                                }
                                else
                                {
                                    data.Status = (EstadoProjecto)2; //TERMINADO

                                    //Update Project on NAV
                                    Task<WSCreateNAVProject.Update_Result> TUpdateNavProj = WSProject.UpdateNavProject(TReadNavProj.Result.WSJob.Key, data, _configws);
                                    bool statusL = true;
                                    try
                                    {
                                        TUpdateNavProj.Wait();
                                    }
                                    catch (Exception ex)
                                    {
                                        result = new ErrorHandler()
                                        {
                                            eReasonCode = 5,
                                            eMessage = ex.InnerException.Message
                                        };
                                        return Json(result);
                                    }

                                    if (!TUpdateNavProj.IsCompletedSuccessfully && statusL)
                                    {
                                        result = new ErrorHandler()
                                        {
                                            eReasonCode = 6,
                                            eMessage = "Ocorreu um erro ao atualizar o projeto no NAV."
                                        };
                                        return Json(result);
                                    }
                                    else
                                    {
                                        Projetos cProject = DBProjects.GetById(data.ProjectNo);

                                        cProject.NºProjeto = data.ProjectNo;
                                        cProject.Descrição = data.Description;
                                        cProject.NºCliente = data.ClientNo;
                                        cProject.Data = data.Date != "" && data.Date != null ? DateTime.Parse(data.Date) : (DateTime?)null;
                                        cProject.Estado = (EstadoProjecto)2; //TERMINADO
                                        cProject.ChefeProjeto = data.ProjectLeader;
                                        cProject.ResponsávelProjeto = data.ProjectResponsible;
                                        cProject.CódObjetoServiço = data.ServiceObjectCode;
                                        cProject.Faturável = data.Billable;
                                        cProject.NºContrato = data.ContractNo;
                                        cProject.NossaProposta = data.OurProposal;
                                        cProject.NºCompromisso = data.CommitmentCode;
                                        cProject.CódigoRegião = data.RegionCode;
                                        cProject.CódigoÁreaFuncional = data.FunctionalAreaCode;
                                        cProject.CódigoCentroResponsabilidade = data.ResponsabilityCenterCode;
                                        cProject.TipoGrupoContabProjeto = data.GroupContabProjectType;
                                        cProject.PedidoDoCliente = data.ClientRequest;
                                        cProject.DataDoPedido = data.RequestDate != "" && data.RequestDate != null ? DateTime.Parse(data.RequestDate) : (DateTime?)null;
                                        cProject.DescriçãoDetalhada = data.DetailedDescription;
                                        cProject.CódEndereçoEnvio = data.ShippingAddressCode;
                                        cProject.EnvioANome = data.ShippingName;
                                        cProject.EnvioAEndereço = data.ShippingAddress;
                                        cProject.EnvioACódPostal = data.ShippingPostalCode;
                                        cProject.EnvioALocalidade = data.ShippingLocality;
                                        cProject.EnvioAContato = data.ShippingContact;
                                        cProject.CódTipoProjeto = data.ProjectTypeCode;
                                        cProject.CategoriaProjeto = data.ProjectCategory;
                                        cProject.NºContratoOrçamento = data.BudgetContractNo;
                                        cProject.ProjetoInterno = data.InternalProject;
                                        cProject.GrupoContabObra = "PROJETO";
                                        cProject.UtilizadorModificação = User.Identity.Name;
                                        cProject.DataHoraModificação = DateTime.Now;
                                        cProject.KWPotenciaInstalada = data.KWPotenciaInstalada;

                                        if (DBProjects.Update(cProject) != null)
                                        {
                                            result = new ErrorHandler()
                                            {
                                                eReasonCode = 0,
                                                eMessage = "Projeto terminado com sucesso."
                                            };
                                            return Json(result);
                                        }
                                        else
                                        {
                                            result = new ErrorHandler()
                                            {
                                                eReasonCode = 7,
                                                eMessage = "Ocorreu um erro ao atualizar o projeto no eSUCH."
                                            };
                                            return Json(result);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            result = new ErrorHandler()
                            {
                                eReasonCode = 8,
                                eMessage = "Ocorreu um erro ao atualizar o projeto."
                            };
                            return Json(result);
                        }
                    }
                }
                else
                {
                    result = new ErrorHandler()
                    {
                        eReasonCode = 9,
                        eMessage = "Não tem permissões para Terminar o Projeto."
                    };
                    return Json(result);
                }
                //}
                //else
                //    {
                //        result = new ErrorHandler()
                //        {
                //            eReasonCode = 9,
                //            eMessage = "Só o Responsável do Projeto é que pode terminar."
                //        };
                //        return Json(result);
                //    }
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult ReabrirProject([FromBody] ProjectDetailsViewModel data)
        {

            if (data != null)
            {
                ErrorHandler result = new ErrorHandler();

                UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Projetos);
                if (UPerm != null && UPerm.Update == true)
                {
                    string NoMecanografico = DBUserConfigurations.GetById(User.Identity.Name).EmployeeNo;
                    if (data.ProjectResponsible == NoMecanografico)
                    {
                        try
                        {
                            //Read NAV Project Key
                            Task<WSCreateNAVProject.Read_Result> TReadNavProj = WSProject.GetNavProject(data.ProjectNo, _configws);
                            try
                            {
                                TReadNavProj.Wait();
                            }
                            catch (Exception ex)
                            {
                                result = new ErrorHandler()
                                {
                                    eReasonCode = 3,
                                    eMessage = "Ocorreu um erro ao ler o projeto do NAV."
                                };
                                return Json(result);
                            }

                            if (TReadNavProj.IsCompletedSuccessfully)
                            {
                                if (TReadNavProj.Result.WSJob == null)
                                {
                                    result = new ErrorHandler()
                                    {
                                        eReasonCode = 4,
                                        eMessage = "Erro ao atualizar: O projeto não existe no NAV."
                                    };
                                    return Json(result);
                                }
                                else
                                {
                                    data.Status = (EstadoProjecto)1; //Encomenda

                                    //Update Project on NAV
                                    Task<WSCreateNAVProject.Update_Result> TUpdateNavProj = WSProject.UpdateNavProject(TReadNavProj.Result.WSJob.Key, data, _configws);
                                    bool statusL = true;
                                    try
                                    {
                                        TUpdateNavProj.Wait();
                                    }
                                    catch (Exception ex)
                                    {
                                        result = new ErrorHandler()
                                        {
                                            eReasonCode = 5,
                                            eMessage = ex.InnerException.Message
                                        };
                                        return Json(result);
                                    }

                                    if (!TUpdateNavProj.IsCompletedSuccessfully && statusL)
                                    {
                                        result = new ErrorHandler()
                                        {
                                            eReasonCode = 6,
                                            eMessage = "Ocorreu um erro ao atualizar o projeto no NAV."
                                        };
                                        return Json(result);
                                    }
                                    else
                                    {
                                        Projetos cProject = DBProjects.GetById(data.ProjectNo);

                                        cProject.NºProjeto = data.ProjectNo;
                                        cProject.Descrição = data.Description;
                                        cProject.NºCliente = data.ClientNo;
                                        cProject.Data = data.Date != "" && data.Date != null ? DateTime.Parse(data.Date) : (DateTime?)null;
                                        cProject.Estado = (EstadoProjecto)1; //Encomenda
                                        cProject.ChefeProjeto = data.ProjectLeader;
                                        cProject.ResponsávelProjeto = data.ProjectResponsible;
                                        cProject.CódObjetoServiço = data.ServiceObjectCode;
                                        cProject.Faturável = data.Billable;
                                        cProject.NºContrato = data.ContractNo;
                                        cProject.NossaProposta = data.OurProposal;
                                        cProject.NºCompromisso = data.CommitmentCode;
                                        cProject.CódigoRegião = data.RegionCode;
                                        cProject.CódigoÁreaFuncional = data.FunctionalAreaCode;
                                        cProject.CódigoCentroResponsabilidade = data.ResponsabilityCenterCode;
                                        cProject.TipoGrupoContabProjeto = data.GroupContabProjectType;
                                        cProject.PedidoDoCliente = data.ClientRequest;
                                        cProject.DataDoPedido = data.RequestDate != "" && data.RequestDate != null ? DateTime.Parse(data.RequestDate) : (DateTime?)null;
                                        cProject.DescriçãoDetalhada = data.DetailedDescription;
                                        cProject.CódEndereçoEnvio = data.ShippingAddressCode;
                                        cProject.EnvioANome = data.ShippingName;
                                        cProject.EnvioAEndereço = data.ShippingAddress;
                                        cProject.EnvioACódPostal = data.ShippingPostalCode;
                                        cProject.EnvioALocalidade = data.ShippingLocality;
                                        cProject.EnvioAContato = data.ShippingContact;
                                        cProject.CódTipoProjeto = data.ProjectTypeCode;
                                        cProject.CategoriaProjeto = data.ProjectCategory;
                                        cProject.NºContratoOrçamento = data.BudgetContractNo;
                                        cProject.ProjetoInterno = data.InternalProject;
                                        cProject.GrupoContabObra = "PROJETO";
                                        cProject.UtilizadorModificação = User.Identity.Name;
                                        cProject.DataHoraModificação = DateTime.Now;
                                        cProject.KWPotenciaInstalada = data.KWPotenciaInstalada;

                                        if (DBProjects.Update(cProject) != null)
                                        {
                                            result = new ErrorHandler()
                                            {
                                                eReasonCode = 0,
                                                eMessage = "Projeto reaberto com sucesso."
                                            };
                                            return Json(result);
                                        }
                                        else
                                        {
                                            result = new ErrorHandler()
                                            {
                                                eReasonCode = 7,
                                                eMessage = "Ocorreu um erro ao atualizar o projeto no eSUCH."
                                            };
                                            return Json(result);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            result = new ErrorHandler()
                            {
                                eReasonCode = 8,
                                eMessage = "Ocorreu um erro ao atualizar o projeto."
                            };
                            return Json(result);
                        }
                    }
                    else
                    {
                        result = new ErrorHandler()
                        {
                            eReasonCode = 9,
                            eMessage = "Não tem permissões para reabrir o Projeto."
                        };
                        return Json(result);
                    }
                }
                else
                {
                    result = new ErrorHandler()
                    {
                        eReasonCode = 9,
                        eMessage = "Só o Responsável do Projeto é que pode o pode reabrir."
                    };
                    return Json(result);
                }
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult DuplicarProject([FromBody] ProjectDetailsViewModel data)
        {
            try
            {
                if (data != null)
                {
                    //Get Project Numeration
                    bool autoGenId = false;
                    Configuração Configs = DBConfigurations.GetById(1);
                    int ProjectNumerationConfigurationId = Configs.NumeraçãoProjetos.Value;
                    string projNoAuto = "";
                    data.ProjectNo = null;
                    ConfigUtilizadores Utilizador = DBUserConfigurations.GetById(User.Identity.Name);

                    if (data.ProjectNo == "" || data.ProjectNo == null)
                    {
                        autoGenId = true;
                        projNoAuto = DBNumerationConfigurations.GetNextNumeration(ProjectNumerationConfigurationId, autoGenId, false);
                        data.ProjectNo = projNoAuto;
                    }

                    if (data.ProjectNo != null)
                    {
                        ErrorHandler resultApprovalMovement = new ErrorHandler();
                        Projetos cProject = new Projetos()
                        {
                            NºProjeto = data.ProjectNo,
                            Área = data.Area,
                            Descrição = data.Description,
                            NºCliente = data.ClientNo,
                            Data = data.Date != "" && data.Date != null ? DateTime.Parse(data.Date) : (DateTime?)null,
                            Estado = EstadoProjecto.Pendente,
                            //MovimentosVenda = data
                            CódigoRegião = data.RegionCode,
                            CódigoÁreaFuncional = data.FunctionalAreaCode,
                            CódigoCentroResponsabilidade = data.ResponsabilityCenterCode,
                            Faturável = data.Billable,
                            NºContrato = data.ContractNo,
                            CódEndereçoEnvio = data.ShippingAddressCode,
                            EnvioANome = data.ShippingName,
                            EnvioAEndereço = data.ShippingAddress,
                            EnvioACódPostal = data.ShippingPostalCode,
                            EnvioALocalidade = data.ShippingLocality,
                            EnvioAContato = data.ShippingContact,
                            CódTipoProjeto = data.ProjectTypeCode,
                            NossaProposta = data.OurProposal,
                            CódObjetoServiço = data.ServiceObjectCode,
                            NºCompromisso = data.CommitmentCode,
                            GrupoContabObra = "PROJETO",
                            TipoGrupoContabProjeto = data.GroupContabProjectType,
                            TipoGrupoContabOmProjeto = data.GroupContabOMProjectType,
                            PedidoDoCliente = data.ClientRequest,
                            DataDoPedido = data.RequestDate != "" && data.RequestDate != null ? DateTime.Parse(data.RequestDate) : (DateTime?)null,
                            ValidadeDoPedido = data.RequestValidity,
                            DescriçãoDetalhada = data.DetailedDescription,
                            CategoriaProjeto = data.ProjectCategory,
                            NºContratoOrçamento = data.BudgetContractNo,
                            ProjetoInterno = data.InternalProject,
                            ChefeProjeto = Utilizador.EmployeeNo,
                            ResponsávelProjeto = "",
                            DataHoraCriação = DateTime.Now,
                            UtilizadorCriação = User.Identity.Name,
                            DataHoraModificação = DateTime.Now,
                            UtilizadorModificação = User.Identity.Name,
                            FaturaPrecosIvaIncluido = data.FaturaPrecosIvaIncluido,
                            FechoAutomatico = data.FechoAutomatico,
                            KWPotenciaInstalada = data.KWPotenciaInstalada

                        };

                        //Create Project On Database
                        cProject = DBProjects.Create(cProject);

                        if (cProject == null)
                        {
                            data.eReasonCode = 3;
                            data.eMessage = "Ocorreu um erro ao criar o projeto duplicado no portal.";
                        }
                        else
                        {
                            resultApprovalMovement = ApprovalMovementsManager.StartApprovalMovement_Projetos(5, data.FunctionalAreaCode, data.ResponsabilityCenterCode, data.RegionCode, 0, data.ProjectNo, User.Identity.Name);

                            if (autoGenId)
                            {
                                ConfiguraçãoNumerações ConfigNumerations = DBNumerationConfigurations.GetById(ProjectNumerationConfigurationId);
                                ConfigNumerations.ÚltimoNºUsado = data.ProjectNo;
                                ConfigNumerations.UtilizadorModificação = User.Identity.Name;
                                DBNumerationConfigurations.Update(ConfigNumerations);
                            }
                            data.eReasonCode = 1;
                            data.eMessage = "Foi criado um projeto duplicado no Portal com o código " + data.ProjectNo;
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
                data.eMessage = "Ocorreu um erro ao criar o projeto";
            }
            return Json(data);
        }

        [HttpPost]
        public JsonResult EnviarAprovacao([FromBody] ProjectDetailsViewModel data)
        {
            data.eReasonCode = 99;
            data.eMessage = "Ocorreu um erro.";

            if (data != null)
            {
                if (data.Status == (EstadoProjecto)0) //PENDENTE
                {
                    if (data.ProjectNo != null)
                    {
                        ErrorHandler resultApprovalMovement = new ErrorHandler();
                        Projetos cProject = new Projetos()
                        {
                            NºProjeto = data.ProjectNo,
                            Área = data.Area,
                            Descrição = data.Description,
                            NºCliente = data.ClientNo,
                            Data = data.Date != "" && data.Date != null ? DateTime.Parse(data.Date) : (DateTime?)null,
                            Estado = data.Status,
                            CódigoRegião = data.RegionCode,
                            CódigoÁreaFuncional = data.FunctionalAreaCode,
                            CódigoCentroResponsabilidade = data.ResponsabilityCenterCode,
                            Faturável = data.Billable,
                            NºContrato = data.ContractNo,
                            CódEndereçoEnvio = data.ShippingAddressCode,
                            EnvioANome = data.ShippingName,
                            EnvioAEndereço = data.ShippingAddress,
                            EnvioACódPostal = data.ShippingPostalCode,
                            EnvioALocalidade = data.ShippingLocality,
                            EnvioAContato = data.ShippingContact,
                            CódTipoProjeto = data.ProjectTypeCode,
                            NossaProposta = data.OurProposal,
                            CódObjetoServiço = data.ServiceObjectCode,
                            NºCompromisso = data.CommitmentCode,
                            GrupoContabObra = "PROJETO",
                            TipoGrupoContabProjeto = data.GroupContabProjectType,
                            //TipoGrupoContabOmProjeto = data.GroupContabOMProjectType,
                            PedidoDoCliente = data.ClientRequest,
                            DataDoPedido = data.RequestDate != "" && data.RequestDate != null ? DateTime.Parse(data.RequestDate) : (DateTime?)null,
                            ValidadeDoPedido = data.RequestValidity,
                            DescriçãoDetalhada = data.DetailedDescription,
                            CategoriaProjeto = data.ProjectCategory,
                            NºContratoOrçamento = data.BudgetContractNo,
                            ProjetoInterno = data.InternalProject,
                            ChefeProjeto = data.ProjectLeader,
                            ResponsávelProjeto = data.ProjectResponsible,
                            UtilizadorCriação = User.Identity.Name,
                            KWPotenciaInstalada = data.KWPotenciaInstalada
                        };

                        //Create Project On Database
                        cProject = DBProjects.Update(cProject);

                        if (cProject == null)
                        {
                            data.eReasonCode = 3;
                            data.eMessage = "Ocorreu um erro ao criar o projeto no portal.";
                        }
                        else
                        {
                            resultApprovalMovement = ApprovalMovementsManager.StartApprovalMovement_Projetos(5, data.FunctionalAreaCode, data.ResponsabilityCenterCode, data.RegionCode, 0, data.ProjectNo, User.Identity.Name);

                            data.eReasonCode = 1;
                            data.eMessage = "O Projeto foi enviado com sucesso para aprovação.";
                        }
                    }
                    else
                    {
                        data.eReasonCode = 5;
                        data.eMessage = "A numeração configurada não é compativel com a inserida.";
                    }
                }
            }
            return Json(data);
        }

        #endregion

        #region DiárioDeProjetos
        public IActionResult DiarioProjeto(string id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.DiárioProjeto);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.ProjectNo = id ?? "";
                ViewBag.UPermissions = UPerm;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetAllProjectDiary([FromBody]  JObject requestParams)
        {
            string projectNo = "";
            string dataReque = "";
            string codServiceCliente = "";
            string codServiceGroup = string.Empty;
            if (requestParams != null)
            {
                projectNo = (requestParams["noproj"] != null) ? requestParams["noproj"].ToString() : "";
                dataReque = (requestParams["data"] != null) ? requestParams["data"].ToString() : "";
                codServiceCliente = (requestParams["codClienteServico"] != null) ? requestParams["codClienteServico"].ToString() : "";
                codServiceGroup = (requestParams["codGrupoServico"] != null) ? requestParams["codGrupoServico"].ToString() : "";
            }

            List<ProjectDiaryViewModel> dp = null;
            if (projectNo == null || projectNo == "")
            {
                dp = DBProjectDiary.GetAllOpen(User.Identity.Name).Select(x => new ProjectDiaryViewModel()
                {
                    LineNo = x.NºLinha,
                    ProjectNo = x.NºProjeto,
                    Date = !x.Data.HasValue ? String.Empty : x.Data.Value.ToString("yyyy-MM-dd"),
                    MovementType = x.TipoMovimento,
                    Type = x.Tipo,
                    Code = x.Código,
                    Description = x.Descrição,
                    Quantity = x.Quantidade.HasValue ? Math.Round((decimal)x.Quantidade, 2, MidpointRounding.AwayFromZero) : decimal.Zero,
                    MeasurementUnitCode = x.CódUnidadeMedida,
                    LocationCode = x.CódLocalização,
                    ProjectContabGroup = x.GrupoContabProjeto,
                    RegionCode = x.CódigoRegião,
                    FunctionalAreaCode = x.CódigoÁreaFuncional,
                    ResponsabilityCenterCode = x.CódigoCentroResponsabilidade,
                    User = x.Utilizador,
                    UnitCost = x.CustoUnitário.HasValue ? Math.Round((decimal)x.CustoUnitário, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                    TotalCost = x.Quantidade.HasValue && x.CustoUnitário.HasValue ? Math.Round((decimal)(x.Quantidade * x.CustoUnitário), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //x.CustoTotal,
                    UnitPrice = x.PreçoUnitário.HasValue ? Math.Round((decimal)x.PreçoUnitário, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                    TotalPrice = x.Quantidade.HasValue && x.PreçoUnitário.HasValue ? Math.Round((decimal)(x.Quantidade * x.PreçoUnitário), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //x.PreçoTotal,
                    Billable = x.Faturável,
                    Registered = x.Registado,
                    Billed = x.Faturada.HasValue ? x.Faturada.Value : false,
                    Currency = x.Moeda,
                    UnitValueToInvoice = x.ValorUnitárioAFaturar,
                    MealType = x.TipoRefeição,
                    ServiceGroupCode = codServiceGroup,
                    ResidueGuideNo = x.NºGuiaResíduos,
                    ExternalGuideNo = x.NºGuiaExterna,
                    ConsumptionDate = !x.DataConsumo.HasValue ? "" : x.DataConsumo.Value.ToString("yyyy-MM-dd"),
                    InvoiceToClientNo = x.FaturaANºCliente,
                    ServiceClientCode = (x.CódServiçoCliente != null && x.CódServiçoCliente != "") ? x.CódServiçoCliente : codServiceCliente,
                    TaxaIVA = x.TaxaIVA
                }).ToList();
                //return Json(dp);
            }
            else
            {
                //List<DiárioDeProjeto> dp1 = DBProjectDiary.GetByProjectNo(projectNo, User.Identity.Name).ToList();
                //foreach (DiárioDeProjeto var in dp1)
                //{
                //    vae
                //}
                dp = DBProjectDiary.GetByProjectNo(projectNo, User.Identity.Name).Select(x => new ProjectDiaryViewModel()
                {
                    LineNo = x.NºLinha,
                    ProjectNo = x.NºProjeto,
                    Date = !x.Data.HasValue ? String.Empty : x.Data.Value.ToString("yyyy-MM-dd"),
                    MovementType = x.TipoMovimento,
                    Type = x.Tipo,
                    Code = x.Código,
                    Description = x.Descrição,
                    Quantity = x.Quantidade.HasValue ? Math.Round((decimal)x.Quantidade, 2, MidpointRounding.AwayFromZero) : decimal.Zero,
                    MeasurementUnitCode = x.CódUnidadeMedida,
                    LocationCode = x.CódLocalização,
                    ProjectContabGroup = x.GrupoContabProjeto,
                    RegionCode = x.CódigoRegião,
                    FunctionalAreaCode = x.CódigoÁreaFuncional,
                    ResponsabilityCenterCode = x.CódigoCentroResponsabilidade,
                    User = x.Utilizador,
                    UnitCost = x.CustoUnitário.HasValue ? Math.Round((decimal)x.CustoUnitário, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                    TotalCost = x.Quantidade.HasValue && x.CustoUnitário.HasValue ? Math.Round((decimal)(x.Quantidade * x.CustoUnitário), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //x.CustoTotal,
                    UnitPrice = x.PreçoUnitário.HasValue ? Math.Round((decimal)x.PreçoUnitário, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                    TotalPrice = x.Quantidade.HasValue && x.PreçoUnitário.HasValue ? Math.Round((decimal)(x.Quantidade * x.PreçoUnitário), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //x.PreçoTotal,
                    Billable = x.Faturável,
                    Registered = x.Registado,
                    Billed = x.Faturada.HasValue ? x.Faturada.Value : false,
                    Currency = x.Moeda,
                    UnitValueToInvoice = x.ValorUnitárioAFaturar,
                    MealType = x.TipoRefeição,
                    ServiceGroupCode = codServiceGroup,
                    ResidueGuideNo = x.NºGuiaResíduos,
                    ExternalGuideNo = x.NºGuiaExterna,
                    ConsumptionDate = !x.DataConsumo.HasValue ? "" : x.DataConsumo.Value.ToString("yyyy-MM-dd"),
                    InvoiceToClientNo = x.FaturaANºCliente,
                    ServiceClientCode = (x.CódServiçoCliente != null && x.CódServiçoCliente != "") ? x.CódServiçoCliente : codServiceCliente,
                    TaxaIVA = x.TaxaIVA
                }).ToList();
                //return Json(dp);
            }
            ProjectDiaryResponse response = new ProjectDiaryResponse();
            response.eReasonCode = 1;
            response.Items = dp;

            return Json(response);
        }


        public class ProjectDiaryResponse : ErrorHandler
        {
            public List<ProjectDiaryViewModel> Items { get; set; }

            public ProjectDiaryResponse()
            {
                this.Items = new List<ProjectDiaryViewModel>();
            }
        }

        [HttpPost]
        public JsonResult UpdateProjectDiary([FromBody] List<ProjectDiaryViewModel> dp, string projectNo)
        {
            ProjectDiaryResponse response = new ProjectDiaryResponse();
            response.eReasonCode = 1;
            response.eMessage = "Diário de Projeto atualizado.";
            if (dp != null)
                response.Items = dp;
            //Update or Create
            try
            {
                List<DiárioDeProjeto> previousList;
                //throw new Exception("aaa");
                if (projectNo == null || projectNo == "")
                {
                    // Get All
                    previousList = DBProjectDiary.GetAll(User.Identity.Name);
                }
                else
                {
                    previousList = DBProjectDiary.GetByProjectNo(projectNo, User.Identity.Name);
                }

                //previousList.RemoveAll(x => !dp.Any(u => u.LineNo == x.NºLinha));
                //previousList.ForEach(x => DBProjectDiary.Delete(x));
                foreach (DiárioDeProjeto line in previousList)
                {
                    if (!dp.Any(x => x.LineNo == line.NºLinha))
                    {
                        DBProjectDiary.Delete(line);

                        TabelaLog TabLog = new TabelaLog
                        {
                            Tabela = "[dbo].[Diário de Projeto]",
                            Descricao = "Delete - [Nº Linha]: " + line.NºLinha.ToString() + " - [Nº Projeto]: " + line.NºProjeto + " - [Data]: " + Convert.ToDateTime(line.Data).ToShortDateString() + " - [Código]: " + line.Código,
                            Utilizador = User.Identity.Name,
                            DataHora = DateTime.Now
                        };
                        DBTabelaLog.Create(TabLog);
                    }
                }

                bool hasItemsWithoutMealType = dp.Any(x => x.FunctionalAreaCode.StartsWith("5") && (!x.MealType.HasValue || x.MealType == 0));
                if (hasItemsWithoutMealType)
                {
                    response.eReasonCode = 5;
                    response.eMessage = "Não é possivel Guardar, por existir pelo menos um Movimento da Área da Alimentação que não têm o campo Tipo de Refeição preenchido.";
                    return Json(response);
                }

                ConfiguracaoParametros Parametro = DBConfiguracaoParametros.GetById(13);
                dp.ForEach(x =>
                {
                    List<DiárioDeProjeto> dpObject = DBProjectDiary.GetByLineNo(x.LineNo, User.Identity.Name);

                    if (dpObject != null && dpObject.Count > 0)
                    {
                        if (Parametro != null && Convert.ToDateTime(Parametro.Valor) > Convert.ToDateTime(x.Date))
                        {
                            response.eReasonCode = 6;
                            response.eMessage = "Não é possivel Guardar, por existir pelo menos uma linha no diário onde a Data é a inferior á data " + Convert.ToDateTime(Parametro.Valor).ToShortDateString();
                        }
                    }
                    else
                    {
                        if (Parametro != null && Convert.ToDateTime(Parametro.Valor) > Convert.ToDateTime(x.Date))
                        {
                            response.eReasonCode = 6;
                            response.eMessage = "Não é possivel Criar, por a Data ser inferior á data " + Convert.ToDateTime(Parametro.Valor).ToShortDateString();
                        }
                    }

                    if (x.Type == 2 && !string.IsNullOrEmpty(x.Code)) //Recurso
                    {
                        NAVResourcesViewModel Resource = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, x.Code, "", 0, "").FirstOrDefault();

                        if (Resource != null && string.IsNullOrEmpty(Resource.GenProdPostingGroup))
                        {
                            response.eReasonCode = 6;
                            response.eMessage = "Não é possivel Criar, por faltar a configuração do Grupo Contabilístico no Recurso " + x.Code;
                        }
                    }
                });
                if (response.eReasonCode == 6)
                    return Json(response);

                decimal IVA = new decimal();
                dp.ForEach(x =>
                {
                    if (!string.IsNullOrEmpty(x.ProjectNo) && !string.IsNullOrEmpty(x.Code))
                    {
                        Projetos Projeto = DBProjects.GetById(x.ProjectNo);

                        if (Projeto != null && !string.IsNullOrEmpty(Projeto.NºCliente))
                        {
                            NAVClientsViewModel Cliente = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, Projeto.NºCliente);

                            if (Cliente != null && !string.IsNullOrEmpty(Cliente.VATBusinessPostingGroup))
                            {
                                if (x.Type == 1) //PRODUTOS
                                {
                                    NAVProductsViewModel Product = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, x.Code).FirstOrDefault();

                                    if (Product != null && !string.IsNullOrEmpty(Product.VATProductPostingGroup))
                                    {
                                        IVA = DBNAV2017VATPostingSetup.GetIVA(_config.NAVDatabaseName, _config.NAVCompanyName, Cliente.VATBusinessPostingGroup, Product.VATProductPostingGroup);
                                    }
                                }

                                if (x.Type == 2) //RECURSOS
                                {

                                    NAVResourcesViewModel Resource = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, x.Code).FirstOrDefault();

                                    if (Resource != null && !string.IsNullOrEmpty(Resource.VATProductPostingGroup))
                                    {
                                        IVA = DBNAV2017VATPostingSetup.GetIVA(_config.NAVDatabaseName, _config.NAVCompanyName, Cliente.VATBusinessPostingGroup, Resource.VATProductPostingGroup);
                                    }
                                }
                            }
                        }
                    }




                    List<DiárioDeProjeto> dpObject = DBProjectDiary.GetByLineNo(x.LineNo, User.Identity.Name);
                    if (dpObject.Count > 0)
                    {
                        DiárioDeProjeto newdp = dpObject.FirstOrDefault();

                        newdp.NºLinha = x.LineNo;
                        newdp.NºProjeto = x.ProjectNo;
                        newdp.Data = x.Date == "" || x.Date == null ? (DateTime?)null : DateTime.Parse(x.Date);
                        newdp.TipoMovimento = x.MovementType;
                        newdp.Tipo = x.Type;
                        newdp.Código = x.Code;
                        newdp.Descrição = x.Description;
                        newdp.Quantidade = x.Quantity.HasValue ? Math.Round((decimal)x.Quantity, 2, MidpointRounding.AwayFromZero) : decimal.Zero;
                        newdp.CódUnidadeMedida = x.MeasurementUnitCode;
                        newdp.CódLocalização = x.Type.HasValue && x.Type == 1 ? "DIR" : x.LocationCode;
                        newdp.GrupoContabProjeto = x.ProjectContabGroup;
                        newdp.CódigoRegião = x.RegionCode;
                        newdp.CódigoÁreaFuncional = x.FunctionalAreaCode;
                        newdp.CódigoCentroResponsabilidade = x.ResponsabilityCenterCode;
                        newdp.Utilizador = User.Identity.Name;
                        newdp.CustoUnitário = x.UnitCost.HasValue ? Math.Round((decimal)x.UnitCost, 4, MidpointRounding.AwayFromZero) : decimal.Zero;
                        newdp.CustoTotal = x.Quantity.HasValue && x.UnitCost.HasValue ? Math.Round((decimal)(x.Quantity * x.UnitCost), 2, MidpointRounding.AwayFromZero) : decimal.Zero; //x.TotalCost;
                        newdp.PreçoUnitário = x.UnitPrice.HasValue ? Math.Round((decimal)x.UnitPrice, 4, MidpointRounding.AwayFromZero) : decimal.Zero;
                        newdp.PreçoTotal = x.Quantity.HasValue && x.UnitPrice.HasValue ? Math.Round((decimal)(x.Quantity * x.UnitPrice), 2, MidpointRounding.AwayFromZero) : decimal.Zero; //x.TotalPrice;
                        newdp.Faturável = x.Billable;
                        newdp.Registado = false;
                        newdp.FaturaANºCliente = x.InvoiceToClientNo;
                        newdp.Moeda = x.Currency;
                        newdp.ValorUnitárioAFaturar = x.UnitValueToInvoice;
                        newdp.TipoRefeição = x.MealType;
                        newdp.CódGrupoServiço = x.ServiceGroupCode;
                        newdp.NºGuiaResíduos = x.ResidueGuideNo;
                        newdp.NºGuiaExterna = x.ExternalGuideNo;
                        newdp.DataConsumo = x.ConsumptionDate == "" || x.ConsumptionDate == null ? (DateTime?)null : DateTime.Parse(x.ConsumptionDate);
                        newdp.CódServiçoCliente = x.ServiceClientCode;
                        newdp.Faturada = x.Billed;
                        newdp.DataHoraModificação = DateTime.Now;
                        newdp.UtilizadorModificação = User.Identity.Name;
                        newdp.PréRegisto = false;
                        newdp.TaxaIVA = IVA;

                        DBProjectDiary.Update(newdp);
                    }
                    else
                    {
                        DiárioDeProjeto newdp = new DiárioDeProjeto()
                        {
                            NºLinha = x.LineNo,
                            NºProjeto = x.ProjectNo,
                            Data = x.Date == "" || x.Date == null ? (DateTime?)null : DateTime.Parse(x.Date),
                            TipoMovimento = x.MovementType,
                            Tipo = x.Type,
                            Código = x.Code,
                            Descrição = x.Description,
                            Quantidade = x.Quantity.HasValue ? Math.Round((decimal)x.Quantity, 2, MidpointRounding.AwayFromZero) : decimal.Zero,
                            CódUnidadeMedida = x.MeasurementUnitCode,
                            CódLocalização = x.Type.HasValue && x.Type == 1 ? "DIR" : x.LocationCode,
                            GrupoContabProjeto = x.ProjectContabGroup,
                            CódigoRegião = x.RegionCode,
                            CódigoÁreaFuncional = x.FunctionalAreaCode,
                            CódigoCentroResponsabilidade = x.ResponsabilityCenterCode,
                            Utilizador = User.Identity.Name,
                            CustoUnitário = x.UnitCost.HasValue ? Math.Round((decimal)x.UnitCost, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                            CustoTotal = x.Quantity.HasValue && x.UnitCost.HasValue ? Math.Round((decimal)(x.Quantity * x.UnitCost), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //x.TotalCost,
                            PreçoUnitário = x.UnitPrice.HasValue ? Math.Round((decimal)x.UnitPrice, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                            PreçoTotal = x.Quantity.HasValue && x.UnitPrice.HasValue ? Math.Round((decimal)(x.Quantity * x.UnitPrice), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //x.TotalPrice,
                            Faturável = x.Billable,
                            Registado = false,
                            FaturaANºCliente = x.InvoiceToClientNo,
                            Moeda = x.Currency,
                            ValorUnitárioAFaturar = x.UnitValueToInvoice,
                            TipoRefeição = x.MealType,
                            CódGrupoServiço = x.ServiceGroupCode,
                            NºGuiaResíduos = x.ResidueGuideNo,
                            NºGuiaExterna = x.ExternalGuideNo,
                            DataConsumo = x.ConsumptionDate == "" || x.ConsumptionDate == null ? (DateTime?)null : DateTime.Parse(x.ConsumptionDate),
                            CódServiçoCliente = x.ServiceClientCode,
                            PréRegisto = false,
                            TaxaIVA = IVA

                        };

                        newdp.Faturada = false;
                        newdp.DataHoraCriação = DateTime.Now;
                        newdp.UtilizadorCriação = User.Identity.Name;
                        DBProjectDiary.Create(newdp);
                    }
                });
            }
            catch (Exception e)
            {
                //throw;
                response.eReasonCode = 2;
                response.eMessage = "Occorreu um erro ao atualizar o Diário de Projeto.";
            }

            return Json(response);
        }

        public JsonResult UpdateProjectDiaryRequisition(List<ProjectDiaryViewModel> dp, string projectNo, string userName)
        {
            List<DiárioDeProjeto> previousList;
            if (projectNo == null || projectNo == "")
            {
                // Get All
                previousList = DBProjectDiary.GetAll(userName);
            }
            else
            {
                previousList = DBProjectDiary.GetByProjectNo(projectNo, userName);
            }


            //previousList.RemoveAll(x => !dp.Any(u => u.LineNo == x.NºLinha));
            //previousList.ForEach(x => DBProjectDiary.Delete(x));
            foreach (DiárioDeProjeto line in previousList)
            {
                if (!dp.Any(x => x.LineNo == line.NºLinha))
                {
                    DBProjectDiary.Delete(line);

                    TabelaLog TabLog = new TabelaLog
                    {
                        Tabela = "[dbo].[Diário de Projeto]",
                        Descricao = "Delete - [Nº Linha]: " + line.NºLinha.ToString() + " - [Nº Projeto]: " + line.NºProjeto + " - [Data]: " + Convert.ToDateTime(line.Data).ToShortDateString() + " - [Código]: " + line.Código,
                        Utilizador = User.Identity.Name,
                        DataHora = DateTime.Now
                    };
                    DBTabelaLog.Create(TabLog);
                }
            }

            //Update or Create
            try
            {
                dp.ForEach(x =>
                {
                    List<DiárioDeProjeto> dpObject = DBProjectDiary.GetByLineNo(x.LineNo, userName);

                    if (dpObject.Count > 0)
                    {
                        DiárioDeProjeto newdp = dpObject.FirstOrDefault();

                        newdp.NºLinha = x.LineNo;
                        newdp.NºProjeto = x.ProjectNo;
                        newdp.Data = x.Date == "" || x.Date == null ? (DateTime?)null : DateTime.Parse(x.Date);
                        newdp.TipoMovimento = x.MovementType;
                        newdp.Tipo = x.Type;
                        newdp.Código = x.Code;
                        newdp.Descrição = x.Description;
                        newdp.Quantidade = x.Quantity.HasValue ? Math.Round((decimal)x.Quantity, 2, MidpointRounding.AwayFromZero) : decimal.Zero;
                        newdp.CódUnidadeMedida = x.MeasurementUnitCode;
                        newdp.CódLocalização = x.LocationCode;
                        newdp.GrupoContabProjeto = x.ProjectContabGroup;
                        newdp.CódigoRegião = x.RegionCode;
                        newdp.CódigoÁreaFuncional = x.FunctionalAreaCode;
                        newdp.CódigoCentroResponsabilidade = x.ResponsabilityCenterCode;
                        newdp.Utilizador = userName;
                        newdp.CustoUnitário = x.UnitCost.HasValue ? Math.Round((decimal)x.UnitCost, 4, MidpointRounding.AwayFromZero) : decimal.Zero;
                        newdp.CustoTotal = x.Quantity.HasValue && x.UnitCost.HasValue ? Math.Round((decimal)(x.Quantity * x.UnitCost), 2, MidpointRounding.AwayFromZero) : decimal.Zero; // x.TotalCost;
                        newdp.PreçoUnitário = x.UnitPrice.HasValue ? Math.Round((decimal)x.UnitPrice, 4, MidpointRounding.AwayFromZero) : decimal.Zero;
                        newdp.PreçoTotal = x.Quantity.HasValue && x.UnitPrice.HasValue ? Math.Round((decimal)(x.Quantity * x.UnitPrice), 2, MidpointRounding.AwayFromZero) : decimal.Zero; //x.TotalPrice;
                        newdp.Faturável = x.Billable;
                        newdp.Registado = false;
                        newdp.FaturaANºCliente = x.InvoiceToClientNo;
                        newdp.Moeda = x.Currency;
                        newdp.ValorUnitárioAFaturar = x.UnitValueToInvoice;
                        newdp.TipoRefeição = x.MealType;
                        newdp.CódGrupoServiço = x.ServiceGroupCode;
                        newdp.NºGuiaResíduos = x.ResidueGuideNo;
                        newdp.NºGuiaExterna = x.ExternalGuideNo;
                        newdp.DataConsumo = x.ConsumptionDate == "" || x.ConsumptionDate == null ? (DateTime?)null : DateTime.Parse(x.ConsumptionDate);
                        newdp.CódServiçoCliente = x.ServiceClientCode;
                        newdp.Faturada = x.Billed;
                        newdp.DataHoraModificação = DateTime.Now;
                        newdp.UtilizadorModificação = userName;
                        newdp.PréRegisto = false;
                        DBProjectDiary.Update(newdp);
                    }
                    else
                    {
                        DiárioDeProjeto newdp = new DiárioDeProjeto()
                        {
                            NºProjeto = x.ProjectNo,
                            Data = x.Date == "" || x.Date == null ? (DateTime?)null : DateTime.Parse(x.Date),
                            TipoMovimento = x.MovementType,
                            Tipo = x.Type,
                            Código = x.Code,
                            Descrição = x.Description,
                            Quantidade = x.Quantity.HasValue ? Math.Round((decimal)x.Quantity, 2, MidpointRounding.AwayFromZero) : decimal.Zero,
                            CódUnidadeMedida = x.MeasurementUnitCode,
                            CódLocalização = x.LocationCode,
                            GrupoContabProjeto = x.ProjectContabGroup,
                            CódigoRegião = x.RegionCode,
                            CódigoÁreaFuncional = x.FunctionalAreaCode,
                            CódigoCentroResponsabilidade = x.ResponsabilityCenterCode,
                            Utilizador = userName,
                            CustoUnitário = x.UnitCost.HasValue ? Math.Round((decimal)x.UnitCost, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                            CustoTotal = x.Quantity.HasValue && x.UnitCost.HasValue ? Math.Round((decimal)(x.Quantity * x.UnitCost), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //x.TotalCost,
                            PreçoUnitário = x.UnitPrice.HasValue ? Math.Round((decimal)x.UnitPrice, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                            PreçoTotal = x.Quantity.HasValue && x.UnitPrice.HasValue ? Math.Round((decimal)(x.Quantity * x.UnitPrice), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //x.TotalPrice,
                            Faturável = x.Billable,
                            Registado = false,
                            FaturaANºCliente = x.InvoiceToClientNo,
                            Moeda = x.Currency,
                            ValorUnitárioAFaturar = x.UnitValueToInvoice,
                            TipoRefeição = x.MealType,
                            CódGrupoServiço = x.ServiceGroupCode,
                            NºGuiaResíduos = x.ResidueGuideNo,
                            NºGuiaExterna = x.ExternalGuideNo,
                            DataConsumo = x.ConsumptionDate == "" || x.ConsumptionDate == null ? (DateTime?)null : DateTime.Parse(x.ConsumptionDate),
                            CódServiçoCliente = x.ServiceClientCode,
                            PréRegisto = false

                        };

                        newdp.Faturada = false;
                        newdp.DataHoraCriação = DateTime.Now;
                        newdp.UtilizadorCriação = userName;
                        DBProjectDiary.Create(newdp);
                    }


                });
            }
            catch (Exception e)
            {
                throw;
            }


            return Json(dp);
        }

        [HttpPost]
        public JsonResult CreatePDByMovProj([FromBody] List<ProjectDiaryViewModel> dp, string projectNo, string Resources, string ProjDiaryPrice, string Date, string DocumentNo)
        {

            ProjectDiaryResponse response = new ProjectDiaryResponse();
            string proj = dp.First().ProjectNo;
            string notCreatedLines = "";
            bool MoreThanOne = false;
            int OrderLine = 0;
            Projetos projecto = DBProjects.GetById(proj);
            if (dp != null)
            {
                foreach (ProjectDiaryViewModel item in dp)
                {
                    item.Date = Date;
                }
                response.Items = dp;
            }

            response.eReasonCode = 1;
            response.eMessage = "Diário de Projeto atualizado.";

            if (!string.IsNullOrEmpty(proj) && !string.IsNullOrEmpty(ProjDiaryPrice) && ProjDiaryPrice == "1")
            {
                if (!string.IsNullOrEmpty(Resources) && Resources != "undefined")
                {
                    if (!string.IsNullOrEmpty(projecto.NºContrato))
                    {
                        List<LinhasContratos> listContractLines = DBContractLines.GetbyContractId(projecto.NºContrato, Resources);
                        if (listContractLines != null && listContractLines.Count > 0)
                        {
                            if (dp.Count > 0)
                            {
                                foreach (ProjectDiaryViewModel pjD in dp)
                                {
                                    OrderLine++;
                                    bool newUnitCost = false;
                                    if (pjD.ServiceClientCode == null || pjD.ServiceClientCode == "")
                                    {
                                        pjD.ServiceClientCode = "";
                                    }
                                    foreach (LinhasContratos lc in listContractLines)
                                    {
                                        if (lc.CódServiçoCliente == null || lc.CódServiçoCliente == "")
                                        {
                                            lc.CódServiçoCliente = "";
                                        }
                                        if (pjD.ServiceClientCode == lc.CódServiçoCliente && newUnitCost == false)
                                        {
                                            pjD.UnitCost = lc.PreçoUnitário.HasValue ? Math.Round((decimal)lc.PreçoUnitário, 4, MidpointRounding.AwayFromZero) : decimal.Zero;
                                            newUnitCost = true;
                                        }
                                    }
                                    if (newUnitCost)
                                    {
                                        decimal IVA = new decimal();
                                        if (!string.IsNullOrEmpty(pjD.ProjectNo) && !string.IsNullOrEmpty(pjD.Code))
                                        {
                                            Projetos Projeto = DBProjects.GetById(pjD.ProjectNo);

                                            if (Projeto != null && !string.IsNullOrEmpty(Projeto.NºCliente))
                                            {
                                                NAVClientsViewModel Cliente = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, Projeto.NºCliente);

                                                if (Cliente != null && !string.IsNullOrEmpty(Cliente.VATBusinessPostingGroup))
                                                {
                                                    NAVProductsViewModel Product = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, pjD.Code).FirstOrDefault();

                                                    if (Product != null && !string.IsNullOrEmpty(Product.VATProductPostingGroup))
                                                    {
                                                        IVA = DBNAV2017VATPostingSetup.GetIVA(_config.NAVDatabaseName, _config.NAVCompanyName, Cliente.VATBusinessPostingGroup, Product.VATProductPostingGroup);
                                                    }
                                                }
                                            }
                                        }

                                        //Create
                                        DiárioDeProjeto newdp = new DiárioDeProjeto()
                                        {
                                            NºLinha = pjD.LineNo,
                                            NºProjeto = pjD.ProjectNo,
                                            Data = pjD.Date == "" || pjD.Date == String.Empty ? (DateTime?)null : DateTime.Parse(pjD.Date),
                                            TipoMovimento = pjD.MovementType,
                                            Tipo = pjD.Type,
                                            Código = pjD.Code,
                                            Descrição = pjD.Description,
                                            Quantidade = pjD.Quantity.HasValue ? Math.Round((decimal)pjD.Quantity, 2, MidpointRounding.AwayFromZero) : decimal.Zero,
                                            CódUnidadeMedida = pjD.MeasurementUnitCode,
                                            CódLocalização = null, // pjD.LocationCode, Pedido do Marco Marcelo Dia 15/11/2019
                                            GrupoContabProjeto = pjD.ProjectContabGroup,
                                            CódigoRegião = projecto.CódigoRegião,
                                            CódigoÁreaFuncional = projecto.CódigoÁreaFuncional,
                                            CódigoCentroResponsabilidade = projecto.CódigoCentroResponsabilidade,
                                            Utilizador = User.Identity.Name,
                                            CustoUnitário = pjD.UnitCost.HasValue ? Math.Round((decimal)pjD.UnitCost, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                                            CustoTotal = pjD.Quantity.HasValue && pjD.UnitCost.HasValue ? Math.Round((decimal)(pjD.Quantity * pjD.UnitCost), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //pjD.TotalCost,
                                            PreçoUnitário = pjD.UnitPrice.HasValue ? Math.Round((decimal)pjD.UnitPrice, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                                            PreçoTotal = pjD.Quantity.HasValue && pjD.UnitPrice.HasValue ? Math.Round((decimal)(pjD.Quantity * pjD.UnitPrice), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //pjD.TotalPrice,
                                            Faturável = true,
                                            Registado = false,
                                            FaturaANºCliente = projecto.NºCliente,
                                            Moeda = pjD.Currency,
                                            ValorUnitárioAFaturar = pjD.UnitValueToInvoice,
                                            PréRegisto = false,
                                            CódDestinoFinalResíduos = pjD.ResidueFinalDestinyCode,
                                            TipoRecurso = pjD.ResourceType,
                                            TaxaIVA = IVA,

                                            NºGuiaResíduos = pjD.ResidueGuideNo,
                                            NºGuiaExterna = pjD.ExternalGuideNo,
                                            TipoRefeição = pjD.MealType,
                                            CódGrupoServiço = pjD.ServiceGroupCode,
                                            DataConsumo = pjD.ConsumptionDate == "" || pjD.ConsumptionDate == String.Empty ? (DateTime?)null : DateTime.Parse(pjD.ConsumptionDate),
                                            CódServiçoCliente = pjD.ServiceClientCode,
                                        };
                                        if (newdp != null && newdp.PreçoTotal.HasValue && newdp.PreçoTotal < 0)
                                            newdp.NºDocumento = DocumentNo;
                                        else
                                            newdp.NºDocumento = "";

                                        if (pjD.LineNo > 0)
                                        {
                                            newdp.Faturada = pjD.Billed;
                                            newdp.DataHoraModificação = DateTime.Now;
                                            newdp.UtilizadorModificação = User.Identity.Name;
                                            DBProjectDiary.Update(newdp);
                                        }
                                        else
                                        {
                                            newdp.Faturada = false;
                                            newdp.DataHoraCriação = DateTime.Now;
                                            newdp.UtilizadorCriação = User.Identity.Name;
                                            DBProjectDiary.Create(newdp);
                                        }
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(notCreatedLines))
                                        {
                                            notCreatedLines = OrderLine + "ª ";
                                        }
                                        else
                                        {
                                            notCreatedLines = notCreatedLines + ", " + OrderLine + "ª ";
                                            MoreThanOne = true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            response.eReasonCode = 4;
                            response.eMessage = "O Contrato " + projecto.NºContrato + " não contém o recurso " + Resources + " nas suas linhas.";
                        }
                    }
                    else
                    {
                        response.eReasonCode = 3;
                        response.eMessage = "O projeto destino não contem contrato associado.";
                    }
                }
                else
                {
                    response.eReasonCode = 2;
                    response.eMessage = "Quando seleciona opção Contrato do campo Preço, é obrigatório selecionar um Recurso.";
                }
                if (!string.IsNullOrEmpty(notCreatedLines) && MoreThanOne)
                {
                    response.eReasonCode = 5;
                    response.eMessage = "Das linhas que foram selecionadas a " + notCreatedLines + " não foram criadas, porque o Código Serviço de Cliente, não existe no Contrato " + projecto.NºContrato;
                }
                else if (!string.IsNullOrEmpty(notCreatedLines))
                {
                    response.eReasonCode = 6;
                    response.eMessage = "A " + notCreatedLines + " linha não foi criada, porque o Código Serviço de Cliente, não existe no Contrato " + projecto.NºContrato;
                }
            }
            else
            {
                try
                {
                    //Create
                    dp.ForEach(x =>
                    {
                        decimal IVA = new decimal();
                        if (!string.IsNullOrEmpty(x.ProjectNo) && !string.IsNullOrEmpty(x.Code))
                        {
                            Projetos Projeto = DBProjects.GetById(x.ProjectNo);

                            if (Projeto != null && !string.IsNullOrEmpty(Projeto.NºCliente))
                            {
                                NAVClientsViewModel Cliente = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, Projeto.NºCliente);

                                if (Cliente != null && !string.IsNullOrEmpty(Cliente.VATBusinessPostingGroup))
                                {
                                    NAVProductsViewModel Product = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, x.Code).FirstOrDefault();

                                    if (Product != null && !string.IsNullOrEmpty(Product.VATProductPostingGroup))
                                    {
                                        IVA = DBNAV2017VATPostingSetup.GetIVA(_config.NAVDatabaseName, _config.NAVCompanyName, Cliente.VATBusinessPostingGroup, Product.VATProductPostingGroup);
                                    }
                                }
                            }
                        }

                        DiárioDeProjeto newdp = new DiárioDeProjeto()
                        {
                            NºLinha = x.LineNo,
                            NºProjeto = x.ProjectNo,
                            Data = x.Date == "" || x.Date == String.Empty ? (DateTime?)null : DateTime.Parse(x.Date),
                            TipoMovimento = x.MovementType,
                            Tipo = x.Type,
                            Código = x.Code,
                            Descrição = x.Description,
                            Quantidade = x.Quantity.HasValue ? Math.Round((decimal)x.Quantity, 2, MidpointRounding.AwayFromZero) : decimal.Zero,
                            CódUnidadeMedida = x.MeasurementUnitCode,
                            CódLocalização = null, // x.LocationCode, Pedido do Marco Marcelo Dia 15/11/2019
                            GrupoContabProjeto = x.ProjectContabGroup,
                            CódigoRegião = projecto.CódigoRegião,
                            CódigoÁreaFuncional = projecto.CódigoÁreaFuncional,
                            CódigoCentroResponsabilidade = projecto.CódigoCentroResponsabilidade,
                            Utilizador = User.Identity.Name,
                            CustoUnitário = x.UnitCost.HasValue ? Math.Round((decimal)x.UnitCost, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                            CustoTotal = x.Quantity.HasValue && x.UnitCost.HasValue ? Math.Round((decimal)(x.Quantity * x.UnitCost), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //x.TotalCost,
                            PreçoUnitário = x.UnitPrice.HasValue ? Math.Round((decimal)x.UnitPrice, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                            PreçoTotal = x.Quantity.HasValue && x.UnitPrice.HasValue ? Math.Round((decimal)(x.Quantity * x.UnitPrice), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //x.TotalPrice,
                            Faturável = true,
                            Registado = false,
                            FaturaANºCliente = projecto.NºCliente,
                            Moeda = x.Currency,
                            ValorUnitárioAFaturar = x.UnitValueToInvoice,
                            PréRegisto = false,
                            CódDestinoFinalResíduos = x.ResidueFinalDestinyCode,
                            TipoRecurso = x.ResourceType,
                            TaxaIVA = IVA,

                            NºGuiaResíduos = x.ResidueGuideNo,
                            NºGuiaExterna = x.ExternalGuideNo,
                            TipoRefeição = x.MealType,
                            CódGrupoServiço = x.ServiceGroupCode,
                            DataConsumo = x.ConsumptionDate == "" || x.ConsumptionDate == String.Empty ? (DateTime?)null : DateTime.Parse(x.ConsumptionDate),
                            CódServiçoCliente = x.ServiceClientCode,
                        };
                        if (newdp != null && newdp.PreçoTotal.HasValue && newdp.PreçoTotal < 0)
                            newdp.NºDocumento = DocumentNo;
                        else
                            newdp.NºDocumento = "";

                        if (x.LineNo > 0)
                        {
                            newdp.Faturada = x.Billed;
                            newdp.DataHoraModificação = DateTime.Now;
                            newdp.UtilizadorModificação = User.Identity.Name;
                            DBProjectDiary.Update(newdp);
                        }
                        else
                        {
                            newdp.Faturada = false;
                            newdp.DataHoraCriação = DateTime.Now;
                            newdp.UtilizadorCriação = User.Identity.Name;
                            DBProjectDiary.Create(newdp);
                        }
                    });
                }
                catch
                {
                    response.eReasonCode = 2;
                    response.eMessage = "Ocorreu um erro ao atualizar o Diário de Projeto.";
                }
            }
            return Json(response);// dp);
        }

        [HttpPost]
        public JsonResult GetRelatedProjectInfo([FromBody] string projectNo)
        {
            //Get Project Info
            Projetos proj = DBProjects.GetById(projectNo);

            if (proj != null)
            {
                string ClientName = "";
                if (!String.IsNullOrEmpty(proj.NºCliente))
                {
                    var getClient = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "").FirstOrDefault(x => x.No_ == proj.NºCliente);
                    if (getClient != null)
                    {
                        ClientName = getClient.Name;
                    }

                }
                ProjectInfo pi = new ProjectInfo
                {
                    //ProjectNo = proj.NºProjeto,
                    ClientName = ClientName,
                    ClientCod = proj.NºCliente,
                    Description = proj.Descrição,
                    ContabGroup = proj.GrupoContabObra,
                    RegionCode = proj.CódigoRegião,
                    FuncAreaCode = proj.CódigoÁreaFuncional,
                    ResponsabilityCenter = proj.CódigoCentroResponsabilidade,
                    InvoiceClientNo = proj.NºCliente,
                    Currency = DBNAV2017Clients.GetClientCurrencyByNo(proj.NºCliente, _config.NAVDatabaseName, _config.NAVCompanyName) //== null ? "EUR" : DBNAV2017Clients.GetClientCurrencyByNo(proj.NºCliente, _config.NAVDatabaseName, _config.NAVCompanyName),
                };

                return Json(pi);
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult RegisterDiaryLines([FromBody]  List<ProjectDiaryViewModel> dp)
        {
            ErrorHandler result = new ErrorHandler();
            result.eReasonCode = 1;
            result.eMessage = "Linhas do Diário registadas com sucesso.";
            string message = string.Empty;
            //SET INTEGRATED IN DB
            if (dp != null)
            {

                //AMARO
                //VER AMANHÃ COM O MARCO
                dp.ForEach(x =>
                {
                    if (x.Quantity == null || x.Quantity == 0)
                        DBProjectDiary.Delete(DBProjectDiary.ParseToDatabase(x));

                    x.MovementType = 1;
                });

                dp.RemoveAll(x => x.Quantity == null || x.Quantity == 0);

                bool hasItemsWithoutMealType = dp.Any(x => x.FunctionalAreaCode.StartsWith("5") && (!x.MealType.HasValue || x.MealType == 0));

                bool hasItemsWithoutDimensions = dp.Any(x => string.IsNullOrEmpty(x.RegionCode) ||
                                                            string.IsNullOrEmpty(x.FunctionalAreaCode) ||
                                                            string.IsNullOrEmpty(x.ResponsabilityCenterCode));

                if (hasItemsWithoutMealType)
                {
                    result.eReasonCode = 2;
                    result.eMessage = "Existem linhas inválidas: o campo Tipo de Refeição é de preenchimento obrigatório para a área da Alimentação.";
                }
                else
                {
                    if (hasItemsWithoutDimensions)
                    {
                        result.eReasonCode = 2;
                        result.eMessage = "Existem linhas inválidas: a Região, Área Funcional e Centro de Responsabilidade são obrigatórios.";
                    }
                    else
                    {
                        ConfiguracaoParametros Parametro = DBConfiguracaoParametros.GetById(13);
                        dp.ForEach(x =>
                        {
                            if (x != null && Parametro != null && Convert.ToDateTime(Parametro.Valor) > Convert.ToDateTime(x.Date))
                            {
                                result.eReasonCode = 6;
                                result.eMessage = "Não é possivel Registar, por existir pelo menos uma linha no diário onde a Data é a inferior á data " + Convert.ToDateTime(Parametro.Valor).ToShortDateString();
                            }
                        });
                        if (result.eReasonCode == 6)
                            return Json(result);

                        Guid transactID = Guid.NewGuid();
                        try
                        {
                            //Create Lines in NAV
                            Task<WSCreateProjectDiaryLine.CreateMultiple_Result> TCreateNavDiaryLine = WSProjectDiaryLine.CreateNavDiaryLines(dp, transactID, _configws);
                            TCreateNavDiaryLine.Wait();
                            try
                            {
                                Task<WSGenericCodeUnit.FxPostJobJrnlLines_Result> TRegisterNavDiaryLine = WSProjectDiaryLine.RegsiterNavDiaryLines(transactID, _configws);
                                TRegisterNavDiaryLine.Wait();
                            }
                            catch (Exception e)
                            {
                                WSProjectDiaryLine.DeleteNavDiaryLines(transactID, _configws);
                                result.eReasonCode = 2;
                                result.eMessage = "Não foi possivel registar: " + e.Message;
                                return Json(result);
                            }
                        }
                        catch (Exception ex)
                        {
                            result.eReasonCode = 2;
                            result.eMessage = "Não foi possivel registar: " + ex.Message;
                            return Json(result);
                        }

                        dp.ForEach(x =>
                        {
                            if (x.Code != null)
                            {
                                DiárioDeProjeto newdp = DBProjectDiary.GetAllByCode(User.Identity.Name, x.Code);
                                if (newdp != null)
                                {
                                    //newdp.Registado = true;
                                    //newdp.UtilizadorModificação = User.Identity.Name;
                                    //newdp.DataHoraModificação = DateTime.Now;
                                    DBProjectDiary.Delete(newdp);

                                    if (!string.IsNullOrEmpty(newdp.NºProjeto) && string.IsNullOrEmpty(newdp.FaturaANºCliente))
                                    {
                                        Projetos PROJ = DBProjects.GetById(newdp.NºProjeto);
                                        if (PROJ != null && !string.IsNullOrEmpty(PROJ.NºCliente))
                                            newdp.FaturaANºCliente = PROJ.NºCliente;
                                    };

                                    MovimentosDeProjeto ProjectMovement = new MovimentosDeProjeto()
                                    {
                                        //NºLinha = newdp.NºLinha,
                                        NºProjeto = newdp.NºProjeto,
                                        Data = newdp.Data,
                                        TipoMovimento = 1, //CONSUMO
                                        Tipo = newdp.Tipo,
                                        Código = newdp.Código,
                                        Descrição = newdp.Descrição,
                                        Quantidade = newdp.Quantidade.HasValue ? Math.Round((decimal)newdp.Quantidade, 2, MidpointRounding.AwayFromZero) : decimal.Zero,
                                        CódUnidadeMedida = newdp.CódUnidadeMedida,
                                        CódLocalização = newdp.CódLocalização,
                                        GrupoContabProjeto = newdp.GrupoContabProjeto,
                                        CódigoRegião = newdp.CódigoRegião,
                                        CódigoÁreaFuncional = newdp.CódigoÁreaFuncional,
                                        CódigoCentroResponsabilidade = newdp.CódigoCentroResponsabilidade,
                                        Utilizador = User.Identity.Name,
                                        CustoUnitário = newdp.CustoUnitário.HasValue ? Math.Round((decimal)newdp.CustoUnitário, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                                        CustoTotal = newdp.Quantidade.HasValue && newdp.CustoUnitário.HasValue ? Math.Round((decimal)(newdp.Quantidade * newdp.CustoUnitário), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //newdp.CustoTotal,
                                        PreçoUnitário = newdp.PreçoUnitário.HasValue ? Math.Round((decimal)newdp.PreçoUnitário, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                                        PreçoTotal = newdp.Quantidade.HasValue && newdp.PreçoUnitário.HasValue ? Math.Round((decimal)(newdp.Quantidade * newdp.PreçoUnitário), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //newdp.PreçoTotal,
                                        Faturável = newdp.Faturável,
                                        Registado = true,
                                        Faturada = false,
                                        FaturaANºCliente = newdp.FaturaANºCliente,
                                        Moeda = newdp.Moeda,
                                        ValorUnitárioAFaturar = newdp.ValorUnitárioAFaturar,
                                        TipoRefeição = newdp.TipoRefeição,
                                        CódGrupoServiço = newdp.CódGrupoServiço,
                                        NºGuiaResíduos = newdp.NºGuiaResíduos,
                                        NºGuiaExterna = newdp.NºGuiaExterna,
                                        DataConsumo = newdp.DataConsumo,
                                        CódServiçoCliente = newdp.CódServiçoCliente,
                                        UtilizadorCriação = User.Identity.Name,
                                        DataHoraCriação = DateTime.Now,
                                        FaturaçãoAutorizada = false,
                                        NºDocumento = "ES_" + newdp.NºProjeto,
                                        CriarMovNav2017 = false,
                                        TaxaIVA = newdp.TaxaIVA
                                    };
                                    if (ProjectMovement != null && ProjectMovement.PreçoTotal.HasValue && ProjectMovement.PreçoTotal < 0)
                                        ProjectMovement.FaturaRelacionada = newdp.NºDocumento;
                                    else
                                        ProjectMovement.FaturaRelacionada = "";

                                    DBProjectMovements.Create(ProjectMovement);
                                }
                            }
                        });
                    }
                }
            }
            else
            {
                result.eReasonCode = 2;
                result.eMessage = "Não existem linhas de Diário para Registar.";
            }
            return Json(result);
        }

        public JsonResult RegisterDiaryLinesRequisition(List<ProjectDiaryViewModel> dp, string userName)
        {
            //Guid transactID = Guid.NewGuid();

            //Create Lines in NAV
            //Task<WSCreateProjectDiaryLine.CreateMultiple_Result> TCreateNavDiaryLine = WSProjectDiaryLine.CreateNavDiaryLines(dp, transactID, _configws);
            //TCreateNavDiaryLine.Wait();

            ////Register Lines in NAV
            //Task<WSGenericCodeUnit.FxPostJobJrnlLines_Result> TRegisterNavDiaryLine = WSProjectDiaryLine.RegsiterNavDiaryLines(transactID, _configws);
            //TRegisterNavDiaryLine.Wait();

            //SET INTEGRATED IN DB
            if (dp != null)
            {
                dp.ForEach(x =>
                {
                    if (x.Code != null)
                    {
                        DiárioDeProjeto newdp = DBProjectDiary.GetAllByCode(userName, x.Code);
                        if (newdp != null)
                        {
                            //newdp.Registado = true;
                            //newdp.UtilizadorModificação = User.Identity.Name;
                            //newdp.DataHoraModificação = DateTime.Now;
                            DBProjectDiary.Delete(newdp);

                            MovimentosDeProjeto ProjectMovement = new MovimentosDeProjeto()
                            {
                                //NºLinha = newdp.NºLinha,
                                NºProjeto = newdp.NºProjeto,
                                Data = newdp.Data,
                                TipoMovimento = newdp.TipoMovimento,
                                Tipo = newdp.Tipo,
                                Código = newdp.Código,
                                Descrição = newdp.Descrição,
                                Quantidade = newdp.Quantidade.HasValue ? Math.Round((decimal)newdp.Quantidade, 2, MidpointRounding.AwayFromZero) : decimal.Zero,
                                CódUnidadeMedida = newdp.CódUnidadeMedida,
                                CódLocalização = newdp.CódLocalização,
                                GrupoContabProjeto = newdp.GrupoContabProjeto,
                                CódigoRegião = newdp.CódigoRegião,
                                CódigoÁreaFuncional = newdp.CódigoÁreaFuncional,
                                CódigoCentroResponsabilidade = newdp.CódigoCentroResponsabilidade,
                                Utilizador = userName,
                                CustoUnitário = newdp.CustoUnitário.HasValue ? Math.Round((decimal)newdp.CustoUnitário, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                                CustoTotal = newdp.Quantidade.HasValue && newdp.CustoUnitário.HasValue ? Math.Round((decimal)(newdp.Quantidade * newdp.CustoUnitário), 2, MidpointRounding.AwayFromZero) : decimal.Zero,
                                PreçoUnitário = newdp.PreçoUnitário.HasValue ? Math.Round((decimal)newdp.PreçoUnitário, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                                PreçoTotal = newdp.Quantidade.HasValue && newdp.PreçoUnitário.HasValue ? Math.Round((decimal)(newdp.Quantidade * newdp.PreçoUnitário), 2, MidpointRounding.AwayFromZero) : decimal.Zero,
                                Faturável = newdp.Faturável,
                                Registado = true,
                                Faturada = false,
                                FaturaANºCliente = newdp.FaturaANºCliente,
                                Moeda = newdp.Moeda,
                                ValorUnitárioAFaturar = newdp.ValorUnitárioAFaturar,
                                TipoRefeição = newdp.TipoRefeição,
                                CódGrupoServiço = newdp.CódGrupoServiço,
                                NºGuiaResíduos = newdp.NºGuiaResíduos,
                                NºGuiaExterna = newdp.NºGuiaExterna,
                                DataConsumo = newdp.DataConsumo,
                                CódServiçoCliente = newdp.CódServiçoCliente,
                                UtilizadorCriação = userName,
                                DataHoraCriação = DateTime.Now,
                                FaturaçãoAutorizada = false
                            };

                            DBProjectMovements.Create(ProjectMovement);
                        }


                    }
                });
            }


            return Json(dp);
        }

        [HttpPost]
        public JsonResult GetMovements([FromBody]  JObject requestParams)
        {
            string projectNo = "";
            string dataReque = "";
            string codServiceCliente = "";
            string codServiceGroup = "";
            if (requestParams != null)
            {
                projectNo = (requestParams["noproj"] != null) ? requestParams["noproj"].ToString() : "";
                dataReque = (requestParams["data"] != null) ? requestParams["data"].ToString() : "";
                codServiceCliente = (requestParams["codClienteServico"] != null) ? requestParams["codClienteServico"].ToString() : "";
                codServiceGroup = (requestParams["codGrupoServico"] != null) ? requestParams["codGrupoServico"].ToString() : "";
            }


            ErrorHandler result = new ErrorHandler();
            result.eReasonCode = 1;
            result.eMessage = "Os movimentos foram obtidos com sucesso";
            List<ProjectDiaryViewModel> projectDiaryItems = new List<ProjectDiaryViewModel>();
            if (!String.IsNullOrEmpty(projectNo))
            {
                Projetos proj = DBProjects.GetById(projectNo);
                if (proj != null && !String.IsNullOrEmpty(proj.NºContrato))
                {
                    Contratos lcontracts = DBContracts.GetActualContract(proj.NºContrato, proj.NºCliente);
                    if (lcontracts != null)
                    {
                        projectDiaryItems = DBContractLines.GetAllByActiveContract(lcontracts.NºDeContrato, lcontracts.NºVersão)
                            .Select(x => x.ParseToProjectDiary(projectNo, User.Identity.Name, dataReque, codServiceCliente, codServiceGroup))
                            .ToList();

                        if (projectDiaryItems.Count == 0)
                        {
                            result.eReasonCode = 4;
                            result.eMessage = "Este projeto não tem contrato com linhas associadas";
                        }
                        foreach (var item in projectDiaryItems)
                        {
                            decimal IVA = new decimal();
                            if (!string.IsNullOrEmpty(item.ProjectNo) && !string.IsNullOrEmpty(item.Code))
                            {
                                Projetos Projeto = DBProjects.GetById(item.ProjectNo);

                                if (Projeto != null && !string.IsNullOrEmpty(Projeto.NºCliente))
                                {
                                    NAVClientsViewModel Cliente = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, Projeto.NºCliente);

                                    if (Cliente != null && !string.IsNullOrEmpty(Cliente.VATBusinessPostingGroup))
                                    {
                                        NAVProductsViewModel Product = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, item.Code).FirstOrDefault();

                                        if (Product != null && !string.IsNullOrEmpty(Product.VATProductPostingGroup))
                                        {
                                            IVA = DBNAV2017VATPostingSetup.GetIVA(_config.NAVDatabaseName, _config.NAVCompanyName, Cliente.VATBusinessPostingGroup, Product.VATProductPostingGroup);
                                        }
                                    }
                                }
                            }

                            DiárioDeProjeto dpValidation = new DiárioDeProjeto();
                            item.Type = 2; //Recurso
                            item.CreateUser = User.Identity.Name;
                            item.CreateDate = DateTime.Now;
                            item.InvoiceToClientNo = proj.NºCliente;
                            item.TaxaIVA = IVA;
                            dpValidation = DBProjectDiary.Create(DBProjectDiary.ParseToDatabase(item));
                            if (dpValidation == null)
                            {
                                result.eReasonCode = 5;
                                result.eMessage = "Ocorreu um erro ao obter os movimentos";
                            }
                        }

                    }
                    else
                    {
                        result.eReasonCode = 4;
                        result.eMessage = "O Cliente " + proj.NºCliente + " do projeto selecinado não existe no Contrato " + proj.NºContrato;
                    }
                }
                else
                {
                    result.eReasonCode = 3;
                    result.eMessage = "Este projeto não tem contrato";
                }
            }
            else
            {
                result.eReasonCode = 2;
                result.eMessage = "Não foi selecionado nenhum projeto";
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetFaturacao(string projectNo, string serviceCod, string serviceGroup, string dateRegist)
        {
            ErrorHandler result = new ErrorHandler();
            result.eReasonCode = 1;
            result.eMessage = "Os movimentos foram obtidos com sucesso";

            try
            {
                Projetos proj = DBProjects.GetById(projectNo);
                List<PriceServiceClientViewModel> dp = DBPriceServiceClient.ParseToViewModel(DBPriceServiceClient.GetAll()).Where(x => x.Client == proj.NºCliente && x.CodServClient == serviceCod).ToList();

                if (dp != null && dp.Count > 0)
                {
                    List<ProjectDiaryViewModel> newRows = new List<ProjectDiaryViewModel>();

                    foreach (PriceServiceClientViewModel item in dp)
                    {
                        decimal IVA = new decimal();
                        if (!string.IsNullOrEmpty(projectNo) && !string.IsNullOrEmpty(item.Resource))
                        {
                            Projetos Projeto = DBProjects.GetById(projectNo);

                            if (Projeto != null && !string.IsNullOrEmpty(Projeto.NºCliente))
                            {
                                NAVClientsViewModel Cliente = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, Projeto.NºCliente);

                                if (Cliente != null && !string.IsNullOrEmpty(Cliente.VATBusinessPostingGroup))
                                {
                                    NAVProductsViewModel Product = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, item.Resource).FirstOrDefault();

                                    if (Product != null && !string.IsNullOrEmpty(Product.VATProductPostingGroup))
                                    {
                                        IVA = DBNAV2017VATPostingSetup.GetIVA(_config.NAVDatabaseName, _config.NAVCompanyName, Cliente.VATBusinessPostingGroup, Product.VATProductPostingGroup);
                                    }
                                }
                            }
                        }

                        ProjectDiaryViewModel newRow = new ProjectDiaryViewModel();
                        DiárioDeProjeto dpValidation = new DiárioDeProjeto();

                        newRow.Date = dateRegist;
                        newRow.ProjectNo = projectNo;
                        newRow.InvoiceToClientNo = proj.NºCliente;
                        newRow.ServiceClientCode = serviceCod;
                        newRow.ServiceGroupCode = serviceGroup;
                        newRow.Type = 2;
                        newRow.Code = item.Resource;
                        newRow.Description = item.ResourceDescription;
                        newRow.MeasurementUnitCode = item.UnitMeasure;
                        newRow.UnitCost = item.PriceCost.HasValue ? Math.Round((decimal)item.PriceCost, 4, MidpointRounding.AwayFromZero) : decimal.Zero;
                        newRow.UnitPrice = item.SalePrice.HasValue ? Math.Round((decimal)item.SalePrice, 4, MidpointRounding.AwayFromZero) : decimal.Zero;
                        if (item.Quantidade.HasValue && item.Quantidade > 0) newRow.Quantity = Math.Round((decimal)item.Quantidade, 4, MidpointRounding.AwayFromZero);
                        newRow.Billable = true;
                        newRow.ProjectContabGroup = proj.GrupoContabObra;
                        newRow.MovementType = 1;
                        if (!String.IsNullOrEmpty(item.TypeMeal))
                        {
                            newRow.MealType = Convert.ToInt32(item.TypeMeal);
                        }
                        else
                        {
                            newRow.MealType = null;
                        }
                        newRow.RegionCode = proj.CódigoRegião;
                        newRow.FunctionalAreaCode = proj.CódigoÁreaFuncional;
                        newRow.ResponsabilityCenterCode = proj.CódigoCentroResponsabilidade;
                        newRow.Utilizador = User.Identity.Name;
                        newRow.User = User.Identity.Name;
                        newRow.Registered = false;
                        newRow.PreRegistered = false;
                        newRow.TaxaIVA = IVA;

                        newRow.CreateUser = User.Identity.Name;
                        newRow.CreateDate = DateTime.Now;

                        dpValidation = DBProjectDiary.Create(DBProjectDiary.ParseToDatabase(newRow));
                        if (dpValidation == null)
                        {
                            result.eReasonCode = 5;
                            result.eMessage = "Ocorreu um erro ao obter os movimentos";
                        }
                        newRows.Add(newRow);
                    }
                }
                else
                {
                    result.eReasonCode = 2;
                    result.eMessage = "Tabela Preços Serviços Cliente não existe nenhuma linha com o Nº Cliente = " + proj.NºCliente + " e o Código Serviço Cliente = " + serviceCod;
                }
            }
            catch (Exception)
            {
                result.eReasonCode = 3;
                result.eMessage = "Ocorreu algum erro ao Obter as linhas da Tabela Preços Serviços";
            }

            return Json(result);
        }

        public class ProjectInfo
        {
            public string ProjectNo { get; set; }
            public string ContabGroup { get; set; }
            public string Description { get; set; }
            public string RegionCode { get; set; }
            public string FuncAreaCode { get; set; }
            public string ResponsabilityCenter { get; set; }
            public string InvoiceClientNo { get; set; }
            public string Currency { get; set; }
            public string ClientCod { get; set; }
            public string ClientName { get; set; }
        }
        #endregion

        #region Job Ledger Entry
        //public IActionResult MovimentosDeProjeto(String id)
        public IActionResult MovimentosDeProjeto(string id, [FromQuery]string areaid)
        {
            UserAccessesViewModel userAccesses = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Projetos);

            if (userAccesses != null && userAccesses.Read.Value)
            {
                if (id != null)
                {
                    DateTime data = DateTime.Now.AddMonths(-3);
                    string ano = data.Year.ToString();
                    string mes = data.Month < 10 ? "0" + data.Month.ToString() : data.Month.ToString();
                    string dia = data.Day < 10 ? "0" + data.Day.ToString() : data.Day.ToString();

                    ViewBag.ProjectNo = id ?? "";
                    ViewBag.PesquisaDate = ano + "-" + mes + "-" + dia;
                    ViewBag.reportServerURL = _config.ReportServerURL;
                    return View();
                }
                else
                {
                    return RedirectToAction("PageNotFound", "Error");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        //public JsonResult GetProjectMovements([FromBody] string ProjectNo, string pesquisaData)
        public JsonResult GetProjectMovements([FromBody] JObject requestParams)
        {
            string ProjectNo = (string)requestParams.GetValue("projectno");
            string pesquisaData = (string)requestParams.GetValue("pesquisadata");


            List<NAVMeasureUnitViewModel> MeasurementUnitList = DBNAV2017MeasureUnit.GetAllMeasureUnit(_config.NAVDatabaseName, _config.NAVCompanyName);
            List<NAVLocationsViewModel> LocationList = DBNAV2017Locations.GetAllLocations(_config.NAVDatabaseName, _config.NAVCompanyName);
            List<NAVClientsViewModel> ClientsList = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "");
            List<TiposRefeição> MealList = DBMealTypes.GetAll();
            List<ClientServicesViewModel> AllServiceGroup = DBClientServices.GetAllServiceGroup(string.Empty, true);
            List<NAVProductsViewModel> AllProducts = DBNAV2017Products.GetAllProducts(_config.NAVDatabaseName, _config.NAVCompanyName, "");
            List<ProjectDiaryViewModel> dp = new List<ProjectDiaryViewModel>();

            try
            {
                if (string.IsNullOrEmpty(pesquisaData))
                {
                    dp = DBProjectMovements.GetRegisteredDiary(ProjectNo).Select(x => new ProjectDiaryViewModel()
                    {
                        LineNo = x.NºLinha,
                        ProjectNo = x.NºProjeto,
                        Date = x.Data == null ? String.Empty : x.Data.Value.ToString("yyyy-MM-dd"),
                        MovementType = x.TipoMovimento,
                        MovementTypeText = x.TipoMovimento != null ? EnumerablesFixed.ProjectDiaryMovements.Where(y => y.Id == x.TipoMovimento).FirstOrDefault() != null ? EnumerablesFixed.ProjectDiaryMovements.Where(y => y.Id == x.TipoMovimento).FirstOrDefault().Value : "" : "",
                        DocumentNo = x.NºDocumento,
                        Type = x.Tipo,
                        TypeText = x.Tipo != null ? EnumerablesFixed.ProjectDiaryTypes.Where(y => y.Id == x.Tipo).FirstOrDefault() != null ? EnumerablesFixed.ProjectDiaryTypes.Where(y => y.Id == x.Tipo).FirstOrDefault().Value : "" : "",
                        Code = x.Código,
                        Description = x.Descrição,
                        CodigoTipoTrabalho = x.CodigoTipoTrabalho,
                        Quantity = x.Quantidade.HasValue ? Math.Round((decimal)x.Quantidade, 2, MidpointRounding.AwayFromZero) : decimal.Zero,
                        MeasurementUnitCode = !string.IsNullOrEmpty(x.CódUnidadeMedida) ? MeasurementUnitList != null ? MeasurementUnitList.Where(y => y.Code == x.CódUnidadeMedida).FirstOrDefault() != null ? MeasurementUnitList.Where(y => y.Code == x.CódUnidadeMedida).FirstOrDefault().Description : "" : "Informação indisponível" : "",
                        LocationCode = !string.IsNullOrEmpty(x.CódLocalização) ? LocationList != null ? LocationList.Where(y => y.Code == x.CódLocalização).FirstOrDefault() != null ? LocationList.Where(y => y.Code == x.CódLocalização).FirstOrDefault().Name : "" : "Informação indisponível" : "",
                        ProjectContabGroup = x.GrupoContabProjeto,
                        RegionCode = x.CódigoRegião,
                        FunctionalAreaCode = x.CódigoÁreaFuncional,
                        ResponsabilityCenterCode = x.CódigoCentroResponsabilidade,
                        User = x.Utilizador,
                        UnitCost = x.CustoUnitário.HasValue ? Math.Round((decimal)x.CustoUnitário, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                        TotalCost = x.Quantidade.HasValue && x.CustoUnitário.HasValue ? Math.Round((decimal)(x.Quantidade * x.CustoUnitário), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //x.CustoTotal,
                        UnitPrice = x.PreçoUnitário.HasValue ? Math.Round((decimal)x.PreçoUnitário, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                        TotalPrice = x.Quantidade.HasValue && x.PreçoUnitário.HasValue ? Math.Round((decimal)(x.Quantidade * x.PreçoUnitário), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //x.PreçoTotal,
                        Billable = x.Faturável,
                        BillableText = x.Faturável.HasValue ? x.Faturável == true ? "Sim" : "Não" : "Não",
                        ResidueGuideNo = x.NºGuiaResíduos,
                        ExternalGuideNo = x.NºGuiaExterna,
                        InvoiceToClientNo = x.FaturaANºCliente,
                        RequestNo = x.NºRequisição,
                        RequestLineNo = x.NºLinhaRequisição,
                        Driver = x.Motorista,
                        MealType = x.TipoRefeição,
                        ResidueFinalDestinyCode = x.CódDestinoFinalResíduos,
                        OriginalDocument = x.DocumentoOriginal,
                        AdjustedDocument = x.DocumentoCorrigido,
                        AdjustedPrice = x.AcertoDePreços,
                        AdjustedPriceText = x.AcertoDePreços.HasValue ? x.AcertoDePreços == true ? "Sim" : "Não" : "Não",
                        AdjustedDocumentData = x.DataDocumentoCorrigido?.ToString("yyyy-MM-dd"),
                        AutorizatedInvoice = x.FaturaçãoAutorizada,
                        AutorizatedInvoiceText = x.FaturaçãoAutorizada.HasValue ? x.FaturaçãoAutorizada == true ? "Sim" : "Não" : "Não",
                        AutorizatedInvoice2 = x.FaturaçãoAutorizada2,
                        AutorizatedInvoice2Text = x.FaturaçãoAutorizada2.HasValue ? x.FaturaçãoAutorizada2 == true ? "Sim" : "Não" : "Não",
                        AutorizatedInvoiceData = x.DataAutorizaçãoFaturação?.ToString("yyyy-MM-dd"),
                        ServiceGroupCode = x.CódGrupoServiço,
                        ServiceGroupCodeDescription = AllServiceGroup != null ? AllServiceGroup.Where(y => y.ClientNumber == x.FaturaANºCliente && y.ServiceCode == x.CódGrupoServiço).FirstOrDefault() != null ? AllServiceGroup.Where(y => y.ClientNumber == x.FaturaANºCliente && y.ServiceCode == x.CódGrupoServiço).FirstOrDefault().ServiceDescription : "Informação indisponível" : "",
                        ResourceType = x.TipoRecurso,
                        FolhaHoras = x.NºFolhaHoras,
                        InternalRequest = x.RequisiçãoInterna,
                        EmployeeNo = x.NºFuncionário,
                        QuantityReturned = Convert.ToDecimal(x.QuantidadeDevolvida),
                        ConsumptionDate = x.DataConsumo?.ToString("yyyy-MM-dd"),
                        CreateDate = x.DataHoraCriação,
                        CreateDateText = x.DataHoraCriação.HasValue ? Convert.ToDateTime(x.DataHoraCriação).ToShortDateString() : "",
                        CreateHourText = x.DataHoraCriação.HasValue ? Convert.ToDateTime(x.DataHoraCriação).ToShortTimeString() : "",
                        UpdateDate = x.DataHoraModificação,
                        CreateUser = x.UtilizadorCriação,
                        UpdateUser = x.UtilizadorModificação,
                        Registered = x.Registado,
                        RegisteredText = x.Registado.HasValue ? x.Registado == true ? "Sim" : "Não" : "Não",
                        Billed = Convert.ToBoolean(x.Faturada),
                        BilledText = x.Faturada.HasValue ? x.Faturada == true ? "Sim" : "Não" : "Não",
                        Coin = x.Moeda,
                        UnitValueToInvoice = x.ValorUnitárioAFaturar,
                        ServiceClientCode = x.CódServiçoCliente,
                        ClientRequest = x.CodCliente,
                        LicensePlate = x.Matricula,
                        ReadingCode = x.CodigoLer,
                        Group = x.Grupo,
                        Operation = x.Operacao,
                        InvoiceGroup = x.GrupoFatura,
                        InvoiceGroupDescription = x.GrupoFaturaDescricao,
                        AuthorizedBy = x.AutorizadoPor,
                        ClientName = !string.IsNullOrEmpty(x.FaturaANºCliente) ? ClientsList != null ? ClientsList.Where(y => y.No_ == x.FaturaANºCliente).FirstOrDefault() != null ? ClientsList.Where(y => y.No_ == x.FaturaANºCliente).FirstOrDefault().Name : "" : "Informação indisponível" : "",
                        MealTypeDescription = x.TipoRefeição != null ? MealList != null ? MealList.Where(y => y.Código == x.TipoRefeição).FirstOrDefault() != null ? MealList.Where(y => y.Código == x.TipoRefeição).FirstOrDefault().Descrição : "" : "Informação indisponível" : "",
                        Utilizador = User.Identity.Name,
                        NameDB = _config.NAVDatabaseName,
                        CompanyName = _config.NAVCompanyName,
                        Fatura = x.Fatura,
                        ProductGroupCode = !string.IsNullOrEmpty(x.Código) ? AllProducts != null ? AllProducts.Where(y => y.Code == x.Código).FirstOrDefault() != null ? AllProducts.Where(y => y.Code == x.Código).FirstOrDefault().ProductGroupCode : "" : "Informação indisponível" : "",
                        TaxaIVA = x.TaxaIVA,
                    }).OrderByDescending(x => x.Date).ToList();
                }
                else
                {
                    DateTime data = Convert.ToDateTime(pesquisaData);

                    List<MovimentosDeProjeto> test = DBProjectMovements.GetRegisteredDiaryByDate(ProjectNo, data);

                    dp = DBProjectMovements.GetRegisteredDiaryByDate(ProjectNo, data).Select(x => new ProjectDiaryViewModel()
                    {
                        LineNo = x.NºLinha,
                        ProjectNo = x.NºProjeto,
                        Date = x.Data == null ? String.Empty : x.Data.Value.ToString("yyyy-MM-dd"),
                        MovementType = x.TipoMovimento,
                        MovementTypeText = x.TipoMovimento != null ? EnumerablesFixed.ProjectDiaryMovements.Where(y => y.Id == x.TipoMovimento).FirstOrDefault() != null ? EnumerablesFixed.ProjectDiaryMovements.Where(y => y.Id == x.TipoMovimento).FirstOrDefault().Value : "" : "",
                        DocumentNo = x.NºDocumento,
                        Type = x.Tipo,
                        TypeText = x.Tipo != null ? EnumerablesFixed.ProjectDiaryTypes.Where(y => y.Id == x.Tipo).FirstOrDefault() != null ? EnumerablesFixed.ProjectDiaryTypes.Where(y => y.Id == x.Tipo).FirstOrDefault().Value : "" : "",
                        Code = x.Código,
                        Description = x.Descrição,
                        CodigoTipoTrabalho = x.CodigoTipoTrabalho,
                        Quantity = x.Quantidade.HasValue ? Math.Round((decimal)x.Quantidade, 2, MidpointRounding.AwayFromZero) : decimal.Zero,
                        MeasurementUnitCode = !string.IsNullOrEmpty(x.CódUnidadeMedida) ? MeasurementUnitList != null ? MeasurementUnitList.Where(y => y.Code == x.CódUnidadeMedida).FirstOrDefault() != null ? MeasurementUnitList.Where(y => y.Code == x.CódUnidadeMedida).FirstOrDefault().Description : "" : "Informação indisponível" : "",
                        LocationCode = !string.IsNullOrEmpty(x.CódLocalização) ? LocationList != null ? LocationList.Where(y => y.Code == x.CódLocalização).FirstOrDefault() != null ? LocationList.Where(y => y.Code == x.CódLocalização).FirstOrDefault().Name : "" : "Informação indisponível" : "",
                        ProjectContabGroup = x.GrupoContabProjeto,
                        RegionCode = x.CódigoRegião,
                        FunctionalAreaCode = x.CódigoÁreaFuncional,
                        ResponsabilityCenterCode = x.CódigoCentroResponsabilidade,
                        User = x.Utilizador,
                        UnitCost = x.CustoUnitário.HasValue ? Math.Round((decimal)x.CustoUnitário, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                        TotalCost = x.Quantidade.HasValue && x.CustoUnitário.HasValue ? Math.Round((decimal)(x.Quantidade * x.CustoUnitário), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //x.CustoTotal,
                        UnitPrice = x.PreçoUnitário.HasValue ? Math.Round((decimal)x.PreçoUnitário, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                        TotalPrice = x.Quantidade.HasValue && x.PreçoUnitário.HasValue ? Math.Round((decimal)(x.Quantidade * x.PreçoUnitário), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //x.PreçoTotal,
                        Billable = x.Faturável,
                        BillableText = x.Faturável.HasValue ? x.Faturável == true ? "Sim" : "Não" : "Não",
                        ResidueGuideNo = x.NºGuiaResíduos,
                        ExternalGuideNo = x.NºGuiaExterna,
                        InvoiceToClientNo = x.FaturaANºCliente,
                        RequestNo = x.NºRequisição,
                        RequestLineNo = x.NºLinhaRequisição,
                        Driver = x.Motorista,
                        MealType = x.TipoRefeição,
                        ResidueFinalDestinyCode = x.CódDestinoFinalResíduos,
                        OriginalDocument = x.DocumentoOriginal,
                        AdjustedDocument = x.DocumentoCorrigido,
                        AdjustedPrice = x.AcertoDePreços,
                        AdjustedPriceText = x.AcertoDePreços.HasValue ? x.AcertoDePreços == true ? "Sim" : "Não" : "Não",
                        AdjustedDocumentData = x.DataDocumentoCorrigido?.ToString("yyyy-MM-dd"),
                        AutorizatedInvoice = x.FaturaçãoAutorizada,
                        AutorizatedInvoiceText = x.FaturaçãoAutorizada.HasValue ? x.FaturaçãoAutorizada == true ? "Sim" : "Não" : "Não",
                        AutorizatedInvoice2 = x.FaturaçãoAutorizada2,
                        AutorizatedInvoice2Text = x.FaturaçãoAutorizada2.HasValue ? x.FaturaçãoAutorizada2 == true ? "Sim" : "Não" : "Não",
                        AutorizatedInvoiceData = x.DataAutorizaçãoFaturação?.ToString("yyyy-MM-dd"),
                        ServiceGroupCode = x.CódGrupoServiço,
                        ServiceGroupCodeDescription = AllServiceGroup != null ? AllServiceGroup.Where(y => y.ClientNumber == x.FaturaANºCliente && y.ServiceCode == x.CódGrupoServiço).FirstOrDefault() != null ? AllServiceGroup.Where(y => y.ClientNumber == x.FaturaANºCliente && y.ServiceCode == x.CódGrupoServiço).FirstOrDefault().ServiceDescription : "Informação indisponível" : "",
                        ResourceType = x.TipoRecurso,
                        FolhaHoras = x.NºFolhaHoras,
                        InternalRequest = x.RequisiçãoInterna,
                        EmployeeNo = x.NºFuncionário,
                        QuantityReturned = Convert.ToDecimal(x.QuantidadeDevolvida),
                        ConsumptionDate = x.DataConsumo?.ToString("yyyy-MM-dd"),
                        CreateDate = x.DataHoraCriação,
                        CreateDateText = x.DataHoraCriação.HasValue ? Convert.ToDateTime(x.DataHoraCriação).ToShortDateString() : "",
                        CreateHourText = x.DataHoraCriação.HasValue ? Convert.ToDateTime(x.DataHoraCriação).ToShortTimeString() : "",
                        UpdateDate = x.DataHoraModificação,
                        CreateUser = x.UtilizadorCriação,
                        UpdateUser = x.UtilizadorModificação,
                        Registered = x.Registado,
                        RegisteredText = x.Registado.HasValue ? x.Registado == true ? "Sim" : "Não" : "Não",
                        Billed = Convert.ToBoolean(x.Faturada),
                        BilledText = x.Faturada.HasValue ? x.Faturada == true ? "Sim" : "Não" : "Não",
                        Coin = x.Moeda,
                        UnitValueToInvoice = x.ValorUnitárioAFaturar,
                        ServiceClientCode = x.CódServiçoCliente,
                        ClientRequest = x.CodCliente,
                        LicensePlate = x.Matricula,
                        ReadingCode = x.CodigoLer,
                        Group = x.Grupo,
                        Operation = x.Operacao,
                        InvoiceGroup = x.GrupoFatura,
                        InvoiceGroupDescription = x.GrupoFaturaDescricao,
                        AuthorizedBy = x.AutorizadoPor,
                        ClientName = !string.IsNullOrEmpty(x.FaturaANºCliente) ? ClientsList != null ? ClientsList.Where(y => y.No_ == x.FaturaANºCliente).FirstOrDefault() != null ? ClientsList.Where(y => y.No_ == x.FaturaANºCliente).FirstOrDefault().Name : "" : "Informação indisponível" : "",
                        MealTypeDescription = x.TipoRefeição != null ? MealList != null ? MealList.Where(y => y.Código == x.TipoRefeição).FirstOrDefault() != null ? MealList.Where(y => y.Código == x.TipoRefeição).FirstOrDefault().Descrição : "" : "Informação indisponível" : "",
                        Utilizador = User.Identity.Name,
                        NameDB = _config.NAVDatabaseName,
                        CompanyName = _config.NAVCompanyName,
                        Fatura = x.Fatura,
                        ProductGroupCode = !string.IsNullOrEmpty(x.Código) ? AllProducts != null ? AllProducts.Where(y => y.Code == x.Código).FirstOrDefault() != null ? AllProducts.Where(y => y.Code == x.Código).FirstOrDefault().ProductGroupCode : "" : "Informação indisponível" : "",
                        TaxaIVA = x.TaxaIVA
                    }).OrderByDescending(x => x.Date).ToList();
                }

                return Json(dp);
            }
            catch (Exception ex)
            {
                return Json(null);
            }

            //foreach (ProjectDiaryViewModel item in dp)
            //{
            //    if (item.MealType != null)
            //    {
            //        TiposRefeição TRrow = DBMealTypes.GetById(item.MealType.Value);
            //        if (TRrow != null)
            //        {
            //            item.MealTypeDescription = TRrow.Descrição;
            //        }
            //    }
            //    else
            //    {
            //        item.MealTypeDescription = "";
            //    }
            //}
        }

        [HttpPost]
        public JsonResult GetProjectMovementsDp([FromBody] string ProjectNo, bool allProjs, string projectTarget, string NoDocument, string Resources, string ProjDiaryPrice, bool InverterSinal)
        {
            List<EnumData> TipoMovimentos = EnumerablesFixed.ProjectDiaryMovements;
            List<EnumData> Tipos = EnumerablesFixed.ProjectDiaryTypes;

            List<ProjectDiaryViewModel> dp = DBProjectMovements.GetRegisteredDiaryDp(ProjectNo, User.Identity.Name, allProjs).Select(x => new ProjectDiaryViewModel()
            {
                LineNo = x.NºLinha,
                ProjectNo = x.NºProjeto,
                Date = x.Data == null ? String.Empty : x.Data.Value.ToString("yyyy-MM-dd"),
                MovementType = x.TipoMovimento,
                MovementTypeText = x.TipoMovimento != null ? TipoMovimentos.Where(y => y.Id == x.TipoMovimento).FirstOrDefault().Value : "",
                Type = x.Tipo,
                TypeText = x.Tipo != null ? Tipos.Where(y => y.Id == x.Tipo).FirstOrDefault().Value : "",
                Code = x.Código,
                Description = x.Descrição,
                CodigoTipoTrabalho = x.CodigoTipoTrabalho,
                Quantity = InverterSinal == false ? x.Quantidade.HasValue ? Math.Round((decimal)x.Quantidade, 2, MidpointRounding.AwayFromZero) : decimal.Zero : x.Quantidade.HasValue ? Math.Round((decimal)x.Quantidade * -1, 2, MidpointRounding.AwayFromZero) : decimal.Zero,
                MeasurementUnitCode = x.CódUnidadeMedida,
                LocationCode = x.CódLocalização,
                ProjectContabGroup = x.GrupoContabProjeto,
                RegionCode = x.CódigoRegião,
                FunctionalAreaCode = x.CódigoÁreaFuncional,
                ResponsabilityCenterCode = x.CódigoCentroResponsabilidade,
                User = x.Utilizador,
                UnitCost = x.CustoUnitário.HasValue ? Math.Round((decimal)x.CustoUnitário, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                TotalCost = InverterSinal == false ? x.Quantidade.HasValue && x.CustoUnitário.HasValue ? Math.Round((decimal)(x.Quantidade * x.CustoUnitário), 2, MidpointRounding.AwayFromZero) : decimal.Zero : x.Quantidade.HasValue && x.CustoUnitário.HasValue ? Math.Round((decimal)(x.Quantidade * x.CustoUnitário), 2, MidpointRounding.AwayFromZero) * -1 : decimal.Zero,
                UnitPrice = x.PreçoUnitário.HasValue ? Math.Round((decimal)x.PreçoUnitário, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                TotalPrice = InverterSinal == false ? x.Quantidade.HasValue && x.PreçoUnitário.HasValue ? Math.Round((decimal)(x.Quantidade * x.PreçoUnitário), 2, MidpointRounding.AwayFromZero) : decimal.Zero : x.Quantidade.HasValue && x.PreçoUnitário.HasValue ? Math.Round((decimal)(x.Quantidade * x.PreçoUnitário), 2, MidpointRounding.AwayFromZero) * -1 : decimal.Zero,
                Billable = x.Faturável,
                Registered = x.Registado,
                DocumentNo = x.NºDocumento,
                ResidueFinalDestinyCode = x.CódDestinoFinalResíduos,

                ResidueGuideNo = x.NºGuiaResíduos,
                ExternalGuideNo = x.NºGuiaExterna,
                MealType = x.TipoRefeição,
                ServiceGroupCode = x.CódGrupoServiço,
                ConsumptionDate = x.DataConsumo == null ? String.Empty : x.DataConsumo.Value.ToString("yyyy-MM-dd"),
                ServiceClientCode = x.CódServiçoCliente,
            }).ToList();
            if (!string.IsNullOrEmpty(NoDocument))
            {
                dp = dp.Where(x => x.DocumentNo == NoDocument).ToList();
            }
            if (!string.IsNullOrEmpty(Resources) && Resources != "undefined")
            {
                dp = dp.Where(x => x.Code == Resources).ToList();
            }


            return Json(dp);
        }

        //[HttpPost]
        //public JsonResult GetJobLedgerEntries([FromBody] string ProjectNo)
        //{
        //List<NAVJobLedgerEntryViewModel> result = DBNAV2017JobLedgerEntries.GetFiltered(ProjectNo, null, _config.NAVDatabaseName, _config.NAVCompanyName);

        //    return Json(result);
        //}

        #endregion

        #region InvoiceAutorization
        public IActionResult AutorizacaoFaturacao(String id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.AutorizaçãoFaturação);
            ConfigUtilizadores Utilizador = DBUserConfigurations.GetById(User.Identity.Name);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.projectNo = id;
                ViewBag.UPermissions = UPerm;
                ViewBag.EditarPrecoUnitario = Utilizador.EditarPrecoUnitario.HasValue ? Utilizador.EditarPrecoUnitario.Value : false;
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetAutorizacaoFaturacao([FromBody] JObject requestParams)
        {
            Result result = new Result();
            string projectNo = requestParams["projectNo"].ToString();

            bool billable = true;
            JValue billableValue = requestParams["billable"] as JValue;
            if (billableValue != null)
                billable = (bool)billableValue.Value;


            var project = DBProjects.GetById(projectNo);

            if (project != null)
            {
                try
                {
                    var projectContract = DBContracts.GetByIdLastVersion(project.NºContrato);

                    List<ProjectMovementViewModel> projectMovements = GetProjectMovements(projectNo, project.NºCliente, billable);

                    if (project.Estado.HasValue && (project.Estado == EstadoProjecto.Encomenda ||
                        project.Estado == EstadoProjecto.Terminado))
                    {
                        result.eReasonCode = 1;
                    }
                    else
                    {
                        result.eReasonCode = 2;
                        result.eMessage = "O estado do projeto não permite autorizar a faturação.";
                    }
                    result.Value = projectMovements;
                }
                catch (Exception ex)
                {
                    result.eReasonCode = 2;
                    result.eMessage = "Ocorreu um erro ao obter os movimentos.";
                    result.eMessages.Add(new TraceInformation(TraceType.Exception, ex.Message));
                    result.Value = new List<ProjectMovementViewModel>();
                }
            }
            else
            {
                result.eReasonCode = 2;
                result.eMessage = "Não foi possivel obter o projecto.";
                result.Value = new List<ProjectMovementViewModel>();
            }
            return Json(result);
        }

        private List<ProjectMovementViewModel> GetProjectMovements(string projectNo, string customerNo, bool? billable)
        {
            List<ProjectMovementViewModel> projectMovements = DBProjectMovements.GetProjectMovementsFor(projectNo, billable)
                        .ParseToViewModel(_config.NAVDatabaseName, _config.NAVCompanyName)
                        .OrderBy(x => x.ClientName).ToList();

            if (projectMovements.Count > 0)
            {
                List<ClientServicesViewModel> customerServices = string.IsNullOrEmpty(customerNo) ? new List<ClientServicesViewModel>() : DBClientServices.GetAllClientService(customerNo, false);

                foreach (var mov in projectMovements)
                {
                    if (mov.MovementType == 3)
                    {
                        mov.Quantity = Math.Abs(mov.Quantity.HasValue ? Math.Round((decimal)mov.Quantity, 2, MidpointRounding.AwayFromZero) : decimal.Zero) * (-1);
                    }

                    if (!String.IsNullOrEmpty(mov.Currency))
                    {
                        mov.UnitPrice = mov.UnitValueToInvoice.HasValue ? Math.Round((decimal)mov.UnitValueToInvoice, 4, MidpointRounding.AwayFromZero) : decimal.Zero;
                    }

                    if (!string.IsNullOrEmpty(mov.ServiceClientCode))
                        mov.ServiceClientDescription = customerServices.Where(x => x.ServiceCode == mov.ServiceClientCode).Select(x => x.ServiceDescription).FirstOrDefault();

                    mov.CreateDateText = mov.CreateDate.HasValue ? Convert.ToDateTime(mov.CreateDate).ToShortDateString() : "";
                    mov.CreateHourText = mov.CreateDate.HasValue ? Convert.ToDateTime(mov.CreateDate).ToShortTimeString() : "";
                }
                var userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
                List<UserDimensionsViewModel> userDimensionsViewModel = userDimensions.ParseToViewModel();
                if (userDimensionsViewModel.Where(x => x.Dimension == (int)Dimensions.Region).Count() > 0)
                    projectMovements.RemoveAll(x => !userDimensionsViewModel.Any(y => y.DimensionValue == x.RegionCode));
                if (userDimensionsViewModel.Where(x => x.Dimension == (int)Dimensions.FunctionalArea).Count() > 0)
                    projectMovements.RemoveAll(x => !userDimensionsViewModel.Any(y => y.DimensionValue == x.FunctionalAreaCode));
                if (userDimensionsViewModel.Where(x => x.Dimension == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                    projectMovements.RemoveAll(x => !userDimensionsViewModel.Any(y => y.DimensionValue == x.ResponsabilityCenterCode));
            }
            return projectMovements;
        }

        [HttpPost]
        public JsonResult GetProjectBillingResume([FromBody] JObject requestParams)
        {
            string projectNo = requestParams["projectNo"].ToString();

            List<ProjectMovementViewModel> projectMovements = GetProjectMovements(projectNo, null, null);
            //prevent errors
            if (projectMovements == null)
                projectMovements = new List<ProjectMovementViewModel>();

            dynamic projBillingResume = new JObject();

            projBillingResume.TotalBillableConsumption = projectMovements
                .Where(x => x.TotalPrice.HasValue &&
                            x.Type == (int)ProjectDiaryMovementTypes.Consumo)
                .Select(x => x.TotalPrice.Value)
                .Sum();
            projBillingResume.AuthorizedBilling = projectMovements
                .Where(x => x.TotalPrice.HasValue &&
                            x.Type == (int)ProjectDiaryMovementTypes.Consumo &&
                            x.AutorizatedInvoice == true)
                .Select(x => x.TotalPrice.Value)
                .Sum();
            projBillingResume.BillingToAuthorize = projectMovements
                .Where(x => x.TotalPrice.HasValue &&
                            x.Type == (int)ProjectDiaryMovementTypes.Consumo &&
                            x.AutorizatedInvoice == false &&
                            x.Billable == true)
                .Select(x => x.TotalPrice.Value)
                .Sum();
            projBillingResume.RegisteredInvoiceValue = projectMovements
                .Where(x => x.TotalPrice.HasValue &&
                            x.Type == (int)ProjectDiaryMovementTypes.Venda)
                .Select(x => x.TotalPrice.Value)
                .Sum();

            decimal? totalInvoiceValue = DBNAV2017Projects.GetTotalInvoiceValue(_config.NAVDatabaseName, _config.NAVCompanyName, projectNo);
            projBillingResume.CreatedInvoicesValue = totalInvoiceValue;

            return Json(projBillingResume);
        }

        private class InvoiceMessages
        {
            public bool Iserror { get; set; }
            public string ClientNo { get; set; }
        }

        [HttpPost]
        public JsonResult GetAllFaturasRelacionadas([FromBody] JObject requestParams)
        {
            JToken customerNoValue;
            string customerNo = string.Empty;
            if (requestParams != null)
            {
                if (requestParams.TryGetValue("customerNo", out customerNoValue))
                    customerNo = (string)customerNoValue;
            }
            List<NAVClientesInvoicesViewModel> result = DBNAV2017Clients.GetInvoicesFatura(_config.NAVDatabaseName, _config.NAVCompanyName, customerNo);

            return Json(result);
        }

        [HttpPost]
        public JsonResult AuthorizeProjectMovements([FromBody] JObject requestParams)
        {
            ErrorHandler result = ValidateMovements(requestParams);
            if (result.eReasonCode != 1)
                return Json(result);

            result.eReasonCode = 2;
            result.eMessage = "Ocorreu um erro ao autorizar.";

            try
            {
                #region HTTP Params
                string projectNo = string.Empty;
                JValue projectNoValue = requestParams["projectNo"] as JValue;
                if (projectNoValue != null)
                    projectNo = (string)projectNoValue.Value;

                string commitmentNumber = string.Empty;
                JValue commitmentNumberValue = requestParams["commitmentNumber"] as JValue;
                if (commitmentNumberValue != null)
                    commitmentNumber = (string)commitmentNumberValue.Value;

                string invoiceGroupDescription = string.Empty;
                JValue invoiceGroupDescriptionValue = requestParams["invoiceGroupDescription"] as JValue;
                if (invoiceGroupDescriptionValue != null)
                    invoiceGroupDescription = (string)invoiceGroupDescriptionValue.Value;

                string customerRequestNo = string.Empty;
                JValue customerRequestNoValue = requestParams["customerRequestNo"] as JValue;
                if (customerRequestNoValue != null)
                    customerRequestNo = (string)customerRequestNoValue.Value;

                DateTime customerRequestDate = DateTime.MinValue;
                JValue customerRequestDateValue = requestParams["customerRequestDate"] as JValue;
                if (customerRequestDateValue != null)
                    DateTime.TryParse((string)customerRequestDateValue.Value, out customerRequestDate);

                DateTime serviceDate = DateTime.MinValue;
                JValue serviceDateValue = requestParams["serviceDate"] as JValue;
                if (serviceDateValue != null)
                    DateTime.TryParse((string)serviceDateValue.Value, out serviceDate);

                DateTime serviceDateFim = DateTime.MinValue;
                JValue serviceDateFimValue = requestParams["serviceDateFim"] as JValue;
                if (serviceDateFimValue != null)
                    DateTime.TryParse((string)serviceDateFimValue.Value, out serviceDateFim);

                string billingPeriod = string.Empty;
                JValue billingPeriodValue = requestParams["dataServPrestado"] as JValue;
                if (billingPeriodValue != null)
                    billingPeriod = (string)billingPeriodValue.Value;

                decimal authorizationTotal = 0;
                JValue authorizationTotalValue = requestParams["authorizationTotalValue"] as JValue;
                if (authorizationTotalValue != null)
                {
                    string str = authorizationTotalValue.Value as string;
                    if (!string.IsNullOrEmpty(str))
                        authorizationTotal = decimal.Parse(str, CultureInfo.InvariantCulture);
                }

                string projectObs = string.Empty;
                JValue projectObsValue = requestParams["projObs"] as JValue;
                if (projectObsValue != null)
                    projectObs = (string)projectObsValue.Value;

                List<ProjectMovementViewModel> projMovements = new List<ProjectMovementViewModel>();
                JArray projMovementsValue = requestParams["projMovements"] as JArray;
                if (projMovementsValue != null)
                    projMovements = projMovementsValue.ToObject<List<ProjectMovementViewModel>>();

                string noFaturaRelacionada = string.Empty;
                JValue noFaturaRelacionadaValue = requestParams["noFaturaRelacionada"] as JValue;
                if (noFaturaRelacionadaValue != null)
                    noFaturaRelacionada = (string)noFaturaRelacionadaValue.Value;

                #endregion

                Projetos project = null;
                Contratos contract = null;
                NAVClientsViewModel customer = null;
                string customerName = string.Empty;

                if (!string.IsNullOrEmpty(projectNo))
                    project = DBProjects.GetById(projectNo);

                if (project != null)
                {
                    contract = DBContracts.GetByIdLastVersion(project.NºContrato);
                    customer = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, project.NºCliente);

                    if (customer != null && !string.IsNullOrEmpty(customer.Name))
                        customerName = customer.Name;

                    if (authorizationTotal < 0 && !string.IsNullOrEmpty(noFaturaRelacionada))
                    {
                        NAVClientesInvoicesViewModel Encomenda = DBNAV2017Clients.GetInvoices(_config.NAVDatabaseName, _config.NAVCompanyName, project.NºCliente).Where(x => x.No_ == noFaturaRelacionada).FirstOrDefault();
                        if (Encomenda != null)
                        {
                            commitmentNumber = Encomenda.NoCompromisso;
                            customerRequestNo = Encomenda.NoPedido;
                            customerRequestDate = Convert.ToDateTime(Encomenda.DocumentDate);
                        }
                    }

                    using (SuchDBContext ctx = new SuchDBContext())
                    {
                        int? lastUsed = ctx.ProjectosAutorizados
                            .Where(x => x.CodProjeto == projectNo)
                            .OrderByDescending(x => x.GrupoFactura)
                            .Select(x => x.GrupoFactura)
                            .FirstOrDefault();

                        int invoiceGroup = lastUsed.HasValue ? lastUsed.Value + 1 : 1;

                        ProjectosAutorizados authorizedProject = new ProjectosAutorizados
                        {
                            CodProjeto = project.NºProjeto,
                            GrupoFactura = invoiceGroup,
                            Faturado = false,
                            DescricaoGrupo = invoiceGroupDescription,
                            NumCompromisso = commitmentNumber,
                            CodCliente = project.NºCliente,
                            NomeCliente = customerName,
                            CodContrato = contract?.NºDeContrato,
                            CodTermosPagamento = contract != null ? contract.CódTermosPagamento : customer?.PaymentTermsCode,
                            CodMetodoPagamento = customer?.PaymentMethodCode,
                            CodRegiao = customer.National || customer.InternalClient ? project.CódigoRegião : customer.RegionCode,
                            CodAreaFuncional = project.CódigoÁreaFuncional,
                            CodCentroResponsabilidade = project.CódigoCentroResponsabilidade,
                            PedidoCliente = customerRequestNo,
                            DataAutorizacao = DateTime.Now,
                            Utilizador = User.Identity.Name,
                            Observacoes = projectObs,
                            DataServPrestado = billingPeriod,
                            DataPedido = customerRequestDate > DateTime.MinValue ? customerRequestDate : (DateTime?)null,
                            DataPrestacaoServico = serviceDate > DateTime.MinValue ? serviceDate : (DateTime?)null,
                            DataPrestacaoServicoFim = serviceDateFim > DateTime.MinValue ? serviceDateFim : (DateTime?)null,
                            CodEnderecoEnvio = !string.IsNullOrEmpty(project.CódEndereçoEnvio) ? project.CódEndereçoEnvio : "",
                            ValorAutorizado = Math.Round((decimal)authorizationTotal, 2, MidpointRounding.AwayFromZero),
                            GrupoContabilisticoProjeto = project.TipoGrupoContabProjeto.HasValue ? project.TipoGrupoContabProjeto.ToString() : string.Empty,
                            NoFaturaRelacionada = !string.IsNullOrEmpty(noFaturaRelacionada) ? noFaturaRelacionada : string.Empty
                        };

                        //Atualizar apenas os campos relativos à autorização. Nos movimentos de projeto atualizar apenas os necessários à listagem/apresentação
                        List<MovimentosProjectoAutorizados> authorizedProjMovements = new List<MovimentosProjectoAutorizados>();
                        var allUnchangedMovementsIds = projMovements.Select(x => x.LineNo).ToList();
                        var unchangedProjectMovements = ctx.MovimentosDeProjeto.Where(x => allUnchangedMovementsIds.Contains(x.NºLinha)).ToList();

                        projMovements.ForEach(x =>
                        {

                            //x.AutorizatedInvoice = true;
                            //x.AutorizatedInvoiceDate = DateTime.Now.ToString("yyyy-MM-dd");
                            //x.AuthorizedBy = User.Identity.Name;
                            //x.InvoiceGroup = invoiceGroup;
                            //x.InvoiceGroupDescription = invoiceGroupDescription;
                            //x.UpdateDate = DateTime.Now;
                            //x.UpdateUser = User.Identity.Name;

                            var projectMovementToUpdate = unchangedProjectMovements.First(y => y.NºLinha == x.LineNo);
                            var index = unchangedProjectMovements.IndexOf(projectMovementToUpdate);

                            projectMovementToUpdate.FaturaçãoAutorizada = true;
                            projectMovementToUpdate.DataAutorizaçãoFaturação = DateTime.Now;
                            projectMovementToUpdate.AutorizadoPor = User.Identity.Name;
                            projectMovementToUpdate.GrupoFatura = invoiceGroup;
                            projectMovementToUpdate.GrupoFaturaDescricao = invoiceGroupDescription;
                            projectMovementToUpdate.DataHoraModificação = DateTime.Now;
                            projectMovementToUpdate.UtilizadorModificação = User.Identity.Name;

                            if (index != -1)
                                unchangedProjectMovements[index] = projectMovementToUpdate;

                            //Create Movement Project Authorized ::RUI
                            MovimentosProjectoAutorizados authorizedProjMovement = new MovimentosProjectoAutorizados();
                            authorizedProjMovement.NumMovimento = x.LineNo;
                            authorizedProjMovement.DataRegisto = Convert.ToDateTime(x.Date);
                            authorizedProjMovement.Tipo = x.Type ?? 0;
                            authorizedProjMovement.Codigo = x.Code;
                            authorizedProjMovement.Descricao = x.Description;
                            authorizedProjMovement.Quantidade = x.Quantity.HasValue ? Math.Round((decimal)x.Quantity, 2, MidpointRounding.AwayFromZero) : decimal.Zero; //?? 0;
                            authorizedProjMovement.CodUnidadeMedida = x.MeasurementUnitCode;
                            authorizedProjMovement.PrecoVenda = x.UnitPrice.HasValue ? Math.Round((decimal)x.UnitPrice, 4, MidpointRounding.AwayFromZero) : decimal.Zero; // ?? 0;
                            authorizedProjMovement.PrecoTotal = x.Quantity.HasValue && x.UnitPrice.HasValue ? Math.Round((decimal)(x.Quantity * x.UnitPrice), 2, MidpointRounding.AwayFromZero) : decimal.Zero; // ?? 0; //x.TotalPrice;
                            authorizedProjMovement.CodProjeto = x.ProjectNo;
                            authorizedProjMovement.CodRegiao = x.RegionCode;
                            authorizedProjMovement.CodAreaFuncional = x.FunctionalAreaCode;
                            authorizedProjMovement.CodCentroResponsabilidade = x.ResponsabilityCenterCode;
                            authorizedProjMovement.CodContrato = contract?.NºDeContrato;
                            authorizedProjMovement.CodGrupoServico = x.ServiceGroupCode;
                            authorizedProjMovement.CodServCliente = x.ServiceClientCode;
                            authorizedProjMovement.DescServCliente = x.ServiceClientDescription;
                            authorizedProjMovement.NumGuiaResiduosGar = x.ResidueGuideNo;
                            authorizedProjMovement.TipoRefeicao = x.MealType ?? 0;
                            authorizedProjMovement.TipoRecurso = x.ResourceType ?? 0;
                            authorizedProjMovement.NumDocumento = x.DocumentNo;
                            authorizedProjMovement.PrecoCusto = x.UnitCost.HasValue ? Math.Round((decimal)x.UnitCost, 4, MidpointRounding.AwayFromZero) : decimal.Zero;
                            authorizedProjMovement.CustoTotal = x.Quantity.HasValue && x.UnitCost.HasValue ? Math.Round((decimal)(x.Quantity * x.UnitCost), 2, MidpointRounding.AwayFromZero) : decimal.Zero; // ?? 0; //x.TotalCost;
                            authorizedProjMovement.CodCliente = x.InvoiceToClientNo;
                            authorizedProjMovement.GrupoFactura = invoiceGroup;

                            if (x.OriginalDocument != null && x.DocumentNo != "")
                                authorizedProjMovement.NumGuiaExterna = x.OriginalDocument;
                            else if (x.AdjustedDocument != null && x.AdjustedDocument != "")
                            {
                                authorizedProjMovement.NumGuiaExterna = x.AdjustedDocument;
                                authorizedProjMovement.DataConsumo = Convert.ToDateTime(x.AdjustedDocumentDate);
                            }
                            else
                            {
                                authorizedProjMovement.NumGuiaExterna = x.ExternalGuideNo;
                                authorizedProjMovement.DataConsumo = (x.ConsumptionDate == null || x.ConsumptionDate == "") ? Convert.ToDateTime(x.Date) : Convert.ToDateTime(x.ConsumptionDate);
                            }
                            authorizedProjMovements.Add(authorizedProjMovement);
                            // END RUI
                        });

                        authorizedProject.ValorAutorizado = Math.Round(authorizedProjMovements.Sum(x => x.PrecoTotal), 2, MidpointRounding.AwayFromZero);

                        ctx.ProjectosAutorizados.Add(authorizedProject);
                        try
                        {
                            ctx.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            result.eReasonCode = 2;
                            result.eMessage = "Ocorreu um erro ao adicionar o Projeto na Tabela dos Projetos Autorizados.";
                            return Json(result);
                        }

                        ctx.MovimentosProjectoAutorizados.AddRange(authorizedProjMovements);
                        try
                        {
                            ctx.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            result.eReasonCode = 2;
                            result.eMessage = "Ocorreu um erro ao adicionar os Movimentos na Tabela Movimentos de Projeto Autorizados.";
                            return Json(result);
                        }

                        ctx.MovimentosDeProjeto.UpdateRange(unchangedProjectMovements);
                        try
                        {
                            ctx.SaveChanges();
                            result.eReasonCode = 1;
                            result.eMessage = "Movimentos autorizados com o Grupo Fatura " + invoiceGroup.ToString();
                        }
                        catch (Exception ex)
                        {
                            result.eReasonCode = 2;
                            result.eMessage = "Ocorreu um erro ao atualizar os Movimentos de Projeto.";
                            return Json(result);
                        }
                    }
                }
                else
                {
                    result.eReasonCode = 2;
                    result.eMessage = "Não foi possivel obter o projeto.";
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                result.eReasonCode = 2;
                result.eMessage = "Ocorreu um erro ao autorizar.";
                return Json(result);
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult ValidateMovementsForAuthorization([FromBody] JObject requestParams)
        {
            var result = ValidateMovements(requestParams);

            return Json(result);
        }

        private ErrorHandler ValidateMovements(JObject requestParams)
        {
            ErrorHandler result = new ErrorHandler();
            try
            {
                string projectNo = string.Empty;
                JValue projectNoValue = requestParams["projectNo"] as JValue;
                if (projectNoValue != null)
                    projectNo = (string)projectNoValue.Value;

                string commitmentNumber = string.Empty;
                JValue commitmentNumberValue = requestParams["commitmentNumber"] as JValue;
                if (commitmentNumberValue != null)
                    commitmentNumber = (string)commitmentNumberValue.Value;

                string invoiceGroupDescription = string.Empty;
                JValue invoiceGroupDescriptionValue = requestParams["invoiceGroupDescription"] as JValue;
                if (invoiceGroupDescriptionValue != null)
                    invoiceGroupDescription = (string)invoiceGroupDescriptionValue.Value;

                string customerRequestNo = string.Empty;
                JValue customerRequestNoValue = requestParams["customerRequestNo"] as JValue;
                if (customerRequestNoValue != null)
                    customerRequestNo = (string)customerRequestNoValue.Value;

                DateTime customerRequestDate = DateTime.MinValue;
                JValue customerRequestDateValue = requestParams["customerRequestDate"] as JValue;
                if (customerRequestDateValue != null)
                    DateTime.TryParse((string)customerRequestDateValue.Value, out customerRequestDate);

                DateTime serviceDate;
                JValue serviceDateValue = requestParams["serviceDate"] as JValue;
                if (serviceDateValue != null)
                    DateTime.TryParse((string)serviceDateValue.Value, out serviceDate);

                DateTime serviceDateFim = DateTime.MinValue;
                JValue serviceDateFimValue = requestParams["serviceDateFim"] as JValue;
                if (serviceDateFimValue != null)
                    DateTime.TryParse((string)serviceDateFimValue.Value, out serviceDateFim);

                decimal authorizationTotal = 0;
                JValue authorizationTotalValue = requestParams["authorizationTotalValue"] as JValue;
                if (authorizationTotalValue != null)
                {
                    string str = authorizationTotalValue.Value as string;
                    if (!string.IsNullOrEmpty(str))
                        authorizationTotal = decimal.Parse(str, CultureInfo.InvariantCulture);
                }

                List<ProjectMovementViewModel> projMovements = new List<ProjectMovementViewModel>();
                JArray projMovementsValue = requestParams["projMovements"] as JArray;
                if (projMovementsValue != null)
                    projMovements = projMovementsValue.ToObject<List<ProjectMovementViewModel>>();

                //string noFaturaRelacionada = string.Empty;
                //JValue noFaturaRelacionadaValue = requestParams["noFaturaRelacionada"] as JValue;
                //if (noFaturaRelacionadaValue != null)
                //    noFaturaRelacionada = (string)noFaturaRelacionadaValue.Value;

                string billingPeriod = string.Empty;
                JValue billingPeriodValue = requestParams["dataServPrestado"] as JValue;
                if (billingPeriodValue != null)
                    billingPeriod = (string)billingPeriodValue.Value;

                List<NAVResourcesViewModel> AllResources = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, "", "", 0, "").ToList();
                NAVResourcesViewModel Resource = new NAVResourcesViewModel();

                bool bArea = false;
                bool bRefeicao = true;
                bool bQuantidade = true;
                bool bPreco = true;
                projMovements.ForEach(x =>
                {
                    if (string.IsNullOrEmpty(x.FunctionalAreaCode) && bArea == true)
                    {
                        result.eMessages.Add(new TraceInformation(TraceType.Error, "O Campo Área Funcional é de preenchimento obrigatório"));
                        bArea = false;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(x.FunctionalAreaCode) && x.FunctionalAreaCode.StartsWith("5") && !x.MealType.HasValue && bRefeicao == true)
                        {
                            result.eMessages.Add(new TraceInformation(TraceType.Error, "O tipo de refeição é obrigatório nas linhas com Área Funcional Nutrição"));
                            bRefeicao = false;
                        }
                    }

                    if (x.Quantity.HasValue && x.Quantity == 0 && bQuantidade == true)
                    {
                        result.eMessages.Add(new TraceInformation(TraceType.Exception, "Existem Movimentos com Quantidade a 0"));
                        bQuantidade = false;
                    }

                    if (x.UnitPrice.HasValue && x.UnitPrice == 0 && bPreco == true)
                    {
                        result.eMessages.Add(new TraceInformation(TraceType.Exception, "Existem Movimentos com Preço Unitário a 0"));
                        bPreco = false;
                    }

                    if (x.Type.HasValue && x.Type == 2 && !string.IsNullOrEmpty(x.Code)) //Recurso
                    {
                        Resource = AllResources.Where(y => y.Code == x.Code).FirstOrDefault();

                        if (Resource != null && string.IsNullOrEmpty(Resource.GenProdPostingGroup))
                        {
                            result.eMessages.Add(new TraceInformation(TraceType.Error, "Falta a configuração do Grupo Contabilístico no Recurso " + x.Code));
                        }
                    }
                });

                Projetos project = null;
                Contratos contract = null;
                NAVClientsViewModel customer = null;

                if (!string.IsNullOrEmpty(projectNo))
                {
                    project = DBProjects.GetById(projectNo);
                    if (project != null)
                    {
                        //21-11-2018
                        //CORRIGIR COM O MARCO MARCELO AS REGRAS DE AUTORIZAÇÃO
                        if (project.TipoGrupoContabProjeto.HasValue)
                        {
                            var contabGroupType = DBCountabGroupTypes.GetById(project.TipoGrupoContabProjeto.Value);
                            if (contabGroupType != null)
                            {
                                if (!string.IsNullOrEmpty(contabGroupType.CódigoÁreaFuncional) && contabGroupType.CódigoÁreaFuncional != project.CódigoÁreaFuncional)
                                {
                                    result.eMessages.Add(new TraceInformation(TraceType.Error, "Verifique a configuração da Área! (Tipos Grupo Contab Projeto"));
                                }
                            }
                            else
                            {
                                result.eMessages.Add(new TraceInformation(TraceType.Error, "O Tipo Grupo Contab. Projeto do projeto é inválido."));
                            }
                        }
                        contract = DBContracts.GetByIdLastVersion(project.NºContrato);
                        customer = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, project.NºCliente);
                    }
                }

                if (customer != null)
                {
                    if (string.IsNullOrEmpty(customer.RegionCode))
                        result.eMessages.Add(new TraceInformation(TraceType.Error, "É necessário configurar a região na Ficha do Cliente."));

                    //if (authorizationTotal < 0)
                    //{
                    //    List<NAVClientesInvoicesViewModel> AllInvoices = DBNAV2017Clients.GetInvoices(_config.NAVDatabaseName, _config.NAVCompanyName, project.NºCliente);

                    //    if (AllInvoices != null && AllInvoices.Count > 0)
                    //    {
                    //        if (!string.IsNullOrEmpty(noFaturaRelacionada))
                    //        {
                    //            NAVClientesInvoicesViewModel Invoice = AllInvoices.Where(y => y.No_ == noFaturaRelacionada).FirstOrDefault();

                    //            if (Invoice == null)
                    //            {
                    //                result.eMessages.Add(new TraceInformation(TraceType.Error, "Não foi encontrada a Fatura Relacionada no NAV2017."));
                    //            }
                    //        }
                    //        else
                    //        {
                    //            result.eMessages.Add(new TraceInformation(TraceType.Error, "O campo Nº Fatura Relacionada é de preenchimento obrigatório."));
                    //        }
                    //    }
                    //}
                }
                else
                    result.eMessages.Add(new TraceInformation(TraceType.Error, "Ocorreu um erro ao validar o cliente."));

                if (project != null)
                {
                    //Apenas movimentos de projeto faturáveis.
                    var notBillableItems = projMovements.Where(x => !x.Billable.Value).Select(x => x.LineNo);
                    if (notBillableItems.Count() > 0)
                        result.eMessages.Add(new TraceInformation(TraceType.Error, "Apenas podem ser autorizados movimentos de projeto faturáveis (ver movimentos: " + string.Join(',', notBillableItems) + ")."));

                    //Não permitir faturar mais do que uma vez
                    var authorizedItems = projMovements.Where(x => x.AutorizatedInvoice.Value);
                    if (authorizedItems.Count() > 0)
                        result.eMessages.Add(new TraceInformation(TraceType.Error, "Existem movimentos que já foram autorizados (ver movimentos: " + string.Join(',', authorizedItems) + ")."));

                    //Procurar por Projetos Autorizados iguais
                    List<ProjectosAutorizados> ProjetosAutorizados = DBAuthotizedProjects.GetAll();
                    if (ProjetosAutorizados != null && ProjetosAutorizados.Count > 0)
                    {
                        List<ProjectosAutorizados> ProjetosAutorizadosIguais = new List<ProjectosAutorizados>();

                        ProjetosAutorizadosIguais = ProjetosAutorizados.Where(x => x.CodProjeto == projectNo &&
                        x.CodCliente == project.NºCliente &&
                        x.ValorAutorizado == Math.Round((decimal)authorizationTotal, 2, MidpointRounding.AwayFromZero) &&
                        x.DataServPrestado == billingPeriod &&
                        x.PedidoCliente == customerRequestNo
                        && x.DataPedido == (customerRequestDate > DateTime.MinValue ? customerRequestDate : (DateTime?)null)).ToList();

                        if (ProjetosAutorizadosIguais != null && ProjetosAutorizadosIguais.Count > 0)
                        {
                            result.eMessages.Add(new TraceInformation(TraceType.Error, "Já existe uma Autorização de Faturação para estes Movimentos."));
                        }
                    }

                    if (contract != null)
                    {
                        //Validar se o contrato indicado no projeto está vigente
                        if (contract.DataInicial.HasValue && contract.DataFimContrato.HasValue &&
                            (DateTime.Now < contract.DataInicial.Value || DateTime.Now > contract.DataFimContrato.Value))
                        {
                            result.eMessages.Add(new TraceInformation(TraceType.Warning, "O Contrato não está vigente."));
                        }
                        //Validar se o compromisso é o que está no contrato                
                        //contract.NºCompromisso = !string.IsNullOrEmpty(contract.NºCompromisso) ? contract.NºCompromisso : "";
                        //if (commitmentNumber != contract.NºCompromisso)
                        commitmentNumber = !string.IsNullOrEmpty(commitmentNumber) ? commitmentNumber : "";
                        List<RequisiçõesClienteContrato> AllRequisicoes = new List<RequisiçõesClienteContrato>();
                        RequisiçõesClienteContrato Requisicao = new RequisiçõesClienteContrato();
                        string REQCompromisso = string.Empty;
                        AllRequisicoes = DBContractClientRequisition.GetByContract(project.NºContrato);
                        if (AllRequisicoes != null && AllRequisicoes.Count > 0)
                        {
                            Requisicao = AllRequisicoes.FirstOrDefault(x => (x.DataInícioCompromisso <= serviceDateFim && x.DataFimCompromisso >= serviceDateFim) && x.NºProjeto == project.NºProjeto);
                            if (Requisicao == null)
                            {
                                Requisicao = AllRequisicoes.FirstOrDefault(x => (x.DataInícioCompromisso <= serviceDateFim && x.DataFimCompromisso >= serviceDateFim) && string.IsNullOrEmpty(x.NºProjeto));
                            }
                            REQCompromisso = Requisicao != null && !string.IsNullOrEmpty(Requisicao.NºCompromisso) ? Requisicao.NºCompromisso : "";
                        }
                        if (commitmentNumber != REQCompromisso)
                        {
                            result.eMessages.Add(new TraceInformation(TraceType.Warning, "O Nº do Compromisso é diferente do que está no Contrato."));
                        }

                        if (commitmentNumber.Length > 50)
                        {
                            result.eMessages.Add(new TraceInformation(TraceType.Error, "O Nº Compromisso não pode ter mais de 50 carateres."));
                        }
                    }

                    //Validar se o cliente está ao abrigo da lei dos compromissos
                    if (string.IsNullOrEmpty(commitmentNumber) && customer != null)
                    {
                        if (customer.UnderCompromiseLaw)
                            result.eMessages.Add(new TraceInformation(TraceType.Error, "O cliente está ao abrigo da lei de compromissos."));
                        else
                            result.eMessages.Add(new TraceInformation(TraceType.Warning, "Não foi indicado um Nº do Compromisso."));
                    }

                    Configuração conf = DBConfigurations.GetById(1);
                    if (conf != null)
                    {
                        string wasteAreaId = conf.CodAreaResiduos;
                        if (!string.IsNullOrEmpty(wasteAreaId))
                        {
                            //No caso da área dos Resíduos, obrigar o preenchimento do campo “Data Consumo” em todas as linhas.
                            var wasteAreaMovements = projMovements.Where(x => x.FunctionalAreaCode == wasteAreaId).ToList();
                            var invalidWasteAreaMovementIds = wasteAreaMovements.Where(x => string.IsNullOrEmpty(x.ConsumptionDate)).Select(x => x.LineNo);
                            if (invalidWasteAreaMovementIds.Count() > 0)
                                result.eMessages.Add(new TraceInformation(TraceType.Error, "O preenchimento do campo 'Data Consumo Guia' é obrigatório nos movimentos da áreas de Residuos (ver movimentos a Autorizar)."));
                        }
                        else
                            result.eMessages.Add(new TraceInformation(TraceType.Error, "A área de residuos não está configurada. Contacte o administrador."));
                    }
                    else
                        result.eMessages.Add(new TraceInformation(TraceType.Error, "Ocorreu um erro ao validar a área de residuos."));
                }
                else
                    result.eMessages.Add(new TraceInformation(TraceType.Error, "Não foi possivel obter detalhes do projeto."));
            }
            catch (Exception ex)
            {
                result.eMessages.Add(new TraceInformation(TraceType.Error, "Ocorreu um erro ao validar os movimentos: " + ex.Message + "."));
            }
            bool hasErrors = result.eMessages.Any(x => x.Type == TraceType.Error);
            if (hasErrors || result.eReasonCode > 1)
            {
                result.eReasonCode = 2;
                result.eMessages.Add(new TraceInformation(TraceType.Error, "Foram detetados erros nos movimentos submetidos."));
            }
            else
            {
                result.eReasonCode = 1;
                result.eMessage = "Não foram detetados erros de validação nos movimentos submetidos.";
            }
            return result;
        }

        [HttpPost]
        public JsonResult GetCommitmentDetails([FromBody] JObject requestParams)
        {
            Result result = new Result();
            dynamic commitmentDetails = new JObject();
            string commitmentNo = string.Empty;
            string customerRequestNo = string.Empty;
            string customerRequestDate = string.Empty;
            bool settedFromProjectOrContract = false;
            Projetos project;
            Contratos contract = null;


            string projectNo = requestParams["projectNo"].ToString();
            string serviceDeliveryValue = requestParams["serviceDeliveryDate"].ToString();
            DateTime serviceDeliveryDate = DateTime.Parse(serviceDeliveryValue);

            project = DBProjects.GetById(projectNo);
            if (project != null && !string.IsNullOrEmpty(project.NºContrato))
                contract = DBContracts.GetByIdLastVersion(project.NºContrato);

            if (project == null)
            {
                result.eReasonCode = 2;
                result.eMessage = "Não foi possivel obter dados do projeto.";
            }
            else
            {
                //get from project then from contract
                if (!string.IsNullOrEmpty(project.NºCompromisso))
                {
                    commitmentNo = project.NºCompromisso;
                    settedFromProjectOrContract = true;
                }
                else
                {
                    if (!string.IsNullOrEmpty(contract?.NºCompromisso))
                    {
                        commitmentNo = contract?.NºCompromisso;
                        settedFromProjectOrContract = true;
                    }
                }


                if (!string.IsNullOrEmpty(project.PedidoDoCliente))
                {
                    customerRequestNo = project.PedidoDoCliente;
                    customerRequestDate = project.DataDoPedido.HasValue ? project.DataDoPedido.Value.ToString("yyyy-MM-dd") : string.Empty;
                }
                else
                {
                    if (!string.IsNullOrEmpty(contract?.NºRequisiçãoDoCliente))
                        customerRequestNo = contract?.NºRequisiçãoDoCliente;
                    customerRequestDate = contract?.DataReceçãoRequisição?.ToString("yyyy-MM-dd");
                }


                if (string.IsNullOrEmpty(commitmentNo) || string.IsNullOrEmpty(customerRequestNo))
                {
                    //Get from Contract Client Requisition
                    List<RequisiçõesClienteContrato> contractReq = null;
                    if (contract != null)
                        contractReq = DBContractClientRequisition.GetByContract(contract.NºDeContrato);

                    if (contractReq != null)
                    {
                        var billingGroupReq = contractReq.Where(x => x.GrupoFatura == 0 && x.DataInícioCompromisso <= serviceDeliveryDate);

                        var projectReq = billingGroupReq.Where(x => x.NºProjeto == project.NºProjeto);

                        if (string.IsNullOrEmpty(commitmentNo))
                            commitmentNo = projectReq.FirstOrDefault(x => x.DataFimCompromisso >= serviceDeliveryDate && !string.IsNullOrEmpty(x.NºCompromisso))?.NºCompromisso;

                        if (string.IsNullOrEmpty(customerRequestNo))
                        {
                            var item = projectReq.FirstOrDefault(x => x.DataFimCompromisso >= serviceDeliveryDate && !string.IsNullOrEmpty(x.NºRequisiçãoCliente));
                            customerRequestNo = item?.NºRequisiçãoCliente;
                            customerRequestDate = item?.DataRequisição?.ToString("yyyy-MM-dd");
                        }

                        if (string.IsNullOrEmpty(commitmentNo) || string.IsNullOrEmpty(customerRequestNo))
                        {
                            if (string.IsNullOrEmpty(commitmentNo))
                                commitmentNo = billingGroupReq.FirstOrDefault(x => x.DataFimCompromisso >= serviceDeliveryDate && string.IsNullOrEmpty(x.NºProjeto) && !string.IsNullOrEmpty(x.NºCompromisso))?.NºCompromisso;

                            if (string.IsNullOrEmpty(customerRequestNo))
                            {
                                var item = billingGroupReq.FirstOrDefault(x => x.DataFimCompromisso >= serviceDeliveryDate && string.IsNullOrEmpty(x.NºProjeto) && !string.IsNullOrEmpty(x.NºRequisiçãoCliente));
                                customerRequestNo = item?.NºRequisiçãoCliente;
                                customerRequestDate = item?.DataRequisição?.ToString("yyyy-MM-dd");
                            }
                        }
                    }
                }
                result.eReasonCode = 1;
            }
            commitmentDetails.commitmentNo = commitmentNo;
            commitmentDetails.customerRequestNo = customerRequestNo;
            commitmentDetails.customerRequestDate = customerRequestDate;
            commitmentDetails.settedFromProjectOrContract = settedFromProjectOrContract;
            result.Value = commitmentDetails;

            return Json(result);
        }


        #endregion InvoiceAutorization

        #region Invoice
        public IActionResult Faturacao()
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.FaturacaoDeProjetos);
            Configuração Config = DBConfigurations.GetById(1);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                ViewBag.DataFechoFaturacao = Convert.ToDateTime(Config.DataFechoFaturacao).ToString("dd/MM/yyyy");
                return View();
            }
            else

            {
                return RedirectToAction("AccessDenied", "Error");
            }
            //return View();
        }

        [HttpPost]
        public JsonResult GetMovimentosFaturacao()
        {
            try
            {
                List<SPInvoiceListViewModel> result = DBProjectMovements.GetAllAutorized();
                List<NAVClientsViewModel> ClientList = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, string.Empty);

                if (result.Count > 0)
                {
                    foreach (var lst in result)
                    {
                        if (lst.MovementType == 3)
                        {
                            lst.Quantity = Math.Abs(lst.Quantity.HasValue ? Math.Round((decimal)lst.Quantity, 2, MidpointRounding.AwayFromZero) : decimal.Zero) * (-1);
                        }

                        if (!String.IsNullOrEmpty(lst.Currency))
                        {
                            lst.UnitPrice = lst.UnitValueToInvoice.HasValue ? Math.Round((decimal)lst.UnitValueToInvoice, 4, MidpointRounding.AwayFromZero) : decimal.Zero;
                        }
                        var customer = ClientList.Where(x => x.No_ == lst.InvoiceToClientNo).FirstOrDefault();
                        lst.ClientName = customer != null ? customer.Name : string.Empty;
                        lst.ClientVATReg = customer != null ? customer.VATRegistrationNo_ : string.Empty;
                    }
                    return Json(result.OrderBy(x => x.ClientName).ToList());
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetAuthorizedMovements()
        {
            //TODO: substituir GetMovimentosFaturacao
            try
            {
                List<AuthorizedProjectViewModel> result = null;
                using (SuchDBContext ctx = new SuchDBContext())
                {
                    result = ctx.ProjectosAutorizados
                        .Where(x => !x.Faturado)
                        .ToList()
                        .ParseToViewModel();

                }
                if (result != null)
                {
                    List<AcessosDimensões> CUserDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
                    //Regions
                    if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.Region).Count() > 0)
                        result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.CodRegiao));
                    //FunctionalAreas
                    if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                        result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.CodAreaFuncional));
                    //ResponsabilityCenter
                    if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                        result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CodCentroResponsabilidade));

                    //List<NAVClientsViewModel> clients = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, string.Join(",", result.Select(r => r.CodCliente).ToList())).ToList();
                    //List<MovimentosProjectoAutorizados> AllAuthorizedProjectMovements = DBAuthorizedProjectMovements.GetAll("");

                    //result.ForEach(x =>
                    //{
                    //    var movements = AllAuthorizedProjectMovements.Where(y => y.GrupoFactura == x.GrupoFactura && y.CodProjeto == x.CodProjeto);
                    //    if (movements != null)
                    //    {
                    //        x.ValorAutorizado = movements.Sum(y => y.PrecoTotal);
                    //    }

                    //    x.NomeCliente = !string.IsNullOrEmpty(x.CodCliente) ? clients.Where(y => y.No_ == x.CodCliente).FirstOrDefault() != null ? clients.Where(y => y.No_ == x.CodCliente).FirstOrDefault().Name : "" : "";
                    //});
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpPost]
        public JsonResult GetAuthorizedMovementsHistoric()
        {
            //TODO: substituir GetMovimentosFaturacao
            try
            {
                List<AuthorizedProjectViewModel> result = null;
                using (SuchDBContext ctx = new SuchDBContext())
                {
                    result = ctx.ProjectosAutorizados
                        .Where(x => x.Faturado)
                        .ToList()
                        .ParseToViewModel();
                }
                if (result != null)
                {
                    var userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
                    List<UserDimensionsViewModel> userDimensionsViewModel = userDimensions.ParseToViewModel();
                    if (userDimensionsViewModel.Where(x => x.Dimension == (int)Dimensions.Region).Count() > 0)
                        result.RemoveAll(x => !userDimensionsViewModel.Any(y => y.DimensionValue == x.CodRegiao));
                    if (userDimensionsViewModel.Where(x => x.Dimension == (int)Dimensions.FunctionalArea).Count() > 0)
                        result.RemoveAll(x => !userDimensionsViewModel.Any(y => y.DimensionValue == x.CodAreaFuncional));
                    if (userDimensionsViewModel.Where(x => x.Dimension == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                        result.RemoveAll(x => !userDimensionsViewModel.Any(y => y.DimensionValue == x.CodCentroResponsabilidade));

                    //List<MovimentosProjectoAutorizados> AllAuthorizedProjectMovements = DBAuthorizedProjectMovements.GetAll("");
                    //List<NAVClientsViewModel> clients = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "").ToList();

                    //result.ForEach(x =>
                    //{
                    //    var movements = AllAuthorizedProjectMovements.Where(y => y.GrupoFactura == x.GrupoFactura && y.CodProjeto == x.CodProjeto).ToList();
                    //    if (movements != null && movements.Count() > 0)
                    //        x.ValorAutorizado = movements.Sum(y => y.PrecoTotal);

                    //    x.NomeCliente = clients.Where(y => y.No_ == x.CodCliente).FirstOrDefault() != null ? clients.Where(y => y.No_ == x.CodCliente).FirstOrDefault().Name : "";
                    //});
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpPost]
        public JsonResult GetProjMovementsLines([FromBody] string ProjNo, int? ProjGroup, bool Faturada = false)
        {
            //TODO: substituir GetMovimentosFaturacao         
            List<MovementAuthorizedProjectViewModel> AuthorizedProjectMovements = new List<MovementAuthorizedProjectViewModel>();
            try
            {
                AuthorizedProjectMovements = DBAuthorizedProjectMovements.GetMovementById((int)ProjGroup, ProjNo)
                    .ParseToViewModel(_config.NAVDatabaseName, _config.NAVCompanyName).ToList();

                if (AuthorizedProjectMovements.Count > 0)
                {
                    var userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
                    List<NAVClientsViewModel> AllClients = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "");
                    foreach (var lst in AuthorizedProjectMovements)
                    {
                        lst.ClientName = !string.IsNullOrEmpty(lst.CodClient) ? AllClients.Where(x => x.No_ == lst.CodClient).FirstOrDefault().Name : "";
                    }
                    List<UserDimensionsViewModel> userDimensionsViewModel = userDimensions.ParseToViewModel();
                    if (userDimensionsViewModel.Where(x => x.Dimension == (int)Dimensions.Region).Count() > 0)
                        AuthorizedProjectMovements.RemoveAll(x => !userDimensionsViewModel.Any(y => y.DimensionValue == x.RegionCode));
                    if (userDimensionsViewModel.Where(x => x.Dimension == (int)Dimensions.FunctionalArea).Count() > 0)
                        AuthorizedProjectMovements.RemoveAll(x => !userDimensionsViewModel.Any(y => y.DimensionValue == x.FunctionalAreaCode));
                    if (userDimensionsViewModel.Where(x => x.Dimension == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                        AuthorizedProjectMovements.RemoveAll(x => !userDimensionsViewModel.Any(y => y.DimensionValue == x.ResponsabilityCenterCode));
                }
            }
            catch (Exception ex)
            {
                AuthorizedProjectMovements = new List<MovementAuthorizedProjectViewModel>();
            }

            return Json(AuthorizedProjectMovements.OrderBy(x => x.ClientName));
        }

        [HttpPost]
        public JsonResult GetCustomers()
        {
            try
            {
                List<ProjectosAutorizados> authProjects = null;
                using (SuchDBContext ctx = new SuchDBContext())
                {
                    authProjects = ctx.ProjectosAutorizados.Where(x => !x.Faturado).ToList();
                }
                if (authProjects.Count > 0)
                {
                    List<NAVClientsViewModel> allCustomers = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, string.Empty);
                    if (allCustomers != null && allCustomers.Count() > 0)
                    {
                        var customerIds = authProjects.Select(x => x.CodCliente).Distinct();
                        var customers = allCustomers.Where(x => customerIds.Contains(x.No_)).ToList();
                        return Json(customers);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(new List<NAVClientsViewModel>());
        }

        [HttpPost]
        public JsonResult GetAllCustomers()
        {
            try
            {
                List<NAVClientsViewModel> allCustomers = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, string.Empty);
                return Json(allCustomers);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult ValidateCustomerForBilling([FromBody]  JObject requestParams)
        {
            //TODO: substituir ValidationCliente

            string customerNo = requestParams["customerNo"].ToString();
            string commitmentNo = requestParams["commitmentNo"].ToString();

            string execDetails = string.Empty;

            ErrorHandler result = new ErrorHandler();

            if (!string.IsNullOrEmpty(customerNo))
            {
                NAVClientsViewModel customer = null;
                if (!string.IsNullOrEmpty(customerNo))
                    customer = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, customerNo);

                if (customer != null)
                {
                    //Nº do Cliente > “999999”.
                    if (Convert.ToInt32(customer.No_) > 999999)
                    {
                        execDetails += "Não é permitido contabilizar Notas de Crédito para o Cliente " + customer.No_ + ".";
                        result.eMessages.Add(new TraceInformation(TraceType.Error, execDetails));
                    }
                    //Garantir que o campo “Nº do Contribuinte”
                    else if (customer.InternalClient == true)// Se Débito Interno
                    {
                        if (customer.VATRegistrationNo_ == "")
                        {
                            ClientDetailsViewModel cli = new ClientDetailsViewModel();
                            cli.VAT_Registration_No = "999999999";
                            Task<WSClientNAV.Update_Result> updateCliente = WSClient.UpdateVATNumber(cli, _configws);
                            updateCliente.Wait();
                        }
                    }
                    else if (customer.VATRegistrationNo_ == "")
                    {
                        execDetails += "Este cliente não tem Nº Contribuinte preenchido!";
                        result.eMessages.Add(new TraceInformation(TraceType.Error, execDetails));
                    }

                    //Abrigo Lei Compromisso
                    if (string.IsNullOrEmpty(commitmentNo))
                    {
                        if (customer.UnderCompromiseLaw == true)
                        {
                            execDetails += "Este cliente está ao abrigo da lei do compromisso. É obigatório o preenchimento do Nº de Compromisso ";
                            result.eMessages.Add(new TraceInformation(TraceType.Error, execDetails));
                        }
                        else
                        {
                            execDetails += "Não indicou Nº Compromisso. Deseja continuar?";
                            result.eMessages.Add(new TraceInformation(TraceType.Warning, execDetails));
                        }
                    }
                }
                else
                {
                    result.eReasonCode = 2;
                    result.eMessage = "O cliente não está definido.";
                }
            }
            else
            {
                result.eReasonCode = 2;
                result.eMessage = "Não foi possivel validar o cliente.";
            }
            return Json(result);
        }

        //FATURAR
        [HttpPost]
        public JsonResult CreateBillingDocuments([FromBody] List<AuthorizedProjectViewModel> authProjectMovements, string OptionInvoice, string dataFormulario, bool agrupar)
        {
            string execDetails = string.Empty;
            string errorMessage = string.Empty;
            bool hasErrors = false;
            ErrorHandler result = new ErrorHandler();
            //string projeto = string.Empty;

            if (authProjectMovements == null)
            {
                result.eReasonCode = 2;
                result.eMessage = "Selecione registos para faturar";
                return Json(result);
            }
            List<Projetos> projectsDetails = new List<Projetos>();
            var projectsIds = authProjectMovements.Select(x => x.CodProjeto).Distinct();
            var billingGroups = authProjectMovements.Select(x => x.GrupoFactura).Distinct();
            var customersIds = authProjectMovements.Select(x => x.CodCliente).Distinct();
            List<PreçosServiçosCliente> customersServicesPrices = new List<PreçosServiçosCliente>();
            //projeto = authProjectMovements.Select(x => x.CodProjeto).FirstOrDefault();

            //get all movements from authProjects
            List<SPInvoiceListViewModel> data = null;
            using (SuchDBContext ctx = new SuchDBContext())
            {
                //NR20181221 - Criar movimentos de projeto no NAV2017
                List<MovimentosDeProjeto> movimentosDeProjetos = new List<MovimentosDeProjeto>();

                foreach (AuthorizedProjectViewModel Movimento in authProjectMovements)
                {
                    string NumProjecto = Movimento.CodProjeto.ToString();
                    int GrupoFatura = Movimento.GrupoFactura;

                    //Obter os movimentos de projecto que sejam para tratar (Campo CriarMovNav2017 = 1)
                    movimentosDeProjetos = ctx.MovimentosDeProjeto.Where(x => x.NºProjeto == NumProjecto).Where(x => x.GrupoFatura == GrupoFatura).Where(x => x.CriarMovNav2017 == true).ToList();

                    foreach (MovimentosDeProjeto movimento in movimentosDeProjetos)
                    {
                        int NumLinha = movimento.NºLinha;
                        DateTime _Data_Agora = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        Guid transactID = Guid.NewGuid();

                        try
                        {
                            WSCreateProjectDiaryLine.WSJobJournalLine wSJobJournalLine = new WSCreateProjectDiaryLine.WSJobJournalLine();


                            wSJobJournalLine.Line_No = NumLinha;
                            wSJobJournalLine.Line_NoSpecified = true;
                            wSJobJournalLine.Job_No = NumProjecto;
                            wSJobJournalLine.Document_Date = movimento.Data ?? DateTime.Now;
                            wSJobJournalLine.Document_DateSpecified = true;
                            //switch (movimento.TipoMovimento.ToString())
                            //{
                            //    case "0":
                            //        wSJobJournalLine.Entry_Type = WSCreateProjectDiaryLine.Entry_Type.Usage;
                            //        wSJobJournalLine.Entry_TypeSpecified = true;
                            //        break;
                            //    case "1":
                            //        wSJobJournalLine.Entry_Type = WSCreateProjectDiaryLine.Entry_Type.Sale;
                            //        wSJobJournalLine.Entry_TypeSpecified = true;
                            //        break;
                            //}
                            wSJobJournalLine.Document_No = movimento.NºDocumento ?? string.Empty;
                            switch (movimento.Tipo.ToString())
                            {
                                case "1":
                                    wSJobJournalLine.Type = WSCreateProjectDiaryLine.Type.Item;
                                    wSJobJournalLine.TypeSpecified = true;
                                    break;
                                case "2":
                                    wSJobJournalLine.Type = WSCreateProjectDiaryLine.Type.Resource;
                                    wSJobJournalLine.TypeSpecified = true;
                                    break;
                                case "3":
                                    wSJobJournalLine.Type = WSCreateProjectDiaryLine.Type.G_L_Account;
                                    wSJobJournalLine.TypeSpecified = true;
                                    break;
                            }
                            wSJobJournalLine.No = movimento.Código;
                            wSJobJournalLine.Description100 = string.Empty;
                            wSJobJournalLine.Quantity = movimento.Quantidade.HasValue ? (decimal)movimento.Quantidade : decimal.Zero; //?? 0;
                            wSJobJournalLine.QuantitySpecified = true;
                            wSJobJournalLine.Unit_of_Measure_Code = movimento.CódUnidadeMedida ?? string.Empty;
                            wSJobJournalLine.Location_Code = movimento.CódLocalização ?? string.Empty;
                            //wSJobJournalLine.Posting_Group = movimento.GrupoContabProjeto ?? string.Empty;
                            wSJobJournalLine.RegionCode20 = string.Empty;
                            wSJobJournalLine.FunctionAreaCode20 = string.Empty;
                            wSJobJournalLine.ResponsabilityCenterCode20 = string.Empty;
                            wSJobJournalLine.Unit_Cost = movimento.CustoUnitário.HasValue ? (decimal)movimento.CustoUnitário : decimal.Zero;
                            wSJobJournalLine.Total_Cost = movimento.Quantidade.HasValue && movimento.CustoUnitário.HasValue ? Math.Round(((decimal)movimento.Quantidade * (decimal)movimento.CustoUnitário), 2, MidpointRounding.AwayFromZero) : decimal.Zero;
                            wSJobJournalLine.Unit_Price = movimento.PreçoUnitário.HasValue ? (decimal)movimento.PreçoUnitário : decimal.Zero;
                            wSJobJournalLine.Total_Price = movimento.Quantidade.HasValue && movimento.PreçoUnitário.HasValue ? Math.Round(((decimal)movimento.Quantidade * (decimal)movimento.PreçoUnitário), 2, MidpointRounding.AwayFromZero) : decimal.Zero;
                            wSJobJournalLine.Unit_CostSpecified = true;
                            wSJobJournalLine.Unit_PriceSpecified = true;
                            switch (movimento.Faturável.ToString())
                            {
                                case "False":
                                    wSJobJournalLine.Chargeable = false;
                                    wSJobJournalLine.ChargeableSpecified = true;
                                    break;
                                case "True":
                                    wSJobJournalLine.Chargeable = true;
                                    wSJobJournalLine.ChargeableSpecified = true;
                                    break;
                                default:
                                    wSJobJournalLine.Chargeable = false;
                                    wSJobJournalLine.ChargeableSpecified = true;
                                    break;
                            }
                            wSJobJournalLine.External_Document_No = movimento.DocumentoOriginal;
                            wSJobJournalLine.Portal_Transaction_No = transactID.ToString();
                            wSJobJournalLine.Posting_Date = (DateTime)movimento.Data;
                            wSJobJournalLine.Posting_DateSpecified = true;
                            wSJobJournalLine.OM_Line_No = 0;
                            wSJobJournalLine.Description = movimento.Descrição ?? string.Empty;
                            wSJobJournalLine.Description_2 = string.Empty;
                            wSJobJournalLine.gDec1 = 0;
                            wSJobJournalLine.gDec2 = 0;
                            wSJobJournalLine.gDec3 = 0;
                            wSJobJournalLine.gDec4 = 0;
                            //wSJobJournalLine.nav

                            //WSCreateProjectDiaryLine.Create_Result create_Result = new WSCreateProjectDiaryLine.Create_Result(wSJobJournalLine);

                            Task<WSCreateProjectDiaryLine.Create_Result> TCreateNavDiaryLine = WSProjectDiaryLine.CreateNavDiaryLines(wSJobJournalLine, transactID, _configws);
                            TCreateNavDiaryLine.Wait();

                            if (TCreateNavDiaryLine != null)
                            {
                                Task<WSGenericCodeUnit.FxPostJobJrnlLines_Result> TRegisterNavDiaryLine = WSProjectDiaryLine.RegsiterNavDiaryLines(transactID, _configws);
                                TRegisterNavDiaryLine.Wait();
                            }

                            //Atualizar movimento para não ser criado novamente
                            movimento.CriarMovNav2017 = false;
                            ctx.MovimentosDeProjeto.Update(movimento);
                            ctx.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            if (!hasErrors)
                                hasErrors = true;

                            execDetails += " Erro ao criar as linhas de movimentos de projeto no Nav2017: ";
                            errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                            result.eMessages.Add(new TraceInformation(TraceType.Exception, execDetails + errorMessage));
                        }
                    }
                }

                projectsDetails = ctx.Projetos.Where(x => projectsIds.Contains(x.NºProjeto)).ToList();

                data = ctx.MovimentosProjectoAutorizados
                    .Join(ctx.MovimentosDeProjeto,
                        mpa => mpa.NumMovimento,
                        mp => mp.NºLinha,
                        (mpa, mp) => new SPInvoiceListViewModel
                        {

                            //ProjectDimension;
                            //##################################    Obter de projetos autorizados (campos editaveis)
                            //CodTermosPagamento = authProjectMovements.FirstOrDefault(y => y.CodProjeto == mpa.CodProjeto && y.GrupoFactura == mpa.GrupoFactura).CodTermosPagamento,
                            //CodMetodoPagamento = authProjectMovements.FirstOrDefault(y => y.CodProjeto == mpa.CodProjeto && y.GrupoFactura == mpa.GrupoFactura).CodMetodoPagamento,
                            //ServiceDate = authProjectMovements.FirstOrDefault(y => y.CodProjeto == mpa.CodProjeto && y.GrupoFactura == mpa.GrupoFactura).DataPrestacaoServico,
                            CommitmentNumber = authProjectMovements.FirstOrDefault(y => y.CodProjeto == mpa.CodProjeto && y.GrupoFactura == mpa.GrupoFactura).NumCompromisso,
                            //SituacoesPendentes = authProjectMovements.FirstOrDefault(y => y.CodProjeto == mpa.CodProjeto && y.GrupoFactura == mpa.GrupoFactura).SituacoesPendentes,
                            //Comments = authProjectMovements.FirstOrDefault(y => y.CodProjeto == mpa.CodProjeto && y.GrupoFactura == mpa.GrupoFactura).Observacoes,
                            Date = authProjectMovements.FirstOrDefault(y => y.CodProjeto == mpa.CodProjeto && y.GrupoFactura == mpa.GrupoFactura).DataPrestacaoServico,
                            DateFim = authProjectMovements.FirstOrDefault(y => y.CodProjeto == mpa.CodProjeto && y.GrupoFactura == mpa.GrupoFactura).DataPrestacaoServicoFim,

                            //##################################    Obter de projetos autorizados (campos não editaveis)
                            InvoiceToClientNo = authProjectMovements.FirstOrDefault(y => y.CodProjeto == mpa.CodProjeto && y.GrupoFactura == mpa.GrupoFactura).CodCliente,
                            //InvoiceGroupDescription = authProjectMovements.FirstOrDefault(y => y.CodProjeto == mpa.CodProjeto && y.GrupoFactura == mpa.GrupoFactura).DescricaoGrupo,
                            ClientRequest = authProjectMovements.FirstOrDefault(y => y.CodProjeto == mpa.CodProjeto && y.GrupoFactura == mpa.GrupoFactura).PedidoCliente,
                            //Opcao = authProjectMovements.FirstOrDefault(y => y.CodProjeto == mpa.CodProjeto && y.GrupoFactura == mpa.GrupoFactura).Opcao,
                            //AutorizatedInvoiceData = authProjectMovements.FirstOrDefault(y => y.CodProjeto == mpa.CodProjeto && y.GrupoFactura == mpa.GrupoFactura).DataAutorizacao,
                            //Billed = authProjectMovements.FirstOrDefault(y => y.CodProjeto == mpa.CodProjeto && y.GrupoFactura == mpa.GrupoFactura).Faturado,
                            //User = authProjectMovements.FirstOrDefault(y => y.CodProjeto == mpa.CodProjeto && y.GrupoFactura == mpa.GrupoFactura).Utilizador,

                            //##################################    Obter de movimentos de projeto autorizados
                            MealType = mpa.TipoRefeicao,
                            Type = mpa.Tipo,
                            Code = mpa.Codigo,
                            Description = mpa.Descricao,
                            RegionCode = mpa.CodRegiao,
                            FunctionalAreaCode = mpa.CodAreaFuncional,
                            ResponsabilityCenterCode = mpa.CodCentroResponsabilidade,
                            ProjectNo = mpa.CodProjeto,
                            ContractNo = projectsDetails.Select(x => x.NºContrato).FirstOrDefault(x => x == mpa.CodProjeto),
                            ConsumptionDate = mpa.DataConsumo.HasValue ? mpa.DataConsumo.Value.ToString("yyyy-MM-dd") : "",
                            ServiceClientCode = mpa.CodServCliente,
                            MeasurementUnitCode = mpa.CodUnidadeMedida,
                            Quantity = mpa.Quantidade,
                            ServiceGroupCode = mpa.CodGrupoServico,
                            ExternalGuideNo = mpa.NumGuiaExterna,
                            WasteGuideNo_GAR = mpa.NumGuiaResiduosGar,
                            InvoiceGroup = mpa.GrupoFactura,
                            //LineNo = mpa.NumMovimento,
                            //MovementType = mpa.Tipo,
                            //TotalCost = mpa.CustoTotal,
                            //TotalPrice = mpa.PrecoTotal,
                            //DocumentNo = mpa.NumDocumento,
                            //ResourceType = mpa.TipoRecurso,                            

                            //##################################    Se necessário, obter de movimentos de projeto
                            //UnitPrice = mp.PreçoUnitário.HasValue ? Math.Round((decimal)mp.PreçoUnitário, 4) : decimal.Zero,
                            //UnitCost = mp.CustoUnitário.HasValue ? Math.Round((decimal)mp.CustoUnitário, 4) :decimal.Zero,
                            UnitPrice = mp.PreçoUnitário.HasValue ? mp.PreçoUnitário : decimal.Zero,
                            UnitCost = mp.CustoUnitário.HasValue ? mp.CustoUnitário : decimal.Zero,
                            LocationCode = mp.CódLocalização,
                            //CreateUser = mp.UtilizadorCriação,
                            ProjectContabGroup = mp.GrupoContabProjeto,
                            //AdjustedDocument = mp.DocumentoCorrigido,
                            //AdjustedDocumentData = mp.DataDocumentoCorrigido.HasValue ? mp.DataDocumentoCorrigido.Value.ToString("yyyy-MM-dd") : "",
                            //AdjustedPrice = mp.AcertoDePreços,
                            //Currency = mp.Moeda,
                            //Driver = mp.Motorista,
                            //EmployeeNo = mp.NºFuncionário,
                            //InternalRequest = mp.RequisiçãoInterna,
                            //OriginalDocument = mp.DocumentoOriginal,
                            //RequestNo = mp.NºRequisição,
                            //RequestLineNo = mp.NºLinhaRequisição,
                            //ResidueFinalDestinyCode = mp.CódDestinoFinalResíduos,
                            //ResidueGuideNo = mp.NºGuiaResíduos,
                            //TimesheetNo = mp.NºFolhaHoras,
                            //UnitValueToInvoice = mp.ValorUnitárioAFaturar,
                            //CreateDate = mp.DataHoraCriação,
                            //Registered = mp.Registado,
                            //QuantityReturned = mp.QuantidadeDevolvida,
                            //AutorizatedInvoice = mp.FaturaçãoAutorizada,
                            //Billable = mp.Faturável,
                            //UpdateDate = mp.DataHoraModificação,
                            //UpdateUser = mp.UtilizadorModificação,

                        }
                    )
                    .Where(x => x.InvoiceGroup.HasValue &&
                        projectsIds.Contains(x.ProjectNo) &&
                        billingGroups.Contains(x.InvoiceGroup.Value))
                    .ToList();

                //TESTE AROMAO CORRIGIR 08/03/2019
                if (authProjectMovements != null && authProjectMovements.Count() > 0)
                {
                    if (data != null && data.Count() > 0)
                    {
                        foreach (AuthorizedProjectViewModel movimento in authProjectMovements)
                        {
                            foreach (SPInvoiceListViewModel dt in data)
                            {
                                if (dt.ProjectNo == movimento.CodProjeto && dt.InvoiceGroup != movimento.GrupoFactura)
                                {
                                    dt.Apagar_Linha = true;
                                }
                            }
                        }
                        data.RemoveAll(z => z.Apagar_Linha == true);
                    }
                }


                customersServicesPrices = ctx.PreçosServiçosCliente
                    .Where(x => customersIds.Contains(x.Cliente))
                    .ToList();
            }

            if (data != null && data.Count() > 0)
            {
                var userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
                List<UserDimensionsViewModel> userDimensionsViewModel = userDimensions.ParseToViewModel();
                if (userDimensionsViewModel.Where(x => x.Dimension == (int)Dimensions.Region).Count() > 0)
                    data.RemoveAll(x => !userDimensionsViewModel.Any(y => y.DimensionValue == x.RegionCode));
                if (userDimensionsViewModel.Where(x => x.Dimension == (int)Dimensions.FunctionalArea).Count() > 0)
                    data.RemoveAll(x => !userDimensionsViewModel.Any(y => y.DimensionValue == x.FunctionalAreaCode));
                if (userDimensionsViewModel.Where(x => x.Dimension == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                    data.RemoveAll(x => !userDimensionsViewModel.Any(y => y.DimensionValue == x.ResponsabilityCenterCode));

                List<NAVClientsViewModel> customers = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, customersIds);

                List<AuthorizedCustomerBilling> groupedbyclient = new List<AuthorizedCustomerBilling>();
                if (agrupar == true)
                {
                    groupedbyclient = data.GroupBy(x => new
                    {
                        x.InvoiceToClientNo
                        //x.Date,
                        //x.DateFim,
                        //x.CommitmentNumber,
                        //x.ClientRequest
                    },
                        x => x,
                        (key, items) => new AuthorizedCustomerBilling
                        {
                            //InvoiceToClientNo = key.InvoiceToClientNo,
                            //Date = key.Date,
                            //DateFim = key.DateFim,
                            //CommitmentNumber = key.CommitmentNumber,
                            //ClientRequest = key.ClientRequest,

                            InvoiceToClientNo = key.InvoiceToClientNo,

                            CodProjeto = items.FirstOrDefault(y => y.InvoiceToClientNo == key.InvoiceToClientNo)?.ProjectNo,
                            Date = items.FirstOrDefault(y => y.InvoiceToClientNo == key.InvoiceToClientNo)?.Date,
                            DateFim = items.FirstOrDefault(y => y.InvoiceToClientNo == key.InvoiceToClientNo)?.DateFim,
                            CommitmentNumber = items.FirstOrDefault(y => y.InvoiceToClientNo == key.InvoiceToClientNo)?.CommitmentNumber,
                            ClientRequest = items.FirstOrDefault(y => y.InvoiceToClientNo == key.InvoiceToClientNo)?.ClientRequest,

                            ClientVATReg = customers.FirstOrDefault(x => x.No_ == key.InvoiceToClientNo)?.VATRegistrationNo_,// DBNAV2017Clients.GetClientVATByNo(key.InvoiceToClientNo, _config.NAVDatabaseName, _config.NAVCompanyName),
                            ContractNo = projectsDetails.Select(x => x.NºContrato).FirstOrDefault(y => !string.IsNullOrEmpty(y)),
                            Currency = items.FirstOrDefault(y => y.InvoiceToClientNo == key.InvoiceToClientNo)?.Currency,
                            LocationCode = items.FirstOrDefault(y => y.InvoiceToClientNo == key.InvoiceToClientNo)?.LocationCode,
                            MovementType = Convert.ToInt32(OptionInvoice),
                            CreateUser = User.Identity.Name,

                            Items = items.ToList(),

                            //##################################    Obter de projetos autorizados
                            CodMetodoPagamento = authProjectMovements
                                .FirstOrDefault(y => y.CodCliente == key.InvoiceToClientNo &&
                                                     y.DataPrestacaoServico == items.FirstOrDefault(z => z.InvoiceToClientNo == key.InvoiceToClientNo)?.Date &&
                                                     y.NumCompromisso == items.FirstOrDefault(z => z.InvoiceToClientNo == key.InvoiceToClientNo)?.CommitmentNumber &&
                                                     y.PedidoCliente == items.FirstOrDefault(z => z.InvoiceToClientNo == key.InvoiceToClientNo)?.ClientRequest)?.CodMetodoPagamento,
                            CodTermosPagamento = authProjectMovements
                                .FirstOrDefault(y => y.CodCliente == key.InvoiceToClientNo &&
                                                     y.DataPrestacaoServico == items.FirstOrDefault(z => z.InvoiceToClientNo == key.InvoiceToClientNo)?.Date &&
                                                     y.NumCompromisso == items.FirstOrDefault(z => z.InvoiceToClientNo == key.InvoiceToClientNo)?.CommitmentNumber &&
                                                     y.PedidoCliente == items.FirstOrDefault(z => z.InvoiceToClientNo == key.InvoiceToClientNo)?.ClientRequest)?.CodTermosPagamento,
                            Comments = authProjectMovements
                                .FirstOrDefault(y => y.CodCliente == key.InvoiceToClientNo &&
                                                     y.DataPrestacaoServico == items.FirstOrDefault(z => z.InvoiceToClientNo == key.InvoiceToClientNo)?.Date &&
                                                     y.NumCompromisso == items.FirstOrDefault(z => z.InvoiceToClientNo == key.InvoiceToClientNo)?.CommitmentNumber &&
                                                     y.PedidoCliente == items.FirstOrDefault(z => z.InvoiceToClientNo == key.InvoiceToClientNo)?.ClientRequest)?.Observacoes,
                            ServiceDate = authProjectMovements
                                .FirstOrDefault(y => y.CodCliente == key.InvoiceToClientNo &&
                                                     y.DataPrestacaoServico == items.FirstOrDefault(z => z.InvoiceToClientNo == key.InvoiceToClientNo)?.Date &&
                                                     y.NumCompromisso == items.FirstOrDefault(z => z.InvoiceToClientNo == key.InvoiceToClientNo)?.CommitmentNumber &&
                                                     y.PedidoCliente == items.FirstOrDefault(z => z.InvoiceToClientNo == key.InvoiceToClientNo)?.ClientRequest)?.DataServPrestado,
                            RegionCode = authProjectMovements
                                .FirstOrDefault(y => y.CodCliente == key.InvoiceToClientNo &&
                                                     y.DataPrestacaoServico == items.FirstOrDefault(z => z.InvoiceToClientNo == key.InvoiceToClientNo)?.Date &&
                                                     y.NumCompromisso == items.FirstOrDefault(z => z.InvoiceToClientNo == key.InvoiceToClientNo)?.CommitmentNumber &&
                                                     y.PedidoCliente == items.FirstOrDefault(z => z.InvoiceToClientNo == key.InvoiceToClientNo)?.ClientRequest)?.CodRegiao,
                            FunctionalAreaCode = authProjectMovements
                                .FirstOrDefault(y => y.CodCliente == key.InvoiceToClientNo &&
                                                     y.DataPrestacaoServico == items.FirstOrDefault(z => z.InvoiceToClientNo == key.InvoiceToClientNo)?.Date &&
                                                     y.NumCompromisso == items.FirstOrDefault(z => z.InvoiceToClientNo == key.InvoiceToClientNo)?.CommitmentNumber &&
                                                     y.PedidoCliente == items.FirstOrDefault(z => z.InvoiceToClientNo == key.InvoiceToClientNo)?.ClientRequest)?.CodAreaFuncional,
                            ResponsabilityCenterCode = authProjectMovements
                                .FirstOrDefault(y => y.CodCliente == key.InvoiceToClientNo &&
                                                     y.DataPrestacaoServico == items.FirstOrDefault(z => z.InvoiceToClientNo == key.InvoiceToClientNo)?.Date &&
                                                     y.NumCompromisso == items.FirstOrDefault(z => z.InvoiceToClientNo == key.InvoiceToClientNo)?.CommitmentNumber &&
                                                     y.PedidoCliente == items.FirstOrDefault(z => z.InvoiceToClientNo == key.InvoiceToClientNo)?.ClientRequest)?.CodCentroResponsabilidade,
                        })
                    .ToList();
                }
                else
                {
                    groupedbyclient = data.GroupBy(x => new
                    {
                        x.ProjectNo
                        //x.Date,
                        //x.DateFim,
                        //x.CommitmentNumber,
                        //x.ClientRequest
                    },
                        x => x,
                        (key, items) => new AuthorizedCustomerBilling
                        {
                            //InvoiceToClientNo = key.InvoiceToClientNo,
                            //Date = key.Date,
                            //DateFim = key.DateFim,
                            //CommitmentNumber = key.CommitmentNumber,
                            //ClientRequest = key.ClientRequest,

                            CodProjeto = key.ProjectNo,
                            InvoiceToClientNo = items.FirstOrDefault(y => y.ProjectNo == key.ProjectNo)?.InvoiceToClientNo,

                            Date = items.FirstOrDefault(y => y.ProjectNo == key.ProjectNo)?.Date,
                            DateFim = items.FirstOrDefault(y => y.ProjectNo == key.ProjectNo)?.DateFim,
                            CommitmentNumber = items.FirstOrDefault(y => y.ProjectNo == key.ProjectNo)?.CommitmentNumber,
                            ClientRequest = items.FirstOrDefault(y => y.ProjectNo == key.ProjectNo)?.ClientRequest,

                            ClientVATReg = customers.FirstOrDefault(x => x.No_ == items.FirstOrDefault(y => y.ProjectNo == key.ProjectNo)?.InvoiceToClientNo)?.VATRegistrationNo_,// DBNAV2017Clients.GetClientVATByNo(key.InvoiceToClientNo, _config.NAVDatabaseName, _config.NAVCompanyName),
                            ContractNo = projectsDetails.Select(x => x.NºContrato).FirstOrDefault(y => !string.IsNullOrEmpty(y)),
                            Currency = items.FirstOrDefault(y => y.ProjectNo == key.ProjectNo)?.Currency,
                            LocationCode = items.FirstOrDefault(y => y.ProjectNo == key.ProjectNo)?.LocationCode,
                            MovementType = Convert.ToInt32(OptionInvoice),
                            CreateUser = User.Identity.Name,

                            Items = items.ToList(),

                            //##################################    Obter de projetos autorizados
                            CodMetodoPagamento = authProjectMovements
                                .FirstOrDefault(y => y.CodCliente == items.FirstOrDefault(w => w.ProjectNo == key.ProjectNo)?.InvoiceToClientNo &&
                                                     y.DataPrestacaoServico == items.FirstOrDefault(z => z.InvoiceToClientNo == items.FirstOrDefault(w => w.ProjectNo == key.ProjectNo)?.InvoiceToClientNo)?.Date &&
                                                     y.NumCompromisso == items.FirstOrDefault(z => z.InvoiceToClientNo == items.FirstOrDefault(w => w.ProjectNo == key.ProjectNo)?.InvoiceToClientNo)?.CommitmentNumber &&
                                                     y.PedidoCliente == items.FirstOrDefault(z => z.InvoiceToClientNo == items.FirstOrDefault(w => w.ProjectNo == key.ProjectNo)?.InvoiceToClientNo)?.ClientRequest)?.CodMetodoPagamento,
                            CodTermosPagamento = authProjectMovements
                                .FirstOrDefault(y => y.CodCliente == items.FirstOrDefault(w => w.ProjectNo == key.ProjectNo)?.InvoiceToClientNo &&
                                                     y.DataPrestacaoServico == items.FirstOrDefault(z => z.InvoiceToClientNo == items.FirstOrDefault(w => w.ProjectNo == key.ProjectNo)?.InvoiceToClientNo)?.Date &&
                                                     y.NumCompromisso == items.FirstOrDefault(z => z.InvoiceToClientNo == items.FirstOrDefault(w => w.ProjectNo == key.ProjectNo)?.InvoiceToClientNo)?.CommitmentNumber &&
                                                     y.PedidoCliente == items.FirstOrDefault(z => z.InvoiceToClientNo == items.FirstOrDefault(w => w.ProjectNo == key.ProjectNo)?.InvoiceToClientNo)?.ClientRequest)?.CodTermosPagamento,
                            Comments = authProjectMovements
                                .FirstOrDefault(y => y.CodCliente == items.FirstOrDefault(w => w.ProjectNo == key.ProjectNo)?.InvoiceToClientNo &&
                                                     y.DataPrestacaoServico == items.FirstOrDefault(z => z.InvoiceToClientNo == items.FirstOrDefault(w => w.ProjectNo == key.ProjectNo)?.InvoiceToClientNo)?.Date &&
                                                     y.NumCompromisso == items.FirstOrDefault(z => z.InvoiceToClientNo == items.FirstOrDefault(w => w.ProjectNo == key.ProjectNo)?.InvoiceToClientNo)?.CommitmentNumber &&
                                                     y.PedidoCliente == items.FirstOrDefault(z => z.InvoiceToClientNo == items.FirstOrDefault(w => w.ProjectNo == key.ProjectNo)?.InvoiceToClientNo)?.ClientRequest)?.Observacoes,
                            ServiceDate = authProjectMovements
                                .FirstOrDefault(y => y.CodCliente == items.FirstOrDefault(w => w.ProjectNo == key.ProjectNo)?.InvoiceToClientNo &&
                                                     y.DataPrestacaoServico == items.FirstOrDefault(z => z.InvoiceToClientNo == items.FirstOrDefault(w => w.ProjectNo == key.ProjectNo)?.InvoiceToClientNo)?.Date &&
                                                     y.NumCompromisso == items.FirstOrDefault(z => z.InvoiceToClientNo == items.FirstOrDefault(w => w.ProjectNo == key.ProjectNo)?.InvoiceToClientNo)?.CommitmentNumber &&
                                                     y.PedidoCliente == items.FirstOrDefault(z => z.InvoiceToClientNo == items.FirstOrDefault(w => w.ProjectNo == key.ProjectNo)?.InvoiceToClientNo)?.ClientRequest)?.DataServPrestado,
                            RegionCode = authProjectMovements
                                .FirstOrDefault(y => y.CodCliente == items.FirstOrDefault(w => w.ProjectNo == key.ProjectNo)?.InvoiceToClientNo &&
                                                     y.DataPrestacaoServico == items.FirstOrDefault(z => z.InvoiceToClientNo == items.FirstOrDefault(w => w.ProjectNo == key.ProjectNo)?.InvoiceToClientNo)?.Date &&
                                                     y.NumCompromisso == items.FirstOrDefault(z => z.InvoiceToClientNo == items.FirstOrDefault(w => w.ProjectNo == key.ProjectNo)?.InvoiceToClientNo)?.CommitmentNumber &&
                                                     y.PedidoCliente == items.FirstOrDefault(z => z.InvoiceToClientNo == items.FirstOrDefault(w => w.ProjectNo == key.ProjectNo)?.InvoiceToClientNo)?.ClientRequest)?.CodRegiao,
                            FunctionalAreaCode = authProjectMovements
                                .FirstOrDefault(y => y.CodCliente == items.FirstOrDefault(w => w.ProjectNo == key.ProjectNo)?.InvoiceToClientNo &&
                                                     y.DataPrestacaoServico == items.FirstOrDefault(z => z.InvoiceToClientNo == items.FirstOrDefault(w => w.ProjectNo == key.ProjectNo)?.InvoiceToClientNo)?.Date &&
                                                     y.NumCompromisso == items.FirstOrDefault(z => z.InvoiceToClientNo == items.FirstOrDefault(w => w.ProjectNo == key.ProjectNo)?.InvoiceToClientNo)?.CommitmentNumber &&
                                                     y.PedidoCliente == items.FirstOrDefault(z => z.InvoiceToClientNo == items.FirstOrDefault(w => w.ProjectNo == key.ProjectNo)?.InvoiceToClientNo)?.ClientRequest)?.CodAreaFuncional,
                            ResponsabilityCenterCode = authProjectMovements
                                .FirstOrDefault(y => y.CodCliente == items.FirstOrDefault(w => w.ProjectNo == key.ProjectNo)?.InvoiceToClientNo &&
                                                     y.DataPrestacaoServico == items.FirstOrDefault(z => z.InvoiceToClientNo == items.FirstOrDefault(w => w.ProjectNo == key.ProjectNo)?.InvoiceToClientNo)?.Date &&
                                                     y.NumCompromisso == items.FirstOrDefault(z => z.InvoiceToClientNo == items.FirstOrDefault(w => w.ProjectNo == key.ProjectNo)?.InvoiceToClientNo)?.CommitmentNumber &&
                                                     y.PedidoCliente == items.FirstOrDefault(z => z.InvoiceToClientNo == items.FirstOrDefault(w => w.ProjectNo == key.ProjectNo)?.InvoiceToClientNo)?.ClientRequest)?.CodCentroResponsabilidade,
                        })
                    .ToList();
                    
                    //data.ForEach(x =>
                    //{
                    //    AuthorizedCustomerBilling Autorizacao = new AuthorizedCustomerBilling();

                    //    Autorizacao.InvoiceToClientNo = x.InvoiceToClientNo;

                    //    Autorizacao.Date = x.Date;
                    //    Autorizacao.DateFim = x.DateFim;
                    //    Autorizacao.CommitmentNumber = x.CommitmentNumber;
                    //    Autorizacao.ClientRequest = x.ClientRequest;

                    //    Autorizacao.ClientVATReg = customers.FirstOrDefault(y => y.No_ == x.InvoiceToClientNo)?.VATRegistrationNo_;
                    //    Autorizacao.ContractNo = projectsDetails.Select(y => y.NºContrato).FirstOrDefault(y => !string.IsNullOrEmpty(y));
                    //    Autorizacao.Currency = x.Currency;
                    //    Autorizacao.LocationCode = x.LocationCode;
                    //    Autorizacao.MovementType = Convert.ToInt32(OptionInvoice);
                    //    Autorizacao.CreateUser = User.Identity.Name;

                    //    Autorizacao.Items = data.Where(y => y.InvoiceToClientNo == x.InvoiceToClientNo).ToList();

                    //    //##################################    Obter de projetos autorizados
                    //    Autorizacao.CodMetodoPagamento = authProjectMovements
                    //        .FirstOrDefault(y => y.CodCliente == x.InvoiceToClientNo &&
                    //                                y.DataPrestacaoServico == x.Date &&
                    //                                y.NumCompromisso == x.CommitmentNumber &&
                    //                                y.PedidoCliente == x.ClientRequest).CodMetodoPagamento;
                    //    Autorizacao.CodTermosPagamento = authProjectMovements
                    //        .FirstOrDefault(y => y.CodCliente == x.InvoiceToClientNo &&
                    //                                y.DataPrestacaoServico == x.Date &&
                    //                                y.NumCompromisso == x.CommitmentNumber &&
                    //                                y.PedidoCliente == x.ClientRequest)?.CodTermosPagamento;
                    //    Autorizacao.Comments = authProjectMovements
                    //        .FirstOrDefault(y => y.CodCliente == x.InvoiceToClientNo &&
                    //                                y.DataPrestacaoServico == x.Date &&
                    //                                y.NumCompromisso == x.CommitmentNumber &&
                    //                                y.PedidoCliente == x.ClientRequest)?.Observacoes;
                    //    Autorizacao.ServiceDate = authProjectMovements
                    //        .FirstOrDefault(y => y.CodCliente == x.InvoiceToClientNo &&
                    //                                y.DataPrestacaoServico == x.Date &&
                    //                                y.NumCompromisso == x.CommitmentNumber &&
                    //                                y.PedidoCliente == x.ClientRequest)?.DataServPrestado;
                    //    Autorizacao.RegionCode = authProjectMovements
                    //        .FirstOrDefault(y => y.CodCliente == x.InvoiceToClientNo &&
                    //                                y.DataPrestacaoServico == x.Date &&
                    //                                y.NumCompromisso == x.CommitmentNumber &&
                    //                                y.PedidoCliente == x.ClientRequest)?.CodRegiao;
                    //    Autorizacao.FunctionalAreaCode = authProjectMovements
                    //        .FirstOrDefault(y => y.CodCliente == x.InvoiceToClientNo &&
                    //                                y.DataPrestacaoServico == x.Date &&
                    //                                y.NumCompromisso == x.CommitmentNumber &&
                    //                                y.PedidoCliente == x.ClientRequest)?.CodAreaFuncional;
                    //    Autorizacao.ResponsabilityCenterCode = authProjectMovements
                    //        .FirstOrDefault(y => y.CodCliente == x.InvoiceToClientNo &&
                    //                                y.DataPrestacaoServico == x.Date &&
                    //                                y.NumCompromisso == x.CommitmentNumber &&
                    //                                y.PedidoCliente == x.ClientRequest)?.CodCentroResponsabilidade;

                    //    groupedbyclient.Add(Autorizacao);
                    //});
                }


                bool allMealTypesAreValid = true;

                groupedbyclient.ForEach(x =>
                {
                    //Set dimensions
                    var authProj = authProjectMovements
                            .FirstOrDefault(y => y.CodProjeto == x.CodProjeto &&
                                                 y.CodCliente == x.InvoiceToClientNo &&
                                                 y.DataPrestacaoServico == x.Date &&
                                                 y.NumCompromisso == x.CommitmentNumber &&
                                                 y.PedidoCliente == x.ClientRequest);

                    var proj = projectsDetails.FirstOrDefault(y => y.NºProjeto == x.Items.FirstOrDefault()?.ProjectNo);
                    string projectRegion = proj != null ? proj.CódigoRegião : string.Empty;
                    var customer = customers.FirstOrDefault(y => y.No_ == x.InvoiceToClientNo);
                    x.SetDimensionsFor(authProj, projectRegion, customer);
                    x.DataPedido = authProj != null ? authProj.DataPedido : null;
                    x.GrupoFatura = authProj.GrupoFactura;
                    x.NoFaturaRelacionada = authProj.NoFaturaRelacionada;

                    TiposGrupoContabProjeto contabGroupType = new TiposGrupoContabProjeto();
                    x.Items.ForEach(item =>
                    {
                        //Set billing group
                        if (!string.IsNullOrEmpty(proj.CódigoÁreaFuncional) && proj.TipoGrupoContabProjeto.HasValue)
                        {
                            if (contabGroupType.Código != proj.TipoGrupoContabProjeto.Value)
                                contabGroupType = DBCountabGroupTypes.GetById(proj.TipoGrupoContabProjeto.Value);
                            if (contabGroupType != null)
                            {
                                item.ProjectContabGroup = contabGroupType.Descrição.Trim();
                            }
                        }

                        //Set Resources MealTypes
                        if (item.FunctionalAreaCode.StartsWith('5') && item.Type == 2 && (!item.MealType.HasValue || item.MealType == 0))
                        {
                            bool itemHasValidMealType = false;
                            var servicePrice = customersServicesPrices.FirstOrDefault(serv => serv.Cliente == item.InvoiceToClientNo && serv.CodServCliente == item.ServiceClientCode && serv.Recurso == item.Code);
                            if (servicePrice != null)
                            {
                                int mealType;
                                int.TryParse(servicePrice.TipoRefeição, out mealType);
                                if (mealType > 0)
                                {
                                    item.MealType = mealType;
                                    itemHasValidMealType = true;
                                }
                            }
                            if (!itemHasValidMealType)
                            {
                                string invalidMealTypeMessage = string.Format("Tipo de Refeição não está configurado (Projeto: {0}; Grupo Fatura: {1}; Recurso: {2})", item.ProjectNo, item.InvoiceGroup, item.Code);
                                result.eMessages.Add(new TraceInformation(TraceType.Exception, invalidMealTypeMessage));
                                allMealTypesAreValid = false;
                            }
                        }
                    });
                });
                //Ensure all meal types are configured
                if (!allMealTypesAreValid)
                    return Json(result);

                //Create Project if exists
                foreach (string projectId in projectsIds)
                {
                    Task<WSCreateNAVProject.Read_Result> Project = WSProject.GetNavProject(projectId, _configws);
                    Project.Wait();
                    if (Project.IsCompletedSuccessfully && Project.Result.WSJob == null)
                    {
                        try
                        {
                            var projectDetails = projectsDetails.First(x => x.NºProjeto == projectId);

                            ProjectDetailsViewModel proj = new ProjectDetailsViewModel();
                            proj.ProjectNo = projectId;
                            proj.Description = projectDetails.Descrição;
                            proj.ClientNo = projectDetails.NºCliente;
                            proj.RegionCode = projectDetails.CódigoRegião;
                            proj.ResponsabilityCenterCode = projectDetails.CódigoCentroResponsabilidade;
                            proj.FunctionalAreaCode = projectDetails.CódigoÁreaFuncional;
                            proj.Status = EstadoProjecto.Encomenda;
                            proj.Visivel = false;
                            Task<WSCreateNAVProject.Create_Result> createProject = WSProject.CreateNavProject(proj, _configws);
                            createProject.Wait();
                        }
                        catch (Exception ex)
                        {
                            if (!hasErrors)
                                hasErrors = true;

                            execDetails += " Erro ao criar Projeto: ";
                            errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                            result.eMessages.Add(new TraceInformation(TraceType.Exception, execDetails + errorMessage));
                            return Json(result);
                        }
                    }
                }


                if (groupedbyclient != null)
                {
                    foreach (var header in groupedbyclient)
                    {
                        string codproject = "";
                        var itemsToInvoice = header.Items.Select(x => new Tuple<string, int>(x.ProjectNo, x.InvoiceGroup.Value)).Distinct().ToList();
                        if (header.Items != null && header.Items.Count() > 0)
                            codproject = !string.IsNullOrEmpty(header.Items.FirstOrDefault().ProjectNo) ? header.Items.FirstOrDefault().ProjectNo : "";

                        try
                        {
                            //var invoiceHeader = header.Items.First();
                            //invoiceHeader.ClientVATReg = header.ClientVATReg;
                            //invoiceHeader.MovementType = Convert.ToInt32(OptionInvoice);
                            //invoiceHeader.CreateUser = User.Identity.Name;
                            header.CreateUser = User.Identity.Name;

                            execDetails = string.Format("Fat. Cliente: {0}, Data: {1}, Nº Compromisso: {2} - ", header.InvoiceToClientNo, header.Date, header.CommitmentNumber);


                            SPInvoiceListViewModel Ship = new SPInvoiceListViewModel();
                            Projetos proj = DBProjects.GetById(codproject);
                            Contratos cont = DBContracts.GetByIdLastVersion(proj.NºContrato);
                            NAVClientsViewModel Cliente = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, proj.NºCliente);

                            if (proj != null && !string.IsNullOrEmpty(proj.CódEndereçoEnvio))
                            {
                                header.InvoiceToClientNo = !string.IsNullOrEmpty(proj.NºCliente) ? proj.NºCliente : "";
                                Ship.Ship_to_Code = !string.IsNullOrEmpty(proj.CódEndereçoEnvio) ? proj.CódEndereçoEnvio : "";
                            }
                            else
                            {
                                if (cont != null && !string.IsNullOrEmpty(cont.CódEndereçoEnvio))
                                {
                                    header.InvoiceToClientNo = !string.IsNullOrEmpty(cont.NºCliente) ? cont.NºCliente : "";
                                    Ship.Ship_to_Code = !string.IsNullOrEmpty(cont.CódEndereçoEnvio) ? cont.CódEndereçoEnvio : "";
                                }
                                else
                                {
                                    if (Cliente != null)
                                    {
                                        header.InvoiceToClientNo = !string.IsNullOrEmpty(Cliente.No_) ? Cliente.No_ : "";
                                        Ship.Ship_to_Code = "";
                                    }
                                }
                            }

                            if (proj.FaturaPrecosIvaIncluido == true)
                                header.FaturaPrecosIvaIncluido = true;

                            //AMARO Clientes Internos
                            if (Cliente != null)
                            {
                                if (Cliente.InternalClient == true)
                                {
                                    header.RegionCode = Cliente.RegionCode;
                                    header.FunctionalAreaCode = Cliente.FunctionalAreaCode;
                                    header.ResponsabilityCenterCode = Cliente.ResponsabilityCenterCode;
                                }
                                else
                                {
                                    if (header.Items.Count > 0 && header.Items.FirstOrDefault().RegionCode != header.RegionCode)
                                    {
                                        header.ResponsabilityCenterCode = "";
                                    }
                                }
                            }

                            //Contratos do tipo Quotas
                            if (cont != null && cont.Tipo == 3)
                            {
                                header.CodMetodoPagamento = !string.IsNullOrEmpty(cont.CódFormaPagamento) ? cont.CódFormaPagamento : DBConfiguracaoParametros.GetByParametro("QuotasMetPagamento").Valor;
                            }

                            Task<WSCreatePreInvoice.Create_Result> TCreatePreInvoice = WSPreInvoice.CreatePreInvoice(header, _configws, dataFormulario, codproject, Ship, header.GrupoFatura);
                            TCreatePreInvoice.Wait();

                            if (TCreatePreInvoice.IsCompletedSuccessfully)
                            {
                                string headerNo = TCreatePreInvoice.Result.WSPreInvoice.No;
                                execDetails += "Criada a fatura núm " + headerNo;

                                try
                                {
                                    header.Items.ForEach(x =>
                                    {
                                        x.DocumentNo = headerNo;
                                        x.ContractNo = projectsDetails
                                                            .Select(y => new { ProjectNo = y.NºProjeto, ContractNo = y.NºContrato })
                                                            .FirstOrDefault(y => y.ProjectNo == x.ProjectNo)?.ContractNo;

                                        //Para Nota de créditos inverter o sinal Marco Marcelo 20/11/2019
                                        if (header.MovementType == 4)
                                            x.TotalPrice = -1 * (x.TotalPrice.HasValue ? (decimal)x.TotalPrice.Value : decimal.Zero);
                                        if (header.MovementType == 4)
                                            x.Quantity = -1 * (x.Quantity.HasValue ? (decimal)x.Quantity.Value : decimal.Zero);
                                    });

                                    List<NAVResourcesViewModel> resourceslines = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, "", "", 0, "");
                                    List<WasteRateViewModel> wasteRates = DBWasteRate.ParseToViewModel(DBWasteRate.GetAll());

                                    //recursos com taxa residuo
                                    var selectedResources = header.Items.Where(x => x.Type == 2).Select(x => x.Code).Distinct().ToList();
                                    var selectedWasteResources = resourceslines.Where(x => selectedResources.Contains(x.Code)).ToList();

                                    foreach (var item in selectedWasteResources)
                                    {
                                        var wasteFamilyResources = wasteRates.Where(x => x.FamiliaRecurso == item.ResourceGroup).ToList();
                                        wasteFamilyResources.ForEach(x =>
                                        {

                                            decimal? quantity = header.Items.Where(y => y.Type == 2 && y.Code == item.Code).Sum(y => y.Quantity.HasValue ? (decimal)y.Quantity : decimal.Zero);
                                            var resourceFirstLine = header.Items.Where(y => y.Type == 2 && y.Code == item.Code).OrderByDescending(y => y.ContractNo).FirstOrDefault();
                                            var resource = resourceslines.Where(y => y.Code == x.Recurso && y.WasteRate == 1).FirstOrDefault();
                                            if (resource != null)
                                            {
                                                if (header.Items.Where(y => y.Code == resource.Code).Count() == 0)
                                                {
                                                    SPInvoiceListViewModel wasteLineToAdd = new SPInvoiceListViewModel();
                                                    wasteLineToAdd.Quantity = quantity.HasValue ? (decimal)quantity : decimal.Zero; // ?? 0;
                                                    wasteLineToAdd.Type = 2;
                                                    wasteLineToAdd.Code = resource.Code;
                                                    wasteLineToAdd.Description = resource.Name;
                                                    wasteLineToAdd.UnitPrice = (decimal)resource.UnitPrice;
                                                    wasteLineToAdd.RegionCode = resourceFirstLine.RegionCode;
                                                    wasteLineToAdd.ResponsabilityCenterCode = resourceFirstLine.ResponsabilityCenterCode;
                                                    wasteLineToAdd.FunctionalAreaCode = resourceFirstLine.FunctionalAreaCode;
                                                    wasteLineToAdd.ProjectDimension = resourceFirstLine.ProjectNo;
                                                    wasteLineToAdd.ContractNo = projectsDetails
                                                                .Select(y => new { ProjectNo = y.NºProjeto, ContractNo = y.NºContrato })
                                                                .FirstOrDefault(y => y.ProjectNo == resourceFirstLine.ProjectNo)?.ContractNo;
                                                    wasteLineToAdd.Date = data.FirstOrDefault().Date;
                                                    wasteLineToAdd.DateFim = data.FirstOrDefault().DateFim;

                                                    header.Items.Add(wasteLineToAdd);
                                                }
                                                else
                                                {
                                                    foreach (var linha in header.Items)
                                                    {
                                                        if (linha.Code == resource.Code)
                                                        {
                                                            linha.Quantity = (linha.Quantity.HasValue ? (decimal)linha.Quantity : decimal.Zero) + (quantity.HasValue ? (decimal)quantity : decimal.Zero); // ?? 0;
                                                        }
                                                    }
                                                }
                                            }
                                        });
                                    }

                                    if (proj.FaturaPrecosIvaIncluido == true)
                                    {
                                        string NoCliente = string.Empty;
                                        string GrupoIVA = string.Empty;
                                        string GrupoCliente = string.Empty;
                                        decimal IVA = new decimal();
                                        foreach (var item in header.Items)
                                        {
                                            NoCliente = item.InvoiceToClientNo;
                                            GrupoIVA = string.Empty;
                                            GrupoCliente = string.Empty;
                                            IVA = 0;

                                            if (!string.IsNullOrEmpty(item.Code))
                                            {
                                                NAVResourcesViewModel Resource = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, item.Code, "", 0, "").FirstOrDefault();
                                                if (Resource != null)
                                                    GrupoIVA = Resource.VATProductPostingGroup;
                                                else
                                                {
                                                    execDetails += " Erro ao criar a fatura: Não foi possível encontrar o recurso Nº: " + item.Code;
                                                    result.eReasonCode = 2;
                                                    result.eMessages.Add(new TraceInformation(TraceType.Exception, execDetails));
                                                    return Json(result);
                                                }

                                                if (!string.IsNullOrEmpty(NoCliente) && !string.IsNullOrEmpty(GrupoIVA))
                                                {
                                                    NAVClientsViewModel cliente = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, NoCliente);
                                                    if (cliente != null)
                                                        GrupoCliente = cliente.VATBusinessPostingGroup;
                                                    else
                                                    {
                                                        execDetails += " Erro ao criar a fatura: Não foi possível encontrar o cliente Nº: " + NoCliente;
                                                        result.eReasonCode = 2;
                                                        result.eMessages.Add(new TraceInformation(TraceType.Exception, execDetails));
                                                        return Json(result);
                                                    }

                                                    if (!string.IsNullOrEmpty(GrupoCliente))
                                                        IVA = DBNAV2017VATPostingSetup.GetIVA(_config.NAVDatabaseName, _config.NAVCompanyName, GrupoCliente, GrupoIVA);

                                                    if (IVA > 0)
                                                        IVA = (IVA / 100) + 1;
                                                }
                                            }

                                            if (IVA > 0)
                                                item.UnitPrice = (item.UnitPrice.HasValue ? (decimal)item.UnitPrice : decimal.Zero) * IVA;
                                        }
                                    }

                                    Task<WSCreatePreInvoiceLine.CreateMultiple_Result> TCreatePreInvoiceLine = WSPreInvoiceLine.CreatePreInvoiceLineListProject(header.Items, headerNo, OptionInvoice, _configws);
                                    TCreatePreInvoiceLine.Wait();

                                    if (TCreatePreInvoiceLine.IsCompletedSuccessfully)
                                    {
                                        execDetails += " Linhas criadas com sucesso.";
                                        //update to Invoiced = true
                                        using (SuchDBContext ctx = new SuchDBContext())
                                        {
                                            itemsToInvoice.ForEach(key =>
                                            {
                                                var authorizedProjects = ctx.ProjectosAutorizados
                                                        .Where(x => x.CodProjeto == key.Item1 && x.GrupoFactura == key.Item2)
                                                        .ToList();
                                                //var authorizedProjectMovements = ctx.MovimentosDeProjeto.Where(x => x.CodProjeto == projectNo && x.GrupoFactura == invoiceGroup);

                                                //Fecho Automático
                                                authorizedProjects.Distinct().ToList().ForEach(x =>
                                                {
                                                    ProjectDetailsViewModel projFA = DBProjects.GetById(x.CodProjeto).ParseToViewModel();
                                                    if (projFA.FechoAutomatico == true && projFA.Status != (EstadoProjecto)2)
                                                    {
                                                        JsonResult resultFA;
                                                        resultFA = TerminarProject(projFA);
                                                    }
                                                });

                                                var projectMovements = ctx.MovimentosDeProjeto
                                                    .Where(x => x.NºProjeto == key.Item1 && x.GrupoFatura == key.Item2)
                                                    .ToList();

                                                authorizedProjects.ForEach(x => x.Faturado = true);
                                                //authorizedProjectMovements.ForEach(x => x.Faturada = true);
                                                projectMovements.ForEach(x => x.Faturada = true);

                                                ctx.ProjectosAutorizados.UpdateRange(authorizedProjects);
                                                //ctx.MovimentosProjetoAutorizados.UpdateRange(authorizedProjectMovements);
                                                ctx.MovimentosDeProjeto.UpdateRange(projectMovements);
                                            });

                                            ctx.SaveChanges();

                                            result.eMessages.Add(new TraceInformation(TraceType.Success, execDetails));
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //A Pedido do Carlos Rodrigues quando o WebService devolve uma mensagem com o texto "maximum message size quota"
                                    //assume-se que o mesmo foi executado com sucesso.
                                    errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                                    if (errorMessage.ToLower().Contains("maximum message size quota".ToLower()) || errorMessage.ToLower().Contains("request channel timed out".ToLower()))
                                    {
                                        errorMessage = "";

                                        execDetails += " Linhas criadas com sucesso.";
                                        //update to Invoiced = true
                                        using (SuchDBContext ctx = new SuchDBContext())
                                        {
                                            itemsToInvoice.ForEach(key =>
                                            {
                                                var authorizedProjects = ctx.ProjectosAutorizados
                                                .Where(x => x.CodProjeto == key.Item1 && x.GrupoFactura == key.Item2)
                                                .ToList();
                                                //var authorizedProjectMovements = ctx.MovimentosDeProjeto.Where(x => x.CodProjeto == projectNo && x.GrupoFactura == invoiceGroup);

                                                //Fecho Automático
                                                authorizedProjects.Distinct().ToList().ForEach(x =>
                                                {
                                                    ProjectDetailsViewModel projFA = DBProjects.GetById(x.CodProjeto).ParseToViewModel();
                                                    if (projFA.FechoAutomatico == true && projFA.Status != (EstadoProjecto)2)
                                                    {
                                                        JsonResult resultFA;
                                                        resultFA = TerminarProject(projFA);
                                                    }
                                                });

                                                var projectMovements = ctx.MovimentosDeProjeto
                                                    .Where(x => x.NºProjeto == key.Item1 && x.GrupoFatura == key.Item2)
                                                    .ToList();

                                                authorizedProjects.ForEach(x => x.Faturado = true);
                                                //authorizedProjectMovements.ForEach(x => x.Faturada = true);
                                                projectMovements.ForEach(x => x.Faturada = true);

                                                ctx.ProjectosAutorizados.UpdateRange(authorizedProjects);
                                                //ctx.MovimentosProjetoAutorizados.UpdateRange(authorizedProjectMovements);
                                                ctx.MovimentosDeProjeto.UpdateRange(projectMovements);
                                            });

                                            ctx.SaveChanges();

                                            result.eMessages.Add(new TraceInformation(TraceType.Success, execDetails));
                                        }
                                    }
                                    else
                                    {
                                        if (!hasErrors)
                                            hasErrors = true;

                                        execDetails += " Erro ao criar as linhas: ";
                                        result.eMessages.Add(new TraceInformation(TraceType.Exception, execDetails + errorMessage));
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (!hasErrors)
                                hasErrors = true;

                            execDetails += " Erro ao criar a fatura: ";
                            errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                            result.eMessages.Add(new TraceInformation(TraceType.Exception, execDetails + " " + errorMessage));
                        }
                    }
                    if (hasErrors)
                    {
                        result.eReasonCode = 2;
                        result.eMessage = "Ocorreram erros na criação de faturas.";
                    }
                    else
                    {
                        result.eReasonCode = 1;
                        result.eMessage = "Faturas criadas com sucesso.";
                    }
                }
                else
                {
                    result.eReasonCode = 2;
                    result.eMessage = "Selecionou registos com 'Nº de Compromisso' diferentes!";
                }
            }
            else
            {
                result.eReasonCode = 2;
                result.eMessage = "É necessário anular a linha e voltar a autorizar a faturação.";
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult ValidationCliente([FromBody] List<SPInvoiceListViewModel> data)
        {
            string execDetails = string.Empty;

            ErrorHandler result = new ErrorHandler();
            SPInvoiceListViewModel line = data[0];
            List<NAVClientsViewModel> Cliente = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, line.InvoiceToClientNo);
            if (Cliente != null)
            {
                //Nº do Cliente > “999999”.
                if (Convert.ToInt32(Cliente[0].No_) > 999999)
                {
                    execDetails += "Não é permitido contabilizar para o Cliente " + Cliente[0].No_ + ".";
                    result.eMessages.Add(new TraceInformation(TraceType.Error, execDetails));
                }
                //Garantir que o campo “Nº do Contribuinte”
                else if (Cliente[0].InternalClient == true)// Se Débito Interno
                {
                    if (Cliente[0].VATRegistrationNo_ == "")
                    {
                        ClientDetailsViewModel cli = new ClientDetailsViewModel();
                        cli.VAT_Registration_No = "999999999";
                        Task<WSClientNAV.Update_Result> updateCliente = WSClient.UpdateVATNumber(cli, _configws);
                        updateCliente.Wait();
                    }
                }
                else if (Cliente[0].VATRegistrationNo_ == "")
                {
                    execDetails += "Este cliente não tem Nº Contribuinte preenchido!";
                    result.eMessages.Add(new TraceInformation(TraceType.Error, execDetails));
                }

                //Abrigo Lei Compromisso
                if (line.CommitmentNumber == "")
                {

                    if (Cliente[0].UnderCompromiseLaw == true)
                    {
                        execDetails += "Este cliente está ao abrigo da lei do compromisso. É obigatório o preenchimento do Nº de Compromisso ";
                        result.eMessages.Add(new TraceInformation(TraceType.Error, execDetails));
                    }
                    else
                    {
                        execDetails += "Não indicou Nº Compromisso. Deseja continuar?";
                        result.eMessages.Add(new TraceInformation(TraceType.Warning, execDetails));
                    }
                }
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult CancelLines([FromBody] List<AuthorizedProjectViewModel> data)
        {
            ErrorHandler result = new ErrorHandler();
            result.eReasonCode = 2;
            result.eMessage = "Ocorreu um erro!!!";

            if (data == null)
            {
                result.eReasonCode = 2;
                result.eMessage = "Selecione os movimentos a faturar";
                return Json(result);
            }

            //Remove Project Authorized
            using (SuchDBContext ctx = new SuchDBContext())
            {
                var ProjectMovements = ctx.MovimentosDeProjeto.Where(x => x.GrupoFatura == data[0].GrupoFactura && x.NºProjeto == data[0].CodProjeto).ToList();
                List<MovimentosProjectoAutorizados> authorizedProjMovements = new List<MovimentosProjectoAutorizados>();

                if (ProjectMovements.Count > 0)
                {
                    MovimentosProjectoAutorizados movAutProject;
                    ProjectMovements.ForEach(x =>
                    {
                        x.FaturaçãoAutorizada = false;
                        x.FaturaçãoAutorizada2 = false;
                        x.Faturada = false;
                        x.GrupoFatura = (int?)null;
                        x.GrupoFaturaDescricao = string.Empty;
                        x.UtilizadorModificação = User.Identity.Name;
                        x.DataHoraModificação = DateTime.Now;

                        movAutProject = new MovimentosProjectoAutorizados();
                        movAutProject.NumMovimento = x.NºLinha;
                        authorizedProjMovements.Add(movAutProject);
                    });
                    ctx.MovimentosDeProjeto.UpdateRange(ProjectMovements);
                    try
                    {
                        ctx.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        result.eReasonCode = 2;
                        result.eMessage = "Ocorreu um erro ao atualizar os Movimentos de Projeto.";
                        return Json(result);
                    }

                    ctx.MovimentosProjectoAutorizados.RemoveRange(authorizedProjMovements);
                    try
                    {
                        ctx.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        result.eReasonCode = 2;
                        result.eMessage = "Ocorreu um erro ao eliminar os Movimentos de Projeto Autorizados.";
                        return Json(result);
                    }
                }

                var authorizedProj = ctx.ProjectosAutorizados.Where(x => x.GrupoFactura == data[0].GrupoFactura && x.CodProjeto == data[0].CodProjeto).ToList();
                if (authorizedProj.Count > 0)
                {
                    ctx.ProjectosAutorizados.RemoveRange(authorizedProj);
                    try
                    {
                        ctx.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        result.eReasonCode = 2;
                        result.eMessage = "Ocorreu um erro ao eliminar o Projeto Autorizados.";
                        return Json(result);
                    }
                }

                result.eReasonCode = 1;
                result.eMessage = "Linha anulada com sucesso.";
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateInvoiceLines([FromBody] List<SPInvoiceListViewModel> data, string OptionInvoice)
        {
            string execDetails = string.Empty;
            string errorMessage = string.Empty;
            bool hasErrors = false;
            ErrorHandler result = new ErrorHandler();
            if (data != null)
            {
                List<SPInvoiceListViewModel> groupedbyclient = data.GroupBy(x => new
                {
                    x.InvoiceToClientNo,
                    x.Date,
                    x.DateFim,
                    x.CommitmentNumber,
                    x.ClientRequest,

                }).Select(x => new SPInvoiceListViewModel
                {
                    InvoiceToClientNo = x.Key.InvoiceToClientNo,
                    Date = x.Key.Date,
                    DateFim = x.Key.DateFim,
                    CommitmentNumber = x.Key.CommitmentNumber,
                    ClientRequest = x.Key.ClientRequest,
                    ClientVATReg = DBNAV2017Clients.GetClientVATByNo(x.Key.InvoiceToClientNo, _config.NAVDatabaseName, _config.NAVCompanyName)

                }).ToList();

                //Create Project if existe
                Task<WSCreateNAVProject.Read_Result> Project = WSProject.GetNavProject(data[0].ProjectNo, _configws);
                Project.Wait();
                if (Project.IsCompletedSuccessfully && Project.Result.WSJob == null)
                {
                    try
                    {
                        ProjectDetailsViewModel proj = new ProjectDetailsViewModel();
                        proj.ProjectNo = data[0].ProjectNo;
                        proj.ClientNo = data[0].InvoiceToClientNo;
                        proj.RegionCode = data[0].RegionCode;
                        proj.ResponsabilityCenterCode = data[0].ResponsabilityCenterCode;
                        proj.FunctionalAreaCode = data[0].FunctionalAreaCode;
                        proj.Status = EstadoProjecto.Encomenda;
                        proj.Visivel = false;
                        Task<WSCreateNAVProject.Create_Result> createProject = WSProject.CreateNavProject(proj, _configws);
                        createProject.Wait();
                    }
                    catch (Exception ex)
                    {
                        if (!hasErrors)
                            hasErrors = true;

                        execDetails += " Erro ao criar Projeto: ";
                        errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                        result.eMessages.Add(new TraceInformation(TraceType.Exception, execDetails + errorMessage));
                        return Json(result);
                    }
                }

                if (groupedbyclient != null)
                {

                    foreach (var header in groupedbyclient)
                    {

                        try
                        {

                            header.MovementType = Convert.ToInt32(OptionInvoice);
                            header.CreateUser = User.Identity.Name;
                            execDetails = string.Format("Fat. Cliente: {0}, Data: {1}, Nº Compromisso: {2} - ", header.InvoiceToClientNo, header.Date, header.CommitmentNumber);
                            Task<WSCreatePreInvoice.Create_Result> TCreatePreInvoice = WSPreInvoice.CreatePreInvoice(header, _configws);
                            TCreatePreInvoice.Wait();

                            if (TCreatePreInvoice.IsCompletedSuccessfully)
                            {
                                string headerNo = TCreatePreInvoice.Result.WSPreInvoice.No;
                                execDetails += "Criada a fatura núm " + headerNo;

                                try
                                {
                                    List<SPInvoiceListViewModel> linesList = new List<SPInvoiceListViewModel>();
                                    foreach (var lines in data)
                                    {
                                        if (lines.InvoiceToClientNo == header.InvoiceToClientNo && lines.Date == header.Date && lines.CommitmentNumber == header.CommitmentNumber && lines.ClientRequest == header.ClientRequest)
                                        {
                                            linesList.Add(lines);
                                        }
                                    }
                                    //declarações
                                    List<NAVResourcesViewModel> Resourceslines = DBNAV2017Resources.GetAllResources(_config.NAVDatabaseName, _config.NAVCompanyName, "", "", 0, "");
                                    List<WasteRateViewModel> wr = DBWasteRate.ParseToViewModel(DBWasteRate.GetAll());
                                    List<ResourceGroupLinesModelView> myRLlist = new List<ResourceGroupLinesModelView>();
                                    List<ResourceGroupLinesModelView> myWRlist = new List<ResourceGroupLinesModelView>();

                                    //recursos com taxa residuo
                                    var selectedResources = linesList.Where(x => x.Type == 2).Select(x => x.Code).Distinct();
                                    var selectedWasteResources = Resourceslines.Where(x => x.WasteRate == 1 && selectedResources.Contains(x.Code));
                                    foreach (var item in selectedWasteResources)
                                    {
                                        decimal? quantity = linesList.Where(x => x.Code == item.Code).Sum(x => x.Quantity.HasValue ? Math.Round((decimal)x.Quantity, 2, MidpointRounding.AwayFromZero) : decimal.Zero);

                                        var wasteFamilyResources = wr.Where(x => x.FamiliaRecurso == item.ResourceGroup).ToList();
                                        wasteFamilyResources.ForEach(x =>
                                        {
                                            SPInvoiceListViewModel wasteLineToAdd = new SPInvoiceListViewModel();
                                            wasteLineToAdd.Quantity = quantity.HasValue ? Math.Round((decimal)quantity, 2, MidpointRounding.AwayFromZero) : decimal.Zero; // ?? 0;
                                            wasteLineToAdd.Code = item.Code;
                                            wasteLineToAdd.Description = item.Name;
                                            wasteLineToAdd.UnitPrice = Math.Round((decimal)item.UnitPrice, 4, MidpointRounding.AwayFromZero);
                                            linesList.Add(wasteLineToAdd);
                                        });
                                    }

                                    Task<WSCreatePreInvoiceLine.CreateMultiple_Result> TCreatePreInvoiceLine = WSPreInvoiceLine.CreatePreInvoiceLineListProject(linesList, headerNo, OptionInvoice, _configws);
                                    TCreatePreInvoiceLine.Wait();

                                    if (TCreatePreInvoiceLine.IsCompletedSuccessfully)
                                    {
                                        execDetails += " Linhas criadas com sucesso.";
                                        //update to Invoiced = true
                                        foreach (var updatelist in linesList)
                                        {
                                            MovimentosDeProjeto mov = DBProjectMovements.GetByLineNo(updatelist.LineNo).FirstOrDefault();
                                            mov.Faturada = true;
                                            DBProjectMovements.Update(mov);
                                        }
                                        result.eMessages.Add(new TraceInformation(TraceType.Success, execDetails));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (!hasErrors)
                                        hasErrors = true;

                                    execDetails += " Erro ao criar as linhas: ";
                                    errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                                    result.eMessages.Add(new TraceInformation(TraceType.Exception, execDetails + errorMessage));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (!hasErrors)
                                hasErrors = true;

                            execDetails += " Erro ao criar a fatura: ";
                            errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                            result.eMessages.Add(new TraceInformation(TraceType.Exception, execDetails + " " + errorMessage));
                        }
                    }
                    if (hasErrors)
                    {
                        result.eReasonCode = 2;
                        result.eMessage = "Ocorreram erros na criação de faturas.";
                    }
                    else
                    {
                        result.eReasonCode = 1;
                        result.eMessage = "Faturas criadas com sucesso.";
                    }
                }
                else
                {
                    result.eReasonCode = 2;
                    result.eMessage = "Selecionou registos com 'Nº de Compromisso' diferentes!";
                }
            }
            else
            {
                result.eReasonCode = 2;
                result.eMessage = "Selecione registos para faturar";
            }
            return Json(result);
        }
        #endregion

        #region Pre registo de Projetos

        public IActionResult PreMovimentosProjetos(string id)
        {
            UserAccessesViewModel userAccesses = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.PreRegistos);
            if (userAccesses != null && userAccesses.Read.Value)
            {
                if (id != null)
                {
                    ViewBag.UPermissions = userAccesses;
                    ViewBag.ProjectNo = id ?? "";
                    ViewBag.reportServerURL = _config.ReportServerURL;
                    return View();
                }
                else
                {
                    return RedirectToAction("PageNotFound", "Error");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }

        [HttpPost]
        public JsonResult GetPreMovementsProject([FromBody] string ProjectNo, bool registadas)
        {
            try
            {
                string NoCliente = DBProjects.GetById(ProjectNo).NºCliente;
                string ClientName = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, NoCliente).Name;

                List<Serviços> AllServices = DBServices.GetAll();
                List<TiposRefeição> AllMealTypes = DBMealTypes.GetAll();
                List<ClientServicesViewModel> AllServiceGroup = DBClientServices.GetAllServiceGroup(string.Empty, true);

                List<EnumData> AllTiposMovimentos = EnumerablesFixed.ProjectDiaryMovements;
                List<EnumData> AllTipos = EnumerablesFixed.ProjectDiaryTypes;
                List<NAVMeasureUnitViewModel> AllUnidadesMedida = DBNAV2017MeasureUnit.GetAllMeasureUnit(_config.NAVDatabaseName, _config.NAVCompanyName);
                List<NAVLocationsViewModel> AllLocations = DBNAV2017Locations.GetAllLocations(_config.NAVDatabaseName, _config.NAVCompanyName);
                List<NAVDimValueViewModel> AllRegions = DBNAV2017DimensionValues.GetByDimType(_config.NAVDatabaseName, _config.NAVCompanyName, 1);
                List<NAVDimValueViewModel> AllAreas = DBNAV2017DimensionValues.GetByDimType(_config.NAVDatabaseName, _config.NAVCompanyName, 2);
                List<NAVDimValueViewModel> AllCresps = DBNAV2017DimensionValues.GetByDimType(_config.NAVDatabaseName, _config.NAVCompanyName, 3);

                List<ProjectDiaryViewModel> dp = DBPreProjectMovements.GetPreRegisteredByProjectAndRegistadas(ProjectNo, registadas).Select(x => new ProjectDiaryViewModel()
                {
                    LineNo = x.NºLinha,
                    ProjectNo = x.NºProjeto,
                    Date = x.Data == null ? String.Empty : x.Data.Value.ToString("yyyy-MM-dd"),
                    MovementType = x.TipoMovimento,
                    MovementTypeText = x.TipoMovimento.HasValue ? AllTiposMovimentos.Where(y => y.Id == x.TipoMovimento).FirstOrDefault() != null ? AllTiposMovimentos.Where(y => y.Id == x.TipoMovimento).FirstOrDefault().Value : "" : "",
                    Type = x.Tipo,
                    TypeText = x.Tipo.HasValue ? AllTipos.Where(y => y.Id == x.Tipo).FirstOrDefault() != null ? AllTipos.Where(y => y.Id == x.Tipo).FirstOrDefault().Value : "" : "",
                    Code = x.Código,
                    Description = x.Descrição,
                    Quantity = x.Quantidade.HasValue ? Math.Round((decimal)x.Quantidade, 2, MidpointRounding.AwayFromZero) : decimal.Zero,
                    MeasurementUnitCode = x.CódUnidadeMedida,
                    MeasurementUnitCodeText = !string.IsNullOrEmpty(x.CódUnidadeMedida) ? AllUnidadesMedida.Where(y => y.Code == x.CódUnidadeMedida).FirstOrDefault() != null ? AllUnidadesMedida.Where(y => y.Code == x.CódUnidadeMedida).FirstOrDefault().Description : "" : "",
                    LocationCode = x.CódLocalização,
                    LocationCodeText = !string.IsNullOrEmpty(x.CódLocalização) ? AllLocations.Where(y => y.Code == x.CódLocalização).FirstOrDefault() != null ? AllLocations.Where(y => y.Code == x.CódLocalização).FirstOrDefault().Name : "" : "",
                    ProjectContabGroup = x.GrupoContabProjeto,
                    RegionCode = x.CódigoRegião,
                    RegionCodeText = !string.IsNullOrEmpty(x.CódigoRegião) ? AllRegions.Where(y => y.Code == x.CódigoRegião).FirstOrDefault() != null ? AllRegions.Where(y => y.Code == x.CódigoRegião).FirstOrDefault().Name : "" : "",
                    FunctionalAreaCode = x.CódigoÁreaFuncional,
                    FunctionalAreaCodeText = !string.IsNullOrEmpty(x.CódigoÁreaFuncional) ? AllAreas.Where(y => y.Code == x.CódigoÁreaFuncional).FirstOrDefault() != null ? AllAreas.Where(y => y.Code == x.CódigoÁreaFuncional).FirstOrDefault().Name : "" : "",
                    ResponsabilityCenterCode = x.CódigoCentroResponsabilidade,
                    ResponsabilityCenterCodeText = !string.IsNullOrEmpty(x.CódigoCentroResponsabilidade) ? AllCresps.Where(y => y.Code == x.CódigoCentroResponsabilidade).FirstOrDefault() != null ? AllCresps.Where(y => y.Code == x.CódigoCentroResponsabilidade).FirstOrDefault().Name : "" : "",
                    User = x.Utilizador,
                    UnitCost = x.CustoUnitário.HasValue ? Math.Round((decimal)x.CustoUnitário, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                    TotalCost = x.Quantidade.HasValue && x.CustoUnitário.HasValue ? Math.Round((decimal)(x.Quantidade * x.CustoUnitário), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //x.CustoTotal,
                    UnitPrice = x.PreçoUnitário.HasValue ? Math.Round((decimal)x.PreçoUnitário, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                    TotalPrice = x.Quantidade.HasValue && x.PreçoUnitário.HasValue ? Math.Round((decimal)(x.Quantidade * x.PreçoUnitário), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //x.PreçoTotal,
                    Billable = x.Faturável,
                    BillableText = x.Faturável.HasValue ? x.Faturável == true ? "Sim" : "Não" : "Não",
                    Registered = x.Registado,
                    MealType = x.TipoRefeição,
                    FolhaHoras = x.NºDocumento,
                    InvoiceToClientNo = x.FaturaANºCliente,
                    ServiceClientCode = x.CódServiçoCliente,
                    ServiceGroupCode = x.CódGrupoServiço,
                    ServiceGroupCodeDescription = AllServiceGroup.Where(y => y.ClientNumber == NoCliente && y.ServiceCode == x.CódGrupoServiço).FirstOrDefault() != null ? AllServiceGroup.Where(y => y.ClientNumber == NoCliente && y.ServiceCode == x.CódGrupoServiço).FirstOrDefault().ServiceDescription : "",
                    ClientName = ClientName,
                    //ClientName = DBNAV2017Clients.GetClientNameByNo(x.FaturaANºCliente, _config.NAVDatabaseName, _config.NAVCompanyName),
                    ServiceClientDescription = !String.IsNullOrEmpty(x.CódServiçoCliente) ? AllServices.Where(y => y.Código == x.CódServiçoCliente).FirstOrDefault().Descrição : "",
                    MealTypeDescription = x.TipoRefeição != null ? AllMealTypes.Where(y => y.Código == x.TipoRefeição).FirstOrDefault().Descrição : ""

                }).ToList();
                //foreach (ProjectDiaryViewModel item in dp)
                //{
                //    if (!String.IsNullOrEmpty(item.ServiceClientCode))
                //    {
                //        Serviços GetService = DBServices.GetById(item.ServiceClientCode);
                //        if (GetService != null)
                //        {
                //            item.ServiceClientDescription = GetService.Descrição;
                //        }

                //    }
                //    if (item.MealType != null)
                //    {
                //        TiposRefeição TRrow = DBMealTypes.GetById(item.MealType.Value);
                //        if (TRrow != null)
                //        {
                //            item.MealTypeDescription = TRrow.Descrição;
                //        }
                //    }
                //    else
                //    {
                //        item.MealTypeDescription = "";
                //    }
                //}
                return Json(dp);
            }
            catch (Exception e)
            {
                //throw;
                return Json(null);
            }
        }

        [HttpPost]
        public JsonResult GetFaturasNotasList([FromBody] string ProjectNo)
        {
            try
            {
                List<FaturasNotasViewModel> result = DBNAV2017Projects.GetFaturasNotasByProject(_config.NAVDatabaseName, _config.NAVCompanyName, ProjectNo);

                return Json(result);
            }
            catch (Exception e)
            {
                return Json(null);
            }
        }

        public IActionResult PreregistoProjetos(String id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.PreRegistos);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                ViewBag.ProjectNo = id ?? "";
                ViewBag.ProjectDescription = "";
                if (ViewBag.ProjectNo != "")
                {
                    ViewBag.ProjectDescription = DBProjects.GetById(id).Descrição;
                }
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        [HttpPost]
        public JsonResult CreatePDByMovPreProj([FromBody] List<ProjectDiaryViewModel> dp, string projectNo, string Resources, string ProjDiaryPrice, string Date)
        {
            ProjectDiaryResponse response = new ProjectDiaryResponse();
            string proj = dp.First().ProjectNo;
            string notCreatedLines = "";
            bool MoreThanOne = false;
            int OrderLine = 0;
            Projetos projecto = DBProjects.GetById(proj);
            if (dp != null)
            {
                response.Items = dp;
                foreach (ProjectDiaryViewModel item in dp)
                {
                    item.Date = Date;
                }
            }


            response.eReasonCode = 1;
            response.eMessage = "Pré-Registo atualizado.";

            if (!string.IsNullOrEmpty(proj) && !string.IsNullOrEmpty(ProjDiaryPrice) && ProjDiaryPrice == "1")
            {
                if (!string.IsNullOrEmpty(Resources) && Resources != "undefined")
                {
                    if (!string.IsNullOrEmpty(projecto.NºContrato))
                    {
                        List<LinhasContratos> listContractLines = DBContractLines.GetbyContractId(projecto.NºContrato, Resources);
                        if (listContractLines != null && listContractLines.Count > 0)
                        {
                            if (dp.Count > 0)
                            {
                                foreach (ProjectDiaryViewModel pjD in dp)
                                {
                                    OrderLine++;
                                    bool newUnitCost = false;
                                    if (pjD.ServiceClientCode == null || pjD.ServiceClientCode == "")
                                    {
                                        pjD.ServiceClientCode = "";
                                    }
                                    foreach (LinhasContratos lc in listContractLines)
                                    {
                                        if (lc.CódServiçoCliente == null || lc.CódServiçoCliente == "")
                                        {
                                            lc.CódServiçoCliente = "";
                                        }
                                        if (pjD.ServiceClientCode == lc.CódServiçoCliente && newUnitCost == false)
                                        {
                                            pjD.UnitCost = lc.PreçoUnitário.HasValue ? Math.Round((decimal)lc.PreçoUnitário, 4, MidpointRounding.AwayFromZero) : decimal.Zero;
                                            newUnitCost = true;
                                        }
                                    }
                                    if (newUnitCost)
                                    {
                                        //Create
                                        DiárioDeProjeto newdp = new DiárioDeProjeto()
                                        {
                                            NºLinha = pjD.LineNo,
                                            NºProjeto = pjD.ProjectNo,
                                            Data = pjD.Date == "" || pjD.Date == String.Empty ? (DateTime?)null : DateTime.Parse(pjD.Date),
                                            TipoMovimento = pjD.MovementType,
                                            Tipo = pjD.Type,
                                            Código = pjD.Code,
                                            Descrição = pjD.Description,
                                            Quantidade = pjD.Quantity.HasValue ? Math.Round((decimal)pjD.Quantity, 2, MidpointRounding.AwayFromZero) : decimal.Zero,
                                            //Quantidade = pjD.Quantity,
                                            CódUnidadeMedida = pjD.MeasurementUnitCode,
                                            CódLocalização = pjD.LocationCode,
                                            GrupoContabProjeto = pjD.ProjectContabGroup,
                                            CódigoRegião = projecto.CódigoRegião,
                                            CódigoÁreaFuncional = projecto.CódigoÁreaFuncional,
                                            CódigoCentroResponsabilidade = projecto.CódigoCentroResponsabilidade,
                                            Utilizador = User.Identity.Name,
                                            CustoUnitário = pjD.UnitCost.HasValue ? Math.Round((decimal)pjD.UnitCost, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                                            CustoTotal = pjD.Quantity.HasValue && pjD.UnitCost.HasValue ? Math.Round((decimal)(pjD.Quantity * pjD.UnitCost), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //pjD.TotalCost,
                                            PreçoUnitário = pjD.UnitPrice.HasValue ? Math.Round((decimal)pjD.UnitPrice, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                                            PreçoTotal = pjD.Quantity.HasValue && pjD.UnitPrice.HasValue ? Math.Round((decimal)(pjD.Quantity * pjD.UnitPrice), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //pjD.TotalPrice,
                                            //CustoUnitário = pjD.UnitCost,
                                            //CustoTotal = pjD.TotalCost,
                                            //PreçoUnitário = pjD.UnitPrice,
                                            //PreçoTotal = pjD.TotalPrice,
                                            Faturável = pjD.Billable,
                                            Registado = false,
                                            FaturaANºCliente = projecto.NºCliente,
                                            Moeda = pjD.Currency,
                                            ValorUnitárioAFaturar = pjD.UnitValueToInvoice,
                                            TipoRefeição = pjD.MealType,
                                            CódGrupoServiço = pjD.ServiceGroupCode,
                                            NºGuiaResíduos = pjD.ResidueGuideNo,
                                            NºGuiaExterna = pjD.ExternalGuideNo,
                                            DataConsumo = pjD.ConsumptionDate == "" || pjD.ConsumptionDate == String.Empty ? (DateTime?)null : DateTime.Parse(pjD.ConsumptionDate),
                                            CódServiçoCliente = pjD.ServiceClientCode,
                                            PréRegisto = true
                                        };
                                        if (pjD.LineNo > 0)
                                        {
                                            newdp.Faturada = pjD.Billed;
                                            newdp.DataHoraModificação = DateTime.Now;
                                            newdp.UtilizadorModificação = User.Identity.Name;
                                            DBProjectDiary.Update(newdp);
                                        }
                                        else
                                        {
                                            newdp.Faturada = false;
                                            newdp.DataHoraCriação = DateTime.Now;
                                            newdp.UtilizadorCriação = User.Identity.Name;
                                            DBProjectDiary.Create(newdp);
                                        }
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(notCreatedLines))
                                        {
                                            notCreatedLines = OrderLine + "ª ";
                                        }
                                        else
                                        {
                                            notCreatedLines = notCreatedLines + ", " + OrderLine + "ª ";
                                            MoreThanOne = true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            response.eReasonCode = 4;
                            response.eMessage = "O Contrato " + projecto.NºContrato + " não contém o recurso " + Resources + " nas suas linhas.";
                        }
                    }
                    else
                    {
                        response.eReasonCode = 3;
                        response.eMessage = "O projeto destino não contem contrato associado.";
                    }
                }
                else
                {
                    response.eReasonCode = 2;
                    response.eMessage = "Quando seleciona opção Contrato do campo Preço, é obrigatório selecionar um Recurso.";
                }
                if (!string.IsNullOrEmpty(notCreatedLines) && MoreThanOne)
                {
                    response.eReasonCode = 5;
                    response.eMessage = "Das linhas que foram selecionadas a " + notCreatedLines + " não foram criadas, porque o Código Serviço de Cliente, não existe no Contrato " + projecto.NºContrato;
                }
                else if (!string.IsNullOrEmpty(notCreatedLines))
                {
                    response.eReasonCode = 6;
                    response.eMessage = "A " + notCreatedLines + " linha não foi criada, porque o Código Serviço de Cliente, não existe no Contrato " + projecto.NºContrato;
                }
            }
            else
            {
                try
                {
                    //Create
                    dp.ForEach(x =>
                    {
                        DiárioDeProjeto newdp = new DiárioDeProjeto()
                        {
                            NºLinha = x.LineNo,
                            NºProjeto = x.ProjectNo,
                            Data = x.Date == "" || x.Date == String.Empty ? (DateTime?)null : DateTime.Parse(x.Date),
                            TipoMovimento = x.MovementType,
                            Tipo = x.Type,
                            Código = x.Code,
                            Descrição = x.Description,
                            Quantidade = x.Quantity.HasValue ? Math.Round((decimal)x.Quantity, 2, MidpointRounding.AwayFromZero) : decimal.Zero,
                            //Quantidade = x.Quantity,
                            CódUnidadeMedida = x.MeasurementUnitCode,
                            CódLocalização = x.LocationCode,
                            GrupoContabProjeto = x.ProjectContabGroup,
                            CódigoRegião = projecto.CódigoRegião,
                            CódigoÁreaFuncional = projecto.CódigoÁreaFuncional,
                            CódigoCentroResponsabilidade = projecto.CódigoCentroResponsabilidade,
                            Utilizador = User.Identity.Name,
                            CustoUnitário = x.UnitCost.HasValue ? Math.Round((decimal)x.UnitCost, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                            CustoTotal = x.Quantity.HasValue && x.UnitCost.HasValue ? Math.Round((decimal)(x.Quantity * x.UnitCost), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //x.TotalCost,
                            PreçoUnitário = x.UnitPrice.HasValue ? Math.Round((decimal)x.UnitPrice, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                            PreçoTotal = x.Quantity.HasValue && x.UnitPrice.HasValue ? Math.Round((decimal)(x.Quantity * x.UnitPrice), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //x.TotalPrice,
                            //CustoUnitário = x.UnitCost,
                            //CustoTotal = x.TotalCost,
                            //PreçoUnitário = x.UnitPrice,
                            //PreçoTotal = x.TotalPrice,
                            Faturável = x.Billable,
                            Registado = false,
                            FaturaANºCliente = projecto.NºCliente,
                            Moeda = x.Currency,
                            ValorUnitárioAFaturar = x.UnitValueToInvoice,
                            TipoRefeição = x.MealType,
                            CódGrupoServiço = x.ServiceGroupCode,
                            NºGuiaResíduos = x.ResidueGuideNo,
                            NºGuiaExterna = x.ExternalGuideNo,
                            DataConsumo = x.ConsumptionDate == "" || x.ConsumptionDate == String.Empty ? (DateTime?)null : DateTime.Parse(x.ConsumptionDate),
                            CódServiçoCliente = x.ServiceClientCode,
                            PréRegisto = true

                        };

                        if (x.LineNo > 0)
                        {
                            newdp.Faturada = x.Billed;
                            newdp.DataHoraModificação = DateTime.Now;
                            newdp.UtilizadorModificação = User.Identity.Name;
                            DBProjectDiary.Update(newdp);
                        }
                        else
                        {
                            newdp.Faturada = false;
                            newdp.DataHoraCriação = DateTime.Now;
                            newdp.UtilizadorCriação = User.Identity.Name;
                            DBProjectDiary.Create(newdp);
                        }
                    });
                }
                catch
                {
                    response.eReasonCode = 2;
                    response.eMessage = "Occorreu um erro ao atualizar o Pré-Registo.";
                }
            }
            return Json(response);// dp);
        }

        [HttpPost]
        public JsonResult GetAllPreRegistProject([FromBody]string projectNo)
        {
            List<ProjectDiaryViewModel> dp = null;
            if (projectNo == null || projectNo == "")
            {
                dp = DBProjectDiary.GetAllOpenPreRegist(User.Identity.Name).Select(x => new ProjectDiaryViewModel()
                {
                    LineNo = x.NºLinha,
                    ProjectNo = x.NºProjeto,
                    Date = !x.Data.HasValue ? String.Empty : x.Data.Value.ToString("yyyy-MM-dd"),
                    MovementType = x.TipoMovimento,
                    Type = x.Tipo,
                    Code = x.Código,
                    Description = x.Descrição,
                    //Quantity = x.Quantidade.HasValue ? Math.Round((decimal)x.Quantidade, 2) : decimal.Zero,
                    Quantity = x.Quantidade,
                    MeasurementUnitCode = x.CódUnidadeMedida,
                    LocationCode = x.CódLocalização,
                    ProjectContabGroup = x.GrupoContabProjeto,
                    RegionCode = x.CódigoRegião,
                    FunctionalAreaCode = x.CódigoÁreaFuncional,
                    ResponsabilityCenterCode = x.CódigoCentroResponsabilidade,
                    User = x.Utilizador,
                    UnitCost = x.CustoUnitário.HasValue ? Math.Round((decimal)x.CustoUnitário, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                    TotalCost = x.Quantidade.HasValue && x.CustoUnitário.HasValue ? Math.Round((decimal)(x.Quantidade * x.CustoUnitário), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //x.CustoTotal,
                    UnitPrice = x.PreçoUnitário.HasValue ? Math.Round((decimal)x.PreçoUnitário, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                    TotalPrice = x.Quantidade.HasValue && x.PreçoUnitário.HasValue ? Math.Round((decimal)(x.Quantidade * x.PreçoUnitário), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //x.PreçoTotal,
                    Billable = x.Faturável,
                    Registered = x.Registado,
                    Billed = x.Faturada.HasValue ? x.Faturada.Value : false,
                    Currency = x.Moeda,
                    UnitValueToInvoice = x.ValorUnitárioAFaturar,
                    MealType = x.TipoRefeição,
                    ServiceGroupCode = x.CódGrupoServiço,
                    ResidueGuideNo = x.NºGuiaResíduos,
                    ExternalGuideNo = x.NºGuiaExterna,
                    ConsumptionDate = !x.DataConsumo.HasValue ? "" : x.DataConsumo.Value.ToString("yyyy-MM-dd"),
                    InvoiceToClientNo = x.FaturaANºCliente,
                    ServiceClientCode = x.CódServiçoCliente
                }).ToList();
            }
            else
            {
                dp = DBProjectDiary.GetPreRegistByProjectNo(projectNo, User.Identity.Name).Select(x => new ProjectDiaryViewModel()
                {
                    LineNo = x.NºLinha,
                    ProjectNo = x.NºProjeto,
                    Date = !x.Data.HasValue ? String.Empty : x.Data.Value.ToString("yyyy-MM-dd"),
                    MovementType = x.TipoMovimento,
                    Type = x.Tipo,
                    Code = x.Código,
                    Description = x.Descrição,
                    //Quantity = x.Quantidade.HasValue ? Math.Round((decimal)x.Quantidade, 2) : decimal.Zero,
                    Quantity = x.Quantidade,
                    MeasurementUnitCode = x.CódUnidadeMedida,
                    LocationCode = x.CódLocalização,
                    ProjectContabGroup = x.GrupoContabProjeto,
                    RegionCode = x.CódigoRegião,
                    FunctionalAreaCode = x.CódigoÁreaFuncional,
                    ResponsabilityCenterCode = x.CódigoCentroResponsabilidade,
                    User = x.Utilizador,
                    UnitCost = x.CustoUnitário.HasValue ? Math.Round((decimal)x.CustoUnitário, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                    TotalCost = x.Quantidade.HasValue && x.CustoUnitário.HasValue ? Math.Round((decimal)(x.Quantidade * x.CustoUnitário), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //x.CustoTotal,
                    UnitPrice = x.PreçoUnitário.HasValue ? Math.Round((decimal)x.PreçoUnitário, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                    TotalPrice = x.Quantidade.HasValue && x.PreçoUnitário.HasValue ? Math.Round((decimal)(x.Quantidade * x.PreçoUnitário), 2, MidpointRounding.AwayFromZero) : decimal.Zero, //x.PreçoTotal,
                    Billable = x.Faturável,
                    Registered = x.Registado,
                    Billed = x.Faturada.HasValue ? x.Faturada.Value : false,
                    Currency = x.Moeda,
                    UnitValueToInvoice = x.ValorUnitárioAFaturar,
                    MealType = x.TipoRefeição,
                    ServiceGroupCode = x.CódGrupoServiço,
                    ResidueGuideNo = x.NºGuiaResíduos,
                    ExternalGuideNo = x.NºGuiaExterna,
                    ConsumptionDate = !x.DataConsumo.HasValue ? "" : x.DataConsumo.Value.ToString("yyyy-MM-dd"),
                    InvoiceToClientNo = x.FaturaANºCliente,
                    ServiceClientCode = x.CódServiçoCliente
                }).ToList();
            }
            ProjectDiaryResponse response = new ProjectDiaryResponse();
            response.eReasonCode = 1;
            response.Items = dp;

            return Json(response);
        }

        [HttpPost]
        public JsonResult UpdatePreRegistProject([FromBody] List<ProjectDiaryViewModel> dp, string projectNo)
        {
            ProjectDiaryResponse response = new ProjectDiaryResponse();
            response.eReasonCode = 1;
            response.eMessage = "Pré-Registo atualizado.";
            if (dp != null)
                response.Items = dp;
            //Update or Create
            try
            {
                List<DiárioDeProjeto> previousList;
                if (projectNo == null || projectNo == "")
                {
                    // Get All
                    previousList = DBProjectDiary.GetAllPreRegist(User.Identity.Name);
                }
                else
                {
                    previousList = DBProjectDiary.GetPreRegistByProjectNo(projectNo, User.Identity.Name);
                }

                foreach (DiárioDeProjeto line in previousList)
                {
                    if (!dp.Any(x => x.LineNo == line.NºLinha))
                    {
                        DBProjectDiary.Delete(line);

                        TabelaLog TabLog = new TabelaLog
                        {
                            Tabela = "[dbo].[Diário de Projeto]",
                            Descricao = "Delete - [Nº Linha]: " + line.NºLinha.ToString() + " - [Nº Projeto]: " + line.NºProjeto + " - [Data]: " + Convert.ToDateTime(line.Data).ToShortDateString() + " - [Código]: " + line.Código,
                            Utilizador = User.Identity.Name,
                            DataHora = DateTime.Now
                        };
                        DBTabelaLog.Create(TabLog);
                    }
                }


                dp.ForEach(x =>
                {
                    List<DiárioDeProjeto> dpObject = DBProjectDiary.GetPreRegistByLineNo(x.LineNo, User.Identity.Name);

                    if (dpObject.Count > 0)
                    {
                        DiárioDeProjeto newdp = dpObject.FirstOrDefault();

                        newdp.NºLinha = x.LineNo;
                        newdp.NºProjeto = x.ProjectNo;
                        newdp.Data = x.Date == "" || x.Date == null ? (DateTime?)null : DateTime.Parse(x.Date);
                        newdp.TipoMovimento = x.MovementType;
                        newdp.Tipo = x.Type;
                        newdp.Código = x.Code;
                        newdp.Descrição = x.Description;
                        //newdp.Quantidade = x.Quantity.HasValue ? Math.Round((decimal)x.Quantity, 2) : decimal.Zero;
                        newdp.Quantidade = x.Quantity;
                        newdp.CódUnidadeMedida = x.MeasurementUnitCode;
                        newdp.CódLocalização = x.LocationCode;
                        newdp.GrupoContabProjeto = x.ProjectContabGroup;
                        newdp.CódigoRegião = x.RegionCode;
                        newdp.CódigoÁreaFuncional = x.FunctionalAreaCode;
                        newdp.CódigoCentroResponsabilidade = x.ResponsabilityCenterCode;
                        newdp.Utilizador = User.Identity.Name;
                        newdp.CustoUnitário = x.UnitCost.HasValue ? Math.Round((decimal)x.UnitCost, 4, MidpointRounding.AwayFromZero) : decimal.Zero;
                        newdp.CustoTotal = x.Quantity.HasValue && x.UnitCost.HasValue ? Math.Round((decimal)(x.Quantity * x.UnitCost), 2, MidpointRounding.AwayFromZero) : decimal.Zero;
                        newdp.PreçoUnitário = x.UnitPrice.HasValue ? Math.Round((decimal)x.UnitPrice, 4, MidpointRounding.AwayFromZero) : decimal.Zero;
                        newdp.PreçoTotal = x.Quantity.HasValue && x.UnitPrice.HasValue ? Math.Round((decimal)(x.Quantity * x.UnitPrice), 2, MidpointRounding.AwayFromZero) : decimal.Zero;
                        newdp.Faturável = x.Billable;
                        newdp.Registado = false;
                        newdp.FaturaANºCliente = x.InvoiceToClientNo;
                        newdp.Moeda = x.Currency;
                        newdp.ValorUnitárioAFaturar = x.UnitValueToInvoice;
                        newdp.TipoRefeição = x.MealType;
                        newdp.CódGrupoServiço = x.ServiceGroupCode;
                        newdp.NºGuiaResíduos = x.ResidueGuideNo;
                        newdp.NºGuiaExterna = x.ExternalGuideNo;
                        newdp.DataConsumo = x.ConsumptionDate == "" || x.ConsumptionDate == null ? (DateTime?)null : DateTime.Parse(x.ConsumptionDate);
                        newdp.CódServiçoCliente = x.ServiceClientCode;
                        newdp.Faturada = x.Billed;
                        newdp.DataHoraModificação = DateTime.Now;
                        newdp.UtilizadorModificação = User.Identity.Name;
                        newdp.PréRegisto = true;
                        DBProjectDiary.Update(newdp);
                    }
                    else
                    {
                        DiárioDeProjeto newdp = new DiárioDeProjeto()
                        {
                            NºLinha = x.LineNo,
                            NºProjeto = x.ProjectNo,
                            Data = x.Date == "" || x.Date == null ? (DateTime?)null : DateTime.Parse(x.Date),
                            TipoMovimento = x.MovementType,
                            Tipo = x.Type,
                            Código = x.Code,
                            Descrição = x.Description,
                            //Quantidade = x.Quantity.HasValue ? Math.Round((decimal)x.Quantity, 2) : decimal.Zero,
                            Quantidade = x.Quantity,
                            CódUnidadeMedida = x.MeasurementUnitCode,
                            CódLocalização = x.LocationCode,
                            GrupoContabProjeto = x.ProjectContabGroup,
                            CódigoRegião = x.RegionCode,
                            CódigoÁreaFuncional = x.FunctionalAreaCode,
                            CódigoCentroResponsabilidade = x.ResponsabilityCenterCode,
                            Utilizador = User.Identity.Name,
                            CustoUnitário = x.UnitCost.HasValue ? Math.Round((decimal)x.UnitCost, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                            CustoTotal = x.Quantity.HasValue && x.UnitCost.HasValue ? Math.Round((decimal)(x.Quantity * x.UnitCost), 2, MidpointRounding.AwayFromZero) : decimal.Zero,
                            PreçoUnitário = x.UnitPrice.HasValue ? Math.Round((decimal)x.UnitPrice, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                            PreçoTotal = x.Quantity.HasValue && x.UnitPrice.HasValue ? Math.Round((decimal)(x.Quantity * x.UnitPrice), 2, MidpointRounding.AwayFromZero) : decimal.Zero,
                            Faturável = x.Billable,
                            Registado = false,
                            FaturaANºCliente = x.InvoiceToClientNo,
                            Moeda = x.Currency,
                            ValorUnitárioAFaturar = x.UnitValueToInvoice,
                            TipoRefeição = x.MealType,
                            CódGrupoServiço = x.ServiceGroupCode,
                            NºGuiaResíduos = x.ResidueGuideNo,
                            NºGuiaExterna = x.ExternalGuideNo,
                            DataConsumo = x.ConsumptionDate == "" || x.ConsumptionDate == null ? (DateTime?)null : DateTime.Parse(x.ConsumptionDate),
                            CódServiçoCliente = x.ServiceClientCode,
                            PréRegisto = true

                        };

                        newdp.Faturada = false;
                        newdp.DataHoraCriação = DateTime.Now;
                        newdp.UtilizadorCriação = User.Identity.Name;
                        DBProjectDiary.Create(newdp);
                    }
                });
            }
            catch (Exception e)
            {
                //throw;
                response.eReasonCode = 2;
                response.eMessage = "Occorreu um erro ao atualizar o Pré-Registo.";
            }

            return Json(response);
        }

        [HttpPost]
        public JsonResult GetPreMovements([FromBody] string projectNo, string data, string codSClient, string codSGroupClient)
        {
            DateTime? DataValue = null;
            if (!String.IsNullOrEmpty(data))
            {
                DataValue = Convert.ToDateTime(data);
            }
            ErrorHandler result = new ErrorHandler();
            result.eReasonCode = 1;
            result.eMessage = "Os movimentos foram obtidos com sucesso";
            List<DiárioDeProjeto> dp = new List<DiárioDeProjeto>();
            if (!String.IsNullOrEmpty(projectNo))
            {
                Projetos proj = DBProjects.GetById(projectNo);
                if (proj != null && !String.IsNullOrEmpty(proj.NºContrato))
                {
                    Contratos lcontracts = DBContracts.GetActualContract(proj.NºContrato, proj.NºCliente);

                    if (lcontracts != null)
                    {
                        if (!String.IsNullOrEmpty(codSClient))
                        {
                            dp = DBContractLines.GetAllBySClient(lcontracts.NºDeContrato, lcontracts.NºVersão, codSClient).Select(
                           x => new DiárioDeProjeto()
                           {
                               Data = DataValue,
                               NºProjeto = projectNo,
                               Tipo = x.Tipo,
                               CódServiçoCliente = x.CódServiçoCliente,
                               TipoMovimento = 1,
                               Código = x.Código,
                               Descrição = x.Descrição,
                               Quantidade = 0,
                               GrupoContabProjeto = proj.GrupoContabObra,
                               CódUnidadeMedida = x.CódUnidadeMedida,
                               CódigoRegião = x.CódigoRegião,
                               CódigoÁreaFuncional = x.CódigoÁreaFuncional,
                               CódigoCentroResponsabilidade = x.CódigoCentroResponsabilidade,
                               Utilizador = User.Identity.Name,
                               PreçoUnitário = x.PreçoUnitário.HasValue ? Math.Round((decimal)x.PreçoUnitário, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                               Faturável = x.Faturável,
                               Registado = false,
                               PréRegisto = true,
                               FaturaANºCliente = proj.NºCliente,
                           }).ToList();
                        }
                        else
                        {
                            dp = DBContractLines.GetAllByActiveContract(lcontracts.NºDeContrato, lcontracts.NºVersão).Select(
                           x => new DiárioDeProjeto()
                           {
                               Data = DataValue,
                               NºProjeto = projectNo,
                               CódServiçoCliente = x.CódServiçoCliente,
                               Tipo = x.Tipo,
                               TipoMovimento = 1,
                               Código = x.Código,
                               Descrição = x.Descrição,
                               Quantidade = 0,
                               GrupoContabProjeto = proj.GrupoContabObra,
                               CódUnidadeMedida = x.CódUnidadeMedida,
                               CódigoRegião = x.CódigoRegião,
                               CódigoÁreaFuncional = x.CódigoÁreaFuncional,
                               CódigoCentroResponsabilidade = x.CódigoCentroResponsabilidade,
                               Utilizador = User.Identity.Name,
                               PreçoUnitário = x.PreçoUnitário.HasValue ? Math.Round((decimal)x.PreçoUnitário, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                               Faturável = x.Faturável,
                               Registado = false,
                               PréRegisto = true,
                               FaturaANºCliente = proj.NºCliente,
                           }).ToList();
                        }

                        if (dp.Count == 0)
                        {
                            result.eReasonCode = 4;
                            result.eMessage = "Este projeto não tem contrato com linhas associadas";
                        }
                        foreach (var item in dp)
                        {
                            DiárioDeProjeto dpValidation = new DiárioDeProjeto();
                            if (!String.IsNullOrEmpty(codSGroupClient))
                            {
                                item.CódGrupoServiço = codSGroupClient;
                            }
                            item.UtilizadorCriação = User.Identity.Name;
                            item.DataHoraCriação = DateTime.Now;
                            dpValidation = DBProjectDiary.Create(item);
                            if (dpValidation == null)
                            {
                                result.eReasonCode = 5;
                                result.eMessage = "Occorreu um erro ao obter os movimentos";
                            }
                        }
                    }
                    else
                    {
                        result.eReasonCode = 4;
                        result.eMessage = "O Cliente " + proj.NºCliente + " do projeto selecinado não existe no Contrato " + proj.NºContrato;
                    }
                }
                else
                {
                    result.eReasonCode = 3;
                    result.eMessage = "Este projeto não tem contrato";
                }
            }
            else
            {
                result.eReasonCode = 2;
                result.eMessage = "Não foi selecionado nenhum projeto";
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult RegisterPreRegistLines([FromBody]  List<ProjectDiaryViewModel> dp)
        {
            //SET INTEGRATED IN DB
            if (dp != null)
            {
                string InvoiceClient = "";
                dp.ForEach(x =>
                {
                    if (x.Code != null)
                    {
                        DiárioDeProjeto newdp = DBProjectDiary.GetAllPreRegByCode(User.Identity.Name, x.Code);
                        if (newdp != null)
                        {
                            if (newdp.Quantidade != null && newdp.Quantidade != 0)
                            {
                                DBProjectDiary.Delete(newdp);
                                if (!String.IsNullOrEmpty(newdp.FaturaANºCliente))
                                {
                                    if (InvoiceClient != "")
                                    {
                                        InvoiceClient = newdp.FaturaANºCliente;
                                    }
                                    else
                                    {
                                        Projetos cProject = DBProjects.GetById(newdp.NºProjeto);
                                        if (cProject != null)
                                        {
                                            InvoiceClient = cProject.NºCliente;
                                        }
                                    }
                                }
                                else
                                {
                                    if (InvoiceClient == "")
                                    {
                                        Projetos cProject = DBProjects.GetById(newdp.NºProjeto);
                                        if (cProject != null)
                                        {
                                            InvoiceClient = cProject.NºCliente;
                                        }
                                    }
                                }
                                PréMovimentosProjeto ProjectMovement = new PréMovimentosProjeto()
                                {
                                    NºProjeto = newdp.NºProjeto,
                                    Data = newdp.Data,
                                    TipoMovimento = newdp.TipoMovimento,
                                    Tipo = newdp.Tipo,
                                    Código = newdp.Código,
                                    Descrição = newdp.Descrição,
                                    Quantidade = newdp.Quantidade.HasValue ? Math.Round((decimal)newdp.Quantidade, 2, MidpointRounding.AwayFromZero) : decimal.Zero,
                                    CódUnidadeMedida = newdp.CódUnidadeMedida,
                                    CódLocalização = newdp.CódLocalização,
                                    GrupoContabProjeto = newdp.GrupoContabProjeto,
                                    CódigoRegião = newdp.CódigoRegião,
                                    CódigoÁreaFuncional = newdp.CódigoÁreaFuncional,
                                    CódigoCentroResponsabilidade = newdp.CódigoCentroResponsabilidade,
                                    Utilizador = User.Identity.Name,
                                    CustoUnitário = newdp.CustoUnitário.HasValue ? Math.Round((decimal)newdp.CustoUnitário, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                                    CustoTotal = newdp.Quantidade.HasValue && newdp.CustoUnitário.HasValue ? Math.Round((decimal)(newdp.Quantidade * newdp.CustoUnitário), 2, MidpointRounding.AwayFromZero) : decimal.Zero,
                                    PreçoUnitário = newdp.PreçoUnitário.HasValue ? Math.Round((decimal)newdp.PreçoUnitário, 4, MidpointRounding.AwayFromZero) : decimal.Zero,
                                    PreçoTotal = newdp.Quantidade.HasValue && newdp.PreçoUnitário.HasValue ? Math.Round((decimal)(newdp.Quantidade * newdp.PreçoUnitário), 2, MidpointRounding.AwayFromZero) : decimal.Zero,
                                    Faturável = newdp.Faturável,
                                    Registado = false,
                                    Faturada = false,
                                    FaturaANºCliente = InvoiceClient,
                                    Moeda = newdp.Moeda,
                                    ValorUnitárioAFaturar = newdp.ValorUnitárioAFaturar,
                                    TipoRefeição = newdp.TipoRefeição,
                                    CódGrupoServiço = newdp.CódGrupoServiço,
                                    NºGuiaResíduos = newdp.NºGuiaResíduos,
                                    NºGuiaExterna = newdp.NºGuiaExterna,
                                    DataConsumo = newdp.DataConsumo,
                                    CódServiçoCliente = newdp.CódServiçoCliente,
                                    UtilizadorCriação = User.Identity.Name,
                                    DataHoraCriação = DateTime.Now,
                                    FaturaçãoAutorizada = false
                                };

                                DBPreProjectMovements.CreatePreRegist(ProjectMovement);
                            }
                        }
                    }
                });
            }
            return Json(dp);
        }

        [HttpPost]
        public JsonResult RegisterPreMovements([FromBody]  List<ProjectDiaryViewModel> dp, string StartDate, string EndDate)
        {
            ErrorHandler response = new ErrorHandler();
            response.eReasonCode = 1;
            response.eMessage = "Movimentos registados com Sucesso";

            try
            {
                List<ProjectDiaryViewModel> premov = new List<ProjectDiaryViewModel>();

                if (dp != null && dp.Count > 0 && !string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate))
                {
                    foreach (ProjectDiaryViewModel item in dp)
                    {
                        if (!string.IsNullOrEmpty(item.Date))
                        {
                            DateTime date = Convert.ToDateTime(item.Date);
                            DateTime Sdate = Convert.ToDateTime(StartDate);
                            DateTime Ddate = Convert.ToDateTime(EndDate);
                            if (date >= Sdate && date <= Ddate)
                            {
                                if (item.Registered == false)
                                {
                                    premov.Add(item);
                                }
                            }
                        }
                    }
                    if (premov != null && premov.Count > 0)
                    {
                        var groupedPreRecords = premov.GroupBy(x => new { x.ProjectNo, x.Code, x.ServiceGroupCode, x.ServiceClientCode },
                         x => x,
                         (Key, items) => new
                         {
                             ProjectNo = Key.ProjectNo,
                             Code = Key.Code,
                             ServiceGroupCode = Key.ServiceGroupCode,
                             ServiceClientCode = Key.ServiceClientCode,
                             Items = items,
                         }).ToList();

                        //load project diary data
                        List<ProjectDiaryViewModel> projectDiaryItems = new List<ProjectDiaryViewModel>();
                        foreach (var item in groupedPreRecords)
                        {
                            MovimentosDeProjeto ProjectMovement = new MovimentosDeProjeto();
                            if (item.Items.ToList().Count > 0)
                            {
                                ProjectDiaryViewModel line = item.Items.First();
                                line.Date = EndDate;
                                line.Quantity = item.Items.Sum(x => x.Quantity.HasValue ? Math.Round((decimal)x.Quantity, 2, MidpointRounding.AwayFromZero) : decimal.Zero);
                                line.TotalCost = line.UnitCost.HasValue && line.Quantity.HasValue ? Math.Round((decimal)(line.UnitCost * line.Quantity), 2, MidpointRounding.AwayFromZero) : decimal.Zero;
                                line.TotalPrice = line.UnitPrice.HasValue && line.Quantity.HasValue ? Math.Round((decimal)(line.UnitPrice * line.Quantity), 2, MidpointRounding.AwayFromZero) : decimal.Zero;
                                projectDiaryItems.Add(line);
                            }
                        }
                        //load project movement data
                        List<MovimentosDeProjeto> projectMovements = new List<MovimentosDeProjeto>();
                        MovimentosDeProjeto projectMovement;
                        foreach (var item in projectDiaryItems)
                        {
                            projectMovement = new MovimentosDeProjeto();
                            projectMovement.NºProjeto = item.ProjectNo;
                            projectMovement.Data = Convert.ToDateTime(EndDate);
                            projectMovement.TipoMovimento = item.MovementType;
                            projectMovement.Tipo = item.Type;
                            projectMovement.Código = item.Code;
                            projectMovement.Descrição = item.Description;
                            projectMovement.CódUnidadeMedida = item.MeasurementUnitCode;
                            projectMovement.CódLocalização = item.LocationCode;
                            projectMovement.GrupoContabProjeto = item.ProjectContabGroup;
                            projectMovement.CódigoRegião = item.RegionCode;
                            projectMovement.CódigoÁreaFuncional = item.FunctionalAreaCode;
                            projectMovement.CódigoCentroResponsabilidade = item.ResponsabilityCenterCode;
                            projectMovement.Utilizador = User.Identity.Name;
                            projectMovement.Faturável = item.Billable;
                            projectMovement.Registado = true;
                            projectMovement.Faturada = false;
                            projectMovement.FaturaANºCliente = item.InvoiceToClientNo;
                            projectMovement.Moeda = item.Coin;
                            projectMovement.ValorUnitárioAFaturar = item.UnitValueToInvoice;
                            projectMovement.TipoRefeição = item.MealType;
                            projectMovement.CódGrupoServiço = item.ServiceGroupCode;
                            projectMovement.NºGuiaResíduos = item.ResidueGuideNo;
                            projectMovement.NºGuiaExterna = item.ExternalGuideNo;
                            projectMovement.DataConsumo = item.ConsumptionDate != "" && item.ConsumptionDate != null ? DateTime.Parse(item.ConsumptionDate) : (DateTime?)null;
                            projectMovement.CódServiçoCliente = item.ServiceClientCode;
                            projectMovement.UtilizadorCriação = User.Identity.Name;
                            projectMovement.DataHoraCriação = DateTime.Now;
                            projectMovement.FaturaçãoAutorizada = false;
                            projectMovement.CustoUnitário = item.UnitCost.HasValue ? Math.Round((decimal)item.UnitCost, 4, MidpointRounding.AwayFromZero) : decimal.Zero;
                            projectMovement.PreçoUnitário = item.UnitPrice.HasValue ? Math.Round((decimal)item.UnitPrice, 4, MidpointRounding.AwayFromZero) : decimal.Zero;
                            projectMovement.Quantidade = item.Quantity.HasValue ? Math.Round((decimal)item.Quantity, 2, MidpointRounding.AwayFromZero) : decimal.Zero;
                            projectMovement.CustoTotal = item.Quantity.HasValue && item.UnitCost.HasValue ? Math.Round((decimal)(item.Quantity * item.UnitCost), 2, MidpointRounding.AwayFromZero) : decimal.Zero; //item.TotalCost;
                            projectMovement.PreçoTotal = item.Quantity.HasValue && item.UnitPrice.HasValue ? Math.Round((decimal)(item.Quantity * item.UnitPrice), 2, MidpointRounding.AwayFromZero) : decimal.Zero; //item.TotalPrice;

                            projectMovements.Add(projectMovement);
                        }

                        projectDiaryItems.RemoveAll(x => x.Quantity == 0);

                        //Create project diary in NAV
                        Guid transactionId = Guid.NewGuid();
                        try
                        {
                            //Create Lines in NAV
                            try
                            {
                                Task<WSCreateProjectDiaryLine.CreateMultiple_Result> createNavDiaryTask = WSProjectDiaryLine.CreateNavDiaryLines(projectDiaryItems, transactionId, _configws);
                                createNavDiaryTask.Wait();
                            }
                            catch (Exception ex)
                            {
                                response.eReasonCode = 3;
                                response.eMessage = "Erro: (" + ex.InnerException != null ? ex.InnerException.Message : ex.Message + ")";
                                return Json(response);
                            }

                            //Register Lines in NAV
                            try
                            {
                                Task<WSGenericCodeUnit.FxPostJobJrnlLines_Result> registerNavDiaryTask = WSProjectDiaryLine.RegsiterNavDiaryLines(transactionId, _configws);
                                registerNavDiaryTask.Wait();

                                if (!registerNavDiaryTask.IsCompletedSuccessfully)
                                {
                                    response.eReasonCode = 3;
                                    response.eMessage = "Não foi possivel criar as linhas no nav";
                                    Response.StatusCode = (int)HttpStatusCode.NoContent;
                                    return Json(premov);
                                }
                                else
                                {
                                    //Create project movement in e-SUCH and update pré-movements. Ensure atomicity
                                    using (var ctx = new SuchDBContext())
                                    {
                                        projectMovements.ForEach(x => x.DataHoraCriação = DateTime.Now);
                                        ctx.MovimentosDeProjeto.AddRange(projectMovements);

                                        foreach (var item in projectDiaryItems)
                                        {
                                            var preMovementIds = groupedPreRecords.SelectMany(x => x.Items).Where(x => x.ProjectNo == item.ProjectNo &&
                                                                                             x.Code == item.Code &&
                                                                                             x.ServiceGroupCode == item.ServiceGroupCode &&
                                                                                             x.ServiceClientCode == item.ServiceClientCode)
                                                                                 .Select(x => x.LineNo)
                                                                                 .ToList();

                                            var preMovements = DBPreProjectMovements.GetUnregisteredById(preMovementIds);
                                            if (preMovements != null && preMovements.Count > 0)
                                            {
                                                preMovements.ForEach(x =>
                                                {
                                                    x.Registado = true;
                                                    x.UtilizadorCriação = User.Identity.Name;
                                                    x.DataHoraModificação = DateTime.Now;
                                                });
                                                ctx.PréMovimentosProjeto.UpdateRange(preMovements);
                                            }
                                        }
                                        ctx.SaveChanges();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                response.eReasonCode = 3;
                                response.eMessage = "Erro: (" + ex.InnerException != null ? ex.InnerException.Message : ex.Message + ")";
                                return Json(response);
                            }

                        }
                        catch (Exception ex)
                        {
                            WSProjectDiaryLine.DeleteNavDiaryLines(transactionId, _configws);
                            response.eReasonCode = 3;
                            response.eMessage = "Não foi possivel criar as linhas no nav (" + ex.InnerException != null ? ex.InnerException.Message : ex.Message + ")";
                            //Response.StatusCode = (int)HttpStatusCode.NoContent;
                            return Json(response);
                        }
                    }
                    else if (dp != null)
                    {
                        response.eReasonCode = 2;
                        response.eMessage = "Não existe Pré-Movimentos por registar no intervalo de tempo selecionado";
                    }
                }
                else
                {
                    response.eReasonCode = 2;
                    response.eMessage = "A tabela Pré-Movimentos está vazia";
                }
            }
            catch (Exception ex)
            {
                response.eReasonCode = 3;
                response.eMessage = "Erro: (" + ex.InnerException != null ? ex.InnerException.Message : ex.Message + ")";
                return Json(response);
            }
            return Json(response);
        }

        [HttpPost]
        public JsonResult CreateDiaryByPriceServiceCient(string projectNo, string serviceCod, string serviceGroup, string dateRegist)
        {
            ProjectDiaryResponse response = new ProjectDiaryResponse();
            try
            {
                Projetos proj = DBProjects.GetById(projectNo);
                List<PriceServiceClientViewModel> dp = DBPriceServiceClient.ParseToViewModel(DBPriceServiceClient.GetAll()).Where(x => x.Client == proj.NºCliente && x.CodServClient == serviceCod).ToList();

                if (dp != null && dp.Count > 0)
                {
                    List<ProjectDiaryViewModel> newRows = new List<ProjectDiaryViewModel>();

                    foreach (PriceServiceClientViewModel item in dp)
                    {
                        ProjectDiaryViewModel newRow = new ProjectDiaryViewModel();
                        newRow.Date = dateRegist;
                        newRow.ProjectNo = projectNo;
                        newRow.InvoiceToClientNo = proj.NºCliente;
                        newRow.ServiceClientCode = serviceCod;
                        newRow.ServiceGroupCode = serviceGroup;
                        newRow.Type = 2;
                        newRow.Code = item.Resource;
                        newRow.Description = item.ResourceDescription;
                        newRow.MeasurementUnitCode = item.UnitMeasure;
                        newRow.UnitCost = item.PriceCost.HasValue ? Math.Round((decimal)item.PriceCost, 4, MidpointRounding.AwayFromZero) : decimal.Zero;
                        newRow.UnitPrice = item.SalePrice.HasValue ? Math.Round((decimal)item.SalePrice, 4, MidpointRounding.AwayFromZero) : decimal.Zero;
                        if (item.Quantidade.HasValue && item.Quantidade > 0) newRow.Quantity = item.Quantidade;
                        newRow.Billable = true;
                        newRow.ProjectContabGroup = proj.GrupoContabObra;
                        newRow.MovementType = 1;
                        if (!String.IsNullOrEmpty(item.TypeMeal))
                            newRow.MealType = Convert.ToInt32(item.TypeMeal);
                        else
                            newRow.MealType = null;
                        newRow.RegionCode = proj.CódigoRegião;
                        newRow.FunctionalAreaCode = proj.CódigoÁreaFuncional;
                        newRow.ResponsabilityCenterCode = proj.CódigoCentroResponsabilidade;
                        newRows.Add(newRow);
                    }
                    response.eReasonCode = 1;
                    response.eMessage = "";
                    response.Items = newRows;

                }
                else
                {
                    response.eReasonCode = 2;
                    response.eMessage = "Tabela Preços Serviços Cliente não existe nenhuma linha com o Nº Cliente = " + proj.NºCliente + " e o Código Serviço Cliente = " + serviceCod;
                }
            }
            catch (Exception)
            {
                response.eReasonCode = 3;
                response.eMessage = "Ocorreu algum erro ao Obter as linhas da Tabela Preços Serviços";
            }
            return Json(response);
        }
        #endregion

        #region Preços Serviços Cliente
        public IActionResult PreçosServiçosCliente()
        {
            UserAccessesViewModel userAccesses = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.PreçoServCliente);
            if (userAccesses != null && userAccesses.Read.Value)
            {
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        [HttpPost]
        public JsonResult GetAllPriceServiceClient()
        {
            List<PriceServiceClientViewModel> dp = DBPriceServiceClient.ParseToViewModel(DBPriceServiceClient.GetAll());
            return Json(dp);
        }
        [HttpPost]
        public JsonResult UpdatePriceServiceClient([FromBody] List<PriceServiceClientViewModel> dp)
        {
            ErrorHandler responde = new ErrorHandler();
            responde.eReasonCode = 1;
            responde.eMessage = "Atualizado com sucesso";
            if (dp != null)
            {
                List<PreçosServiçosCliente> getAllLines = DBPriceServiceClient.GetAll();
                if (getAllLines != null && getAllLines.Count > 0)
                {
                    foreach (PreçosServiçosCliente psc in getAllLines)
                    {
                        if (!dp.Any(x => x.Client == psc.Cliente && x.CodServClient == psc.CodServCliente && x.Resource == psc.Recurso))
                        {
                            DBPriceServiceClient.Delete(psc);
                        }
                    }
                    dp.ForEach(x =>
                    {
                        string nome1 = "", nome2 = "", resto = "";
                        int n = 0, n2 = 0;
                        List<PreçosServiçosCliente> dpObject = DBPriceServiceClient.GetByC_SC_R(x.Client, x.CodServClient, x.Resource);
                        if (dpObject != null && dpObject.Count > 0)
                        {
                            PreçosServiçosCliente newdp = DBPriceServiceClient.ParseToDatabase(x);
                            if (x.CompleteName != null && x.CompleteName.Length > 0)
                            {
                                if (x.CompleteName[x.CompleteName.Length - 1] == ' ')
                                {
                                    x.CompleteName = x.CompleteName.Substring(0, x.CompleteName.Length - 1);
                                }
                                if (x.CompleteName.Length > 80)
                                {
                                    nome1 = x.CompleteName.Substring(0, 80);
                                    nome2 = x.CompleteName.Substring(80, x.CompleteName.Length);
                                    if (nome1[nome1.Length - 1] != ' ')
                                    {
                                        if (nome2[0] != ' ')
                                        {
                                            n = nome1.LastIndexOf(" ");
                                            nome1 = x.CompleteName.Substring(0, n);
                                            nome2 = x.CompleteName.Substring(n + 1, x.CompleteName.Length);
                                            if (nome2.Length > 50)
                                            {
                                                nome2 = nome2.Substring(0, 50);
                                                resto = nome2.Substring(50, nome2.Length);
                                                if (nome2[nome2.Length - 1] != ' ')
                                                {
                                                    if (resto[0] != ' ')
                                                    {
                                                        n2 = nome2.LastIndexOf(" ");
                                                        nome2 = nome2.Substring(0, n2);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            nome2 = x.CompleteName.Substring(81, x.CompleteName.Length);
                                            if (nome2.Length > 50)
                                            {
                                                nome2 = nome2.Substring(0, 50);
                                                resto = nome2.Substring(50, nome2.Length);
                                                if (nome2[nome2.Length - 1] != ' ')
                                                {
                                                    if (resto[0] != ' ')
                                                    {
                                                        n2 = nome2.LastIndexOf(" ");
                                                        nome2 = nome2.Substring(0, n2);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        nome1 = x.CompleteName.Substring(0, 79);
                                        nome2 = x.CompleteName.Substring(80, x.CompleteName.Length);
                                        if (nome2.Length > 50)
                                        {
                                            nome2 = nome2.Substring(0, 50);
                                            resto = nome2.Substring(50, nome2.Length);
                                            if (nome2[nome2.Length - 1] != ' ')
                                            {
                                                if (resto[0] != ' ')
                                                {
                                                    n2 = nome2.LastIndexOf(" ");
                                                    nome2 = nome2.Substring(0, n2);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    nome1 = x.CompleteName;
                                    nome2 = "";
                                }
                                newdp.Nome = nome1;
                                newdp.Nome2 = nome2;
                            }


                            newdp.DataHoraModificação = DateTime.Now;
                            newdp.UtilizadorModificação = User.Identity.Name;
                            DBPriceServiceClient.Update(newdp);
                        }
                        else
                        {
                            PreçosServiçosCliente newdp = DBPriceServiceClient.ParseToDatabase(x);
                            if (x.CompleteName != null && x.CompleteName.Length > 0)
                            {
                                if (x.CompleteName[x.CompleteName.Length - 1] == ' ')
                                {
                                    x.CompleteName = x.CompleteName.Substring(0, x.CompleteName.Length - 1);
                                }
                                if (x.CompleteName.Length > 80)
                                {
                                    nome1 = x.CompleteName.Substring(0, 80);
                                    nome2 = x.CompleteName.Substring(80, x.CompleteName.Length);
                                    if (nome1[nome1.Length - 1] != ' ')
                                    {
                                        if (nome2[0] != ' ')
                                        {
                                            n = nome1.LastIndexOf(" ");
                                            nome1 = x.CompleteName.Substring(0, n);
                                            nome2 = x.CompleteName.Substring(n + 1, x.CompleteName.Length);
                                            if (nome2.Length > 50)
                                            {
                                                nome2 = nome2.Substring(0, 50);
                                                resto = nome2.Substring(50, nome2.Length);
                                                if (nome2[nome2.Length - 1] != ' ')
                                                {
                                                    if (resto[0] != ' ')
                                                    {
                                                        n2 = nome2.LastIndexOf(" ");
                                                        nome2 = nome2.Substring(0, n2);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            nome2 = x.CompleteName.Substring(81, x.CompleteName.Length);
                                            if (nome2.Length > 50)
                                            {
                                                nome2 = nome2.Substring(0, 50);
                                                resto = nome2.Substring(50, nome2.Length);
                                                if (nome2[nome2.Length - 1] != ' ')
                                                {
                                                    if (resto[0] != ' ')
                                                    {
                                                        n2 = nome2.LastIndexOf(" ");
                                                        nome2 = nome2.Substring(0, n2);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        nome1 = x.CompleteName.Substring(0, 79);
                                        nome2 = x.CompleteName.Substring(80, x.CompleteName.Length);
                                        if (nome2.Length > 50)
                                        {
                                            nome2 = nome2.Substring(0, 50);
                                            resto = nome2.Substring(50, nome2.Length);
                                            if (nome2[nome2.Length - 1] != ' ')
                                            {
                                                if (resto[0] != ' ')
                                                {
                                                    n2 = nome2.LastIndexOf(" ");
                                                    nome2 = nome2.Substring(0, n2);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    nome1 = x.CompleteName;
                                    nome2 = "";
                                }
                                newdp.Nome = nome1;
                                newdp.Nome2 = nome2;
                            }
                            newdp.DataHoraCriação = DateTime.Now;
                            newdp.UtilizadorCriação = User.Identity.Name;
                            DBPriceServiceClient.Create(newdp);
                        }
                    });
                }
                else
                {
                    dp.ForEach(x =>
                    {
                        PreçosServiçosCliente newdp = DBPriceServiceClient.ParseToDatabase(x);
                        string nome1 = "", nome2 = "", resto = "";
                        int n = 0, n2 = 0;
                        if (x.CompleteName != null && x.CompleteName.Length > 0)
                        {
                            if (x.CompleteName[x.CompleteName.Length - 1] == ' ')
                            {
                                x.CompleteName = x.CompleteName.Substring(0, x.CompleteName.Length - 1);
                            }
                            if (x.CompleteName.Length > 80)
                            {
                                nome1 = x.CompleteName.Substring(0, 80);
                                nome2 = x.CompleteName.Substring(80, x.CompleteName.Length);
                                if (nome1[nome1.Length - 1] != ' ')
                                {
                                    if (nome2[0] != ' ')
                                    {
                                        n = nome1.LastIndexOf(" ");
                                        nome1 = x.CompleteName.Substring(0, n);
                                        nome2 = x.CompleteName.Substring(n + 1, x.CompleteName.Length);
                                        if (nome2.Length > 50)
                                        {
                                            nome2 = nome2.Substring(0, 50);
                                            resto = nome2.Substring(50, nome2.Length);
                                            if (nome2[nome2.Length - 1] != ' ')
                                            {
                                                if (resto[0] != ' ')
                                                {
                                                    n2 = nome2.LastIndexOf(" ");
                                                    nome2 = nome2.Substring(0, n2);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        nome2 = x.CompleteName.Substring(81, x.CompleteName.Length);
                                        if (nome2.Length > 50)
                                        {
                                            nome2 = nome2.Substring(0, 50);
                                            resto = nome2.Substring(50, nome2.Length);
                                            if (nome2[nome2.Length - 1] != ' ')
                                            {
                                                if (resto[0] != ' ')
                                                {
                                                    n2 = nome2.LastIndexOf(" ");
                                                    nome2 = nome2.Substring(0, n2);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    nome1 = x.CompleteName.Substring(0, 79);
                                    nome2 = x.CompleteName.Substring(80, x.CompleteName.Length);
                                    if (nome2.Length > 50)
                                    {
                                        nome2 = nome2.Substring(0, 50);
                                        resto = nome2.Substring(50, nome2.Length);
                                        if (nome2[nome2.Length - 1] != ' ')
                                        {
                                            if (resto[0] != ' ')
                                            {
                                                n2 = nome2.LastIndexOf(" ");
                                                nome2 = nome2.Substring(0, n2);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                nome1 = x.CompleteName;
                                nome2 = "";
                            }
                            newdp.Nome = nome1;
                            newdp.Nome2 = nome2;
                        }
                        newdp.DataHoraCriação = DateTime.Now;
                        newdp.UtilizadorCriação = User.Identity.Name;
                        DBPriceServiceClient.Create(newdp);
                    });
                }

            }
            else
            {
                responde.eReasonCode = 2;
                responde.eMessage = "Ocorreu um erro ao atualizar";
            }
            return Json(responde);
        }
        #region Export Excel
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel([FromBody] List<PriceServiceClientViewModel> dp)
        {
            string sWebRootFolder = _generalConfig.FileUploadFolder + "Projetos\\" + "tmp\\";
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
                ISheet excelSheet = workbook.CreateSheet("Preços Serviços Cliente");
                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("Cliente");
                row.CreateCell(1).SetCellValue("Nome");
                row.CreateCell(2).SetCellValue("Cod. Serv. Cliente");
                row.CreateCell(3).SetCellValue("Descrição Serviço");
                row.CreateCell(4).SetCellValue("Preço Venda");
                row.CreateCell(5).SetCellValue("Preço de Custo");
                row.CreateCell(6).SetCellValue("Data");
                row.CreateCell(7).SetCellValue("Recurso");
                row.CreateCell(8).SetCellValue("Descrição do Recurso");
                row.CreateCell(9).SetCellValue("Unidade Medida");
                row.CreateCell(10).SetCellValue("Tipo Refeição");
                row.CreateCell(11).SetCellValue("Descrição Tipo Refeição");
                row.CreateCell(12).SetCellValue("Codigo Região");
                row.CreateCell(13).SetCellValue("Codigo Area");
                row.CreateCell(14).SetCellValue("Codigo Centro Responsabilidade");
                row.CreateCell(15).SetCellValue("Quantidade");

                if (dp != null)
                {
                    int count = 1;
                    foreach (PriceServiceClientViewModel item in dp)
                    {
                        row = excelSheet.CreateRow(count);
                        row.CreateCell(0).SetCellValue(item.Client);
                        row.CreateCell(1).SetCellValue(item.CompleteName);
                        row.CreateCell(2).SetCellValue(item.CodServClient);
                        row.CreateCell(3).SetCellValue(item.ServiceDescription);
                        row.CreateCell(4).SetCellValue(item.SalePrice.HasValue ? item.SalePrice.ToString() : "");
                        row.CreateCell(5).SetCellValue(item.PriceCost.HasValue ? item.PriceCost.ToString() : "");
                        row.CreateCell(6).SetCellValue(item.Date);
                        row.CreateCell(7).SetCellValue(item.Resource);
                        row.CreateCell(8).SetCellValue(item.ResourceDescription);
                        row.CreateCell(9).SetCellValue(item.UnitMeasure);
                        row.CreateCell(10).SetCellValue(item.TypeMeal);
                        row.CreateCell(11).SetCellValue(item.TypeMealDescription);
                        row.CreateCell(12).SetCellValue(item.RegionCode);
                        row.CreateCell(13).SetCellValue(item.FunctionalAreaCode);
                        row.CreateCell(14).SetCellValue(item.ResponsabilityCenterCode);
                        row.CreateCell(15).SetCellValue(item.Quantidade.HasValue ? item.Quantidade.ToString() : "");
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
        public IActionResult ExportToExcelDownload(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Projetos\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Preços Serviços Cliente.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
        #endregion
        #region Upload Excel
        [HttpPost]
        public JsonResult OnPostImport()
        {
            var files = Request.Form.Files;
            List<PriceServiceClientViewModel> ListToCreate = DBPriceServiceClient.ParseToViewModel(DBPriceServiceClient.GetAll());
            PriceServiceClientViewModel nrow = new PriceServiceClientViewModel();
            for (int i = 0; i < files.Count; i++)
            {
                IFormFile file = files[i];
                string folderName = "Upload";
                string webRootPath = _generalConfig.FileUploadFolder + "MercadoLocal\\" + "tmp\\";
                string newPath = Path.Combine(webRootPath, folderName);
                StringBuilder sb = new StringBuilder();
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                    ISheet sheet;
                    string fullPath = Path.Combine(newPath, file.FileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        stream.Position = 0;
                        if (sFileExtension == ".xls")
                        {
                            HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                        }
                        else
                        {
                            XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                        }
                        for (int j = (sheet.FirstRowNum + 1); j <= sheet.LastRowNum; j++)
                        {
                            IRow row = sheet.GetRow(j);
                            if (row != null)
                            {
                                nrow = new PriceServiceClientViewModel();
                                nrow.Client = row.GetCell(0) == null ? "" : row.GetCell(0).ToString();
                                nrow.CompleteName = row.GetCell(1) == null ? "" : row.GetCell(1).ToString();
                                nrow.CodServClient = row.GetCell(2) == null ? "" : row.GetCell(2).ToString();
                                nrow.ServiceDescription = row.GetCell(3) == null ? "" : row.GetCell(3).ToString();
                                nrow.strSalePrice = row.GetCell(4) == null ? "" : row.GetCell(4).ToString();
                                nrow.strPriceCost = row.GetCell(5) == null ? "" : row.GetCell(5).ToString();
                                nrow.Date = row.GetCell(6) == null ? "" : row.GetCell(6).ToString();
                                nrow.Resource = row.GetCell(7) == null ? "" : row.GetCell(7).ToString();
                                nrow.ResourceDescription = row.GetCell(8) == null ? "" : row.GetCell(8).ToString();
                                nrow.UnitMeasure = row.GetCell(9) == null ? "" : row.GetCell(9).ToString();
                                nrow.TypeMeal = row.GetCell(10) == null ? "" : row.GetCell(10).ToString();
                                nrow.TypeMealDescription = row.GetCell(11) == null ? "" : row.GetCell(11).ToString();
                                nrow.RegionCode = row.GetCell(12) == null ? "" : row.GetCell(12).ToString();
                                nrow.FunctionalAreaCode = row.GetCell(13) == null ? "" : row.GetCell(13).ToString();
                                nrow.ResponsabilityCenterCode = row.GetCell(14) == null ? "" : row.GetCell(14).ToString();
                                nrow.strQuantidade = row.GetCell(15) == null ? "" : row.GetCell(15).ToString();
                                ListToCreate.Add(nrow);
                            }
                        }
                    }
                }
                if (ListToCreate.Count > 0)
                {
                    foreach (PriceServiceClientViewModel item in ListToCreate)
                    {
                        if (!string.IsNullOrEmpty(item.strPriceCost))
                        {
                            item.PriceCost = Convert.ToDecimal(item.strPriceCost);
                            item.strPriceCost = "";
                        }
                        if (!string.IsNullOrEmpty(item.strSalePrice))
                        {
                            item.SalePrice = Convert.ToDecimal(item.strSalePrice);
                            item.strSalePrice = "";
                        }
                        if (!string.IsNullOrEmpty(item.strQuantidade))
                        {
                            item.Quantidade = Convert.ToDecimal(item.strQuantidade);
                            item.strQuantidade = "";
                        }
                    }
                }
            }
            return Json(ListToCreate);
        }
        #endregion
        #endregion

        #region Movimentos_Lista

        [HttpPost]
        public JsonResult GetListMovimentos([FromBody] JObject requestParams)
        {
            List<ProjectMovementViewModel> result = new List<ProjectMovementViewModel>();
            List<MovimentosDeProjeto> AllMovFilter = new List<MovimentosDeProjeto>();
            try
            {
                string filtroArea = (string)requestParams.GetValue("area");
                string filtroCliente = (string)requestParams.GetValue("cliente");
                int filtroTipoMovimento = (string)requestParams.GetValue("tipomovimento") != "" ? Convert.ToInt32(requestParams.GetValue("tipomovimento")) : 0;
                DateTime filtroDataInicio = (string)requestParams.GetValue("datainicio") != "" ? Convert.ToDateTime(requestParams.GetValue("datainicio")).Date : DateTime.MinValue.Date;
                DateTime filtroDataFim = (string)requestParams.GetValue("datafim") != "" ? Convert.ToDateTime(requestParams.GetValue("datafim")).Date : DateTime.MaxValue.Date;

                AllMovFilter = DBProjectMovements.GetAllByArea(filtroArea);

                //Apply User Dimensions Validations
                List<AcessosDimensões> CUserDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
                //Regions
                if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.Region).Count() > 0)
                    AllMovFilter.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.Region && y.ValorDimensão == x.CódigoRegião));
                //FunctionalAreas
                if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.FunctionalArea).Count() > 0)
                    AllMovFilter.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.FunctionalArea && y.ValorDimensão == x.CódigoÁreaFuncional));
                //ResponsabilityCenter
                if (CUserDimensions.Where(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                    AllMovFilter.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == (int)Dimensions.ResponsabilityCenter && y.ValorDimensão == x.CódigoCentroResponsabilidade));

                if (!string.IsNullOrEmpty(filtroCliente))
                    AllMovFilter.RemoveAll(x => x.FaturaANºCliente != filtroCliente);

                if (filtroTipoMovimento > 0)
                    AllMovFilter.RemoveAll(x => x.TipoMovimento != filtroTipoMovimento);

                if (filtroDataInicio > DateTime.MinValue)
                    AllMovFilter.RemoveAll(x => Convert.ToDateTime(x.Data).Date < filtroDataInicio);

                if (filtroDataFim > DateTime.MinValue)
                    AllMovFilter.RemoveAll(x => Convert.ToDateTime(x.Data).Date > filtroDataFim);

                if (AllMovFilter.Count <= 15000)
                {
                    result = AllMovFilter.ParseToViewModelMovimentList(_config.NAVDatabaseName, _config.NAVCompanyName);

                    List<EnumData> AllMovementType = EnumerablesFixed.ProjectDiaryMovements;
                    List<EnumData> AllType = EnumerablesFixed.ProjectDiaryTypes;
                    List<Projetos> AllProjects = DBProjects.GetAll();
                    List<NAVClientsViewModel> AllClients = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "");
                    List<ClientServicesViewModel> AllServicosCliente = DBClientServices.GetAllClientService("", true);
                    Projetos Project = new Projetos();

                    result.ForEach(x =>
                    {
                        Project = null;
                        Project = AllProjects.Where(y => y.NºProjeto == x.ProjectNo).FirstOrDefault();
                        if (Project != null)
                        {
                            x.ProjectDescription = !string.IsNullOrEmpty(Project.Descrição) ? Project.Descrição : "";
                            x.ContractNo = !string.IsNullOrEmpty(Project.NºContrato) ? Project.NºContrato : "";
                        }
                        else
                        {
                            x.ProjectDescription = "";
                            x.ContractNo = "";
                        }

                        //x.ProjectDescription = AllProjects.Where(y => y.NºProjeto == x.ProjectNo).FirstOrDefault() != null ? AllProjects.Where(y => y.NºProjeto == x.ProjectNo).FirstOrDefault().Descrição : "";
                        //x.ContractNo = AllProjects.Where(y => y.NºProjeto == x.ProjectNo).FirstOrDefault() != null ? AllProjects.Where(y => y.NºProjeto == x.ProjectNo).FirstOrDefault().NºContrato : "";
                        x.MovementTypeText = x.MovementType != null ? AllMovementType.Where(y => y.Id == x.MovementType).FirstOrDefault() != null ? AllMovementType.Where(y => y.Id == x.MovementType).FirstOrDefault().Value : "" : "";
                        x.TypeText = x.Type != null ? AllType.Where(y => y.Id == x.Type).FirstOrDefault() != null ? AllType.Where(y => y.Id == x.Type).FirstOrDefault().Value : "" : "";
                        x.BillableText = x.Billable.HasValue ? x.Billable == true ? "Sim" : "Não" : "";
                        x.ClientName = AllClients.Where(y => y.No_ == x.ClientRequest).FirstOrDefault() != null ? AllClients.Where(y => y.No_ == x.ClientRequest).FirstOrDefault().Name : "";
                        x.ServiceClientDescription = AllServicosCliente.Where(y => y.ServiceCode == x.ServiceClientCode).FirstOrDefault() != null ? AllServicosCliente.Where(y => y.ServiceCode == x.ServiceClientCode).FirstOrDefault().ServiceDescription : "";
                    });
                }
                else
                {
                    return Json(null);
                }
            }
            catch (Exception ex)
            {
                return Json(null);
            }

            return Json(result.OrderBy(x => x.ProjectNo).ThenBy(x => x.Date));
        }






        #endregion

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_Projetos([FromBody] List<ProjectListItemViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "Projetos\\" + "tmp\\";
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
                ISheet excelSheet = workbook.CreateSheet("Projetos");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["projectNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Projeto");
                    Col = Col + 1;
                }
                if (dp["dateText"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data");
                    Col = Col + 1;
                }
                if (dp["statusDescription"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Estado");
                    Col = Col + 1;
                }
                if (dp["description"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Descrição");
                    Col = Col + 1;
                }
                if (dp["clientNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Cliente");
                    Col = Col + 1;
                }
                if (dp["clientName"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nome Cliente");
                    Col = Col + 1;
                }
                if (dp["clientRegionCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cliente Código Região");
                    Col = Col + 1;
                }
                if (dp["regionCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Código Região");
                    Col = Col + 1;
                }
                if (dp["functionalAreaCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Área Funcional");
                    Col = Col + 1;
                }
                if (dp["responsabilityCenterCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Centro Responsabilidade");
                    Col = Col + 1;
                }
                if (dp["contractoNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Contrato");
                    Col = Col + 1;
                }
                if (dp["projectTypeCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Tipo Projeto");
                    Col = Col + 1;
                }
                if (dp["projectTypeDescription"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Tipo Projeto");
                    Col = Col + 1;
                }
                if (dp["movimentosVenda"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Movimentos Venda");
                    Col = Col + 1;
                }
                if (dp["descricaoDetalhada"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Descrição Detalhada");
                    Col = Col + 1;
                }

                if (dp != null)
                {
                    int count = 1;
                    foreach (ProjectListItemViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["projectNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ProjectNo);
                            Col = Col + 1;
                        }
                        if (dp["dateText"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.DateText);
                            Col = Col + 1;
                        }
                        if (dp["statusDescription"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.StatusDescription);
                            Col = Col + 1;
                        }
                        if (dp["description"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Description);
                            Col = Col + 1;
                        }
                        if (dp["clientNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ClientNo);
                            Col = Col + 1;
                        }
                        if (dp["clientName"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ClientName);
                            Col = Col + 1;
                        }
                        if (dp["clientRegionCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ClientRegionCode);
                            Col = Col + 1;
                        }
                        if (dp["regionCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RegionCode);
                            Col = Col + 1;
                        }
                        if (dp["functionalAreaCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.FunctionalAreaCode);
                            Col = Col + 1;
                        }
                        if (dp["responsabilityCenterCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ResponsabilityCenterCode);
                            Col = Col + 1;
                        }
                        if (dp["contractoNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ContractoNo);
                            Col = Col + 1;
                        }
                        if (dp["projectTypeCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ProjectTypeCode.ToString());
                            Col = Col + 1;
                        }
                        if (dp["projectTypeDescription"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ProjectTypeDescription);
                            Col = Col + 1;
                        }
                        if (dp["movimentosVenda"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.MovimentosVenda);
                            Col = Col + 1;
                        }
                        if (dp["descricaoDetalhada"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.DescricaoDetalhada);
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
        public IActionResult ExportToExcelDownload_Projetos(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Projetos\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Projetos.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_AutorizacaoFaturacao([FromBody] List<ProjectMovementViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "Projetos\\" + "tmp\\";
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
                ISheet excelSheet = workbook.CreateSheet("Autorização de Faturação");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["projectNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Projeto");
                    Col = Col + 1;
                }
                if (dp["date"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data");
                    Col = Col + 1;
                }
                if (dp["movementType"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Tipo Movimento");
                    Col = Col + 1;
                }
                if (dp["type"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Tipo");
                    Col = Col + 1;
                }
                if (dp["code"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Código");
                    Col = Col + 1;
                }
                if (dp["description"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Descrição");
                    Col = Col + 1;
                }
                if (dp["measurementUnitCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Unidade Medida");
                    Col = Col + 1;
                }
                if (dp["quantity"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Quantidade");
                    Col = Col + 1;
                }
                if (dp["unitCost"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Custo Unitário");
                    Col = Col + 1;
                }
                if (dp["totalCost"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Custo Total");
                    Col = Col + 1;
                }
                if (dp["unitPrice"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Preço Unitário");
                    Col = Col + 1;
                }
                if (dp["totalPrice"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Preço Total");
                    Col = Col + 1;
                }
                if (dp["invoiceToClientNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Fatura-a Nº Cliente");
                    Col = Col + 1;
                }
                if (dp["clientName"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nome Cliente");
                    Col = Col + 1;
                }
                if (dp["locationCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Localização");
                    Col = Col + 1;
                }
                if (dp["regionCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Código Região");
                    Col = Col + 1;
                }
                if (dp["functionalAreaCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Código Área");
                    Col = Col + 1;
                }
                if (dp["responsabilityCenterCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Código Centro Responsabilidade");
                    Col = Col + 1;
                }
                if (dp["commitmentNumber"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Compromisso");
                    Col = Col + 1;
                }
                if (dp["serviceGroupCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Grupo Serviço");
                    Col = Col + 1;
                }
                if (dp["createDateText"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data Criação");
                    Col = Col + 1;
                }
                if (dp["createHourText"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Hora Criação");
                    Col = Col + 1;
                }

                if (dp != null)
                {
                    int count = 1;
                    foreach (ProjectMovementViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["projectNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ProjectNo);
                            Col = Col + 1;
                        }
                        if (dp["date"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Date);
                            Col = Col + 1;
                        }
                        if (dp["movementType"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.MovementType.ToString());
                            Col = Col + 1;
                        }
                        if (dp["type"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Type.ToString());
                            Col = Col + 1;
                        }
                        if (dp["code"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Code);
                            Col = Col + 1;
                        }
                        if (dp["description"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Description);
                            Col = Col + 1;
                        }
                        if (dp["measurementUnitCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.MeasurementUnitCode);
                            Col = Col + 1;
                        }
                        if (dp["quantity"]["hidden"].ToString() == "False")
                        {
                            var quantityCell = row.CreateCell(Col);
                            quantityCell.SetCellType(CellType.Numeric);
                            quantityCell.SetCellValue((double)(item.Quantity.HasValue ? Math.Round((decimal)item.Quantity, 2, MidpointRounding.AwayFromZero) : decimal.Zero));

                            Col = Col + 1;
                        }
                        if (dp["unitCost"]["hidden"].ToString() == "False")
                        {
                            var unitCostCell = row.CreateCell(Col);
                            unitCostCell.SetCellType(CellType.Numeric);
                            unitCostCell.SetCellValue((double)(item.UnitCost.HasValue ? Math.Round((decimal)item.UnitCost, 4, MidpointRounding.AwayFromZero) : decimal.Zero));

                            Col = Col + 1;
                        }
                        if (dp["totalCost"]["hidden"].ToString() == "False")
                        {
                            var totalCostCell = row.CreateCell(Col);
                            totalCostCell.SetCellType(CellType.Numeric);
                            totalCostCell.SetCellValue((double)(item.Quantity.HasValue && item.UnitCost.HasValue ? Math.Round((decimal)(item.Quantity * item.UnitCost), 2, MidpointRounding.AwayFromZero) : decimal.Zero));

                            Col = Col + 1;
                        }
                        if (dp["unitPrice"]["hidden"].ToString() == "False")
                        {
                            var unitPriceCell = row.CreateCell(Col);
                            unitPriceCell.SetCellType(CellType.Numeric);
                            unitPriceCell.SetCellValue((double)(item.UnitPrice.HasValue ? Math.Round((decimal)item.UnitPrice, 4, MidpointRounding.AwayFromZero) : decimal.Zero));

                            Col = Col + 1;
                        }
                        if (dp["totalPrice"]["hidden"].ToString() == "False")
                        {
                            var totalPriceCell = row.CreateCell(Col);
                            totalPriceCell.SetCellType(CellType.Numeric);
                            totalPriceCell.SetCellValue((double)(item.Quantity.HasValue && item.UnitPrice.HasValue ? Math.Round((decimal)(item.Quantity * item.UnitPrice), 2, MidpointRounding.AwayFromZero) : decimal.Zero));

                            Col = Col + 1;
                        }
                        if (dp["invoiceToClientNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.InvoiceToClientNo);
                            Col = Col + 1;
                        }
                        if (dp["clientName"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ClientName);
                            Col = Col + 1;
                        }
                        if (dp["locationCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.LocationCode);
                            Col = Col + 1;
                        }
                        if (dp["regionCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RegionCode);
                            Col = Col + 1;
                        }
                        if (dp["functionalAreaCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.FunctionalAreaCode);
                            Col = Col + 1;
                        }
                        if (dp["responsabilityCenterCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ResponsabilityCenterCode);
                            Col = Col + 1;
                        }
                        if (dp["commitmentNumber"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CommitmentNumber);
                            Col = Col + 1;
                        }
                        if (dp["serviceGroupCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ServiceGroupCode);
                            Col = Col + 1;
                        }
                        if (dp["createDateText"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CreateDateText);
                            Col = Col + 1;
                        }
                        if (dp["createHourText"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CreateHourText);
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
        public IActionResult ExportToExcelDownload_AutorizacaoFaturacao(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Projetos\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Autorização de Faturação.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_FaturacaoProjetos([FromBody] List<AuthorizedProjectViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "Projetos\\" + "tmp\\";
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
                ISheet excelSheet = workbook.CreateSheet("Faturação de Projetos");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["codProjeto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Projeto"); Col = Col + 1; }
                if (dp["nomeCliente"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nome Cliente"); Col = Col + 1; }
                if (dp["codCliente"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Cliente"); Col = Col + 1; }
                if (dp["valorAutorizado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Valor Autorizado"); Col = Col + 1; }
                //if (dp["valorPorFaturar"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Valor por Faturar"); Col = Col + 1; }
                if (dp["codRegiao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Código Região"); Col = Col + 1; }
                if (dp["codAreaFuncional"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Código Área Funcional"); Col = Col + 1; }
                if (dp["codCentroResponsabilidade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Código Centro Responsabilidade"); Col = Col + 1; }
                if (dp["codTermosPagamento"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Termos de Pagamento"); Col = Col + 1; }
                if (dp["dataServPrestado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data de Serviço Prestado"); Col = Col + 1; }
                //if (dp["dataPrestacaoServico"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Início Serv. Prestado"); Col = Col + 1; }
                //if (dp["dataPrestacaoServicoFim"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Fim Serv. Prestado"); Col = Col + 1; }
                if (dp["numCompromisso"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Núm. Compromisso"); Col = Col + 1; }
                if (dp["pedidoCliente"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Pedido Cliente"); Col = Col + 1; }
                if (dp["dataPedido"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Pedido"); Col = Col + 1; }
                if (dp["grupoFactura"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Grupo Factura"); Col = Col + 1; }
                if (dp["descricaoGrupo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Descrição Grupo"); Col = Col + 1; }
                if (dp["situacoesPendentes"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Situações Pendentes"); Col = Col + 1; }
                if (dp["dataAutorizacao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Autorização"); Col = Col + 1; }
                if (dp["utilizador"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Autorizado Por"); Col = Col + 1; }
                if (dp["observacoes"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Observações"); Col = Col + 1; }

                if (dp != null)
                {
                    int count = 1;
                    foreach (AuthorizedProjectViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["codProjeto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodProjeto); Col = Col + 1; }
                        if (dp["nomeCliente"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NomeCliente); Col = Col + 1; }
                        if (dp["codCliente"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodCliente); Col = Col + 1; }
                        if (dp["valorAutorizado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue((double)item.ValorAutorizado); Col = Col + 1; }
                        /*ToDo*/
                        //if (dp["valorPorFaturar"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(""); Col = Col + 1; }
                        if (dp["codRegiao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodRegiao); Col = Col + 1; }
                        if (dp["codAreaFuncional"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodAreaFuncional); Col = Col + 1; }
                        if (dp["codCentroResponsabilidade"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodCentroResponsabilidade); Col = Col + 1; }
                        if (dp["codTermosPagamento"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodTermosPagamento); Col = Col + 1; }
                        if (dp["dataServPrestado"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DataServPrestado); Col = Col + 1; }
                        //if (dp["dataPrestacaoServico"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DataPrestacaoServico); Col = Col + 1; }
                        //if (dp["dataPrestacaoServicoFim"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DataPrestacaoServicoFim); Col = Col + 1; }
                        if (dp["numCompromisso"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.NumCompromisso); Col = Col + 1; }
                        if (dp["pedidoCliente"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.PedidoCliente); Col = Col + 1; }
                        if (dp["dataPedido"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DataPedido); Col = Col + 1; }
                        if (dp["grupoFactura"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.GrupoFactura); Col = Col + 1; }
                        if (dp["descricaoGrupo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DescricaoGrupo); Col = Col + 1; }
                        if (dp["situacoesPendentes"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.SituacoesPendentes); Col = Col + 1; }
                        if (dp["dataAutorizacao"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DataAutorizacao); Col = Col + 1; }
                        if (dp["utilizador"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Utilizador); Col = Col + 1; }
                        if (dp["observacoes"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Observacoes); Col = Col + 1; }

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
        public IActionResult ExportToExcelDownload_FaturacaoProjetos(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Projetos\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Faturação de Projetos.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_DetalhesAutorizacao([FromBody] List<MovementAuthorizedProjectViewModel> Lista)
        {
            string sWebRootFolder = _generalConfig.FileUploadFolder + "Projetos\\" + "tmp\\";
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
                ISheet excelSheet = workbook.CreateSheet("Faturação de Projetos Detalhes");
                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("Data");
                //row.CreateCell(1).SetCellValue("Tipo Movimento");
                row.CreateCell(1).SetCellValue("Tipo");
                row.CreateCell(2).SetCellValue("Código");
                row.CreateCell(3).SetCellValue("Descrição");
                row.CreateCell(4).SetCellValue("Quantidade");
                row.CreateCell(5).SetCellValue("Cód. Unidade Medida");
                row.CreateCell(6).SetCellValue("Preço Unitário");
                row.CreateCell(7).SetCellValue("Preço Total");
                //row.CreateCell(9).SetCellValue("Faturável");
                row.CreateCell(8).SetCellValue("Tipo de Recurso");
                row.CreateCell(9).SetCellValue("Código Serviço");
                row.CreateCell(10).SetCellValue("Descrição Serviço");
                row.CreateCell(11).SetCellValue("Grupo Serviços");
                row.CreateCell(12).SetCellValue("Nº Guia Externa");
                row.CreateCell(13).SetCellValue("Data Consumo Guia");
                row.CreateCell(14).SetCellValue("Nº Guia de Residuos");
                //row.CreateCell(17).SetCellValue("Nº Guia Corrigida");
                //row.CreateCell(18).SetCellValue("Data Consumo Guia Corrigida");
                //row.CreateCell(19).SetCellValue("Destino Final Residuos");
                row.CreateCell(15).SetCellValue("Tipo Refeição");
                row.CreateCell(16).SetCellValue("Nº Documento");
                //row.CreateCell(22).SetCellValue("Localização");
                row.CreateCell(17).SetCellValue("Custo Unitário");
                row.CreateCell(18).SetCellValue("Custo Total");
                row.CreateCell(19).SetCellValue("Região");
                row.CreateCell(20).SetCellValue("Área Funcional");
                row.CreateCell(21).SetCellValue("Centro Responsabilidade");
                row.CreateCell(22).SetCellValue("Nº Projeto");
                row.CreateCell(23).SetCellValue("Nº Movimento");
                row.CreateCell(24).SetCellValue("Fatura-a Nº Cliente");
                row.CreateCell(25).SetCellValue("Nome Cliente");

                int count = 1;
                foreach (MovementAuthorizedProjectViewModel item in Lista)
                {
                    row = excelSheet.CreateRow(count);

                    row.CreateCell(0).SetCellValue(item.DateTexto);
                    //row.CreateCell(1).SetCellValue(item.MovementType.ToString());
                    row.CreateCell(1).SetCellValue(item.Type.ToString());
                    row.CreateCell(2).SetCellValue(item.Code);
                    row.CreateCell(3).SetCellValue(item.Description);
                    row.CreateCell(4).SetCellValue((double)Math.Round((decimal)item.Quantity, 2, MidpointRounding.AwayFromZero));
                    row.CreateCell(5).SetCellValue(item.UnitCode);
                    row.CreateCell(6).SetCellValue((double)Math.Round((decimal)item.SalesPrice, 4, MidpointRounding.AwayFromZero));
                    row.CreateCell(7).SetCellValue((double)Math.Round((decimal)(item.SalesPrice * item.Quantity), 2, MidpointRounding.AwayFromZero));
                    //row.CreateCell(9).SetCellValue(item.Billable.ToString());
                    row.CreateCell(8).SetCellValue(item.TypeResourse.ToString());
                    row.CreateCell(9).SetCellValue(item.CodServClient);
                    row.CreateCell(10).SetCellValue(item.DescServClient);
                    row.CreateCell(11).SetCellValue(item.CodServiceGroup);
                    row.CreateCell(12).SetCellValue(item.NumGuideExternal);
                    row.CreateCell(13).SetCellValue(item.DateConsumeTexto);
                    row.CreateCell(14).SetCellValue(item.NumGuideResiduesGar);
                    //row.CreateCell(17).SetCellValue(item.AdjustedDocument);
                    //row.CreateCell(18).SetCellValue(item.AdjustedDocumentDate);
                    //row.CreateCell(19).SetCellValue(item.ResidueFinalDestinyCode.ToString());
                    row.CreateCell(15).SetCellValue(item.TypeMeal.ToString());
                    row.CreateCell(16).SetCellValue(item.NumDocument);
                    //row.CreateCell(22).SetCellValue(item.LocationCode);
                    row.CreateCell(17).SetCellValue((double)(item.CostPrice.HasValue ? Math.Round((decimal)item.CostPrice, 4, MidpointRounding.AwayFromZero) : decimal.Zero));
                    row.CreateCell(18).SetCellValue((double)(item.CostPrice.HasValue ? Math.Round((decimal)(item.CostPrice * item.Quantity), 2, MidpointRounding.AwayFromZero) : decimal.Zero));
                    row.CreateCell(19).SetCellValue(item.RegionCode);
                    row.CreateCell(20).SetCellValue(item.FunctionalAreaCode);
                    row.CreateCell(21).SetCellValue(item.ResponsabilityCenterCode);
                    row.CreateCell(22).SetCellValue(item.CodProject);
                    row.CreateCell(23).SetCellValue(item.NoMovement);
                    row.CreateCell(24).SetCellValue(item.CodClient);
                    row.CreateCell(25).SetCellValue(item.ClientName);

                    count++;
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
        public IActionResult ExportToExcelDownload_DetalhesAutorizacao(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Projetos\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Faturação de Projetos Detalhes.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_MovimentosProjetos([FromBody] List<ProjectDiaryViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "Projetos\\" + "tmp\\";
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
                ISheet excelSheet = workbook.CreateSheet("Movimentos de Projetos");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["lineNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Linha");
                    Col = Col + 1;
                }
                if (dp["projectNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Projeto");
                    Col = Col + 1;
                }
                if (dp["date"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data");
                    Col = Col + 1;
                }
                if (dp["movementType"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Tipo Movimento");
                    Col = Col + 1;
                }
                if (dp["documentNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Documento");
                    Col = Col + 1;
                }
                if (dp["type"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Tipo");
                    Col = Col + 1;
                }
                if (dp["code"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Código");
                    Col = Col + 1;
                }
                if (dp["description"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Descrição");
                    Col = Col + 1;
                }
                if (dp["quantity"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Quantidade");
                    Col = Col + 1;
                }
                if (dp["measurementUnitCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Unidade Medida");
                    Col = Col + 1;
                }
                if (dp["locationCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Localização");
                    Col = Col + 1;
                }
                if (dp["projectContabGroup"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Grupo Contab. Projeto");
                    Col = Col + 1;
                }
                if (dp["productGroupCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Grupo de Produto");
                    Col = Col + 1;
                }
                if (dp["regionCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Região");
                    Col = Col + 1;
                }
                if (dp["functionalAreaCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Área Funcional");
                    Col = Col + 1;
                }
                if (dp["responsabilityCenterCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Centro Responsabilidade");
                    Col = Col + 1;
                }
                if (dp["user"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Utilizador");
                    Col = Col + 1;
                }
                if (dp["unitCost"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Custo Unitário");
                    Col = Col + 1;
                }
                if (dp["totalCost"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Custo Total");
                    Col = Col + 1;
                }
                if (dp["unitPrice"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Preço Unitário");
                    Col = Col + 1;
                }
                if (dp["totalPrice"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Preço Total");
                    Col = Col + 1;
                }
                if (dp["billable"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Faturável");
                    Col = Col + 1;
                }
                if (dp["residueGuideNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Guia Resíduos");
                    Col = Col + 1;
                }
                if (dp["externalGuideNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Guia Externa");
                    Col = Col + 1;
                }
                if (dp["invoiceToClientNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Fatura-a Nº Cliente");
                    Col = Col + 1;
                }
                if (dp["clientName"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nome Cliente");
                    Col = Col + 1;
                }
                if (dp["requestNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Requisição");
                    Col = Col + 1;
                }
                if (dp["requestLineNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Linha Requisição");
                    Col = Col + 1;
                }
                if (dp["driver"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Motorista");
                    Col = Col + 1;
                }
                if (dp["mealTypeDescription"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Tipo de Refeição");
                    Col = Col + 1;
                }
                if (dp["residueFinalDestinyCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Destino Final Resíduos");
                    Col = Col + 1;
                }
                if (dp["originalDocument"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Documento Original");
                    Col = Col + 1;
                }
                if (dp["adjustedDocument"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Documento Corrigido");
                    Col = Col + 1;
                }
                if (dp["adjustedPrice"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Acerto De Preços");
                    Col = Col + 1;
                }
                if (dp["adjustedDocumentData"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data Documento Corrigido");
                    Col = Col + 1;
                }
                if (dp["autorizatedInvoice"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Faturação Autorizada");
                    Col = Col + 1;
                }
                if (dp["autorizatedInvoice2"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Faturação Autorizada 2");
                    Col = Col + 1;
                }
                if (dp["autorizatedInvoiceData"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data Autorização Faturação");
                    Col = Col + 1;
                }
                if (dp["serviceGroupCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Grupo Serviço");
                    Col = Col + 1;
                }
                if (dp["serviceGroupCodeDescription"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Grupo Serviço");
                    Col = Col + 1;
                }
                if (dp["resourceType"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Tipo Recurso");
                    Col = Col + 1;
                }
                if (dp["folhaHoras"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Folha Horas");
                    Col = Col + 1;
                }
                if (dp["codigoTipoTrabalho"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Código Tipo Trabalho");
                    Col = Col + 1;
                }
                if (dp["internalRequest"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Requisição Interna");
                    Col = Col + 1;
                }
                if (dp["employeeNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Funcionário");
                    Col = Col + 1;
                }
                if (dp["quantityReturned"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Quantidade Devolvida");
                    Col = Col + 1;
                }
                if (dp["consumptionDate"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data Consumo");
                    Col = Col + 1;
                }
                if (dp["registered"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Registado");
                    Col = Col + 1;
                }
                if (dp["billed"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Faturada");
                    Col = Col + 1;
                }
                if (dp["coin"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Moeda");
                    Col = Col + 1;
                }
                if (dp["unitValueToInvoice"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Valor Unitário A Faturar");
                    Col = Col + 1;
                }
                if (dp["serviceClientCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Serviço Cliente");
                    Col = Col + 1;
                }
                if (dp["clientRequest"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Cliente");
                    Col = Col + 1;
                }
                if (dp["licensePlate"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Matrícula");
                    Col = Col + 1;
                }
                if (dp["readingCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Código Ler");
                    Col = Col + 1;
                }
                if (dp["group"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Grupo");
                    Col = Col + 1;
                }
                if (dp["operation"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Operação");
                    Col = Col + 1;
                }
                if (dp["invoiceGroup"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Grupo Fatura");
                    Col = Col + 1;
                }
                if (dp["invoiceGroupDescription"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Grupo Fatura Descrição");
                    Col = Col + 1;
                }
                if (dp["authorizedBy"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Autorizado Por");
                    Col = Col + 1;
                }
                if (dp["serviceObject"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Objeto Serviço");
                    Col = Col + 1;
                }
                if (dp["fatura"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Fatura");
                    Col = Col + 1;
                }
                if (dp["taxaIVA"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Taxa IVA");
                    Col = Col + 1;
                }
                if (dp["createUser"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Utilizador Criação");
                    Col = Col + 1;
                }
                if (dp["createDateText"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Data Criação");
                    Col = Col + 1;
                }
                if (dp["createHourText"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Hora Criação");
                    Col = Col + 1;
                }

                if (dp != null)
                {
                    int count = 1;
                    foreach (ProjectDiaryViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["lineNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.LineNo);
                            Col = Col + 1;
                        }
                        if (dp["projectNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ProjectNo);
                            Col = Col + 1;
                        }
                        if (dp["date"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Date);
                            Col = Col + 1;
                        }
                        if (dp["movementType"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.MovementTypeText);
                            Col = Col + 1;
                        }
                        if (dp["documentNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.DocumentNo);
                            Col = Col + 1;
                        }
                        if (dp["type"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.TypeText);
                            Col = Col + 1;
                        }
                        if (dp["code"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Code);
                            Col = Col + 1;
                        }
                        if (dp["description"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Description);
                            Col = Col + 1;
                        }
                        if (dp["quantity"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue((double)(item.Quantity.HasValue ? Math.Round((decimal)item.Quantity, 2, MidpointRounding.AwayFromZero) : decimal.Zero));
                            Col = Col + 1;
                        }
                        if (dp["measurementUnitCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.MeasurementUnitCode);
                            Col = Col + 1;
                        }
                        if (dp["locationCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.LocationCode);
                            Col = Col + 1;
                        }
                        if (dp["projectContabGroup"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ProjectContabGroup);
                            Col = Col + 1;
                        }
                        if (dp["productGroupCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ProductGroupCode);
                            Col = Col + 1;
                        }
                        if (dp["regionCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RegionCode);
                            Col = Col + 1;
                        }
                        if (dp["functionalAreaCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.FunctionalAreaCode);
                            Col = Col + 1;
                        }
                        if (dp["responsabilityCenterCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ResponsabilityCenterCode);
                            Col = Col + 1;
                        }
                        if (dp["user"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.User);
                            Col = Col + 1;
                        }
                        if (dp["unitCost"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue((double)(item.UnitCost.HasValue ? Math.Round((decimal)item.UnitCost, 4, MidpointRounding.AwayFromZero) : decimal.Zero));
                            Col = Col + 1;
                        }
                        if (dp["totalCost"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue((double)(item.Quantity.HasValue && item.UnitCost.HasValue ? Math.Round((decimal)(item.Quantity * item.UnitCost), 2, MidpointRounding.AwayFromZero) : decimal.Zero));
                            Col = Col + 1;
                        }
                        if (dp["unitPrice"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue((double)(item.UnitPrice.HasValue ? Math.Round((decimal)item.UnitPrice, 4, MidpointRounding.AwayFromZero) : decimal.Zero));
                            Col = Col + 1;
                        }
                        if (dp["totalPrice"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue((double)(item.Quantity.HasValue && item.UnitPrice.HasValue ? Math.Round((decimal)(item.Quantity * item.UnitPrice), 2, MidpointRounding.AwayFromZero) : decimal.Zero));
                            Col = Col + 1;
                        }
                        if (dp["billable"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.BillableText);
                            Col = Col + 1;
                        }
                        if (dp["residueGuideNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ResidueGuideNo);
                            Col = Col + 1;
                        }
                        if (dp["externalGuideNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ExternalGuideNo);
                            Col = Col + 1;
                        }
                        if (dp["invoiceToClientNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.InvoiceToClientNo);
                            Col = Col + 1;
                        }
                        if (dp["clientName"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ClientName);
                            Col = Col + 1;
                        }
                        if (dp["requestNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RequestNo);
                            Col = Col + 1;
                        }
                        if (dp["requestLineNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RequestLineNo.ToString());
                            Col = Col + 1;
                        }
                        if (dp["driver"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Driver);
                            Col = Col + 1;
                        }
                        if (dp["mealTypeDescription"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.MealTypeDescription);
                            Col = Col + 1;
                        }
                        if (dp["residueFinalDestinyCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ResidueFinalDestinyCode.ToString());
                            Col = Col + 1;
                        }
                        if (dp["originalDocument"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.OriginalDocument);
                            Col = Col + 1;
                        }
                        if (dp["adjustedDocument"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.AdjustedDocument);
                            Col = Col + 1;
                        }
                        if (dp["adjustedPrice"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.AdjustedPriceText);
                            Col = Col + 1;
                        }
                        if (dp["adjustedDocumentData"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.AdjustedDocumentData);
                            Col = Col + 1;
                        }
                        if (dp["autorizatedInvoice"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.AutorizatedInvoiceText);
                            Col = Col + 1;
                        }
                        if (dp["autorizatedInvoice2"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.AutorizatedInvoice2Text);
                            Col = Col + 1;
                        }
                        if (dp["autorizatedInvoiceData"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.AutorizatedInvoiceData);
                            Col = Col + 1;
                        }
                        if (dp["serviceGroupCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ServiceGroupCode);
                            Col = Col + 1;
                        }
                        if (dp["serviceGroupCodeDescription"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ServiceGroupCodeDescription);
                            Col = Col + 1;
                        }
                        if (dp["resourceType"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ResourceType.ToString());
                            Col = Col + 1;
                        }
                        if (dp["folhaHoras"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.FolhaHoras);
                            Col = Col + 1;
                        }
                        if (dp["codigoTipoTrabalho"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CodigoTipoTrabalho);
                            Col = Col + 1;
                        }
                        if (dp["internalRequest"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.InternalRequest);
                            Col = Col + 1;
                        }
                        if (dp["employeeNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.EmployeeNo);
                            Col = Col + 1;
                        }
                        if (dp["quantityReturned"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.QuantityReturned.ToString());
                            Col = Col + 1;
                        }
                        if (dp["consumptionDate"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ConsumptionDate);
                            Col = Col + 1;
                        }
                        if (dp["registered"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RegisteredText);
                            Col = Col + 1;
                        }
                        if (dp["billed"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.BilledText);
                            Col = Col + 1;
                        }
                        if (dp["coin"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Coin);
                            Col = Col + 1;
                        }
                        if (dp["unitValueToInvoice"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue((double)(item.UnitValueToInvoice ?? 0));
                            Col = Col + 1;
                        }
                        if (dp["serviceClientCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ServiceClientCode);
                            Col = Col + 1;
                        }
                        if (dp["clientRequest"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ClientRequest);
                            Col = Col + 1;
                        }
                        if (dp["licensePlate"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.LicensePlate);
                            Col = Col + 1;
                        }
                        if (dp["readingCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ReadingCode);
                            Col = Col + 1;
                        }
                        if (dp["group"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Group);
                            Col = Col + 1;
                        }
                        if (dp["operation"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Operation);
                            Col = Col + 1;
                        }
                        if (dp["invoiceGroup"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.InvoiceGroup.ToString());
                            Col = Col + 1;
                        }
                        if (dp["invoiceGroupDescription"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.InvoiceGroupDescription);
                            Col = Col + 1;
                        }
                        if (dp["authorizedBy"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.AuthorizedBy);
                            Col = Col + 1;
                        }
                        if (dp["serviceObject"]["hidden"].ToString() == "False")
                        {
                            //row.CreateCell(Col).SetCellValue(item.ServiceObject.ToString());
                            Col = Col + 1;
                        }
                        if (dp["fatura"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Fatura);
                            Col = Col + 1;
                        }
                        if (dp["taxaIVA"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.TaxaIVA.ToString());
                            Col = Col + 1;
                        }
                        if (dp["createUser"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CreateUser);
                            Col = Col + 1;
                        }
                        if (dp["createDateText"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CreateDateText);
                            Col = Col + 1;
                        }
                        if (dp["createHourText"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CreateHourText);
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
        public IActionResult ExportToExcelDownload_MovimentosProjetos(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Projetos\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Movimentos de Projetos.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_PreMovimentosProjetos([FromBody] List<ProjectDiaryViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "Projetos\\" + "tmp\\";
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
                ISheet excelSheet = workbook.CreateSheet("Pré-Movimentos de Projetos");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["registered"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Registado");
                    Col = Col + 1;
                }
                if (dp["projectNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Projeto");
                    Col = Col + 1;
                }
                if (dp["date"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Date");
                    Col = Col + 1;
                }
                if (dp["movementType"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Tipo Movimento");
                    Col = Col + 1;
                }
                if (dp["type"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Tipo");
                    Col = Col + 1;
                }
                if (dp["code"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Código");
                    Col = Col + 1;
                }
                if (dp["description"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Descrição");
                    Col = Col + 1;
                }
                if (dp["measurementUnitCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Unidade Medida");
                    Col = Col + 1;
                }
                if (dp["quantity"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Quantidade");
                    Col = Col + 1;
                }
                if (dp["unitPrice"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Preço Unitário");
                    Col = Col + 1;
                }
                if (dp["totalPrice"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Preço total");
                    Col = Col + 1;
                }
                if (dp["unitCost"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Custo Unitário");
                    Col = Col + 1;
                }
                if (dp["totalCost"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Custo Total");
                    Col = Col + 1;
                }
                if (dp["invoiceToClientNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Fatura-a Nº Cliente");
                    Col = Col + 1;
                }
                if (dp["clientName"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nome Cliente");
                    Col = Col + 1;
                }
                if (dp["locationCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Localização");
                    Col = Col + 1;
                }
                //if (dp["serviceObject"]["hidden"].ToString() == "False")
                //{
                //    row.CreateCell(Col).SetCellValue("Objeto Serviço");
                //    Col = Col + 1;
                //}
                if (dp["regionCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Região");
                    Col = Col + 1;
                }
                if (dp["functionalAreaCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód Área Funcional");
                    Col = Col + 1;
                }
                if (dp["responsabilityCenterCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Centro Responsabilidade");
                    Col = Col + 1;
                }
                if (dp["user"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Utilizador");
                    Col = Col + 1;
                }
                if (dp["billable"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Faturável");
                    Col = Col + 1;
                }
                if (dp["folhaHoras"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Folha de Horas");
                    Col = Col + 1;
                }
                if (dp["serviceClientCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Serviço Cliente");
                    Col = Col + 1;
                }
                if (dp["serviceClientDescription"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Serviço Cliente");
                    Col = Col + 1;
                }
                if (dp["mealTypeDescription"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Tipo de Refeição");
                    Col = Col + 1;
                }
                if (dp["serviceGroupCode"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Cód. Grupo Serviço");
                    Col = Col + 1;
                }
                if (dp["serviceGroupCodeDescription"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Grupo Serviço");
                    Col = Col + 1;
                }

                if (dp != null)
                {
                    int count = 1;
                    foreach (ProjectDiaryViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["registered"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Registered == null ? "Não" : item.Registered == false ? "Não" : "Sim");
                            Col = Col + 1;
                        }
                        if (dp["projectNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ProjectNo);
                            Col = Col + 1;
                        }
                        if (dp["date"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Date);
                            Col = Col + 1;
                        }
                        if (dp["movementType"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.MovementTypeText);
                            Col = Col + 1;
                        }
                        if (dp["type"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.TypeText);
                            Col = Col + 1;
                        }
                        if (dp["code"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Code);
                            Col = Col + 1;
                        }
                        if (dp["description"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Description);
                            Col = Col + 1;
                        }
                        if (dp["measurementUnitCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.MeasurementUnitCodeText);
                            Col = Col + 1;
                        }
                        if (dp["quantity"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue((double)(item.Quantity.HasValue ? Math.Round((decimal)item.Quantity, 2, MidpointRounding.AwayFromZero) : decimal.Zero));
                            Col = Col + 1;
                        }
                        if (dp["unitPrice"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue((double)(item.UnitPrice.HasValue ? Math.Round((decimal)item.UnitPrice, 4, MidpointRounding.AwayFromZero) : decimal.Zero));
                            Col = Col + 1;
                        }
                        if (dp["totalPrice"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue((double)(item.Quantity.HasValue && item.UnitPrice.HasValue ? Math.Round((decimal)(item.Quantity * item.UnitPrice), 2, MidpointRounding.AwayFromZero) : decimal.Zero));
                            Col = Col + 1;
                        }
                        if (dp["unitCost"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue((double)(item.UnitCost.HasValue ? Math.Round((decimal)item.UnitCost, 4, MidpointRounding.AwayFromZero) : decimal.Zero));
                            Col = Col + 1;
                        }
                        if (dp["totalCost"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue((double)(item.Quantity.HasValue && item.UnitCost.HasValue ? Math.Round((decimal)(item.Quantity * item.UnitCost), 2, MidpointRounding.AwayFromZero) : decimal.Zero));
                            Col = Col + 1;
                        }
                        if (dp["invoiceToClientNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.InvoiceToClientNo);
                            Col = Col + 1;
                        }
                        if (dp["clientName"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ClientName);
                            Col = Col + 1;
                        }
                        if (dp["locationCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.LocationCodeText);
                            Col = Col + 1;
                        }
                        //if (dp["serviceObject"]["hidden"].ToString() == "False")
                        //{
                        //    row.CreateCell(Col).SetCellValue(item.ServiceObject);
                        //    Col = Col + 1;
                        //}
                        if (dp["regionCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.RegionCodeText);
                            Col = Col + 1;
                        }
                        if (dp["functionalAreaCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.FunctionalAreaCodeText);
                            Col = Col + 1;
                        }
                        if (dp["responsabilityCenterCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ResponsabilityCenterCodeText);
                            Col = Col + 1;
                        }
                        if (dp["user"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.User);
                            Col = Col + 1;
                        }
                        if (dp["billable"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Billable.ToString());
                            Col = Col + 1;
                        }
                        if (dp["folhaHoras"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.FolhaHoras);
                            Col = Col + 1;
                        }
                        if (dp["serviceClientCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ServiceClientCode);
                            Col = Col + 1;
                        }
                        if (dp["serviceClientDescription"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ServiceClientDescription);
                            Col = Col + 1;
                        }
                        if (dp["mealTypeDescription"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.MealTypeDescription);
                            Col = Col + 1;
                        }
                        if (dp["serviceGroupCode"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ServiceGroupCode);
                            Col = Col + 1;
                        }
                        if (dp["serviceGroupCodeDescription"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ServiceGroupCodeDescription);
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
        public IActionResult ExportToExcelDownload_PreMovimentosProjetos(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Projetos\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Pré-Movimentos de Projetos.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_MovimentosList([FromBody] List<ProjectMovementViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "Projetos\\" + "tmp\\";
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
                ISheet excelSheet = workbook.CreateSheet("Autorização de Faturação");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["projectNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Projeto"); Col = Col + 1; }
                if (dp["projectDescription"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Descrição Projeto"); Col = Col + 1; }
                if (dp["invoiceToClientNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Cliente"); Col = Col + 1; }
                if (dp["contractNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Contrato"); Col = Col + 1; }
                if (dp["date"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data"); Col = Col + 1; }
                if (dp["movementTypeText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Tipo Movimento"); Col = Col + 1; }
                if (dp["documentNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Documento"); Col = Col + 1; }
                if (dp["typeText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Tipo"); Col = Col + 1; }
                if (dp["code"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Código"); Col = Col + 1; }
                if (dp["description"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Descrição"); Col = Col + 1; }
                if (dp["quantity"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Quantidade"); Col = Col + 1; }
                if (dp["measurementUnitCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Unidade Medida"); Col = Col + 1; }
                if (dp["unitCost"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Custo Unitário"); Col = Col + 1; }
                if (dp["totalCost"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Custo Total"); Col = Col + 1; }
                if (dp["unitPrice"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Preço Unitário"); Col = Col + 1; }
                if (dp["totalPrice"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Preço Total"); Col = Col + 1; }
                if (dp["regionCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Região"); Col = Col + 1; }
                if (dp["functionalAreaCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Área Funcional"); Col = Col + 1; }
                if (dp["responsabilityCenterCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Centro Responsabilidade"); Col = Col + 1; }
                if (dp["billableText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Faturável"); Col = Col + 1; }
                if (dp["requestNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Requisição"); Col = Col + 1; }
                if (dp["timesheetNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Folha Horas"); Col = Col + 1; }
                if (dp["codigoTipoTrabalho"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Tipo Trabalho"); Col = Col + 1; }
                if (dp["employeeNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Funcionário"); Col = Col + 1; }
                if (dp["clientRequest"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Cliente"); Col = Col + 1; }
                if (dp["clientName"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nome Cliente"); Col = Col + 1; }
                if (dp["serviceClientCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Serviço Cliente"); Col = Col + 1; }
                if (dp["serviceClientDescription"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Serviço Cliente"); Col = Col + 1; }
                if (dp["residueGuideNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Guia Resíduos"); Col = Col + 1; }
                if (dp["readingCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Ler"); Col = Col + 1; }
                if (dp["mealTypeDescription"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Tipo Refeição"); Col = Col + 1; }
                if (dp["serviceGroupCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Cód. Grupo Serviço"); Col = Col + 1; }
                if (dp["internalRequest"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Requisição Interna"); Col = Col + 1; }
                if (dp["consumptionDate"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Consumo"); Col = Col + 1; }
                if (dp["autorizatedInvoiceDate"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Autorização Faturação"); Col = Col + 1; }
                if (dp["authorizedBy"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Autorizado Por"); Col = Col + 1; }
                if (dp["invoiceGroup"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Grupo Fatura"); Col = Col + 1; }
                if (dp["taxaIVA"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Taxa IVA"); Col = Col + 1; }
                if (dp["user"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Utilizador"); Col = Col + 1; }
                if (dp["createUser"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Criador"); Col = Col + 1; }
                if (dp["createDateText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Criação"); Col = Col + 1; }

                if (dp != null)
                {
                    int count = 1;
                    foreach (ProjectMovementViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["projectNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ProjectNo); Col = Col + 1; }
                        if (dp["projectDescription"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ProjectDescription); Col = Col + 1; }
                        if (dp["invoiceToClientNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.InvoiceToClientNo); Col = Col + 1; }
                        if (dp["contractNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ContractNo); Col = Col + 1; }
                        if (dp["date"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Date); Col = Col + 1; }
                        if (dp["movementTypeText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.MovementTypeText); Col = Col + 1; }
                        if (dp["documentNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DocumentNo); Col = Col + 1; }
                        if (dp["typeText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.TypeText); Col = Col + 1; }
                        if (dp["code"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Code); Col = Col + 1; }
                        if (dp["description"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Description); Col = Col + 1; }
                        if (dp["quantity"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Quantity.HasValue ? Math.Round((decimal)item.Quantity, 2, MidpointRounding.AwayFromZero).ToString() : decimal.Zero.ToString()); Col = Col + 1; }
                        if (dp["measurementUnitCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.MeasurementUnitCode); Col = Col + 1; }
                        if (dp["unitCost"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.UnitCost.HasValue ? Math.Round((decimal)item.UnitCost, 4, MidpointRounding.AwayFromZero).ToString() : decimal.Zero.ToString()); Col = Col + 1; }
                        if (dp["totalCost"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Quantity.HasValue && item.UnitCost.HasValue ? Math.Round((decimal)(item.Quantity * item.UnitCost), 2, MidpointRounding.AwayFromZero).ToString() : decimal.Zero.ToString()); Col = Col + 1; }
                        if (dp["unitPrice"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.UnitPrice.HasValue ? Math.Round((decimal)item.UnitPrice, 4, MidpointRounding.AwayFromZero).ToString() : decimal.Zero.ToString()); Col = Col + 1; }
                        if (dp["totalPrice"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Quantity.HasValue && item.UnitPrice.HasValue ? Math.Round((decimal)(item.Quantity * item.UnitPrice), 2, MidpointRounding.AwayFromZero).ToString() : decimal.Zero.ToString()); Col = Col + 1; }
                        if (dp["regionCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.RegionCode); Col = Col + 1; }
                        if (dp["functionalAreaCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.FunctionalAreaCode); Col = Col + 1; }
                        if (dp["responsabilityCenterCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ResponsabilityCenterCode); Col = Col + 1; }
                        if (dp["billableText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.BillableText); Col = Col + 1; }
                        if (dp["requestNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.RequestNo); Col = Col + 1; }
                        if (dp["timesheetNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.TimesheetNo); Col = Col + 1; }
                        if (dp["codigoTipoTrabalho"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CodigoTipoTrabalho); Col = Col + 1; }
                        if (dp["employeeNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.EmployeeNo); Col = Col + 1; }
                        if (dp["clientRequest"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ClientRequest); Col = Col + 1; }
                        if (dp["clientName"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ClientName); Col = Col + 1; }
                        if (dp["serviceClientCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ServiceClientCode); Col = Col + 1; }
                        if (dp["serviceClientDescription"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ServiceClientDescription); Col = Col + 1; }
                        if (dp["residueGuideNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ResidueGuideNo); Col = Col + 1; }
                        if (dp["readingCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ReadingCode); Col = Col + 1; }
                        if (dp["mealTypeDescription"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.MealTypeDescription); Col = Col + 1; }
                        if (dp["serviceGroupCode"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ServiceGroupCode); Col = Col + 1; }
                        if (dp["internalRequest"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.InternalRequest); Col = Col + 1; }
                        if (dp["consumptionDate"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ConsumptionDate); Col = Col + 1; }
                        if (dp["autorizatedInvoiceDate"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.AutorizatedInvoiceDate); Col = Col + 1; }
                        if (dp["authorizedBy"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.AuthorizedBy); Col = Col + 1; }
                        if (dp["invoiceGroup"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.InvoiceGroup.ToString()); Col = Col + 1; }
                        if (dp["taxaIVA"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.TaxaIVA.ToString()); Col = Col + 1; }
                        if (dp["user"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.User); Col = Col + 1; }
                        if (dp["createUser"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CreateUser); Col = Col + 1; }
                        if (dp["createDateText"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.CreateDateText); Col = Col + 1; }

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
        public IActionResult ExportToExcelDownload_MovimentosList(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Projetos\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Autorização de Faturação.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        //1
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<JsonResult> ExportToExcel_FaturasNotasList([FromBody] List<FaturasNotasViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

            string sWebRootFolder = _generalConfig.FileUploadFolder + "Projetos\\" + "tmp\\";
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
                ISheet excelSheet = workbook.CreateSheet("Autorização de Faturação");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["type"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Tipo"); Col = Col + 1; }
                if (dp["documentNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Nº Documento"); Col = Col + 1; }
                if (dp["documentDateTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data"); Col = Col + 1; }
                if (dp["valorSemIVA"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Valor sem IVA"); Col = Col + 1; }
                if (dp["valorComIVA"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Valor com IVA"); Col = Col + 1; }
                if (dp["parcial"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Valor Total"); Col = Col + 1; }

                if (dp != null)
                {
                    int count = 1;
                    foreach (FaturasNotasViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["type"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Type); Col = Col + 1; }
                        if (dp["documentNo"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DocumentNo); Col = Col + 1; }
                        if (dp["documentDateTexto"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DocumentDateTexto); Col = Col + 1; }
                        if (dp["valorSemIVA"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ValorSemIVA.ToString()); Col = Col + 1; }
                        if (dp["valorComIVA"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.ValorComIVA.ToString()); Col = Col + 1; }
                        if (dp["parcial"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.Parcial); Col = Col + 1; }

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
        public IActionResult ExportToExcelDownload_FaturasNotasList(string sFileName)
        {
            sFileName = _generalConfig.FileUploadFolder + "Projetos\\" + "tmp\\" + sFileName;
            //return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Autorização de Faturação.xlsx");
            return new FileStreamResult(new FileStream(sFileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [HttpPost]
        public JsonResult DeletePreMovimento([FromBody] ProjectDiaryViewModel projecto)
        {
            ErrorHandler result = new ErrorHandler();
            try
            {
                if (projecto != null)
                {
                    if (projecto.Registered == false)
                    {
                        if (projecto.LineNo > 0)
                        {
                            PréMovimentosProjeto projetoToDelete = new PréMovimentosProjeto();
                            projetoToDelete = DBPreProjectMovements.GetByLine(projecto.LineNo);

                            if (projetoToDelete != null)
                            {
                                if (DBPreProjectMovements.Delete(projetoToDelete) == true)
                                {
                                    result.eReasonCode = 1;
                                    result.eMessage = "O Pré-Movimento de Projeto foi eliminado com sucesso.";
                                }
                                else
                                {
                                    result.eReasonCode = 2;
                                    result.eMessage = "Ocorreu um erro ao elinar o Pré-Movimento de Projeto.";
                                }
                            }
                            else
                            {
                                result.eReasonCode = 3;
                                result.eMessage = "Não foi possivel obter o Pré-Movimento de Projeto para eliminar.";
                            }
                        }
                        else
                        {
                            result.eReasonCode = 4;
                            result.eMessage = "Não foi possivel obter o Nº da linha do Pré-Movimento de projeto.";
                        }
                    }
                    else
                    {
                        result.eReasonCode = 5;
                        result.eMessage = "Não pode eliminar o Pré-Movimento de Projeto pois o mesmo já está registado.";
                    }
                }
                else
                {
                    result.eReasonCode = 6;
                    result.eMessage = "O Pré-Movimento de Projeto não pode ser vazio.";
                }
            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro eliminar o Pré-Movimento de Projeto.";
                return Json(result);
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateProjectMovements([FromBody] ProjectMovementViewModel data)
        {
            try
            {
                if (data != null)
                {
                    data.Quantity = data.Quantity.HasValue ? Math.Round((decimal)data.Quantity, 2, MidpointRounding.AwayFromZero) : decimal.Zero;
                    data.UnitPrice = data.UnitPrice.HasValue ? Math.Round((decimal)data.UnitPrice, 4, MidpointRounding.AwayFromZero) : decimal.Zero;
                    data.TotalPrice = data.Quantity.HasValue && data.UnitPrice.HasValue ? Math.Round((decimal)(data.Quantity * data.UnitPrice), 2, MidpointRounding.AwayFromZero) : decimal.Zero;
                    data.UpdateUser = User.Identity.Name;
                    if (DBProjectMovements.Update(DBProjectMovements.ParseToDB(data)) != null)
                    {
                        //List<ProjectMovementViewModel> projectMovements = GetProjectMovements(data.ProjectNo, data.InvoiceToClientNo, true).Where(x => x.Selecionada == true).ToList();
                        //if (projectMovements != null && projectMovements.Count > 0)
                        //    data.TotalPriceLinhas = Math.Round(projectMovements.Sum(x => (decimal)x.TotalPrice), 2);
                        //else
                        //    data.TotalPriceLinhas = 0;

                        data.eReasonCode = 1;
                        data.eMessage = "Movimento de projeto atualizado com sucesso.";
                    }
                    else
                    {
                        data.eReasonCode = 2;
                        data.eMessage = "Ocorreu um erro ao atualizar o movimento de projeto.";
                    }
                }
                else
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Os dados a serem atualizados não podem ser nulos.";
                }
            }
            catch (Exception ex)
            {
                data.eReasonCode = 99;
                data.eMessage = "Ocorreu um erro.";
            }

            return Json(data);
        }

        [HttpPost]
        public JsonResult UpdateLinhasFaturacao([FromBody] AuthorizedProjectViewModel data)
        {

            try
            {
                if (data != null)
                {
                    if (DBAuthotizedProjects.Update(DBAuthotizedProjects.ParseToDB(data)) != null)
                    {
                        data.eReasonCode = 1;
                        data.eMessage = "Projeto atualizado com sucesso.";
                    }
                    else
                    {
                        data.eReasonCode = 2;
                        data.eMessage = "Ocorreu um erro ao atualizar o Projeto.";
                    }
                }
                else
                {
                    data.eReasonCode = 3;
                    data.eMessage = "Os dados a serem atualizados não podem ser nulos.";
                }
            }
            catch (Exception ex)
            {
                data.eReasonCode = 99;
                data.eMessage = "Ocorreu um erro.";
            }

            return Json(data);
        }

        [HttpPost]
        public JsonResult ResetProjeto([FromBody] List<AuthorizedProjectViewModel> authProjectMovements)
        {
            ErrorHandler result = new ErrorHandler();
            result.eReasonCode = 99;
            result.eMessage = "Ocorreu um erro.";
            try
            {
                if (authProjectMovements != null && authProjectMovements.Count == 1)
                {
                    foreach (AuthorizedProjectViewModel item in authProjectMovements)
                    {
                        //Read NAV2017 PreInvoice Key
                        Task<WSSuchNav2017.WSgetNumPreRegisto_Result> TReadPreInvoice = WSPreInvoice.GetPreInvoice(item.CodProjeto, item.GrupoFactura, _configws);
                        try
                        {
                            TReadPreInvoice.Wait();
                        }
                        catch (Exception ex)
                        {
                            result.eReasonCode = 5;
                            result.eMessage = "Erro: Não foi possivel obter o Nº da Fatura do NAV2017.";
                            return Json(result);
                        }

                        if (TReadPreInvoice.IsCompletedSuccessfully)
                        {
                            string NoPreInvoice = TReadPreInvoice.Result.return_value.ToString();
                            string TypePreInvoice = "";

                            if (string.IsNullOrEmpty(NoPreInvoice))
                            {
                                result.eReasonCode = 5;
                                result.eMessage = "Não foi possivel obter o Nº da Fatura do NAV2017.";
                                return Json(result);
                            }
                            if (NoPreInvoice == "1")
                            {
                                result.eReasonCode = 5;
                                result.eMessage = "Não pode anular esta autorização. O documento de venda não está disponível no pré-registo.";
                                return Json(result);
                            }
                            if (NoPreInvoice == "2")
                            {
                                result.eReasonCode = 5;
                                result.eMessage = "Não é possível anular a autorização por incluir vários Projetos/OMs.";
                                return Json(result);
                            }
                            if (NoPreInvoice.Length < 5)
                            {
                                result.eReasonCode = 5;
                                result.eMessage = "Não foi possivel obter o Nº da Fatura do NAV2017.";
                                return Json(result);
                            }

                            if (item.ValorAutorizado >= 0)
                                TypePreInvoice = "2";
                            else
                                TypePreInvoice = "3";

                            if (!string.IsNullOrEmpty(NoPreInvoice) && !string.IsNullOrEmpty(TypePreInvoice))
                            {
                                bool wsOK = true;
                                while (wsOK == true)
                                {
                                    //Read NAV PreInvoice Key
                                    Task<WSCreatePreInvoice.Read_Result> TReadNavPreInvoice = WSPreInvoice.GetNavPreInvoice(NoPreInvoice, TypePreInvoice, _configws);
                                    try
                                    {
                                        TReadNavPreInvoice.Wait();
                                    }
                                    catch (Exception ex)
                                    {
                                        result.eReasonCode = 5;
                                        result.eMessage = "Ocorreu um erro ao ler a chave da Fatura do NAV2017.";
                                        return Json(result);
                                    }

                                    if (TReadNavPreInvoice.IsCompletedSuccessfully && !string.IsNullOrEmpty(TReadNavPreInvoice.Result.WSPreInvoice.Key))
                                    {
                                        Task<WSCreatePreInvoice.Delete_Result> TDeleteNavPreInvoice = WSPreInvoice.DeletePreInvoice(TReadNavPreInvoice.Result.WSPreInvoice.Key, _configws);
                                        try
                                        {
                                            TDeleteNavPreInvoice.Wait();

                                            if (!TDeleteNavPreInvoice.IsCompletedSuccessfully)
                                            {
                                                result.eReasonCode = 5;
                                                result.eMessage = "Não é possivel eliminar a Fatura no NAV2017.";
                                                return Json(result);
                                            }
                                            else
                                            {
                                                result.eReasonCode = 1;
                                                result.eMessage = "A Fatura foi eliminada com sucesso do NAV2017.";
                                                return Json(result);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            result.eReasonCode = 5;
                                            result.eMessage = "Ocorreu um erro ao eliminar a Fatura do NAV2017.";
                                            return Json(result);
                                        }
                                    }
                                    else
                                    {
                                        wsOK = false;
                                        //result.eReasonCode = 5;
                                        //result.eMessage = "Não foi possivel obter a chave da fatura do NAV2017.";
                                        //return Json(result);
                                    }
                                }
                            }
                            else
                            {
                                result.eReasonCode = 5;
                                result.eMessage = "Não foi possivel obter o código da fatura do NAV2017.";
                                return Json(result);
                            }
                        }
                        else
                        {
                            result.eReasonCode = 5;
                            result.eMessage = "Erro: Não foi possivel obter o Nº da Fatura do NAV2017.";
                            return Json(result);
                        }
                    }
                }
                else
                {
                    if (authProjectMovements == null)
                    {
                        result.eReasonCode = 5;
                        result.eMessage = "Têm que escolher 1 movimento de faturação.";
                        return Json(result);
                    }

                    if (authProjectMovements.Count != 1)
                    {
                        result.eReasonCode = 5;
                        result.eMessage = "Só pode escolher 1 movimento de faturação.";
                        return Json(result);
                    }
                }
            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro.";
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult UndoProjeto([FromBody] List<AuthorizedProjectViewModel> authProjectMovements)
        {
            ErrorHandler result = new ErrorHandler();
            result.eReasonCode = 99;
            result.eMessage = "Ocorreu um erro.";
            try
            {
                if (authProjectMovements != null && authProjectMovements.Count == 1)
                {
                    foreach (AuthorizedProjectViewModel item in authProjectMovements)
                    {
                        ProjectosAutorizados AuthorizedProject = null;
                        using (SuchDBContext ctx = new SuchDBContext())
                        {
                            AuthorizedProject = ctx.ProjectosAutorizados
                                .Where(x => x.Faturado == true && x.CodProjeto == item.CodProjeto && x.GrupoFactura == item.GrupoFactura)
                                .FirstOrDefault();
                        }

                        if (AuthorizedProject != null)
                        {
                            //Read NAV2017 PreInvoice Key
                            Task<WSSuchNav2017.WSgetNumPreRegisto_Result> TReadPreInvoice = WSPreInvoice.GetPreInvoice(item.CodProjeto, item.GrupoFactura, _configws);
                            try
                            {
                                TReadPreInvoice.Wait();
                            }
                            catch (Exception ex)
                            {
                                result.eReasonCode = 5;
                                result.eMessage = "Erro: Não foi possivel obter o Nº da Fatura do NAV2017.";
                                return Json(result);
                            }

                            if (TReadPreInvoice.IsCompletedSuccessfully)
                            {
                                string NoPreInvoice = TReadPreInvoice.Result.return_value.ToString();
                                string TypePreInvoice = "";

                                if (string.IsNullOrEmpty(NoPreInvoice))
                                {
                                    result.eReasonCode = 5;
                                    result.eMessage = "Não foi possivel obter o Nº da Fatura do NAV2017.";
                                    return Json(result);
                                }
                                if (NoPreInvoice == "1")
                                {
                                    result.eReasonCode = 5;
                                    result.eMessage = "Não pode anular esta autorização. O documento de venda já não está disponível no pré-registo.";
                                    return Json(result);
                                }
                                if (NoPreInvoice == "2")
                                {
                                    result.eReasonCode = 5;
                                    result.eMessage = "Não é possível anular a autorização por incluir vários Projetos/OMs.";
                                    return Json(result);
                                }
                                if (NoPreInvoice.Length < 5)
                                {
                                    result.eReasonCode = 5;
                                    result.eMessage = "Não foi possivel obter o Nº da Fatura do NAV2017.";
                                    return Json(result);
                                }


                                if (item.ValorAutorizado >= 0)
                                    TypePreInvoice = "2";
                                else
                                    TypePreInvoice = "3";

                                if (!string.IsNullOrEmpty(NoPreInvoice) && !string.IsNullOrEmpty(TypePreInvoice))
                                {
                                    //Read NAV PreInvoice Key
                                    Task<WSCreatePreInvoice.Read_Result> TReadNavPreInvoice = WSPreInvoice.GetNavPreInvoice(NoPreInvoice, TypePreInvoice, _configws);
                                    try
                                    {
                                        TReadNavPreInvoice.Wait();
                                    }
                                    catch (Exception ex)
                                    {
                                        result.eReasonCode = 5;
                                        result.eMessage = "Ocorreu um erro ao ler a chave da Fatura do NAV2017.";
                                        return Json(result);
                                    }

                                    if (TReadNavPreInvoice.IsCompletedSuccessfully)
                                    {
                                        //Anular a Autorização
                                        AuthorizedProject.Faturado = false;

                                        if (DBAuthotizedProjects.Update(AuthorizedProject) == null)
                                        {
                                            result.eReasonCode = 5;
                                            result.eMessage = "Não foi possivel anular a Autorização no e-SUCH.";
                                            return Json(result);
                                        }

                                        //Delete Pre Invoice
                                        Task<WSCreatePreInvoice.Delete_Result> TDeleteNavPreInvoice = WSPreInvoice.DeletePreInvoice(TReadNavPreInvoice.Result.WSPreInvoice.Key, _configws);
                                        try
                                        {
                                            TDeleteNavPreInvoice.Wait();

                                            if (!TDeleteNavPreInvoice.IsCompletedSuccessfully)
                                            {
                                                result.eReasonCode = 5;
                                                result.eMessage = "Não é possivel eliminar a Fatura no NAV2017.";
                                                return Json(result);
                                            }
                                            else
                                            {
                                                result.eReasonCode = 1;
                                                result.eMessage = "Autorização anulada com sucesso.";
                                                return Json(result);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            result.eReasonCode = 5;
                                            result.eMessage = "Ocorreu um erro ao eliminar a Fatura do NAV2017.";
                                            return Json(result);
                                        }
                                    }
                                    else
                                    {
                                        result.eReasonCode = 5;
                                        result.eMessage = "Não foi possivel obter a chave da fatura do NAV2017.";
                                        return Json(result);
                                    }
                                }
                                else
                                {
                                    result.eReasonCode = 5;
                                    result.eMessage = "Não foi possivel obter o código da fatura do NAV2017.";
                                    return Json(result);
                                }
                            }
                            else
                            {
                                result.eReasonCode = 5;
                                result.eMessage = "Erro: Não foi possivel obter o Nº da Fatura do NAV2017.";
                                return Json(result);
                            }
                        }
                        else
                        {
                            result.eReasonCode = 5;
                            result.eMessage = "Erro: Não foi possivel obter a Autorização no e-SUCH.";
                            return Json(result);
                        }
                    }
                }
                else
                {
                    if (authProjectMovements == null)
                    {
                        result.eReasonCode = 5;
                        result.eMessage = "Têm que escolher 1 movimento de faturação.";
                        return Json(result);
                    }

                    if (authProjectMovements.Count != 1)
                    {
                        result.eReasonCode = 5;
                        result.eMessage = "Só pode escolher 1 movimento de faturação.";
                        return Json(result);
                    }
                }
            }
            catch (Exception ex)
            {
                result.eReasonCode = 99;
                result.eMessage = "Ocorreu um erro.";
            }

            return Json(result);
        }


    }
}