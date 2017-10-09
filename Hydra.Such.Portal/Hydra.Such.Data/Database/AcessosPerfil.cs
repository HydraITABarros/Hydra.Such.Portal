using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class AcessosPerfil
    {
        public int IdPerfil { get; set; }
        public int Área { get; set; }
        public int Funcionalidade { get; set; }
        public bool? Leitura { get; set; }
        public bool? Inserção { get; set; }
        public bool? Modificação { get; set; }
        public bool? Eliminação { get; set; }

        public PerfisModelo IdPerfilNavigation { get; set; }
    }
}
