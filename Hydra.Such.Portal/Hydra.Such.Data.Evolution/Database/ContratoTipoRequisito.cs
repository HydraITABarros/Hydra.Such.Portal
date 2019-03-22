using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
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

        public ICollection<ContratoAssTipoRequisito> ContratoAssTipoRequisito { get; set; }
    }
}
