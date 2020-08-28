using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Clients
{
    public class ListDividaAllClientsViewModel : ErrorHandler
    {
        public string CustomerRegionNo { get; set; }
        public string CustomerRegionName { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public decimal? Value { get; set; }
        public decimal? DueValue { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }
    }
}
