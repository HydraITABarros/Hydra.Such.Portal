using System;
using System.Collections.Generic;


namespace Hydra.Such.Data.Database
{
    public partial class TipoTrabalhoFh
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string CodUnidadeMedida { get; set; }
        public bool? HoraViagem { get; set; }
        public int? TipoHora { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
    }
}
