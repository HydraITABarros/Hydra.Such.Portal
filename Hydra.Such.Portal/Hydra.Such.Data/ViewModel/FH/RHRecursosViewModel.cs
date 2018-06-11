using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.ViewModel.FH
{
    public partial class RHRecursosViewModel : ErrorHandler
    {
        public string NoEmpregado { get; set; }
        public string Recurso { get; set; }
        public string NomeRecurso { get; set; }
        public string FamiliaRecurso { get; set; }
        public string NomeEmpregado { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }

        public List<AnexosErrosViewModel> AnexosErros { get; set; }
    }
}
