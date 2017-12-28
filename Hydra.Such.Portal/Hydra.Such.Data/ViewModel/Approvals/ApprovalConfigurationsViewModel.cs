using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Approvals
{
    public class ApprovalConfigurationsViewModel
    {
        public int Id { get; set; }
        public int? Type { get; set; }
        public int? Area { get; set; }
        public int? Level { get; set; }
        public decimal? ApprovalValue { get; set; }
        public string ApprovalUser { get; set; }
        public int? ApprovalGroup { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
