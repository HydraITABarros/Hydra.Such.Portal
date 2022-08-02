using Hydra.Such.Data.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.Database
{
    public partial class SessaoAccaoFormacao
    {
        public string IdSessaoFormacao { get; set; }
        public string IdAccao { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime? DataSessao { get; set; }
        public string HoraInicioSessao { get; set; }
        public string HoraFimSessao { get; set; }
        public decimal? DuracaoSessao { get; set; }

        public DateTime? DataHoraActualizacao { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public AccaoFormacao AccaoNavigation { get; set; }
    }
}
