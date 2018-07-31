using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class PriceServiceClientViewModel
    {
        public string Client { get; set; }
        public string CompleteName { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
        public string CodServClient { get; set; }
        public string ServiceDescription { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? PriceCost { get; set; }
        public string Date { get; set; }
        public string Resource { get; set; }
        public string ResourceDescription { get; set; }
        public string UnitMeasure { get; set; }
        public string TypeMeal { get; set; }
        public string TypeMealDescription { get; set; }
        public string RegionCode { get; set; }
        public string FunctionalAreaCode { get; set; }
        public string ResponsabilityCenterCode { get; set; }
        public string CreateDateTime { get; set; }
        public string UpdateDateTime { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }
        public string strSalePrice { get; set; }
        public string strPriceCost { get; set; }
        public bool Selected { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }
    }
}
