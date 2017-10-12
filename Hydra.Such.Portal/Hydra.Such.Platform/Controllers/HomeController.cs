using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.Projects;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.NAV;
using System.Net;
using Hydra.Such.NavApi;

namespace Hydra.Such.Platform.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            Projetos cProject = DBProjects.GetById("PROJ0001");

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





                NavApi.WSCreateProject.Status StatusValue;

                switch (result.Status)
                {
                    case 1:
                        StatusValue = NavApi.WSCreateProject.Status.Planning;
                        break;
                    case 2:
                        StatusValue = NavApi.WSCreateProject.Status.Quote;
                        break;
                    case 3:
                        StatusValue = NavApi.WSCreateProject.Status.Open;
                        break;
                    default:
                        StatusValue = NavApi.WSCreateProject.Status.Completed;
                        break;

                }


                NavApi.WSCreateProject.WSJob NAVProj = new NavApi.WSCreateProject.WSJob()
                {
                    No = result.ProjectNo,
                    Description100 = result.Description,
                    Bill_to_Customer_No = result.ClientNo,
                    Creation_Date = DateTime.Parse(result.Date),
                    Status = StatusValue,
                    FunctionAreaCode20 = result.FunctionalAreaCode,
                    ResponsabilityCenterCode20 = result.ResponsabilityCenterCode,
                    Job_Posting_Group = "",
                    Project_Manager = result.ProjectLeader,
                    Person_Responsible = result.ProjectResponsible
                };

                NAVProjects.CreateNavProject(NAVProj);
               
            }
            return View();
        }

        
    }
}
