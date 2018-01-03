using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Compras
{
    /// <summary>
    /// Purchase From Supplier Header DTO
    /// </summary>
    public class PurchFromSupplierDTO
    {
        public string RequisitionId { get; set; }
        public string NAVPurchaseId { get; set; }
        public string SupplierId { get; set; }
        public string RegionCode { get; set; }
        public string FunctionalAreaCode { get; set; }
        public string CenterResponsibilityCode { get; set; }

        public List<PurchFromSupplierLinesDTO> Lines { get; set; }
    }
    /// <summary>
    /// Purchase From Supplier Lines DTO
    /// </summary>
    public class PurchFromSupplierLinesDTO
    {
        //public int? LineId { get; set; }
        //public int? NAVLineId { get; set; }
        public int? Type { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal? QuantityRequired { get; set; }
        public decimal? UnitCost { get; set; }
        public string ProjectNo { get; set; }
        public string LocationCode { get; set; }
        public string OpenOrderNo { get; set; }
        public int? OpenOrderLineNo { get; set; }
    }
}
