using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.OData.Edm.Library;

namespace Hydra.Such.Data.ViewModel
{
   public class NAVOpenOrderLinesViewModels
    {
        public string id { get; set; }
        public int DocumentType { get; set; }
        public string DocumentNO { get; set; }
        public int Line_No { get; set; }
        public string BuyFromVendorNo { get; set; }
        public int Type { get; set; }
        public string Numb { get; set; }
        public string LocationCode { get; set; }
        public DateTime ExpectedReceiptDate { get; set; }
        public string Description { get; set; }
        public string UnitofMeasure { get; set; }
        public decimal Quantity { get; set; }
        public decimal OutstandingQuantity { get; set; }
        public decimal QtytoInvoice { get; set; }
        public decimal QtytoReceive { get; set; }
        public decimal DirectUnitCost { get; set; }
        public decimal UnitCostLCY { get; set; }
        public decimal Amount { get; set; }
        public decimal UnitPriceLCY { get; set; }
        public decimal OutstandingAmount { get; set; }
        public decimal QuantityReceived { get; set; }
        public decimal QuantityInvoiced { get; set; }
        public string PaytoVendorNo { get; set; }
        public string VendorItemNo { get; set; }
        public string GenBusPostingGroup { get; set; }
        public string GenProdPostingGroup { get; set; }
        public string EntryPoint { get; set; }
        public string CurrencyCode { get; set; }
        public decimal VATBaseAmount { get; set; }
        public decimal UnitCost { get; set; }
        public int SystemCreatedEntry { get; set; }
        public decimal LineAmount { get; set; }
        public int ICPartnerRefType { get; set; }
        public string ICPartnerReference { get; set; }
        public string ICPartnerCode { get; set; }
        public int DimensionSetID { get; set; }
        public DateTime ReturnsDeferralStartDate { get; set; }
        public string ProdOrderNo { get; set; }
        public decimal QtyperUnitofMeasure { get; set; }
        public string UnitofMeasureCode { get; set; }
        public decimal QuantityBase { get; set; }
        public decimal OutstandingQtyBase { get; set; }
        public decimal QtytoInvoiceBase { get; set; }
        public decimal QtytoReceiveBase { get; set; }
        public decimal QtyRcdNotInvoicedBase { get; set; }
        public decimal QtyInvoicedBase { get; set; }
        public decimal SalvageValue { get; set; }
        public string UnitofMeasureCrossRef { get; set; }
        public string ItemCategoryCode { get; set; }
        public string ProductGroupCode { get; set; }
        public DateTime OrderDate { get; set; }
        public string WorkCenterNo { get; set; }
        public int Finished { get; set; }
        public int ProdOrderLineN { get; set; }
        public DateTime ContractStartingDate { get; set; }
        public DateTime ContractEndingDate { get; set; }

    }
}
