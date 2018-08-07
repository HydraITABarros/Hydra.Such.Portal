using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
{
    public partial class AcessosLocalizacoes
    {
        public string IdUtilizador { get; set; }
        public string Localizacao { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string UtilizadorModificacao { get; set; }
    }
}
