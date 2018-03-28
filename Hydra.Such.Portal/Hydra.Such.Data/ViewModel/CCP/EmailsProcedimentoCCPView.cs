using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;

namespace Hydra.Such.Data.ViewModel.CCP
{
    public class EmailsProcedimentoCCPView
    {
        public string NoProcedimento { get; set; }
        public int NoLinha { get; set; }
        public string Esclarecimento { get; set; }
        public string Resposta { get; set; }
        public DateTime? DataHoraPedido { get; set; }
        public string UtilizadorPedidoEscl { get; set; }
        public DateTime? DataHoraResposta { get; set; }
        public string UtilizadorResposta { get; set; }
        public bool? Anexo { get; set; }
        public bool? Anexo1 { get; set; }
        public bool? Email { get; set; }
        public string Destinatario { get; set; }
        public string Assunto { get; set; }
        public DateTime? DataHoraEmail { get; set; }
        public string TextoEmail { get; set; }
        public string EmailDestinatario { get; set; }
        public string UtilizadorEmail { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }

        public string DataEmail { get; set; }
        public string HoraEmail { get; set; }

        public ProcedimentosCcp NoProcedimentoNavigation { get; set; }
    }
}
