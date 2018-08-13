using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Nutrition
{
    public class FichaProdutoViewModel : ErrorHandler
    {
        public string No { get; set; }
        public string Code { get; set; }
        public string Descricao { get; set; }
        public bool? ListaDeMateriais { get; set; }
        public string ListaDeMateriaisText { get; set; }
        public string UnidadeMedidaBase { get; set; }
        public string UnidadeMedidaBaseText { get; set; }
        public string NoPrateleira { get; set; }
        public decimal? PrecoUnitario { get; set; }
        public string PrecoUnitarioText { get; set; }
        public decimal? CustoUnitario { get; set; }
        public string CustoUnitarioText { get; set; }
        public decimal? Inventario { get; set; }
        public string InventarioText { get; set; }
        public byte[] Imagem { get; set; }
        public decimal? ValorEnergetico { get; set; }
        public string ValorEnergeticoText { get; set; }
        public decimal? ValorEnergetico100g { get; set; }
        public string ValorEnergetico100gText { get; set; }
        public decimal? Proteinas { get; set; }
        public string ProteinasText { get; set; }
        public decimal? Proteinas100g { get; set; }
        public string Proteinas100gText { get; set; }
        public decimal? Glicidos { get; set; }
        public string GlicidosText { get; set; }
        public decimal? Glicidos100g { get; set; }
        public string Glicidos100gText { get; set; }
        public decimal? Lipidos { get; set; }
        public string LipidosText { get; set; }
        public decimal? Lipidos100g { get; set; }
        public string Lipidos100gText { get; set; }
        public decimal? FibraAlimentar { get; set; }
        public string FibraAlimentarText { get; set; }
        public decimal? FibraAlimentar100g { get; set; }
        public string FibraAlimentar100gText { get; set; }
        public decimal? QuantUnidadeMedida { get; set; }
        public string QuantUnidadeMedidaText { get; set; }
        public decimal? GramasPorQuantUnidMedida { get; set; }
        public string GramasPorQuantUnidMedidaText { get; set; }
        public string TipoRefeicao { get; set; }
        public string DescricaoRefeicao { get; set; }
        public bool? Taras { get; set; }
        public string TarasText { get; set; }
        public decimal? AcidosGordosSaturados { get; set; }
        public string AcidosGordosSaturadosText { get; set; }
        public decimal? Acucares { get; set; }
        public string AcucaresText { get; set; }
        public decimal? Sal { get; set; }
        public string SalText { get; set; }
        public bool? Cereais { get; set; }
        public string CereaisText { get; set; }
        public bool? Crustaceos { get; set; }
        public string CrustaceosText { get; set; }
        public bool? Ovos { get; set; }
        public string OvosText { get; set; }
        public bool? Peixes { get; set; }
        public string PeixesText { get; set; }
        public bool? Amendoins { get; set; }
        public string AmendoinsText { get; set; }
        public bool? Soja { get; set; }
        public string SojaText { get; set; }
        public bool? Leite { get; set; }
        public string LeiteText { get; set; }
        public bool? FrutasDeCascaRija { get; set; }
        public string FrutasDeCascaRijaText { get; set; }
        public bool? Aipo { get; set; }
        public string AipoText { get; set; }
        public bool? Mostarda { get; set; }
        public string MostardaText { get; set; }
        public bool? SementesDeSesamo { get; set; }
        public string SementesDeSesamoText { get; set; }
        public bool? DioxidoDeEnxofreESulfitos { get; set; }
        public string DioxidoDeEnxofreESulfitosText { get; set; }
        public bool? Tremoco { get; set; }
        public string TremocoText { get; set; }
        public bool? Moluscos { get; set; }
        public string MoluscosText { get; set; }
        public int? Tipo { get; set; }
        public string TipoText { get; set; }
        public decimal? VitaminaA { get; set; }
        public string VitaminaAText { get; set; }
        public decimal? VitaminaD { get; set; }
        public string VitaminaDText { get; set; }
        public decimal? Colesterol { get; set; }
        public string ColesterolText { get; set; }
        public decimal? Sodio { get; set; }
        public string SodioText { get; set; }
        public decimal? Potacio { get; set; }
        public string PotacioText { get; set; }
        public decimal? Calcio { get; set; }
        public string CalcioText { get; set; }
        public decimal? Ferro { get; set; }
        public string FerroText { get; set; }
        public decimal? Edivel { get; set; }
        public string EdivelText { get; set; }
        public decimal? Alcool { get; set; }
        public string AlcoolText { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }


        public List<UnitMeasureProductViewModel> ListUnidadeMedidaProduto { get; set; }
    }
}
