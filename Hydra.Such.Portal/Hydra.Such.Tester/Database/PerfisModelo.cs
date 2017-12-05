using System;
using System.Collections.Generic;

namespace Hydra.Such.Tester.Database
{
    public partial class PerfisModelo
    {
        public PerfisModelo()
        {
            AcessosPerfil = new HashSet<AcessosPerfil>();
            PerfisUtilizador = new HashSet<PerfisUtilizador>();
        }

        public int Id { get; set; }
        public string Descrição { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }

        public ICollection<AcessosPerfil> AcessosPerfil { get; set; }
        public ICollection<PerfisUtilizador> PerfisUtilizador { get; set; }
    }
}
