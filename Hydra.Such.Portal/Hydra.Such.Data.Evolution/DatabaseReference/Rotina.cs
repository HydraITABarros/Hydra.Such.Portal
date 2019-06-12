using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class Rotina
    {
        public Rotina()
        {
            EquipPimp = new HashSet<EquipPimp>();
            InstituicaoPimp = new HashSet<InstituicaoPimp>();
            OrdemManutencaoEquipamentos = new HashSet<OrdemManutencaoEquipamentos>();
            OrdemManutencaoLinha = new HashSet<OrdemManutencaoLinha>();
            SolicitacoesLinha = new HashSet<SolicitacoesLinha>();
        }

        public int IdRotina { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }

        public virtual ICollection<EquipPimp> EquipPimp { get; set; }
        public virtual ICollection<InstituicaoPimp> InstituicaoPimp { get; set; }
        public virtual ICollection<OrdemManutencaoEquipamentos> OrdemManutencaoEquipamentos { get; set; }
        public virtual ICollection<OrdemManutencaoLinha> OrdemManutencaoLinha { get; set; }
        public virtual ICollection<SolicitacoesLinha> SolicitacoesLinha { get; set; }
    }
}
