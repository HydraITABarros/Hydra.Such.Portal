using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class ContratoTipoRequisito
    {
        public ContratoTipoRequisito()
        {
            ContratoAssTipoRequisito = new HashSet<ContratoAssTipoRequisito>();
        }

        public int IdTipoRequisito { get; set; }
        public string Grupo { get; set; }
        public string Descricao { get; set; }

        public virtual ICollection<ContratoAssTipoRequisito> ContratoAssTipoRequisito { get; set; }
    }
}
