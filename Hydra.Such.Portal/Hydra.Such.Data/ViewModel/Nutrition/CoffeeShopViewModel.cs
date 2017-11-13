using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Nutrition
{
    public class CoffeeShopViewModel
    {
        public int ProductivityUnitNo { get; set; }
        public int Type { get; set; }
        public int Code { get; set; }
        public string Description { get; set; }
        public string CodeRegion { get; set; }
        public string CodeFunctionalArea { get; set; }
        public string CodeResponsabilityCenter { get; set; }
        public string CodeResponsible { get; set; }
        public string StartDateExploration { get; set; }
        public string EndDateExploration { get; set; }
        public string Warehouse { get; set; }
        public string WarehouseSupplier { get; set; }
        public string ProjectNo { get; set; }
        public bool? Active { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
