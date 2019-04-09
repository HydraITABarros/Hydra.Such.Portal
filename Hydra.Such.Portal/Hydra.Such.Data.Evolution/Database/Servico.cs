using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class Servico
    {
        public Servico()
        {
            Contactos = new HashSet<Contactos>();
            Equipamento = new HashSet<Equipamento>();
            OrdemManutencao = new HashSet<OrdemManutencao>();
            OrdemManutencaoEquipamentos = new HashSet<OrdemManutencaoEquipamentos>();
        }

        public int IdServico { get; set; }
        public string Nome { get; set; }
        public int Instituicao { get; set; }
        public string TreePath { get; set; }
        public bool? Activo { get; set; }
        public string CentroCusto { get; set; }
        public string Morada { get; set; }
        public string NoNavision { get; set; }

        public virtual Instituicao InstituicaoNavigation { get; set; }
        public virtual ICollection<Contactos> Contactos { get; set; }
        public virtual ICollection<Equipamento> Equipamento { get; set; }
        public virtual ICollection<OrdemManutencao> OrdemManutencao { get; set; }
        public virtual ICollection<OrdemManutencaoEquipamentos> OrdemManutencaoEquipamentos { get; set; }
    }
}
