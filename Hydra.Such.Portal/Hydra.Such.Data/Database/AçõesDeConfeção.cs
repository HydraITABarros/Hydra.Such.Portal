using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class AçõesDeConfeção
    {
        public AçõesDeConfeção()
        {
            ProcedimentosDeConfeção = new HashSet<ProcedimentosDeConfeção>();
        }

        public int Código { get; set; }
        public string Descrição { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorCriação { get; set; }
        public string UtilizadorModificação { get; set; }

        public ICollection<ProcedimentosDeConfeção> ProcedimentosDeConfeção { get; set; }
    }
}
