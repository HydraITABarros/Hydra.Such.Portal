using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class ServiçosCliente
    {
        public string NºCliente { get; set; }
        public int CódServiço { get; set; }
        public bool? GrupoServiços { get; set; }

        public Serviços CódServiçoNavigation { get; set; }
    }
}
