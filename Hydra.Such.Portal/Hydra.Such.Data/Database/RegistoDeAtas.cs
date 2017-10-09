using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class RegistoDeAtas
    {
        public string NºProcedimento { get; set; }
        public string NºAta { get; set; }
        public DateTime? DataDaAta { get; set; }
        public string Observações { get; set; }

        public ProcedimentosCcp ProcedimentosCcp { get; set; }
    }
}
