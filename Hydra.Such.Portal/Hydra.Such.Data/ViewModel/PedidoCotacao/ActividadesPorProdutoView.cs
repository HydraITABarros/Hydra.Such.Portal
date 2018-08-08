using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.PedidoCotacao
{
    public class ActividadesPorProdutoView : ErrorHandler
    {
        public int Id { get; set; }
        public string CodProduto { get; set; }
        public string CodActividade { get; set; }
    }
}
