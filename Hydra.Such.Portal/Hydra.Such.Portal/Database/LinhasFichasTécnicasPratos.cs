using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
{
    public partial class LinhasFichasTécnicasPratos
    {
        public string NºPrato { get; set; }
        public int NºLinha { get; set; }
        public int? Tipo { get; set; }
        public string Código { get; set; }
        public string Descrição { get; set; }
        public decimal? Quantidade { get; set; }
        public string CódUnidadeMedida { get; set; }
        public decimal? QuantidadeDeProdução { get; set; }
        public decimal? ValorEnergético { get; set; }
        public decimal? Proteínas { get; set; }
        public decimal? HidratosDeCarbono { get; set; }
        public decimal? Lípidos { get; set; }
        public decimal? Fibras { get; set; }
        public decimal? PreçoCustoEsperado { get; set; }
        public decimal? PreçoCustoAtual { get; set; }
        public decimal? TpreçoCustoEsperado { get; set; }
        public decimal? TpreçoCustoAtual { get; set; }
        public string CódLocalização { get; set; }
        public decimal? ProteínasPorQuantidade { get; set; }
        public decimal? GlícidosPorQuantidade { get; set; }
        public decimal? LípidosPorQuantidade { get; set; }
        public decimal? FibasPorQuantidade { get; set; }
        public decimal? ValorEnergético2 { get; set; }
        public decimal? VitaminaA { get; set; }
        public decimal? VitaminaD { get; set; }
        public decimal? Colesterol { get; set; }
        public decimal? Sódio { get; set; }
        public decimal? Potássio { get; set; }
        public decimal? Cálcio { get; set; }
        public decimal? Ferro { get; set; }
        public decimal? Edivel { get; set; }
        public decimal? VitaminaAPorQuantidade { get; set; }
        public decimal? VitaminaDPorQuantidade { get; set; }
        public decimal? ColesterolPorQuantidade { get; set; }
        public decimal? SódioPorQuantidade { get; set; }
        public decimal? PotássioPorQuantidade { get; set; }
        public decimal? FerroPorQuantidade { get; set; }
        public decimal? CálcioPorQuantidade { get; set; }
        public decimal? ÁcidosGordosSaturados { get; set; }
        public decimal? Açucares { get; set; }
        public decimal? Sal { get; set; }
        public decimal? QuantidadePrato { get; set; }
        public string Preparação { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }

        public FichasTécnicasPratos NºPratoNavigation { get; set; }
    }
}
