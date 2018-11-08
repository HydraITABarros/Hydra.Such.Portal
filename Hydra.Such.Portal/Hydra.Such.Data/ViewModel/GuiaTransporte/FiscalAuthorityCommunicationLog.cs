using Hydra.Such.Data.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hydra.Such.Data.ViewModel.GuiaTransporte
{
    public class FiscalAuthorityCommunicationLog
    {
        public string SourceNo { get; set; }
        public string DocumentCodeId { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy HH:mm:ss")]
        public DateTime CommunicationDateTime { get; set; }
        public string ReturnCode { get; set; }
        public string ReturnMessage { get; set; }
    }
}
