using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.PBIGestiControl
{
    public class PBIGestiControl_MovProducaoCRespViewModel: ErrorHandler
    {
        public string ID { get; set; }
        public string IdCResp { get; set; }
        public string DataPro { get; set; }
        public string NumContratos { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }
    }
}
