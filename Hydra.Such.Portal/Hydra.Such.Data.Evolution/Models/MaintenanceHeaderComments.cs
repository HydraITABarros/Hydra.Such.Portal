using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    [ModelMetadataType(typeof(IMaintenanceHeaderComments))]
    public partial class MaintenanceHeaderComments : IMaintenanceHeaderComments
    {
    }

    public interface IMaintenanceHeaderComments
    {
         byte[] Timestamp { get; set; }
         int TableName { get; set; }
         string No { get; set; }
         int LineNo { get; set; }
         DateTime Date { get; set; }
         string Code { get; set; }
         string Comment { get; set; }
         int OrcAlternativo { get; set; }
    }
}
