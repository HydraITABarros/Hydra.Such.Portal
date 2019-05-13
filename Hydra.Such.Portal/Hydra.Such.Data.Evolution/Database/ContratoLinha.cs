using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class ContratoLinha
    {
        public ContratoLinha()
        {
            ClientePimp = new HashSet<ClientePimp>();
        }

        public int IdContratoLinha { get; set; }
        public string IdContrato { get; set; }
        public int? TreePath { get; set; }
        public int? IdRegiao { get; set; }
        public int? IdArea { get; set; }
        public int? IdEquipa { get; set; }
        public int? IdAreaOp { get; set; }
        public int? Periodicidade { get; set; }
        public decimal? NumHorasIntervencao { get; set; }
        public decimal? ValorMensal { get; set; }
        public string Descricao { get; set; }
        public string Obs { get; set; }
        public int? IdEstado { get; set; }
        public bool? Activo { get; set; }
        public int? NumTecnicos { get; set; }
        public string RegiaoNav { get; set; }
        public string AreaNav { get; set; }
        public string EquipaNav { get; set; }
        public decimal? NumTecnicosNav { get; set; }
        public int? LinhaNum { get; set; }
        public int? NumVersao { get; set; }
        public decimal? Quantidade { get; set; }

        public virtual ICollection<ClientePimp> ClientePimp { get; set; }
    }
}
