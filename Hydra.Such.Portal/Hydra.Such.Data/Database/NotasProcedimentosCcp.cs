using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class NotasProcedimentosCcp
    {
        public string NºProcedimento { get; set; }
        public int NºLinha { get; set; }
        public DateTime? DataHora { get; set; }
        public string Nota { get; set; }
        public string Utilizador { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }

        public ProcedimentosCcp NºProcedimentoNavigation { get; set; }
    }
}
