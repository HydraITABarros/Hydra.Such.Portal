using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Compras
{
    /// <summary>
    /// Purchase Order Header DTO
    /// </summary>
    public class PurchOrderDTO
    {
        public string RequisitionId { get; set; }
        public string NAVPrePurchOrderId { get; set; }
        public string NAVPurchOrderFittingId { get; set; }
        public string NAVPurchOrderCommitmentId { get; set; }
        public string SupplierId { get; set; }
        public string RegionCode { get; set; }
        public string FunctionalAreaCode { get; set; }
        public string CenterResponsibilityCode { get; set; }

        public string OpenOrderNo { get; set; }
        public int? OpenOrderLineNo { get; set; }

        public List<PurchOrderLineDTO> Lines { get; set; }
    }
    /// <summary>
    /// Purchase Order Line DTO
    /// </summary>
    public class PurchOrderLineDTO
    {
        public int? LineId { get; set; }
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
        public string PurchOrderFitId { get; set; }
        public string PurchOrderCommitmentId { get; set; }
    }
}
