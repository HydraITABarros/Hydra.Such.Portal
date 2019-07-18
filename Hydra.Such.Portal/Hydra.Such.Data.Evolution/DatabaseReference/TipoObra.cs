using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class TipoObra
    {
        public TipoObra()
        {
            OrdemManutencao = new HashSet<OrdemManutencao>();
        }

        public int IdTipoObra { get; set; }
        public string TipoObra1 { get; set; }
        public string Descricao { get; set; }

        public virtual ICollection<OrdemManutencao> OrdemManutencao { get; set; }
    }
}
