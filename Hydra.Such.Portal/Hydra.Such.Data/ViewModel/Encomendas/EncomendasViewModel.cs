using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Encomendas
{
    public class EncomendasViewModel
    {
        public string No { get; set; }
        public string PayToVendorNo { get; set; }
        public string PayToName { get; set; }
        public string YourReference { get; set; }
        public DateTime OrderDate { get; set; }
        public string NoConsulta { get; set; }
        public DateTime ExpectedReceiptDate { get; set; }
        public string RequisitionNo { get; set; }

        public string RegionId { get; set; }
        public string FunctionalAreaId { get; set; }
        public string RespCenterId { get; set; }
    }
}
