using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.Database
{
    public partial class TipoProcedimentoCcp
    {
        public int IdTipo { get; set; }
        public string Abreviatura { get; set; }
        public string DescricaoTipo { get; set; }
        public bool? Activo { get; set; }

        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }

        public ICollection<FundamentoLegalTipoProcedimentoCcp> Fundamentos { get; set; }
        public ICollection<ProcedimentosCcp> Procedimentos { get; set; }
    }
}
