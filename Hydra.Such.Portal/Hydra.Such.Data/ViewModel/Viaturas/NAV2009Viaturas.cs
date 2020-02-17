using System;

namespace Hydra.Such.Data.ViewModel.Viaturas
{
    public class NAV2009Viaturas : ErrorHandler
    {
        public string Matricula { get; set; }
        public DateTime DataMatricula { get; set; }
        public string NoQuadro { get; set; }
        public int Estado { get; set; }
        public string PesoBruto { get; set; }
        public string Tara { get; set; }
        public string Cilindrada { get; set; }
        public string Potencia { get; set; }
        public int Combustivel { get; set; }
        public string NoLugares { get; set; }
        public string Cor { get; set; }
        public string DistanciaEntreEixos { get; set; }
        public string PneumaticosFrente { get; set; }
        public string PneumaticosRetaguarda { get; set; }
        public DateTime DataAquisicao { get; set; }
        public int TipoPropriedade { get; set; }
        public string GlobalDimension1Code { get; set; }
        public string GlobalDimension2Code { get; set; }
        public string ShortcutDimension3Code { get; set; }
        public string Observacoes { get; set; }
        public string Utilizador { get; set; }
        public DateTime DataAlteracao { get; set; }
        public string LocalParqueamento { get; set; }
        public int IntervaloRevisoes { get; set; }
        public Decimal ConsumoIndicativoViatura { get; set; }
    }
}
