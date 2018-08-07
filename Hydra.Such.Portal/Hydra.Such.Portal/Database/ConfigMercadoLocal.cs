using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
{
    public partial class ConfigMercadoLocal
    {
        public ConfigMercadoLocal()
        {
            Compras = new HashSet<Compras>();
        }

        public string RegiaoMercadoLocal { get; set; }
        public string Responsavel1 { get; set; }
        public string Responsavel2 { get; set; }
        public string Responsavel3 { get; set; }
        public string Responsavel4 { get; set; }

        public ICollection<Compras> Compras { get; set; }
    }
}
