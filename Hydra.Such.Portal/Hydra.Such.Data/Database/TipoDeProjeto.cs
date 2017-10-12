using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class TipoDeProjeto
    {
        public TipoDeProjeto()
        {
            Projetos = new HashSet<Projetos>();
        }

        public int Código { get; set; }
        public string Descrição { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }

        public ICollection<Projetos> Projetos { get; set; }
    }
}
