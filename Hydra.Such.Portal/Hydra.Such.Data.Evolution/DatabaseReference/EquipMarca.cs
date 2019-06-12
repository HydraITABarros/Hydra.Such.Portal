using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class EquipMarca
    {
        public EquipMarca()
        {
            Acessorio = new HashSet<Acessorio>();
            Equipamento = new HashSet<Equipamento>();
        }

        public int IdMarca { get; set; }
        public string Nome { get; set; }
        public bool? Activo { get; set; }

        public virtual ICollection<Acessorio> Acessorio { get; set; }
        public virtual ICollection<Equipamento> Equipamento { get; set; }
    }
}
