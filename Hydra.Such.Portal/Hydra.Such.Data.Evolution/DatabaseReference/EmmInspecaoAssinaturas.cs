using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class EmmInspecaoAssinaturas
    {
        public int Id { get; set; }
        public string IdSupervisor { get; set; }
        public string FileName { get; set; }
        public byte[] Ficheiro { get; set; }
        public bool? Activo { get; set; }
    }
}
