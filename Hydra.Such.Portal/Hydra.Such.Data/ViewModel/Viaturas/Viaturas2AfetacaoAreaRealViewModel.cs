using System;

namespace Hydra.Such.Data.ViewModel.Viaturas
{
    public class Viaturas2AfetacaoAreaRealViewModel : ErrorHandler
    {
        public int ID { get; set; }

        public string AreaReal { get; set; }

        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string DataCriacaoTexto { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        public string DataModificacaoTexto { get; set; }
    }
}
