using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Nutrition
{
    public class CoffeeShopMovimentsViewModel
    {
        public int MovimentNo { get; set; }
        public int? CoffeeShopCode { get; set; }
        public int? ProdutiveUnityNo { get; set; }
        public int? Type { get; set; }
        public string RegistryDate { get; set; }
        public string ResourceNo { get; set; }
        public string Description { get; set; }
        public decimal? Value { get; set; }
        public int? MovementType { get; set; }
        public string RegionCode { get; set; }
        public string FunctionalAreaCode { get; set; }
        public string ResponsabilityCenterCode { get; set; }
        public string User { get; set; }
        public string SystemCreateDateTime { get; set; }
        public string CreateDateTime { get; set; }
        public string CreateUser { get; set; }
        public string UpdateDateTime { get; set; }
        public string UpdateUser { get; set; }
    }
}
