using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class Configuração
    {
        public int Id { get; set; }
        public int? NumeraçãoProjetos { get; set; }
        public int? NumeraçãoContratos { get; set; }
        public int? NumeraçãoFolhasDeHoras { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorCriação { get; set; }
        public string UtilizadorModificação { get; set; }
        public int? NumeraçãoProcedimentoAquisição { get; set; }
        public int? NumeraçãoProcedimentoSimplificado { get; set; }
        public int? NumeraçãoOportunidades { get; set; }
        public int? NumeraçãoPropostas { get; set; }
        public int? NumeraçãoContactos { get; set; }
        public int? NumeracaoContratosInternos { get; set; }
        public int? NumeracaoContratosQuotas { get; set; }
        public TimeSpan? InicioHoraAlmoco { get; set; }
        public TimeSpan? FimHoraAlmoco { get; set; }
        public TimeSpan? InicioHoraJantar { get; set; }
        public TimeSpan? FimHoraJantar { get; set; }
        public int? NumeraçãoPréRequisições { get; set; }
        public int? NumeraçãoRequisições { get; set; }
        public int? NumeracaoRequisicoesComprasDinheiro { get; set; }
        public int? NumeraçãoFichasTécnicasDePratos { get; set; }
        public int? NumeraçãoModReqSimplificadas { get; set; }
        public int? NumeraçãoRequisiçõesSimplificada { get; set; }
        public int? NumeracaoModelosRequisicao { get; set; }
        public int? NumeracaoProdutos { get; set; }
        public int? ConsultaMercado { get; set; }
        public string CodAreaResiduos { get; set; }
        public string ReportUsername { get; set; }
        public string ReportPassword { get; set; }
        public string ArmazemCompraDireta { get; set; }

        public ConfiguraçãoNumerações NumeraçãoContratosNavigation { get; set; }
        public ConfiguraçãoNumerações NumeraçãoFolhasDeHorasNavigation { get; set; }
        public ConfiguraçãoNumerações NumeraçãoProcedimentoAquisiçãoNavigation { get; set; }
        public ConfiguraçãoNumerações NumeraçãoProcedimentoSimplificadoNavigation { get; set; }
        public ConfiguraçãoNumerações NumeraçãoProjetosNavigation { get; set; }
    }
}
