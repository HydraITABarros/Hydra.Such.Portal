using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.Database
{
    public partial class EntidadeFormadora
    {
        public EntidadeFormadora()
        {
            AccoesEntidade = new HashSet<AccaoFormacao>();
        }

        public string IdEntidade { get; set; }
        public string Referencia { get; set; }
        public string DescricaoEntidade { get; set; }

        public ICollection<AccaoFormacao> AccoesEntidade { get; set; }
    }
}
