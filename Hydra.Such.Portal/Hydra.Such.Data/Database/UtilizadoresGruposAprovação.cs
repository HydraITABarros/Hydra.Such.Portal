using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class UtilizadoresGruposAprovação
    {
        public int GrupoAprovação { get; set; }
        public string UtilizadorAprovação { get; set; }

        public GruposAprovação GrupoAprovaçãoNavigation { get; set; }
    }
}
