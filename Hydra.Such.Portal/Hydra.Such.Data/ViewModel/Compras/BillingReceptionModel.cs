using System;
using System.Collections.Generic;
using System.Text;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Data.ViewModel.Compras
{
    public class BillingReceptionModel : ErrorHandler
    {
        public string Id { get; set; }
        public BillingDocumentTypes TipoDocumento { get; set; }
        public BillingReceptionStates Estado { get; set; }
        public string DataRececao { get; set; }
        public string CodFornecedor { get; set; }
        public string NumDocFornecedor { get; set; }
        public string DataDocFornecedor { get; set; }
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
        public string Destinatario { get; set; }
        public BillingReceptionAreas IdAreaPendente { get; set; }
        public string DataUltimaInteracao { get; set; }
        public DateTime? DocumentoCriadoEm { get; set; }
        public string DocumentoCriadoPor { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string CriadoPor { get; set; }
        public DateTime? DataModificacao { get; set; }
        public string ModificadoPor { get; set; }
        public string TipoProblema { get; set; }
        public string Descricao { get; set; }
        public string DescricaoProblema { get; set; }
        public DateTime? DataPassaPendente { get; set; }
        public string AreaPendente { get; set; }
        public string AreaPendente2 { get; set; }
        public DateTime? DataResolucao { get; set; }

        public List<BillingRecWorkflowModel> WorkflowItems { get; set; }
    }
}
