using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class DistribuiçãoCustoFolhaDeHoras
    {
        public string NºFolhasDeHoras { get; set; }
        public int NºLinhaPercursosEAjudasCustoDespesas { get; set; }
        public int NºLinha { get; set; }
        public int? TipoObra { get; set; }
        public string NºObra { get; set; }
        public decimal? Valor { get; set; }
        public decimal? Valor1 { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }

        public FolhasDeHoras NºFolhasDeHorasNavigation { get; set; }
        public Projetos NºObraNavigation { get; set; }
    }
}
