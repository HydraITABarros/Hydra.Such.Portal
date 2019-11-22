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

    public class FichaManutencaoEmmEquipamentosViewModel
    {
        public int Id { get; set; }
        public int? IdTipo { get; set; }
        public string TipoDescricao { get; set; }
        public int? IdMarca { get; set; }
        public string MarcaText { get; set; }
        public int? IdModelo { get; set; }
        public string ModeloText { get; set; }
        public string NumSerie { get; set; }
    }
}
