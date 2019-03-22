using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class OrigemAvaria
    {
        public OrigemAvaria()
        {
            OrdemManutencao = new HashSet<OrdemManutencao>();
        }

        public int IdOrigemAvaria { get; set; }
        public string OrigemAvaria1 { get; set; }

        public ICollection<OrdemManutencao> OrdemManutencao { get; set; }
    }
}
