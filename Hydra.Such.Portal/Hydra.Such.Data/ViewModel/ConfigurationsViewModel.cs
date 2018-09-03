using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class ConfigurationsViewModel
    {
        public int Id { get; set; }
        public int? ProjectNumeration { get; set; }
        public int? ContractNumeration { get; set; }
        public int? TimeSheetNumeration { get; set; }
        public int? OportunitiesNumeration { get; set; }
        public int? ProposalsNumeration { get; set; }
        public int? ContactsNumeration { get; set; }
        public int? PurchasingProceduresNumeration  { get; set; }
        public int? SimplifiedProceduresNumeration { get; set; }
        public int? PreRequisitionNumeration { get; set; }
        public int? RequisitionNumeration { get; set; }
        public int? DishesTechnicalSheetsNumeration { get; set; }
        public int? SimplifiedReqTemplatesNumeration { get; set; }
        public int? SimplifiedRequisitionNumeration { get; set; }
        public int? ProdutosNumeration { get; set; }
        public int? ConsultaMercadoNumeration { get; set; }
        public TimeSpan? LunchStartTime { get; set; }
        public TimeSpan? LunchEndTime { get; set; }
        public TimeSpan? DinnerStartTime { get; set; }
        public TimeSpan? DinnerEndTime { get; set; }
        public string WasteAreaId { get; set; }
    }
}
