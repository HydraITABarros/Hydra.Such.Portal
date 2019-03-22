using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class EmmInspeccoesResultados
    {
        public int Id { get; set; }
        public string Resultado { get; set; }
        public bool? Activo { get; set; }
    }
}
