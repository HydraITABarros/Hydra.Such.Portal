using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Clients
{
    public class ClientExtractViewModel : ErrorHandler
    {
        public string Customer_No { get; set; }
        public DateTime Date { get; set; }
        public DateTime Due_Date { get; set; }
        public string Document_Type { get; set; }
        public string Document_No { get; set; }
        public string Global_Dimension_2_Code { get; set; }
        public Decimal Value { get; set; }
        public string Factoring_Sem_Recurso { get; set; }
    }
    
    public enum Document_Type
    {
        [Description(" ")]
        NotSet = 0,
        [Description("Nota Crédito")]
        Nota_Credito = 1,
        [Description("Fatura")]
        Fatura = 2
    }
}
