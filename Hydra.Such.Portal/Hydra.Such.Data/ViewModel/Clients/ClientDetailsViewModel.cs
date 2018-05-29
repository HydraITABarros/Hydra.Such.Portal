using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WSClientNAV;

namespace Hydra.Such.Data.ViewModel.Clients
{
    public class ClientDetailsViewModel : ErrorHandler
    {
        public string No { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Address_2 { get; set; }
        public string Post_Code { get; set; }

        public string City { get; set; }
        public string Phone_No { get; set; }

        public string E_Mail { get; set; }
        public string Fax_No { get; set; }

        public string Home_Page { get; set; }
        public string County { get; set; }

        public string VAT_Registration_No { get; set; }
        public bool Cliente_Associado { get; set; }

        public string Associado_A_N { get; set; }

        public Tipo_Cliente Tipo_Cliente { get; set; }

        public Natureza_Cliente Natureza_Cliente { get; set; }

        // Endereços de envio ???
    }

    /*public enum Tipo_Cliente
    {
        [Display(Name = " ")]
        NotSet = 0,
        [Display(Name = "A")]
        A = 1,
        [Display(Name = "B")]
        B = 2,
        [Display(Name = "C")]
        C = 3
    }
    public enum Natureza_Cliente
    {
        [Display(Name = " ")]
        NotSet = 0,
        [Display(Name = "Cliente_Estado_do_SNS")]
        Cliente_Estado_do_SNS = 1,
        [Display(Name = "Cliente_Estado_excepto_SNS")]
        Cliente_Estado_excepto_SNS = 2,
        [Display(Name = "Cliente_Externo_ao_Estado")]
        Cliente_Externo_ao_Estado = 3,
        [Display(Name = "Cliente_Interno")]
        Cliente_Interno = 4
    }*/
}
