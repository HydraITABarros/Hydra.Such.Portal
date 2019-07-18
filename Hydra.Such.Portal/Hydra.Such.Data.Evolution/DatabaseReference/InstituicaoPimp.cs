using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class InstituicaoPimp
    {
        public int IdInstituicaoPimp { get; set; }
        public int IdInstituicao { get; set; }
        public DateTime DataPlano { get; set; }
        public int Dia { get; set; }
        public int Semana { get; set; }
        public int Mes { get; set; }
        public int Trimestre { get; set; }
        public int Semestre { get; set; }
        public int Ano { get; set; }
        public int IdRotina { get; set; }
        public int IdEquipa { get; set; }
        public DateTime? DataExecucao { get; set; }
        public int? PlanoExecutado { get; set; }
        public int? ResultadoPimp { get; set; }
        public bool? Replicado { get; set; }

        public virtual Equipa IdEquipaNavigation { get; set; }
        public virtual Instituicao IdInstituicaoNavigation { get; set; }
        public virtual Rotina IdRotinaNavigation { get; set; }
        public virtual PlanoExecutado PlanoExecutadoNavigation { get; set; }
    }
}
