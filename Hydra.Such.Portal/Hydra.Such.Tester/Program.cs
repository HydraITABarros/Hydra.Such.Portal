using Hydra.Such.Data.Database;
using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.Project;
using Hydra.Such.Data.NAV;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.Projects;
using Hydra.Such.Portal.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;

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
                ApprovalTest();
            }
            catch (Exception ex)
            {

                Console.Write(ex.Message);
            }
            Console.ReadLine();
        }

        public static void NumerationTest()
        {
            string numeration = DBNumerationConfigurations.GetNextNumeration(1, true);
            string numeration2 = DBNumerationConfigurations.GetNextNumeration(2, true);
            string numeration3 = DBNumerationConfigurations.GetNextNumeration(3, true);

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

        public static bool IsServerConnected()
        {
            using (var l_oConnection = new SqlConnection("data source=10.101.1.10\\SQLNAVDEV;initial catalog=PlataformaOperacionalSUCH;user id=such_portal_user;password=SuchPW.2K17;"))
            {
                try
                {
                    l_oConnection.Open();
                    return true;
                }
                catch (SqlException ex)
                {
                    return false;
                }
            }
        }

        public static void ApprovalTest()
        {
            Data.Database.SuchDBContext.ConnectionString = "data source=10.101.1.10\\sqlnavdev;initial catalog=PlataformaOperacionalSUCH;user id=such_portal_user;password=SuchPW.2K17;";
            //var x = ApprovalMovementsManager.StartApprovalMovement(1, 1, "", "", "",1000, "REQ0037", "hydra06@such.pt");
            //var x = ApprovalMovementsManager.ApproveMovement(6, "hydra06@such.pt");

            //var x = ApprovalMovementsManager.RejectMovement(2, "hydra06@such.pt", "O Valor não faz sentido face á dificuldade do projeto");
            var items = Data.Logic.Approvals.DBApprovalConfigurations.GetByTypeAreaValueDateAndDimensions(1, "01", "010AG", "23", 600, DateTime.Now);

            //var lowLevel = items.Where(x => x.NívelAprovação.HasValue).OrderBy(x => x.NívelAprovação.Value).FirstOrDefault();
            //items.RemoveAll(x => x.NívelAprovação != lowLevel.NívelAprovação);
            return;
            ErrorHandler result = ApprovalMovementsManager.StartApprovalMovement(1, "10", "010AG", "23", 5000, "JP123", "hydra08@such.pt");

            int movNo = Convert.ToInt32(result.eMessages[0].Message);
            ErrorHandler ApproveResult = ApprovalMovementsManager.ApproveMovement(movNo, "hydra08@such.pt");

            Console.ReadLine();
        }

    }
}
