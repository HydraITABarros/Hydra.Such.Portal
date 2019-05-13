using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class EmmEntidade
    {
        public int Id { get; set; }
        public string Entidade { get; set; }
        public bool Activo { get; set; }
    }
}
