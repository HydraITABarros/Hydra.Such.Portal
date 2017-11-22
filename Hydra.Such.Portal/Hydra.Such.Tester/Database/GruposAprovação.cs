using System;
using System.Collections.Generic;

namespace Hydra.Such.Tester.Database
{
    public partial class GruposAprovação
    {
        public GruposAprovação()
        {
            ConfiguraçãoAprovações = new HashSet<ConfiguraçãoAprovações>();
            MovimentosDeAprovação = new HashSet<MovimentosDeAprovação>();
            UtilizadoresGruposAprovação = new HashSet<UtilizadoresGruposAprovação>();
        }

        public int Código { get; set; }
        public string Descrição { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }

        public ICollection<ConfiguraçãoAprovações> ConfiguraçãoAprovações { get; set; }
        public ICollection<MovimentosDeAprovação> MovimentosDeAprovação { get; set; }
        public ICollection<UtilizadoresGruposAprovação> UtilizadoresGruposAprovação { get; set; }
    }
}
