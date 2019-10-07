using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class OrdemManutencaoLinhaMateriais
    {
        public int IdOmLinhaMateriais { get; set; }
        public int IdOmLinha { get; set; }
        public string No { get; set; }
        public int? IdMaterial { get; set; }
        public string DescMaterial { get; set; }
        public int? QtdMaterial { get; set; }
        public DateTime? DataCriacao { get; set; }
        public int? UtilizadorCriacao { get; set; }
        public DateTime? HoraInicio { get; set; }
        public DateTime? HoraFim { get; set; }
    }
}
