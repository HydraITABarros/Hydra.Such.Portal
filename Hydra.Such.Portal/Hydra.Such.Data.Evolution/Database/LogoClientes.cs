using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class LogoClientes
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string ContentType { get; set; }
        public byte[] Ficheiro { get; set; }

        public virtual Cliente IdNavigation { get; set; }
    }
}
