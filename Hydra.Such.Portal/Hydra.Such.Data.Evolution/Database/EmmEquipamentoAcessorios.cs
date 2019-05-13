using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class EmmEquipamentoAcessorios
    {
        public int Id { get; set; }
        public int EquipamentoNumEmm { get; set; }
        public int AcessorioNumEmm { get; set; }
        public bool? Activo { get; set; }
    }
}
