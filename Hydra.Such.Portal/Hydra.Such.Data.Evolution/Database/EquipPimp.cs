using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class EquipPimp
    {
        public int IdEquipPimp { get; set; }
        public int IdEquipamento { get; set; }
        public DateTime DataPlano { get; set; }
        public int Dia { get; set; }
        public int Semana { get; set; }
        public int Mes { get; set; }
        public int Trimestre { get; set; }
        public int Semestre { get; set; }
        public int Ano { get; set; }
        public int? IdRotina { get; set; }
        public DateTime? DataExecucao { get; set; }
        public string FolhaAssociada { get; set; }
        public int? Tecnico { get; set; }
        public int? ResultadoPimp { get; set; }
        public int? IdCliente { get; set; }
        public string IdContrato { get; set; }
        public int? IdUtilizadorAlteracao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int? PlanoExecutado { get; set; }
        public bool? Replicado { get; set; }
        public int? IdEquipa { get; set; }

        public Cliente IdClienteNavigation { get; set; }
        public Contrato IdContratoNavigation { get; set; }
        public Equipa IdEquipaNavigation { get; set; }
        public Equipamento IdEquipamentoNavigation { get; set; }
        public Rotina IdRotinaNavigation { get; set; }
        public Utilizador IdUtilizadorAlteracaoNavigation { get; set; }
        public PlanoExecutado PlanoExecutadoNavigation { get; set; }
        public EquipEstado ResultadoPimpNavigation { get; set; }
        public Utilizador TecnicoNavigation { get; set; }
    }
}
