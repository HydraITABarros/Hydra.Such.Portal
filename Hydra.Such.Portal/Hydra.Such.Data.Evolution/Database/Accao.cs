using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class Accao
    {
        public Accao()
        {
            Logs = new HashSet<Logs>();
        }

        public int IdAccao { get; set; }
        public string Nome { get; set; }

        public ICollection<Logs> Logs { get; set; }
    }
}
