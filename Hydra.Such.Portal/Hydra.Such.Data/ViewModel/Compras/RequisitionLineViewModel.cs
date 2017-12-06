using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Compras
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
        public decimal? QtyByUnitOfMeasure { get; set; }
        public decimal? UnitCostsould  { get; set; }
        public decimal? BudgetValue { get; set; }
        public int? MaintenanceOrderLineNo { get; set; }
        public bool? CreateMarketSearch { get; set; }
        public bool? SubmitPrePurchase { get; set; }
        public bool? SendPrePurchase { get; set; }
        public DateTime? LocalMarketDate { get; set; }
        public string LocalMarketUser { get; set; }
        public bool? SendForPurchase { get; set; }
        public DateTime? SendForPurchaseDate { get; set; }
        public bool? PurchaseValidated { get; set; }
        public bool? PruchaseRefused { get; set; }
        public string ReasonToRejectionLocalMarket { get; set; }
        public DateTime? RejectionLocalMarketDate { get; set; }
        public int? PruchaseId { get; set; }
        public string SupplierNo { get; set; }
        public string OpenOrderNo { get; set; }
        public int? OpenOrderLineNo { get; set; }
        public string QueryCreatedMarketNo { get; set; }
        public string CreatedOderNo { get; set; }
        public string SupplierProductCode { get; set; }
        public string UnitNutritionProduction { get; set; }
        public string MarketLocalRegion { get; set; }
        public string CustomerNo { get; set; }
        public string Approvers { get; set; }
    }
}
