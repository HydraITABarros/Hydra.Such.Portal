using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
{
    public partial class DistanciaFh
    {
        public string CódigoOrigem { get; set; }
        public string CódigoDestino { get; set; }
        public decimal? Distância { get; set; }
        public string CriadoPor { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string AlteradoPor { get; set; }
        public DateTime? DataHoraÚltimaAlteração { get; set; }
    }
}
