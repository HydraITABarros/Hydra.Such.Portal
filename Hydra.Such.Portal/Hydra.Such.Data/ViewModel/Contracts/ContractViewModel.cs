using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Contracts
{
    public class ContractViewModel : ErrorHandler
    {
        public int ContractType { get; set; }
        public string ContractNo { get; set; }
        public int VersionNo { get; set; }
        public int? Area { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
        public string StatusDescription { get; set; }
        public int? ChangeStatus { get; set; }
        public string ClientNo { get; set; }
        public string ClientName { get; set; }
        public string CodeRegion { get; set; }
        public string CodeFunctionalArea { get; set; }
        public string CodeResponsabilityCenter { get; set; }
        public string CodeShippingAddress { get; set; }
        public string ShippingName { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingZipCode { get; set; }
        public string ShippingLocality { get; set; }
        public int? InvocePeriod { get; set; }
        public string LastInvoiceDate { get; set; }
        public string NextInvoiceDate { get; set; }
        public string StartData { get; set; }
        public string EndData { get; set; }
        public string DueDate { get; set; }
        public bool? BatchInvoices { get; set; }
        public string NextBillingPeriod { get; set; }
        public bool? ContractLinesInBilling { get; set; }
        public string CodePaymentTerms { get; set; }
        public int? ProposalType { get; set; }
        public int? BillingType { get; set; }
        public int? MaintenanceContractType { get; set; }
        public string ClientRequisitionNo { get; set; }
        public string ReceiptDateRequisition { get; set; }
        public string PromiseNo { get; set; }
        public decimal? ProvisioningFee { get; set; }
        public decimal? Mc { get; set; }
        public decimal? DisplacementFee { get; set; }
        public bool? FixedVowsAgreement { get; set; }
        public int? ServiceObject { get; set; }
        public bool? VariableAvengeAgrement { get; set; }
        public string Notes { get; set; }
        public string RelatedContract { get; set; }
        public string ContractStartDate { get; set; }
        public string ContractEndDate { get; set; }
        public string ContractDurationDescription { get; set; }
        public string StartDateFirstContract { get; set; }
        public string FirstContractReference { get; set; }
        public string ContractMaxDuration { get; set; }
        public int? TerminationTermNotice { get; set; }
        public int? RenovationConditions { get; set; }
        public string RenovationConditionsAnother { get; set; }
        public int? PaymentTerms { get; set; }
        public string PaymentTermsAnother { get; set; }
        public bool? CustomerSigned { get; set; }
        public bool? Interests { get; set; }
        public string SignatureDate { get; set; }
        public string CustomerShipmentDate { get; set; }
        public int? ProvisionUnit { get; set; }
        public string ProvisionUnitText { get; set; }
        public string ContractReference { get; set; }
        public decimal? TotalProposalValue { get; set; }
        public string PhysicalFileLocation { get; set; }
        public string OportunityNo { get; set; }
        public string ProposalNo { get; set; }
        public string ContactNo { get; set; }
        public string ContactNoText { get; set; }
        public string DateProposedState { get; set; }
        public int? OrderOrigin { get; set; }
        public string OrderOriginText { get; set; }
        public string OrdOrderSource { get; set; }
        public string InternalNumeration { get; set; }
        public string ProposalChangeDate { get; set; }
        public string ProposalChangeTime { get; set; }
        public string LimitClarificationDate { get; set; }
        public string LimitClarificationTime { get; set; }
        public string ErrorsOmissionsDate { get; set; }
        public string ErrorsOmissionsTime { get; set; }
        public string FinalReportDate { get; set; }
        public string FinalReportTime { get; set; }
        public string DocumentationHabilitationDate { get; set; }
        public string DocumentationHabilitationTime { get; set; }
        public bool? CompulsoryCompulsoryNo { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }
        public bool? Filed { get; set; }
        public string ArchiveReason { get; set; }
        public int? RequestOrigin { get; set; }
        public string RequestOriginDescription { get; set; }
        public string SentData { get; set; }
        public string UpdateData { get; set; }
        public decimal? TotalValue { get; set; }
        public string ClarificationLimite { get; set; }
        public string ProposalDelivery { get; set; }
        public string ProposalDeliveryTime { get; set; }
        public string LastReport { get; set; }
        public string NextInvoicePeriod { get; set; }
        public decimal? BaseValueProcedure { get; set; }
        public string PreviousHearing { get; set; }
        public string PreviousHearingTime { get; set; }
        public int? ActionCode { get; set; }
        public bool? History { get; set; }
        public int Type { get; set; }
        public string NoVEP { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }

        public List<ContractClientRequisitionViewModel> ClientRequisitions { get; set; }
        public List<ContractInvoiceTextViewModel> InvoiceTexts { get; set; }
        public List<ContractLineViewModel> Lines { get; set; }

        public ContractViewModel()
        {
            Lines = new List<ContractLineViewModel>();
        }
    }

    public class UpdateContractRequest
    {
        public ContractViewModel Contract;
        public bool PartialUpdateOnly;
    }

    public enum ContractType
    {
        Oportunity = 1,
        Proposal = 2,
        Contract = 3
    }

    public class UpdateContractPricesRequest
    {
        public int ContractType { get; set; }
        public string ContractNo { get; set; }
        public int VersionNo { get; set; }
        
        public string NextInvoiceDate { get; set; }
        public string StartDate { get; set; }
        public string DueDate { get; set; }
        public string ClientRequisitionNo { get; set; }
        public string RequisitionReceiveDate { get; set; }
        public decimal percentageToApllyInLines { get; set; }

        //public UpdateContractPricesRequest()
        //{
        //    Lines = new List<ContractLineViewModel>();
        //}
    }
}
