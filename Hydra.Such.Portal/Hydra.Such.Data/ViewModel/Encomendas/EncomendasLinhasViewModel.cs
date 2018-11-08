using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Encomendas
{
    public class EncomendasLinhasViewModel
    {
        public int? LineNo { get; set; }
        public string No { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? DirectUnitCost { get; set; }
        public decimal? VAT { get; set; }
        public decimal? Amount { get; set; }
        public decimal? AmountIncludingVAT { get; set; }
        public string JobNo { get; set; }
        public string AllocationNo { get; set; }
        public string LocationCode { get; set; }
        public decimal? QuantityReceived { get; set; }
        public decimal? QuantityInvoiced { get; set; }

        public string RegionId { get; set; }
        public string FunctionalAreaId { get; set; }
        public string RespCenterId { get; set; }
    }
}
