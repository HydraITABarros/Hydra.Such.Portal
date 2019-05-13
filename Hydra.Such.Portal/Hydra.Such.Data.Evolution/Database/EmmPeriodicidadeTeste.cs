using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class EmmPeriodicidadeTeste
    {
        public int Id { get; set; }
        public string Periodicidade { get; set; }
        public int? Ano { get; set; }
        public int? Mes { get; set; }
        public int? Dia { get; set; }
        public int? TotalDias { get; set; }
        public bool? Activo { get; set; }
    }
}
