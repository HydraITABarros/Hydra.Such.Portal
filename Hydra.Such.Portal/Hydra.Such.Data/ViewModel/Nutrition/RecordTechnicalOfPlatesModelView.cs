using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Nutrition
{
    public class RecordTechnicalOfPlatesModelView : ErrorHandler
    {
        public string PlateNo { get; set; }
        public string Description { get; set; }
        public string UnitMeasureCode { get; set; }
        public int? State { get; set; }
        public string RecordTechnicalName { get; set; }
        public string LocalizationCode { get; set; }
        public int? PreparationTime { get; set; }
        public int? TechnicalCooking { get; set; }
        public string Group { get; set; }
        public string Epoch { get; set; }
        public int? DosesNo { get; set; }
        public int? PreparationTemperature { get; set; }
        public int? FinalTemperatureConfection { get; set; }
        public int? ServeTemperature { get; set; }
        public byte[] Image { get; set; }
        public int? VariationPriceCost { get; set; }
        public int? ClassFt1 { get; set; }
        public int? ClassFt2 { get; set; }
        public string ClassFt3 { get; set; }
        public string ClassFt4 { get; set; }
        public string ClassFt5 { get; set; }
        public string ClassFt6 { get; set; }
        public string ClassFt7 { get; set; }
        public string ClassFt8 { get; set; }
        public string CenterResponsibilityCode { get; set; }
        public string Observations { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }
        public bool? WithGluten { get; set; }
        public bool? BasedCrustaceans { get; set; }
        public bool? BasedEggs { get; set; }
        public bool? BasedFish { get; set; }
        public bool? BasedPeanuts { get; set; }
        public bool? BasedSoy { get; set; }
        public bool? BasedMilk { get; set; }
        public bool? BasedFruitShardShell { get; set; }
        public bool? BasedCelery { get; set; }
        public bool? BasedMustard { get; set; }
        public bool? BasedSesameSeeds { get; set; }
        public bool? BasedLupine { get; set; }
        public bool? BasedMolluscs { get; set; }
        public bool? BasedSulfurDioxeAndSulphites { get; set; }
    }
}
