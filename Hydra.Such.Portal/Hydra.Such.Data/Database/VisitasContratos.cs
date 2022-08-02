using System;

namespace Hydra.Such.Data.Database
{
    public partial class VisitasContratos
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
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
    }
}
