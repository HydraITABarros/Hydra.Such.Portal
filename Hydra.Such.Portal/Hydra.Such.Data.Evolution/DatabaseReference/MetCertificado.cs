using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class MetCertificado
    {
        public int IdMetFicheiro { get; set; }
        public int IdMetEquipamento { get; set; }
        public int IdMetCalibracao { get; set; }
        public string Nome { get; set; }
        public DateTime Data { get; set; }
        public string ContentType { get; set; }
        public byte[] Ficheiro { get; set; }
        public bool? Activo { get; set; }
    }
}
