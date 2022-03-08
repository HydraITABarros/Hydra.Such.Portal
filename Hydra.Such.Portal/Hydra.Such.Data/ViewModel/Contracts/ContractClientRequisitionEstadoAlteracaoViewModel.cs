using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Contracts
{
    public class ContractClientRequisitionEstadoAlteracaoViewModel
    {
        public int NoLinha { get; set; }
        public string ContractNo { get; set; }
        public int InvoiceGroup { get; set; }
        public string ProjectNo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ClientRequisitionNo { get; set; }
        public string RequisitionDate { get; set; }
        public string PromiseNo { get; set; }
        public string LastInvoiceDate { get; set; }
        public string InvoiceNo { get; set; }
        public decimal? InvoiceValue { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }
}
