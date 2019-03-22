using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using WSCustomerNAV;

namespace Hydra.Such.Data.ViewModel
{
    public class NAVClientsViewModel
    {
        public string No_ { get; set; }
        public string Name { get; set; }
        public string VATRegistrationNo_ { get; set; }
        public string Address { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string CountryRegionCode { get; set; }
        public bool UnderCompromiseLaw { get; set; }
        public bool National { get; set; }
        public bool InternalClient { get; set; }
        public string PaymentTermsCode { get; set; }
        public string PaymentMethodCode { get; set; }
        public string RegionCode { get; set; }
        public string FunctionalAreaCode { get; set; }
        public string ResponsabilityCenterCode { get; set; }
        public Regiao_Cliente RegiaoCliente { get; set; }
        public string PhoneNo { get; set; }
        public string EMail { get; set; }
        public string FaxNo { get; set; }
        public string HomePage { get; set; }
        public string County { get; set; }
        public bool ClienteAssociado { get; set; }
        public string AssociadoAN { get; set; }
        public Tipo_Cliente? TipoCliente { get; set; }
        public Natureza_Cliente? NaturezaCliente { get; set; }
        public string NoSeries { get; set; }
        public decimal TaxaAprovisionamento { get; set; }
        public bool AbrigoLeiCompromisso { get; set; }
        public bool ClienteNacional { get; set; }
        public bool ClienteInterno { get; set; }
        public string NoFornecedorAssoc { get; set; }
        public Blocked Blocked { get; set; }
    }
}
