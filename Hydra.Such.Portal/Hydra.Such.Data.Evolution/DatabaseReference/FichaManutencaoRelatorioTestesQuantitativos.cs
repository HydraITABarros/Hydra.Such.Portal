using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class FichaManutencaoRelatorioTestesQuantitativos
    {
        public int Id { get; set; }
        public int IdRelatorio { get; set; }
        public int IdTestesQuantitativos { get; set; }
        public string ResultadoRotina { get; set; }
        public string Observacoes { get; set; }
    }
}
