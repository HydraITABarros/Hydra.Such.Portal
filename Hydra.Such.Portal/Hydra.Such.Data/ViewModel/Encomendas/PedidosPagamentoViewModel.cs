using System;
using System.Collections.Generic;
using System.Text;


namespace Hydra.Such.Data.ViewModel.Encomendas
{
    public class PedidosPagamentoViewModel : ErrorHandler
    {
        public int NoPedido { get; set; }
        public DateTime? Data { get; set; }
        public string DataText { get; set; }
        public int? Tipo { get; set; }
        public string TipoText { get; set; }
        public int? Estado { get; set; }
        public string EstadoText { get; set; }
        public bool? Aprovado { get; set; }
        public string AprovadoText { get; set; }
        public decimal? Valor { get; set; }
        public string NoEncomenda { get; set; }
        public string NoRequisicao { get; set; }
        public string CodigoFornecedor { get; set; }
        public string Fornecedor { get; set; }
        public string NIB { get; set; }
        public string IBAN { get; set; }
        public DateTime? DataPedido { get; set; }
        public string DataPedidoText { get; set; }
        public string UserPedido { get; set; }
        public string UserAprovacao { get; set; }
        public DateTime? DataAprovacao { get; set; }
        public string DataAprovacaoText { get; set; }
        public string UserFinanceiros { get; set; }
        public DateTime? DataDisponibilizacao { get; set; }
        public string DataDisponibilizacaoText { get; set; }
        public string Descricao { get; set; }
        public DateTime? DataEnvioAprovacao { get; set; }
        public string DataEnvioAprovacaoText { get; set; }
        public DateTime? DataValidacao { get; set; }
        public string DataValidacaoText { get; set; }
        public string UserValidacao { get; set; }
        public string UserEditorPrioritario { get; set; }
        public bool? BloqueadoFaltaPagamento { get; set; }
        public string BloqueadoFaltaPagamentoText { get; set; }
        public string Aprovadores { get; set; }
        public decimal? ValorEncomenda { get; set; }
        public string UserLiquidado { get; set; }
        public DateTime? DataLiquidado { get; set; }
        public string DataLiquidadoText { get; set; }
        public bool? Arquivado { get; set; }
        public string ArquivadoText { get; set; }
        public string UserArquivo { get; set; }
        public DateTime? DataArquivo { get; set; }
        public string DataArquivoText { get; set; }
        public string MotivoAnulacao { get; set; }
        public bool? Resolvido { get; set; }
        public string ResolvidoText { get; set; }
        public bool? Prioritario { get; set; }
        public string PrioritarioText { get; set; }
        public DateTime? DataPrioridade { get; set; }
        public string DataPrioridadeText { get; set; }
        public string NumeroTransferencia { get; set; }
        public string RegiaoMercadoLocal { get; set; }
        public string RegiaoMercadoLocalText { get; set; }
        public string CodigoRegiao { get; set; }
        public string CodigoRegiaoText { get; set; }
        public string CodigoAreaFuncional { get; set; }
        public string CodigoAreaFuncionalText { get; set; }
        public string CodigoCentroResponsabilidade { get; set; }
        public string CodigoCentroResponsabilidadeText { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        public decimal? ValorJaPedido { get; set; }

        public bool? EditarPrioritario { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }
    }
}
