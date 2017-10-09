using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class ProcedimentosDeConfeção
    {
        public string NºPrato { get; set; }
        public int CódigoAção { get; set; }
        public string Descrição { get; set; }
        public int? NºOrdem { get; set; }

        public AçõesDeConfeção CódigoAçãoNavigation { get; set; }
        public FichasTécnicasPratos NºPratoNavigation { get; set; }
    }
}
