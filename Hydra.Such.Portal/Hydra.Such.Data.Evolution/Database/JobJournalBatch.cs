using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class JobJournalBatch
    {
        public byte[] Timestamp { get; set; }
        public string JournalTemplateName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ReasonCode { get; set; }
        public string NoSeries { get; set; }
        public string PostingNoSeries { get; set; }
        public byte RequisitionBatch { get; set; }
        public byte FolhaHoras { get; set; }
        public byte Receitas { get; set; }
        public byte Desperdicios { get; set; }
        public byte Gtroupas { get; set; }
    }
}
