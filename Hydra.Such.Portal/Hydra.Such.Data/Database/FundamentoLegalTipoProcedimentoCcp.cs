using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.Database
{
    public partial class FundamentoLegalTipoProcedimentoCcp
    {
        public int IdTipo { get; set; }
        public int IdFundamento { get; set; }
        public string DescricaoFundamento { get; set; }
        public bool? Activo { get; set; }

        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        public TipoProcedimentoCcp TipoNavigation { get; set; }
        public ICollection<ProcedimentosCcp> Procedimentos { get; set; }
    }
}
