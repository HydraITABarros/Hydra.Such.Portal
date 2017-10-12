using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class DiárioRequisiçãoUnidProdutiva
    {
        public int NºLinha { get; set; }
        public int? NºUnidadeProdutiva { get; set; }
        public string NºProduto { get; set; }
        public string Descrição { get; set; }
        public string CódUnidadeMedida { get; set; }
        public decimal? Quantidade { get; set; }
        public decimal? CustoUnitárioDireto { get; set; }
        public decimal? Valor { get; set; }
        public string NºProjeto { get; set; }
        public string NºFornecedor { get; set; }
        public int? TipoRefeição { get; set; }
        public bool? TabelaPreçosFornecedor { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorCriação { get; set; }
        public string UtilizadorModificação { get; set; }

        public UnidadesProdutivas NºUnidadeProdutivaNavigation { get; set; }
        public TiposRefeição TipoRefeiçãoNavigation { get; set; }
    }
}
