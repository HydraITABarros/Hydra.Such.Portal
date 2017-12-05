using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Compras
{
    public class RequesitionTypeViewModel
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public bool? Fleet { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public string UpdateUser { get; set; }
    }
}
