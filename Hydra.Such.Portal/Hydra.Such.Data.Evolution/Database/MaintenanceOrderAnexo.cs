using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class MaintenanceOrderAnexo
    {
        public int AnexNo { get; set; }
        public string MoNo { get; set; }
        public int IdUser { get; set; }
        public byte[] Ficheiro { get; set; }
        public string Extensao { get; set; }
        public string Nome { get; set; }
        public DateTime? Data { get; set; }
    }
}
