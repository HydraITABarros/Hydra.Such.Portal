using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Clients
{
    public class ListMovimentosAllClientsViewModel : ErrorHandler
    {
        public string CustomerNo { get; set; }
        public DateTime? Date { get; set; }
        public string DateTexto { get; set; }
        public DateTime? DueDate { get; set; }
        public string DueDateTexto { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string DimensionValue { get; set; }
        public Decimal? Value { get; set; }
        public string FactoringSemRecurso { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }
    }
}
