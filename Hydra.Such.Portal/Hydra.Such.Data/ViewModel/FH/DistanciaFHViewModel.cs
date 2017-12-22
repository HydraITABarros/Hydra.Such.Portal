using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.FH
{
    public partial class DistanciaFHViewModel
    {
        public string Origem { get; set; }
        public string Destino { get; set; }
        public decimal? Distancia { get; set; }
        public string CriadoPor { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string AlteradoPor { get; set; }
        public DateTime? DataHoraUltimaAlteracao { get; set; }
    }
}
