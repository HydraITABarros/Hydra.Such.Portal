using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class MoTasks
    {
        public byte[] Timestamp { get; set; }
        public int DocumentType { get; set; }
        public string MoNo { get; set; }
        public int MoLineNo { get; set; }
        public int LineNo { get; set; }
        public string TaskListNo { get; set; }
        public int No { get; set; }
        public string ResourceNo { get; set; }
        public string Description { get; set; }
        public decimal ProcessTime { get; set; }
        public string ProcessTimeUnitOfMeasure { get; set; }
        public string Description2 { get; set; }
        public decimal ConcurrentCapacities { get; set; }
        public decimal QtyPerUnitOfMeasure { get; set; }
        public decimal Duration { get; set; }
        public string DurationUnitOfMeasure { get; set; }
        public decimal UnitCostPer { get; set; }
        public decimal CostAmount { get; set; }
        public decimal DirectUnitCost { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Profit { get; set; }
        public string LocationCode { get; set; }
        public string OperationCondition { get; set; }
        public string MaintenanceActivity { get; set; }
        public string SkillCode { get; set; }
        public string TaskListLinkCode { get; set; }
        public string GenProductPostingGroup { get; set; }
        public string GenBusPostingGroup { get; set; }
        public byte Chargeable { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ContractNo { get; set; }
        public int ObjectType { get; set; }
        public string ObjectNo { get; set; }
        public string ObjectDescription { get; set; }
        public string CustomerNo { get; set; }
        public string JobNo { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime OrderTime { get; set; }
        public string ResourceGroupNo { get; set; }
        public string WorkTypeCode { get; set; }
        public string ShortcutDimension1Code { get; set; }
        public string ShortcutDimension2Code { get; set; }
        public byte Comment { get; set; }
        public int ObjectRefType { get; set; }
        public string ObjectRefNo { get; set; }
        public decimal QuantityBase { get; set; }
        public decimal QtyToInvoice { get; set; }
        public decimal QtyToInvoiceBase { get; set; }
        public decimal OutstandingQtyBase { get; set; }
        public string TransactionType { get; set; }
        public string Area { get; set; }
        public string TransactionSpecification { get; set; }
        public string ShortcutDimension3Code { get; set; }
        public string ShortcutDimension4Code { get; set; }
        public int TipoRecurso { get; set; }
        public int NºOrçamentoAs4000 { get; set; }
        public int Estado { get; set; }
        public int? OrcAlternativo { get; set; }
    }
}
