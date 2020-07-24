using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;

namespace Hydra.Such.Data.ViewModel.PedidoCotacao
{
    public class SeleccaoEntidadesView : ErrorHandler
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
        public DateTime? PedidoCotacaoCriadoEm { get; set; }
        public string NoRequisicao { get; set; }
        public string CodRegiao { get; set; }
        public string CodArea { get; set; }
        public string CodCresp { get; set; }
        public decimal CustoTotalPrevisto { get; set; }
        public bool? Historico { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }

        //Campos tratados
        public string DataEnvioAoFornecedor_Show { get; set; }
        public string DataRecepcaoProposta_Show { get; set; }
        public string DataRespostaEsperada_Show { get; set; }
        public string DataPedidoEsclarecimento_Show { get; set; }
        public string DataRespostaEsclarecimento_Show { get; set; }
        public string DataRespostaDoFornecedor_Show { get; set; }
        public string DataEnvioPropostaArea_Show { get; set; }
        public string DataRespostaArea_Show { get; set; }
        public string DataPedidoCotacaoCriadoEm_Show { get; set; }
        public string Historico_Show { get; set; }
        public string NotaEncomenda_Show { get; set; }

        public string Selecionado_Show { get; set; }
        public string Preferencial_Show { get; set; }
        public string NaoRespostaDoFornecedor_Show { get; set; }
    }
}
