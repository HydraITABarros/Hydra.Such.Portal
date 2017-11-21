using System;
using System.Collections.Generic;

namespace Hydra.Such.Tester.Database
{
    public partial class Anexos
    {
        public int TipoOrigem { get; set; }
        public string NºOrigem { get; set; }
        public int NºLinha { get; set; }
        public string UrlAnexo { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorCriação { get; set; }
        public string UtilizadorModificação { get; set; }

        public Requisição NºOrigem1 { get; set; }
        public PréRequisição NºOrigemNavigation { get; set; }
    }
}
