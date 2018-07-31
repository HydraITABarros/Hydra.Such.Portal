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
        public string NextInvoiceDate { get; set; }
        public string RegisterDate { get; set; }
        public string Situation { get; set; } 
        public int? Status { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string CreationUser { get; set; }
        public string ModificationUser { get; set; }
        public decimal InvoiceGroupCount { get; set; }
        public int? InvoiceGroupValue { get; set; }
        public string Document_No { get; set; }
        public string ExpiryDate { get; set; }
        public string StartDate { get; set; }
        public int? InvoicePeriod { get; set; }
        public decimal InvoiceGroupValue { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }
    }
}
