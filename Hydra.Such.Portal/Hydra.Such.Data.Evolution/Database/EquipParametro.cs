using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class EquipParametro
    {
        public EquipParametro()
        {
            EquipDadosTecnicos = new HashSet<EquipDadosTecnicos>();
        }

        public int IdEquipParametro { get; set; }
        public string Parametro { get; set; }
        public string Unidades { get; set; }

        public virtual ICollection<EquipDadosTecnicos> EquipDadosTecnicos { get; set; }
    }
}
