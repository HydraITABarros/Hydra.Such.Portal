using System;

namespace Hydra.Such.Data.Database
{
    public partial class ConfiguracaoTabelas
    {
        public string Tabela { get; set; }
        public int ID { get; set; }
        public string Descricao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataCriacao { get; set; }
    }
}
