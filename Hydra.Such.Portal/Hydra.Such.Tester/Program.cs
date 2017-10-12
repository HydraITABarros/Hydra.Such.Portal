using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.Project;
using Hydra.Such.Data.NAV;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Projects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;

namespace Hydra.Such.Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            //NumerationTest();

            //StoredProcedureTeste();
            try
            {
                CreateNAVProj();
            }
            catch (Exception ex)
            {

                Console.Write(ex.Message);
            }
            Console.ReadLine();
        }

        public static void NumerationTest()
        {
            string numeration = DBNumerationConfigurations.GetNextNumeration(1);
            string numeration2 = DBNumerationConfigurations.GetNextNumeration(2);
            string numeration3 = DBNumerationConfigurations.GetNextNumeration(3);
            
            //Console.WriteLine(numeration);
            //Console.WriteLine(numeration2);
            //Console.WriteLine(numeration3);

            Console.ReadLine();
        }


        public static void StoredProcedureTeste()
        {
            var x = DBNAV2017ShippingAddresses.GetAll("SUCH_NAV_DEV", "CRONUS Portugal Ltd_");

            Console.ReadLine();
        }

        public static void CreateNAVProj()
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

                //WSProject.CreateNavProject(result);
            }


        }
    }
}
