using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class Datas
    {
        public int IdSemana { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public int Semana { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
    }
}
