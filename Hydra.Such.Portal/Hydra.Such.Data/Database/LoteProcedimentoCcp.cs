using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.Database
{
    public partial class LoteProcedimentoCcp
    {
        public string NoProcedimento { get; set; }
        public int IdLote { get; set; }
        public int? EstadoLote { get; set; }
        public decimal? ValorEstimado { get; set; }
        public decimal? ValorAdjudicado { get; set; }
        public string UtilizadorAdjudicacao { get; set; }
        public DateTime? DataAdjudicacaco { get; set; }
        public TimeSpan? HoraAdjudicacao { get; set; }
        public string ComentarioNotificacao { get; set; }
        public int? DiasPrazoNotificacao { get; set; }
        public string UtilizadorNotificacao { get; set; }
        public DateTime? DataNotificacao { get; set; }
        public TimeSpan? HoraNotificacao { get; set; }

        public ProcedimentosCcp ProcedimentoNavigation { get; set; }
        public ICollection<FluxoTrabalhoListaControlo> Fluxos { get; set; }

    }
}
