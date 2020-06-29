using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class LinhasAcordoPrecos
    {
        public string NoProcedimento { get; set; }
        public string NoFornecedor { get; set; }
        public string NomeFornecedor { get; set; }
        public string NoSubFornecedor { get; set; }
        public string NomeSubFornecedor { get; set; }
        public string CodProduto { get; set; }
        public DateTime DtValidadeInicio { get; set; }
        public DateTime? DtValidadeFim { get; set; }
        public string Cresp { get; set; }
        public string Area { get; set; }
        public string Regiao { get; set; }
        public string Localizacao { get; set; }
        public decimal? CustoUnitario { get; set; }
        public string DescricaoProduto { get; set; }
        public string Um { get; set; }
        public decimal? QtdPorUm { get; set; }
        public decimal? PesoUnitario { get; set; }
        public string CodProdutoFornecedor { get; set; }
        public string DescricaoProdFornecedor { get; set; }
        public int? FormaEntrega { get; set; }
        public string UserId { get; set; }
        public DateTime? DataCriacao { get; set; }
        public int? TipoPreco { get; set; }
        public string GrupoRegistoIvaProduto { get; set; }
        public string CodCategoriaProduto { get; set; }
    }
}
