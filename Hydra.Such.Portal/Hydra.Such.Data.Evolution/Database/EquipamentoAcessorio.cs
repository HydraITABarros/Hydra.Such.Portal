using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class EquipamentoAcessorio
    {
        public int IdEquipamentoAcessorio { get; set; }
        public int IdEquipamento { get; set; }
        public int IdAcessorio { get; set; }

        public Acessorio IdAcessorioNavigation { get; set; }
        public Equipamento IdEquipamentoNavigation { get; set; }
    }
}
