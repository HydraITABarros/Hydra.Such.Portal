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
                        projNoAuto = DBNumerationConfigurations.GetNextNumeration(ProjectNumerationConfigurationId, autoGenId, false);
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
                                if (TCreateNavProj.Exception.InnerException != null)
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
        public JsonResult GetAllProjectDiary([FromBody]  JObject requestParams)
        {
            string projectNo = "";
            string dataReque = "";
            string codServiceCliente = "";
            int codServiceGroup = 0;
            if (requestParams != null)
            {
                projectNo = (requestParams["noproj"] != null) ? requestParams["noproj"].ToString() : "";
                dataReque = (requestParams["data"] != null) ? requestParams["data"].ToString() : "";
                codServiceCliente = (requestParams["codClienteServico"] != null) ? requestParams["codClienteServico"].ToString() : "";
                if (requestParams["codGrupoServico"] != null)
                {
                    codServiceGroup = (requestParams["codGrupoServico"].ToString() != "") ? Convert.ToInt32(requestParams["codGrupoServico"].ToString()) : 0;
                }
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
                    ServiceGroupCode =  codServiceGroup,
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
                string ClientName ="";
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
        public JsonResult GetMovements([FromBody]  JObject requestParams)
        {
            string projectNo = "";
            string dataReque = "";
            string codServiceCliente = "";
            int codServiceGroup = 0;
            if (requestParams != null)
            {
                projectNo = (requestParams["noproj"] != null) ? requestParams["noproj"].ToString() : "";
                dataReque = (requestParams["data"] != null) ? requestParams["data"].ToString() : "";
                codServiceCliente = (requestParams["codClienteServico"] != null) ? requestParams["codClienteServico"].ToString() : "";
                if (requestParams["codGrupoServico"] != null)
                {
                    codServiceGroup = (requestParams["codGrupoServico"].ToString() != "") ? Convert.ToInt32(requestParams["codGrupoServico"].ToString()) : 0;
                }
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
                MealType = x.TipoRefeição,
                UnitCost = x.CustoUnitário,
                TotalCost = x.CustoTotal,
                UnitPrice = x.PreçoUnitário,
                TotalPrice = x.PreçoTotal,
                Billable = x.Faturável,
                Registered = x.Registado,
                FolhaHoras = x.NºDocumento,
                InvoiceToClientNo = x.FaturaANºCliente,
                ClientName = DBNAV2017Clients.GetClientNameByNo(x.FaturaANºCliente, _config.NAVDatabaseName, _config.NAVCompanyName)

            }).ToList();
            foreach (ProjectDiaryViewModel item in dp)
            {
                if (item.MealType != null)
                {
                    TiposRefeição TRrow = DBMealTypes.GetById(item.MealType.Value);
                    if (TRrow != null)
                    {
                        item.MealTypeDescription = TRrow.Descrição;
                    }
                }
                else
                {
                    item.MealTypeDescription = "";
                }
                
            }

            return Json(dp);
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
            
            ProjectBillingAuthorization p = new ProjectBillingAuthorization(projectNo, User.Identity.Name, _config.NAVDatabaseName, _config.NAVCompanyName);
            if (project != null)
            {
                try
                {
                    var projectContract = DBContracts.GetByIdLastVersion(project.NºContrato);

                    List<ProjectMovementViewModel> projectMovements = GetProjectMovements(projectNo, billable);

                    if (project.Estado.HasValue && (project.Estado == 3 || project.Estado == 4))
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
        
        private List<ProjectMovementViewModel> GetProjectMovements(string projectNo, bool? billable)
        {
            List<ProjectMovementViewModel> projectMovements = DBProjectMovements.GetProjectMovementsFor(projectNo, billable)
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
            return projectMovements;
        }

        [HttpPost]
        public JsonResult GetProjectBillingResume([FromBody] JObject requestParams)
        {
            string projectNo = requestParams["projectNo"].ToString();

            List<ProjectMovementViewModel> projectMovements = GetProjectMovements(projectNo, null);
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

        [HttpPost]
        public JsonResult UpdateProjectMovements([FromBody] List<ProjectMovementViewModel> projectMovements)
        {
            Result result = new Result();

            string projectNo = projectMovements.Select(x => x.ProjectNo).FirstOrDefault();
            List<int> movementIds = projectMovements.Select(x => x.LineNo).ToList();

            var movementsToUpdate = DBProjectMovements.GetProjectMovementsFor(projectNo, true);
            if (movementsToUpdate != null)
                movementsToUpdate.RemoveAll(x => !movementIds.Contains(x.NºLinha));


            movementsToUpdate.ForEach(x =>
            {
                var item = projectMovements.First(y => y.LineNo == x.NºLinha);
                x.PreçoUnitário = item.UnitPrice;
                x.Faturável = item.Billable;
                x.TipoRecurso = item.ResourceType;
                x.CódServiçoCliente = item.ServiceClientCode;
                x.CódGrupoServiço = item.ServiceGroupCode;
                x.NºGuiaExterna = item.ExternalGuideNo;
                x.DataConsumo = string.IsNullOrEmpty(item.ConsumptionDate) ? (DateTime?)null : DateTime.Parse(item.ConsumptionDate);
                x.NºGuiaResíduos = item.ResidueGuideNo;
                x.FaturaçãoAutorizada2 = item.AutorizatedInvoice2;
                x.TipoRefeição = item.MealType;
            });
            var updatedMovements = DBProjectMovements.Update(movementsToUpdate);
            if (updatedMovements != null)
            {
                result.eReasonCode = 1;
                result.eMessage = "Movimentos atualizados com sucesso.";
                result.Value = updatedMovements.ParseToViewModel(_config.NAVDatabaseName, _config.NAVCompanyName);
            }
            else
            {
                result.eReasonCode = 2;
                result.eMessage = "Não foi possivel atualizar os movimentos.";
                result.Value = projectMovements;
            }
            return Json(result);
        }

        private class InvoiceMessages
        {
            public bool Iserror { get; set; }
            public string ClientNo { get; set; }
        }

        [HttpPost]
        public JsonResult AuthorizeProjectMovements([FromBody] JObject requestParams)//[FromBody] List<ProjectDiaryViewModel> data
        {
            ErrorHandler result = ValidateMovements(requestParams);
            if (result.eReasonCode != 1)
                return Json(result);

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

                DateTime serviceDate = DateTime.MinValue;
                JValue serviceDateValue = requestParams["serviceDate"] as JValue;
                if (serviceDateValue != null)
                    DateTime.TryParse((string)serviceDateValue.Value, out serviceDate);

                decimal authorizationTotal;
                JValue authorizationTotalValue = requestParams["authorizationTotalValue"] as JValue;
                if (authorizationTotalValue != null)
                {
                    string str = (string)authorizationTotalValue.Value;
                    authorizationTotal = decimal.Parse(str, CultureInfo.InvariantCulture);
                }

                List<ProjectMovementViewModel> projMovements = new List<ProjectMovementViewModel>();
                JArray projMovementsValue = requestParams["projMovements"] as JArray;
                if (projMovementsValue != null)
                    projMovements = projMovementsValue.ToObject<List<ProjectMovementViewModel>>();

                Projetos project = null;
                Contratos contract = null;
                NAVClientsViewModel customer = null;

                if (!string.IsNullOrEmpty(projectNo))
                    project = DBProjects.GetById(projectNo);

                if (project != null)
                {
                    contract = DBContracts.GetByIdLastVersion(project.NºContrato);
                    customer = DBNAV2017Clients.GetClientById(_config.NAVDatabaseName, _config.NAVCompanyName, project.NºCliente);

                    //Se o “Pedido” não estiver preenchido no projeto, preenche o “Pedido” e “Data do Pedido” com a informação que estiver indicada no Contrato/ Requisições do Contrato;
                    //Se o campo “Observações” não estiver preenchido, vai buscar esta informação ao Contrato/ Texto Faturas Contrato;
                    //Preenche as Dimensões (Região, Área e CResp) com as dimensões do projeto, exceto no caso do Cliente não ser nacional, que preenche a Região com a Região do Cliente.
                    //No caso do projeto estar relacionado com um contrato, o “Código Termos de Pagamento” é o que estiver indicado no Contrato.Se não, preenche com o que estiver configurado na ficha do Cliente.
                    //O campo “Método de Pagamento” é o que estiver configurado na ficha do Cliente.
                    //Utilizador
                    //Data de Autorização

                    SuchDBContext ctx = new SuchDBContext();
                    int? lastUsed = ctx.ProjectosAutorizados
                        .Where(x => x.CodProjeto == projectNo)
                        .Select(x => x.GrupoFactura)
                        .Max();
                    int invoiceGroup = lastUsed.HasValue ? lastUsed.Value + 1 : 1;
                    ProjectosAutorizados authorizedProject = new ProjectosAutorizados();
                    authorizedProject.CodProjeto = project.NºProjeto;
                    authorizedProject.GrupoFactura = invoiceGroup;
                    authorizedProject.DescricaoGrupo = invoiceGroupDescription;
                    authorizedProject.NumCompromisso = commitmentNumber;
                    authorizedProject.CodCliente = project.NºCliente;
                    authorizedProject.CodContrato = contract?.NºDeContrato;
                    authorizedProject.DataAutorizacao = DateTime.Now;
                    authorizedProject.CodTermosPagamento = contract != null ? contract.CódTermosPagamento : customer?.PaymentTermsCode;
                    authorizedProject.CodMetodoPagamento = customer?.PaymentTermsCode;
                    authorizedProject.CodRegiao = !customer.National ? customer.RegionCode : project.CódigoRegião;
                    authorizedProject.CodAreaFuncional = project.CódigoÁreaFuncional;
                    authorizedProject.CodCentroResponsabilidade = authorizedProject.CodCentroResponsabilidade;
                    authorizedProject.PedidoCliente = customerRequestNo;
                    //proj.DataPedido Não definido
                    if(serviceDate > DateTime.MinValue)
                        authorizedProject.DataPrestacaoServico = serviceDate;

                    projMovements.ForEach(x =>
                    {
                        x.AutorizatedInvoice = true;
                        x.AutorizatedInvoiceDate = DateTime.Now.ToString("yyyy-MM-dd");
                        x.AuthorizedBy = User.Identity.Name;
                        x.InvoiceGroup = invoiceGroup;
                    });

                    ctx.ProjectosAutorizados.Add(authorizedProject);
                    ctx.MovimentosDeProjeto.UpdateRange(projMovements.ParseToDB());

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
            return Json(result);// null;
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
                    string str = (string)authorizationTotalValue.Value;
                    authorizationTotal = decimal.Parse(str, CultureInfo.InvariantCulture);
                }

                List<ProjectMovementViewModel> projMovements = new List<ProjectMovementViewModel>();
                JArray projMovementsValue = requestParams["projMovements"] as JArray;
                if (projMovementsValue != null)
                    projMovements = projMovementsValue.ToObject<List<ProjectMovementViewModel>>();

                Projetos project = null;
                Contratos contract = null;

                if (!string.IsNullOrEmpty(projectNo))
                {
                    project = DBProjects.GetById(projectNo);
                    if (project != null)
                        contract = DBContracts.GetByIdLastVersion(project.NºContrato);
                }
                
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
                    if (string.IsNullOrEmpty(commitmentNumber))
                    {
                        string customerNo = project.NºCliente;
                        var customers = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, customerNo);
                        if (customers != null)
                        {
                            var customer = customers.FirstOrDefault();
                            if (customer != null)
                            {
                                if (customer.UnderCompromiseLaw)
                                    result.eMessages.Add(new TraceInformation(TraceType.Error, "O cliente está ao abrigo da lei de compromissos."));
                                else
                                    result.eMessages.Add(new TraceInformation(TraceType.Warning, "Não foi indicado um Nº do Compromisso."));
                            }
                            else
                                result.eMessages.Add(new TraceInformation(TraceType.Error, "Ocorreu um erro ao validar o cliente.")); 
                        }
                        else
                            result.eMessages.Add(new TraceInformation(TraceType.Error, "Ocorreu um erro ao validar o cliente."));
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
            bool settedFromProjectOrContract = false;
            Projetos project;
            Contratos contract = null;


            string projectNo = requestParams["projectNo"].ToString();
            string serviceDeliveryValue = requestParams["serviceDeliveryDate"].ToString();
            DateTime serviceDeliveryDate = DateTime.Parse(serviceDeliveryValue);

            project = DBProjects.GetById(projectNo);
            if (project != null)
                contract = DBContracts.GetByIdLastVersion(project.NºContrato);

            if (project == null || contract == null)
            {
                result.eReasonCode = 2;
                result.eMessage = "Não foi possivel obter dados do projeto e/ou contrato.";
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
                    if (!string.IsNullOrEmpty(contract.NºCompromisso))
                    {
                        commitmentNo = contract.NºCompromisso;
                        settedFromProjectOrContract = true;
                    }
                }
                if (!string.IsNullOrEmpty(project.PedidoDoCliente))
                    customerRequestNo = project.PedidoDoCliente;
                else
                {
                    if (!string.IsNullOrEmpty(contract.NºRequisiçãoDoCliente))
                        customerRequestNo = contract.NºRequisiçãoDoCliente;
                }

                if (string.IsNullOrEmpty(commitmentNo) || string.IsNullOrEmpty(customerRequestNo))
                {
                    //Get from Contract Client Requisition
                    var contractReq = DBContractClientRequisition.GetByContract(contract.NºDeContrato);
                    if (contractReq != null)
                    {
                        var billingGroupReq = contractReq.Where(x => x.GrupoFatura == 0 && x.DataInícioCompromisso <= serviceDeliveryDate);
                        var projectReq = billingGroupReq.Where(x => x.NºProjeto == project.NºProjeto);

                        if (string.IsNullOrEmpty(commitmentNo))
                            commitmentNo = projectReq.FirstOrDefault(x => x.DataFimCompromisso >= serviceDeliveryDate && !string.IsNullOrEmpty(x.NºCompromisso))?.NºCompromisso;
                        if (string.IsNullOrEmpty(customerRequestNo))
                            customerRequestNo = projectReq.FirstOrDefault(x => x.DataFimCompromisso >= serviceDeliveryDate && !string.IsNullOrEmpty(x.NºRequisiçãoCliente))?.NºRequisiçãoCliente;

                        if (string.IsNullOrEmpty(commitmentNo) || string.IsNullOrEmpty(customerRequestNo))
                        {
                            if (string.IsNullOrEmpty(commitmentNo))
                                commitmentNo = billingGroupReq.FirstOrDefault(x => x.DataFimCompromisso >= serviceDeliveryDate && !string.IsNullOrEmpty(x.NºCompromisso))?.NºCompromisso;
                            if (string.IsNullOrEmpty(customerRequestNo))
                                customerRequestNo = billingGroupReq.FirstOrDefault(x => x.DataFimCompromisso >= serviceDeliveryDate && !string.IsNullOrEmpty(x.NºRequisiçãoCliente))?.NºRequisiçãoCliente;
                        }
                    }
                }
                result.eReasonCode = 1;
            }
            commitmentDetails.commitmentNo = commitmentNo;
            commitmentDetails.customerRequestNo = customerRequestNo;
            commitmentDetails.settedFromProjectOrContract = settedFromProjectOrContract;
            result.Value = commitmentDetails;

            return Json(result);
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
                List<SPInvoiceListViewModel> result = DBProjectMovements.GetAllAutorized();
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
        public JsonResult ValidationCliente([FromBody] List<SPInvoiceListViewModel> data, string OptionInvoice)
        {
            foreach (SPInvoiceListViewModel line in data)
            {
                //List<NAVClientsViewModel> Cliente = DBNAV2017Clients.GetClients(_config.NAVDatabaseName, _config.NAVCompanyName, line.InvoiceToClientNo);
                //if (Cliente != null)
                //{
                //    if (Cliente[0].)
                //    {
                //        if(Cliente[0].)
                //    }
                //        999999999
                //    if (line.CommitmentNumber == "")
                //    {

                //        if (Cliente[0].UnderCompromiseLaw == true)
                //        {
                //            execDetails += "Este cliente está ao abrigo da lei do compromisso. É obigatório o preenchimento do Nº de Compromisso ";
                //            result.eMessages.Add(new TraceInformation(TraceType.Exception, execDetails));
                //        }
                //        else
                //        {
                //            execDetails += "Não indicou Nº Compromisso. Deseja continuar?";
                //            result.eMessages.Add(new TraceInformation(TraceType.Warning, execDetails));
                //        }
                //    }
                //}
            }
            return null;
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
                    ProjectDetailsViewModel proj = new ProjectDetailsViewModel();
                    proj.ProjectNo = data[0].ProjectNo;
                    proj.ClientNo = data[0].InvoiceToClientNo;
                    proj.RegionCode = data[0].RegionCode;
                    proj.ResponsabilityCenterCode = data[0].ResponsabilityCenterCode;
                    proj.FunctionalAreaCode = data[0].FunctionalAreaCode;
                    Task<WSCreateNAVProject.Create_Result> createProject = WSProject.CreateNavProject(proj, _configws);
                    createProject.Wait();
                }

                if (groupedbyclient != null)
                {

                    foreach (var header in groupedbyclient)
                    {
                        try
                        {
                            
                            header.MovementType = Convert.ToInt32(OptionInvoice);
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
                MealType = x.TipoRefeição,
                FolhaHoras = x.NºDocumento,
                InvoiceToClientNo = x.FaturaANºCliente,
                ServiceClientCode = x.CódServiçoCliente,
                ClientName = DBNAV2017Clients.GetClientNameByNo(x.FaturaANºCliente, _config.NAVDatabaseName, _config.NAVCompanyName),

            }).ToList();
            foreach (ProjectDiaryViewModel item in dp)
            {
                if (!String.IsNullOrEmpty(item.ServiceClientCode))
                {
                    Serviços GetService = DBServices.GetById(item.ServiceClientCode);
                    if (GetService != null)
                    {
                        item.ServiceClientDescription = GetService.Descrição;
                    }
                    
                }
                if (item.MealType != null)
                {
                    TiposRefeição TRrow = DBMealTypes.GetById(item.MealType.Value);
                    if (TRrow != null)
                    {
                        item.MealTypeDescription = TRrow.Descrição;
                    }
                }
                else
                {
                    item.MealTypeDescription = "";
                }
            }
            return Json(dp);
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
                                item.CódGrupoServiço = Convert.ToInt32(codSGroupClient);
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
                            DBProjectDiary.Delete(newdp);
                            if (newdp.Quantidade != null && newdp.Quantidade > 0)
                            {
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
                        try
                        {
                            Task<WSCreateProjectDiaryLine.CreateMultiple_Result> TCreateNavDiaryLine = WSProjectDiaryLine.CreateNavDiaryLines(premov, transactID, _configws);
                            TCreateNavDiaryLine.Wait();
                        }
                        catch (Exception ex)
                        {
                            erro.eReasonCode = 3;
                            erro.eMessage = ex.Message;
                            //Response.StatusCode = (int)HttpStatusCode.NoContent;
                            return Json(erro);
                        }
                       

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
                        erro.eReasonCode = 3;
                        erro.eMessage = "Não foi possivel criar as linhas no nav";
                        //Response.StatusCode = (int)HttpStatusCode.NoContent;
                        return Json(erro);
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
        [HttpPost]
        public JsonResult CreateDiaryByPriceServiceCient(string projectNo, string serviceCod, string serviceGroup, string dateRegist) 
        {
            ProjectDiaryResponse response = new ProjectDiaryResponse();
            try
            {
                Projetos proj = DBProjects.GetById(projectNo);
                List<PriceServiceClientViewModel> dp = DBPriceServiceClient.ParseToViewModel(DBPriceServiceClient.GetAll()).Where(x => x.Client == proj.NºCliente && x.CodServClient == serviceCod).ToList();
                int? sGroup = null;
                if (serviceGroup != "" && serviceGroup != null)
                {
                    sGroup = Convert.ToInt32(serviceGroup);
                }
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
                        newRow.ServiceGroupCode = sGroup;
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
                    response.eMessage = "Tabela Preços Serviços Cliente não existe nenhuma linha com o Nº Cliente = " + proj.NºCliente + " e o Código Serviço Cliente = "+ serviceCod;
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
                        if (dpObject != null && dpObject.Count >0)
                        {
                            PreçosServiçosCliente newdp = DBPriceServiceClient.ParseToDatabase(x);
                            if (x.CompleteName != null && x.CompleteName.Length >0)
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
                    dp.ForEach(x=>{
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
            string sFileName = @"" + user + ".xlsx";
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
            string sFileName = @"" + user + ".xlsx";
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
        public async Task<JsonResult> ExportToExcel_AutorizacaoFaturacao([FromBody] List<ProjectDiaryViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

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

                if (dp != null)
                {
                    int count = 1;
                    foreach (ProjectDiaryViewModel item in Lista)
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
                            row.CreateCell(Col).SetCellValue(item.Quantity.ToString());
                            Col = Col + 1;
                        }
                        if (dp["unitCost"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.UnitCost.ToString());
                            Col = Col + 1;
                        }
                        if (dp["totalCost"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.TotalCost.ToString());
                            Col = Col + 1;
                        }
                        if (dp["unitPrice"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.UnitPrice.ToString());
                            Col = Col + 1;
                        }
                        if (dp["totalPrice"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.TotalPrice.ToString());
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
        public async Task<JsonResult> ExportToExcel_FaturacaoProjetos([FromBody] List<SPInvoiceListViewModel> Lista)
        {
            JObject dp = (JObject)Lista[0].ColunasEXCEL;

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
                ISheet excelSheet = workbook.CreateSheet("Faturação de Projetos");
                IRow row = excelSheet.CreateRow(0);
                int Col = 0;

                if (dp["projectNo"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Projeto");
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
                if (dp["commitmentNumber"]["hidden"].ToString() == "False")
                {
                    row.CreateCell(Col).SetCellValue("Nº Compromisso");
                    Col = Col + 1;
                }

                if (dp != null)
                {
                    int count = 1;
                    foreach (SPInvoiceListViewModel item in Lista)
                    {
                        Col = 0;
                        row = excelSheet.CreateRow(count);

                        if (dp["projectNo"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.ProjectNo);
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
                        if (dp["quantity"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.Quantity.ToString());
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
                        if (dp["unitCost"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.UnitCost.ToString());
                            Col = Col + 1;
                        }
                        if (dp["totalCost"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.TotalCost.ToString());
                            Col = Col + 1;
                        }
                        if (dp["unitPrice"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.UnitPrice.ToString());
                            Col = Col + 1;
                        }
                        if (dp["totalPrice"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.TotalPrice.ToString());
                            Col = Col + 1;
                        }
                        if (dp["commitmentNumber"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.CommitmentNumber);
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
        public IActionResult ExportToExcelDownload_FaturacaoProjetos(string sFileName)
        {
            sFileName = @"/Upload/temp/" + sFileName;
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Faturação de Projetos.xlsx");
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
            string sFileName = @"" + user + ".xlsx";
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

                if (dp != null)
                {
                    int count = 1;
                    foreach (ProjectDiaryViewModel item in Lista)
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
                            row.CreateCell(Col).SetCellValue(item.Quantity.ToString());
                            Col = Col + 1;
                        }
                        if (dp["unitPrice"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.UnitPrice.ToString());
                            Col = Col + 1;
                        }
                        if (dp["totalPrice"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.TotalPrice.ToString());
                            Col = Col + 1;
                        }
                        if (dp["unitCost"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.UnitCost.ToString());
                            Col = Col + 1;
                        }
                        if (dp["totalCost"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.TotalCost.ToString());
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
            string sFileName = @"" + user + ".xlsx";
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
                            row.CreateCell(Col).SetCellValue(item.Quantity.ToString());
                            Col = Col + 1;
                        }
                        if (dp["unitPrice"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.UnitPrice.ToString());
                            Col = Col + 1;
                        }
                        if (dp["totalPrice"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.TotalPrice.ToString());
                            Col = Col + 1;
                        }
                        if (dp["unitCost"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.UnitCost.ToString());
                            Col = Col + 1;
                        }
                        if (dp["totalCost"]["hidden"].ToString() == "False")
                        {
                            row.CreateCell(Col).SetCellValue(item.TotalCost.ToString());
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

    }
}