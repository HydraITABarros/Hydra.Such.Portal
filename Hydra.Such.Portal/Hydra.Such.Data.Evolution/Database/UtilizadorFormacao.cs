using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class UtilizadorFormacao
    {
        public int IdFormacao { get; set; }
        public int IdUtilizador { get; set; }
        public string Descricao { get; set; }
        public DateTime DataFormacao { get; set; }
        public string Entidade { get; set; }
        public string Observacao { get; set; }

        public Utilizador IdUtilizadorNavigation { get; set; }
    }
}
