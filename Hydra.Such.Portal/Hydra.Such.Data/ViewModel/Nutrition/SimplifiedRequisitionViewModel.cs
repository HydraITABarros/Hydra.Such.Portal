using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Nutrition
{
    public class SimplifiedRequisitionViewModel : ErrorHandler
    {
        public string RequisitionNo { get; set; }
        public int? Status { get; set; }
        public string RequisitionDate { get; set; }
        public string RequisitionTime { get; set; }
        public string RegistrationDate { get; set; }
        public string LocationCode { get; set; }
        public string RegionCode { get; set; }
        public string FunctionalAreaCode { get; set; }
        public string ResponsabilityCenterCode { get; set; }
        public int? MealType { get; set; }
        public string ProjectNo { get; set; }
        public string ApprovalDate { get; set; }
        public string ApprovalTime { get; set; }
        public string ShipDate { get; set; }
        public string ShipTime { get; set; }
        public string AvailabilityDate { get; set; }
        public string AvailabilityTime { get; set; }
        public string CreateResponsible { get; set; }
        public string ApprovalResponsible { get; set; }
        public string ShipResponsible { get; set; }
        public string ReceiptResponsible { get; set; }
        public bool? Print { get; set; }
        public bool? Atach { get; set; }
        public string EmployeeNo { get; set; }
        public bool? Urgent { get; set; }
        public int? ProductivityNo { get; set; }
        public string Observations { get; set; }
        public bool? Finished { get; set; }
        public string AimResponsible { get; set; }
        public string AimDate { get; set; }
        public string AimTime { get; set; }
        public bool? Authorized { get; set; }
        public string AuthorizedResponsible { get; set; }
        public string AuthorizedDate { get; set; }
        public string AuthorizedTime { get; set; }
        public string Visor { get; set; }
        public bool? ReceiptLinesDate { get; set; }
        public bool? NutritionRequisition { get; set; }
        public string ReceiptPreviewDate { get; set; }
        public bool? ModelRequisition { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }

        
    }
}
