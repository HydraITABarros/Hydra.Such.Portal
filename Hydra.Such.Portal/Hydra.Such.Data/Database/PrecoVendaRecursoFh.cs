using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class PrecoVendaRecursoFh
    {
        public string Code { get; set; }
        public string Descricao { get; set; }
        public string CodTipoTrabalho { get; set; }
        public decimal? PrecoUnitario { get; set; }
        public decimal? CustoUnitario { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        public string FamiliaRecurso { get; set; }
        public string CriadoPor { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string AlteradoPor { get; set; }
        public DateTime? DataHoraUltimaAlteracao { get; set; }
    }
}
