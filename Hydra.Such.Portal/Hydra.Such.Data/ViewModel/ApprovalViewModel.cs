
using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class ApprovalViewModel
    {
        public int Id { get; set; }
        public int? Type { get; set; }
        public int? Area { get; set; }
        public int? LevelApproval { get; set; }
        public decimal? ValueApproval { get; set; }
        public string UserApproval { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }
        public int? GroupApproval { get; set; }
    }

}