using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class EquipCategoria
    {
        public EquipCategoria()
        {
            EquipModelo = new HashSet<EquipModelo>();
            Equipamento = new HashSet<Equipamento>();
        }

        public int IdCategoria { get; set; }
        public string Nome { get; set; }
        public bool? Activo { get; set; }
        public int? IdGrupo { get; set; }
        public int? IdFamilia { get; set; }
        public string FichaManutencao { get; set; }
        public bool? Emm { get; set; }
        public int? IdTipo { get; set; }

        public virtual Familia IdFamiliaNavigation { get; set; }
        public virtual Grupo IdGrupoNavigation { get; set; }
        public virtual EquipTipo IdTipoNavigation { get; set; }
        public virtual ICollection<EquipModelo> EquipModelo { get; set; }
        public virtual ICollection<Equipamento> Equipamento { get; set; }
    }
}
