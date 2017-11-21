using System;
using System.Collections.Generic;

namespace Hydra.Such.Tester.Database
{
    public partial class Viaturas
    {
        public Viaturas()
        {
            DiárioMovimentosViaturas = new HashSet<DiárioMovimentosViaturas>();
            LinhasRequisição = new HashSet<LinhasRequisição>();
            MovimentosViaturas = new HashSet<MovimentosViaturas>();
            Requisição = new HashSet<Requisição>();
        }

        public string Matrícula { get; set; }
        public DateTime? DataMatrícula { get; set; }
        public string NºQuadro { get; set; }
        public int? Estado { get; set; }
        public int? CódigoTipoViatura { get; set; }
        public int? CódigoMarca { get; set; }
        public int? CódigoModelo { get; set; }
        public string CartãoCombustível { get; set; }
        public string Apólice { get; set; }
        public int? PesoBruto { get; set; }
        public int? Tara { get; set; }
        public int? Cilindrada { get; set; }
        public int? TipoCombustível { get; set; }
        public int? NºLugares { get; set; }
        public string Cor { get; set; }
        public string AtribuídaA { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public int? TipoPropriedade { get; set; }
        public string NºImobilizado { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }
        public DateTime? DataAquisição { get; set; }
        public DateTime? DataEntradaFuncionamento { get; set; }
        public DateTime? DataAbate { get; set; }
        public string LocalParqueamento { get; set; }
        public string Observações { get; set; }
        public int? Potência { get; set; }
        public int? DistânciaEntreEixos { get; set; }
        public string PneumáticosFrente { get; set; }
        public string PneumáticosRetaguarda { get; set; }
        public decimal? ConsumoIndicativo { get; set; }
        public string CartaVerde { get; set; }
        public DateTime? ValidadeCartaVerde { get; set; }
        public string NºViaVerde { get; set; }
        public DateTime? DataUltimaInspeção { get; set; }
        public DateTime? ProximaInspeçãoAté { get; set; }
        public decimal? ValorAquisição { get; set; }
        public decimal? ValorVenda { get; set; }
        public DateTime? ValidadeApólice { get; set; }
        public DateTime? ValidadeCartãoCombustivel { get; set; }
        public byte[] Imagem { get; set; }
        public DateTime? DataUltimaRevisão { get; set; }
        public int? KmUltimaRevisão { get; set; }
        public int? IntervaloRevisões { get; set; }
        public int? DuraçãoPneus { get; set; }

        public Modelos CódigoM { get; set; }
        public Marcas CódigoMarcaNavigation { get; set; }
        public TiposViatura CódigoTipoViaturaNavigation { get; set; }
        public ICollection<DiárioMovimentosViaturas> DiárioMovimentosViaturas { get; set; }
        public ICollection<LinhasRequisição> LinhasRequisição { get; set; }
        public ICollection<MovimentosViaturas> MovimentosViaturas { get; set; }
        public ICollection<Requisição> Requisição { get; set; }
    }
}
