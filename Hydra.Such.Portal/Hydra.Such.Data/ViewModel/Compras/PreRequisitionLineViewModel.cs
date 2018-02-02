using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Compras
{
    public class PreRequisitionLineViewModel
    {
        public string PreRequisitionLineNo { get; set; }
        public int LineNo { get; set; }
        public int? Type { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string LocalCode { get; set; }
        public string UnitMeasureCode { get; set; }
        public decimal? QuantityToRequire { get; set; }
        public string RegionCode { get; set; }
        public string FunctionalAreaCode { get; set; }
        public string CenterResponsibilityCode { get; set; }
        public string ProjectNo { get; set; }
        public string CreateDateTime { get; set; }
        public string CreateUser { get; set; }
        public string UpdateDateTime { get; set; }
        public string UpdateUser { get; set; }
        public string Description2 { get; set; }
        public decimal? QtyByUnitOfMeasure { get; set; }
        public decimal? QuantityRequired { get; set; }
        public decimal? QuantityPending { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? SellUnityPrice { get; set; }
        public decimal? BudgetValue { get; set; }
        public string ExpectedReceivingDate { get; set; }
        public bool? Billable { get; set; }
        public int? MaintenanceOrderLineNo { get; set; }
        public string EmployeeNo { get; set; }
        public string Vehicle { get; set; }
        public string SupplierNo { get; set; }
        public string SupplierProductCode { get; set; }
        public string UnitNutritionProduction { get; set; }
        public string CustomerNo { get; set; }
        public string OpenOrderNo { get; set; }
        public int? OpenOrderLineNo { get; set; }
        public decimal? TotalCost { get; set; }
        //EXTRAS
        public bool Selected { get; set; }
    }
}
