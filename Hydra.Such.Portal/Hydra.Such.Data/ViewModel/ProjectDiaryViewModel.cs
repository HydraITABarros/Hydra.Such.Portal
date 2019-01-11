using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class ProjectDiaryViewModel : ErrorHandler
    {
        public int LineNo { get; set; }
        public string ProjectNo { get; set; }
        public string Date { get; set; }
        public int? MovementType { get; set; }
        public string MovementTypeText { get; set; }
        public string DocumentNo { get; set; }
        public int? Type { get; set; }
        public string TypeText { get; set; }
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
        public string BillableText { get; set; }
        public string ResidueGuideNo { get; set; }
        public string ExternalGuideNo { get; set; }
        public string InvoiceToClientNo { get; set; }
        public string ClientName { get; set; }
        public string ClientVATReg { get; set; }
        public string CommitmentNumber { get; set; }
        public decimal? UnitValueToInvoice { get; set; }
        public string Currency { get; set; }
        public bool Billed { get; set; }
        public string BilledText { get; set; }
        public string FolhaHoras { get; set; }
        //public string RequisitionNo { get; set; }
        //public int? RequisitionLineNo { get; set; }
        //public string Driver { get; set; }
        public int? MealType { get; set; }
        public string MealTypeDescription { get; set; }
        //public int? FinalDestintyResidueCode { get; set; }
        //public string OriginalDocument { get; set; }
        //public string CorrectedDocument { get; set; }
        //public bool? PriceAdjustment { get; set; }
        //public DateTime? CorrectedDocumentDate { get; set; }
        //public bool? AuthorizedInvoice { get; set; }
        //public DateTime? AuthorizedInvoiceDate { get; set; }
        public string ServiceGroupCode { get; set; }
        //public int? ResourceType { get; set; }
        //public string TimeSheetNo { get; set; }
        //public string InternalRequisition { get; set; }
        //public string EmployeeNo { get; set; }
        //public decimal? ReturnedQuantity { get; set; }
        public string ConsumptionDate { get; set; }
        public bool? Registered { get; set; }
        public string RegisteredText { get; set; }
        public string TypeDescription { get; set; }
        //public DestinosFinaisResíduos CódDestinoFinalResíduosNavigation { get; set; }
        //public LinhasRequisição Nº { get; set; }
        //public Projetos NºProjetoNavigation { get; set; }
        //public Requisição NºRequisiçãoNavigation { get; set; }
        //public TiposRefeição TipoRefeiçãoNavigation { get; set; }
        public bool Selected { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }




        public string ServiceData { get; set; }
        public string ClientRequest { get; set; }
        public string ServiceClientCode { get; set; }
        public string RequestNo { get; set; }
        public int? RequestLineNo { get; set; }
        public string Driver { get; set; }
        public int? ResidueFinalDestinyCode { get; set; }
        public string OriginalDocument { get; set; }
        public string AdjustedDocument { get; set; }
        public bool? AdjustedPrice { get; set; }
        public string AdjustedPriceText { get; set; }
        public string AdjustedDocumentData { get; set; }
        public bool? AutorizatedInvoice { get; set; }
        public string AutorizatedInvoiceText { get; set; }
        public bool? AutorizatedInvoice2 { get; set; }
        public string AutorizatedInvoice2Text { get; set; }
        public string AutorizatedInvoiceData { get; set; }
        public int? ResourceType { get; set; }
        public string TimesheetNo { get; set; }
        public string InternalRequest { get; set; }
        public string EmployeeNo { get; set; }
        public decimal QuantityReturned { get; set; }
        public string Coin { get; set; }
        public bool? PreRegistered { get; set; }
        public string ServiceClientDescription { get; set; }


        public string LicensePlate { get; set; }
        public string ReadingCode { get; set; }
        public string Group { get; set; }
        public string Operation { get; set; }
        public int? InvoiceGroup { get; set; }
        public string InvoiceGroupDescription { get; set; }
        public string AuthorizedBy { get; set; }


        public string Utilizador { get; set; }
        public string NameDB { get; set; }
        public string CompanyName { get; set; }
    }
}
