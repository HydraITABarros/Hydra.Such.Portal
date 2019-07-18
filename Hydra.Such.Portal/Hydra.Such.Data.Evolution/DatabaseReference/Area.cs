using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class Area
    {
        public Area()
        {
            Acessorio = new HashSet<Acessorio>();
            ClientePimp = new HashSet<ClientePimp>();
            Equipa = new HashSet<Equipa>();
            Equipamento = new HashSet<Equipamento>();
        }

        public int IdArea { get; set; }
        public string Nome { get; set; }
        public bool? Activo { get; set; }
        public string Descricao { get; set; }

        public virtual ICollection<Acessorio> Acessorio { get; set; }
        public virtual ICollection<ClientePimp> ClientePimp { get; set; }
        public virtual ICollection<Equipa> Equipa { get; set; }
        public virtual ICollection<Equipamento> Equipamento { get; set; }
    }
}
