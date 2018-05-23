using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class NAVProductsViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string MeasureUnit { get; set; }
        public string ProductGroupCode { get; set; }
        public string ItemCategoryCode { get; set; }
        public string VendorProductNo { get; set; }
        public string VendorNo { get; set; }
        public decimal LastCostDirect { get; set; }
        public decimal? UnitCost { get; set; }

    }
}
