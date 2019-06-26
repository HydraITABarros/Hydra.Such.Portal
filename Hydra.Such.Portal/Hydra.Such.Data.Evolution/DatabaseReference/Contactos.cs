using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class Contactos
    {
        public int IdContacto { get; set; }
        public int TipoContacto { get; set; }
        public int? Cliente { get; set; }
        public int? Instituicao { get; set; }
        public int? Servico { get; set; }
        public string NomeContacto { get; set; }
        public string Contacto { get; set; }
        public string Email { get; set; }

        public virtual Cliente ClienteNavigation { get; set; }
        public virtual Instituicao InstituicaoNavigation { get; set; }
        public virtual Servico ServicoNavigation { get; set; }
        public virtual TipoDeContacto TipoContactoNavigation { get; set; }
    }
}
