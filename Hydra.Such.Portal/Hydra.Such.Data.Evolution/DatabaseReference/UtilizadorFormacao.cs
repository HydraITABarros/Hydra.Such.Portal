using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class UtilizadorFormacao
    {
        public int IdFormacao { get; set; }
        public int IdUtilizador { get; set; }
        public string Descricao { get; set; }
        public DateTime DataFormacao { get; set; }
        public string Entidade { get; set; }
        public string Observacao { get; set; }

        public virtual Utilizador IdUtilizadorNavigation { get; set; }
    }
}
