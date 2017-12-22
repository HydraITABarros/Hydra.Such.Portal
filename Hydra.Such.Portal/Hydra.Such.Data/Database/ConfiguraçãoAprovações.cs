using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class ConfiguraçãoAprovações
    {
        public int Id { get; set; }
        public int Tipo { get; set; }
        public int Área { get; set; }
        public int NívelAprovação { get; set; }
        public decimal ValorAprovação { get; set; }
        public string UtilizadorAprovação { get; set; }
        public int GrupoAprovação { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorCriação { get; set; }
        public string UtilizadorModificação { get; set; }

        public GruposAprovação GrupoAprovaçãoNavigation { get; set; }
    }
}
