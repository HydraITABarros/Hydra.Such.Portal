using System;
using System.Collections.Generic;

namespace Hydra.Such.Tester.Database
{
    public partial class Contactos
    {
        public string Nº { get; set; }
        public string Nome { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorCriação { get; set; }
        public string UtilizadorModificação { get; set; }
    }
}
