using System;
using System.Collections.Generic;

namespace Hydra.Such.Tester.Database
{
    public partial class ElementosJuri
    {
        public string NºProcedimento { get; set; }
        public int NºLinha { get; set; }
        public string Utilizador { get; set; }
        public string NºEmpregado { get; set; }
        public bool? Presidente { get; set; }
        public bool? Vogal { get; set; }
        public bool? Suplente { get; set; }
        public string Email { get; set; }
        public bool? EnviarEmail { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorCriação { get; set; }
        public string UtilizadorModificação { get; set; }

        public ProcedimentosCcp NºProcedimentoNavigation { get; set; }
    }
}
