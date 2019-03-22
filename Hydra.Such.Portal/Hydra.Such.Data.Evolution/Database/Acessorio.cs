using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
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

        public Area IdAreaNavigation { get; set; }
        public AreaOp IdAreaOpNavigation { get; set; }
        public Equipa IdEquipaNavigation { get; set; }
        public EquipMarca IdMarcaNavigation { get; set; }
        public EquipModelo IdModeloNavigation { get; set; }
        public Regiao IdRegiaoNavigation { get; set; }
        public ICollection<EquipamentoAcessorio> EquipamentoAcessorio { get; set; }
    }
}
