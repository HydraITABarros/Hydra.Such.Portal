using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class TiposViatura
    {
        public TiposViatura()
        {
            Viaturas = new HashSet<Viaturas>();
        }

        public int CódigoTipo { get; set; }
        public string Descrição { get; set; }

        public ICollection<Viaturas> Viaturas { get; set; }
    }
}
