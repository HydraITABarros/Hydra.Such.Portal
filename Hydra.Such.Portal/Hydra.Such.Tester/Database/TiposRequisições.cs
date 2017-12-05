using System;
using System.Collections.Generic;

namespace Hydra.Such.Tester.Database
{
    public partial class TiposRequisições
    {
        public TiposRequisições()
        {
            PréRequisição = new HashSet<PréRequisição>();
        }

        public int Código { get; set; }
        public string Descrição { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }

        public ICollection<PréRequisição> PréRequisição { get; set; }
    }
}
