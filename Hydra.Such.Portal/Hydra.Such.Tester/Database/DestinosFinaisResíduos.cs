using System;
using System.Collections.Generic;

namespace Hydra.Such.Tester.Database
{
    public partial class DestinosFinaisResíduos
    {
        public DestinosFinaisResíduos()
        {
            DiárioDeProjeto = new HashSet<DiárioDeProjeto>();
        }

        public int Código { get; set; }
        public string Descrição { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorCriação { get; set; }
        public string UtilizadorModificação { get; set; }

        public ICollection<DiárioDeProjeto> DiárioDeProjeto { get; set; }
    }
}
