using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    [ModelMetadataType(typeof(IOrdemManutencaoEquipamentos))]
    public partial class OrdemManutencaoEquipamentos : IOrdemManutencaoEquipamentos
    { }

    public interface IOrdemManutencaoEquipamentos
    {
        int IdOmEquipamento { get; set; }
        int IdOm { get; set; }
        int Cliente { get; set; }
        int Servico { get; set; }
        int IdEquipamento { get; set; }
        int? IdRotina { get; set; }
        int? IdEquipEstado { get; set; }
        int? TempoEntreAvarias { get; set; }

        Cliente ClienteNavigation { get; set; }
        EquipEstado IdEquipEstadoNavigation { get; set; }
        Equipamento IdEquipamentoNavigation { get; set; }
        OrdemManutencao IdOmNavigation { get; set; }
        Rotina IdRotinaNavigation { get; set; }
        Servico ServicoNavigation { get; set; }
    }
}
