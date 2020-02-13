using System;

namespace Hydra.Such.Data.Database
{
    public partial class Viaturas2Abastecimentos
    {
        public int ID { get; set; }
        public string Matricula { get; set; }

        public DateTime? Data { get; set; }
        public Decimal? Kms { get; set; }
        public Decimal? Litros { get; set; }
        public Decimal? PrecoUnitario { get; set; }
        public Decimal? PrecoTotal { get; set; }
        public int? IDCombustivel { get; set; }
        public int? IDEmpresa { get; set; }
        public int? IDCartao { get; set; }
        public string Local { get; set; }
        public int? IDCondutor { get; set; }
        public string NoDocumento { get; set; }
        public string Observacoes { get; set; }

        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }
    }
}
