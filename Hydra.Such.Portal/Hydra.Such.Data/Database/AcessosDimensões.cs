using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class AcessosDimensões
    {
        public string IdUtilizador { get; set; }
        public int Dimensão { get; set; }
        public string ValorDimensão { get; set; }

        public ConfigUtilizadores IdUtilizadorNavigation { get; set; }
    }
}
