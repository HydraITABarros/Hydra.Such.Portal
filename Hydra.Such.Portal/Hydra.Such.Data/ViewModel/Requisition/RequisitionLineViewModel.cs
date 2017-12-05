using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Requisition
{
    public class RequisitionLineViewModel
    {
        public string RequestNo { get; set; }
        public int LineNo { get; set; }
        public int? Type { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string UnitMeasureCode { get; set; }
        public string LocalCode { get; set; }
        public bool? localMarket { get; set; }
        public decimal? QuantityToRequire { get; set; }
        public decimal? QuantityRequired { get; set; }
        public decimal? QuantityToProvide { get; set; }
        public decimal? QuantityAvailable { get; set; }
        public decimal? QuantityReceivable { get; set; }
        public decimal? QuantityReceived { get; set; }
        public decimal? QuantityPending { get; set; }
        public decimal? UnitCost { get; set; }
        public string ExpectedReceivingDate { get; set; }
        public bool? Billable { get; set; }
        public string ProjectNo { get; set; }
        public string RegionCode { get; set; }
        public string FunctionalAreaCode { get; set; }
        public string CenterResponsibilityCode { get; set; }
        public string FunctionalNo { get; set; }
        public string Vehicle { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public string UpdateUser { get; set; }
    }
}
