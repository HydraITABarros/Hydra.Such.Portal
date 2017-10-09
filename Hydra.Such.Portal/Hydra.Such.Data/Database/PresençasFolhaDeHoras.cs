using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class PresençasFolhaDeHoras
    {
        public string NºFolhaDeHoras { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan? Hora1ªEntrada { get; set; }
        public TimeSpan? Hora1ªSaída { get; set; }
        public TimeSpan? Hora2ªEntrada { get; set; }
        public TimeSpan? Hora2ªSaída { get; set; }

        public FolhasDeHoras NºFolhaDeHorasNavigation { get; set; }
    }
}
