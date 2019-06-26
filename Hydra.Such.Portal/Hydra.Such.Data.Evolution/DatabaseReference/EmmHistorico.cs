using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class EmmHistorico
    {
        public int Id { get; set; }
        public int IdUtilizador { get; set; }
        public int IdAccao { get; set; }
        public int IdTabela { get; set; }
        public int? NumEmm { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public bool Activo { get; set; }
    }
}
