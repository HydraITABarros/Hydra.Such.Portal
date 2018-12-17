using System;
using System.Collections.Generic;
using System.Text;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Data.ViewModel.Compras
{
    public class BillingReceptionModel : ErrorHandler
    {
        private BillingReceptionStates estado;
        private BillingDocumentTypes tipoDocumento;

        public string Id { get; set; }
        public BillingDocumentTypes TipoDocumento
        {
            get
            {
                return tipoDocumento;
            }
            set
            {
                tipoDocumento = value;
                TipoDocumentoDescricao = EnumHelper.GetDescriptionFor(typeof(BillingDocumentTypes), (int)tipoDocumento);
            }
        }
        public string TipoDocumentoDescricao
        {
            get;
            private set;
        }
        public BillingReceptionStates Estado
        {
            get
            {
                return estado;
            }
            set
            {
                estado = value;
                EstadoDescricao = EnumHelper.GetDescriptionFor(typeof(BillingReceptionStates), (int)estado);
            }
        }
        public string EstadoDescricao
        {
            get;
            private set;
        }
        public string DataRececao { get; set; }
        public string CodFornecedor { get; set; }
        public string NomeFornecedor { get; set; }
        public string NumDocFornecedor { get; set; }
        public string DataDocFornecedor { get; set; }
        public string NumEncomenda { get; set; }
        public string NumEncomendaManual { get; set; }
        public decimal? ValorEncomendaOriginal { get; set; }
        public decimal? QuantidadeEncomenda { get; set; }
        public decimal? QuantidadeRecebida { get; set; }
        public decimal? ValorRecebidoNaoContabilizado { get; set; }
        public decimal? Valor { get; set; }
        public decimal? ValorEncomendaActual { get; set; }
        public string CodRegiao { get; set; }
        public string CodAreaFuncional { get; set; }
        public string CodCentroResponsabilidade { get; set; }
        public string CodLocalizacao { get; set; }
        public string Local { get; set; }
        public string NumAcordoFornecedor { get; set; }
        public string Destinatario { get; set; }
        public BillingReceptionAreas? IdAreaPendente { get; set; }
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
        public string DescricaoStatus { get; set; }
        public string DataPassaPendente { get; set; }
        public string AreaPendente { get; set; }
        public string AreaPendente2 { get; set; }
        public string DataResolucao { get; set; }
        public string AreaUltimaInteracao { get; set; }
        public string UserUltimaInteracao { get; set; }
        public string link { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }

        public List<BillingRecWorkflowModel> WorkflowItems { get; set; }
    }
}
