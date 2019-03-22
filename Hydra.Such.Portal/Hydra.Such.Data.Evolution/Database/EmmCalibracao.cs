using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class EmmCalibracao
    {
        public int Id { get; set; }
        public string Calibracao { get; set; }
        public bool? Activo { get; set; }
    }
}
