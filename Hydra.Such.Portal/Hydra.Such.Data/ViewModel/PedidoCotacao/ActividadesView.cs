using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;

namespace Hydra.Such.Data.ViewModel.PedidoCotacao
{
    public class ActividadesView : ErrorHandler
    {
        public string CodActividade { get; set; }
        public string Descricao { get; set; }

        public ICollection<ActividadesPorFornecedorView> ActividadesPorFornecedor { get; set; }
        public ICollection<ActividadesPorProdutoView> ActividadesPorProduto { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }
    }
}
