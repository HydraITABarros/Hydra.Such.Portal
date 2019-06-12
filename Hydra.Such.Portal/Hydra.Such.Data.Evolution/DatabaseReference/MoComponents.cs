using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class MoComponents
    {
        public byte[] Timestamp { get; set; }
        public int DocumentType { get; set; }
        public string MoNo { get; set; }
        public int MoLineNo { get; set; }
        public int LineNo { get; set; }
        public int Type { get; set; }
        public string No { get; set; }
        public string Description { get; set; }
        public string VariantCode { get; set; }
        public byte CreatedFromNonstockItem { get; set; }
        public string BinCode { get; set; }
        public string LocationCode { get; set; }
        public string Description2 { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantityBase { get; set; }
        public decimal QtyToInvoice { get; set; }
        public decimal QtyToInvoiceBase { get; set; }
        public string UnitOfMeasureCode { get; set; }
        public decimal OutstandingQtyBase { get; set; }
        public decimal UnitCost { get; set; }
        public decimal CostAmount { get; set; }
        public decimal QtyPerUnitOfMeasure { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Profit { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerPriceGroup { get; set; }
        public string GenProductPostingGroup { get; set; }
        public string PostingGroup { get; set; }
        public byte Chargeable { get; set; }
        public int ObjectType { get; set; }
        public string ObjectNo { get; set; }
        public string ObjectDescription { get; set; }
        public string BomNo { get; set; }
        public string TaskListNo { get; set; }
        public int TaskNo { get; set; }
        public string JobNo { get; set; }
        public string ContractNo { get; set; }
        public string GenBusPostingGroup { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime OrderTime { get; set; }
        public string ShortcutDimension1Code { get; set; }
        public string ShortcutDimension2Code { get; set; }
        public string Class { get; set; }
        public string ProductGroupCode { get; set; }
        public string ItemCategoryCode { get; set; }
        public string TransactionType { get; set; }
        public string TransportMethod { get; set; }
        public string CountryCode { get; set; }
        public string EntryExitPoint { get; set; }
        public string Area { get; set; }
        public string TransactionSpecification { get; set; }
        public string TaskListLinkCode { get; set; }
        public DateTime Date { get; set; }
        public decimal ReservedQtyBase { get; set; }
        public decimal ReservedQuantity { get; set; }
        public int Reserve { get; set; }
        public int ObjectRefType { get; set; }
        public string ObjectRefNo { get; set; }
        public decimal RequestedQty { get; set; }
        public decimal OutstandingQty { get; set; }
        public string ShortcutDimension3Code { get; set; }
        public string ShortcutDimension4Code { get; set; }
        public int NºOrçamentoAs400 { get; set; }
        public int Estado { get; set; }
        public decimal TaxaAprovisionamento { get; set; }
        public int? OrcAlternativo { get; set; }
    }
}
