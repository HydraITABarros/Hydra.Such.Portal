using System;

namespace Hydra.Such.Data.ViewModel.Viaturas
{
    public class ConfiguracaoTabelasViewModel : ErrorHandler
    {
        public string Tabela { get; set; }
        public int ID { get; set; }
        public string Descricao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string DataCriacaoTexto { get; set; }
    }
}
