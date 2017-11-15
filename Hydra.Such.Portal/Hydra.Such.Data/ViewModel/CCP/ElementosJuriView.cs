using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;

namespace Hydra.Such.Data.ViewModel.CCP
{
    public class ElementosJuriView
    {
        public string NoProcedimento { get; set; }
        public int NoLinha { get; set; }
        public string Utilizador { get; set; }
        public string NoEmpregado { get; set; }

        public string NomeEmpregado { get; set; }   // zpgm. this field doesn't exist in the RegistoDeAtas table. Is used to display the employee name

        public bool? Presidente { get; set; }
        public bool? Vogal { get; set; }
        public bool? Suplente { get; set; }
        public string Email { get; set; }
        public bool? EnviarEmail { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }

        public ProcedimentosCcp NoProcedimentoNavigation { get; set; }
    }
}
