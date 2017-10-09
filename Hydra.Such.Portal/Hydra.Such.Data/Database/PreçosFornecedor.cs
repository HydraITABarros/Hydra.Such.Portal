using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class PreçosFornecedor
    {
        public string NºFornecedor { get; set; }
        public string NºProduto { get; set; }
        public DateTime DataValidadeInício { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public string CódLocalização { get; set; }
        public DateTime? DataValidadeFim { get; set; }
        public decimal? CustoUnitário { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódUnidadeMedida { get; set; }
        public decimal? PesoUnitário { get; set; }
        public string CódigoProdutoFornecedor { get; set; }
        public string DescriçãoProdutoFornecedor { get; set; }
        public int? FormaEntrega { get; set; }
        public string Utilizador { get; set; }
        public DateTime? DataHoraSistema { get; set; }
        public string UnidadeMedidaFornecedor { get; set; }
        public bool? Seleção { get; set; }
        public string SubFornecedor { get; set; }
        public bool? SegundaFeira { get; set; }
        public bool? TerçaFeira { get; set; }
        public bool? QuartaFeira { get; set; }
        public bool? QuintaFeira { get; set; }
        public bool? SextaFeira { get; set; }
        public string Marca { get; set; }
        public int? TipoPreço { get; set; }
    }
}
