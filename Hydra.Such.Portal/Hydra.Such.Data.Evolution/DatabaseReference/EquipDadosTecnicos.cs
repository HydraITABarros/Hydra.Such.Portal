using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class EquipDadosTecnicos
    {
        public int IdEquipDadosTecnicos { get; set; }
        public int IdEquipamento { get; set; }
        public int IdEquipParametro { get; set; }
        public decimal? Valor { get; set; }

        public virtual EquipParametro IdEquipParametroNavigation { get; set; }
        public virtual Equipamento IdEquipamentoNavigation { get; set; }
    }
}
