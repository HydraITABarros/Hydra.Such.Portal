using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
{
    public partial class Locais
    {
        public Locais()
        {
            PréRequisiçãoCódigoLocalEntregaNavigation = new HashSet<PréRequisição>();
            PréRequisiçãoCódigoLocalRecolhaNavigation = new HashSet<PréRequisição>();
        }

        public int Código { get; set; }
        public string Descrição { get; set; }
        public string Endereço { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }

        public ICollection<PréRequisição> PréRequisiçãoCódigoLocalEntregaNavigation { get; set; }
        public ICollection<PréRequisição> PréRequisiçãoCódigoLocalRecolhaNavigation { get; set; }
    }
}
