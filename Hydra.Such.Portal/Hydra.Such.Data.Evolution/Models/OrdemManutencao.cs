using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    [ModelMetadataType(typeof(IOrdemManutencao))]
    public partial class OrdemManutencao : IOrdemManutencao
    { }

    public interface IOrdemManutencao
    {
        [JsonProperty(PropertyName = "IdOm")]
        int IdOm { get; set; }
        int NumOm { get; set; }
        int IdTipoObra { get; set; }
        int IdEstadoObra { get; set; }
        int IdOrigemAvaria { get; set; }
        int IdTipoContacto { get; set; }
        int RegistadoPor { get; set; }
        DateTime DataRegisto { get; set; }
        DateTime? DataEncerramento { get; set; }
        int Cliente { get; set; }
        int Instituicao { get; set; }
        int Servico { get; set; }
        string Contrato { get; set; }
        DateTime DataPedido { get; set; }
        int Ano { get; set; }
        int Semestre { get; set; }
        int Trimestre { get; set; }
        int Mes { get; set; }
        int Dia { get; set; }
        string NumReqCliente { get; set; }
        string Contacto { get; set; }
        TimeSpan? HoraAvaria { get; set; }
        string DescAvaria { get; set; }
        string Relatorio { get; set; }

        Cliente ClienteNavigation { get; set; }
        Contrato ContratoNavigation { get; set; }
        EstadoObra IdEstadoObraNavigation { get; set; }
        OrigemAvaria IdOrigemAvariaNavigation { get; set; }
        TipoContacto IdTipoContactoNavigation { get; set; }
        TipoObra IdTipoObraNavigation { get; set; }
        Instituicao InstituicaoNavigation { get; set; }
        Utilizador RegistadoPorNavigation { get; set; }
        Servico ServicoNavigation { get; set; }
        ICollection<OrdemManutencaoEquipamentos> OrdemManutencaoEquipamentos { get; set; }
        ICollection<OrdemManutencaoMateriais> OrdemManutencaoMateriais { get; set; }
    }
}
