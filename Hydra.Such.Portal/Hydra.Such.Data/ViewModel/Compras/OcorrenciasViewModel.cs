using System;

namespace Hydra.Such.Data.ViewModel.Compras
{
    public class OcorrenciasViewModel : ErrorHandler
    {
        public int CodOcorrencia { get; set; }
        public int? CodEstado { get; set; }
        public string NomeEstado { get; set; }
        public string CodFornecedor { get; set; }
        public string NomeFornecedor { get; set; }
        public string CodEncomenda { get; set; }
        public string CodRegiao { get; set; }
        public string CodAreaFuncional { get; set; }
        public string CodCentroResponsabilidade { get; set; }
        public DateTime? DataOcorrencia { get; set; }
        public string DataOcorrenciaTexto { get; set; }
        public string LocalEntrega { get; set; }
        public string NoDocExterno { get; set; }
        public string CodArtigo { get; set; }
        public string Descricao { get; set; }
        public string UnidMedida { get; set; }
        public decimal? Quantidade { get; set; }
        public string Motivo { get; set; }
        public int? GrauGravidade { get; set; }
        public string GrauGravidadeTexto { get; set; }
        public string Observacao { get; set; }
        public string MedidaCorretiva { get; set; }
        public string UtilizadorMedidaCorretiva { get; set; }
        public DateTime? DataMedidaCorretiva { get; set; }
        public string DataMedidaCorretivaTexto { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string DataCriacaoTexto { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        public string DataModificacaoTexto { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }
    }
}
