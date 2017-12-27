using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Approvals
{
    public class ApprovalEmailViewModel
    {
        public int Id { get; set; }
        public int MovementNo { get; set; }
        public string ToEmail { get; set; }
        public string ToName { get; set; }
        public string Subject { get; set; }
        public DateTime SentDate { get; set; }
        public string EmailContent { get; set; }
        public bool Sent { get; set; }
        public string SendObs { get; set; }
    }
}
