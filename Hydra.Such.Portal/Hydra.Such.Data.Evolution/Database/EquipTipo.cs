using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class EquipTipo
    {
        public EquipTipo()
        {
            EquipCategoria = new HashSet<EquipCategoria>();
        }

        public int IdTipo { get; set; }
        public string Designacao { get; set; }

        public virtual ICollection<EquipCategoria> EquipCategoria { get; set; }
    }
}
