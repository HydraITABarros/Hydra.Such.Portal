using Hydra.Such.Data.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class NAVClientesInvoicesDetailsViewModel
    {
        public string DocumentNo { get; set; }
        public string LineNo { get; set; }
        public string SellToCustomerNo { get; set; }
        public string Tipo { get; set; }
        public string No { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public string UnitOfMeasure { get; set; }
        public string Quantity { get; set; }
        public string UnitPrice { get; set; }
        public string VAT { get; set; }
        public string Amount { get; set; }
        public string AmountIncludingVAT { get; set; }
        public string DimensionSetID { get; set; }
        public string ServiceContractNo { get; set; }
        public string RegionId { get; set; }
        public string FunctionalAreaId { get; set; }
        public string RespCenterId { get; set; }
    }
}
