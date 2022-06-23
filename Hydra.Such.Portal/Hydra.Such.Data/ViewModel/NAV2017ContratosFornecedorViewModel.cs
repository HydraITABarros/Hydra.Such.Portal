using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class NAV2017ContratosFornecedorViewModel
    {
        public string NoContrato { get; set; }
        public string NameContrato { get; set; }
        public string NoFornecedor { get; set; }
        public string NameFornecedor { get; set; }
        public DateTime? DataCelebracao { get; set; }
        public string DataCelebracaoTexto { get; set; }
        public DateTime? DataConclusaoInicial { get; set; }
        public string DataConclusaoInicialTexto { get; set; }
        public DateTime? DataConclusaoRevista { get; set; }
        public string DataConclusaoRevistaTexto { get; set; }
        public decimal? PrecoBase { get; set; }
        public decimal? PrecoContratual { get; set; }
    }
}
