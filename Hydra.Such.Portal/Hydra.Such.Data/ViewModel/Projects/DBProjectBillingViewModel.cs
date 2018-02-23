using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Projects
{
    public class DBProjectBillingViewModel
    {
        public int ProductivityUnitNo { get; set; }
        public string ProjectNo { get; set; }
        public Boolean? Active { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }

        public string ClientNo { get; set; }
        public string ClientName { get; set; }
        public decimal TotalSales { get; set; }

        //EXTRAS
        public bool Selected { get; set; }
    }
}
