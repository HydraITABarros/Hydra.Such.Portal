using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
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

        public EquipCategoria IdCategoriaNavigation { get; set; }
        public ICollection<Acessorio> Acessorio { get; set; }
        public ICollection<Equipamento> Equipamento { get; set; }
    }
}
