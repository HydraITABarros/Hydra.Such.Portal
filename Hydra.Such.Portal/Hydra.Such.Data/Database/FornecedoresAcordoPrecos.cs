using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class FornecedoresAcordoPrecos
    {
        public FornecedoresAcordoPrecos()
        {
            LinhasAcordoPrecos = new HashSet<LinhasAcordoPrecos>();
        }

        public string NoProcedimento { get; set; }
        public string NoFornecedor { get; set; }
        public string NomeFornecedor { get; set; }
        public decimal? Valor { get; set; }
        public decimal? ValorConsumido { get; set; }

        public AcordoPrecos NoProcedimentoNavigation { get; set; }
        public ICollection<LinhasAcordoPrecos> LinhasAcordoPrecos { get; set; }
    }
}
