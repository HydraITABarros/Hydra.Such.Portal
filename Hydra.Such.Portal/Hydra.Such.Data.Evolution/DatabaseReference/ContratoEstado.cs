using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class ContratoEstado
    {
        public ContratoEstado()
        {
            Contrato = new HashSet<Contrato>();
        }

        public int IdEstado { get; set; }
        public string Nome { get; set; }

        public virtual ICollection<Contrato> Contrato { get; set; }
    }
}
