using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class Instituicao
    {
        public Instituicao()
        {
            Contactos = new HashSet<Contactos>();
            InstituicaoPimp = new HashSet<InstituicaoPimp>();
            OrdemManutencao = new HashSet<OrdemManutencao>();
            Servico = new HashSet<Servico>();
        }

        public int IdInstituicao { get; set; }
        public string Nome { get; set; }
        public int? Mae { get; set; }
        public int Cliente { get; set; }
        public string TreePath { get; set; }
        public bool? Activo { get; set; }
        public string DescricaoTreePath { get; set; }
        public string Morada { get; set; }
        public string NoNavision { get; set; }

        public virtual Cliente ClienteNavigation { get; set; }
        public virtual ICollection<Contactos> Contactos { get; set; }
        public virtual ICollection<InstituicaoPimp> InstituicaoPimp { get; set; }
        public virtual ICollection<OrdemManutencao> OrdemManutencao { get; set; }
        public virtual ICollection<Servico> Servico { get; set; }
    }
}
