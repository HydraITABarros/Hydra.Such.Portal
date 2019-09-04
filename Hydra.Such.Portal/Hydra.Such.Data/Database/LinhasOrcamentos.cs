using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.Database
{
    public partial class LinhasOrcamentos
    {
        public int NoLinha { get; set; }
        public string OrcamentosNo { get; set; }
        public int? Ordem { get; set; }
        public string Descricao { get; set; }
        public decimal? Quantidade { get; set; }
        public decimal? ValorUnitario { get; set; }
        public decimal? TaxaIVA { get; set; }
        public decimal? TotalLinha { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        public string UtilizadorModificacao { get; set; }
    }
}
