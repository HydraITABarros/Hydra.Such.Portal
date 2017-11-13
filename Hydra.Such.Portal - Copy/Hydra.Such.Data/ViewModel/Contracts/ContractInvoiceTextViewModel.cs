using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Contracts
{
    public class ContractInvoiceTextViewModel
    {
        public string ContractNo { get; set; }
        public int InvoiceGroup { get; set; }
        public string ProjectNo { get; set; }
        public string InvoiceText { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }
}
