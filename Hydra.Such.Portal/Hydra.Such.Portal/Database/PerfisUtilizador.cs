using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
{
    public partial class PerfisUtilizador
    {
        public string IdUtilizador { get; set; }
        public int IdPerfil { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }

        public PerfisModelo IdPerfilNavigation { get; set; }
        public ConfigUtilizadores IdUtilizadorNavigation { get; set; }
    }
}
