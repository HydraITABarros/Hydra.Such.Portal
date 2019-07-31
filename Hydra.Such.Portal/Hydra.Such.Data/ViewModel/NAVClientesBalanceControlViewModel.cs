using Hydra.Such.Data.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class NAVClientesBalanceControlViewModel
    {
        public int EntryNo { get; set; }
        public string CustomerNo { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime? PostingDate { get; set; }
        public string DocumentType { get; set; }
        public string DocumentTypeText { get; set; }
        public string DocumentNo { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime? DocumentDate { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime? DueDate { get; set; }
        public string Description { get; set; }

        public decimal? Amount { get; set; }
        public decimal? RemainingAmount { get; set; }

        public bool SinalizacaoRec { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime? DataConcil { get; set; }
        public string Obs { get; set; }
        public string RegionId { get; set; }
        public string FunctionalAreaId { get; set; }
        public string RespCenterId { get; set; }
    }
}
