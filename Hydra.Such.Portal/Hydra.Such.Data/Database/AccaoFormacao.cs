using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.Database
{
    public partial class AccaoFormacao
    {
        public AccaoFormacao()
        {
            SessoesFormacao = new HashSet<SessaoAccaoFormacao>();
        }

        public string IdAccao { get; set; }
        public string DesignacaoAccao { get; set; }
        public string IdTema { get; set; }
        public int? Activa { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string IdEntidadeFormadora { get; set; }
        public decimal? NumeroTotalHoras { get; set; }
        public string UrlImagem { get; set; }

        public ICollection<SessaoAccaoFormacao> SessoesFormacao { get; set; }
    }
}
