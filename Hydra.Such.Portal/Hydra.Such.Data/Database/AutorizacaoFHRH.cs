using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class AutorizacaoFhRh
    {
        public string NoEmpregado { get; set; }
        public string NoResponsavel1 { get; set; }
        public string NoResponsavel2 { get; set; }
        public string NoResponsavel3 { get; set; }
        public string ValidadorRh1 { get; set; }
        public string ValidadorRh2 { get; set; }
        public string ValidadorRh3 { get; set; }
        public string ValidadorRhKm1 { get; set; }
        public string ValidadorRhKm2 { get; set; }
        public string CriadoPor { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string AlteradoPor { get; set; }
        public DateTime? DataHoraÚltimaAlteração { get; set; }
    }
}
