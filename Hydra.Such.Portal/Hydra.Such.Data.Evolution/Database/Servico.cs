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

        public Instituicao InstituicaoNavigation { get; set; }
        public ICollection<Contactos> Contactos { get; set; }
        public ICollection<Equipamento> Equipamento { get; set; }
        public ICollection<OrdemManutencao> OrdemManutencao { get; set; }
        public ICollection<OrdemManutencaoEquipamentos> OrdemManutencaoEquipamentos { get; set; }
    }
}
