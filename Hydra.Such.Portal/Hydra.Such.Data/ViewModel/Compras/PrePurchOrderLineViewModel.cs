using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Compras
{
    public class PrePurchOrderLineViewModel : ErrorHandler
    {
        public string PrePurchOrderNo { get; set; }
        public int PrePurchOrderLineNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public string UnitOfMeasureCode { get; set; }
        public string LocationCode { get; set; }
        public decimal? QuantityAvailable { get; set; }
        public decimal? UnitCost { get; set; }
        public string ProjectNo { get; set; }
        public string RegionCode { get; set; }
        public string FunctionalAreaCode { get; set; }
        public string CenterResponsibilityCode { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public string UpdateUser { get; set; }
        public string SupplierNo { get; set; }
        public string RequisitionNo { get; set; }
        public int? RequisitionLineNo { get; set; }
    }
}
