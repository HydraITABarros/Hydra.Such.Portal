using Hydra.Such.Data.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hydra.Such.Data.ViewModel.GuiaTransporte
{
    public class ThirdPartyViewModel
    {
        public int EntityType { get; set; }
        public string EntityId { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string PhoneNo { get; set; }
        public string CountryCode { get; set; }
        public string CustomerAddress { get; set; }
        public string VatRegistrationNo { get; set; }
    }
}
