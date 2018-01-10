using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Compras
{
    public class TransferShipmentLine
    {
        public string TransferShipmentNo { get; set; }
        public int TransferShipmentLineNo { get; set; }
        public string ProductNo { get; set; }
        public string ProductDescription { get; set; }
        public decimal? Quantity { get; set; }
        public string UnitOfMeasureNo { get; set; }
        public decimal? UnitCost { get; set; }

        public string RegionNo { get; set; }
        public string FunctionalAreaNo { get; set; }
        public string CenterResponsibilityNo { get; set; }
    }
}
