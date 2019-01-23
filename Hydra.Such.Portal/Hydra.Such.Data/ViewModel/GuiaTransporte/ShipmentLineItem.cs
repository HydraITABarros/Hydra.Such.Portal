using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.GuiaTransporte
{
    public class ShipmentLineItem
    {
        public int ItemType { get; set; }   
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public string UnitOfMeasure { get; set; }
    }
}
