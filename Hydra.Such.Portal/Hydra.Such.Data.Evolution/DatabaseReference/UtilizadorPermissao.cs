using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class UtilizadorPermissao
    {
        public int IdUtilizadorPermissao { get; set; }
        public int IdUser { get; set; }
        public string Regiao { get; set; }
        public string Equipa { get; set; }
        public string Area { get; set; }
        public string AreaOp { get; set; }

        public virtual Utilizador IdUserNavigation { get; set; }
    }
}
