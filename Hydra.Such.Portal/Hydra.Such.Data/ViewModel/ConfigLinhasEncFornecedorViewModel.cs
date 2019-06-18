using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class ConfigLinhasEncFornecedorViewModel : ErrorHandler
    {
        public string VendorNo { get; set; }
        public int LineNo { get; set; }
        public int? Type { get; set; }
        public string No { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public decimal? Quantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal? Valor { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string UtilizadorModificacao { get; set; }
    }
}
