using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class PresençasFolhaDeHoras
    {
        public string NºFolhaDeHoras { get; set; }
        public DateTime Data { get; set; }
        public string NoEmpregado { get; set; }
        public TimeSpan? Hora1ªEntrada { get; set; }
        public TimeSpan? Hora1ªSaída { get; set; }
        public TimeSpan? Hora2ªEntrada { get; set; }
        public TimeSpan? Hora2ªSaída { get; set; }
        public string Observacoes { get; set; }
        public int? Validado { get; set; }
        public int? IntegradoTr { get; set; }
        public DateTime? DataIntTr { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorModificação { get; set; }
        public DateTime? DataHoraModificação { get; set; }

        public FolhasDeHoras NºFolhaDeHorasNavigation { get; set; }
    }
}
