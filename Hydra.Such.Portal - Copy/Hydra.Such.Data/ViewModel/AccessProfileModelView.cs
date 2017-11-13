using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class AccessProfileModelView
    {
        public int IdProfile { get; set; }
        public int Area { get; set; }
        public int Feature { get; set; }
        public bool? Read { get; set; }
        public bool? Create { get; set; }
        public bool? Update { get; set; }
        public bool? Delete { get; set; }

    }
}
