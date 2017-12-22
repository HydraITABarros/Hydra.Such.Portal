using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class UtilizadoresMovimentosDeAprovação
    {
        public int NºMovimento { get; set; }
        public string Utilizador { get; set; }

        public MovimentosDeAprovação NºMovimentoNavigation { get; set; }
    }
}
