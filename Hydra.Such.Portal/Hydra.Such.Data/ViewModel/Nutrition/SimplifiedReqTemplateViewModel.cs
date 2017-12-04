using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Nutrition
{
    public class SimplifiedReqTemplateViewModel : ErrorHandler
    {
        public string RequisitionTemplateId { get; set; }
        public string Description { get; set; }
        public string CodeRegion { get; set; }
        public string CodeFunctionalArea { get; set; }
        public string CodeResponsabilityCenter { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }

        public List<SimplifiedReqTemplateLinesViewModel> Lines { get; set; }
    }
}
