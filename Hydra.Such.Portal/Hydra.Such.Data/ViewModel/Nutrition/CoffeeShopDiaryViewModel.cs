using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Nutrition
{
    public class CoffeeShopDiaryViewModel
    {
        public int LineNo { get; set; }
        public int? CoffeShopCode { get; set; }
        public string RegistryDate { get; set; }
        public string ResourceNo { get; set; }
        public string Description { get; set; }
        public decimal? Value { get; set; }
        public string ProjectNo { get; set; }
        public string RegionCode { get; set; }
        public string FunctionalAreaCode { get; set; }
        public string ResponsabilityCenterCode { get; set; }
        public decimal? Quantity { get; set; }
        public int? ProdutiveUnityNo { get; set; }
        public int? MealType { get; set; }
        public int? MovementType { get; set; }
        public string User { get; set; }
        public string CreateDateTime { get; set; }
        public string UpdateDateTime { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }
        public string DescriptionTypeMeal { get; set; }
        
        public string DateToday { get; set; }
    }
}
