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

    public class FichaManutencaoRelatorioManutencaoViewModel
    {
        public int? IdManutencao { get; set; }
        public string Descricao { get; set; }
        public int? Resultado { get; set; }
        public string Observacoes { get; set; }
        [JsonIgnore]
        public string Codigo { get; set; }
        [JsonIgnore]
        public string Versao { get; set; }
        [JsonIgnore]
        public string Rotinas { get; set; }
        public List<int> RotinasList { get; set; }
    }
}