using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
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
    }
}
