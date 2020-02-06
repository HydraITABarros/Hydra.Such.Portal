using System;

namespace Hydra.Such.Data.Database
{
    public partial class Viaturas2Km
    {
        public int ID { get; set; }
        public string Matricula { get; set; }
        public Decimal? Km { get; set; }
        public DateTime? Data { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }
    }
}
