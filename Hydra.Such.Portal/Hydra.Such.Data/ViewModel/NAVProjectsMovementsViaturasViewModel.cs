using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class NAVProjectsMovementsViaturasViewModel
    {
        public string Data { get; set; }
        public string Tipo { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public decimal? Quantidade { get; set; }
        public string CodigoUnidadeMedida { get; set; }
        public decimal? CustoUnitario { get; set; }
        public decimal? CustoTotal { get; set; }
        public string Regiao { get; set; }
        public string Area { get; set; }
        public string Cresp { get; set; }
        public string DocumentoNo { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }
    }
}
