using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class UserAccessesViewModel
    {
        public string IdUser { get; set; }
        public int? Area { get; set; }
        public int Feature { get; set; }
        public string FeatureNome { get; set; }
        public bool? Read { get; set; }
        public bool? Create { get; set; }
        public bool? Update { get; set; }
        public bool? Delete { get; set; }
        public bool? VerTudo { get; set; }
    }
}
