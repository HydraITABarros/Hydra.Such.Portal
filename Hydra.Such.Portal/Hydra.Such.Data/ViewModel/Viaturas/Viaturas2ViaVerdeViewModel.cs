using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.ViewModel.Viaturas
{
    public class Viaturas2ViaVerdeViewModel : ErrorHandler
    {
        public int ID { get; set; }
        public string Matricula { get; set; }

        public int? IDEmpresa { get; set; }
        public string Empresa { get; set; }
        public string NoIdentificador { get; set; }
        public string NoContrato { get; set; }
        public DateTime? Data { get; set; }
        public string DataTexto { get; set; }

        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string DataCriacaoTexto { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        public string DataModificacaoTexto { get; set; }
    }
}
