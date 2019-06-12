using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class EmmAnexos
    {
        public int Id { get; set; }
        public int? IdGrupo { get; set; }
        public int? NumEmm { get; set; }
        public int? IdEquipamento { get; set; }
        public int? IdUtilizador { get; set; }
        public string Nome { get; set; }
        public string Extensao { get; set; }
        public byte[] Anexo { get; set; }
        public DateTime? Data { get; set; }
        public bool? Activo { get; set; }
    }
}
