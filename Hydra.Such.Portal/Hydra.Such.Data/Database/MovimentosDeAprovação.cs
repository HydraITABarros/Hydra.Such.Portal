using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class MovimentosDeAprovação
    {
        public MovimentosDeAprovação()
        {
            UtilizadoresMovimentosDeAprovação = new HashSet<UtilizadoresMovimentosDeAprovação>();
        }

        public int NºMovimento { get; set; }
        public int? Tipo { get; set; }
        public int? Área { get; set; }
        public string Número { get; set; }
        public string UtilizadorSolicitou { get; set; }
        public decimal? Valor { get; set; }
        public DateTime? DataHoraAprovação { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }
        public int Estado { get; set; }
        public string MotivoDeRecusa { get; set; }
        public int Nivel { get; set; }

        public ICollection<UtilizadoresMovimentosDeAprovação> UtilizadoresMovimentosDeAprovação { get; set; }
    }
}
