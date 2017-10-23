using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
{
    public partial class MovimentosDeAprovação
    {
        public int NºMovimento { get; set; }
        public int? Tipo { get; set; }
        public int? Área { get; set; }
        public string Número { get; set; }
        public string UtilizadorSolicitou { get; set; }
        public string UtilizadorAprovador { get; set; }
        public int? GrupoAprovador { get; set; }
        public decimal? Valor { get; set; }
        public bool? Aprovado { get; set; }
        public string UtilizadorAprovação { get; set; }
        public DateTime? DataHoraAprovação { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }

        public GruposAprovação GrupoAprovadorNavigation { get; set; }
    }
}
