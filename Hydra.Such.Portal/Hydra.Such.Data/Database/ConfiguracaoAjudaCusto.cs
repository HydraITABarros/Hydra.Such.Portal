using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class ConfiguracaoAjudaCusto
    {
        public string CodigoTipoCusto { get; set; }
        public int CodigoRefCusto { get; set; }
        public bool DataChegadaDataPartida { get; set; }
        public decimal? DistanciaMinima { get; set; }
        public TimeSpan? LimiteHoraPartida { get; set; }
        public TimeSpan? LimiteHoraChegada { get; set; }
        public bool? Prioritario { get; set; }
        public int? TipoCusto { get; set; }
        public int? SinalHoraPartida { get; set; }
        public TimeSpan? HoraPartida { get; set; }
        public int? SinalHoraChegada { get; set; }
        public TimeSpan? HoraChegada { get; set; }
        public bool TipoEstadia { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string UtilizadorModificacao { get; set; }
    }
}
