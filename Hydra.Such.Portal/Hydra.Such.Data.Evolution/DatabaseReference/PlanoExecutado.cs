using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
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

        public virtual ICollection<EquipPimp> EquipPimp { get; set; }
        public virtual ICollection<InstituicaoPimp> InstituicaoPimp { get; set; }
    }
}
