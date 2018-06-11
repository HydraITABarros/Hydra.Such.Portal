using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Clients
{
    public class ShipToAddressViewModel : ErrorHandler
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Address_2 { get; set; }
        public string Post_Code { get; set; }
        public string City { get; set; }
        public string Country_Region_Code { get; set; }
        public string Phone_No { get; set; }
        public string Contact { get; set; }
        public string Fax_No { get; set; }
        public string E_Mail { get; set; }
        public string Customer_No { get; set; }
        // Endereços de envio ???
        [NotMapped]
        public bool Selected { get; set; }
    }
    
}
