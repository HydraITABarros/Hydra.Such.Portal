﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class NAVClientsViewModel
    {
        public string No_ { get; set; }
        public string Name { get; set; }
        public string VATRegistrationNo_ { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Country_RegionCode { get; set; }
        public bool UnderCompromiseLaw { get; set; }
        public bool National { get; set; }
        public bool InternalClient { get; set; }
        public string PaymentTermsCode { get; set; }
        public string PaymentMethodCode { get; set; }
        public string RegionCode { get; set; }
        public string FunctionalAreaCode { get; set; }
        public string ResponsabilityCenterCode { get; set; }
        public Regiao_Cliente? RegiaoCliente { get; set; }
        
    }

    public enum Regiao_Cliente
    {
        [Description(" ")]
        NotSet = 0,
        [Description("Norte")]
        Norte = 1,
        [Description("Centro")]
        Centro = 2,
        [Description("Sul")]
        Sul = 3
    }
}
