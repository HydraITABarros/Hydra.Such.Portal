using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Hydra.Such.Data.Evolution.Database
{
    //[JsonSchemaProcessor(typeof(ReactSchemaProcessor))]
    [ModelMetadataType(typeof(IOrdemManutencao))]
    public partial class OrdemManutencao
    { }

    public interface IOrdemManutencao
    {
        [JsonProperty(PropertyName = "IdOm")]
        int IdOm { get; set; }

        int NumOm { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        BuildingZone IdTipoObra { get; set; }

        int IdEstadoObra { get; set; }

        int IdOrigemAvaria { get; set; }

        int IdTipoContacto { get; set; }

        int RegistadoPor { get; set; }

        [JsonSchema(JsonObjectType.String, Format = "date-time")]
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

        ICollection<IOrdemManutencaoEquipamentos> OrdemManutencaoEquipamentos { get; set; }

        ICollection<IOrdemManutencaoMateriais> OrdemManutencaoMateriais { get; set; }
    }

    public enum BuildingZone
    {
        Residential,
        Commercial,
        Industrial,
        Novo
    }
}
