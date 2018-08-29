using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class NAVResourcesViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string MeasureUnit { get; set; }
        public string ResourceGroup { get; set; }
        public int WasteRate { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitCost { get; set; }
}
}
