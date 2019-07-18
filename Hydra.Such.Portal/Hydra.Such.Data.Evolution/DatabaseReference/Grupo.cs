using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class Grupo
    {
        public Grupo()
        {
            EquipCategoria = new HashSet<EquipCategoria>();
        }

        public int IdGrupo { get; set; }
        public string Designacao { get; set; }

        public virtual ICollection<EquipCategoria> EquipCategoria { get; set; }
    }
}
