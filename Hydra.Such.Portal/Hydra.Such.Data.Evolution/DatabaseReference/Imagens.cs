using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class Imagens
    {
        public int IdImagem { get; set; }
        public string Nome { get; set; }
        public string ContentType { get; set; }
        public byte[] Ficheiro { get; set; }
        public bool? Activo { get; set; }
    }
}
