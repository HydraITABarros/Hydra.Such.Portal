using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class TextoFaturaContrato
    {
        public string NºContrato { get; set; }
        public int GrupoFatura { get; set; }
        public string NºProjeto { get; set; }
        public string TextoFatura { get; set; }
    }
}
