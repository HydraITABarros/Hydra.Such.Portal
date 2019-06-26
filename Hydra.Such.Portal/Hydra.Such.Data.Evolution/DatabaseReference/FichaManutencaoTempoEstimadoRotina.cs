using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class FichaManutencaoTempoEstimadoRotina
    {
        public int IdTempoEstimadoRotina { get; set; }
        public string CodigoFicha { get; set; }
        public string VersaoFicha { get; set; }
        public int IdRotina { get; set; }
        public decimal TempoEstimado { get; set; }
    }
}
