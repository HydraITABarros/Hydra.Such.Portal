using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class FichaManutencaoRelatorioImagens
    {
        public int Id { get; set; }
        public int IdRelatorio { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
    }
}
