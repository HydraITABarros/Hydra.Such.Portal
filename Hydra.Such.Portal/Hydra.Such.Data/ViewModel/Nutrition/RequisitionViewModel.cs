using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Nutrition
{
    public class RequisitionViewModel
    {
        public string RequisitionNo { get; set; }
        public int? Area { get; set; }
        public int? State { get; set; }
        public string ProjectNo { get; set; }
        public string RegionCode { get; set; }
        public string FunctionalAreaCode { get; set; }
        public string CenterResponsibilityCode { get; set; }
        public string LocalCode { get; set; }
        public string EmployeeNo { get; set; }
        public string Vehicle { get; set; }
        public string ReceivedDate { get; set; }
        public bool? Urgent { get; set; }
        public bool? Sample { get; set; }
        public bool? Attachment { get; set; }
        public bool? Immobilized { get; set; }
        public bool? BuyCash { get; set; }
        public int? LocalCollectionCode { get; set; }
        public int? LocalDeliveryCode { get; set; }
        public string Comments { get; set; }
        public bool? RequestModel { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}
