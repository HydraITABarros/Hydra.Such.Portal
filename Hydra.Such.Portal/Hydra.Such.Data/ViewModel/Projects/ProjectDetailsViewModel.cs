using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Projects
{
    public class ProjectDetailsViewModel
    {
        public string ProjectNo { get; set; }
        public int? Area { get; set; }
        public string Description { get; set; }
        public string ClientNo { get; set; }
        public string Date { get; set; }
        public int? Status { get; set; }
        public string RegionCode { get; set; }
        public string FunctionalAreaCode { get; set; }
        public string ResponsabilityCenterCode { get; set; }
        public bool? Billable { get; set; }
        public string ContractNo { get; set; }
        public string ShippingAddressCode { get; set; }
        public string ShippingName { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingPostalCode { get; set; }
        public string ShippingLocality { get; set; }
        public string ShippingContact { get; set; }
        public int? ProjectTypeCode { get; set; }
        public string OurProposal { get; set; }
        public int? ServiceObjectCode { get; set; }
        public string CommitmentCode { get; set; }
        public string AccountWorkGroup { get; set; }
        public int? GroupContabProjectType { get; set; }
        public int? GroupContabOMProjectType { get; set; }
        public string ClientRequest { get; set; }
        public string RequestDate { get; set; }
        public DateTime? RequestValidity { get; set; }
        public string DetailedDescription { get; set; }
        public int? ProjectCategory { get; set; }
        public string BudgetContractNo { get; set; }
        public bool? InternalProject { get; set; }
        public string ProjectLeader { get; set; }
        public string ProjectResponsible { get; set; }
    }
}
