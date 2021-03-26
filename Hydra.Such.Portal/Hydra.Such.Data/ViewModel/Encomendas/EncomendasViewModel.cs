using Hydra.Such.Data.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Encomendas
{
    public class EncomendasViewModel
    {
        public string No { get; set; }
        public string PayToVendorNo { get; set; }
        public string PayToName { get; set; }
        public string YourReference { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime OrderDate { get; set; }
        public string NoConsulta { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime? ExpectedReceiptDate { get; set; }
        public string RequisitionNo { get; set; }
        public string RegionId { get; set; }
        public string FunctionalAreaId { get; set; }
        public string RespCenterId { get; set; }

        public string VATRegistrationNo { get; set; }
        public string LocationCode { get; set; }
        public string AllocationNo { get; set; }
        public string CommitmentNo { get; set; }
        public string BuyFromVendorNo { get; set; }
        public string VPropNum { get; set; }
        public string PaymentTermsCode { get; set; }
        public string PayToAddress { get; set; }
        public string PayToAddress2 { get; set; }
        public string PayToPostCode { get; set; }
        public string PayToCity { get; set; }
        public string PayToCounty { get; set; }
        public string PayToCountryRegionCode { get; set; }
        public string PostingDescription { get; set; }
        public string ShipToName { get; set; }
        public string VendorShipmentNo { get; set; }

        public bool HasAnAdvance { get; set; }
        public decimal Total { get; set; }

        public decimal VlrRececionadoComIVA { get; set; }
        public decimal VlrRececionadoSemIVA { get; set; }
        public string PedidosPagamentoPendentes { get; set; }
        public string NoPedidosPagamento { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }

    }
}
