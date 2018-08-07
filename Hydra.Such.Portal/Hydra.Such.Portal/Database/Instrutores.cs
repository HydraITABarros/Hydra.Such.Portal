using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
{
    public partial class Instrutores
    {
        public Instrutores()
        {
            ProcessosDisciplinaresInquérito = new HashSet<ProcessosDisciplinaresInquérito>();
        }

        public int Nº { get; set; }
        public string Nome { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }

        public ICollection<ProcessosDisciplinaresInquérito> ProcessosDisciplinaresInquérito { get; set; }
    }
}
