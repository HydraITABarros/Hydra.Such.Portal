using System;

namespace Hydra.Such.Data.Database
{
    public partial class SISLOGProdutos
    {
        public string Codigo { get; set; }
        public decimal? Quantidade { get; set; }
        public decimal? QtdDisponivel { get; set; }
        public decimal? QtdPendente { get; set; }
        public decimal? QtdPendenteRececao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}
