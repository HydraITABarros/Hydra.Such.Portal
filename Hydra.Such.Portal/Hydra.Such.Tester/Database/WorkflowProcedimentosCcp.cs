using System;
using System.Collections.Generic;

namespace Hydra.Such.Tester.Database
{
    public partial class WorkflowProcedimentosCcp
    {
        public string NºProcedimento { get; set; }
        public int Estado { get; set; }
        public DateTime DataHora { get; set; }
        public int? TipoEstado { get; set; }
        public string Comentário { get; set; }
        public string Resposta { get; set; }
        public int? TipoResposta { get; set; }
        public DateTime? DataResposta { get; set; }
        public string Utilizador { get; set; }
        public bool? Imobilizado { get; set; }
        public int? EstadoAnterior { get; set; }
        public int? EstadoSeguinte { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }

        public ProcedimentosCcp NºProcedimentoNavigation { get; set; }
    }
}
