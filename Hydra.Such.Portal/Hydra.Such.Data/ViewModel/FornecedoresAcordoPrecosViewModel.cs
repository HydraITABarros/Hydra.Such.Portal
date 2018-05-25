using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class FornecedoresAcordoPrecosViewModel : ErrorHandler
    {
        public string NoProcedimento { get; set; }
        public string NoFornecedor { get; set; }
        public string NomeFornecedor { get; set; }
        public decimal? Valor { get; set; }
        public decimal? ValorConsumido { get; set; }
    }
}
