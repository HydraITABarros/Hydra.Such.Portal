using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class FichaManutencaoRelatorioMaterial
    {
        public int Id { get; set; }
        public int IdRelatorio { get; set; }
        public string Descricao { get; set; }
        public string Quantidade { get; set; }
        public int? FornecidoPor { get; set; }
    }
}
