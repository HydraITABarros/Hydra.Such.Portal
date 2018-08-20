using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;

namespace Hydra.Such.Data.ViewModel.PedidoCotacao
{
    public class ActividadesPorFornecedorView : ErrorHandler
    {
        public int Id { get; set; }
        public string CodFornecedor { get; set; }
        public string CodActividade { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }
    }
}
