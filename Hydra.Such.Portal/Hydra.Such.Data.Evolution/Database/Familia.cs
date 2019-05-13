using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class Familia
    {
        public Familia()
        {
            EquipCategoria = new HashSet<EquipCategoria>();
        }

        public int IdFamilia { get; set; }
        public string Familia1 { get; set; }
        public string SubFamilia { get; set; }

        public virtual ICollection<EquipCategoria> EquipCategoria { get; set; }
    }
}
