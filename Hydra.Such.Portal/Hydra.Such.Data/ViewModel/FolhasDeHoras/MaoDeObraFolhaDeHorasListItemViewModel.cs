using System;
using System.Collections.Generic;
using System.Text;


namespace Hydra.Such.Data.ViewModel.FolhasDeHoras
{
    public class MaoDeObraFolhaDeHorasListItemViewModel
    {
        public string FolhaDeHorasNo { get; set; }
        public int? LineNo { get; set; }
        public DateTime? Date { get; set; }
        public string EmployedNo { get; set; }
        public string ProjectNo { get; set; }
        public int? WorkTypeCode { get; set; }
        public DateTime? StartTime { get; set; }
        public String StartTimeText { get; set; }
        public DateTime? EndTime { get; set; }
        public string EndTimeText { get; set; }
        public Boolean? LunchTime { get; set; }
        public Boolean? DinnerTime { get; set; }
        public string FamilyCodeResource { get; set; }
        public string ResourceNo { get; set; }
        public string UnitCodeMeasure { get; set; }
        public int? OMTypeCode { get; set; }
        public DateTime? HoursNo { get; set; }
        public string HoursNoText { get; set; }
        public decimal DirectUnitCost { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime? DateTimeCreation { get; set; }
        public string UserCreation { get; set; }
        public DateTime? DateTimeModification { get; set; }
        public string UserModification { get; set; }
    }
}
