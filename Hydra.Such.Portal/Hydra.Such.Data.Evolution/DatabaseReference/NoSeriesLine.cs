using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class NoSeriesLine
    {
        public byte[] Timestamp { get; set; }
        public string SeriesCode { get; set; }
        public int LineNo { get; set; }
        public DateTime StartingDate { get; set; }
        public string StartingNo { get; set; }
        public string EndingNo { get; set; }
        public string WarningNo { get; set; }
        public int IncrementByNo { get; set; }
        public string LastNoUsed { get; set; }
        public byte Open { get; set; }
        public DateTime LastDateUsed { get; set; }
        public string ConfigControlo { get; set; }
        public DateTime DataFacturação { get; set; }
        public string LastHashUsed { get; set; }
        public string LastNoPosted { get; set; }
        public DateTime PreviousLastDateUsed { get; set; }
    }
}
