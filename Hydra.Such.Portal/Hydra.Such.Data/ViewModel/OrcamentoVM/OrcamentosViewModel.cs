using Hydra.Such.Data.Database;
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
        public string ClienteText { get; set; }
        public string NoContacto { get; set; }
        public string ContactoText { get; set; }
        public string ContactoNome { get; set; }
        public string ContactoTelefone { get; set; }
        public string ContactoEmail { get; set; }
        public string ContactoNotas { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime? DataValidade { get; set; }
        public string DataValidadeText { get; set; }
        public int? IDEstado { get; set; }
        public string EstadoText { get; set; }
        public string Descricao { get; set; }
        public string CodRegiao { get; set; }
        public string RegiaoText { get; set; }
        public int? UnidadePrestacao { get; set; }
        public string UnidadePrestacaoText { get; set; }
        public int? TipoFaturacao { get; set; }
        public string TipoFaturacaoText { get; set; }
        public int? CondicoesPagamento { get; set; }
        public string CondicoesPagamentoText { get; set; }
        public decimal? TotalSemIVA { get; set; }
        public decimal? TotalComIVA { get; set; }
        public string NoProposta { get; set; }
        public string ProjetoAssociado { get; set; }
        public string ProjetoAssociadoText { get; set; }
        public string Email { get; set; }
        public string EmailAssunto { get; set; }
        public string EmailCorpo { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime? EmailDataEnvio { get; set; }
        public string EmailDataEnvioText { get; set; }
        public string EmailUtilizadorEnvio { get; set; }
        public string EmailUtilizadorEnvioText { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime? DataCriacao { get; set; }
        public string DataCriacaoText { get; set; }
        public string UtilizadorCriacao { get; set; }
        public string UtilizadorCriacaoText { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime? DataAceite { get; set; }
        public string DataAceiteText { get; set; }
        public string UtilizadorAceite { get; set; }
        public string UtilizadorAceiteText { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime? DataNaoAceite { get; set; }
        public string DataNaoAceiteText { get; set; }
        public string UtilizadorNaoAceite { get; set; }
        public string UtilizadorNaoAceiteText { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime? DataConcluido { get; set; }
        public string DataConcluidoText { get; set; }
        public string UtilizadorConcluido { get; set; }
        public string UtilizadorConcluidoText { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime? DataModificacao { get; set; }
        public string DataModificacaoText { get; set; }
        public string UtilizadorModificacao { get; set; }
        public string UtilizadorModificacaoText { get; set; }

        public List<LinhasOrcamentosViewModel> LinhasOrcamentos { get; set; }
        public List<AttachmentsViewModel> AnexosOrcamentos { get; set; }


        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }
    }
}
