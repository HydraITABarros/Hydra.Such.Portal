using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Data.ViewModel.Projects
{
    public class ProjectBillingAuthorization : ProjectDetailsViewModel
    {
        private Projetos project;
        
        public ProjectBillingAuthorization()
        {
            this.Movements = new List<ProjectDiaryViewModel>();
        }

        public ProjectBillingAuthorization(Projetos project)
        {
            this.project = project;
            //if (project != null)
            //    this.Movements = project.MovimentosDeProjeto;
        }

        public List<ProjectDiaryViewModel> Movements { get; set; }
        public decimal TotalBillableConsumption
        {
            get
            {
                return this.Movements
                    .Where(x => x.TotalPrice.HasValue &&
                                x.Type == (int)ProjectDiaryMovementTypes.Consumo)
                    .Select(x => x.TotalPrice.Value)
                    .Sum();
            }
        }
        public decimal AuthorizedBilling
        {
            get
            {
                return this.Movements
                    .Where(x => x.TotalPrice.HasValue &&
                                x.Type == (int)ProjectDiaryMovementTypes.Consumo &&
                                x.AutorizatedInvoice == true)
                    .Select(x => x.TotalPrice.Value)
                    .Sum();
            }
        }
        public decimal BillingToAuthorize
        {
            get
            {
                return this.Movements
                    .Where(x => x.TotalPrice.HasValue &&
                                x.Type == (int)ProjectDiaryMovementTypes.Consumo &&
                                x.AutorizatedInvoice == false &&
                                x.Billable == true)
                    .Select(x => x.TotalPrice.Value)
                    .Sum();
            }
        }
        public decimal RegisteredInvoiceValue
        {
            get
            {
                return this.Movements
                    .Where(x => x.TotalPrice.HasValue &&
                                x.Type == (int)ProjectDiaryMovementTypes.Venda)
                    .Select(x => x.TotalPrice.Value)
                    .Sum();
            }
        }
        public decimal CreatedInvoicesValue
        {
            get
            {
                return 0;
            }
        }
    }
}
