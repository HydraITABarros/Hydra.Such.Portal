using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Compras
{
    public class RequisitionViewModel : ErrorHandler
    {
        public string RequisitionNo { get; set; }
        public int? Area { get; set; }
        public int? State { get; set; }
        public string ProjectNo { get; set; }
        public string RegionCode { get; set; }
        public string FunctionalAreaCode { get; set; }
        public string CenterResponsibilityCode { get; set; }
        public string LocalCode { get; set; }
        public string EmployeeNo { get; set; }
        public string Vehicle { get; set; }
        public string ReceivedDate { get; set; }
        public bool? Urgent { get; set; }
        public bool? Sample { get; set; }
        public bool? Attachment { get; set; }
        public bool? Immobilized { get; set; }
        public bool? BuyCash { get; set; }
        public int? LocalCollectionCode { get; set; }
        public int? LocalDeliveryCode { get; set; }
        public string Comments { get; set; }
        public bool? RequestModel { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? RelatedSearches { get; set; }
        public bool? Exclusive { get; set; }
        public bool? AlreadyPerformed { get; set; }
        public bool? Equipment { get; set; }
        public bool? StockReplacement { get; set; }
        public bool? Reclamation { get; set; }
        public string RequestReclaimNo { get; set; }
        public string ResponsibleCreation { get; set; }
        public string ResponsibleApproval { get; set; }
        public string ResponsibleValidation { get; set; }
        public string ResponsibleReception { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? ValidationDate { get; set; }
        public string UnitFoodProduction  { get; set; }
        public bool? RequestNutrition { get; set; }
        public bool? RequestforDetergents { get; set; }
        public string ProcedureCcpNo { get; set; }
        public string Approvers { get; set; }
        public bool? LocalMarket { get; set; }
        public string LocalMarketRegion { get; set; }
        public bool? RepairWithWarranty { get; set; }
        public bool? Emm { get; set; }
        public DateTime? WarehouseDeliveryDate { get; set; }
        public int? LocalCollection { get; set; }
        public string CollectionAddress{ get; set; }
        public string Collection2Address{ get; set; }
        public string CollectionPostalCode{ get; set; }
        public string CollectionLocality { get; set; }
        public string CollectionContact  { get; set; }
        public string CollectionResponsibleReception { get; set; }
        public int? LocalDelivery { get; set; }
        public string DeliveryAddress { get; set; }
        public string Delivery2Address { get; set; }
        public string DeliveryPostalCode { get; set; }
        public string LocalityDelivery { get; set; }
        public string DeliveryContact { get; set; }
        public string ResponsibleReceptionReception { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? LocalMarketDate { get; set; }
        public decimal? EstimatedValue { get; set; }
        public string MarketInquiryNo { get; set; }
        public string OrderNo { get; set; }
        public DateTime? RequisitionDate { get; set; }
        public string dimension { get; set; }
        public decimal? Budget { get; set; }

        public List<RequisitionLineViewModel> Lines { get; set; }
    }
}
