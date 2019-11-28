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

    public class FichaManutencaoRelatorioMaterialViewModel
    {
        public int? Id { get; set; }
        public string Descricao { get; set; }
        public string Quantidade { get; set; }
        public int? FornecidoPor { get; set; }
    }
}