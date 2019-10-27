using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class FichaManutencaoRelatorioTestesQualitativos
    {
        public string Om { get; set; }
        public int IdEquipamento { get; set; }
        public string Codigo { get; set; }
        public string Versao { get; set; }
        public int IdUtilizador { get; set; }
        public DateTime Data { get; set; }
        public int IdTesteQualitativos { get; set; }
        public string RotinaTipo { get; set; }
        public byte ResultadoRotina { get; set; }
        public string Observacoes { get; set; }
        public int Id { get; set; }
    }
}
