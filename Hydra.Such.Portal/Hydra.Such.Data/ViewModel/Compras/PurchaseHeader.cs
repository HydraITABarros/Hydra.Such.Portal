using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Compras
{
    public class PurchaseHeader
    {
        public int DocumentType { get; set; }
		public string No { get; set; }
		public string OrderDate { get; set; }
		public string DueDate { get; set; }
		public string LocationCode { get; set; }
        public int DimensionSetID { get; set; }
        public string RegionId { get; set; }
		public string FunctionalAreaId { get; set; }
        public string RespCenterId { get; set; }
        public string VendorOrderNo { get; set; }
		public string VendorInvoiceNo { get; set; }
		public string VendorCrMemoNo { get; set; }
        public string BuyFromVendorNo { get; set; } 
        public string BuyFromVendorName { get; set; }
		public string DocumentDate { get; set; }
		public string RelatedDocument { get; set; }
		public decimal ValorFactura { get; set; }
		public string SourceDocNo { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantityReceived { get; set; }
        public decimal AmountRcdNotInvoiced { get; set; }
        public decimal Amount { get; set; }
    }
}
