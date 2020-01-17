using System;

namespace Hydra.Such.Data.Database
{
    public partial class Viaturas2
    {
        //public Viaturas2()
        //{
        //    DiárioMovimentosViaturas = new HashSet<DiárioMovimentosViaturas>();
        //    LinhasRequisição = new HashSet<LinhasRequisição>();
        //    MovimentosViaturas = new HashSet<MovimentosViaturas>();
        //    Requisição = new HashSet<Requisição>();
        //}


        public string Matricula { get; set; }
        public int? IDEstado { get; set; }
        public int? IDMarca { get; set; }
        public int? IDModelo { get; set; }
        public DateTime? Data1Matricula { get; set; }
        public string Cor { get; set; }
        public DateTime? DataMatricula { get; set; }
        public int? IDCategoria { get; set; }
        public int? IDTipo { get; set; }
        public string Classificacao { get; set; }
        public int? Cilindrada { get; set; }
        public int? IDCombustivel { get; set; }
        public decimal? ConsumoReferencia { get; set; }
        public int? CapacidadeDeposito { get; set; }
        public int? Autonomia { get; set; }
        public int? PesoBruto { get; set; }
        public int? CargaMaxima { get; set; }
        public int? Tara { get; set; }
        public int? Potencia { get; set; }
        public decimal? DistanciaEixos { get; set; }
        public int? NoLugares { get; set; }
        public int? NoAnosGarantia { get; set; }
        public string NoQuadro { get; set; }
        public int? IDTipoCaixa { get; set; }
        public string PneuFrente { get; set; }
        public string PneuRetaguarda { get; set; }
        public string Observacoes { get; set; }
        public string NomeImagem { get; set; }
        public DateTime? DataEstado { get; set; }
        public int? IDTipoPropriedade { get; set; }
        public int? IDPropriedade { get; set; }
        public int? IDSegmentacao { get; set; }
        public DateTime? DataProximaInspecao { get; set; }
        public int? IntervaloRevisoes { get; set; }
        public int? IDLocalParqueamento { get; set; }
        public bool? AlvaraLicenca { get; set; }
        public string CodRegiao { get; set; }
        public string CodAreaFuncional { get; set; }
        public string CodCentroResponsabilidade { get; set; }
        public string NoProjeto { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }


        //public ICollection<DiárioMovimentosViaturas> DiárioMovimentosViaturas { get; set; }
        //public ICollection<LinhasRequisição> LinhasRequisição { get; set; }
        //public ICollection<MovimentosViaturas> MovimentosViaturas { get; set; }
        //public ICollection<Requisição> Requisição { get; set; }
    }
}
