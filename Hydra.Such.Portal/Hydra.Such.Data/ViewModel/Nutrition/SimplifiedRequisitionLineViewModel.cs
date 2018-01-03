using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Nutrition
{
    public class SimplifiedRequisitionLineViewModel
    {
        public string RequisitionNo { get; set; }
        public int LineNo { get; set; }
        public int? Type { get; set; }
        public string Code { get; set; }
        public string LocationCode { get; set; }
        public int? Status { get; set; }
        public string Description { get; set; }
        public string MeasureUnitNo { get; set; }
        public decimal? QuantityToRequire { get; set; }
        public decimal? QuantityApproved { get; set; }
        public decimal? QuantityReceipt { get; set; }
        public decimal? QuantityToApprove { get; set; }
        public decimal? TotalCost { get; set; }
        public string ProjectNo { get; set; }
        public int? MealType { get; set; }
        public string RegionCode { get; set; }
        public string FunctionAreaCode { get; set; }
        public string ResponsabilityCenterCode { get; set; }
        public string EmployeeNo { get; set; }
        public decimal? UnitCost { get; set; }
        public string RequisitionDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }
}
