using System;
using System.Collections.Generic;

namespace Hydra.Such.Tester.Database
{
    public partial class Modelos
    {
        public Modelos()
        {
            Viaturas = new HashSet<Viaturas>();
        }

        public int CódigoMarca { get; set; }
        public int CódigoModelo { get; set; }
        public string Descrição { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }

        public ICollection<Viaturas> Viaturas { get; set; }
    }
}
