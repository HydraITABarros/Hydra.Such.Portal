using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;

namespace Hydra.Such.Data.ViewModel.CCP
{
    public class FluxoTrabalhoListaControloCCPView
    {
        public string No { get; set; }
        public int Estado { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }
        public string Data_Show { get; set; }
        public string Hora_Show { get; set; }
        public int? TipoEstado { get; set; }
        public string Comentario { get; set; }
        public string Resposta { get; set; }
        public int? TipoResposta { get; set; }
        public DateTime? DataResposta { get; set; }
        public string User { get; set; }
        public string NomeUser { get; set; }
        public bool? ImobSimNao { get; set; }
        public int? EstadoAnterior { get; set; }
        public int? EstadoSeguinte { get; set; }
        public string Comentario2 { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }

        public string Nr_Workflow { get; set; }    //Para controlar a côr da coluna 

        public ProcedimentosCcp NoNavigation { get; set; }
    }
}
