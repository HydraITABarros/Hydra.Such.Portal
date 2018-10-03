using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class Comprador
    {
        public string CodComprador { get; set; }
        public string NomeComprador { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorCriação { get; set; }
        public string UtilizadorModificação { get; set; }
    }
}
