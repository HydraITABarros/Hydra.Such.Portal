using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class Modelos
    {
        public Modelos()
        {
            Viaturas = new HashSet<Viaturas>();
        }

        public int CódigoMarca { get; set; }
        public int CódigoModelo { get; set; }
        public string Descrição { get; set; }

        public ICollection<Viaturas> Viaturas { get; set; }
    }
}
