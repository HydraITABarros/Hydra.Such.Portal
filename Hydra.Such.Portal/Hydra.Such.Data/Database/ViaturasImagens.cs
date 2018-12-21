using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.Database
{
    public partial class ViaturasImagens
    {
        public string Matricula { get; set; }
        public byte[] Imagem { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string UtilizadorModificacao { get; set; }
    }
}
