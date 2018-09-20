using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Contracts
{
    public class FetcUnitViewModel : ErrorHandler
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Email3 { get; set; }
        public string EmailRegiao12 { get; set; }
        public string EmailRegiao23 { get; set; }
        public string EmailRegiao33 { get; set; }
        public string EmailRegiao43 { get; set; }
        public string CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateDate { get; set; }
        public string UpdateUser { get; set; }


    }
}
