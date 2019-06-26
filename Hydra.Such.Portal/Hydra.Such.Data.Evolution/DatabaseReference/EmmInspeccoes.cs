using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class EmmInspeccoes
    {
        public int Id { get; set; }
        public int IdInspecao { get; set; }
        public int? IdTipo { get; set; }
        public int? IdEstado { get; set; }
        public DateTime? DataEstado { get; set; }
        public int? IdGrupo { get; set; }
        public int? NumEmm { get; set; }
        public string IdSupervisor { get; set; }
        public string NumRequesicao { get; set; }
        public DateTime? DataLimite { get; set; }
        public DateTime? DataInspecao { get; set; }
        public int? IdEntidade { get; set; }
        public string IdLaboratorio { get; set; }
        public int? IdResultado { get; set; }
        public string NumCertificado { get; set; }
        public string Informacao { get; set; }
        public decimal? TempAmbiente { get; set; }
        public bool? Limpeza { get; set; }
        public bool? Conservacao { get; set; }
        public string Observacao { get; set; }
        public bool? Activo { get; set; }
    }
}
