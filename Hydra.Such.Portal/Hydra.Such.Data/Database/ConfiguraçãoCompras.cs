using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class ConfiguraçãoCompras
    {
        public int Id { get; set; }
        public string Email1Regiao12 { get; set; }
        public string Email2Regiao12 { get; set; }
        public string Email1Regiao23 { get; set; }
        public string Email2Regiao23 { get; set; }
        public string Email1Regiao33 { get; set; }
        public string Email2Regiao33 { get; set; }
        public string Email1Regiao43 { get; set; }
        public string Email2Regiao43 { get; set; }
        public int? DiasParaEnvioAlerta { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
    }
}
