using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class DiariosRegAut
    {
        public byte[] Timestamp { get; set; }
        public int Tipo { get; set; }
        public string LivroDiário { get; set; }
        public string SecçãoDoDiário { get; set; }
        public string Recurso { get; set; }
        public string TipoDesc { get; set; }
    }
}
