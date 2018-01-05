using Hydra.Such.Data.ViewModel.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Nutrition
{
    public class ProductMovementViewModel : ErrorHandler
    {
        public int MovementNo { get; set; }
        public string DateRegister { get; set; }
        public int? MovementType { get; set; }
        public int? DocumentNo { get; set; }
        public string ProductNo { get; set; }
        public string Description { get; set; }
        public string CodLocation { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? Val { get; set; }
        public int? ProjectNo { get; set; }
        public string CodeRegion { get; set; }
        public string CodeFunctionalArea { get; set; }
        public string CodeResponsabilityCenter { get; set; }
            
        
    }
}
