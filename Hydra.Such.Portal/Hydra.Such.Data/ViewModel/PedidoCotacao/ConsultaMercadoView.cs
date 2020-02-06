using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;

namespace Hydra.Such.Data.ViewModel.PedidoCotacao
{
    public class ConsultaMercadoView : ErrorHandler
    {
        public string NumConsultaMercado { get; set; }
        public string CodProjecto { get; set; }
        public string Descricao { get; set; }
        public string CodRegiao { get; set; }
        public string CodAreaFuncional { get; set; }
        public string CodCentroResponsabilidade { get; set; }
        public string CodActividade { get; set; }
        public DateTime? DataPedidoCotacao { get; set; }
        public string FornecedorSelecionado { get; set; }
        public string NumDocumentoCompra { get; set; }
        public string CodLocalizacao { get; set; }
        public string FiltroActividade { get; set; }
        public decimal? ValorPedidoCotacao { get; set; }
        public int? Destino { get; set; }
        public int? Estado { get; set; }
        public string UtilizadorRequisicao { get; set; }
        public DateTime? DataLimite { get; set; }
        public bool? EspecificacaoTecnica { get; set; }
        public int? Fase { get; set; }
        public int? Modalidade { get; set; }
        public DateTime? PedidoCotacaoCriadoEm { get; set; }
        public string PedidoCotacaoCriadoPor { get; set; }
        public DateTime? ConsultaEm { get; set; }
        public string ConsultaPor { get; set; }
        public DateTime? NegociacaoContratacaoEm { get; set; }
        public string NegociacaoContratacaoPor { get; set; }
        public DateTime? AdjudicacaoEm { get; set; }
        public string AdjudicacaoPor { get; set; }
        public string NumRequisicao { get; set; }
        public string PedidoCotacaoOrigem { get; set; }
        public decimal? ValorAdjudicado { get; set; }
        public string CodFormaPagamento { get; set; }
        public bool? SeleccaoEfectuada { get; set; }
        public string NumEncomenda { get; set; }
        public bool? EmailEnviado { get; set; }
        public string RegiaoMercadoLocal { get; set; }
        public DateTime? DataEntregaFornecedor { get; set; }
        public DateTime? DataRecolha { get; set; }
        public DateTime? DataEntregaArmazem { get; set; }
        public string CodComprador { get; set; }
        public int? LocalEntrega { get; set; }
        public bool? Equipamento { get; set; }
        public bool? Amostra { get; set; }
        public bool? Urgente { get; set; }
        public bool? Historico { get; set; }
        public string Obs { get; set; }
        public string UserHistoricoToAtivo { get; set; }
        public string UserToHistorico { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public DateTime? DataHoraAlteracao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }

        public ICollection<CondicoesPropostasFornecedoresView> CondicoesPropostasFornecedores { get; set; }
        public ICollection<LinhasCondicoesPropostasFornecedoresView> LinhasCondicoesPropostasFornecedores { get; set; }
        public ICollection<LinhasConsultaMercadoView> LinhasConsultaMercado { get; set; }
        public ICollection<SeleccaoEntidadesView> SeleccaoEntidades { get; set; }
        public ICollection<RegistoDePropostasView> RegistoDePropostas { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }

        //Campos tratados
        public string Destino_Show { get; set; }
        public string Estado_Show { get; set; }
        public string Fase_Show { get; set; }
        public string Modalidade_Show { get; set; }
        public string DataPedidoCotacao_Show { get; set; }
        public string DataLimite_Show { get; set; }
        public string PedidoCotacaoCriadoEm_Show { get; set; }
        public string ConsultaEm_Show { get; set; }
        public string NegociacaoContratacaoEm_Show { get; set; }
        public string AdjudicacaoEm_Show { get; set; }
        public string DataEntregaFornecedor_Show { get; set; }
        public string DataRecolha_Show { get; set; }
        public string DataEntregaArmazem_Show { get; set; }
        public string LocalEntrega_Show { get; set; }

        //Campos Calculados
        public string NumVersoesArquivadas_CalcField { get; set; }
    }
}
