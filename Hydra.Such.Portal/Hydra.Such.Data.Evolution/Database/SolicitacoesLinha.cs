using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class SolicitacoesLinha
    {
        public int IdOmLinha { get; set; }
        public string No { get; set; }
        public int NumLinha { get; set; }
        public int IdEquipamento { get; set; }
        public int? IdEquipEstado { get; set; }
        public int? IdRotina { get; set; }
        public int? Tbf { get; set; }

        public virtual EquipEstado IdEquipEstadoNavigation { get; set; }
        public virtual Equipamento IdEquipamentoNavigation { get; set; }
        public virtual Rotina IdRotinaNavigation { get; set; }
    }
}
