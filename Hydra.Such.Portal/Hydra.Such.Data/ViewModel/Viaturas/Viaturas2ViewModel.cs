using System;

namespace Hydra.Such.Data.ViewModel.Viaturas
{
    public class Viaturas2ViewModel : ErrorHandler
    {
        public string Matricula { get; set; }

        public int? IDEstado { get; set; }
        public int? IDEstadoOriginalDB { get; set; }
        public string Estado { get; set; }
        public DateTime? DataEstado { get; set; }
        public DateTime? DataEstadoLast { get; set; }
        public int? IDMarca { get; set; }
        public string Marca { get; set; }
        public int? IDModelo { get; set; }
        public string Modelo { get; set; }
        public DateTime? Data1Matricula { get; set; }
        public string Data1MatriculaTexto { get; set; }
        public string Cor { get; set; }
        public DateTime? DataMatricula { get; set; }
        public string DataMatriculaTexto { get; set; }
        public int? IDCategoria { get; set; }
        public string Categoria { get; set; }
        public int? IDTipo { get; set; }
        public string Tipo { get; set; }
        public string Classificacao { get; set; }
        public int? Cilindrada { get; set; }
        public int? IDCombustivel { get; set; }
        public string Combustivel { get; set; }
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
        public string TipoCaixa { get; set; }
        public string PneuFrente { get; set; }
        public string PneuRetaguarda { get; set; }
        public string Observacoes { get; set; }
        public string NomeImagem { get; set; }
        public int? IDTipoPropriedade { get; set; }
        public string TipoPropriedade { get; set; }
        public int? IDPropriedade { get; set; }
        public int? IDPropriedadeOriginalDB { get; set; }
        public string Propriedade { get; set; }
        public DateTime? DataPropriedade { get; set; }
        public DateTime? DataPropriedadeLast { get; set; }
        public int? IDSegmentacao { get; set; }
        public string Segmentacao { get; set; }
        public DateTime? DataProximaInspecao { get; set; }
        public string DataProximaInspecaoTexto { get; set; }
        public int? IntervaloRevisoes { get; set; }
        public int? IDLocalParqueamento { get; set; }
        public int? IDLocalParqueamentoOriginalDB { get; set; }
        public DateTime? DataParqueamento { get; set; }
        public DateTime? DataParqueamentoLast { get; set; }
        public int? IDLocal { get; set; }
        public string LocalParqueamento { get; set; }
        public bool? AlvaraLicenca { get; set; }
        public string AlvaraLicencaTexto { get; set; }
        public string CodRegiao { get; set; }
        public string CodRegiaoOriginalDB { get; set; }
        public string CodAreaFuncional { get; set; }
        public string CodAreaFuncionalOriginalDB { get; set; }
        public string CodCentroResponsabilidade { get; set; }
        public string CodCentroResponsabilidadeOriginalDB { get; set; }
        public DateTime? DataDimensao { get; set; }
        public DateTime? DataDimensaoLast { get; set; }
        public string NoProjeto { get; set; }
        public string Projeto { get; set; }
        public DateTime? DataAquisicao { get; set; }
        public string DataAquisicaoTexto { get; set; }
        public int? IDGestor { get; set; }
        public string Gestor { get; set; }

        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string DataCriacaoTexto { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        public string DataModificacaoTexto { get; set; }

        public string Idade { get; set; }
        public string Condutor { get; set; }
        public string GarantiaSituacao { get; set; }
        public string SeguroSituacao { get; set; }
        public string DataFimSeguro { get; set; }
        public string UltimaInspecao { get; set; }
        public string IUCate { get; set; }
        public string Substituicao { get; set; }
        public string Afetacao { get; set; }
        public int AfetacaoIDAreaReal { get; set; }


        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }
    }
}
