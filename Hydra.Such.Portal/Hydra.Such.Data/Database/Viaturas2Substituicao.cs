using System;

namespace Hydra.Such.Data.Database
{
    public partial class Viaturas2Substituicao
    {
        public int ID { get; set; }
        public string Matricula { get; set; }

        public string MatriculaSubstituicao { get; set; }
        public DateTime? DataInicio { get; set; }
        public Decimal? KmInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public Decimal? KmFim { get; set; }
        public string Observacoes { get; set; }

        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }
    }
}
