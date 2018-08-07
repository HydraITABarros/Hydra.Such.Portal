using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
{
    public partial class AcordoPrecos
    {
        public string NoProcedimento { get; set; }
        public DateTime? DtInicio { get; set; }
        public DateTime? DtFim { get; set; }
        public decimal? ValorTotal { get; set; }
    }
}
