using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Nutrition
{
    public class LinesRecordTechnicalOfPlatesViewModel
    {
        public string PlateNo { get; set; }
        public int LineNo { get; set; }
        public int? Type { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal? Quantity { get; set; }
        public string UnitMeasureCode { get; set; }
        public decimal? QuantityOfProduction { get; set; }
        public decimal? EnergeticValue { get; set; }
        public decimal? Proteins { get; set; }
        public decimal? HydratesOfCarbon { get; set; }
        public decimal? Lipids { get; set; }
        public decimal? Fibers { get; set; }
        public decimal? ExpectedCostPrice { get; set; }
        public decimal? CurrentCostPrice { get; set; }
        public decimal? TimeExpectedCostPrice { get; set; }
        public decimal? TimeCurrentCostPrice { get; set; }
        public string LocalizationCode { get; set; }
        public decimal? ProteinsByQuantity { get; set; }
        public decimal? GlicansByQuantity { get; set; }
        public decimal? LipidsByQuantity { get; set; }
        public decimal? FibersByQuantity { get; set; }
        public decimal? EnergeticValue2 { get; set; }
        public decimal? VitaminA { get; set; }
        public decimal? VitaminD { get; set; }
        public decimal? Cholesterol { get; set; }
        public decimal? Sodium { get; set; }
        public decimal? Potassium { get; set; }
        public decimal? Calcium { get; set; }
        public decimal? Iron { get; set; }
        public decimal? Edivel { get; set; }
        public decimal? VitaminAByQuantity { get; set; }
        public decimal? VitaminDByQuantity { get; set; }
        public decimal? CholesterolByQuantity { get; set; }
        public decimal? SodiumByQuantity { get; set; }
        public decimal? PotassiumByQuantity { get; set; }
        public decimal? IronByQuantity { get; set; }
        public decimal? CalciumByQuantity { get; set; }
        public decimal? SaturatedFattyAcids { get; set; }
        public decimal? Sugarcane { get; set; }
        public decimal? salt { get; set; }
        public decimal? QuantityPlates { get; set; }
        public string Preparation { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public string UpdateUser { get; set; }
    }
}
