using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class MetEquipamento
    {
        public int IdMetEquipamento { get; set; }
        public int IdCliente { get; set; }
        public int IdInstituicao { get; set; }
        public int IdServico { get; set; }
        public int IdCategoria { get; set; }
        public int IdMarca { get; set; }
        public int IdModelo { get; set; }
        public string NumSerie { get; set; }
        public string NumInventario { get; set; }
        public DateTime? DataUltimaCalibracao { get; set; }
        public DateTime? DataProximaCalibracao { get; set; }
        public DateTime? DataEfectivaCalibracao { get; set; }
        public string NumCertificado { get; set; }
        public string CriterioAceitacao { get; set; }
        public string Conformidade { get; set; }
        public bool? Activo { get; set; }
    }
}
