using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class AcordoPrecosModelView
    {
        public string NoProcedimento { get; set; }
        public DateTime? DtInicio { get; set; }
        public string DtInicioTexto { get; set; }
        public DateTime? DtFim { get; set; }
        public string DtFimTexto { get; set; }
        public decimal? ValorTotal { get; set; }
    }
}
