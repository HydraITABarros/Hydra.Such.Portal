using Hydra.Such.Data.ViewModel.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Nutrition
{
    public class StockkeepingUnitViewModel : ErrorHandler
    {
        public string ProductNo { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal? Inventory { get; set; }
        public bool? Blocked { get; set; }
        public string CodeUnitMeasure { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? WareHouseValue { get; set; }
        public bool? CodeWareHouse { get; set; }
        public string ShelfNo { get; set; }
        public string VendorNo { get; set; }
        public string VendorItemNo { get; set; }
        public decimal? LastCostDirect { get; set; }
        public string CodeProcuctCategory { get; set; }
        public string CodeProcuctGroup { get; set; }
        public decimal? PriceSale { get; set; }
    }
}
