using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Compras
{
    public class PreRequesitionLineHelperViewModel : ErrorHandler
    {
        public string PreRequisitionNo { get; set; }
        public List<PreRequisitionLineViewModel> Lines { get; set; }
    }
}
