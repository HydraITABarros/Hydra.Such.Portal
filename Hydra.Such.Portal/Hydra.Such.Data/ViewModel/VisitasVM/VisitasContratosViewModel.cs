using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.VisitasVM
{
    public class VisitasContratosViewModel : ErrorHandler
    {
        public string CodVisita { get; set; }
        public string NoContrato { get; set; }
        public string AmbitoServico { get; set; }
        public string NoCliente { get; set; }
        public string NomeCliente { get; set; }
        public string CodArea { get; set; }
        public string NomeArea { get; set; }
        public string CodCresp { get; set; }
        public string NomeCresp { get; set; }
        public string UtilizadorCriacao { get; set; }
        public string UtilizadorCriacaoTexto { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string DataHoraCriacaoTexto { get; set; }
        public string UtilizadorModificacao { get; set; }
        public string UtilizadorModificacaoTexto { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string DataHoraModificacaoTexto { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }
    }
}
