using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class FichaManutencao
    {
        public string Codigo { get; set; }
        public string Designacao { get; set; }
        public DateTime Data { get; set; }
        public byte Aprovada { get; set; }
        public int? AprovadaPor { get; set; }
        public DateTime? AprovadaEm { get; set; }
        public string Versao { get; set; }
        public DateTime? PeriodoInicio { get; set; }
        public DateTime? PeriodoFim { get; set; }
        public int? IdCliente { get; set; }
        public int IdCategoria { get; set; }
        public string AreaOperacional { get; set; }
        public byte ParaAprovacao { get; set; }
        public int IdImagem { get; set; }
        public int? IdTipo { get; set; }
    }
}
