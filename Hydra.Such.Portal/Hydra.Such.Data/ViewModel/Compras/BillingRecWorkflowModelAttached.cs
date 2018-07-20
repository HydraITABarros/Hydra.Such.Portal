using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Compras
{
    public class BillingRecWorkflowModelAttached
    {
        public int Id { get; set; }
        public int? IdWorkFlow { get; set; }       
        public string File { get; set; }
        public string Description { get; set; }
    }
}
