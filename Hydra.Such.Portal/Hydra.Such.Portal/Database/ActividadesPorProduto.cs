using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
{
    public partial class ActividadesPorProduto
    {
        public int Id { get; set; }
        public string CodProduto { get; set; }
        public string CodActividade { get; set; }

        public Actividades CodActividadeNavigation { get; set; }
    }
}
