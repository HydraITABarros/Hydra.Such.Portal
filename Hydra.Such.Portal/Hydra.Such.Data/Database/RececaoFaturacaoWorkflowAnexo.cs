using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class RececaoFaturacaoWorkflowAnexo
    {
        public int Id { get; set; }
        public int? Idwokflow { get; set; }
        public string Caminho { get; set; }
        public string Comentario { get; set; }
    }
}
