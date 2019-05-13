using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class EmmInspecaoRotina
    {
        public int Id { get; set; }
        public int IdInspecao { get; set; }
        public int? IdGrupo { get; set; }
        public int? NumEmm { get; set; }
        public int? IdSetPoint { get; set; }
        public int? EmmPadraoEquipamento { get; set; }
        public int? EmmPadraoAcessorio { get; set; }
        public decimal? ErroIncerteza { get; set; }
        public decimal? PontoCalibracao { get; set; }
        public int? NumMedida { get; set; }
        public decimal? LmP { get; set; }
        public decimal? LmT { get; set; }
        public decimal? ErroT { get; set; }
        public bool? CalculoAceitacao { get; set; }
        public string Ema { get; set; }
        public bool Activo { get; set; }
    }
}
