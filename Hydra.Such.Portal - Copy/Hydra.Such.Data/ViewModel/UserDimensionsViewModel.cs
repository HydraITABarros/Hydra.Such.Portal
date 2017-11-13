using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class UserDimensionsViewModel : ErrorHandler
    {
        public string UserId { get; set; }
        public int Dimension { get; set; }
        public string DimensionValue { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }
    }
}
