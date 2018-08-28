using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class TaxaResiduos
    {
        public string Recurso { get; set; }
        public string FamiliaRecurso { get; set; }
        public DateTime? Data { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }
    }
}
