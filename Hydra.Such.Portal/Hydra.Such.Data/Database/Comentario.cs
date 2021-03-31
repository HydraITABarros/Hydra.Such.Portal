using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Extensions;
using Newtonsoft.Json;

namespace Hydra.Such.Data.Database
{
    public partial class Comentario
    {
        public string NoDocumento { get; set; }
        public DateTime DataHoraComentario { get; set; }
        public string TextoComentario { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }

    }
}
