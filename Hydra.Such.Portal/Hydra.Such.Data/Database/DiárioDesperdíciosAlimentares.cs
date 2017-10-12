using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class DiárioDesperdíciosAlimentares
    {
        public int NºLinha { get; set; }
        public int? Tipo { get; set; }
        public int? NºUnidadeProdutiva { get; set; }
        public string Código { get; set; }
        public string Descrição { get; set; }
        public decimal? Quantidade { get; set; }
        public string CódUnidadeMedida { get; set; }
        public decimal? ValorCusto { get; set; }
        public decimal? ValorVenda { get; set; }
        public int? TipoRefeição { get; set; }
        public string CódLocalização { get; set; }
        public string Utilizador { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorCriação { get; set; }
        public string UtilizadorModificação { get; set; }

        public UnidadesProdutivas NºUnidadeProdutivaNavigation { get; set; }
        public TiposRefeição TipoRefeiçãoNavigation { get; set; }
    }
}
