using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class EquipamentoAcessorio
    {
        public int IdEquipamentoAcessorio { get; set; }
        public int IdEquipamento { get; set; }
        public int IdAcessorio { get; set; }

        public virtual Acessorio IdAcessorioNavigation { get; set; }
        public virtual Equipamento IdEquipamentoNavigation { get; set; }
    }
}
