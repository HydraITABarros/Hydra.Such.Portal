using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class ConfiguraçãoVendasAlertas
    {
        public int Id { get; set; }
        public string Email1Regiao12 { get; set; }
        public string Email2Regiao12 { get; set; }
        public string Email3Regiao12 { get; set; }
        public string Email4Regiao12 { get; set; }
        public string Email1Regiao23 { get; set; }
        public string Email2Regiao23 { get; set; }
        public string Email3Regiao23 { get; set; }
        public string Email4Regiao23 { get; set; }
        public string Email1Regiao33 { get; set; }
        public string Email2Regiao33 { get; set; }
        public string Email3Regiao33 { get; set; }
        public string Email4Regiao33 { get; set; }
        public string Email1Regiao43 { get; set; }
        public string Email2Regiao43 { get; set; }
        public string Email3Regiao43 { get; set; }
        public string Email4Regiao43 { get; set; }
        public int? DiasParaEnvioAlerta { get; set; }
        public int? DiasParaEnvioAlertaAudienciaPrevia { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
    }
}
