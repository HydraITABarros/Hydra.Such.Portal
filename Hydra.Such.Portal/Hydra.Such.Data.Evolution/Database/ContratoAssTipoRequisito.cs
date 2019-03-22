using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class ContratoAssTipoRequisito
    {
        public int IdContratoAssTipoRequisito { get; set; }
        public string IdContrato { get; set; }
        public int IdTipoRequisito { get; set; }

        public Contrato IdContratoNavigation { get; set; }
        public ContratoTipoRequisito IdTipoRequisitoNavigation { get; set; }
    }
}
