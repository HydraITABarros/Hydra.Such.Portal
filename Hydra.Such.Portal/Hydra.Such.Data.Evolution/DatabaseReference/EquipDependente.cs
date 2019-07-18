using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class EquipDependente
    {
        public int IdEquipDependente { get; set; }
        public int IdEquipPrincipal { get; set; }
        public int IdEquipSecundario { get; set; }

        public virtual Equipamento IdEquipPrincipalNavigation { get; set; }
        public virtual Equipamento IdEquipSecundarioNavigation { get; set; }
    }
}
