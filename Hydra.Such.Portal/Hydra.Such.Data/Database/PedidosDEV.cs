using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class PedidosDEV
    {
        public int ID { get; set; }
        public string Processo { get; set; }
        public string Descricao { get; set; }
        public string Acao { get; set; }
        public string URL { get; set; }
        public int? Estado { get; set; }
        public DateTime? DataEstado { get; set; }
        public DateTime? DataPedido { get; set; }
        public string PedidoPor { get; set; }
        public DateTime? DataConclusao { get; set; }
        public string Intervenientes { get; set; }
        public int? NoHorasPrevistas { get; set; }
        public int? NoHorasRealizadas { get; set; }
        public string CriadoPor { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string AlteradoPor { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}
