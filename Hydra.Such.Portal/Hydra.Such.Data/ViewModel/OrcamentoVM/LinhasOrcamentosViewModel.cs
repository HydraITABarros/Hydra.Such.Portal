using Hydra.Such.Data.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Orcamentos
{
    public class LinhasOrcamentosViewModel : ErrorHandler
    {
        public int NoLinha { get; set; }
        public string NoOrcamento { get; set; }
        public string Descricao { get; set; }
        public decimal? Quantidade { get; set; }
        public decimal? ValorUnitario { get; set; }
        public decimal? TaxaIVA { get; set; }
        public decimal? TotalLinha { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime? DataCriacao { get; set; }
        public string DataCriacaoText { get; set; }
        public string UtilizadorCriacao { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime? DataModificacao { get; set; }
        public string DataModificacaoText { get; set; }
        public string UtilizadorModificacao { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }
    }
}
