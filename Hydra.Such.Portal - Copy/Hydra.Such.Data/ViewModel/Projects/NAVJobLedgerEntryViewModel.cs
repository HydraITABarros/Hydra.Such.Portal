using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Projects
{
    public class NAVJobLedgerEntryViewModel
    {
        public string EntryNo { get; set; }
        public string JobNo { get; set; }
        public string PostingDate { get; set; }
        public string DocumentDate { get; set; }
        public string EntryType { get; set; }
        public string DocumentNo { get; set; }
        public string Type { get; set; }
        public string No { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public string UnitOfMeasureCode { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal LineAmount { get; set; }
        public string LocationCode { get; set; }
    }
}
