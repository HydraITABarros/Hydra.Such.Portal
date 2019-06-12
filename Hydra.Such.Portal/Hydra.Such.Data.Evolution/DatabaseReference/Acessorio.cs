using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class Acessorio
    {
        public Acessorio()
        {
            EquipamentoAcessorio = new HashSet<EquipamentoAcessorio>();
        }

        public int IdAcessorio { get; set; }
        public string Nome { get; set; }
        public int IdMarca { get; set; }
        public int IdModelo { get; set; }
        public string NumSerie { get; set; }
        public string NumInventario { get; set; }
        public string Tipo { get; set; }
        public string Localizacao { get; set; }
        public bool? Activo { get; set; }
        public int? IdRegiao { get; set; }
        public int? IdArea { get; set; }
        public int? IdEquipa { get; set; }
        public int? IdAreaOp { get; set; }
        public int? IdFornecedor { get; set; }
        public string Observacao { get; set; }

        public virtual Area IdAreaNavigation { get; set; }
        public virtual AreaOp IdAreaOpNavigation { get; set; }
        public virtual Equipa IdEquipaNavigation { get; set; }
        public virtual EquipMarca IdMarcaNavigation { get; set; }
        public virtual EquipModelo IdModeloNavigation { get; set; }
        public virtual Regiao IdRegiaoNavigation { get; set; }
        public virtual ICollection<EquipamentoAcessorio> EquipamentoAcessorio { get; set; }
    }
}
