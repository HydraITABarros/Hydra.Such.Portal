using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.FolhasDeHoras
{
    public class FolhaDeHoraDetailsViewModel : ErrorHandler
    {
        public string FolhaDeHorasNo { get; set; }
        public int? Area { get; set; }
        public string AreaText { get; set; }
        public string ProjectNo { get; set; }
        public string EmployeeNo { get; set; }
        public DateTime? DateDepartureTime { get; set; }
        public string DateDepartureTimeText { get; set; }
        public DateTime? DateTimeArrival { get; set; }
        public string DateTimeArrivalText { get; set; }
        public int? TypeDeslocation { get; set; }
        public string TypeDeslocationText { get; set; }
        public string CodeTypeKms { get; set; }
        public int? CodeTypeKmsInt { get; set; }
        public bool? DisplacementOutsideCity { get; set; }
        public int? DisplacementOutsideCityInt { get; set; }
        public string Validators { get; set; }
        public int? Status { get; set; }
        public string StatusText { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateTimeCreation { get; set; }
        public string DateTimeCreationText { get; set; }
        public DateTime? DateTimeLastState { get; set; }
        public string DateTimeLastStateText { get; set; }
        public string UserCreation { get; set; }
        public DateTime? DateTimeModification { get; set; }
        public string DateTimeModificationText { get; set; }
        public string UserModification { get; set; }
    }
}
