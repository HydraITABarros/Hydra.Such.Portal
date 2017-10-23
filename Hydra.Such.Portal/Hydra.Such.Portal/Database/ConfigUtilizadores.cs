﻿using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
{
    public partial class ConfigUtilizadores
    {
        public ConfigUtilizadores()
        {
            AcessosUtilizador = new HashSet<AcessosUtilizador>();
            PerfisUtilizador = new HashSet<PerfisUtilizador>();
        }

        public string IdUtilizador { get; set; }
        public string Nome { get; set; }
        public bool? Ativo { get; set; }
        public bool Administrador { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorCriação { get; set; }
        public string UtilizadorModificação { get; set; }

        public ICollection<AcessosUtilizador> AcessosUtilizador { get; set; }
        public ICollection<PerfisUtilizador> PerfisUtilizador { get; set; }
    }
}
