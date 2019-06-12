using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    [ModelMetadataType(typeof(IMaintenanceOrderClienteIteracao))]
    public partial class MaintenanceOrderClienteIteracao
    {
    }

    public interface IMaintenanceOrderClienteIteracao
    {
        int IdClienteIteracao { get; set; }
        string NumOm { get; set; }
        int IdUser { get; set; }
        string TipoContactoCliente { get; set; }
        string NumDocumento { get; set; }
        string NumCompromisso { get; set; }
        int? NumAnexo { get; set; }
        string Observacao { get; set; }
    }
}
