using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class DestinosFinaisResíduos
    {
        public DestinosFinaisResíduos()
        {
            DiárioDeProjeto = new HashSet<DiárioDeProjeto>();
        }

        public int Código { get; set; }
        public string Descrição { get; set; }

        public ICollection<DiárioDeProjeto> DiárioDeProjeto { get; set; }
    }
}
