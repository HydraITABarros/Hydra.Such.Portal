using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class EquipEstado
    {
        public EquipEstado()
        {
            EquipPimp = new HashSet<EquipPimp>();
            OrdemManutencaoEquipamentos = new HashSet<OrdemManutencaoEquipamentos>();
            OrdemManutencaoLinha = new HashSet<OrdemManutencaoLinha>();
            SolicitacoesLinha = new HashSet<SolicitacoesLinha>();
        }

        public int IdEquipEstado { get; set; }
        public string Nome { get; set; }
        public int Indice { get; set; }

        public virtual ICollection<EquipPimp> EquipPimp { get; set; }
        public virtual ICollection<OrdemManutencaoEquipamentos> OrdemManutencaoEquipamentos { get; set; }
        public virtual ICollection<OrdemManutencaoLinha> OrdemManutencaoLinha { get; set; }
        public virtual ICollection<SolicitacoesLinha> SolicitacoesLinha { get; set; }
    }
}
