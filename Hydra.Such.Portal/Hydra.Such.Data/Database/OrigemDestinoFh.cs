using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class OrigemDestinoFh
    {
        public string Código { get; set; }
        public string Descrição { get; set; }
        public string CriadoPor { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string AlteradoPor { get; set; }
        public DateTime? DataHoraÚltimaAlteração { get; set; }
    }
}
