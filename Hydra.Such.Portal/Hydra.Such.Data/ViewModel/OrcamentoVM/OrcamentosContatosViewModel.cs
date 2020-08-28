using Hydra.Such.Data.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.ViewModel.OrcamentoVM
{
    public class OrcamentosContatosViewModel : ErrorHandler
    {
        public string ID { get; set; }
        public string Organizacao { get; set; }
        public string Nome { get; set; }
        public string Telemovel { get; set; }
        public string Email { get; set; }
        public string NIF { get; set; }
        public string Notas { get; set; }
        public string CriadoPor { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime? DataCriacao { get; set; }
        public string DataCriacaoText { get; set; }
        public string AlteradoPor { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public DateTime? DataAlteracao { get; set; }
        public string DataAlteracaoText { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }
    }
}
