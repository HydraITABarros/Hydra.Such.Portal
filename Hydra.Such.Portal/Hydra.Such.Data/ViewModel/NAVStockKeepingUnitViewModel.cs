using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class NAVStockKeepingUnitViewModel
    {
      
        //public timestamp { get; set; }
        public string LocationCode { get; set; }
        public int ItemNo_ { get; set; }
        //public VariantCode { get; set; }
        //public ShelfNo_ { get; set; }
        public decimal UnitCost { get; set; }
        //public StandardCost { get; set; }
        //public LastDirectCost { get; set; }
        public String VendorNo_ { get; set; }
        public string VendorItemNo_ { get; set; }
        public DateTime? LeadTimeCalculation { get; set; }
        //public ReorderPoint { get; set; }
        //public MaximumInventory { get; set; }
        //public ReorderQuantity { get; set; }
        //public LastDateModified { get; set; }
        //public AssemblyPolicy { get; set; }
        //public Transfer_LevelCode { get; set; }
        //public LotSize { get; set; }
        //public DiscreteOrderQuantity { get; set; }
        //public MinimumOrderQuantity { get; set; }
        //public MaximumOrderQuantity { get; set; }
        public decimal? SafetyStockQuantity { get; set; }
        //public OrderMultiple { get; set; }
        public DateTime? SafetyLeadTime { get; set; }
        //public ComponentsatLocation { get; set; }
        //public FlushingMethod { get; set; }
        //public ReplenishmentSystem { get; set; }
        public DateTime? TimeBucket { get; set; }
        //public ReorderingPolicy { get; set; }
        //public IncludeInventory { get; set; }
        //public ManufacturingPolicy { get; set; }
        //public ReschedulingPeriod { get; set; }
        //public LotAccumulationPeriod { get; set; }
        //public DampenerPeriod { get; set; }
        //public DampenerQuantity { get; set; }
        //public OverflowLevel { get; set; }
        //public Transfer_fromCode { get; set; }
        //public SpecialEquipmentCode { get; set; }
        //public Put_awayTemplateCode { get; set; }
        //public Put_awayUnitofMeasureCode { get; set; }
        //public PhysInvtCountingPeriodCode { get; set; }
        //public LastCountingPeriodUpdate { get; set; }
        //public UseCross_Docking { get; set; }
        public DateTime? NextCountingStartDate { get; set; }
        public DateTime? NextCountingEndDate { get; set; }
    }
}
