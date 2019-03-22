using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class Equipa
    {
        public Equipa()
        {
            Acessorio = new HashSet<Acessorio>();
            AreaOp = new HashSet<AreaOp>();
            ClientePimp = new HashSet<ClientePimp>();
            EquipPimp = new HashSet<EquipPimp>();
            Equipamento = new HashSet<Equipamento>();
            InstituicaoPimp = new HashSet<InstituicaoPimp>();
        }

        public int IdEquipa { get; set; }
        public string Nome { get; set; }
        public bool? Activo { get; set; }
        public string Descricao { get; set; }
        public int? IdArea { get; set; }
        public int? IdRegiao { get; set; }

        public Area IdAreaNavigation { get; set; }
        public Regiao IdRegiaoNavigation { get; set; }
        public ICollection<Acessorio> Acessorio { get; set; }
        public ICollection<AreaOp> AreaOp { get; set; }
        public ICollection<ClientePimp> ClientePimp { get; set; }
        public ICollection<EquipPimp> EquipPimp { get; set; }
        public ICollection<Equipamento> Equipamento { get; set; }
        public ICollection<InstituicaoPimp> InstituicaoPimp { get; set; }
    }
}
