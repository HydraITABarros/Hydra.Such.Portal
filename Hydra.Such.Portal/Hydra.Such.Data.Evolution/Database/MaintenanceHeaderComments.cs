using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class MaintenanceHeaderComments
    {
        public byte[] Timestamp { get; set; }
        public int TableName { get; set; }
        public string No { get; set; }
        public int LineNo { get; set; }
        public DateTime Date { get; set; }
        public string Code { get; set; }
        public string Comment { get; set; }
        public int OrcAlternativo { get; set; }
    }
}
