using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class TabelaConfRecursosFHViewModel
    {
        public int Tipo { get; set; }
        public string CodigoRecurso { get; set; }
        public string Descricao { get; set; }
        public decimal? PrecoUnitarioCusto { get; set; }
        public decimal? PrecoUnitarioVenda { get; set; }
        public string UnidMedida { get; set; }
        public string RubricaSalarial { get; set; }
        public bool CalculoAutomatico { get; set; }
    }
}
