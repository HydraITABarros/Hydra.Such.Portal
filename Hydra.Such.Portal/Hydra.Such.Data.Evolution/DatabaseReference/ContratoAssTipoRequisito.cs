using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class ContratoAssTipoRequisito
    {
        public int IdContratoAssTipoRequisito { get; set; }
        public string IdContrato { get; set; }
        public int IdTipoRequisito { get; set; }

        public virtual Contrato IdContratoNavigation { get; set; }
        public virtual ContratoTipoRequisito IdTipoRequisitoNavigation { get; set; }
    }
}
