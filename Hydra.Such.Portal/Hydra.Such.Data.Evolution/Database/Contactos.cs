using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
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

        public Cliente ClienteNavigation { get; set; }
        public Instituicao InstituicaoNavigation { get; set; }
        public Servico ServicoNavigation { get; set; }
        public TipoDeContacto TipoContactoNavigation { get; set; }
    }
}
