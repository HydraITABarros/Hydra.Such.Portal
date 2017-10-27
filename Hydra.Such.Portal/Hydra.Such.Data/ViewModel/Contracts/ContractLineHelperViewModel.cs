using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Contracts
{
    public class ContractLineHelperViewModel : ErrorHandler
    {
        public string ContractNo { get; set; }
        public int VersionNo { get; set; }
        public List<ContractLineViewModel> Lines { get; set; }
    }
}
