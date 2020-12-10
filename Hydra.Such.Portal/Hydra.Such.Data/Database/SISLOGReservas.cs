using System;

namespace Hydra.Such.Data.Database
{
    public partial class SISLOGReservas
    {
        public string NoRequisicao { get; set; }
        public int NoLinha { get; set; }
        public string NoProduto { get; set; }
        public decimal? QuantidadeReserva { get; set; }
        public DateTime? DataInicioReserva { get; set; }
        public DateTime? DataFimReserva { get; set; }
        public bool? Reservado { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
    }
}
