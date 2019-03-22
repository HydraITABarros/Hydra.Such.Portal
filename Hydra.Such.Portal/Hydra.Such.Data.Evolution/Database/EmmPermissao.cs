using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class EmmPermissao
    {
        public int Id { get; set; }
        public string NumColaborador { get; set; }
        public string CodeArea { get; set; }
        public bool? Editor { get; set; }
        public bool? Supervisor { get; set; }
        public bool? Activo { get; set; }
    }
}
