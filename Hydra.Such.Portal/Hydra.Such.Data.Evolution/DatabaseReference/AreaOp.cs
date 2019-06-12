using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class AreaOp
    {
        public AreaOp()
        {
            Acessorio = new HashSet<Acessorio>();
            ClientePimp = new HashSet<ClientePimp>();
            Equipamento = new HashSet<Equipamento>();
        }

        public int IdAreaOp { get; set; }
        public string Nome { get; set; }
        public int IdEquipa { get; set; }
        public bool? Activo { get; set; }

        public virtual Equipa IdEquipaNavigation { get; set; }
        public virtual ICollection<Acessorio> Acessorio { get; set; }
        public virtual ICollection<ClientePimp> ClientePimp { get; set; }
        public virtual ICollection<Equipamento> Equipamento { get; set; }
    }
}
