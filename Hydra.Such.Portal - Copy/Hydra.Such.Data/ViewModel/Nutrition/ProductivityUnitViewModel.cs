using Hydra.Such.Data.ViewModel.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Nutrition
{
    public class ProductivityUnitViewModel : ErrorHandler
    {
        public int ProductivityUnitNo { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
        public string StatusDescription { get; set; }
        public bool? Active { get; set; }
        public string ClientNo { get; set; }
        public string ClientName { get; set; }
        public string CodeRegion { get; set; }
        public string CodeFunctionalArea { get; set; }
        public string CodeResponsabilityCenter { get; set; }
        public string StartDateExploration { get; set; }
        public string EndDateExploration { get; set; }
        public string Warehouse { get; set; }
        public string WarehouseSupplier { get; set; }
        public string ProjectKitchen { get; set; }
        public decimal ProjectKitchenTotalMovs { get; set; }
        public string ProjectWaste { get; set; }
        public decimal ProjectWasteTotalMovs { get; set; }
        public string ProjectWasteFeedstock { get; set; }
        public decimal ProjectWasteFeedstockTotalMovs { get; set; }
        public string ProjectSubsidiaries { get; set; }
        public decimal ProjectSubsidiariesTotalMovs { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }


        public List<DBProjectBillingViewModel> BillingProjects { get; set; }
        public List<CoffeeShopViewModel> CoffeeShops { get; set; }

        
    }
}
