using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Approvals
{
    public class ApprovalMovementsViewModel
    {

        public int MovementNo { get; set; }
        public int? Type { get; set; }
        public string TypeText { get; set; }
        public int? Area { get; set; }
        public string AreaText { get; set; }
        public string Number { get; set; }
        public string RequestUser { get; set; }
        public decimal? Value { get; set; }
        public DateTime? DateTimeApprove { get; set; }
        public DateTime? DateTimeCreate { get; set; }
        public string UserCreate { get; set; }
        public DateTime? DateTimeUpdate { get; set; }
        public string UserUpdate { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public string ReproveReason { get; set; }
        public int Level { get; set; }
    }
}
