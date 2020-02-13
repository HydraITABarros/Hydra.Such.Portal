using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.ViewModel.Viaturas
{
    public class Viaturas2GestoresViewModel : ErrorHandler
    {
        public int ID { get; set; }
        public string Matricula { get; set; }

        public int? IDTipo { get; set; }
        public int? IDGestor { get; set; }
        public string Gestor { get; set; }
        public DateTime? DataInicio { get; set; }
        public string DataInicioTexto { get; set; }
        public DateTime? DataFim { get; set; }
        public String DataFimTexto { get; set; }

        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string DataCriacaoTexto { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        public string DataModificacaoTexto { get; set; }
    }
}
