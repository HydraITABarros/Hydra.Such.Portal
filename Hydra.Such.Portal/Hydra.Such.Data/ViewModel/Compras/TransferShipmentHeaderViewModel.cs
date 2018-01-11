using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Compras
{
    public class TransferShipment
    {
        public string TransferShipmentNo { get; set; }
        public string Comments { get; set; }
        public string RequisitionNo { get; set; }
        public string ProjectNo { get; set; }
        public string RegionNo { get; set; }
        public string FunctionalAreaNo { get; set; }
        public string ResponsibilityCenterNo { get; set; }

        public List<TransferShipmentLine> Lines { get; set; }
}
}
