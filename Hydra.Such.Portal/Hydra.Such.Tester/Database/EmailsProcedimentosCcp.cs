using System;
using System.Collections.Generic;

namespace Hydra.Such.Tester.Database
{
    public partial class EmailsProcedimentosCcp
    {
        public string NºProcedimento { get; set; }
        public int NºLinha { get; set; }
        public string Esclarecimento { get; set; }
        public string Resposta { get; set; }
        public DateTime? DataHoraPedido { get; set; }
        public string UtilizadorPedidoEscl { get; set; }
        public DateTime? DataHoraResposta { get; set; }
        public string UtilizadorResposta { get; set; }
        public bool? Anexo { get; set; }
        public bool? Anexo1 { get; set; }
        public bool? Email { get; set; }
        public string Destinatário { get; set; }
        public string Assunto { get; set; }
        public DateTime? DataHoraEmail { get; set; }
        public string TextoEmail { get; set; }
        public string EmailDestinatário { get; set; }
        public string UtilizadorEmail { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorCriação { get; set; }
        public string UtilizadorModificação { get; set; }

        public ProcedimentosCcp NºProcedimentoNavigation { get; set; }
    }
}
