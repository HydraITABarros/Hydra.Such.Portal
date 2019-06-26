using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    [ModelMetadataType(typeof(IMaintenanceOrderAnexo))]
    public partial class MaintenanceOrderAnexo
    {
    }

    public interface IMaintenanceOrderAnexo
    {
        int AnexNo { get; set; }
        string MoNo { get; set; }
        int IdUser { get; set; }
        byte[] Ficheiro { get; set; }
        string Extensao { get; set; }
        string Nome { get; set; }
        DateTime? Data { get; set; }
    }
}
