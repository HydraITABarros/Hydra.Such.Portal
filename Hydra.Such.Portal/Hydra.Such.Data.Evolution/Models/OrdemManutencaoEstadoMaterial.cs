using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    [ModelMetadataType(typeof(IOrdemManutencaoEstadoMaterial))]
    public partial class OrdemManutencaoEstadoMaterial
    { }

    public interface IOrdemManutencaoEstadoMaterial
    {
        int IdEstadoMaterial { get; set; }
        string Nome { get; set; }

        ICollection<OrdemManutencaoMateriais> OrdemManutencaoMateriais { get; set; }
    }
}
