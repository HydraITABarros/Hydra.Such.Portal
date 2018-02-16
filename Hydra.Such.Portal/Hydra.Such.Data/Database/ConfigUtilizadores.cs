using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class ConfigUtilizadores
    {
        public ConfigUtilizadores()
        {
            AcessosDimensões = new HashSet<AcessosDimensões>();
            AcessosUtilizador = new HashSet<AcessosUtilizador>();
            PerfisUtilizador = new HashSet<PerfisUtilizador>();
        }

        public string IdUtilizador { get; set; }
        public string Nome { get; set; }
        public string EmployeeNo { get; set; }
        public bool? Ativo { get; set; }
        public bool Administrador { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorCriação { get; set; }
        public string UtilizadorModificação { get; set; }
        public string ProcedimentosEmailEnvioParaCa { get; set; }
        public string ProcedimentosEmailEnvioParaArea { get; set; }
        public string ProcedimentosEmailEnvioParaArea2 { get; set; }
        public string RegiaoPorDefeito { get; set; }
        public string AreaPorDefeito { get; set; }
        public string CentroRespPorDefeito { get; set; }

        public ICollection<AcessosDimensões> AcessosDimensões { get; set; }
        public ICollection<AcessosUtilizador> AcessosUtilizador { get; set; }
        public ICollection<PerfisUtilizador> PerfisUtilizador { get; set; }
    }
}
