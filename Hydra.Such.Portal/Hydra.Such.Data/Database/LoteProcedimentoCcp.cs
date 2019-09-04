using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class LoteProcedimentoCcp
    {
        public LoteProcedimentoCcp()
        {
            FluxoTrabalhoListaControlo = new HashSet<FluxoTrabalhoListaControlo>();
        }
        public string NoProcedimento { get; set; }
        public int IdLote { get; set; }
        public int? EstadoLote { get; set; }
        public string DescricaoObjectoLote { get; set; }
        public decimal? ValorEstimado { get; set; }

        public decimal? ValorAdjudicacao { get; set; }
        public string ComentarioAdjudicacao { get; set; }
        public string UtilizadorAdjudicacao { get; set; }
        public DateTime? DataAdjudicacao { get; set; }
        public DateTime? HoraAdjudicacao { get; set; }

        public string ComentarioAutorizacaoAdjudicacao { get; set; }
        public string UtilizadorAutorizacaoAdjudicacao { get; set; }
        public DateTime? DataAutorizacaoAdjudicacao { get; set; }
        public DateTime? HoraAutorizacaoAdjudicacao { get; set; }

        public string ComentarioNaoAdjudicacao { get; set; }
        public string UtilizadorNaoAdjudicacao { get; set; }
        public DateTime? DataNaoAdjudicacao { get; set; }
        public DateTime? HoraNaoAdjudicacao { get; set; }
        public string ComentarioNotificacao { get; set; }
        public int? DiasPrazoNotificacao { get; set; }
        public string UtilizadorNotificacao { get; set; }
        public DateTime? DataNotificacao { get; set; }
        public DateTime? HoraNotificacao { get; set; }

        public bool? VistoAberturaPeloAprovisionamento { get; set; }
        public bool? VistoAdjudicacaoPeloAprovisionamento { get; set; }

        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }

        public ProcedimentosCcp NoProcedimentoNavigation { get; set; }
        public ICollection<FluxoTrabalhoListaControlo> FluxoTrabalhoListaControlo { get; set; }

    }
}
