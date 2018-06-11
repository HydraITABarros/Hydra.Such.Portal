using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
{
    public partial class TextoFaturaContrato
    {
        public string NºContrato { get; set; }
        public int GrupoFatura { get; set; }
        public string NºProjeto { get; set; }
        public string TextoFatura { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }
    }
}
