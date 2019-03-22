using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class ContratoTipo
    {
        public ContratoTipo()
        {
            Contrato = new HashSet<Contrato>();
        }

        public int IdTipoContrato { get; set; }
        public string Nome { get; set; }

        public ICollection<Contrato> Contrato { get; set; }
    }
}
