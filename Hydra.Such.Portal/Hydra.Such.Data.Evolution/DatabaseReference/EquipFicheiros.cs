using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class EquipFicheiros
    {
        public int IdEquipFicheiro { get; set; }
        public int? IdEquipamento { get; set; }
        public byte[] Ficheiro { get; set; }
        public string Extensao { get; set; }
        public DateTime? Data { get; set; }
        public string Nome { get; set; }
    }
}
