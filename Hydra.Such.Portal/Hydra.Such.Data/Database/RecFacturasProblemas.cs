using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class RecFacturasProblemas
    {
        public RecFacturasProblemas()
        {
            RececaoFaturacaoWorkflow = new HashSet<RececaoFaturacaoWorkflow>();
        }

        public string Codigo { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public string EnvioAreas { get; set; }
        public bool? Devolvido { get; set; }
        public bool? EnvioEmail { get; set; }
        public int? CodModeloEmail { get; set; }
        public bool? Bloqueado { get; set; }

        public ICollection<RececaoFaturacaoWorkflow> RececaoFaturacaoWorkflow { get; set; }
    }
}
