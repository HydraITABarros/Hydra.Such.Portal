using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    [ModelMetadataType(typeof(IMaintenanceOrderLine))]
    public partial class MaintenanceOrderLine
    {
    }

    public interface IMaintenanceOrderLine
    {
        byte[] Timestamp { get; set; }
        int DocumentType { get; set; }
        string MoNo { get; set; }
        int LineNo { get; set; }
        int? OrderStatus { get; set; }
        string SortField { get; set; }
        int? ObjectRefType { get; set; }
        string ObjectRefNo { get; set; }
        int? ObjectType { get; set; }
        string ObjectNo { get; set; }
        string ObjectDescription { get; set; }
        string ObjectDescription2 { get; set; }
        string FunctionalLocationNo { get; set; }
        string TaskListNo { get; set; }
        int? Priority { get; set; }
        string AdditionalData { get; set; }
        byte? Warranty { get; set; }
        string BomNo { get; set; }
        DateTime? WarrantyDate { get; set; }
        string ComponentOf { get; set; }
        int? CriticalLevel { get; set; }
        decimal? ResponseTimeHours { get; set; }
        decimal? MaintenanceTimeHours { get; set; }
        DateTime? StartingDate { get; set; }
        DateTime? StartingTime { get; set; }
        DateTime? FinishingDate { get; set; }
        DateTime? FinishingTime { get; set; }
        int? NotificationType { get; set; }
        string NotificationNo { get; set; }
        string ShortcutDimension1Code { get; set; }
        string ShortcutDimension2Code { get; set; }
        string JobNo { get; set; }
        int? LineStatus { get; set; }
        string CustomerNo { get; set; }
        string ResponsibilityCenter { get; set; }
        string PlannerGroupNo { get; set; }
        string ResourceNo { get; set; }
        DateTime? PostingDate { get; set; }
        DateTime? DocumentDate { get; set; }
        DateTime? OrderDate { get; set; }
        DateTime? OrderTime { get; set; }
        string OrderType { get; set; }
        byte? ResourceFilterYesNo { get; set; }
        int? FinalState { get; set; }
        byte? UsedDmmFilterYesNo { get; set; }
        string ShortcutDimension3Code { get; set; }
        string ShortcutDimension4Code { get; set; }
        byte? LinhaOrçamento { get; set; }
        string InventoryNo { get; set; }
        int? EstadoLinhasOrçamento { get; set; }
        int? FaultReasonCode { get; set; }
        int? IdEquipamento { get; set; }
        int? IdEquipEstado { get; set; }
        int? IdRotina { get; set; }
        int? Tbf { get; set; }
        int? IdInstituicao { get; set; }
        int? IdServico { get; set; }
    }
}
