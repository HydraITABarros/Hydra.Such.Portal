using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Nutrition
{
    public class SimplifiedReqTemplateLinesViewModel : ErrorHandler
    {
        public string RequisitionTemplateId { get; set; }
        public int RequisitionTemplateLineId { get; set; }
        
        public int Type { get; set; }

        /// <summary>
        /// Map to código
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Map to Descrição
        /// </summary>
        public string ProductDescription { get; set; }

        /// <summary>
        /// Map to CódLocalização
        /// </summary>
        public string SupplierId { get; set; }

        public int Status { get; set; }
        public string UnitOfMeasure { get; set; }

        /// <summary>
        /// Map to QuantidadeARequerer
        /// </summary>
        public decimal Quantity { get; set; }
        public decimal QuantityApproved { get; set; }
        public decimal QuantityReceived { get; set; }
        public decimal QuantityToApprove { get; set; }
        public decimal TotalCost { get; set; }
        public string ProjectId { get; set; }
        public int MealType { get; set; }
        public string EmployeeId { get; set; }
        public decimal UnitCost { get; set; }

        public string CodeRegion { get; set; }
        public string CodeFunctionalArea { get; set; }
        public string CodeResponsabilityCenter { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }
    }
}
