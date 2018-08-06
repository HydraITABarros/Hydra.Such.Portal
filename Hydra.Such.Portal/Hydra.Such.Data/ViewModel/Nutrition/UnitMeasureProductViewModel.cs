using Hydra.Such.Data.ViewModel.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Nutrition
{
    public class UnitMeasureProductViewModel : ErrorHandler
    {

        public string ProductNo { get; set; }
        public string ProductNoText { get; set; }
        public string Code { get; set; }
        public string CodeText { get; set; }
        public decimal? QtdUnitMeasure { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Heigth { get; set; }
        public decimal? Cubage { get; set; }
        public decimal? Weight { get; set; }

        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }
}
