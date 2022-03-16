using System;

namespace Hydra.Such.Data.Database
{
    public partial class VisitasEstados
    {
        public int ID { get; set; }
        public int CodEstado { get; set; }
        public string Estado { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
    }
}
