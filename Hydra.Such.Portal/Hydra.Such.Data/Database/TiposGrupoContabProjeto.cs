using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class TiposGrupoContabProjeto
    {
        public TiposGrupoContabProjeto()
        {
            Projetos = new HashSet<Projetos>();
        }

        public int Código { get; set; }
        public string Descrição { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }

        public ICollection<Projetos> Projetos { get; set; }
    }
}
