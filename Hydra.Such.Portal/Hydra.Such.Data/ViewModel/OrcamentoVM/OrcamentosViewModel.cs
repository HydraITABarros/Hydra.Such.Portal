using Hydra.Such.Data.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.OrcamentoVM
{
    public class OrcamentosViewModel : ErrorHandler
    {
        public string No { get; set; }
        public string NoCliente { get; set; }
        public string NoContacto { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime? DataValidade { get; set; }
        public string DataValidadeText { get; set; }
        public int? IDEstado { get; set; }
        public string Descricao { get; set; }
        public string CodRegiao { get; set; }
        public int? UnidadePrestacao { get; set; }
        public int? TipoFaturacao { get; set; }
        public decimal? TotalSemIVA { get; set; }
        public decimal? TotalComIVA { get; set; }
        public string NoProposta { get; set; }
        public string Email { get; set; }
        public string EmailAssunto { get; set; }
        public string EmailCorpo { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime? EmailDataEnvio { get; set; }
        public string EmailDataEnvioText { get; set; }
        public string EmailUtilizadorEnvio { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime? DataCriacao { get; set; }
        public string DataCriacaoText { get; set; }
        public string UtilizadorCriacao { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime? DataAceite { get; set; }
        public string DataAceiteText { get; set; }
        public string UtilizadorAceite { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime? DataConcluido { get; set; }
        public string DataConcluidoText { get; set; }
        public string UtilizadorConcluido { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime? DataModificacao { get; set; }
        public string DataModificacaoText { get; set; }
        public string UtilizadorModificacao { get; set; }

        public List<Orcamentos.LinhasOrcamentosViewModel> LinhasOrcamentos { get; set; }


        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }
    }
}
