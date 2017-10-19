using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Contracts
{
    public class ContractViewModel
    {
        public int ContractType { get; set; }
        public string ContractNo { get; set; }
        public int VersionNo { get; set; }
        public int? Area { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
        public int? ChangeStatus { get; set; }
        public string ClientNo { get; set; }
        public string CodeRegion { get; set; }
        public string CodeFunctionalArea { get; set; }
        public string CodeResponsabilityCenter { get; set; }
        public string CodeShippingAddress { get; set; }
        public string ShippingName { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingZipCode { get; set; }
        public string ShippingLocality { get; set; }
        public int? InvocePeriod { get; set; }
        public DateTime? LastInvoiceDate { get; set; }
        public DateTime? NextInvoiceDate { get; set; }
        public DateTime? StartData { get; set; }
        public DateTime? DueDate { get; set; }
        public bool? BatchInvoices { get; set; }
        public string NextBillingPeriod { get; set; }
        public bool? ContractLinesInBilling { get; set; }
        public string CodePaymentTerms { get; set; }
        public int? ProposalType { get; set; }
        public int? BillingType { get; set; }
        public int? MaintenanceContractType { get; set; }
        public string ClientRequestNo { get; set; }
        public DateTime? ReceiptDateRequest { get; set; }
        public string PromiseNo { get; set; }
        public decimal? ProvisioningFee { get; set; }
        public decimal? Mc { get; set; }
        public decimal? DisplacementFee { get; set; }
        public bool? FixedVowsAgreement { get; set; }
        public int? ServiceObject { get; set; }
        public bool? VariableAvengeAgrement { get; set; }
        public string Notes { get; set; }
        //public string NºContrato { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public string ContractDurationDescription { get; set; }
        public DateTime? StartDateFirstContract { get; set; }
        public string FirstContractReference { get; set; }
        public DateTime? ContractMaxDuration { get; set; }
        public int? TerminationTermNotice { get; set; }
        public int? RenovationConditions { get; set; }
        public string RenovationConditionsAnother { get; set; }
        public int? PaymentTerms { get; set; }
        public string PaymentTermsAnother { get; set; }
        public bool? CustomerSigned { get; set; }
        public bool? Interests { get; set; }
        public DateTime? SignatureDate { get; set; }
        public DateTime? CustomerShipmentDate { get; set; }
        public int? ProvisionUnit { get; set; }
        public string ContractReference { get; set; }
        public decimal? TotalProposalValue { get; set; }
        public string PhysicalFileLocation { get; set; }
        public string OportunityNo { get; set; }
        public string ProposalNo { get; set; }
        public string ContactNo { get; set; }
        public DateTime? DateProposedState { get; set; }
        public int? OrderOrigin { get; set; }
        public string OrdOrderSource { get; set; }
        public string InternalNumeration { get; set; }
        public DateTime? ProposalChangeDate { get; set; }
        public DateTime? LimitClarificationDate { get; set; }
        public DateTime? ErrorsOmissionsDate { get; set; }
        public DateTime? FinalReportDate { get; set; }
        public DateTime? DocumentationHabilitationDate { get; set; }
        public bool? CompulsoryCompulsoryNo { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }
        public bool? Filed { get; set; }
    }
}
