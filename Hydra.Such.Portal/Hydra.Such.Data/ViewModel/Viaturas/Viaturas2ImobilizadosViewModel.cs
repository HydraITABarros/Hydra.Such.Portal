using System;

namespace Hydra.Such.Data.ViewModel.Viaturas
{
    public class Viaturas2ImobilizadosViewModel : ErrorHandler
    {
        public int ID { get; set; }
        public string Matricula { get; set; }

        public string NoImobilizado { get; set; }
        public string Descricao { get; set; }
        public string DataCompraTexto { get; set; }
        public string DocumentoCompra { get; set; }
        public string ValorCompra { get; set; }
        public string DataIncioAmortizacaoTexto { get; set; }
        public string DataFinalAmortizacaoTexto { get; set; }
        public string ValorAmortizado { get; set; }
        public string VendaAbate { get; set; }
        public string DataVendaAbateTexto { get; set; }
        public string DocumentoVendaAbate { get; set; }
        public string ValorVendaAbate { get; set; }
        public string EstadoImobilizado { get; set; }
        public string Bloqueado { get; set; }
        public string Pai { get; set; }
        public string PaiFilho { get; set; }

        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string DataCriacaoTexto { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        public string DataModificacaoTexto { get; set; }
    }
}
