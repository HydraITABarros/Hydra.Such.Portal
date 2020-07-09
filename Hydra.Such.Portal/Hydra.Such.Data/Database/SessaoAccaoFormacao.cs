using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.Database
{
    public partial class SessaoAccaoFormacao
    {
        public string IdSessaoFormacao { get; set; }
        public string IdAccao { get; set; }
        public string HoraInicioSessao { get; set; }
        public string HoraFimSessao { get; set; }
        public decimal? DuracaoSessao { get; set; }
    }
}
