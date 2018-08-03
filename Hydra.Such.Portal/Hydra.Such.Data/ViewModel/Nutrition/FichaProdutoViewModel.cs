using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Nutrition
{
    public class FichaProdutoViewModel : ErrorHandler
    {
        public string No { get; set; }
        public string Descricao { get; set; }
        public bool? ListaDeMateriais { get; set; }
        public string UnidadeMedidaBase { get; set; }
        public string NoPrateleira { get; set; }
        public decimal? PrecoUnitario { get; set; }
        public decimal? CustoUnitario { get; set; }
        public decimal? Inventario { get; set; }
        public byte[] Imagem { get; set; }
        public decimal? ValorEnergetico { get; set; }
        public decimal? ValorEnergetico100g { get; set; }
        public decimal? Proteinas { get; set; }
        public decimal? Proteinas100g { get; set; }
        public decimal? Glicidos { get; set; }
        public decimal? Glicidos100g { get; set; }
        public decimal? Lipidos { get; set; }
        public decimal? Lipidos100g { get; set; }
        public decimal? FibraAlimentar { get; set; }
        public decimal? FibraAlimentar100g { get; set; }
        public decimal? QuantUnidadeMedida { get; set; }
        public decimal? GramasPorQuantUnidMedida { get; set; }
        public string TipoRefeicao { get; set; }
        public string DescricaoRefeicao { get; set; }
        public bool? Taras { get; set; }
        public decimal? AcidosGordosSaturados { get; set; }
        public decimal? Acucares { get; set; }
        public decimal? Sal { get; set; }
        public bool? Cereais { get; set; }
        public bool? Crustaceos { get; set; }
        public bool? Ovos { get; set; }
        public bool? Peixes { get; set; }
        public bool? Amendoins { get; set; }
        public bool? Soja { get; set; }
        public bool? Leite { get; set; }
        public bool? FrutasDeCascaRija { get; set; }
        public bool? Aipo { get; set; }
        public bool? Mostarda { get; set; }
        public bool? SementesDeSesamo { get; set; }
        public bool? DioxidoDeEnxofreESulfitos { get; set; }
        public bool? Tremoco { get; set; }
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
        public DateTime? DataHoraCriacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
    }
}
