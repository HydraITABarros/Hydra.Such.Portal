using System;
using System.Collections.Generic;

namespace Hydra.Such.Tester.Database
{
    public partial class Serviços
    {
        public Serviços()
        {
            LinhasFaturaçãoContrato = new HashSet<LinhasFaturaçãoContrato>();
            ServiçosCliente = new HashSet<ServiçosCliente>();
        }

        public int Código { get; set; }
        public string Descrição { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }

        public ICollection<LinhasFaturaçãoContrato> LinhasFaturaçãoContrato { get; set; }
        public ICollection<ServiçosCliente> ServiçosCliente { get; set; }
    }
}
