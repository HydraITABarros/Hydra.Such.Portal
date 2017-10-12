using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class Configuração
    {
        public int Id { get; set; }
        public int? NumeraçãoProjetos { get; set; }
        public int? NumeraçãoContratos { get; set; }
        public int? NumeraçãoFolhasDeHoras { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }

        public ConfiguraçãoNumerações NumeraçãoContratosNavigation { get; set; }
        public ConfiguraçãoNumerações NumeraçãoFolhasDeHorasNavigation { get; set; }
        public ConfiguraçãoNumerações NumeraçãoProjetosNavigation { get; set; }
    }
}
