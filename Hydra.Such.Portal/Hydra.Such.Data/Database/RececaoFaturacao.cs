using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class RececaoFaturacao
    {
        public RececaoFaturacao()
        {
            RececaoFaturacaoWorkflow = new HashSet<RececaoFaturacaoWorkflow>();
        }

        public string Id { get; set; }
        public int? TipoDocumento { get; set; }
        public int? Estado { get; set; }
        public DateTime? DataRececao { get; set; }
        public string CodFornecedor { get; set; }
        public string NumDocFornecedor { get; set; }
        public DateTime? DataDocFornecedor { get; set; }
        public string NumEncomenda { get; set; }
        public string NumEncomendaManual { get; set; }
        public decimal? ValorEncomendaOriginal { get; set; }
        public decimal? QuantidadeEncomenda { get; set; }
        public decimal? QuantidadeRecebida { get; set; }
        public decimal? ValorRecebidoNaoContabilizado { get; set; }
        public decimal? Valor { get; set; }
        public string CodRegiao { get; set; }
        public string CodAreaFuncional { get; set; }
        public string CodCentroResponsabilidade { get; set; }
        public string CodLocalizacao { get; set; }
        public string Local { get; set; }
        public string NumAcordoFornecedor { get; set; }
        public DateTime? DocumentoCriadoEm { get; set; }
        public string DocumentoCriadoPor { get; set; }
        public string Destinatario { get; set; }
        public int? AreaPendente { get; set; }
        public DateTime? DataUltimaInteracao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string CriadoPor { get; set; }
        public DateTime? DataModificacao { get; set; }
        public string ModificadoPor { get; set; }

        public ICollection<RececaoFaturacaoWorkflow> RececaoFaturacaoWorkflow { get; set; }
    }
}
