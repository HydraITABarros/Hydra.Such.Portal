using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class ProjetosFaturação
    {
        public int NºUnidadeProdutiva { get; set; }
        public string NºProjeto { get; set; }
        public bool? Ativo { get; set; }

        public Projetos NºProjetoNavigation { get; set; }
        public UnidadesProdutivas NºUnidadeProdutivaNavigation { get; set; }
    }
}
