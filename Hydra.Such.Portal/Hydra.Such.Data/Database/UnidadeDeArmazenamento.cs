using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class UnidadeDeArmazenamento
    {
        public string NºProduto { get; set; }
        public string CódLocalização { get; set; }
        public string Descrição { get; set; }
        public bool? ListaMateriais { get; set; }
        public string NºPrateleira { get; set; }
        public decimal? CustoUnitário { get; set; }
        public decimal? CustoPadrão { get; set; }
        public decimal? UltimoCustoDirecto { get; set; }
        public string NºFornecedor { get; set; }
        public string CódProdForn { get; set; }
        public decimal? Inventário { get; set; }
        public decimal? TamanhoLote { get; set; }
        public decimal? PreçoDeVenda { get; set; }
        public bool? Bloqueado { get; set; }
        public decimal? ValorEmArmazem { get; set; }
        public bool? ArmazémPrincipal { get; set; }
        public bool? UsarMrp2 { get; set; }
        public string CódCategoriaProduto { get; set; }
        public string CódGrupoProduto { get; set; }
        public string CódUnidadeMedidaProduto { get; set; }
        public decimal? ConsumoMédio { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }
    }
}
