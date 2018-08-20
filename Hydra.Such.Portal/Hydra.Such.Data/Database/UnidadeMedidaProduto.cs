using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class UnidadeMedidaProduto
    {
        public string NºProduto { get; set; }
        public string Código { get; set; }
        public decimal? QtdPorUnidadeMedida { get; set; }
        public decimal? Comprimento { get; set; }
        public decimal? Largura { get; set; }
        public decimal? Altura { get; set; }
        public decimal? Cubagem { get; set; }
        public decimal? Peso { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }
    }
}
