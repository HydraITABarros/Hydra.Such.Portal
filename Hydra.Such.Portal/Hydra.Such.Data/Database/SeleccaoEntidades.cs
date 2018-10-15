using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class SeleccaoEntidades
    {
        public int IdSeleccaoEntidades { get; set; }
        public string NumConsultaMercado { get; set; }
        public string CodFornecedor { get; set; }
        public string NomeFornecedor { get; set; }
        public string CodActividade { get; set; }
        public string CidadeFornecedor { get; set; }
        public string CodTermosPagamento { get; set; }
        public string CodFormaPagamento { get; set; }
        public bool? Selecionado { get; set; }
        public bool? Preferencial { get; set; }
        public string EmailFornecedor { get; set; }
        public DateTime? DataEnvioAoFornecedor { get; set; }
        public DateTime? DataRecepcaoProposta { get; set; }
        public string UtilizadorEnvio { get; set; }
        public string UtilizadorRecepcaoProposta { get; set; }
        public int Fase { get; set; }
        public int? PrazoResposta { get; set; }
        public DateTime? DataRespostaEsperada { get; set; }
        public DateTime? DataPedidoEsclarecimento { get; set; }
        public DateTime? DataRespostaEsclarecimento { get; set; }
        public DateTime? DataRespostaDoFornecedor { get; set; }
        public bool? NaoRespostaDoFornecedor { get; set; }
        public DateTime? DataEnvioPropostaArea { get; set; }
        public DateTime? DataRespostaArea { get; set; }

        public ConsultaMercado NumConsultaMercadoNavigation { get; set; }
    }
}
