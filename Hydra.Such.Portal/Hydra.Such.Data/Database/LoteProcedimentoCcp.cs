using System;
using System.Collections.Generic;
using System.Text;

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
        public string UtilizadorAdjudicacao { get; set; }
        public DateTime? DataAdjudicacao { get; set; }
        public TimeSpan? HoraAdjudicacao { get; set; }
        public string ComentarioNotificacao { get; set; }
        public int? DiasPrazoNotificacao { get; set; }
        public string UtilizadorNotificacao { get; set; }
        public DateTime? DataNotificacao { get; set; }
        public TimeSpan? HoraNotificacao { get; set; }

        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }

        public ProcedimentosCcp NoProcedimentoNavigation { get; set; }
        public ICollection<FluxoTrabalhoListaControlo> FluxoTrabalhoListaControlo { get; set; }

    }
}
