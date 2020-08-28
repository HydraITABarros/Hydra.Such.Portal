using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class ConfiguraçãoNumerações
    {
        public ConfiguraçãoNumerações()
        {
            ConfiguraçãoNumeraçãoContratosNavigation = new HashSet<Configuração>();
            ConfiguraçãoNumeraçãoFolhasDeHorasNavigation = new HashSet<Configuração>();
            ConfiguraçãoNumeraçãoProcedimentoAquisiçãoNavigation = new HashSet<Configuração>();
            ConfiguraçãoNumeraçãoProcedimentoSimplificadoNavigation = new HashSet<Configuração>();
            ConfiguraçãoNumeraçãoProjetosNavigation = new HashSet<Configuração>();
        }

        public int Id { get; set; }
        public string Descrição { get; set; }
        public bool? Automático { get; set; }
        public bool? Manual { get; set; }
        public string Prefixo { get; set; }
        public int? NºDígitosIncrementar { get; set; }
        public int? QuantidadeIncrementar { get; set; }
        public string ÚltimoNºUsado { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorCriação { get; set; }
        public string UtilizadorModificação { get; set; }

        public ICollection<Configuração> ConfiguraçãoNumeraçãoContratosNavigation { get; set; }
        public ICollection<Configuração> ConfiguraçãoNumeraçãoFolhasDeHorasNavigation { get; set; }
        public ICollection<Configuração> ConfiguraçãoNumeraçãoProcedimentoAquisiçãoNavigation { get; set; }
        public ICollection<Configuração> ConfiguraçãoNumeraçãoProcedimentoSimplificadoNavigation { get; set; }
        public ICollection<Configuração> ConfiguraçãoNumeraçãoProjetosNavigation { get; set; }

        #region zpgm.SGPPF
        public ICollection<Configuração> ConfiguracaoNumeracoPedidoFormacao { get; set; }
        #endregion
    }
}
