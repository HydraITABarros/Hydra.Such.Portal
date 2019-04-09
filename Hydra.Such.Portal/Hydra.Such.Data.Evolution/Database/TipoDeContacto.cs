using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class TipoDeContacto
    {
        public TipoDeContacto()
        {
            Contactos = new HashSet<Contactos>();
        }

        public int IdTipoDeContacto { get; set; }
        public string Descricao { get; set; }

        public virtual ICollection<Contactos> Contactos { get; set; }
    }
}
