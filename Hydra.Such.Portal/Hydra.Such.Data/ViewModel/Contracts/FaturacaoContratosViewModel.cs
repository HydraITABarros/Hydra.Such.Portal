using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Contracts
{
    public class FaturacaoContratosViewModel
    {
        public string ContractNo { get; set; }
        //public int GrupoFatura { get; set; }
        public string Description { get; set; }
        public string ClientNo { get; set; }
        public string ClientName { get; set; }
        public decimal? InvoiceValue { get; set; }
        public int? NumberOfInvoices { get; set; }
        public decimal? InvoiceTotal { get; set; }
        public decimal? ContractValue { get; set; }
        public decimal? ValueToInvoice { get; set; }
        public decimal? BilledValue { get; set; }
        public string RegionCode { get; set; }
        public string FunctionalAreaCode { get; set; }
        public string ResponsabilityCenterCode { get; set; }
        //public DateTime? DataInicial { get; set; }
        //public DateTime? DataDeExpiração { get; set; }
        public DateTime? NextInvoiceDate { get; set; }
        public DateTime? RegisterDate { get; set; }
        public int? Status { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string CreationUser { get; set; }
        public string ModificationUser { get; set; }
    }
}
