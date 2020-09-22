using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.PBIGestiControl
{
    public class PBIGestiControl_MovProducaoCRespViewModel: ErrorHandler
    {
        public string ID { get; set; }
        public string IdArea { get; set; }
        public string Area { get; set; }
        public string IdIndicador { get; set; }
        public string Indicador { get; set; }
        public DateTime DataPro { get; set; }
        public decimal VProdGrafico { get; set; }
    }
}
