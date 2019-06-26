using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class PostedMaintenanceOrderLine
    {
        public byte[] Timestamp { get; set; }
        public int DocumentType { get; set; }
        public string MoNo { get; set; }
        public int LineNo { get; set; }
        public int OrderStatus { get; set; }
        public string SortField { get; set; }
        public int ObjectType { get; set; }
        public string ObjectNo { get; set; }
        public string ObjectDescription { get; set; }
        public string ObjectDescription2 { get; set; }
        public string FunctionalLocationNo { get; set; }
        public string TaskListNo { get; set; }
        public int Priority { get; set; }
        public string AdditionalData { get; set; }
        public byte Warranty { get; set; }
        public string BomNo { get; set; }
        public DateTime WarrantyDate { get; set; }
        public string ComponentOf { get; set; }
        public decimal ResponseTimeHours { get; set; }
        public decimal MaintenanceTimeHours { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime StartingTime { get; set; }
        public DateTime FinishingDate { get; set; }
        public DateTime FinishingTime { get; set; }
        public int NotificationType { get; set; }
        public string NotificationNo { get; set; }
        public string ShortcutDimension1Code { get; set; }
        public string ShortcutDimension2Code { get; set; }
        public string ContractNo { get; set; }
        public string JobNo { get; set; }
        public int LineStatus { get; set; }
        public string CustomerNo { get; set; }
        public string ResponsibilityCenter { get; set; }
        public string PlannerGroupNo { get; set; }
        public string ResourceNo { get; set; }
        public DateTime PostingDate { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime OrderTime { get; set; }
        public byte ResourceFilterYesNo { get; set; }
        public byte GuiaDeTransporte { get; set; }
        public DateTime DataEntrada { get; set; }
        public int FinalState { get; set; }
        public string ShortcutDimension3Code { get; set; }
        public string ShortcutDimension4Code { get; set; }
        public string InventoryNo { get; set; }
        public int FaultReasonCode { get; set; }
        public int? IdEquipamento { get; set; }
        public int? IdEquipEstado { get; set; }
        public int? IdRotina { get; set; }
        public int? Tbf { get; set; }
        public int? IdInstituicao { get; set; }
        public int? IdServico { get; set; }
    }
}
