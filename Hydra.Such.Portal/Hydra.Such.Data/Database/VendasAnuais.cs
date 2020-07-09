using System;

namespace Hydra.Such.Data.Database
{
    public partial class VendasAnuais
    {
        public int? Ano { get; set; }
        public string Regiao { get; set; }
        public string NoAssociado { get; set; }
        public string NomeAssociado { get; set; }
        public decimal? Jan { get; set; }
        public decimal? Fev { get; set; }
        public decimal? Mar { get; set; }
        public decimal? Abr { get; set; }
        public decimal? Mai { get; set; }
        public decimal? Jun { get; set; }
        public decimal? Jul { get; set; }
        public decimal? Ago { get; set; }
        public decimal? Set { get; set; }
        public decimal? Out { get; set; }
        public decimal? Nov { get; set; }
        public decimal? Dez { get; set; }
        public decimal? Total { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }
    }
}
