using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Projects
{
    public class FaturasNotasViewModel
    {
        public string Type { get; set; }
        public string DocumentNo { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string DocumentDateTexto { get; set; }
        public Decimal? ValorSemIVA { get; set; }
        public Decimal ValorComIVA { get; set; }
        public string Parcial { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }
    }
}
