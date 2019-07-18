using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class EmmGamaUtilizacaoTmp
    {
        public int Id { get; set; }
        public int IdGrupo { get; set; }
        public string Timestamp { get; set; }
        public string Parametro { get; set; }
        public string ValorMinimo { get; set; }
        public string ValorMaximo { get; set; }
        public string PontosInspecao { get; set; }
        public string Tolerancia { get; set; }
        public string Ema { get; set; }
        public bool? Activo { get; set; }
    }
}
