using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.ViewModel.FH
{
    public partial class AutorizacaoFHRHViewModel
    {
        public string NoEmpregado { get; set; }
        public string NoResponsavel1 { get; set; }
        public string NoResponsavel2 { get; set; }
        public string NoResponsavel3 { get; set; }
        public string ValidadorRH1 { get; set; }
        public string ValidadorRH2 { get; set; }
        public string ValidadorRH3 { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
    }
}
