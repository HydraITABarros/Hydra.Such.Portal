using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;

namespace Hydra.Such.Data.ViewModel.CCP
{
    public class NotasProcedimentoCCPView
    {
        public string NoProcedimento { get; set; }
        public int NoLinha { get; set; }
        public DateTime? DataHora { get; set; }
        public string Nota { get; set; }
        public string Utilizador { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string UtilizadorModificacao { get; set; }

        public ProcedimentosCcp NoProcedimentoNavigation { get; set; }
    }
}
