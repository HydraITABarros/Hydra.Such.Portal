using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Projects
{
    public class SPInvoiceListViewModel
    {
        public string CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string ServiceDate { get; set; }
        public string ClientRequest { get; set; }
        public string ServiceClientCode { get; set; }
        public string RequestNo { get; set; }
        public int? RequestLineNo { get; set; }
        public string Driver { get; set; }
        public int? ResidueFinalDestinyCode { get; set; }
        public string OriginalDocument { get; set; }
        public string AdjustedDocument { get; set; }
        public bool? AdjustedPrice { get; set; }
        public string AdjustedDocumentData { get; set; }
        public bool? AutorizatedInvoice { get; set; }
        public string AutorizatedInvoiceData { get; set; }
        public int? ResourceType { get; set; }
        public string TimesheetNo { get; set; }
        public string InternalRequest { get; set; }
        public string EmployeeNo { get; set; }
        public decimal? QuantityReturned { get; set; }
        public int LineNo { get; set; }
        public string ProjectNo { get; set; }
        public string Date { get; set; }
        public int? MovementType { get; set; }
        public int? Type { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal? Quantity { get; set; }
        public string MeasurementUnitCode { get; set; }
        public string LocationCode { get; set; }
        public string ProjectContabGroup { get; set; }
        public string RegionCode { get; set; }
        public string FunctionalAreaCode { get; set; }
        public string ResponsabilityCenterCode { get; set; }
        public string User { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? TotalCost { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public bool? Billable { get; set; }
        public string ResidueGuideNo { get; set; }
        public string ExternalGuideNo { get; set; }
        public string InvoiceToClientNo { get; set; }
        public string ClientName { get; set; }
        public string ClientVATReg { get; set; }
        public string CommitmentNumber { get; set; }
        public decimal? UnitValueToInvoice { get; set; }
        public string Currency { get; set; }
        public bool? Billed { get; set; }
        public int? MealType { get; set; }
        public int? ServiceGroupCode { get; set; }
        public string ConsumptionDate { get; set; }
        public bool? Registered { get; set; }
        public string TypeDescription { get; set; }
        public string DocumentNo { get; set; }
        public int? InvoiceGroup { get; set; }
        public string InvoiceGroupDescription { get; set; }
        public string CodTermosPagamento { get; set; }
        //public string PedidoCliente { get; set; }
        public string SituacoesPendentes { get; set; }
        public int? Opcao { get; set; }
        public string Comments { get; set; }
        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }

    }
}
