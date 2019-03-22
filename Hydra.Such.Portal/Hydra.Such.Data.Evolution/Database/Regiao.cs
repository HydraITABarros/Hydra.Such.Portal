using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class Regiao
    {
        public Regiao()
        {
            Acessorio = new HashSet<Acessorio>();
            Cliente = new HashSet<Cliente>();
            ClientePimp = new HashSet<ClientePimp>();
            Equipa = new HashSet<Equipa>();
            Equipamento = new HashSet<Equipamento>();
        }

        public int IdRegiao { get; set; }
        public string Regiao1 { get; set; }
        public string Codigo { get; set; }
        public bool? Activo { get; set; }

        public ICollection<Acessorio> Acessorio { get; set; }
        public ICollection<Cliente> Cliente { get; set; }
        public ICollection<ClientePimp> ClientePimp { get; set; }
        public ICollection<Equipa> Equipa { get; set; }
        public ICollection<Equipamento> Equipamento { get; set; }
    }
}
