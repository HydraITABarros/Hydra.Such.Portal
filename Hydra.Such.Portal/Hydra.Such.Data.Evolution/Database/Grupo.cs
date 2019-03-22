using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class Grupo
    {
        public Grupo()
        {
            EquipCategoria = new HashSet<EquipCategoria>();
        }

        public int IdGrupo { get; set; }
        public string Designacao { get; set; }

        public ICollection<EquipCategoria> EquipCategoria { get; set; }
    }
}
