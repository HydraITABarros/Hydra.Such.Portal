using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    [ModelMetadataType(typeof(IOrdemManutencaoLinhaMateriais))]
    public partial class OrdemManutencaoLinhaMateriais
    { }

    public interface IOrdemManutencaoLinhaMateriais
    {
        int IdOmLinhaMateriais { get; set; }
        int IdOmLinha { get; set; }
        string No { get; set; }
        int? IdMaterial { get; set; }
        string DescMaterial { get; set; }
        int? QtdMaterial { get; set; }
        DateTime? DataCriacao { get; set; }
        int? UtilizadorCriacao { get; set; }
        DateTime? HoraInicio { get; set; }
        DateTime? HoraFim { get; set; }
    }
}
