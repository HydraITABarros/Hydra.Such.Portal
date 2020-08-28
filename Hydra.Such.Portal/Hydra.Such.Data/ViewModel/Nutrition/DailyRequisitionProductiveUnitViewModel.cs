using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Nutrition
{
   public class DailyRequisitionProductiveUnitViewModel : ErrorHandler
    {
        public string id { get; set; }
        public int LineNo { get; set; }
        public int? ProductionUnitNo  { get; set; }
        public string ProductNo { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public string UnitMeasureCode { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? QuantidadeDisponivel { get; set; }
        public decimal? QuantidadeReservada { get; set; }
        public decimal?DirectUnitCost { get; set; }
        public decimal? TotalValue { get; set; }
        public string ProjectNo { get; set; }
        public string SupplierNo { get; set; }
        public string SubSupplierNo { get; set; }
        public int? MealType { get; set; }
        public bool? TableSupplierPrice { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }
        public string ExpectedReceptionDate { get; set; }
        public string DateByPriceSupplier { get; set; }
        public string LocalCode { get; set; }
        public decimal? QuantitybyUnitMeasure { get; set; }
        public string SupplierProductCode { get; set; }
        public string SupplierProductDescription { get; set; }
        public string SupplierName { get; set; }
        public string SubSupplierName { get; set; }
        public string OpenOrderNo { get; set; }
        public string OrderLineOpenNo { get; set; }
        public string ProductUnitDescription { get; set; }
        public string DocumentNo { get; set; }
        public string Observation { get; set; }
        public string GrupoRegistoIvaProduto { get; set; }
        public int? Tipo { get; set; }
        public int? Interface { get; set; }
        public decimal? DirectUnitCostSubSupplier { get; set; }

    }
}
