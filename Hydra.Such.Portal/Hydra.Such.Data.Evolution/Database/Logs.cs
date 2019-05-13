using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class Logs
    {
        public int IdLog { get; set; }
        public int IdUtilizador { get; set; }
        public int IdAccao { get; set; }
        public DateTime Data { get; set; }
        public string Descritivo { get; set; }
        public string Tabela { get; set; }

        public virtual Accao IdAccaoNavigation { get; set; }
    }
}
