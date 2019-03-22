using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class EquipDadosTecnicos
    {
        public int IdEquipDadosTecnicos { get; set; }
        public int IdEquipamento { get; set; }
        public int IdEquipParametro { get; set; }
        public decimal? Valor { get; set; }

        public EquipParametro IdEquipParametroNavigation { get; set; }
        public Equipamento IdEquipamentoNavigation { get; set; }
    }
}
