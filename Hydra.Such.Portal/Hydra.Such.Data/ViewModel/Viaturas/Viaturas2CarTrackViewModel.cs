using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.ViewModel.Viaturas
{
    public class Viaturas2CarTrackViewModel : ErrorHandler
    {
        public int ID { get; set; }
        public string Matricula { get; set; }

        public int? IDEmpresa { get; set; }
        public string Empresa { get; set; }
        public DateTime? DataInicio { get; set; }
        public string DataInicioTexto { get; set; }
        public DateTime? DataFim { get; set; }
        public string DataFimTexto { get; set; }

        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string DataCriacaoTexto { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        public string DataModificacaoTexto { get; set; }
    }
}
