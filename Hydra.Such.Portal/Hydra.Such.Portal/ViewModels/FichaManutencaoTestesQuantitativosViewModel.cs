using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NJsonSchema;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Hydra.Such.Portal.ViewModels
{

    public class FichaManutencaoTestesQuantitativosViewModel
    {
        public int IdTestesQuantitativos { get; set; }
        public string Descricao { get; set; }
        public string UnidadeCampo1 { get; set; }
        public string Resultado { get; set; }
        public string Observacoes { get; set; }
        [JsonIgnore]
        public string Codigo { get; set; }
        [JsonIgnore]
        public string Versao { get; set; }
    }
}
