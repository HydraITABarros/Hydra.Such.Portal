using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Extensions;
using Newtonsoft.Json;

namespace Hydra.Such.Data.Database
{
    public partial class AccaoFormacao
    {
        public AccaoFormacao()
        {
            SessoesFormacao = new HashSet<SessaoAccaoFormacao>();
            PedidosParticipacao = new HashSet<PedidoParticipacaoFormacao>();
        }

        public string IdAccao { get; set; }
        public string CodigoInterno { get; set; }
        public string DesignacaoAccao { get; set; }
        public string IdTema { get; set; }
        public int? Activa { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime? DataInicio { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime? DataFim { get; set; }
        public string IdEntidadeFormadora { get; set; }
        public decimal? NumeroTotalHoras { get; set; }
        public string UrlImagem { get; set; }


        [Newtonsoft.Json.JsonIgnore]
        public TemaFormacao TemaNavigation { get; set; }
        //[Newtonsoft.Json.JsonIgnore]
        public EntidadeFormadora EntidadeNavigation { get; set; }

        public ICollection<SessaoAccaoFormacao> SessoesFormacao { get; set; }
        public ICollection<PedidoParticipacaoFormacao> PedidosParticipacao { get; set; }
    }
}
