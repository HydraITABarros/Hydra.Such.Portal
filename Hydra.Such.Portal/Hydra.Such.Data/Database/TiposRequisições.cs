using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class TiposRequisições
    {
        public TiposRequisições()
        {
            PréRequisição = new HashSet<PréRequisição>();
        }

        public int Código { get; set; }
        public string Descrição { get; set; }

        public ICollection<PréRequisição> PréRequisição { get; set; }
    }
}
