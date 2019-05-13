using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class TipoContacto
    {
        public TipoContacto()
        {
            OrdemManutencao = new HashSet<OrdemManutencao>();
        }

        public int IdTipoContacto { get; set; }
        public string TipoContacto1 { get; set; }

        public virtual ICollection<OrdemManutencao> OrdemManutencao { get; set; }
    }
}
