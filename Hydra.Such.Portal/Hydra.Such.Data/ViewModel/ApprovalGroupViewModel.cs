using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class ApprovalGroupViewModel
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public int Area { get; set; }
        public int LevelApproval { get; set; }
        public decimal ValueApproval { get; set; }
        public string UserApproval { get; set; }
        public int GroupApproval { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DataUpdate { get; set; }
        public string UserCreate { get; set; }
        public string UserUpdate { get; set; }
    }

}