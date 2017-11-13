using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.ProjectView
{
    public class ContabGroupTypesOMProjectViewModel
    {
        public int Code { get; set; }
        public int? Type { get; set; }
        public string Description { get; set; }
        public bool? CorrectiveMaintenance { get; set; }
        public bool? PreventiveMaintenance { get; set; }
        public int? FailType { get; set; }
        public bool? ResponseTimeIndicator { get; set; }
        public bool? StopTimeIndicator { get; set; }
        public bool? RepairEffectiveTimeIndicator { get; set; }
        public bool? ClosingWorksTimeIndicator { get; set; }
        public bool? BillingTimeIndicator { get; set; }
        public bool? EmployeesOccupationTimeIndicator { get; set; }
        public bool? CostSaleValueIndicator { get; set; }
        public bool? CATComplianceRateIndicator { get; set; }
        public bool? CATCoverageRateIndicator { get; set; }
        public bool? MPRoutineFulfillmentRateIndicator { get; set; }
        public bool? BreakoutIncidentsIndicator { get; set; }
        public bool? OrdernInProgressIndicator { get; set; }

    }
}
