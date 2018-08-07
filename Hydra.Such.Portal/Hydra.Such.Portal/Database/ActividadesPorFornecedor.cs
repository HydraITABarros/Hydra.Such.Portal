using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
{
    public partial class ActividadesPorFornecedor
    {
        public int Id { get; set; }
        public string CodFornecedor { get; set; }
        public string CodActividade { get; set; }

        public Actividades CodActividadeNavigation { get; set; }
    }
}
