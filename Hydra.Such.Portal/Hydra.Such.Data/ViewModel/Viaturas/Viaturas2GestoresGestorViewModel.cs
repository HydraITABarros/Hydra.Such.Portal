using System;

namespace Hydra.Such.Data.ViewModel.Viaturas
{
    public class Viaturas2GestoresGestorViewModel : ErrorHandler
    {
        public int ID { get; set; }
        public string Gestor { get; set; }
        public string NoMecanografico { get; set; }
        public string Mail { get; set; }
        public int? IDTipo { get; set; }
        public string Tipo { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string DataCriacaoTexto { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        public string DataModificacaoTexto { get; set; }
    }
}
