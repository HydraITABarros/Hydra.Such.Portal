﻿using System;

namespace Hydra.Such.Data.ViewModel.VisitasVM
{
    public class VisitasTarefasTarefasViewModel: ErrorHandler
    {
        public int CodTarefa { get; set; }
        public string Tarefa { get; set; }
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
