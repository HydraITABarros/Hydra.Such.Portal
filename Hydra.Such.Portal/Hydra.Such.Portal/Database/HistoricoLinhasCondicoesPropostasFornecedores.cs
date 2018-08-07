using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
{
    public partial class HistoricoLinhasCondicoesPropostasFornecedores
    {
        public int NumLinha { get; set; }
        public string NumConsultaMercado { get; set; }
        public int NumVersao { get; set; }
        public string CodFornecedor { get; set; }
        public string Alternativa { get; set; }
        public string CodProduto { get; set; }
        public decimal? Quantidade { get; set; }
        public string NumProjecto { get; set; }
        public string CodLocalizacao { get; set; }
        public decimal? PrecoFornecedor { get; set; }
        public DateTime? DataEntregaPrevista { get; set; }
        public string Validade { get; set; }
        public decimal? QuantidadeAAdjudicar { get; set; }
        public string MotivoRejeicao { get; set; }
        public decimal? QuantidadeAdjudicada { get; set; }
        public decimal? QuantidadeAEncomendar { get; set; }
        public decimal? QuantidadeEncomendada { get; set; }
        public string CodUnidadeMedida { get; set; }
        public string PrazoEntrega { get; set; }
        public string CodActividade { get; set; }
        public decimal? PercentagemDescontoLinha { get; set; }
        public decimal? ValorAdjudicadoDl { get; set; }
        public int EstadoRespostaFornecedor { get; set; }
        public DateTime? DataEntregaPrometida { get; set; }
        public bool? RespostaFornecedor { get; set; }
        public decimal? QuantidadeRespondida { get; set; }

        public Actividades CodActividadeNavigation { get; set; }
        public Projetos NumProjectoNavigation { get; set; }
    }
}
