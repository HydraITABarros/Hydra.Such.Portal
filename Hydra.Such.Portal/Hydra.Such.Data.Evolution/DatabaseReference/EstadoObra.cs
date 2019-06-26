using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class EstadoObra
    {
        public EstadoObra()
        {
            OrdemManutencao = new HashSet<OrdemManutencao>();
        }

        public int IdEstadoObra { get; set; }
        public string Estado { get; set; }

        public virtual ICollection<OrdemManutencao> OrdemManutencao { get; set; }
    }
}
