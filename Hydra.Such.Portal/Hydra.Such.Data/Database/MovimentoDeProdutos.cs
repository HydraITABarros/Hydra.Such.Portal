using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class MovimentoDeProdutos
    {
        public int NºMovimentos { get; set; }
        public DateTime? DataRegisto { get; set; }
        public int? TipoMovimento { get; set; }
        public byte[] NºDocumento { get; set; }
        public string NºProduto { get; set; }
        public string Descrição { get; set; }
        public string CódLocalização { get; set; }
        public decimal? Quantidade { get; set; }
        public decimal? CustoUnitário { get; set; }
        public decimal? Valor { get; set; }
        public int? NºProjecto { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁrea { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
    }
}
