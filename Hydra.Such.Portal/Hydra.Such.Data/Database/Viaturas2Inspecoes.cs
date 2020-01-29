using System;

namespace Hydra.Such.Data.Database
{
    public partial class Viaturas2Inspecoes
    {
        public int ID { get; set; }
        public string Matricula { get; set; }
        public DateTime? DataInspecao { get; set; }
        public Decimal? KmInspecao { get; set; }
        public int? IDResultado { get; set; }
        public DateTime? ProximaInspecao { get; set; }
        public string Observacao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }
    }
}
