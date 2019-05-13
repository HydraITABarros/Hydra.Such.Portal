using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class EmmInspecaoPadraoTmp
    {
        public int Id { get; set; }
        public string Timestamp { get; set; }
        public int? IdSetPoint { get; set; }
        public decimal? Erro { get; set; }
        public decimal? Incerteza { get; set; }
        public decimal? ErroIncerteza { get; set; }
        public decimal? PontoCalibracao { get; set; }
        public bool Activo { get; set; }
    }
}
