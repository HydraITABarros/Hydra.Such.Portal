using System;

namespace Hydra.Such.Data.ViewModel.Viaturas
{
    public class Viaturas2ContraOrdenacoesViewModel : ErrorHandler
    {
        public int ID { get; set; }
        public string Matricula { get; set; }
        public string Local { get; set; }
        public DateTime? Data { get; set; }
        public string DataTexto { get; set; }
        public int? IDCondutor { get; set; }
        public string Condutor { get; set; }
        public int? IDResponsabilidade { get; set; }
        public string Responsabilidade { get; set; }
        public Decimal? Valor { get; set; }
        public string Observacoes { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string DataCriacaoTexto { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        public string DataModificacaoTexto { get; set; }
    }
}
