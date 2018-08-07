using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
{
    public partial class AcessosDimensões
    {
        public string IdUtilizador { get; set; }
        public int Dimensão { get; set; }
        public string ValorDimensão { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorCriação { get; set; }
        public string UtilizadorModificação { get; set; }

        public ConfigUtilizadores IdUtilizadorNavigation { get; set; }
    }
}
