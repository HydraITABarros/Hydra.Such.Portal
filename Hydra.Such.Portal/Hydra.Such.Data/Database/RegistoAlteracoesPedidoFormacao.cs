using Hydra.Such.Data.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.Database
{
    public partial class RegistoAlteracoesPedidoFormacao
    {
        public string IdPedidoFormacao { get; set; }
        public int? IdRegisto { get; set; }
        public int? TipoAlteracao { get; set; }
        public string DescricaoAlteracao { get; set; }
        public string UtilizadorAlteracao { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy HH:mm:ss")]
        public DateTime? DataHoraAlteracao { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public PedidoParticipacaoFormacao PedidoNavigation { get; set; }
    }
}
