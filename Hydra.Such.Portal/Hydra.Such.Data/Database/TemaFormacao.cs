using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.Database
{
    public partial class TemaFormacao
    {
        public TemaFormacao()
        {
            AccoesTema = new HashSet<AccaoFormacao>();
        }

        public string IdTema { get; set; }
        public string CodigoInterno { get; set; }
        public string DescricaoTema { get; set; }
        public string UrlImagem { get; set; }
        public int? Activo { get; set; }

        public ICollection<AccaoFormacao> AccoesTema { get; set; }
    }
}
