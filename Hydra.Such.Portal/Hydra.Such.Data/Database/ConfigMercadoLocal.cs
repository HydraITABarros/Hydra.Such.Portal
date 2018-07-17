﻿using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class ConfigMercadoLocal
    {
        public ConfigMercadoLocal()
        {
            Compras = new HashSet<ComprasModel>();
        }

        public string RegiaoMercadoLocal { get; set; }
        public string Responsavel1 { get; set; }
        public string Responsavel2 { get; set; }
        public string Responsavel3 { get; set; }

        public ICollection<ComprasModel> Compras { get; set; }
    }
}
