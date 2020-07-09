using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.Database
{
    public partial class PedidoParticipacaoFormacao
    {
        public PedidoParticipacaoFormacao()
        {
            RegistosAlteracoes = new HashSet<RegistoAlteracoesPedidoFormacao>();
        }

        public string IdPedido { get; set; }
        public int? Estado { get; set; }
        public string IdEmpregado { get; set; }
        public string NomeEmpregado { get; set; }
        public string FuncaoEmpregado { get; set; }
        public string ProjectoEmpregado { get; set; }
        public string IdAreaFuncional { get; set; } 
        public string AreaFuncionalEmpregado { get; set; }
        public string IdCentroResponsabilidade { get; set; }
        public string CentroResponsabilidadeEmpregado { get; set; }
        public string IdEstabelecimento { get; set; }
        public string DescricaoEstabelecimento { get; set; }
        public string IdAccaoFormacao { get; set; }
        public string DesignacaoAccao { get; set; }
        public string LocalRealizacao { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public decimal? NumeroTotalHoras { get; set; }
        public string IdEntidadeFormadora { get; set; }
        public string DescricaoEntidadeFormadora { get; set; }
        public decimal? CustoInscricao { get; set; }
        public decimal? ValorIva { get; set; }
        public decimal? CustoDeslocacoes { get; set; }
        public decimal? CustoEstadia { get; set; }
        public string DescricaoConhecimentos { get; set; }
        public string FundamentacaoChefia { get; set; }
        public int? Planeada { get; set; }
        public int? TemDotacaoOrcamental { get; set; }
        public string ParecerAcademia { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorUltimaModificacao { get; set; }
        public DateTime? DataHoraUltimaModificacao { get; set; }

        public ICollection<RegistoAlteracoesPedidoFormacao> RegistosAlteracoes { get; set; }
    }
}
