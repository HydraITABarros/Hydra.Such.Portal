using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class NAVFornecedoresViewModel
    {
        public string No { get; set; }
        public string Name { get; set; }
        public string FullAddress { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string HomePage { get; set; }
        public string VATRegistrationNo { get; set; }
        public string PaymentTermsCode { get; set; }
        public string PaymentMethodCode { get; set; }
        public string NoClienteAssociado { get; set; }
        public int? Blocked { get; set; }
        public string BlockedText { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string Distrito { get; set; }
        public int? Criticidade { get; set; }
        public string CriticidadeText { get; set; }
        public string Observacoes { get; set; }
    }
}
