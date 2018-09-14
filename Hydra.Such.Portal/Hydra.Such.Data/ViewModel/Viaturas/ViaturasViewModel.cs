using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Viaturas
{
    public class ViaturasViewModel : ErrorHandler
    {
        public string Matricula { get; set; }
        public string DataMatricula { get; set; }
        public string NQuadro { get; set; }
        public int? Estado { get; set; }
        public string EstadoDescricao { get; set; }
        public string CodigoTipoViatura { get; set; }
        public string TipoCombustivelDescricao { get; set; }
        public string CodigoMarca { get; set; }
        public string CodigoModelo { get; set; }
        public string CartaoCombustivel { get; set; }
        public string Apolice { get; set; }
        public int? PesoBruto { get; set; }
        public int? Tara { get; set; }
        public int? Cilindrada { get; set; }
        public int? TipoCombustivel { get; set; }
        public int? NLugares { get; set; }
        public string Cor { get; set; }
        public string AtribuidaA { get; set; }
        public string CodigoRegiao { get; set; }
        public string CodigoAreaFuncional { get; set; }
        public string CodigoCentroResponsabilidade { get; set; }
        public int? TipoPropriedade { get; set; }
        public string TipoPropriedadeDescricao { get; set; }
        public string NImobilizado { get; set; }
        public string DataHoraCriacao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public string DataHoraModificacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public string DataAquisicao { get; set; }
        public string DataEntradaFuncionamento { get; set; }
        public string DataAbate { get; set; }
        public string LocalParqueamento { get; set; }
        public string Observacoes { get; set; }
        public int? Potencia { get; set; }
        public int? DistanciaEntreEixos { get; set; }
        public string PneumaticosFrente { get; set; }
        public string PneumaticosRetaguarda { get; set; }
        public decimal? ConsumoIndicativo { get; set; }
        public string CartaVerde { get; set; }
        public string ValidadeCartaVerde { get; set; }
        public string NViaVerde { get; set; }
        public string DataUltimaInspecao { get; set; }
        public string ProximaInspecaoAte { get; set; }
        public decimal? ValorAquisicao { get; set; }
        public decimal? ValorVenda { get; set; }
        public string ValidadeApolice { get; set; }
        public string ValidadeCartaoCombustivel { get; set; }
        public byte[] Imagem { get; set; }
        public string DataUltimaRevisao { get; set; }
        public int? KmUltimaRevisao { get; set; }
        public int? IntervaloRevisoes { get; set; }
        public int? DuracaoPneus { get; set; }
        public string NoProjeto { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }

        public ModelosViewModel Modelo { get; set; }
        public MarcasViewModel Marca { get; set; }
        public TiposViaturaViewModel TipoViatura { get; set; }
    }
}
