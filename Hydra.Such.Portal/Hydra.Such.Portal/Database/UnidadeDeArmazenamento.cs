using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
{
    public partial class UnidadeDeArmazenamento
    {
        public string NºProduto { get; set; }
        public string CódLocalização { get; set; }
        public string Descrição { get; set; }
        public decimal? Inventário { get; set; }
        public bool? Bloqueado { get; set; }
        public string CódUnidadeMedidaProduto { get; set; }
        public decimal? CustoUnitário { get; set; }
        public decimal? ValorEmArmazem { get; set; }
        public string NºPrateleira { get; set; }
        public string NºFornecedor { get; set; }
        public string CódProdForn { get; set; }
        public string CódGrupoProduto { get; set; }
        public string CódCategoriaProduto { get; set; }
        public bool? ArmazémPrincipal { get; set; }
        public decimal? PreçoDeVenda { get; set; }
        public decimal? UltimoCustoDirecto { get; set; }
    }
}
