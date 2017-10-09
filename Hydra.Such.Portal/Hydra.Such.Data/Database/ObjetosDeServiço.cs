using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class ObjetosDeServiço
    {
        public ObjetosDeServiço()
        {
            Contratos = new HashSet<Contratos>();
        }

        public int Código { get; set; }
        public string Descrição { get; set; }
        public bool? Bloqueado { get; set; }
        public string CódÁrea { get; set; }

        public ICollection<Contratos> Contratos { get; set; }
    }
}
