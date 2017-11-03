using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.FolhasDeHoras
{
    public class PresencasFolhaDeHorasListItemViewModel
    {
        public string FolhaDeHorasNo { get; set; }
        public DateTime? Date { get; set; }
        public string DateText { get; set; }
        public string FirstHourEntry { get; set; }
        public string FirstHourDeparture { get; set; }
        public string SecondHourEntry { get; set; }
        public string SecondHourDeparture { get; set; }
        public DateTime? DateTimeCreation { get; set; }
        public string DateTimeCreationText { get; set; }
        public string UserCreation { get; set; }
        public DateTime? DateTimeModification { get; set; }
        public string DateTimeModificationText { get; set; }
        public string UserModification { get; set; }
    }
}
