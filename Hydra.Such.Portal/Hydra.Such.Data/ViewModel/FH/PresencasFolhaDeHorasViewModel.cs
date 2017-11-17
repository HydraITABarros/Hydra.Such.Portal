using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.FH
{
    public class PresencasFolhaDeHorasViewModel
    {
        public string FolhaDeHorasNo { get; set; }
        public DateTime? Data { get; set; }
        public string DataTexto { get; set; }
        public string Hora1Entrada { get; set; }
        public string Hora1Saida { get; set; }
        public string Hora2Entrada { get; set; }
        public string Hora2Saida { get; set; }
        public string Observacoes { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string DataHoraCriacaoTexto { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string DataHoraModificacaoTexto { get; set; }
    }
}
