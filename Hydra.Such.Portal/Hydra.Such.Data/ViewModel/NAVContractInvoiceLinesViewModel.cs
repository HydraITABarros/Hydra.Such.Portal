using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class NAVContractInvoiceLinesViewModel
    {
        public string No_ { get; set; }
        public string DocNo { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Amount { get; set; }
        public decimal? AmountIncludingVAT { get; set; }
        public string JobNo { get; set; }
        public string ExternalShipmentNo_ { get; set; }
        public string DataRegistoDiario { get; set; }
    }
}
