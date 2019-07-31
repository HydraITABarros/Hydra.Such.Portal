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

    public class FichaManutencaoManutencaoViewModel
    {
        public int? IdManutencao { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string Rotinas { get; set; }
        public int? Numero { get; set; }
        public string Versao { get; set; }
        public int Value { get; set; }
    }

}
