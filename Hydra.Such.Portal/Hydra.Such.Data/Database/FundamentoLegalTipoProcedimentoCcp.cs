using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.Database
{
    public partial class FundamentoLegalTipoProcedimentoCcp
    {
        public FundamentoLegalTipoProcedimentoCcp()
        {
            ProcedimentosCcp = new HashSet<ProcedimentosCcp>();
        }
        public int IdTipo { get; set; }
        public int IdFundamento { get; set; }
        public string DescricaoFundamento { get; set; }
        public bool? Activo { get; set; }

        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }

        /* 
            * /!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\
            *  zpgm.The [Newtonsoft.Json.JsonIgnore] attribute is needed to overcome the exception:
            *      (...)
            *          Newtonsoft.Json.JsonSerializationException: 
            *          Self referencing loop detected for property 'idTipoNavigation' with type 'Hydra.Such.Data.Database.TipoProcedimentoCcp'. Path 'fundamentos[0]'.
            *      (...)
            * /!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\/!\                  
        */
        [Newtonsoft.Json.JsonIgnore]
        public TipoProcedimentoCcp IdTipoNavigation { get; set; }
        public ICollection<ProcedimentosCcp> ProcedimentosCcp { get; set; }
    }
}
