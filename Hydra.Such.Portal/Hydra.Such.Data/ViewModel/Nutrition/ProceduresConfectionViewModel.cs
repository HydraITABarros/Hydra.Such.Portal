using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Nutrition
{
    public class ProceduresConfectionViewModel
    {
        public string TechnicalSheetNo { get; set; }
        public int actionNo { get; set; }
        public string description { get; set; }
        public int? orderNo { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public string UpdateUser { get; set; }
    }
}
