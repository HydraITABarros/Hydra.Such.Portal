using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;

namespace Hydra.Such.Data.ViewModel.PedidoCotacao
{
    public class CondicoesPropostasFornecedoresView : ErrorHandler
    {
        public int IdCondicaoPropostaFornecedores { get; set; }
        public string NumConsultaMercado { get; set; }
        public string CodFornecedor { get; set; }
        public string Alternativa { get; set; }
        public string CodActividade { get; set; }
        public string ValidadeProposta { get; set; }
        public string CodTermosPagamento { get; set; }
        public string CodFormaPagamento { get; set; }
        public bool? FornecedorSelecionado { get; set; }
        public DateTime? DataProposta { get; set; }
        public string NumProjecto { get; set; }
        public bool? Preferencial { get; set; }
        public string NomeFornecedor { get; set; }
        public bool? EnviarPedidoProposta { get; set; }
        public bool? Notificado { get; set; }
    }
}
