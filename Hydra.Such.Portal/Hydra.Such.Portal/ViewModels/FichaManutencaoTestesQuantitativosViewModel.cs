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

    public partial class FichaManutencaoTestesQuantitativosViewModel
    {
        [Key]
        public int IdTestesQuantitativos { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string Rotinas { get; set; }
        public int Numero { get; set; }
        public string Versao { get; set; }
        public byte? UsarCampo1 { get; set; }
        public string DescricaoCampo1 { get; set; }
        public string TipoCampo1 { get; set; }
        public string UnidadeCampo1 { get; set; }
        public byte? UsarCampo2 { get; set; }
        public string DescricaoCampo2 { get; set; }
        public string TipoCampo2 { get; set; }
        public string UnidadeCampo2 { get; set; }
        public byte? UsarCampo3 { get; set; }
        public string DescricaoCampo3 { get; set; }
        public string TipoCampo3 { get; set; }
        public string UnidadeCampo3 { get; set; }
        public byte? UsarCampo4 { get; set; }
        public string DescricaoCampo4 { get; set; }
        public string TipoCampo4 { get; set; }
        public string UnidadeCampo4 { get; set; }
        public byte? UsarCampo5 { get; set; }
        public string DescricaoCampo5 { get; set; }
        public string TipoCampo5 { get; set; }
        public string UnidadeCampo5 { get; set; }
        public byte? UsarCampo6 { get; set; }
        public string DescricaoCampo6 { get; set; }
        public string TipoCampo6 { get; set; }
        public string UnidadeCampo6 { get; set; }
        public byte? UsarCampo7 { get; set; }
        public string DescricaoCampo7 { get; set; }
        public string TipoCampo7 { get; set; }
        public string UnidadeCampo7 { get; set; }
        public byte? Na { get; set; }
        public byte? Ignoravel { get; set; }
        public byte? Subcontratacao { get; set; }
    }
}
