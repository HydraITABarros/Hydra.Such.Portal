using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
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

        public Cliente ClienteNavigation { get; set; }
        public EquipEstado IdEquipEstadoNavigation { get; set; }
        public Equipamento IdEquipamentoNavigation { get; set; }
        public OrdemManutencao IdOmNavigation { get; set; }
        public Rotina IdRotinaNavigation { get; set; }
        public Servico ServicoNavigation { get; set; }
    }
}
