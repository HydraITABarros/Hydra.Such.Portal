using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class OrdemManutencaoEquipamentos
    {
        public int IdOmEquipamento { get; set; }
        public int IdOm { get; set; }
        public int Cliente { get; set; }
        public int Servico { get; set; }
        public int IdEquipamento { get; set; }
        public int? IdRotina { get; set; }
        public int? IdEquipEstado { get; set; }
        public int? TempoEntreAvarias { get; set; }

        public virtual Cliente ClienteNavigation { get; set; }
        public virtual EquipEstado IdEquipEstadoNavigation { get; set; }
        public virtual Equipamento IdEquipamentoNavigation { get; set; }
        public virtual OrdemManutencao IdOmNavigation { get; set; }
        public virtual Rotina IdRotinaNavigation { get; set; }
        public virtual Servico ServicoNavigation { get; set; }
    }
}
