using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class Instrutores
    {
        public Instrutores()
        {
            ProcessosDisciplinaresInquérito = new HashSet<ProcessosDisciplinaresInquérito>();
        }

        public int Nº { get; set; }
        public string Nome { get; set; }

        public ICollection<ProcessosDisciplinaresInquérito> ProcessosDisciplinaresInquérito { get; set; }
    }
}
