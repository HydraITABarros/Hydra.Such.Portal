using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class MetCalibracao
    {
        public int IdMetCalibracao { get; set; }
        public int IdMetEquipamento { get; set; }
        public DateTime DataCalibracao { get; set; }
        public DateTime? DataExecucao { get; set; }
        public string NumCertificado { get; set; }
        public string CriterioAceitacao { get; set; }
        public string Conformidade { get; set; }
        public bool? Activo { get; set; }
    }
}
