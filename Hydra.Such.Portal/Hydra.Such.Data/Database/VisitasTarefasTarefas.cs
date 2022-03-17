using System;

namespace Hydra.Such.Data.Database
{
    public partial class VisitasTarefasTarefas
    {
        public int CodTarefa { get; set; }
        public string Tarefa { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
    }
}
