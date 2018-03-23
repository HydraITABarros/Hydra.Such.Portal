using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Compras
{
    public class PreRequesitionsViewModel : ErrorHandler
    {
        public string PreRequesitionsNo { get; set; }
        public int? Area { get; set; }
        public string RequesitionType { get; set; }
        public string ProjectNo { get; set; }
        public string RegionCode { get; set; }
        public string FunctionalAreaCode { get; set; }
        public string ResponsabilityCenterCode { get; set; }
        public bool? Urgent { get; set; }
        public bool? Sample { get; set; }
        public bool? Attachment { get; set; }
        public bool? Immobilized { get; set; }
        public bool? MoneyBuy { get; set; }
        public int? LocalCollectionCode { get; set; }
        public int? LocalDeliveryCode { get; set; }
        public string Notes { get; set; }
        public bool? PreRequesitionModel { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public string UpdateUser { get; set; }
        public bool? Exclusive { get; set; }
        public bool? AlreadyExecuted { get; set; }
        public bool? Equipment { get; set; }
        public bool? StockReplacement { get; set; }
        public bool? Complaint { get; set; }
        public string LocationCode { get; set; }
        public bool? FittingBudget { get; set; }
        public string EmployeeNo { get; set; }
        public string Vehicle { get; set; }
        public string ClaimedRequesitionNo { get; set; }
        public string CreateResponsible { get; set; }
        public string ApprovalResponsible { get; set; }
        public string ValidationResponsible { get; set; }
        public string ReceptionResponsible { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? ValidationDate { get; set; }
        public string ReceptionDate { get; set; }
        public string FoodProductionUnit { get; set; }
        public bool? NutritionRequesition { get; set; }
        public bool? DetergentsRequisition { get; set; }
        public string CCPProcedureNo { get; set; }
        public string Approvers { get; set; }
        public bool? LocalMarket { get; set; }
        public string LocalMarketRegion { get; set; }
        public bool? WarrantyRepair { get; set; }
        public bool? EMM { get; set; }
        public string DeliveryWarehouseDate { get; set; }
        public int? CollectionLocal { get; set; }
        public string CollectionAddress { get; set; }
        public string CollectionAddress2 { get; set; }
        public string CollectionPostalCode { get; set; }
        public string CollectionLocality { get; set; }
        public string CollectionContact { get; set; }
        public string CollectionReceptionResponsible { get; set; }
        public int? DeliveryLocal { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryAddress2 { get; set; }
        public string DeliveryPostalCode { get; set; }
        public string DeliveryLocality { get; set; }
        public string DeliveryContact { get; set; }
        public string ReceptionReceptionResponsible { get; set; }
        public string InvoiceNo { get; set; }
        //public int eReasonCode { get; set; }
        //public string eMessage { get; set; }
    }
}
