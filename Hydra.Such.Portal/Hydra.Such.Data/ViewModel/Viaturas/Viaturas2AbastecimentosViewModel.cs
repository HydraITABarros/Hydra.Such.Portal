using System;

namespace Hydra.Such.Data.ViewModel.Viaturas
{
    public class Viaturas2AbastecimentosViewModel : ErrorHandler
    {
        public int ID { get; set; }
        public string Matricula { get; set; }

        public DateTime? Data { get; set; }
        public string DataTexto { get; set; }
        public Decimal? Kms { get; set; }
        public Decimal? Litros { get; set; }
        public Decimal? PrecoUnitario { get; set; }
        public Decimal? PrecoTotal { get; set; }
        public int? IDCombustivel { get; set; }
        public string Combustivel { get; set; }
        public int? IDEmpresa { get; set; }
        public string Empresa { get; set; }
        public int? IDCartao { get; set; }
        public string Cartao { get; set; }
        public string Local { get; set; }
        public int? IDCondutor { get; set; }
        public string Condutor { get; set; }
        public string NoDocumento { get; set; }
        public string Observacoes { get; set; }

        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string DataCriacaoTexto { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        public string DataModificacaoTexto { get; set; }
    }
}
