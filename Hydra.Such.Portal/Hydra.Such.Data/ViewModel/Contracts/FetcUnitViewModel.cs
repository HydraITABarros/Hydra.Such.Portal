using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Contracts
{
    public class FetcUnitViewModel : ErrorHandler
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public string CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateDate { get; set; }
        public string UpdateUser { get; set; }


    }
}
