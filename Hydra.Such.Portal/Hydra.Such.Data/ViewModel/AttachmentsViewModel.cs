using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class AttachmentsViewModel : ErrorHandler
    {
        public int DocType { get; set; }
        public string DocNumber { get; set; }
        public int DocLineNo { get; set; }
        public string Url{ get; set; }
        public string CreateDateTime { get; set; }
        public string UpdateDateTime { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }
    }
}
