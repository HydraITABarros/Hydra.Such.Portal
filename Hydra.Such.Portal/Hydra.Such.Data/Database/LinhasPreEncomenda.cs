using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class LinhasPreEncomenda
    {
        public string NºPreEncomenda { get; set; }
        public int NºLinhaPreEncomenda { get; set; }
        public string CódigoProduto { get; set; }
        public string DescriçãoProduto { get; set; }
        public string CódigoLocalização { get; set; }
        public string CódigoUnidadeMedida { get; set; }
        public decimal? QuantidadeDisponibilizada { get; set; }
        public decimal? CustoUnitário { get; set; }
        public string NºFornecedor { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public string NºRequisição { get; set; }
        public int? NºLinhaRequisição { get; set; }
        public string NºProjeto { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }
        public int? DocumentoaCriar { get; set; }
        public bool CriarDocumento { get; set; }
        public string NºEncomendaAberto { get; set; }
        public int? NºLinhaEncomendaAberto { get; set; }
        public bool Tratada { get; set; }
        public int? DocumentoACriar { get; set; }
    }
}
