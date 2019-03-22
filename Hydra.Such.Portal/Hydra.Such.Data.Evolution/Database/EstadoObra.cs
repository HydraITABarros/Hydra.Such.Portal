using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class EstadoObra
    {
        public EstadoObra()
        {
            OrdemManutencao = new HashSet<OrdemManutencao>();
        }

        public int IdEstadoObra { get; set; }
        public string Estado { get; set; }

        public ICollection<OrdemManutencao> OrdemManutencao { get; set; }
    }
}
