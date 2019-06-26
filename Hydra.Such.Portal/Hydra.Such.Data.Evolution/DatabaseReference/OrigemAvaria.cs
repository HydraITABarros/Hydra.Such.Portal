using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class OrigemAvaria
    {
        public OrigemAvaria()
        {
            OrdemManutencao = new HashSet<OrdemManutencao>();
        }

        public int IdOrigemAvaria { get; set; }
        public string OrigemAvaria1 { get; set; }

        public virtual ICollection<OrdemManutencao> OrdemManutencao { get; set; }
    }
}
