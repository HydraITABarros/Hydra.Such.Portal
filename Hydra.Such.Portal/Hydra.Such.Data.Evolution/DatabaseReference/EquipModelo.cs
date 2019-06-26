using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class EquipModelo
    {
        public EquipModelo()
        {
            Acessorio = new HashSet<Acessorio>();
            Equipamento = new HashSet<Equipamento>();
        }

        public int IdModelo { get; set; }
        public int IdCategoria { get; set; }
        public int IdMarca { get; set; }
        public int IdModelos { get; set; }
        public bool? Activo { get; set; }

        public virtual EquipCategoria IdCategoriaNavigation { get; set; }
        public virtual ICollection<Acessorio> Acessorio { get; set; }
        public virtual ICollection<Equipamento> Equipamento { get; set; }
    }
}
