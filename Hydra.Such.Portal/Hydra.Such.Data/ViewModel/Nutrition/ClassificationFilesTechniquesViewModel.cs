using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Nutrition
{
    public class ClassificationFilesTechniquesViewModel : ErrorHandler
    {
        public int Code { get; set; }
        public int? Type { get; set; }
        public int? Group { get; set; }
        public string Description { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
