using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.Database
{
    public class ConfiguracaoParametros
    {
        public int ID { get; set; }
        public string Parametro { get; set; }
        public string Descricao { get; set; }
        public string Valor { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string UtilizadorModificacao { get; set; }
    }
}
