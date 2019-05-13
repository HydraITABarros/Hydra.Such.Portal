using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class EmmUtilizadores
    {
        public int Id { get; set; }
        public int IdGrupo { get; set; }
        public int NumEmm { get; set; }
        public string IdUtilizador { get; set; }
        public bool? Activo { get; set; }
    }
}
