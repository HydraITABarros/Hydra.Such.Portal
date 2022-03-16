using System;

namespace Hydra.Such.Data.ViewModel.VisitasVM
{
    public class VisitasTarefasViewModel: ErrorHandler
    {
        public int ID { get; set; }
        public string CodVisita { get; set; }
        public int? Ordem { get; set; }
        public int CodTarefa { get; set; }
        public string Tarefa { get; set; }
        public DateTime? DataDuracao { get; set; }
        public string DataTexto { get; set; }
        public string DuracaoTexto { get; set; }
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
