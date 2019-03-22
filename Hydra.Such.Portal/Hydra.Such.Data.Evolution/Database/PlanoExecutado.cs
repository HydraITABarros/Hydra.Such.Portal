using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class PlanoExecutado
    {
        public PlanoExecutado()
        {
            EquipPimp = new HashSet<EquipPimp>();
            InstituicaoPimp = new HashSet<InstituicaoPimp>();
        }

        public int IdPlanoExecutado { get; set; }
        public string Nome { get; set; }
        public bool? Activo { get; set; }

        public ICollection<EquipPimp> EquipPimp { get; set; }
        public ICollection<InstituicaoPimp> InstituicaoPimp { get; set; }
    }
}
