using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.ProjectDiary
{
    public class ClientServicesViewModel : ErrorHandler
    {
        public string ClientNumber { get; set; }
        public int ServiceCode { get; set; }
        public bool? ServiceGroup { get; set; }
        public string ClientName { get; set; }
        public string ServiceDescription { get; set; }
    }
}
