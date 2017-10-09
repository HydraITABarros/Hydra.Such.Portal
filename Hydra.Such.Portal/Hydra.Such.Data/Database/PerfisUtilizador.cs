using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class PerfisUtilizador
    {
        public string IdUtilizador { get; set; }
        public int IdPerfil { get; set; }

        public PerfisModelo IdPerfilNavigation { get; set; }
        public ConfigUtilizadores IdUtilizadorNavigation { get; set; }
    }
}
