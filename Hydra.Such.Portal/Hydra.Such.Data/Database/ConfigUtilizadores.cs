using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class ConfigUtilizadores
    {
        public ConfigUtilizadores()
        {
            AcessosDimensões = new HashSet<AcessosDimensões>();
            PerfisUtilizador = new HashSet<PerfisUtilizador>();
        }

        public string IdUtilizador { get; set; }
        public string Nome { get; set; }
        public string EmployeeNo { get; set; }
        public bool? Ativo { get; set; }
        public bool Administrador { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorCriação { get; set; }
        public string UtilizadorModificação { get; set; }
        public string ProcedimentosEmailEnvioParaCa { get; set; }
        public string ProcedimentosEmailEnvioParaArea { get; set; }
        public string ProcedimentosEmailEnvioParaArea2 { get; set; }
        public string RegiãoPorDefeito { get; set; }
        public string AreaPorDefeito { get; set; }
        public string CentroRespPorDefeito { get; set; }
        public int? PerfilNumeraçãoRecDocCompras { get; set; }
        public int? Rfperfil { get; set; }
        public int? RfperfilVisualizacao { get; set; }
        public string RffiltroArea { get; set; }
        public string RfnomeAbreviado { get; set; }
        public bool? RfrespostaContabilidade { get; set; }
        public bool? RfalterarDestinatarios { get; set; }
        public string RfmailEnvio { get; set; }
        public string NumSerieNotasCredito { get; set; }
        public string NumSeriePreFaturasCompra { get; set; }
        public string NumSerieFaturas { get; set; }
        public string NumSerieNotasDebito { get; set; }
        public string NumSerieNotasCreditoCompra { get; set; }
        public string NumSeriePreFaturasCompraCf { get; set; }
        public string NumSeriePreFaturasCompraCp { get; set; }
        public string CentroDeResponsabilidade { get; set; }
        public string SuperiorHierarquico { get; set; }
        public bool? RequisicaoStock { get; set; }
        public string AprovadorPedidoPag1 { get; set; }
        public string AprovadorPedidoPag2 { get; set; }
        public bool? AnulacaoPedidoPagamento { get; set; }
        public bool? ValidarPedidoPagamento { get; set; }
        public bool? CriarProjetoSemAprovacao { get; set; }
        public bool? CMHistoricoToActivo { get; set; }
        public bool? ArquivarREQPendentes { get; set; }
        public bool? RegistoDataDiarioCafetaria { get; set; }

        #region SGPPF
        public int? TipoUtilizadorFormacao { get; set; }
        #endregion

        public ICollection<AcessosDimensões> AcessosDimensões { get; set; }
        public ICollection<PerfisUtilizador> PerfisUtilizador { get; set; }
    }
}
