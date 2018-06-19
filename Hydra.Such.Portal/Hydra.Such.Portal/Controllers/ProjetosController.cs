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

namespace Hydra.Such.Portal.Controllers
{
    [Authorize]
    public class ProjetosController : Controller
    {
        private readonly NAVConfigurations _config;
        private readonly NAVWSConfigurations _configws;

        public ProjetosController(IOptions<NAVConfigurations> appSettings, IOptions<NAVWSConfigurations> NAVWSConfigs)
        {
            _config = appSettings.Value;
            _configws = NAVWSConfigs.Value;
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

            List<ProjectListItemViewModel> result = DBProjects.GetAllByAreaToList();

            if (!Ended)
            {
                result.RemoveAll(x => x.Status == 5);
            }

            result.ForEach(x =>
            {
                if (x.Status.HasValue)
                {
                    x.StatusDescription = EnumerablesFixed.ProjectStatus.Where(y => y.Id == x.Status).FirstOrDefault().Value;
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

        [HttpPost]
        public JsonResult GetByContract([FromBody] JObject requestParams)
        {
            string contractId = requestParams["contractId"].ToString();

            List<ProjectListItemViewModel> result = string.IsNullOrEmpty(contractId) ? new List<ProjectListItemViewModel>() : DBProjects.GetByContract(contractId);

            result.ForEach(x =>
            {
                if (x.Status.HasValue)
                {
                    x.StatusDescription = EnumerablesFixed.ProjectStatus.Where(y => y.Id == x.Status).FirstOrDefault().Value;
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
                        ProjectResponsible = cProject.ResponsávelProjeto
                    };

                    return Json(result);
                }
                ProjectDetailsViewModel finalr = new ProjectDetailsViewModel()
                {
                    Status = 1
                };
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
                        projNoAuto = DBNumerationConfigurations.GetNextNumeration(ProjectNumerationConfigurationId, autoGenId);
                        data.ProjectNo = projNoAuto;
                    }

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

                        //Create Project On Database
                        cProject = DBProjects.Create(cProject);

                        if (cProject == null)
                        {
                            data.eReasonCode = 3;
                            data.eMessage = "Ocorreu um erro ao criar o projeto no portal.";
                        }
                        else
                        {
                            //Create Project on NAV
                            Task<WSCreateNAVProject.Create_Result> TCreateNavProj = WSProject.CreateNavProject(data, _configws);
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
                                //Delete Created Project on Database
                                DBProjects.Delete(cProject.NºProjeto);

                                data.eReasonCode = 3;
                                data.eMessage = "Ocorreu um erro ao criar o projeto no NAV.";
                                if (TCreateNavProj.Exception != null)
                                    data.eMessages.Add(new TraceInformation(TraceType.Exception, TCreateNavProj.Exception.Message));
                                if(TCreateNavProj.Exception.InnerException != null)
                                    data.eMessages.Add(new TraceInformation(TraceType.Exception, TCreateNavProj.Exception.InnerException.ToString()));
                            }
                            else
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
                List<DiárioDeProjeto> Movements = DBProjectDiary.GetByProjectNo(data.ProjectNo, User.Identity.Name);
                Movements.RemoveAll(x => !x.Registado.Value);

                ErrorHandler result = new ErrorHandler();
                if (Movements.Count() > 0)
                {
                    result = new ErrorHandler()
                    {
                        eReasonCode = 1,
                        eMessage = "Já existem movimentos de projeto."
                    };
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
                        data.eReasonCode = 3;
                        data.eMessage = "Ocorreu um erro ao atualizar o projeto no NAV.";
                    }

                    if (TReadNavProj.IsCompletedSuccessfully)
                    {
                        Task<WSCreateNAVProject.Delete_Result> TDeleteNavProj = WSProject.DeleteNavProject(TReadNavProj.Result.WSJob.Key, _configws);
                        try
                        {
                            TDeleteNavProj.Wait();

                            if (!TDeleteNavProj.IsCompletedSuccessfully)
                            {
                                result.eReasonCode = 2;
                                result.eMessage = "Não é possivel remover o projeto no nav.";
                            }
                            else
                            {
                                DBProjects.Delete(data.ProjectNo);
                                result = new ErrorHandler()
                                {
                                    eReasonCode = 0,
                                    eMessage = "Projeto removido com sucesso."
                                };
                            }
                        }
                        catch (Exception ex)
                        {
                            result.eReasonCode = 2;
                            result.eMessage = "Não é possivel remover o projeto no nav.";
                        }
                    }

                }
                return Json(result);
            }
            return Json(false);
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
        public JsonResult GetAllProjectDiary([FromBody]string projectNo)
        {
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
                    ServiceGroupCode = x.CódGrupoServiço,
                    ResidueGuideNo = x.NºGuiaResíduos,
                    ExternalGuideNo = x.NºGuiaExterna,
                    ConsumptionDate = !x.DataConsumo.HasValue ? "" : x.DataConsumo.Value.ToString("yyyy-MM-dd"),
                    InvoiceToClientNo = x.FaturaANºCliente,
                    ServiceClientCode = x.CódServiçoCliente
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
                    ServiceGroupCode = x.CódGrupoServiço,
                    ResidueGuideNo = x.NºGuiaResíduos,
                    ExternalGuideNo = x.NºGuiaExterna,
                    ConsumptionDate = !x.DataConsumo.HasValue ? "" : x.DataConsumo.Value.ToString("yyyy-MM-dd"),
                    InvoiceToClientNo = x.FaturaANºCliente,
                    ServiceClientCode = x.CódServiçoCliente
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
        public JsonResult CreatePDByMovProj([FromBody] List<ProjectDiaryViewModel> dp, string projectNo, string Resources, string ProjDiaryPrice)
        {
            ProjectDiaryResponse response = new ProjectDiaryResponse();
            string proj = dp.First().ProjectNo;
            string notCreatedLines = "";
            bool MoreThanOne = false;
            int OrderLine = 0;
            Projetos projecto = DBProjects.GetById(proj);
            if (dp != null)
                response.Items = dp;

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
                                            PréRegisto = false
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
                            PréRegisto = false

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
                    response.eMessage = "Occorreu um erro ao atualizar o Diário de Projeto.";
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
                ProjectInfo pi = new ProjectInfo
                {
                    //ProjectNo = proj.NºProjeto,
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
            //SET INTEGRATED IN DB
            if (dp != null)
            {
                Guid transactID = Guid.NewGuid();
                try
                {
                    //Create Lines in NAV
                    Task<WSCreateProjectDiaryLine.CreateMultiple_Result> TCreateNavDiaryLine = WSProjectDiaryLine.CreateNavDiaryLines(dp, transactID, _configws);
                    TCreateNavDiaryLine.Wait();

                    ////Register Lines in NAV
                    Task<WSGenericCodeUnit.FxPostJobJrnlLines_Result> TRegisterNavDiaryLine = WSProjectDiaryLine.RegsiterNavDiaryLines(transactID, _configws);
                    TRegisterNavDiaryLine.Wait();

                    if (TRegisterNavDiaryLine == null)
                    {
                        Response.StatusCode = (int)HttpStatusCode.NoContent;
                        return Json(dp);
                    }
                }
                catch (Exception ex)
                {
                    Response.StatusCode = (int)HttpStatusCode.NoContent;
                    return Json(dp);
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
                                FaturaçãoAutorizada = false
                            };

                            DBProjectMovements.Create(ProjectMovement);
                        }
                    }
                });
            }
            return Json(dp);
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
        public JsonResult GetMovements([FromBody] string projectNo)
        {
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
                        dp = DBContractLines.GetAllByActiveContract(lcontracts.NºDeContrato, lcontracts.NºVersão).Select(
                            x => new DiárioDeProjeto()
                            {
                                NºProjeto = projectNo,
                                Tipo = x.Tipo,
                                Código = x.Código,
                                Descrição = x.Descrição,
                                Quantidade = 0,
                                CódUnidadeMedida = x.CódUnidadeMedida,
                                CódigoRegião = x.CódigoRegião,
                                CódigoÁreaFuncional = x.CódigoÁreaFuncional,
                                CódigoCentroResponsabilidade = x.CódigoCentroResponsabilidade,
                                Utilizador = User.Identity.Name,
                                PreçoUnitário = x.PreçoUnitário,
                                Faturável = x.Faturável,
                                Registado = false,
                                PréRegisto = false
                            }).ToList();
                        if (dp.Count == 0)
                        {
                            result.eReasonCode = 4;
                            result.eMessage = "Este projeto não tem contrato com linhas associadas";
                        }
                        foreach (var item in dp)
                        {

                            DiárioDeProjeto dpValidation = new DiárioDeProjeto();
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
            List<ProjectDiaryViewModel> dp = DBProjectMovements.GetRegisteredDiary(ProjectNo).Select(x => new ProjectDiaryViewModel()
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
                FolhaHoras = x.NºDocumento,
                InvoiceToClientNo = x.FaturaANºCliente,
                ClientName = DBNAV2017Clients.GetClientNameByNo(x.FaturaANºCliente, _config.NAVDatabaseName, _config.NAVCompanyName),

            }).ToList();

            return Json(dp);
        }

        [HttpPost]
        public JsonResult GetProjectMovementsDp([FromBody] string ProjectNo, bool allProjs, string projectTarget, string NoDocument, string Resources, string Date, string ProjDiaryPrice)
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
                ConsumptionDate = x.DataConsumo == null ? String.Empty : x.DataConsumo.Value.ToString("yyyy-MM-dd")
            }).ToList();
            if (!string.IsNullOrEmpty(NoDocument))
            {
                dp = dp.Where(x => x.DocumentNo == NoDocument).ToList();
            }
            if (!string.IsNullOrEmpty(Resources) && Resources != "undefined")
            {
                dp = dp.Where(x => x.Code == Resources).ToList();
            }
            if (!string.IsNullOrEmpty(Date))
            {
                foreach (ProjectDiaryViewModel item in dp)
                {
                    item.Date = Date;
                }
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
            return View();
        }

        [HttpPost]
        public JsonResult GetAutorizacaoFaturacao([FromBody]  JObject requestParams)
        {
            int areaId = int.Parse(requestParams["areaId"].ToString());
            string projectNo = requestParams["projectNo"].ToString();

            try
            {
                List<ProjectDiaryViewModel> result = DBProjectMovements.GetAllTableByAreaProjectNo(User.Identity.Name, areaId, projectNo).Select(x => new ProjectDiaryViewModel()
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
                    UnitValueToInvoice = x.ValorUnitárioAFaturar,
                    Currency = x.Moeda,
                    Billable = x.Faturável,
                    Billed = (bool)x.Faturada,
                    Registered = x.Registado,
                    InvoiceToClientNo = x.FaturaANºCliente,
                    CommitmentNumber = DBProjects.GetAllByProjectNumber(x.NºProjeto).NºCompromisso,
                    ClientName = DBNAV2017Clients.GetClientNameByNo(x.FaturaANºCliente, _config.NAVDatabaseName, _config.NAVCompanyName),
                    ClientVATReg = DBNAV2017Clients.GetClientVATByNo(x.FaturaANºCliente, _config.NAVDatabaseName, _config.NAVCompanyName)
                }).OrderBy(x => x.ClientName).ToList();

                if (result.Count > 0)
                {
                    var userDimensions = DBUserDimensions.GetByUserId(User.Identity.Name);
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
                    }
                    List<UserDimensionsViewModel> userDimensionsViewModel = userDimensions.ParseToViewModel();
                    if (userDimensionsViewModel.Where(x => x.Dimension == (int)Dimensions.Region).Count() > 0)
                        result.RemoveAll(x => !userDimensionsViewModel.Any(y => y.DimensionValue == x.RegionCode));
                    if (userDimensionsViewModel.Where(x => x.Dimension == (int)Dimensions.FunctionalArea).Count() > 0)
                        result.RemoveAll(x => !userDimensionsViewModel.Any(y => y.DimensionValue == x.FunctionalAreaCode));
                    if (userDimensionsViewModel.Where(x => x.Dimension == (int)Dimensions.ResponsabilityCenter).Count() > 0)
                        result.RemoveAll(x => !userDimensionsViewModel.Any(y => y.DimensionValue == x.ResponsabilityCenterCode));
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        private class InvoiceMessages
        {
            public bool Iserror { get; set; }
            public string ClientNo { get; set; }
        }

        [HttpPost]
        public JsonResult InvoiceLinesAuthorize([FromBody] List<ProjectDiaryViewModel> data)
        {
            try
            {
                if (data != null)
                {
                    foreach (var updatedata in data)
                    {
                        MovimentosDeProjeto lines = DBProjectMovements.GetByLineNo(updatedata.LineNo).FirstOrDefault();
                        lines.FaturaçãoAutorizada = true;
                        lines.DataAutorizaçãoFaturação = DateTime.Now;
                        DBProjectMovements.Update(lines);
                    }

                    return Json(data);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }
        #endregion InvoiceAutorization

        #region Invoice
        public IActionResult Faturacao()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetMovimentosFaturacao()
        {
            try
            {
                List<SPInvoiceListViewModel> result = DBProjectMovements.GetAllAutorized().OrderBy(x => x.ClientName).ToList();
                List<NAVClientsViewModel> ClientList = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, null);

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
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpPost]
        public JsonResult CreateInvoiceLines([FromBody] List<SPInvoiceListViewModel> data)
        {
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


                if (groupedbyclient != null)
                {
                    foreach (var header in groupedbyclient)
                    {
                        Task<WSCreatePreInvoice.Create_Result> TCreatePreInvoice = WSPreInvoice.CreatePreInvoice(header, _configws);
                        TCreatePreInvoice.Wait();

                        if (TCreatePreInvoice.IsCompletedSuccessfully)
                        {
                            string HeaderNo = TCreatePreInvoice.Result.WSPreInvoice.No;

                            List<SPInvoiceListViewModel> linesList = new List<SPInvoiceListViewModel>();

                            foreach (var lines in data)
                            {
                                if (lines.InvoiceToClientNo == header.InvoiceToClientNo && lines.Date == header.Date && lines.CommitmentNumber == header.CommitmentNumber && lines.ClientRequest == header.ClientRequest)
                                {
                                    linesList.Add(lines);
                                }
                            }

                            Task<WSCreatePreInvoiceLine.CreateMultiple_Result> TCreatePreInvoiceLine = WSPreInvoiceLine.CreatePreInvoiceLineListProject(linesList, HeaderNo, _configws);
                            TCreatePreInvoiceLine.Wait();

                            if (TCreatePreInvoiceLine.IsCompletedSuccessfully)
                            {
                                //update to Invoiced = true
                                foreach (var updatelist in linesList)
                                {
                                    MovimentosDeProjeto mov = DBProjectMovements.GetByLineNo(updatelist.LineNo).FirstOrDefault();
                                    mov.Faturada = true;
                                    DBProjectMovements.Update(mov);
                                }
                            }
                        }
                    }
                }
            }
            return Json(data);
        }
        #endregion

        #region Pre registo de Projetos
        public IActionResult PreMovimentosProjetos(string id)
        {
            UserAccessesViewModel userAccesses = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Features.Projetos);
            if (userAccesses != null && userAccesses.Read.Value)
            {
                if (id != null)
                {
                    ViewBag.ProjectNo = id ?? "";
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
                FolhaHoras = x.NºDocumento,
                InvoiceToClientNo = x.FaturaANºCliente,
                ClientName = DBNAV2017Clients.GetClientNameByNo(x.FaturaANºCliente, _config.NAVDatabaseName, _config.NAVCompanyName),

            }).ToList();

            return Json(dp);
        }
        public IActionResult PreregistoProjetos(String id)
        {
             UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, Enumerations.Areas.Administração, Enumerations.Features.PreRegistos);
            if (UPerm != null && UPerm.Read.Value)
            {
                ViewBag.UPermissions = UPerm;
                ViewBag.ProjectNo = id ?? "";
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        [HttpPost]
        public JsonResult CreatePDByMovPreProj([FromBody] List<ProjectDiaryViewModel> dp, string projectNo, string Resources, string ProjDiaryPrice)
        {
            ProjectDiaryResponse response = new ProjectDiaryResponse();
            string proj = dp.First().ProjectNo;
            string notCreatedLines = "";
            bool MoreThanOne = false;
            int OrderLine = 0;
            Projetos projecto = DBProjects.GetById(proj);
            if (dp != null)
                response.Items = dp;

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
        public JsonResult GetPreMovements([FromBody] string projectNo)
        {
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
                        dp = DBContractLines.GetAllByActiveContract(lcontracts.NºContrato, lcontracts.NºVersão).Select(
                            x => new DiárioDeProjeto()
                            {
                                NºProjeto = projectNo,
                                Tipo = x.Tipo,
                                Código = x.Código,
                                Descrição = x.Descrição,
                                Quantidade = 0,
                                CódUnidadeMedida = x.CódUnidadeMedida,
                                CódigoRegião = x.CódigoRegião,
                                CódigoÁreaFuncional = x.CódigoÁreaFuncional,
                                CódigoCentroResponsabilidade = x.CódigoCentroResponsabilidade,
                                Utilizador = User.Identity.Name,
                                PreçoUnitário = x.PreçoUnitário,
                                Faturável = x.Faturável,
                                Registado = false,
                                PréRegisto = true
                            }).ToList();
                        if (dp.Count == 0)
                        {
                            result.eReasonCode = 4;
                            result.eMessage = "Este projeto não tem contrato com linhas associadas";
                        }
                        foreach (var item in dp)
                        {

                            DiárioDeProjeto dpValidation = new DiárioDeProjeto();
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
                dp.ForEach(x =>
                {
                    if (x.Code != null)
                    {
                        DiárioDeProjeto newdp = DBProjectDiary.GetAllPreRegByCode(User.Identity.Name, x.Code);
                        if (newdp != null)
                        {
                            DBProjectDiary.Delete(newdp);

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
                                FaturaçãoAutorizada = false
                            };

                            DBPreProjectMovements.CreatePreRegist(ProjectMovement);
                        }
                    }
                });
            }
            return Json(dp);
        }
         [HttpPost]
        public JsonResult RegisterPreMovements([FromBody]  List<ProjectDiaryViewModel> dp, string StartDate, string EndDate)
        {
            ErrorHandler erro = new ErrorHandler();
            erro.eReasonCode = 1;
            erro.eMessage = "Movimentos registados com Sucesso";
            List<ProjectDiaryViewModel> premov = new List<ProjectDiaryViewModel>();

            if (dp != null && dp.Count>0 && !string.IsNullOrEmpty(StartDate)  && !string.IsNullOrEmpty(EndDate))
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
                if (premov != null && premov.Count>0)
                {
                    Guid transactID = Guid.NewGuid();
                    try
                    {
                        //Create Lines in NAV
                        Task<WSCreateProjectDiaryLine.CreateMultiple_Result> TCreateNavDiaryLine = WSProjectDiaryLine.CreateNavDiaryLines(premov, transactID, _configws);
                        TCreateNavDiaryLine.Wait();

                        //Register Lines in NAV
                        Task<WSGenericCodeUnit.FxPostJobJrnlLines_Result> TRegisterNavDiaryLine = WSProjectDiaryLine.RegsiterNavDiaryLines(transactID, _configws);
                        TRegisterNavDiaryLine.Wait();

                        if (TRegisterNavDiaryLine == null)
                        {
                             erro.eReasonCode = 3;
                             erro.eMessage = "Não foi possivel criar as linhas no nav";
                            Response.StatusCode = (int)HttpStatusCode.NoContent;
                            return Json(premov);
                        }
                    }
                    catch (Exception ex)
                    {
                        Response.StatusCode = (int)HttpStatusCode.NoContent;
                        return Json(premov);
                    }

                    var PreRegistGrouped = premov.GroupBy(x => new { x.ProjectNo, x.Code, x.ServiceGroupCode, x.ServiceClientCode },
                     x => x,
                     (Key, items) => new {
                         ProjectNo = Key.ProjectNo,
                         Code = Key.Code,
                         ServiceGroupCode = Key.ServiceGroupCode,
                         ServiceClientCode = Key.ServiceClientCode,
                         Items = items,
                     }).ToList();
                    foreach (var item in PreRegistGrouped)
                    {
                        List<ProjectDiaryViewModel> nwl = new List<ProjectDiaryViewModel>();
                        MovimentosDeProjeto ProjectMovement = new MovimentosDeProjeto();
                        if (item.Items.ToList().Count > 0)
                        {
                            nwl = item.Items.ToList();
                        }
                        List<int> premovId = new List<int>();
                        foreach (ProjectDiaryViewModel preReg in nwl)
                        {
                            ProjectMovement.NºProjeto = item.ProjectNo;
                            ProjectMovement.Data = Convert.ToDateTime(EndDate);
                            if (ProjectMovement.Quantidade == null)
                            {
                                ProjectMovement.Quantidade = 0;
                            }
                            if (preReg.Quantity == null)
                            {
                                preReg.Quantity = 0;
                            }
                            if (ProjectMovement.Quantidade == 0)
                            {
                                ProjectMovement.Quantidade = (decimal)preReg.Quantity;
                            }
                            else
                            {
                                ProjectMovement.Quantidade = ProjectMovement.Quantidade + (decimal)preReg.Quantity;
                            }
                            ProjectMovement.TipoMovimento = preReg.MovementType;
                            ProjectMovement.Tipo = preReg.Type;
                            ProjectMovement.Código = item.Code;
                            ProjectMovement.Descrição = preReg.Description;
                            ProjectMovement.CódUnidadeMedida = preReg.MeasurementUnitCode;
                            ProjectMovement.CódLocalização = preReg.LocationCode;
                            ProjectMovement.GrupoContabProjeto = preReg.ProjectContabGroup;
                            ProjectMovement.CódigoRegião = preReg.RegionCode;
                            ProjectMovement.CódigoÁreaFuncional = preReg.FunctionalAreaCode;
                            ProjectMovement.CódigoCentroResponsabilidade = preReg.ResponsabilityCenterCode;
                            ProjectMovement.Utilizador = User.Identity.Name;
                            ProjectMovement.CustoUnitário = preReg.UnitCost;
                            ProjectMovement.CustoTotal = ProjectMovement.Quantidade * preReg.UnitCost;
                            ProjectMovement.PreçoUnitário = preReg.UnitPrice;
                            ProjectMovement.PreçoTotal = ProjectMovement.Quantidade * preReg.UnitPrice;
                            ProjectMovement.Faturável = preReg.Billable;
                            ProjectMovement.Registado = true;
                            ProjectMovement.Faturada = false;
                            ProjectMovement.FaturaANºCliente = preReg.InvoiceToClientNo;
                            ProjectMovement.Moeda = preReg.Coin;
                            ProjectMovement.ValorUnitárioAFaturar = preReg.UnitValueToInvoice;
                            ProjectMovement.TipoRefeição = preReg.MealType;
                            ProjectMovement.CódGrupoServiço = item.ServiceGroupCode;
                            ProjectMovement.NºGuiaResíduos = preReg.ResidueGuideNo;
                            ProjectMovement.NºGuiaExterna = preReg.ExternalGuideNo;
                            ProjectMovement.DataConsumo = preReg.ConsumptionDate != "" && preReg.ConsumptionDate != null ? DateTime.Parse(preReg.ConsumptionDate) : (DateTime?)null;
                            ProjectMovement.CódServiçoCliente = item.ServiceClientCode;
                            ProjectMovement.UtilizadorCriação = User.Identity.Name;
                            ProjectMovement.DataHoraCriação = DateTime.Now;
                            ProjectMovement.FaturaçãoAutorizada = false;
                            premovId.Add(preReg.LineNo);
                        }
                         MovimentosDeProjeto CreatedMovProj = DBProjectMovements.Create(ProjectMovement);
                        if (CreatedMovProj.NºLinha > 0 && premovId.Count > 0)
                        {
                            foreach (int pmid in premovId)
                            {
                                PréMovimentosProjeto preMovLine = DBPreProjectMovements.GetByLine(pmid);
                                if (preMovLine != null)
                                {
                                     preMovLine.Registado = true;
                                     preMovLine.UtilizadorCriação = User.Identity.Name;
                                     DBPreProjectMovements.Update(preMovLine);
                                }
                            }
                        }
                    }
                }
                else if (dp != null)
                {
                     erro.eReasonCode = 2;
                     erro.eMessage = "Não existe Pré-Movimentos por registar no intervalo de tempo selecionado";
                }
            }
            else
            {
                erro.eReasonCode = 2;
                erro.eMessage = "A tabela Pré-Movimentos está vazia";
            }
            return Json(erro);
        }
        #endregion
    }
}