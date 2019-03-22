using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class OrdemManutencaoEstadoMaterial
    {
        public OrdemManutencaoEstadoMaterial()
        {
            OrdemManutencaoMateriais = new HashSet<OrdemManutencaoMateriais>();
        }

        public int IdEstadoMaterial { get; set; }
        public string Nome { get; set; }

        public ICollection<OrdemManutencaoMateriais> OrdemManutencaoMateriais { get; set; }
    }
}
