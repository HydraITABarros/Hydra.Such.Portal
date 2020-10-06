using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.PBIGestiControl
{
    public class PBIGestiControl_GeralViewModel: ErrorHandler
    {
        public string ID { get; set; }
        public DateTime DataFecho { get; set; }
        public string DataFechoText { get; set; }
    }
}
