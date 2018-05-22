using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class AcordoPrecos
    {
        public AcordoPrecos()
        {
            FornecedoresAcordoPrecos = new HashSet<FornecedoresAcordoPrecos>();
            LinhasAcordoPrecos = new HashSet<LinhasAcordoPrecos>();
        }

        public string NoProcedimento { get; set; }
        public DateTime? DtInicio { get; set; }
        public DateTime? DtFim { get; set; }
        public decimal? ValorTotal { get; set; }

        public ICollection<FornecedoresAcordoPrecos> FornecedoresAcordoPrecos { get; set; }
        public ICollection<LinhasAcordoPrecos> LinhasAcordoPrecos { get; set; }
    }
}
