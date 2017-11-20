using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class TabelaConfRecursosFH
    {
        public string Tipo { get; set; }
        public string CodigoRecurso { get; set; }
        public string Descricao { get; set; }
        public decimal? PrecoUnitarioCusto { get; set; }
        public decimal? PrecoUnitarioVenda { get; set; }
        public string UnidMedida { get; set; }
    }
}
