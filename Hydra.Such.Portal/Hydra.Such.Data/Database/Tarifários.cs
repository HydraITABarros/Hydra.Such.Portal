using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class Tarifários
    {
        public Tarifários()
        {
            CartõesTelemóveis = new HashSet<CartõesTelemóveis>();
        }

        public int Código { get; set; }
        public string Descrição { get; set; }

        public ICollection<CartõesTelemóveis> CartõesTelemóveis { get; set; }
    }
}
