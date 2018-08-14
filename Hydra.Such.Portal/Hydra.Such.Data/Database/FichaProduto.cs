using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class FichaProduto
    {
        public string Nº { get; set; }
        public string Descrição { get; set; }
        public bool? ListaDeMateriais { get; set; }
        public string UnidadeMedidaBase { get; set; }
        public string NºPrateleira { get; set; }
        public decimal? PreçoUnitário { get; set; }
        public decimal? CustoUnitário { get; set; }
        public decimal? Inventário { get; set; }
        public byte[] Imagem { get; set; }
        public decimal? ValorEnergético { get; set; }
        public decimal? ValorEnergético100g { get; set; }
        public decimal? Proteínas { get; set; }
        public decimal? Proteínas100g { get; set; }
        public decimal? Glícidos { get; set; }
        public decimal? Glícidos100g { get; set; }
        public decimal? Lípidos { get; set; }
        public decimal? Lípidos100g { get; set; }
        public decimal? FibraAlimentar { get; set; }
        public decimal? FibraAlimentar100g { get; set; }
        public decimal? QuantUnidadeMedida { get; set; }
        public decimal? GramasPorQuantUnidMedida { get; set; }
        public string TipoRefeição { get; set; }
        public string DescriçãoRefeição { get; set; }
        public bool? Taras { get; set; }
        public decimal? ÁcidosGordosSaturados { get; set; }
        public decimal? Açucares { get; set; }
        public decimal? Sal { get; set; }
        public bool? Cereais { get; set; }
        public bool? Crustáceos { get; set; }
        public bool? Ovos { get; set; }
        public bool? Peixes { get; set; }
        public bool? Amendoins { get; set; }
        public bool? Soja { get; set; }
        public bool? Leite { get; set; }
        public bool? FrutasDeCascaRija { get; set; }
        public bool? Aipo { get; set; }
        public bool? Mostarda { get; set; }
        public bool? SementesDeSésamo { get; set; }
        public bool? DióxidoDeEnxofreESulfitos { get; set; }
        public bool? Tremoço { get; set; }
        public bool? Moluscos { get; set; }
        public int? Tipo { get; set; }
        public decimal? VitaminaA { get; set; }
        public decimal? VitaminaD { get; set; }
        public decimal? Colesterol { get; set; }
        public decimal? Sodio { get; set; }
        public decimal? Potacio { get; set; }
        public decimal? Calcio { get; set; }
        public decimal? Ferro { get; set; }
        public decimal? Edivel { get; set; }
        public decimal? Alcool { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorCriação { get; set; }
        public string UtilizadorModificação { get; set; }

        public UnidadeMedida UnidadeMedidaBaseNavigation { get; set; }
    }
}
