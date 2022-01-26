﻿using System;
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
        public string LocalRealizacao { get; set; }
        public decimal? CustoInscricao { get; set; }


        [Newtonsoft.Json.JsonIgnore]
        public TemaFormacao Tema { get; set; }
        //[Newtonsoft.Json.JsonIgnore]
        public EntidadeFormadora Entidade { get; set; }

        public ICollection<SessaoAccaoFormacao> SessoesFormacao { get; set; }
        public ICollection<PedidoParticipacaoFormacao> PedidosParticipacao { get; set; }
    }
}
