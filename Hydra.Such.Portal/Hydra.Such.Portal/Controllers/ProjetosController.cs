using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.ViewModel.Projects;
using Hydra.Such.Data.Logic;
using Hydra.Such.Portal.Configurations;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic.Project;
using Microsoft.Extensions.Options;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.NAV;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json.Linq;

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


        #region Home
        [HttpPost]
        public JsonResult GetListProjectsByArea([FromBody] JObject requestParams)
        {
            int AreaId = int.Parse(requestParams["areaid"].ToString());
            Boolean Ended = Boolean.Parse(requestParams["ended"].ToString());

            List<ProjectListItemViewModel> result = DBProjects.GetAllByAreaToList(AreaId);

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
            if (CUserDimensions.Where(y => y.Dimensão == 1).Count() > 0)
                result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == 1 && y.ValorDimensão == x.RegionCode));
            //FunctionalAreas
            if (CUserDimensions.Where(y => y.Dimensão == 2).Count() > 0)
                result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == 2 && y.ValorDimensão == x.FunctionalAreaCode));
            //ResponsabilityCenter
            if (CUserDimensions.Where(y => y.Dimensão == 3).Count() > 0)
                result.RemoveAll(x => !CUserDimensions.Any(y => y.Dimensão == 3 && y.ValorDimensão == x.ResponsabilityCenterCode));

            return Json(result);
        }
        #endregion

        #region Details
        public IActionResult Detalhes(String id)
        {
            return View();
        }

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
                    Configuração Configs = DBConfigurations.GetById(1);
                    int ProjectNumerationConfigurationId = Configs.NumeraçãoProjetos.Value;
                    string projNoAuto = "";
                    if (data.ProjectNo == "" || data.ProjectNo == null)
                    {
                        projNoAuto = DBNumerationConfigurations.GetNextNumeration(ProjectNumerationConfigurationId, (data.ProjectNo == "" || data.ProjectNo == null));
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
                            GrupoContabObra = data.AccountWorkGroup,
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
                            }
                            else
                            {
                                //Update Last Numeration Used
                                ConfiguraçãoNumerações ConfigNumerations = DBNumerationConfigurations.GetById(ProjectNumerationConfigurationId);
                                ConfigNumerations.ÚltimoNºUsado = data.ProjectNo;
                                DBNumerationConfigurations.Update(ConfigNumerations);

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
                    Projetos cProject = new Projetos()
                    {
                        NºProjeto = data.ProjectNo,
                        Área = data.Area,
                        Descrição = data.Description,
                        NºCliente = data.ClientNo,
                        Data = data.Date == "" ? (DateTime?)null : DateTime.Parse(data.Date),
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
                        GrupoContabObra = data.AccountWorkGroup,
                        TipoGrupoContabProjeto = data.GroupContabProjectType,
                        TipoGrupoContabOmProjeto = data.GroupContabOMProjectType,
                        PedidoDoCliente = data.ClientRequest,
                        DataDoPedido = data.RequestDate == "" ? (DateTime?)null : DateTime.Parse(data.RequestDate),
                        ValidadeDoPedido = data.RequestValidity,
                        DescriçãoDetalhada = data.DetailedDescription,
                        CategoriaProjeto = data.ProjectCategory,
                        NºContratoOrçamento = data.BudgetContractNo,
                        ProjetoInterno = data.InternalProject,
                        ChefeProjeto = data.ProjectLeader,
                        ResponsávelProjeto = data.ProjectResponsible,
                        UtilizadorModificação = User.Identity.Name
                    };

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
        public IActionResult DiarioProjeto(String id)
        {
            //UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 1, 2);
            //if (UPerm != null && UPerm.Read.Value)
            //{
            //  ViewBag.UPermissions = UPerm;
            ViewBag.ProjectNo = id ?? "";
            return View();
            //}
            //else
            //{
            //    return RedirectToAction("AccessDenied", "Error");
            //}
        }

        [HttpPost]
        public JsonResult GetAllProjectDiary([FromBody]string projectNo)
        {
            if (projectNo == null || projectNo == "")
            {
                List<ProjectDiaryViewModel> dp = DBProjectDiary.GetAllOpen(User.Identity.Name).Select(x => new ProjectDiaryViewModel()
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
                return Json(dp);
            }
            else
            {
                //List<DiárioDeProjeto> dp1 = DBProjectDiary.GetByProjectNo(projectNo, User.Identity.Name).ToList();
                //foreach (DiárioDeProjeto var in dp1)
                //{
                //    vae
                //}
                List<ProjectDiaryViewModel> dp = DBProjectDiary.GetByProjectNo(projectNo, User.Identity.Name).Select(x => new ProjectDiaryViewModel()
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
                return Json(dp);
            }
        }

        [HttpPost]
        public JsonResult UpdateProjectDiary([FromBody] List<ProjectDiaryViewModel> dp, string projectNo)
        {
            List<DiárioDeProjeto> previousList;
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

            //Update or Create
            try
            {
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
                                       CódServiçoCliente = x.ServiceClientCode

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
                throw;
            }


            return Json(dp);
        }

        public JsonResult CreatePDByMovProj([FromBody] List<ProjectDiaryViewModel> dp, string projectNo)
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
                    DataConsumo = x.ConsumptionDate == "" || x.ConsumptionDate == String.Empty ? (DateTime?)null : DateTime.Parse(x.ConsumptionDate),
                    CódServiçoCliente = x.ServiceClientCode

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

            return Json(dp);
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
                        DiárioDeProjeto newdp = DBProjectDiary.GetAllByCode(User.Identity.Name, x.Code);
                        if (newdp != null)
                        {
                            newdp.Registado = true;
                            newdp.UtilizadorModificação = User.Identity.Name;
                            newdp.DataHoraModificação = DateTime.Now;
                            DBProjectDiary.Update(newdp);
                        }
                    }
                });
            }


            return Json(dp);
        }

        [HttpPost]
        public JsonResult GetMovements([FromBody] string projectNo)
        {
            //Get Contract from Project
            List<DiárioDeProjeto> dp = new List<DiárioDeProjeto>();
            if (projectNo != null && projectNo != "")
            {
                dp = DBProjectDiary.GetRegisteredDiary(projectNo).Select(x => new DiárioDeProjeto()
                {
                    NºProjeto = x.NºProjeto,
                    Data = x.Data,
                    TipoMovimento = x.TipoMovimento,
                    Tipo = x.Tipo,
                    Código = x.Código,
                    Descrição = x.Descrição,
                    Quantidade = x.Quantidade,
                    CódUnidadeMedida = x.CódUnidadeMedida,
                    CódLocalização = x.CódLocalização,
                    GrupoContabProjeto = x.GrupoContabProjeto,
                    CódigoRegião = x.CódigoRegião,
                    CódigoÁreaFuncional = x.CódigoÁreaFuncional,
                    CódigoCentroResponsabilidade = x.CódigoCentroResponsabilidade,
                    Utilizador = x.Utilizador,
                    CustoUnitário = x.CustoUnitário,
                    CustoTotal = x.CustoTotal,
                    PreçoUnitário = x.PreçoUnitário,
                    PreçoTotal = x.PreçoTotal,
                    Faturável = x.Faturável,
                    Registado = false,
                    DataConsumo = x.DataConsumo.ToString() == "" || x.DataConsumo.ToString() == String.Empty ? (DateTime?)null : DateTime.Parse(x.DataConsumo.ToString()),

                }).ToList();

                foreach (var item in dp)
                {
                    DBProjectDiary.Create(item);
                }
                return Json(dp);
            }
            return Json(false);
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
        public IActionResult MovimentosDeProjeto(String id)
        {
            UserAccessesViewModel UPerm = DBUserAccesses.GetByUserAreaFunctionality(User.Identity.Name, 1, 2);
            if (UPerm != null && UPerm.Read.Value)
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
            List<ProjectDiaryViewModel> dp = DBProjectDiary.GetRegisteredDiary(ProjectNo).Select(x => new ProjectDiaryViewModel()
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
                Registered = x.Registado
            }).ToList();

            return Json(dp);
        }

        [HttpPost]
        public JsonResult GetProjectMovementsDp([FromBody] string ProjectNo, bool allProjs)
        {
            List<ProjectDiaryViewModel> dp = DBProjectDiary.GetRegisteredDiaryDp(ProjectNo, User.Identity.Name, allProjs).Select(x => new ProjectDiaryViewModel()
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
        public JsonResult GetAutorizacaoFaturacao([FromBody] int areaId)
        {
            try
            {
                List<ProjectDiaryViewModel> result = DBProjectDiary.GetAllTableByArea(User.Identity.Name, areaId).Select(x => new ProjectDiaryViewModel()
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
                    if (userDimensionsViewModel.Where(x => x.Dimension == 1).Count() > 0)
                        result.RemoveAll(x => !userDimensionsViewModel.Any(y => y.DimensionValue == x.RegionCode));
                    if (userDimensionsViewModel.Where(x => x.Dimension == 2).Count() > 0)
                        result.RemoveAll(x => !userDimensionsViewModel.Any(y => y.DimensionValue == x.FunctionalAreaCode));
                    if (userDimensionsViewModel.Where(x => x.Dimension == 3).Count() > 0)
                        result.RemoveAll(x => !userDimensionsViewModel.Any(y => y.DimensionValue == x.ResponsabilityCenterCode));
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [HttpPost]
        public JsonResult CreateInvoiceLines([FromBody] List<ProjectDiaryViewModel> data)
        {
            string num_cliente = "";
            string PKey = "";
            int lineNo = 1;
            List<InvoiceMessages> ClientsError = new List<InvoiceMessages>();
            try
            {
                if (data != null)
                {
                    List<ProjectDiaryViewModel> NewList = new List<ProjectDiaryViewModel>();

                    foreach (var lines in data)
                    {
                        if (num_cliente != lines.InvoiceToClientNo)
                        {
                            if (NewList.Count() > 0)
                            {
                                //update to Invoiced = true
                                foreach (var lst in NewList)
                                {
                                    DiárioDeProjeto upDate = DBProjectDiary.GetByLineNo(lst.LineNo, User.Identity.Name).FirstOrDefault();
                                    upDate.Faturada = true;
                                    DBProjectDiary.Update(upDate);
                                }
                                InvoiceMessages Messages = new InvoiceMessages();
                                Messages.ClientNo = lines.InvoiceToClientNo;
                                Messages.Iserror = false;

                                ClientsError.Add(Messages);
                                NewList.Clear();
                            }

                            try
                            {
                                PKey = "";
                                Task<WSCreatePreInvoice.Create_Result> TCreatePreInvoice = WSPreInvoice.CreatePreInvoice(lines, _configws);
                                TCreatePreInvoice.Wait();
                                if (TCreatePreInvoice.IsCompletedSuccessfully)
                                {
                                    num_cliente = lines.InvoiceToClientNo;
                                    PKey = TCreatePreInvoice.Result.WSPreInvoice.No;
                                }
                                else
                                {
                                    num_cliente = lines.InvoiceToClientNo;
                                    PKey = "";
                                }
                            }
                            catch (Exception ex)
                            {
                                PKey = "";
                                num_cliente = lines.InvoiceToClientNo;
                                throw;
                            }
                        }

                        if (!String.IsNullOrEmpty(PKey) && PKey != "error")
                        {
                            try
                            {
                                Task<WSCreatePreInvoiceLine.Create_Result> TCreatePreInvoiceLine = WSPreInvoiceLine.CreatePreInvoiceLine(lines, _configws, PKey);
                                TCreatePreInvoiceLine.Wait();

                                if (TCreatePreInvoiceLine.IsCompletedSuccessfully && !String.IsNullOrEmpty(TCreatePreInvoiceLine.Result.WsPreInvoiceLine.Key))
                                {
                                    num_cliente = lines.InvoiceToClientNo;
                                    NewList.Add(lines);
                                    if (data.Count() == lineNo)
                                    {
                                        //update to Invoiced = true
                                        foreach (var lst in NewList)
                                        {
                                            DiárioDeProjeto upDate = DBProjectDiary.GetByLineNo(lst.LineNo, User.Identity.Name).FirstOrDefault();
                                            upDate.Faturada = true;
                                            DBProjectDiary.Update(upDate);

                                            InvoiceMessages Messages = new InvoiceMessages();
                                            Messages.ClientNo = num_cliente;
                                            Messages.Iserror = false;

                                            ClientsError.Add(Messages);
                                        }
                                    }
                                }
                                else
                                {
                                    Task<WSCreatePreInvoice.Delete_Result> DeleteHeader = WSPreInvoice.DeletePreInvoiceLineList(PKey, _configws);
                                    num_cliente = lines.InvoiceToClientNo;
                                    PKey = "error";

                                    InvoiceMessages Messages = new InvoiceMessages();
                                    Messages.ClientNo = lines.InvoiceToClientNo;
                                    Messages.Iserror = true;

                                    ClientsError.Add(Messages);
                                }
                            }
                            catch (Exception ex)
                            {
                                Task<WSCreatePreInvoice.Delete_Result> DeleteHeader = WSPreInvoice.DeletePreInvoiceLineList(PKey, _configws);
                                PKey = "error";

                                InvoiceMessages Messages = new InvoiceMessages();
                                Messages.ClientNo = lines.InvoiceToClientNo;
                                Messages.Iserror = true;

                                ClientsError.Add(Messages);
                            }
                        }
                        else
                        {
                            num_cliente = lines.InvoiceToClientNo;
                        }
                        lineNo += 1;
                    }
                }
            }
            catch (Exception ex)
            {
                ProjectDiaryViewModel dataerror = new ProjectDiaryViewModel();
                dataerror.eReasonCode = 4;
                dataerror.eMessage = "Ocorreu um erro ao criar Pré Fatura";
                return Json(dataerror);
            }
            //ProjectDiaryViewModel message = new ProjectDiaryViewModel();
            //message.eReasonCode = 1;
            //message.eMessage = "Linhas de Fatura criadas com sucesso";
            data.Clear();
            return Json(ClientsError);
        }

        private class InvoiceMessages
        {
            public bool Iserror { get; set; }
            public string ClientNo { get; set; }
        }
        #endregion InvoiceAutorization

    }
}