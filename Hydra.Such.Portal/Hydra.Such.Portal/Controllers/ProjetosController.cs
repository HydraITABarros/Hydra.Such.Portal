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

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class ProjetosController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;
        private readonly IHostingEnvironment _hostingEnvironment;
        public ProjetosController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs, IHostingEnvironment _hostingEnvironment)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
            this._hostingEnvironment = _hostingEnvironment;
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
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.ProjectNo = id == null ? "" : id;
                ViewBag.UPermissions = UPerm;
                ViewBag.reportServerURL = _config.ReportServerURL;
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

        #region Home
        [HttpPost]
        public JsonResult GetListProjectsByArea([FromBody] JObject requestParams)
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
                //x.ClientName = DBNAV2017Clients.GetClientNameByNo(x.ClientNo, _config.NAVDatabaseName, _config.NAVCompanyName);
            });

            return Json(result);
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
                    ProjectDetailsViewModel result = new ProjectDetailsViewModel()
                    {
                        ProjectNo = cProject.NºProjeto,
                        Area = cProject.Área,
                        Description = cProject.Descrição,
                        ClientNo = cProject.NºCliente,
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
                        ObservacoesAutorizarFaturacao = ""
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
        public JsonResult ValidateNumeration([FromBody] ProjectDetailsViewModel data)
        {
            //Get Project Numeration
            Configuração Cfg = DBConfigurations.GetById(1);
            int ProjectNumerationConfigurationId = Cfg.NumeraçãoProjetos.Value;

            ConfiguraçãoNumerações CfgNumeration = DBNumerationConfigurations.GetById(ProjectNumerationConfigurationId);

            //Validate if ProjectNo is valid
            if (!(data.ProjectNo == "" || data.ProjectNo == null) && !CfgNumeration.Manual.Value)
            {
                return Json("A numeração configurada para projetos não permite inserção manual.");
            }
            else if (data.ProjectNo == "" && !CfgNumeration.Automático.Value)
            {
                return Json("É obrigatório inserir o Nº de Projeto.");
            }

            return Json("");
        }

        [HttpPost]
        public JsonResult GetAddressData([FromBody] string AddressCode)
        {
            NAVAddressesViewModel result = DBNAV2017ShippingAddresses.GetByCode(AddressCode, _config.NAVDatabaseName, _config.NAVCompanyName);

            return Json(result);
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
                                UtilizadorCriação = User.Identity.Name
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
                                    UtilizadorCriação = User.Identity.Name
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

            if (data != null)
            {
                try
                {
                    Projetos cProject = new Projetos();

                    cProject.NºProjeto = data.ProjectNo;
                    cProject.Área = data.Area;
                    cProject.Descrição = data.Description;
                    cProject.NºCliente = data.ClientNo;
                    cProject.Data = data.Date != "" && data.Date != null ? DateTime.Parse(data.Date) : (DateTime?)null;
                    cProject.Estado = data.Status;
                    cProject.CódigoRegião = data.RegionCode;
                    cProject.CódigoÁreaFuncional = data.FunctionalAreaCode;
                    cProject.CódigoCentroResponsabilidade = data.ResponsabilityCenterCode;
                    cProject.Faturável = data.Billable;
                    cProject.NºContrato = data.ContractNo;
                    cProject.CódEndereçoEnvio = data.ShippingAddressCode;
                    cProject.EnvioANome = data.ShippingName;
                    cProject.EnvioAEndereço = data.ShippingAddress;
                    cProject.EnvioACódPostal = data.ShippingPostalCode;
                    cProject.EnvioALocalidade = data.ShippingLocality;
                    cProject.EnvioAContato = data.ShippingContact;
                    cProject.CódTipoProjeto = data.ProjectTypeCode;
                    cProject.NossaProposta = data.OurProposal;
                    cProject.CódObjetoServiço = data.ServiceObjectCode;
                    cProject.NºCompromisso = data.CommitmentCode;
                    cProject.GrupoContabObra = "PROJETO";
                    cProject.TipoGrupoContabProjeto = data.GroupContabProjectType;
                    //cProject.TipoGrupoContabOmProjeto = data.GroupContabOMProjectType;
                    cProject.PedidoDoCliente = data.ClientRequest;
                    cProject.DataDoPedido = data.RequestDate != "" && data.RequestDate != null ? DateTime.Parse(data.RequestDate) : (DateTime?)null;
                    cProject.ValidadeDoPedido = data.RequestValidity;
                    cProject.DescriçãoDetalhada = data.DetailedDescription;
                    cProject.CategoriaProjeto = data.ProjectCategory;
                    cProject.NºContratoOrçamento = data.BudgetContractNo;
                    cProject.ProjetoInterno = data.InternalProject;
                    cProject.ChefeProjeto = data.ProjectLeader;
                    cProject.ResponsávelProjeto = data.ProjectResponsible;
                    cProject.UtilizadorModificação = User.Identity.Name;

                    DBProjects.Update(cProject);

                    data.eReasonCode = 1;


                    //Read NAV Project Key
                    Task<WSCreateNAVProject.Read_Result> TReadNavProj = WSProject.GetNavProject(data.ProjectNo, _configws);
                    try
                    {
                        TReadNavProj.Wait();
                    }
                    catch (Exception ex)
                    {
                        data.eReasonCode = 3;
                        data.eMessage = "Ocorreu um erro ao atualizar o projeto no NAV.";
                    }

                    if (TReadNavProj.IsCompletedSuccessfully)
                    {
                        if (TReadNavProj.Result.WSJob == null)
                        {
                            data.eReasonCode = 3;
                            data.eMessage = "Erro ao atualizar: O projeto não existe no NAV";
                        }
                        else
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
                                data.eMessage = ex.InnerException.Message;
                                statusL = false;
                            }

                            if (!TUpdateNavProj.IsCompletedSuccessfully && statusL)
                            {
                                data.eReasonCode = 3;
                                data.eMessage = "Ocorreu um erro ao atualizar o projeto no NAV.";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    data.eReasonCode = 2;
                    data.eMessage = "Ocorreu um erro ao atualizar o projeto.";
                }


                return Json(data);
            }
            return Json(false);
        }



        [HttpPost]
        public JsonResult DeleteProject([FromBody] ProjectDetailsViewModel data)
        {

            if (data != null)
            {
                ErrorHandler result = new ErrorHandler();

                MovimentosDeAprovação MovAprovacao = DBApprovalMovements.GetAll().Where(x => x.Número == data.ProjectNo && x.Tipo == 5).LastOrDefault();
                if (MovAprovacao != null && MovAprovacao.Estado == 1)
                {
                    result = new ErrorHandler()
                    {
                        eReasonCode = 7,
                        eMessage = "Existe movimento de aprovação por aprovar."
                    };
                    return Json(result);
                }
                else
                {
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
                            UtilizadorCriação = User.Identity.Name
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
                    Quantity = x.Quantidade,
                    MeasurementUnitCode = x.CódUnidadeMedida,
                    LocationCode = x.CódLocalização,
                    ProjectContabGroup = x.GrupoContabProjeto,
                    RegionCode = x.CódigoRegião,
                    FunctionalAreaCode = x.CódigoÁreaFuncional,
                    ResponsabilityCenterCode = x.CódigoCentroResponsabilidade,
                    User = x.Utilizador,
                    UnitCost = x.CustoUnitário,
                    TotalCost = x.CustoTotal,
                    UnitPrice = x.PreçoUnitário,
                    TotalPrice = x.PreçoTotal,
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
                    ServiceClientCode = (x.CódServiçoCliente != null && x.CódServiçoCliente != "") ? x.CódServiçoCliente : codServiceCliente
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
                    Quantity = x.Quantidade,
                    MeasurementUnitCode = x.CódUnidadeMedida,
                    LocationCode = x.CódLocalização,
                    ProjectContabGroup = x.GrupoContabProjeto,
                    RegionCode = x.CódigoRegião,
                    FunctionalAreaCode = x.CódigoÁreaFuncional,
                    ResponsabilityCenterCode = x.CódigoCentroResponsabilidade,
                    User = x.Utilizador,
                    UnitCost = x.CustoUnitário,
                    TotalCost = x.CustoTotal,
                    UnitPrice = x.PreçoUnitário,
                    TotalPrice = x.PreçoTotal,
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
                    ServiceClientCode = (x.CódServiçoCliente != null && x.CódServiçoCliente != "") ? x.CódServiçoCliente : codServiceCliente
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
                    }
                }


                dp.ForEach(x =>
                {
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
                        newdp.Quantidade = x.Quantity;
                        newdp.CódUnidadeMedida = x.MeasurementUnitCode;
                        newdp.CódLocalização = x.LocationCode;
                        newdp.GrupoContabProjeto = x.ProjectContabGroup;
                        newdp.CódigoRegião = x.RegionCode;
                        newdp.CódigoÁreaFuncional = x.FunctionalAreaCode;
                        newdp.CódigoCentroResponsabilidade = x.ResponsabilityCenterCode;
                        newdp.Utilizador = User.Identity.Name;
                        newdp.CustoUnitário = x.UnitCost;
                        newdp.CustoTotal = x.TotalCost;
                        newdp.PreçoUnitário = x.UnitPrice;
                        newdp.PreçoTotal = x.TotalPrice;
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
                            Quantidade = x.Quantity,
                            CódUnidadeMedida = x.MeasurementUnitCode,
                            CódLocalização = x.LocationCode,
                            GrupoContabProjeto = x.ProjectContabGroup,
                            CódigoRegião = x.RegionCode,
                            CódigoÁreaFuncional = x.FunctionalAreaCode,
                            CódigoCentroResponsabilidade = x.ResponsabilityCenterCode,
                            Utilizador = User.Identity.Name,
                            CustoUnitário = x.UnitCost,
                            CustoTotal = x.TotalCost,
                            PreçoUnitário = x.UnitPrice,
                            PreçoTotal = x.TotalPrice,
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
                        newdp.Quantidade = x.Quantity;
                        newdp.CódUnidadeMedida = x.MeasurementUnitCode;
                        newdp.CódLocalização = x.LocationCode;
                        newdp.GrupoContabProjeto = x.ProjectContabGroup;
                        newdp.CódigoRegião = x.RegionCode;
                        newdp.CódigoÁreaFuncional = x.FunctionalAreaCode;
                        newdp.CódigoCentroResponsabilidade = x.ResponsabilityCenterCode;
                        newdp.Utilizador = userName;
                        newdp.CustoUnitário = x.UnitCost;
                        newdp.CustoTotal = x.TotalCost;
                        newdp.PreçoUnitário = x.UnitPrice;
                        newdp.PreçoTotal = x.TotalPrice;
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
                            Quantidade = x.Quantity,
                            CódUnidadeMedida = x.MeasurementUnitCode,
                            CódLocalização = x.LocationCode,
                            GrupoContabProjeto = x.ProjectContabGroup,
                            CódigoRegião = x.RegionCode,
                            CódigoÁreaFuncional = x.FunctionalAreaCode,
                            CódigoCentroResponsabilidade = x.ResponsabilityCenterCode,
                            Utilizador = userName,
                            CustoUnitário = x.UnitCost,
                            CustoTotal = x.TotalCost,
                            PreçoUnitário = x.UnitPrice,
                            PreçoTotal = x.TotalPrice,
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
        public JsonResult CreatePDByMovProj([FromBody] List<ProjectDiaryViewModel> dp, string projectNo, string Resources, string ProjDiaryPrice, string Date)
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
                                            pjD.UnitCost = lc.PreçoUnitário;
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
                                            Quantidade = pjD.Quantity,
                                            CódUnidadeMedida = pjD.MeasurementUnitCode,
                                            CódLocalização = pjD.LocationCode,
                                            GrupoContabProjeto = pjD.ProjectContabGroup,
                                            CódigoRegião = projecto.CódigoRegião,
                                            CódigoÁreaFuncional = projecto.CódigoÁreaFuncional,
                                            CódigoCentroResponsabilidade = projecto.CódigoCentroResponsabilidade,
                                            Utilizador = User.Identity.Name,
                                            CustoUnitário = pjD.UnitCost,
                                            CustoTotal = pjD.TotalCost,
                                            PreçoUnitário = pjD.UnitPrice,
                                            PreçoTotal = pjD.TotalPrice,
                                            Faturável = true,
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
                                            PréRegisto = false,
                                            CódDestinoFinalResíduos = pjD.ResidueFinalDestinyCode,
                                            TipoRecurso = pjD.ResourceType
                                            
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
                            Quantidade = x.Quantity,
                            CódUnidadeMedida = x.MeasurementUnitCode,
                            CódLocalização = x.LocationCode,
                            GrupoContabProjeto = x.ProjectContabGroup,
                            CódigoRegião = projecto.CódigoRegião,
                            CódigoÁreaFuncional = projecto.CódigoÁreaFuncional,
                            CódigoCentroResponsabilidade = projecto.CódigoCentroResponsabilidade,
                            Utilizador = User.Identity.Name,
                            CustoUnitário = x.UnitCost,
                            CustoTotal = x.TotalCost,
                            PreçoUnitário = x.UnitPrice,
                            PreçoTotal = x.TotalPrice,
                            Faturável = true,
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
                            PréRegisto = false,
                            CódDestinoFinalResíduos = x.ResidueFinalDestinyCode,
                            TipoRecurso = x.ResourceType


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

                bool hasItemsWithoutDimensions = dp.Any(x => string.IsNullOrEmpty(x.RegionCode) ||
                                                            string.IsNullOrEmpty(x.FunctionalAreaCode) ||
                                                            string.IsNullOrEmpty(x.ResponsabilityCenterCode));
                if (hasItemsWithoutDimensions)
                {
                    result.eReasonCode = 2;
                    result.eMessage = "Existem linhas inválidas: a Região, Área Funcional e Centro de Responsabilidade são obrigatórios.";
                }
                else
                {
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

                    try
                    {
                        ////Register Lines in NAV
                        //Task<WSGenericCodeUnit.FxPostJobJrnlLines_Result> TRegisterNavDiaryLine = WSProjectDiaryLine.RegsiterNavDiaryLines(transactID, _configws);
                        //TRegisterNavDiaryLine.Wait();

                        //if (TRegisterNavDiaryLine == null)
                        //{
                        //    Response.StatusCode = (int)HttpStatusCode.NoContent;
                        //    return Json(result);
                        //}
                    }
                    catch (Exception ex)
                    {
                        //Response.StatusCode = (int)HttpStatusCode.NoContent;
                        //return Json(result);
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

                                MovimentosDeProjeto ProjectMovement = new MovimentosDeProjeto()
                                {
                                    //NºLinha = newdp.NºLinha,
                                    NºProjeto = newdp.NºProjeto,
                                    Data = newdp.Data,
                                    TipoMovimento = 1, //CONSUMO
                                    Tipo = newdp.Tipo,
                                    Código = newdp.Código,
                                    Descrição = newdp.Descrição,
                                    Quantidade = newdp.Quantidade,
                                    CódUnidadeMedida = newdp.CódUnidadeMedida,
                                    CódLocalização = newdp.CódLocalização,
                                    GrupoContabProjeto = newdp.GrupoContabProjeto,
                                    CódigoRegião = newdp.CódigoRegião,
                                    CódigoÁreaFuncional = newdp.CódigoÁreaFuncional,
                                    CódigoCentroResponsabilidade = newdp.CódigoCentroResponsabilidade,
                                    Utilizador = User.Identity.Name,
                                    CustoUnitário = newdp.CustoUnitário,
                                    CustoTotal = newdp.CustoTotal,
                                    PreçoUnitário = newdp.PreçoUnitário,
                                    PreçoTotal = newdp.PreçoTotal,
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
                                    CriarMovNav2017 = false
                                };

                                DBProjectMovements.Create(ProjectMovement);
                            }
                        }
                    });
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
                                Quantidade = newdp.Quantidade,
                                CódUnidadeMedida = newdp.CódUnidadeMedida,
                                CódLocalização = newdp.CódLocalização,
                                GrupoContabProjeto = newdp.GrupoContabProjeto,
                                CódigoRegião = newdp.CódigoRegião,
                                CódigoÁreaFuncional = newdp.CódigoÁreaFuncional,
                                CódigoCentroResponsabilidade = newdp.CódigoCentroResponsabilidade,
                                Utilizador = userName,
                                CustoUnitário = newdp.CustoUnitário,
                                CustoTotal = newdp.CustoTotal,
                                PreçoUnitário = newdp.PreçoUnitário,
                                PreçoTotal = newdp.PreçoTotal,
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

                            DiárioDeProjeto dpValidation = new DiárioDeProjeto();
                            item.Type = 2; //Recurso
                            item.CreateUser = User.Identity.Name;
                            item.CreateDate = DateTime.Now;
                            item.InvoiceToClientNo = proj.NºCliente;
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
                        newRow.UnitCost = item.PriceCost;
                        newRow.UnitPrice = item.SalePrice;
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
        public JsonResult GetProjectMovements([FromBody] string ProjectNo)
        {
            List<NAVMeasureUnitViewModel> MeasurementUnitList = DBNAV2017MeasureUnit.GetAllMeasureUnit(_config.NAVDatabaseName, _config.NAVCompanyName);
            List<NAVLocationsViewModel> LocationList = DBNAV2017Locations.GetAllLocations(_config.NAVDatabaseName, _config.NAVCompanyName);
            List<NAVClientsViewModel> ClientsList = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, "");
            List<TiposRefeição> MealList = DBMealTypes.GetAll();

            try
            {
                List<ProjectDiaryViewModel> dp = DBProjectMovements.GetRegisteredDiary(ProjectNo).Select(x => new ProjectDiaryViewModel()
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
                    Quantity = x.Quantidade,
                    MeasurementUnitCode = !string.IsNullOrEmpty(x.CódUnidadeMedida) ? MeasurementUnitList.Where(y => y.Code == x.CódUnidadeMedida).FirstOrDefault() != null ? MeasurementUnitList.Where(y => y.Code == x.CódUnidadeMedida).FirstOrDefault().Description : "" : "",
                    LocationCode = !string.IsNullOrEmpty(x.CódLocalização) ? LocationList.Where(y => y.Code == x.CódLocalização).FirstOrDefault() != null ? LocationList.Where(y => y.Code == x.CódLocalização).FirstOrDefault().Name : "" : "",
                    ProjectContabGroup = x.GrupoContabProjeto,
                    RegionCode = x.CódigoRegião,
                    FunctionalAreaCode = x.CódigoÁreaFuncional,
                    ResponsabilityCenterCode = x.CódigoCentroResponsabilidade,
                    User = x.Utilizador,
                    UnitCost = x.CustoUnitário,
                    TotalCost = x.CustoTotal,
                    UnitPrice = x.PreçoUnitário,
                    TotalPrice = x.PreçoTotal,
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
                    ClientName = !string.IsNullOrEmpty(x.FaturaANºCliente) ? ClientsList.Where(y => y.No_ == x.FaturaANºCliente).FirstOrDefault() != null ? ClientsList.Where(y => y.No_ == x.FaturaANºCliente).FirstOrDefault().Name : "" : "",
                    MealTypeDescription = x.TipoRefeição != null ? MealList.Where(y => y.Código == x.TipoRefeição).FirstOrDefault() != null ? MealList.Where(y => y.Código == x.TipoRefeição).FirstOrDefault().Descrição : "" : "",
                    Utilizador = User.Identity.Name,
                    NameDB = _config.NAVDatabaseName,
                    CompanyName = _config.NAVCompanyName

                }).ToList();

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
        public JsonResult GetProjectMovementsDp([FromBody] string ProjectNo, bool allProjs, string projectTarget, string NoDocument, string Resources, string ProjDiaryPrice)
        {

            List<ProjectDiaryViewModel> dp = DBProjectMovements.GetRegisteredDiaryDp(ProjectNo, User.Identity.Name, allProjs).Select(x => new ProjectDiaryViewModel()
            {
                LineNo = x.NºLinha,
                ProjectNo = x.NºProjeto,
                Date = x.Data == null ? String.Empty : x.Data.Value.ToString("yyyy-MM-dd"),
                MovementType = x.TipoMovimento,
                Type = x.Tipo,
                Code = x.Código,
                Description = x.Descrição,
                Quantity = x.Quantidade,
                MeasurementUnitCode = x.CódUnidadeMedida,
                LocationCode = x.CódLocalização,
                ProjectContabGroup = x.GrupoContabProjeto,
                RegionCode = x.CódigoRegião,
                FunctionalAreaCode = x.CódigoÁreaFuncional,
                ResponsabilityCenterCode = x.CódigoCentroResponsabilidade,
                User = x.Utilizador,
                UnitCost = x.CustoUnitário,
                TotalCost = x.CustoTotal,
                UnitPrice = x.PreçoUnitário,
                TotalPrice = x.PreçoTotal,
                Billable = x.Faturável,
                Registered = x.Registado,
                DocumentNo = x.NºDocumento,
                MealType = x.TipoRefeição,
                ConsumptionDate = x.DataConsumo == null ? String.Empty : x.DataConsumo.Value.ToString("yyyy-MM-dd"),
                ResidueGuideNo = x.NºGuiaResíduos,
                ResidueFinalDestinyCode = x.CódDestinoFinalResíduos,
                ExternalGuideNo = x.NºGuiaExterna,
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
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.projectNo = id;
                ViewBag.UPermissions = UPerm;
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
                        mov.Quantity = Math.Abs((decimal)mov.Quantity) * (-1);
                    }

                    if (!String.IsNullOrEmpty(mov.Currency))
                    {
                        mov.UnitPrice = mov.UnitValueToInvoice;
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
        public JsonResult AuthorizeProjectMovements([FromBody] JObject requestParams)
        {
            ErrorHandler result = ValidateMovements(requestParams);
            if (result.eReasonCode != 1)
                return Json(result);

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

                #endregion

                Projetos project = null;
                Contratos contract = null;
                NAVClientsViewModel customer = null;

                if (!string.IsNullOrEmpty(projectNo))
                    project = DBProjects.GetById(projectNo);

                if (project != null)
                {
                    contract = DBContracts.GetByIdLastVersion(project.NºContrato);
                    customer = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, project.NºCliente);

                    using (SuchDBContext ctx = new SuchDBContext())
                    {
                        int? lastUsed = ctx.ProjectosAutorizados
                            .Where(x => x.CodProjeto == projectNo)
                            .OrderByDescending(x => x.GrupoFactura)
                            .Select(x => x.GrupoFactura)
                            .FirstOrDefault();

                        int invoiceGroup = lastUsed.HasValue ? lastUsed.Value + 1 : 1;

                        ProjectosAutorizados authorizedProject = new ProjectosAutorizados();
                        authorizedProject.CodProjeto = project.NºProjeto;
                        authorizedProject.GrupoFactura = invoiceGroup;
                        authorizedProject.Faturado = false;
                        authorizedProject.DescricaoGrupo = invoiceGroupDescription;
                        authorizedProject.NumCompromisso = commitmentNumber;
                        authorizedProject.CodCliente = project.NºCliente;
                        authorizedProject.CodContrato = contract?.NºDeContrato;
                        authorizedProject.CodTermosPagamento = contract != null ? contract.CódTermosPagamento : customer?.PaymentTermsCode;
                        authorizedProject.CodMetodoPagamento = customer?.PaymentMethodCode;
                        authorizedProject.CodRegiao = customer.National || customer.InternalClient ? project.CódigoRegião : customer.RegionCode;
                        authorizedProject.CodAreaFuncional = project.CódigoÁreaFuncional;
                        authorizedProject.CodCentroResponsabilidade = project.CódigoCentroResponsabilidade;
                        authorizedProject.PedidoCliente = customerRequestNo;
                        if (customerRequestDate > DateTime.MinValue)
                            authorizedProject.DataPedido = customerRequestDate;
                        authorizedProject.DataAutorizacao = DateTime.Now;
                        authorizedProject.Utilizador = User.Identity.Name;
                        authorizedProject.Observacoes = projectObs;
                        if (serviceDate > DateTime.MinValue)
                            authorizedProject.DataPrestacaoServico = serviceDate;
                        authorizedProject.DataServPrestado = billingPeriod;

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
                            authorizedProjMovement.Quantidade = x.Quantity ?? 0;
                            authorizedProjMovement.CodUnidadeMedida = x.MeasurementUnitCode;
                            authorizedProjMovement.PrecoVenda = x.UnitPrice ?? 0;
                            authorizedProjMovement.PrecoTotal = x.TotalPrice ?? 0;
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
                            authorizedProjMovement.PrecoCusto = x.UnitCost;
                            authorizedProjMovement.CustoTotal = x.TotalCost;
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

                        ctx.ProjectosAutorizados.Add(authorizedProject);
                        ctx.MovimentosDeProjeto.UpdateRange(unchangedProjectMovements);// projMovements.ParseToDB());
                        ctx.MovimentosProjectoAutorizados.AddRange(authorizedProjMovements);

                        try
                        {
                            ctx.SaveChanges();
                            result.eReasonCode = 1;
                            result.eMessage = "Movimentos autorizados com o Grupo Fatura " + invoiceGroup.ToString();
                        }
                        catch (Exception ex)
                        {
                            result.eReasonCode = 2;
                            result.eMessage = "Ocorreu um erro ao tentar autorizar os movimentos.";
                        }
                    }
                }
                else
                {
                    result.eReasonCode = 2;
                    result.eMessage = "Não foi possivel obter o projeto.";
                }
            }
            catch (Exception ex)
            {
                result.eReasonCode = 2;
                result.eMessage = "Ocorreu um erro ao autorizar.";
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

                DateTime serviceDate;
                JValue serviceDateValue = requestParams["serviceDate"] as JValue;
                if (serviceDateValue != null)
                    DateTime.TryParse((string)serviceDateValue.Value, out serviceDate);

                decimal authorizationTotal;
                JValue authorizationTotalValue = requestParams["authorizationTotalValue"] as JValue;
                if (authorizationTotalValue != null)
                {
                    string str = authorizationTotalValue.Value.ToString();
                    authorizationTotal = decimal.Parse(str, CultureInfo.InvariantCulture);
                }

                List<ProjectMovementViewModel> projMovements = new List<ProjectMovementViewModel>();
                JArray projMovementsValue = requestParams["projMovements"] as JArray;
                if (projMovementsValue != null)
                    projMovements = projMovementsValue.ToObject<List<ProjectMovementViewModel>>();
                projMovements.ForEach(x =>
                {
                    if (x.FunctionalAreaCode.StartsWith("5") && !x.MealType.HasValue)
                    {
                        result.eMessages.Add(new TraceInformation(TraceType.Error, "O tipo de refeição é obrigatório nas linhas com Área Funcional Nutrição"));
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

                    if (contract != null)
                    {
                        //Validar se o contrato indicado no projeto está vigente
                        if (contract.DataInicial.HasValue && contract.DataFimContrato.HasValue &&
                            (DateTime.Now < contract.DataInicial.Value || DateTime.Now > contract.DataFimContrato.Value))
                        {
                            result.eMessages.Add(new TraceInformation(TraceType.Warning, "O Contrato não está vigente."));
                        }
                        //Validar se o compromisso é o que está no contrato                
                        if (commitmentNumber != contract.NºCompromisso)
                        {
                            result.eMessages.Add(new TraceInformation(TraceType.Warning, "O Nº do Compromisso é diferente do que está no Contrato."));
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
                                result.eMessages.Add(new TraceInformation(TraceType.Error, "O preenchimento da Data de Consumo é obrigatório nos movimentos da áreas de residuos (ver movimentos: " + string.Join(',', authorizedItems) + ")."));
                        }
                        else
                            result.eMessages.Add(new TraceInformation(TraceType.Error, "A área de residuos não está configurada. Contacte o administrador."));
                    }
                    else
                        result.eMessages.Add(new TraceInformation(TraceType.Error, "Ocorreu um erro ao validar a área de residuos."));
                }
                else
                {
                    result.eReasonCode = 2;
                    result.eMessage = "Não foi possivel obter detalhes do projeto.";
                }
            }
            catch (Exception ex)
            {
                result.eReasonCode = 2;
                result.eMessages.Add(new TraceInformation(TraceType.Exception, "Ocorreu um erro ao validar os movimentos: " + ex.Message + "."));
            }
            bool hasErrors = result.eMessages.Any(x => x.Type == TraceType.Error || x.Type == TraceType.Exception);
            if (hasErrors || result.eReasonCode > 1)
            {
                result.eReasonCode = 2;
                result.eMessage = "Foram detetados erros nos movimentos submetidos.";
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
                                commitmentNo = billingGroupReq.FirstOrDefault(x => x.DataFimCompromisso >= serviceDeliveryDate && !string.IsNullOrEmpty(x.NºCompromisso))?.NºCompromisso;
                            if (string.IsNullOrEmpty(customerRequestNo))
                            {
                                var item = billingGroupReq.FirstOrDefault(x => x.DataFimCompromisso >= serviceDeliveryDate && !string.IsNullOrEmpty(x.NºRequisiçãoCliente));
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
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
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
                            lst.Quantity = Math.Abs((decimal)lst.Quantity) * (-1);
                        }

                        if (!String.IsNullOrEmpty(lst.Currency))
                        {
                            lst.UnitPrice = lst.UnitValueToInvoice;
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

                    List<NAVClientsViewModel> clients = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, string.Join(",", result.Select(r=>r.CodCliente).ToList()) ).ToList();
                    List<MovimentosProjectoAutorizados> AllAuthorizedProjectMovements = DBAuthorizedProjectMovements.GetAll("");

                    result.ForEach(x =>
                    {
                        var movements = AllAuthorizedProjectMovements.Where(y => y.GrupoFactura == x.GrupoFactura && y.CodProjeto == x.CodProjeto);
                        if (movements != null) { 
                            x.ValorAutorizado = movements.Sum(y => y.PrecoTotal);
                        }

                        x.NomeCliente = !string.IsNullOrEmpty(x.CodCliente) ? clients.Where(y => y.No_ == x.CodCliente).FirstOrDefault() != null ? clients.Where(y => y.No_ == x.CodCliente).FirstOrDefault().Name : "" : "";
                    });
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

                    result.ForEach(x =>
                    {
                        var movements = DBAuthorizedProjectMovements.GetMovementById(x.GrupoFactura, x.CodProjeto);
                        if (movements != null)
                            x.ValorAutorizado = movements.Sum(y => y.PrecoTotal);
                    });
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
            List<ProjectMovementViewModel> projectMovements = new List<ProjectMovementViewModel>();
            try
            {
                projectMovements = DBProjectMovements.GetProjMovementsById(ProjNo, ProjGroup, Faturada)
               .ParseToViewModel(_config.NAVDatabaseName, _config.NAVCompanyName)
               .OrderBy(x => x.ClientName).ToList();

                if (projectMovements.Count > 0)
                {
                    var userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
                    foreach (var lst in projectMovements)
                    {
                        if (lst.MovementType == 3)
                        {
                            lst.Quantity = Math.Abs((decimal)lst.Quantity) * (-1);
                        }

                        if (!String.IsNullOrEmpty(lst.Currency))
                        {
                            lst.UnitPrice = lst.UnitValueToInvoice;
                        }
                    }
                    List<UserDimensionsViewModel> userDimensionsViewModel = userDimensions.ParseToViewModel();
                    if (userDimensionsViewModel.Where(x => x.Dimension == (int)Dimensions.Region).Count() > 0)
                        projectMovements.RemoveAll(x => !userDimensionsViewModel.Any(y => y.DimensionValue == x.RegionCode));
                    if (userDimensionsViewModel.Where(x => x.Dimension == (int)Dimensions.FunctionalArea).Count() > 0)
                        projectMovements.RemoveAll(x => !userDimensionsViewModel.Any(y => y.DimensionValue == x.FunctionalAreaCode));
                    if (userDimensionsViewModel.Where(x => x.Dimension == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                        projectMovements.RemoveAll(x => !userDimensionsViewModel.Any(y => y.DimensionValue == x.ResponsabilityCenterCode));
                }
            }
            catch (Exception ex)
            {
                projectMovements = new List<ProjectMovementViewModel>();
            }
            return Json(projectMovements);
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

        [HttpPost]
        public JsonResult CreateBillingDocuments([FromBody] List<AuthorizedProjectViewModel> authProjectMovements, string OptionInvoice, string dataFormulario)
        {
            string execDetails = string.Empty;
            string errorMessage = string.Empty;
            bool hasErrors = false;
            ErrorHandler result = new ErrorHandler();
            string projeto = string.Empty;

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
            projeto = authProjectMovements.Select(x => x.CodProjeto).FirstOrDefault();

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
                            wSJobJournalLine.Quantity = movimento.Quantidade ?? 0;
                            wSJobJournalLine.QuantitySpecified = true;
                            wSJobJournalLine.Unit_of_Measure_Code = movimento.CódUnidadeMedida ?? string.Empty;
                            wSJobJournalLine.Location_Code = movimento.CódLocalização ?? string.Empty;
                            //wSJobJournalLine.Posting_Group = movimento.GrupoContabProjeto ?? string.Empty;
                            wSJobJournalLine.RegionCode20 = string.Empty;
                            wSJobJournalLine.FunctionAreaCode20 = string.Empty;
                            wSJobJournalLine.ResponsabilityCenterCode20 = string.Empty;
                            wSJobJournalLine.Unit_Cost = movimento.CustoUnitário ?? 0;
                            wSJobJournalLine.Total_Cost = movimento.CustoTotal ?? 0;
                            wSJobJournalLine.Unit_Price = movimento.PreçoUnitário ?? 0;
                            wSJobJournalLine.Total_Price = movimento.PreçoTotal ?? 0;
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

                //AMARO NOVO METODO PARA IR BUSCAR AS LINHAS
                //INICIO
                /*
                List<MovimentosProjectoAutorizados> ListMPA = DBAuthorizedProjectMovements.GetAll("");
                foreach (AuthorizedProjectViewModel AuthProj in authProjectMovements)
                {
                    List<MovimentosDeProjeto> MOV = DBProjectMovements.GetByProjectNo(AuthProj.CodProjeto);
                    foreach (MovimentosProjectoAutorizados MPA in ListMPA)
                    {
                        if (MPA.CodProjeto == AuthProj.CodProjeto && MPA.GrupoFactura == AuthProj.GrupoFactura)
                        {
                            SPInvoiceListViewModel addlinha = new SPInvoiceListViewModel
                            {
                                CommitmentNumber = AuthProj.NumCompromisso,
                                Date = AuthProj.DataPrestacaoServico,
                                InvoiceToClientNo = AuthProj.CodCliente,
                                ClientRequest = AuthProj.PedidoCliente,

                                MealType = MPA.TipoRefeicao,
                                Type = MPA.Tipo,
                                Code = MPA.Codigo,
                                Description = MPA.Descricao,
                                RegionCode = MPA.CodRegiao,
                                FunctionalAreaCode = MPA.CodAreaFuncional,
                                ResponsabilityCenterCode = MPA.CodCentroResponsabilidade,
                                ProjectNo = MPA.CodProjeto,
                                ContractNo = projectsDetails.Select(x => x.NºContrato).FirstOrDefault(x => x == MPA.CodProjeto),
                                ConsumptionDate = MPA.DataConsumo.HasValue ? MPA.DataConsumo.Value.ToString("yyyy-MM-dd") : "",
                                ServiceClientCode = MPA.CodServCliente,
                                MeasurementUnitCode = MPA.CodUnidadeMedida,
                                Quantity = MPA.Quantidade,
                                ServiceGroupCode = MPA.CodGrupoServico,
                                ExternalGuideNo = MPA.NumGuiaExterna,
                                WasteGuideNo_GAR = MPA.NumGuiaResiduosGar,
                                InvoiceGroup = MPA.GrupoFactura,

                                UnitPrice = MOV.FirstOrDefault(y => y.NºLinha == MPA.NumMovimento).PreçoUnitário,
                                UnitCost = MOV.FirstOrDefault(y => y.NºLinha == MPA.NumMovimento).CustoUnitário,
                                LocationCode = MOV.FirstOrDefault(y => y.NºLinha == MPA.NumMovimento).CódLocalização,
                                ProjectContabGroup = MOV.FirstOrDefault(y => y.NºLinha == MPA.NumMovimento).GrupoContabProjeto,
                            };
                            data.Add(addlinha);
                        }
                    }
                }
                */
                //FIM


                
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
                            UnitPrice = mp.PreçoUnitário,
                            UnitCost = mp.CustoUnitário,
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

                var groupedbyclient = data.GroupBy(x => new
                {
                    x.InvoiceToClientNo,
                    x.Date,
                    x.CommitmentNumber,
                    x.ClientRequest
                },
                    x => x,
                    (key, items) => new AuthorizedCustomerBilling
                    {
                        InvoiceToClientNo = key.InvoiceToClientNo,
                        Date = key.Date,
                        CommitmentNumber = key.CommitmentNumber,
                        ClientRequest = key.ClientRequest,
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
                                                 y.DataPrestacaoServico == key.Date &&
                                                 y.NumCompromisso == key.CommitmentNumber &&
                                                 y.PedidoCliente == key.ClientRequest)?.CodMetodoPagamento,
                        CodTermosPagamento = authProjectMovements
                            .FirstOrDefault(y => y.CodCliente == key.InvoiceToClientNo &&
                                                 y.DataPrestacaoServico == key.Date &&
                                                 y.NumCompromisso == key.CommitmentNumber &&
                                                 y.PedidoCliente == key.ClientRequest)?.CodTermosPagamento,
                        Comments = authProjectMovements
                            .FirstOrDefault(y => y.CodCliente == key.InvoiceToClientNo &&
                                                 y.DataPrestacaoServico == key.Date &&
                                                 y.NumCompromisso == key.CommitmentNumber &&
                                                 y.PedidoCliente == key.ClientRequest)?.Observacoes,
                        ServiceDate = authProjectMovements
                            .FirstOrDefault(y => y.CodCliente == key.InvoiceToClientNo &&
                                                 y.DataPrestacaoServico == key.Date &&
                                                 y.NumCompromisso == key.CommitmentNumber &&
                                                 y.PedidoCliente == key.ClientRequest)?.DataServPrestado,
                        RegionCode = authProjectMovements
                            .FirstOrDefault(y => y.CodCliente == key.InvoiceToClientNo &&
                                                 y.DataPrestacaoServico == key.Date &&
                                                 y.NumCompromisso == key.CommitmentNumber &&
                                                 y.PedidoCliente == key.ClientRequest)?.CodRegiao,
                        FunctionalAreaCode = authProjectMovements
                            .FirstOrDefault(y => y.CodCliente == key.InvoiceToClientNo &&
                                                 y.DataPrestacaoServico == key.Date &&
                                                 y.NumCompromisso == key.CommitmentNumber &&
                                                 y.PedidoCliente == key.ClientRequest)?.CodAreaFuncional,
                        ResponsabilityCenterCode = authProjectMovements
                            .FirstOrDefault(y => y.CodCliente == key.InvoiceToClientNo &&
                                                 y.DataPrestacaoServico == key.Date &&
                                                 y.NumCompromisso == key.CommitmentNumber &&
                                                 y.PedidoCliente == key.ClientRequest)?.CodCentroResponsabilidade,
                    })
                .ToList();

                bool allMealTypesAreValid = true;

                groupedbyclient.ForEach(x =>
                {
                    //Set dimensions
                    var authProj = authProjectMovements
                            .FirstOrDefault(y => y.CodCliente == x.InvoiceToClientNo &&
                                                 y.DataPrestacaoServico == x.Date &&
                                                 y.NumCompromisso == x.CommitmentNumber &&
                                                 y.PedidoCliente == x.ClientRequest);
                    var proj = projectsDetails.FirstOrDefault(y => y.NºProjeto == x.Items.FirstOrDefault()?.ProjectNo);
                    string projectRegion = proj != null ? proj.CódigoRegião : string.Empty;
                    var customer = customers.FirstOrDefault(y => y.No_ == x.InvoiceToClientNo);
                    x.SetDimensionsFor(authProj, projectRegion, customer);
                    x.DataPedido = authProj != null ? authProj.DataPedido : null;

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
                        var itemsToInvoice = header.Items.Select(x => new Tuple<string, int>(x.ProjectNo, x.InvoiceGroup.Value)).Distinct().ToList();
                        try
                        {
                            //var invoiceHeader = header.Items.First();
                            //invoiceHeader.ClientVATReg = header.ClientVATReg;
                            //invoiceHeader.MovementType = Convert.ToInt32(OptionInvoice);
                            //invoiceHeader.CreateUser = User.Identity.Name;
                            header.CreateUser = User.Identity.Name;

                            execDetails = string.Format("Fat. Cliente: {0}, Data: {1}, Nº Compromisso: {2} - ", header.InvoiceToClientNo, header.Date, header.CommitmentNumber);


                            SPInvoiceListViewModel Ship = new SPInvoiceListViewModel();
                            Projetos proj = DBProjects.GetById(projeto);
                            Contratos cont = DBContracts.GetByIdLastVersion(proj.NºContrato);
                            NAVClientsViewModel cli = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, proj.NºCliente);

                            if (proj != null && !string.IsNullOrEmpty(proj.EnvioANome) && !string.IsNullOrEmpty(proj.EnvioAEndereço))
                            {
                                Ship.Ship_to_Address = proj.EnvioAEndereço.Length > 50 ? proj.EnvioAEndereço.Substring(1, 50) : proj.EnvioAEndereço;
                                Ship.Ship_to_Address_2 = "";
                                Ship.Ship_to_City = proj.EnvioALocalidade;
                                Ship.Ship_to_Code = proj.CódEndereçoEnvio;
                                Ship.Ship_to_Contact = proj.EnvioAContato;
                                Ship.Ship_to_Country_Region_Code = "";
                                Ship.Ship_to_County = "";
                                Ship.Ship_to_Name = proj.EnvioANome.Length > 50 ? proj.EnvioANome.Substring(1, 50) : proj.EnvioANome;
                                Ship.Ship_to_Name_2 = "";
                                Ship.Ship_to_Post_Code = proj.EnvioACódPostal;
                            }
                            else
                            {
                                if (cont != null && !string.IsNullOrEmpty(cont.EnvioANome) && !string.IsNullOrEmpty(cont.EnvioAEndereço))
                                {
                                    Ship.Ship_to_Address = cont.EnvioAEndereço.Length > 50 ? cont.EnvioAEndereço.Substring(1, 50) : cont.EnvioAEndereço;
                                    Ship.Ship_to_Address_2 = "";
                                    Ship.Ship_to_City = cont.EnvioALocalidade;
                                    Ship.Ship_to_Code = cont.CódEndereçoEnvio;
                                    Ship.Ship_to_Contact = "";
                                    Ship.Ship_to_Country_Region_Code = "";
                                    Ship.Ship_to_County = "";
                                    Ship.Ship_to_Name = cont.EnvioANome.Length > 50 ? cont.EnvioANome.Substring(1, 50) : cont.EnvioANome;
                                    Ship.Ship_to_Name_2 = "";
                                    Ship.Ship_to_Post_Code = cont.EnvioACódPostal;
                                }
                                else
                                {
                                    if (cli != null)
                                    {
                                        Ship.Ship_to_Address = cli.Address.Length > 50 ? cli.Address.Substring(1, 50) : cli.Address;
                                        Ship.Ship_to_Address_2 = "";
                                        Ship.Ship_to_City = cli.City;
                                        Ship.Ship_to_Code = "";
                                        Ship.Ship_to_Contact = cli.PhoneNo;
                                        Ship.Ship_to_Country_Region_Code = cli.CountryRegionCode;
                                        Ship.Ship_to_County = cli.County;
                                        Ship.Ship_to_Name = cli.Name.Length > 50 ? cli.Name.Substring(1, 50) : cli.Name;
                                        Ship.Ship_to_Name_2 = "";
                                        Ship.Ship_to_Post_Code = cli.PostCode;
                                    }
                                }
                            }

                            Task<WSCreatePreInvoice.Create_Result> TCreatePreInvoice = WSPreInvoice.CreatePreInvoice(header, _configws, dataFormulario, projeto, Ship);
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

                                        //Para Nota de crédito passar o valor para positivo
                                        if (header.MovementType == 4 && x.TotalPrice.HasValue && x.TotalPrice < 0)
                                            x.TotalPrice = Math.Abs(x.TotalPrice.Value);
                                        if (header.MovementType == 4 && x.Quantity.HasValue && x.Quantity < 0)
                                            x.Quantity = Math.Abs(x.Quantity.Value);
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

                                            decimal? quantity = header.Items.Where(y => y.Type == 2 && y.Code == item.Code).Sum(y => y.Quantity);
                                            var resourceFirstLine = header.Items.Where(y => y.Type == 2 && y.Code == item.Code).OrderByDescending(y => y.ContractNo).FirstOrDefault();
                                            var resource = resourceslines.Where(y => y.Code == x.Recurso && y.WasteRate == 1).FirstOrDefault();
                                            if (resource != null)
                                            {
                                                SPInvoiceListViewModel wasteLineToAdd = new SPInvoiceListViewModel();
                                                wasteLineToAdd.Quantity = quantity ?? 0;
                                                wasteLineToAdd.Type = 2;
                                                wasteLineToAdd.Code = resource.Code;
                                                wasteLineToAdd.Description = resource.Name;
                                                wasteLineToAdd.UnitPrice = resource.UnitPrice;
                                                wasteLineToAdd.RegionCode = resourceFirstLine.RegionCode;
                                                wasteLineToAdd.ResponsabilityCenterCode = resourceFirstLine.ResponsabilityCenterCode;
                                                wasteLineToAdd.FunctionalAreaCode = resourceFirstLine.FunctionalAreaCode;
                                                wasteLineToAdd.ProjectDimension = resourceFirstLine.ProjectNo;
                                                wasteLineToAdd.ContractNo = projectsDetails
                                                            .Select(y => new { ProjectNo = y.NºProjeto, ContractNo = y.NºContrato })
                                                            .FirstOrDefault(y => y.ProjectNo == resourceFirstLine.ProjectNo)?.ContractNo;

                                                header.Items.Add(wasteLineToAdd);
                                            }
                                        });
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

            if (data == null)
            {
                result.eReasonCode = 2;
                result.eMessage = "Selecione os movimentos a faturar";
                return Json(result);
            }

            //Remove Project Authorized
            using (SuchDBContext ctx = new SuchDBContext())
            {
                bool hasChanges = false;
                var authorizedProj = ctx.ProjectosAutorizados.Where(x => x.GrupoFactura == data[0].GrupoFactura && x.CodProjeto == data[0].CodProjeto).ToList();
                if (authorizedProj.Count > 0)
                {
                    ctx.ProjectosAutorizados.RemoveRange(authorizedProj);
                    hasChanges = true;
                }

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

                        movAutProject = new MovimentosProjectoAutorizados();
                        movAutProject.NumMovimento = x.NºLinha;
                        authorizedProjMovements.Add(movAutProject);
                    });
                    ctx.MovimentosDeProjeto.UpdateRange(ProjectMovements);
                    ctx.MovimentosProjectoAutorizados.RemoveRange(authorizedProjMovements);
                    hasChanges = true;
                }
                if (hasChanges)
                {
                    try
                    {
                        ctx.SaveChanges();
                        result.eReasonCode = 1;
                        result.eMessage = "Os registo foram anulados com sucesso.";
                    }
                    catch (Exception ex)
                    {
                        result.eReasonCode = 2;
                        result.eMessage = "Não existem movimentos para anular";
                        result.eMessages.Add(new TraceInformation(TraceType.Exception, ex.Message));
                    }
                }
                else
                {
                    result.eReasonCode = 2;
                    result.eMessage = "Não existem registos para anular";
                }
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
                    x.CommitmentNumber,
                    x.ClientRequest,

                }).Select(x => new SPInvoiceListViewModel
                {
                    InvoiceToClientNo = x.Key.InvoiceToClientNo,
                    Date = x.Key.Date,
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
                                        decimal? quantity = linesList.Where(x => x.Code == item.Code).Sum(x => x.Quantity);

                                        var wasteFamilyResources = wr.Where(x => x.FamiliaRecurso == item.ResourceGroup).ToList();
                                        wasteFamilyResources.ForEach(x =>
                                        {
                                            SPInvoiceListViewModel wasteLineToAdd = new SPInvoiceListViewModel();
                                            wasteLineToAdd.Quantity = quantity ?? 0;
                                            wasteLineToAdd.Code = item.Code;
                                            wasteLineToAdd.Description = item.Name;
                                            wasteLineToAdd.UnitPrice = item.UnitPrice;
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
        public JsonResult GetPreMovementsProject([FromBody] string ProjectNo)
        {
            try
            {
                string NoCliente = DBProjects.GetById(ProjectNo).NºCliente;
                string ClientName = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, NoCliente).Name;

                List<Serviços> AllServices = DBServices.GetAll();
                List<TiposRefeição>  AllMealTypes = DBMealTypes.GetAll();
                List<ClientServicesViewModel> AllServiceGroup = DBClientServices.GetAllServiceGroup(string.Empty, true);

                List<ProjectDiaryViewModel> dp = DBPreProjectMovements.GetPreRegistered(ProjectNo).Select(x => new ProjectDiaryViewModel()
                {
                    LineNo = x.NºLinha,
                    ProjectNo = x.NºProjeto,
                    Date = x.Data == null ? String.Empty : x.Data.Value.ToString("yyyy-MM-dd"),
                    MovementType = x.TipoMovimento,
                    Type = x.Tipo,
                    Code = x.Código,
                    Description = x.Descrição,
                    Quantity = x.Quantidade,
                    MeasurementUnitCode = x.CódUnidadeMedida,
                    LocationCode = x.CódLocalização,
                    ProjectContabGroup = x.GrupoContabProjeto,
                    RegionCode = x.CódigoRegião,
                    FunctionalAreaCode = x.CódigoÁreaFuncional,
                    ResponsabilityCenterCode = x.CódigoCentroResponsabilidade,
                    User = x.Utilizador,
                    UnitCost = x.CustoUnitário,
                    TotalCost = x.CustoTotal,
                    UnitPrice = x.PreçoUnitário,
                    TotalPrice = x.PreçoTotal,
                    Billable = x.Faturável,
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
                                            pjD.UnitCost = lc.PreçoUnitário;
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
                                            Quantidade = pjD.Quantity,
                                            CódUnidadeMedida = pjD.MeasurementUnitCode,
                                            CódLocalização = pjD.LocationCode,
                                            GrupoContabProjeto = pjD.ProjectContabGroup,
                                            CódigoRegião = projecto.CódigoRegião,
                                            CódigoÁreaFuncional = projecto.CódigoÁreaFuncional,
                                            CódigoCentroResponsabilidade = projecto.CódigoCentroResponsabilidade,
                                            Utilizador = User.Identity.Name,
                                            CustoUnitário = pjD.UnitCost,
                                            CustoTotal = pjD.TotalCost,
                                            PreçoUnitário = pjD.UnitPrice,
                                            PreçoTotal = pjD.TotalPrice,
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
                            Quantidade = x.Quantity,
                            CódUnidadeMedida = x.MeasurementUnitCode,
                            CódLocalização = x.LocationCode,
                            GrupoContabProjeto = x.ProjectContabGroup,
                            CódigoRegião = projecto.CódigoRegião,
                            CódigoÁreaFuncional = projecto.CódigoÁreaFuncional,
                            CódigoCentroResponsabilidade = projecto.CódigoCentroResponsabilidade,
                            Utilizador = User.Identity.Name,
                            CustoUnitário = x.UnitCost,
                            CustoTotal = x.TotalCost,
                            PreçoUnitário = x.UnitPrice,
                            PreçoTotal = x.TotalPrice,
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
                    Quantity = x.Quantidade,
                    MeasurementUnitCode = x.CódUnidadeMedida,
                    LocationCode = x.CódLocalização,
                    ProjectContabGroup = x.GrupoContabProjeto,
                    RegionCode = x.CódigoRegião,
                    FunctionalAreaCode = x.CódigoÁreaFuncional,
                    ResponsabilityCenterCode = x.CódigoCentroResponsabilidade,
                    User = x.Utilizador,
                    UnitCost = x.CustoUnitário,
                    TotalCost = x.CustoTotal,
                    UnitPrice = x.PreçoUnitário,
                    TotalPrice = x.PreçoTotal,
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
                    Quantity = x.Quantidade,
                    MeasurementUnitCode = x.CódUnidadeMedida,
                    LocationCode = x.CódLocalização,
                    ProjectContabGroup = x.GrupoContabProjeto,
                    RegionCode = x.CódigoRegião,
                    FunctionalAreaCode = x.CódigoÁreaFuncional,
                    ResponsabilityCenterCode = x.CódigoCentroResponsabilidade,
                    User = x.Utilizador,
                    UnitCost = x.CustoUnitário,
                    TotalCost = x.CustoTotal,
                    UnitPrice = x.PreçoUnitário,
                    TotalPrice = x.PreçoTotal,
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
                        newdp.Quantidade = x.Quantity;
                        newdp.CódUnidadeMedida = x.MeasurementUnitCode;
                        newdp.CódLocalização = x.LocationCode;
                        newdp.GrupoContabProjeto = x.ProjectContabGroup;
                        newdp.CódigoRegião = x.RegionCode;
                        newdp.CódigoÁreaFuncional = x.FunctionalAreaCode;
                        newdp.CódigoCentroResponsabilidade = x.ResponsabilityCenterCode;
                        newdp.Utilizador = User.Identity.Name;
                        newdp.CustoUnitário = x.UnitCost;
                        newdp.CustoTotal = x.Quantity * x.UnitCost;
                        newdp.PreçoUnitário = x.UnitPrice;
                        newdp.PreçoTotal = x.Quantity * x.UnitPrice;
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
                            Quantidade = x.Quantity,
                            CódUnidadeMedida = x.MeasurementUnitCode,
                            CódLocalização = x.LocationCode,
                            GrupoContabProjeto = x.ProjectContabGroup,
                            CódigoRegião = x.RegionCode,
                            CódigoÁreaFuncional = x.FunctionalAreaCode,
                            CódigoCentroResponsabilidade = x.ResponsabilityCenterCode,
                            Utilizador = User.Identity.Name,
                            CustoUnitário = x.UnitCost,
                            CustoTotal = x.Quantity * x.UnitCost,
                            PreçoUnitário = x.UnitPrice,
                            PreçoTotal = x.Quantity * x.UnitPrice,
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
                               PreçoUnitário = x.PreçoUnitário,
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
                               PreçoUnitário = x.PreçoUnitário,
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
                                    Quantidade = newdp.Quantidade,
                                    CódUnidadeMedida = newdp.CódUnidadeMedida,
                                    CódLocalização = newdp.CódLocalização,
                                    GrupoContabProjeto = newdp.GrupoContabProjeto,
                                    CódigoRegião = newdp.CódigoRegião,
                                    CódigoÁreaFuncional = newdp.CódigoÁreaFuncional,
                                    CódigoCentroResponsabilidade = newdp.CódigoCentroResponsabilidade,
                                    Utilizador = User.Identity.Name,
                                    CustoUnitário = newdp.CustoUnitário,
                                    CustoTotal = newdp.CustoTotal,
                                    PreçoUnitário = newdp.PreçoUnitário,
                                    PreçoTotal = newdp.PreçoTotal,
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
                            line.Quantity = item.Items.Sum(x => x.Quantity);
                            line.TotalCost = line.UnitCost * line.Quantity;
                            line.TotalPrice = line.UnitPrice * line.Quantity;
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
                        projectMovement.CustoUnitário = item.UnitCost;
                        projectMovement.PreçoUnitário = item.UnitPrice;
                        projectMovement.Quantidade = item.Quantity;
                        projectMovement.CustoTotal = item.TotalCost;
                        projectMovement.PreçoTotal = item.TotalPrice;

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
                        newRow.UnitCost = item.PriceCost;
                        newRow.UnitPrice = item.SalePrice;
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
        public async Task<JsonResult> ExportToExcel([FromBody] List<PriceServiceClientViewModel> dp)
        {
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
            sFileName = @"/Upload/temp/" + sFileName;
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Preços Serviços Cliente.xlsx");
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
                string webRootPath = _hostingEnvironment.WebRootPath + "\\Upload\\temp";
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
                    }
                }
            }
            return Json(ListToCreate);
        }
        #endregion
        #endregion

        //1
        [HttpPost]
        public async Task<JsonResult> ExportToExcel_Projetos([FromBody] List<ProjectListItemViewModel> Lista)
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
            sFileName = @"/Upload/temp/" + sFileName;
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Projetos.xlsx");
        }

        //1
        [HttpPost]
        public async Task<JsonResult> ExportToExcel_AutorizacaoFaturacao([FromBody] List<ProjectMovementViewModel> Lista)
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
                            quantityCell.SetCellValue((double)(item.Quantity != null ? item.Quantity : 0));
                            
                            Col = Col + 1;
                        }
                        if (dp["unitCost"]["hidden"].ToString() == "False")
                        {
                            var unitCostCell = row.CreateCell(Col);
                            unitCostCell.SetCellType(CellType.Numeric);
                            unitCostCell.SetCellValue((double)(item.UnitCost != null ? item.UnitCost : 0));

                            Col = Col + 1;
                        }
                        if (dp["totalCost"]["hidden"].ToString() == "False")
                        {
                            var totalCostCell = row.CreateCell(Col);
                            totalCostCell.SetCellType(CellType.Numeric);
                            totalCostCell.SetCellValue((double)(item.TotalCost != null ? item.TotalCost : 0));

                            Col = Col + 1;
                        }
                        if (dp["unitPrice"]["hidden"].ToString() == "False")
                        {
                            var unitPriceCell = row.CreateCell(Col);
                            unitPriceCell.SetCellType(CellType.Numeric);
                            unitPriceCell.SetCellValue((double)(item.UnitPrice != null ? item.UnitPrice : 0));

                            Col = Col + 1;
                        }
                        if (dp["totalPrice"]["hidden"].ToString() == "False")
                        {
                            var totalPriceCell = row.CreateCell(Col);
                            totalPriceCell.SetCellType(CellType.Numeric);
                            totalPriceCell.SetCellValue((double)(item.TotalPrice != null ? item.TotalPrice: 0));

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
            sFileName = @"/Upload/temp/" + sFileName;
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Autorização de Faturação.xlsx");
        }

        //1
        [HttpPost]
        public async Task<JsonResult> ExportToExcel_FaturacaoProjetos([FromBody] List<AuthorizedProjectViewModel> Lista)
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
                if (dp["dataPrestacaoServico"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue("Data Serv. Prestado"); Col = Col + 1; }
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
                        if (dp["dataPrestacaoServico"]["hidden"].ToString() == "False") { row.CreateCell(Col).SetCellValue(item.DataPrestacaoServico); Col = Col + 1; }
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
            sFileName = @"/Upload/temp/" + sFileName;
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Faturação de Projetos.xlsx");
        }

        //1
        [HttpPost]
        public async Task<JsonResult> ExportToExcel_DetalhesAutorizacao([FromBody] List<ProjectMovementViewModel> Lista)
        {
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
                ISheet excelSheet = workbook.CreateSheet("Faturação de Projetos Detalhes");
                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("Data");
                row.CreateCell(1).SetCellValue("Tipo Movimento");
                row.CreateCell(2).SetCellValue("Tipo");
                row.CreateCell(3).SetCellValue("Código");
                row.CreateCell(4).SetCellValue("Descrição");
                row.CreateCell(5).SetCellValue("Quantidade");
                row.CreateCell(6).SetCellValue("Cód. Unidade Medida");
                row.CreateCell(7).SetCellValue("Preço Unitário");
                row.CreateCell(8).SetCellValue("Preço Total");
                row.CreateCell(9).SetCellValue("Faturável");
                row.CreateCell(10).SetCellValue("Tipo de Recurso");
                row.CreateCell(11).SetCellValue("Código Serviço");
                row.CreateCell(12).SetCellValue("Descrição Serviço");
                row.CreateCell(13).SetCellValue("Grupo Serviços");
                row.CreateCell(14).SetCellValue("Nº Guia Externa");
                row.CreateCell(15).SetCellValue("Data Consumo Guia");
                row.CreateCell(16).SetCellValue("Nº Guia de Residuos");
                row.CreateCell(17).SetCellValue("Nº Guia Corrigida");
                row.CreateCell(18).SetCellValue("Data Consumo Guia Corrigida");
                row.CreateCell(19).SetCellValue("Destino Final Residuos");
                row.CreateCell(20).SetCellValue("Tipo Refeição");
                row.CreateCell(21).SetCellValue("Nº Documento");
                row.CreateCell(22).SetCellValue("Localização");
                row.CreateCell(23).SetCellValue("Custo Unitário");
                row.CreateCell(24).SetCellValue("Custo Total");
                row.CreateCell(25).SetCellValue("Região");
                row.CreateCell(26).SetCellValue("Área Funcional");
                row.CreateCell(27).SetCellValue("Centro Responsabilidade");
                row.CreateCell(28).SetCellValue("Nº Projeto");
                row.CreateCell(29).SetCellValue("Nº Movimento");
                row.CreateCell(30).SetCellValue("Fatura-a Nº Cliente");
                row.CreateCell(31).SetCellValue("Nome Cliente");

                int count = 1;
                foreach (ProjectMovementViewModel item in Lista)
                {
                    row = excelSheet.CreateRow(count);

                    row.CreateCell(0).SetCellValue(item.Date);
                    row.CreateCell(1).SetCellValue(item.MovementType.ToString());
                    row.CreateCell(2).SetCellValue(item.Type.ToString());
                    row.CreateCell(3).SetCellValue(item.Code);
                    row.CreateCell(4).SetCellValue(item.Description);
                    if (item.Quantity != null) row.CreateCell(5).SetCellValue((double)item.Quantity);
                    row.CreateCell(6).SetCellValue(item.MeasurementUnitCode);
                    if (item.UnitPrice != null) row.CreateCell(7).SetCellValue((double)item.UnitPrice);
                    if (item.TotalPrice != null) row.CreateCell(8).SetCellValue((double)item.TotalPrice);
                    row.CreateCell(9).SetCellValue(item.Billable.ToString());
                    row.CreateCell(10).SetCellValue(item.ResourceType.ToString());
                    row.CreateCell(11).SetCellValue(item.ServiceClientCode);
                    row.CreateCell(12).SetCellValue(item.ServiceClientDescription);
                    row.CreateCell(13).SetCellValue(item.ServiceGroupCode);
                    row.CreateCell(14).SetCellValue(item.ExternalGuideNo);
                    row.CreateCell(15).SetCellValue(item.ConsumptionDate);
                    row.CreateCell(16).SetCellValue(item.ResidueGuideNo);
                    row.CreateCell(17).SetCellValue(item.AdjustedDocument);
                    row.CreateCell(18).SetCellValue(item.AdjustedDocumentDate);
                    row.CreateCell(19).SetCellValue(item.ResidueFinalDestinyCode.ToString());
                    row.CreateCell(20).SetCellValue(item.MealType.ToString());
                    row.CreateCell(21).SetCellValue(item.DocumentNo);
                    row.CreateCell(22).SetCellValue(item.LocationCode);
                    if (item.UnitCost != null) row.CreateCell(23).SetCellValue((double)item.UnitCost);
                    if (item.TotalCost != null) row.CreateCell(24).SetCellValue((double)item.TotalCost);
                    row.CreateCell(25).SetCellValue(item.RegionCode);
                    row.CreateCell(26).SetCellValue(item.FunctionalAreaCode);
                    row.CreateCell(27).SetCellValue(item.ResponsabilityCenterCode);
                    row.CreateCell(28).SetCellValue(item.ProjectNo);
                    row.CreateCell(29).SetCellValue(item.LineNo);
                    row.CreateCell(30).SetCellValue(item.InvoiceToClientNo);
                    row.CreateCell(31).SetCellValue(item.ClientName);

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
            sFileName = @"/Upload/temp/" + sFileName;
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Faturação de Projetos Detalhes.xlsx");
        }

        //1
        [HttpPost]
        public async Task<JsonResult> ExportToExcel_MovimentosProjetos([FromBody] List<ProjectDiaryViewModel> Lista)
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
                            row.CreateCell(Col).SetCellValue((double)item.Quantity);
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
                            row.CreateCell(Col).SetCellValue((double)(item.UnitCost??0));
                            Col = Col + 1;
                        }
                        if (dp["totalCost"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue((double)(item.TotalCost??0));
                            Col = Col + 1;
                        }
                        if (dp["unitPrice"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue((double)(item.UnitPrice??0));
                            Col = Col + 1;
                        }
                        if (dp["totalPrice"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue((double)(item.TotalPrice??0));
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
                            row.CreateCell(Col).SetCellValue((double)(item.UnitValueToInvoice??0));
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
            sFileName = @"/Upload/temp/" + sFileName;
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Movimentos de Projetos.xlsx");
        }

        //1
        [HttpPost]
        public async Task<JsonResult> ExportToExcel_PreMovimentosProjetos([FromBody] List<ProjectDiaryViewModel> Lista)
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
                            row.CreateCell(Col).SetCellValue((double)(item.Quantity??0));
                            Col = Col + 1;
                        }
                        if (dp["unitPrice"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue((double)(item.UnitPrice??0));
                            Col = Col + 1;
                        }
                        if (dp["totalPrice"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue((double)(item.TotalPrice??0));
                            Col = Col + 1;
                        }
                        if (dp["unitCost"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue((double)(item.UnitCost??0));
                            Col = Col + 1;
                        }
                        if (dp["totalCost"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue((double)(item.TotalCost??0));
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
                        //if (dp["serviceObject"]["hidden"].ToString() == "False")
                        //{
                        //    row.CreateCell(Col).SetCellValue(item.ServiceObject);
                        //    Col = Col + 1;
                        //}
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
            sFileName = @"/Upload/temp/" + sFileName;
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Pré-Movimentos de Projetos.xlsx");
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
                    data.TotalPrice = data.Quantity * data.UnitPrice;
                    data.UpdateUser = User.Identity.Name;
                    if (DBProjectMovements.Update(DBProjectMovements.ParseToDB(data)) != null)
                    {
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




    }
}