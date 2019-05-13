using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class MaintenanceOrderClienteIteracao
    {
        public int IdClienteIteracao { get; set; }
        public string NumOm { get; set; }
        public int IdUser { get; set; }
        public string TipoContactoCliente { get; set; }
        public string NumDocumento { get; set; }
        public string NumCompromisso { get; set; }
        public int? NumAnexo { get; set; }
        public string Observacao { get; set; }
    }
}
