using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WSCustomerNAV;

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
        public WSCustomerNAV.Tipo_Cliente? Tipo_Cliente { get; set; }
        public string Tipo_Cliente_Text { get; set; }
        public WSCustomerNAV.Natureza_Cliente? Natureza_Cliente { get; set; }
        public string Natureza_Cliente_Text { get; set; }
        public string No_Series { get; set; }
        //[JsonConverter(typeof(StringEnumConverter))]
        public Regiao_Cliente Regiao_Cliente { get; set; }
        public string Regiao_Cliente_Text { get; set; }
        public decimal Taxa_Aprovisionamento { get; set; }
        public bool Abrigo_Lei_Compromisso { get; set; }
        public string Payment_Terms_Code { get; set; }
        public string Payment_Method_Code { get; set; }
        public string Global_Dimension_1_Code { get; set; }
        public string Centro_Res_Dimensao { get; set; }
        public bool Cliente_Nacional { get; set; }
        public bool Cliente_Interno { get; set; }
        public string No_Fornecedor_Assoc { get; set; }
        public string Country_Region_Code { get; set; }
        public string Centro_Resp_Dimensao { get; set; }
        public Blocked Blocked { get; set; }
        public string Blocked_Text { get; set; }
        // Endereços de envio ???

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }

    }

    public enum Tipo_Cliente
    {
        [Description(" ")]
        NotSet = 0,
        [Description("A")]
        A = 1,
        [Description("B")]
        B = 2,
        [Description("C")]
        C = 3
    }
    public enum Natureza_Cliente
    {
        [Description(" ")]
        NotSet = 0,
        [Description("Estado do SNS")]
        Cliente_Estado_do_SNS = 1,
        [Description("Estado excepto SNS")]
        Cliente_Estado_excepto_SNS = 2,
        [Description("Externo ao Estado")]
        Cliente_Externo_ao_Estado = 3,
        [Description("Interno")]
        Cliente_Interno = 4
    }


    public enum Blocked
    {
        [Description(" ")]
        _blank_,
        [Description("Envio")]
        Ship,
        [Description("Fatura")]
        Invoice,
        [Description("Tudo")]
        All,
    }
}
