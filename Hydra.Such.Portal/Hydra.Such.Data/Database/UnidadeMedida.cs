using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class UnidadeMedida
    {
        public UnidadeMedida()
        {
            FichaProduto = new HashSet<FichaProduto>();
        }

        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }

        public ICollection<FichaProduto> FichaProduto { get; set; }
    }
}
