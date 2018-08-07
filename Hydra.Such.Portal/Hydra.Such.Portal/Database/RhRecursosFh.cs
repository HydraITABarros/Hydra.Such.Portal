using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
{
    public partial class RhRecursosFh
    {
        public string NoEmpregado { get; set; }
        public string Recurso { get; set; }
        public string NomeRecurso { get; set; }
        public string FamiliaRecurso { get; set; }
        public string NomeEmpregado { get; set; }
        public string CriadoPor { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string AlteradoPor { get; set; }
        public DateTime? DataHoraUltimaAlteracao { get; set; }
    }
}
