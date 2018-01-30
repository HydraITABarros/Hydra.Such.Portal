using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class PontosSituaçãoRq
    {
        public string NºRequisição { get; set; }
        public int NºPedido { get; set; }
        public string PedidoDePontoSituação { get; set; }
        public DateTime DataPedido { get; set; }
        public string UtilizadorPedido { get; set; }
        public string Resposta { get; set; }
        public DateTime? DataResposta { get; set; }
        public string UtilizadorResposta { get; set; }
        public bool ConfirmaçãoLeitura { get; set; }
    }
}
