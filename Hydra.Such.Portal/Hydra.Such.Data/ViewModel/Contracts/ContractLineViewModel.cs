using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Contracts
{
    public class ContractLineViewModel
    {
        public int ContractType { get; set; }
        public string ContractNo { get; set; }
        public int VersionNo { get; set; }
        public int LineNo { get; set; }
        public int? Type { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal? Quantity { get; set; }
        public string CodeMeasureUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? LineDiscount { get; set; }
        public bool? Billable { get; set; }
        public string CodeRegion { get; set; }
        public string CodeFunctionalArea { get; set; }
        public string CodeResponsabilityCenter { get; set; }
        public decimal? Frequency { get; set; }
        public decimal? InterventionHours { get; set; }
        public decimal? TotalTechinicians { get; set; }
        public int? ProposalType { get; set; }
        public string VersionStartDate { get; set; }
        public string VersionEndDate { get; set; }
        public string ResponsibleNo { get; set; }
        public string ServiceClientNo { get; set; }
        public int? InvoiceGroup { get; set; }
        public bool? CreateContract { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public string ProjectNo { get; set; }


        //EXTRAS
        public bool Selected { get; set; }
    }
}
