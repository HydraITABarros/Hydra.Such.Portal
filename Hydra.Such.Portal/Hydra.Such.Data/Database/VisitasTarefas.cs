using System;

namespace Hydra.Such.Data.Database
{
    public partial class VisitasTarefas
    {
        public string CodVisita { get; set; }
        public int? Ordem { get; set; }
        public int? CodTarefa { get; set; }
        public string Tarefa { get; set; }
        public DateTime? Data { get; set; }
        public TimeSpan? Duracao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
    }
}
