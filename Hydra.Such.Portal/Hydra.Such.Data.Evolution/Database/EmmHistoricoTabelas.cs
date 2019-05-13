using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class EmmHistoricoTabelas
    {
        public int Id { get; set; }
        public string Tabela { get; set; }
        public bool Activo { get; set; }
    }
}
