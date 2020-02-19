using System;

namespace Hydra.Such.Data.Database
{
    public partial class Viaturas2RentingContrato
    {
        public int ID { get; set; }
        public string Matricula { get; set; }

        public int? IDFornecedor { get; set; }
        public string NoContrato { get; set; }
        public Decimal? KmInicio { get; set; }
        public Decimal? KmContratados { get; set; }
        public Decimal? PrecoKmAdicionalSemIVA { get; set; }
        public Decimal? PrecoKmNaoPercorridoSemIVA { get; set; }
        public Decimal? KmMaximo { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataTermo { get; set; }
        public int? DuracaoContratoMensal { get; set; }
        public DateTime? InicioPagamento { get; set; }
        public int? NoPagamentos { get; set; }
        public int? IDPeriodicidade { get; set; }
        public Decimal? TotalSemIVA { get; set; }
        public Decimal? IVA { get; set; }
        public Decimal? TotalComIVA { get; set; }
        public Decimal? FranquiaSemIVA { get; set; }
        public string ResponsavelContrato { get; set; }
        public string Observacoes { get; set; }

        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }
    }
}
