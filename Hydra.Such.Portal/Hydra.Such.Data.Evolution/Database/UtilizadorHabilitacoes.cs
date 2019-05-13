using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class UtilizadorHabilitacoes
    {
        public int IdHabilitacao { get; set; }
        public int IdUtilizador { get; set; }
        public string Descricao { get; set; }
        public DateTime DataConclusao { get; set; }
        public string Entidade { get; set; }
        public string Observacao { get; set; }

        public virtual Utilizador IdUtilizadorNavigation { get; set; }
    }
}
