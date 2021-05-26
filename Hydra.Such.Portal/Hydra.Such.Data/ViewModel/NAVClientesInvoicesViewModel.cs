using Hydra.Such.Data.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class NAVClientesInvoicesViewModel
    {
        public string Tipo { get; set; }
        public string No_ { get; set; }
        public string ProjectNo { get; set; }
        public string DataServPrestado { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime? CreationDate { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime? DocumentDate { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime? DueDate { get; set; }
        public string AmountIncludingVAT { get; set; }
        public string ValorPendente { get; set; }
        public bool Paid { get; set; }
        public string SellToCustomerNo { get; set; }
        public string BillToCustomerNo { get; set; }
        public string NoContrato { get; set; }
        public string NoCompromisso { get; set; }
        public string NoPedido { get; set; }
        public string RegionId { get; set; }
        public string FunctionalAreaId { get; set; }
        public string RespCenterId { get; set; }

        public string DocumentDateText { get; set; }
    }
}
