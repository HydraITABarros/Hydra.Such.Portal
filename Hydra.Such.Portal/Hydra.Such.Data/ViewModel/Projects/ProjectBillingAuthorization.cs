using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static Hydra.Such.Data.Enumerations;
using Hydra.Such.Data.Logic.ProjectMovements;
using Hydra.Such.Data.Logic.Project;
using Hydra.Such.Data.Logic.Contracts;
using Hydra.Such.Data.Logic;

namespace Hydra.Such.Data.ViewModel.Projects
{
    public class ProjectBillingAuthorization : ProjectDetailsViewModel
    {
        private ProjectDetailsViewModel project;
        private Contracts.ContractViewModel contract;
        private string navDatabaseName;
        private string navCompanyName;
        private string userName;

        public List<ProjectMovementViewModel> Movements { get; set; }

        public decimal TotalBillableConsumption { get; private set; }
        public decimal AuthorizedBilling { get; private set; }
        public decimal BillingToAuthorize { get; private set; }
        public decimal RegisteredInvoiceValue { get; private set; }
        public decimal? CreatedInvoicesValue { get; private set; }
        public string AutorizationCommitmentNo { get; private set; }
        public ProjectBillingAuthorization()
        {
            this.Movements = new List<ProjectMovementViewModel>();
        }

        public ProjectBillingAuthorization(string projectNo, string userName, string navDatabaseName, string navCompanyName)
        {
            this.navCompanyName = navCompanyName;
            this.navDatabaseName = navDatabaseName;
            this.userName = userName;

            this.project = DBProjects.GetById(projectNo).ParseToViewModel();
            if (project != null)
                this.contract = DBContracts.ParseToViewModel(DBContracts.GetByIdLastVersion(project.ProjectNo), navDatabaseName, navCompanyName);

            if (project != null)
            {
                this.Movements = GetProjectMovements(null);
            }
            LoadBillingResume();
        }

        private List<ProjectMovementViewModel> GetProjectMovements(bool? billable)
        {
            List<ProjectMovementViewModel> projectMovements = DBProjectMovements.GetProjectMovementsFor(this.ProjectNo, billable)
                        .ParseToViewModel(navDatabaseName, navCompanyName)
                        .OrderBy(x => x.ClientName).ToList();

            if (projectMovements.Count > 0)
            {
                var userDimensions = Logic.DBUserDimensions.GetByUserId(userName);
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

        private void LoadBillingResume()
        {
            TotalBillableConsumption = this.Movements
                    .Where(x => x.TotalPrice.HasValue &&
                                x.Type == (int)ProjectDiaryMovementTypes.Consumo)
                    .Select(x => x.TotalPrice.Value)
                    .Sum();

            AuthorizedBilling = this.Movements
                    .Where(x => x.TotalPrice.HasValue &&
                                x.Type == (int)ProjectDiaryMovementTypes.Consumo &&
                                x.AutorizatedInvoice == true)
                    .Select(x => x.TotalPrice.Value)
                    .Sum();

            BillingToAuthorize = this.Movements
                    .Where(x => x.TotalPrice.HasValue &&
                                x.Type == (int)ProjectDiaryMovementTypes.Consumo &&
                                x.AutorizatedInvoice == false &&
                                x.Billable == true)
                    .Select(x => x.TotalPrice.Value)
                    .Sum();

            RegisteredInvoiceValue = this.Movements
                    .Where(x => x.TotalPrice.HasValue &&
                                x.Type == (int)ProjectDiaryMovementTypes.Venda)
                    .Select(x => x.TotalPrice.Value)
                    .Sum();

            CreatedInvoicesValue = Logic.DBNAV2017Projects.GetTotalInvoiceValue(navDatabaseName, navCompanyName, project.ProjectNo);
        }
    }
}
