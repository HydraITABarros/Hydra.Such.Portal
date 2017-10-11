using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;

namespace Hydra.Such.Portal.Controllers
{
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
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetListProjectsByArea([FromBody] int id)
        {
            List<ProjectListItemViewModel> result = DBProjects.GetAllByAreaToList(id);

            result.ForEach(x =>
            {
                x.StatusDescription = EnumerablesFixed.ProjectStatus.Where(y => y.Id == x.Status).FirstOrDefault().Value;
            });
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
                        Date = cProject.Data.Value.ToString("yyyy-MM-dd"),
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
                        RequestDate = cProject.DataDoPedido.Value.ToString("yyyy-MM-dd"),
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

                return Json(new ProjectDetailsViewModel());
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
            if (data.ProjectNo != "" && !CfgNumeration.Manual.Value)
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
                    data.ProjectNo = DBNumerationConfigurations.GetNextNumeration(ProjectNumerationConfigurationId);

                    Projetos cProject = new Projetos()
                    {
                        NºProjeto = data.ProjectNo,
                        Área = data.Area,
                        Descrição = data.Description,
                        NºCliente = data.ClientNo,
                        Data = DateTime.Parse(data.Date),
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
                        DataDoPedido = DateTime.Parse(data.RequestDate),
                        ValidadeDoPedido = data.RequestValidity,
                        DescriçãoDetalhada = data.DetailedDescription,
                        CategoriaProjeto = data.ProjectCategory,
                        NºContratoOrçamento = data.BudgetContractNo,
                        ProjetoInterno = data.InternalProject,
                        ChefeProjeto = data.ProjectLeader,
                        ResponsávelProjeto = data.ProjectResponsible
                    };

                    //Create Project On Database
                    cProject = DBProjects.Create(cProject);

                    if (true)
                    {
                        data.eReasonCode = 3;
                        data.eMessage = "Ocorreu um erro ao criar o projeto no portal.";
                    }
                    else
                    {
                        //Create Project on NAV
                        Task<WSCreateNAVProject.Create_Result> TCreateNavProj = WSProject.CreateNavProject(data, _configws);
                        TCreateNavProj.Wait();
                        if (!TCreateNavProj.IsCompletedSuccessfully)
                        {
                            //Delete Created Project on Database
                            DBProjects.Delete(cProject);
                            
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
                Projetos cProject = new Projetos()
                {
                    NºProjeto = data.ProjectNo,
                    Área = data.Area,
                    Descrição = data.Description,
                    NºCliente = data.ClientNo,
                    Data = DateTime.Parse(data.Date),
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
                    DataDoPedido = DateTime.Parse(data.RequestDate),
                    ValidadeDoPedido = data.RequestValidity,
                    DescriçãoDetalhada = data.DetailedDescription,
                    CategoriaProjeto = data.ProjectCategory,
                    NºContratoOrçamento = data.BudgetContractNo,
                    ProjetoInterno = data.InternalProject,
                    ChefeProjeto = data.ProjectLeader,
                    ResponsávelProjeto = data.ProjectResponsible
                };

                DBProjects.Update(cProject);
                return Json(data);
            }
            return Json(false);
        }
        #endregion



        #region Job Ledger Entry
        public IActionResult MovimentosDeProjeto(String ProjectNo)
        {
            ViewBag.ProjectNo = ProjectNo;
            return View();
        }
        

        [HttpPost]
        public JsonResult GetJobLedgerEntries([FromBody] string ProjectNo)
        {
            List<NAVJobLedgerEntryViewModel> result = DBNAV2017JobLedgerEntries.GetFiltered(ProjectNo,null, _config.NAVDatabaseName, _config.NAVCompanyName);

            return Json(result);
        }

        #endregion

    }
}