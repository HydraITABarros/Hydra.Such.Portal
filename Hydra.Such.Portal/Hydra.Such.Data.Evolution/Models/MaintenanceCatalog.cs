using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    [ModelMetadataType(typeof(IMaintenanceCatalog))]
    public partial class MaintenanceCatalog
    {
    }

    public interface IMaintenanceCatalog
    {
        byte[] Timestamp { get; set; }
        int Type { get; set; }
        string Code { get; set; }
        string Description { get; set; }
        byte ManutCorrectiva { get; set; }
        byte ManutPreventiva { get; set; }
        int FaultReasonType { get; set; }
        byte TempoResposta { get; set; }
        byte TempoImobilização { get; set; }
        byte TempoEfectivoReparação { get; set; }
        byte TempoFechoObras { get; set; }
        byte TempoFacturação { get; set; }
        byte TempoOcupColaboradores { get; set; }
        byte ValorCustoVenda { get; set; }
        byte TaxaCumprimentoCats { get; set; }
        byte TaxaCoberturaCats { get; set; }
        byte TaxaCumprimentoRotinasMp { get; set; }
        byte IncidenciasAvarias { get; set; }
        byte OrdensEmCurso { get; set; }
    }
}
