using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.FolhasDeHoras
{
    public class PercursosEAjudasCustoDespesasFolhaDeHorasListItemViewModel
    {
        public string FolhaDeHorasNo { get; set; }
        public int? CostType { get; set; }
        public int? LineNo { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Destiny { get; set; }
        public DateTime? DateTravel { get; set; }
        public string DateTravelText { get; set; }
        public decimal Distance { get; set; }
        public decimal Amount { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal UnitPrice { get; set; }
        public string Justification { get; set; }
        public string Payroll { get; set; }
        public DateTime? DateTimeCreation { get; set; }
        public string DateTimeCreationText { get; set; }
        public string UserCreation { get; set; }
        public DateTime? DateTimeModification { get; set; }
        public string DateTimeModificationText { get; set; }
        public string UserModification { get; set; }
    }
}
