using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class Feriado
    {
        public int IdFeriado { get; set; }
        public DateTime DataFeriado { get; set; }
        public string Descricao { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public int Dia { get; set; }
        public int Semestre { get; set; }
        public int Trimestre { get; set; }
        public bool DataFixa { get; set; }
        public int? DiasDiferencaPascoa { get; set; }
    }
}
