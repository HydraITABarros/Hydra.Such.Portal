using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Projects
{
    public class MovementAuthorizedProjectViewModel
    {
        public int NoMovement { get; set; }
        public DateTime Date { get; set; }
        public int Type { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public string UnitCode { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string CodProject { get; set; }
        public string RegionCode { get; set; }
        public string FunctionalAreaCode { get; set; }
        public string ResponsabilityCenterCode { get; set; }
        public string CodContract { get; set; }
        public int CodServiceGroup { get; set; }
        public string CodServClient { get; set; }
        public string DescServClient { get; set; }
        public string NumGuideResiduesGar { get; set; }
        public string NumGuideExternal { get; set; }
        public DateTime? DateConsume { get; set; }
        public int TypeMeal { get; set; }
        public int TypeResourse { get; set; }
        public string NumDocument { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal? CostTotal { get; set; }
        public string CodClient { get; set; }
        public int InvoiceGroup { get; set; }
    }
}
