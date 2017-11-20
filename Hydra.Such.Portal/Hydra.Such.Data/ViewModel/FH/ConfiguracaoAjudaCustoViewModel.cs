using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.ViewModel.FH
{
    public partial class ConfiguracaoAjudaCustoViewModel
    {
        public string CodigoTipoCusto { get; set; }
        public int CodigoRefCusto { get; set; }
        public bool? DataChegadaDataPartida { get; set; }
        public decimal? DistanciaMinima { get; set; }
        public string LimiteHoraPartida { get; set; }
        public string LimiteHoraChegada { get; set; }
        public bool? Prioritario { get; set; }
        public int? TipoCusto { get; set; }
        public int? SinalHoraPartida { get; set; }
        public string HoraPartida { get; set; }
        public int? SinalHoraChegada { get; set; }
        public string HoraChegada { get; set; }
        public bool? TipoEstadia { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string UtilizadorModificacao { get; set; }
    }
}
