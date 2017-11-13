using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;

namespace Hydra.Such.Data.ViewModel.CCP
{
    public class WorkflowProcedimentosCCPView
    {
        public string NoProcedimento { get; set; }
        public int Estado { get; set; }
        public DateTime DataHora { get; set; }
        public int? TipoEstado { get; set; }
        public string Comentario { get; set; }
        public string Resposta { get; set; }
        public int? TipoResposta { get; set; }
        public DateTime? DataResposta { get; set; }
        public string Utilizador { get; set; }
        public bool? Imobilizado { get; set; }
        public int? EstadoAnterior { get; set; }
        public int? EstadoSeguinte { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string UtilizadorModificacao { get; set; }

        public ProcedimentosCcp NoProcedimentoNavigation { get; set; }
    }
}
