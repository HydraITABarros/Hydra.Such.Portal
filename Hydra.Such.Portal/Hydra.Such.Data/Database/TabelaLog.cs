using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.Database
{
    public partial class TabelaLog
    {
        public int ID { get; set; }
        public string Tabela { get; set; }
        public string Descricao { get; set; }
        public string Utilizador { get; set; }
        public DateTime? DataHora { get; set; }
    }
}
