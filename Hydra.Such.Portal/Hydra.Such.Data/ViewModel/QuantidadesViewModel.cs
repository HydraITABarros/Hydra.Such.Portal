using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public partial class QuantidadesViewModel
    {
        public string Codigo { get; set; }
        public string CodLocalizacao { get; set; }
        public decimal QuantDisponivel { get; set; }
        public decimal QuantReservada { get; set; }
    }
}
