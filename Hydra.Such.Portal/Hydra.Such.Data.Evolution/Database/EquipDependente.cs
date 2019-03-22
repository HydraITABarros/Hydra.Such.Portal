using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class EquipDependente
    {
        public int IdEquipDependente { get; set; }
        public int IdEquipPrincipal { get; set; }
        public int IdEquipSecundario { get; set; }

        public Equipamento IdEquipPrincipalNavigation { get; set; }
        public Equipamento IdEquipSecundarioNavigation { get; set; }
    }
}
