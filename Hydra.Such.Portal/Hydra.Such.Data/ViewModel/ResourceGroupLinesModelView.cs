using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class ResourceGroupLinesModelView
    {
        public int? LineNo { get; set; }
        public string Resource { get; set; }
        public string ResourceName { get; set; }
        public string ResourceGroup { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? cost { get; set; }
    }
}
