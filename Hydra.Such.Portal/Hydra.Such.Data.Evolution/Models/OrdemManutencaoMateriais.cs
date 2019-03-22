using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    [ModelMetadataType(typeof(IOrdemManutencaoMateriais))]
    public partial class OrdemManutencaoMateriais : IOrdemManutencaoMateriais
    { }

    public interface IOrdemManutencaoMateriais
    {
        int IdOmMaterias { get; set; }
        int IdOm { get; set; }
        int IdMaterial { get; set; }
        string ReferenciaMaterial { get; set; }
        int IdFornecedor { get; set; }
        int QtdMaterial { get; set; }
        decimal PrecoUnitario { get; set; }
        decimal PrecoTotal { get; set; }
        int IdEstadoMaterial { get; set; }
        DateTime? DataAplicacao { get; set; }
        int? Ano { get; set; }
        int? Semestre { get; set; }
        int? Trimestre { get; set; }
        int? Mes { get; set; }
        int? Dia { get; set; }
    }
}
