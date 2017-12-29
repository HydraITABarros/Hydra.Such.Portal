using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Nutrition
{
    public class LocationViewModel : ErrorHandler
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string MobilePhone { get; set; }
        public string Fax { get; set; }
        public string Contact { get; set; }
        public string ZipCode { get; set; }
        public string Email { get; set; }
        public bool? Locked { get; set; }
        public string Region { get; set; }
        public string Area { get; set; }
        public string ResponsabilityCenter { get; set; }
        public string SupplierLocation { get; set; }
        public int? ShipLocationCode { get; set; }
        public string WarehouseManager { get; set; }
        public bool? WarehouseEnvironment { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }
}
