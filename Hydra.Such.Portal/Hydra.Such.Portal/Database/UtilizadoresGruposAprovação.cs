using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
{
    public partial class UtilizadoresGruposAprovação
    {
        public int GrupoAprovação { get; set; }
        public string UtilizadorAprovação { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }

        public GruposAprovação GrupoAprovaçãoNavigation { get; set; }
    }
}
