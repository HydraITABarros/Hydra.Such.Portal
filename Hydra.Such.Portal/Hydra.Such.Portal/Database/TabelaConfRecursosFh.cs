using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
{
    public partial class TabelaConfRecursosFh
    {
        public string Tipo { get; set; }
        public string CodRecurso { get; set; }
        public string Descricao { get; set; }
        public decimal? PrecoUnitarioCusto { get; set; }
        public decimal? PrecoUnitarioVenda { get; set; }
        public string UnidMedida { get; set; }
        public string RubricaSalarial { get; set; }
    }
}
