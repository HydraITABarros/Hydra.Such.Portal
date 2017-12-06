using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class PlacesViewModel
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public string Postalcode { get; set; }
        public string Address { get; set; }
        public string Responsiblerecept { get; set; }
        public string Locality { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }
    }
}

