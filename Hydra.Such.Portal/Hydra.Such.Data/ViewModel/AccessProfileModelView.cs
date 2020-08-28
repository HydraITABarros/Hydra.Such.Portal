using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class AccessProfileModelView : ErrorHandler
    {
        public int IdProfile { get; set; }
        public int Area { get; set; }
        public int Feature { get; set; }
        public bool? Read { get; set; }
        public bool? Create { get; set; }
        public bool? Update { get; set; }
        public bool? Delete { get; set; }
        // zpgm. Esta propriedade não existe na BD
        //public bool? VerTudo { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
