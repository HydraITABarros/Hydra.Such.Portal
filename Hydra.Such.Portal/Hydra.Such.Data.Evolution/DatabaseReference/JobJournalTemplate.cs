using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class JobJournalTemplate
    {
        public byte[] Timestamp { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TestReportId { get; set; }
        public int FormId { get; set; }
        public int PostingReportId { get; set; }
        public byte ForcePostingReport { get; set; }
        public string SourceCode { get; set; }
        public string ReasonCode { get; set; }
        public byte Recurring { get; set; }
        public string NoSeries { get; set; }
        public string PostingNoSeries { get; set; }
        public byte FolhaHoras { get; set; }
        public int FormIdRequisition { get; set; }
        public int FormIdTratamentoAmbiente { get; set; }
        public int FormIdTratamentoRoupa { get; set; }
        public int FormIdAlimentação { get; set; }
        public int FormIdProjectosEObras { get; set; }
        public int FormIdRejeicoes { get; set; }
        public int FormIdTratamento { get; set; }
    }
}
