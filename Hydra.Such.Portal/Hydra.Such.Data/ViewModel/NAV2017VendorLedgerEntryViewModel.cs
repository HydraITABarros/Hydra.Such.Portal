using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class NAV2017VendorLedgerEntryViewModel
    {
        public string VendorNo { get; set; }
        public int? DocumentType { get; set; }
        public int? Open { get; set; }
        public DateTime? PostingDate { get; set; }
    }
}
