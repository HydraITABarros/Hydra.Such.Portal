using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.FH
{
    public partial class OrigemDestinoFHViewModel
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public bool? RegiaoAutonoma { get; set; }
        public string CriadoPor { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string AlteradoPor { get; set; }
        public DateTime? DataHoraUltimaAlteracao { get; set; }

    }
}
