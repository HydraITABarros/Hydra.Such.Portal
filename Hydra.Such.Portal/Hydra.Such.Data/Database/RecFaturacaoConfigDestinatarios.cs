using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class RecFaturacaoConfigDestinatarios
    {
        public string Codigo { get; set; }
        public string CodArea { get; set; }
        public string CodCentroResponsabilidade { get; set; }
        public string Destinatario { get; set; }
        public bool? Mostra { get; set; }
        public bool? EnviaEmail { get; set; }
        public string Notas { get; set; }
    }
}
