using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class FichaManutencaoRelatorioTestesQualitativos
    {
        public int Id { get; set; }
        public int IdRelatorio { get; set; }
        public int IdTesteQualitativos { get; set; }
        public byte ResultadoRotina { get; set; }
        public string Observacoes { get; set; }
    }
}
