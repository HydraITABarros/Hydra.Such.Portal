using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class ContratoEstado
    {
        public ContratoEstado()
        {
            Contrato = new HashSet<Contrato>();
        }

        public int IdEstado { get; set; }
        public string Nome { get; set; }

        public ICollection<Contrato> Contrato { get; set; }
    }
}
