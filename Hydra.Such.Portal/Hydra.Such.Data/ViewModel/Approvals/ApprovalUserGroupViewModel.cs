using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Approvals
{
    public class ApprovalUserGroupViewModel
    {
        public int ApprovalGroup { get; set; }
        public string ApprovalUser { get; set; }
        public bool? EnviarEmailAlerta { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }
}
