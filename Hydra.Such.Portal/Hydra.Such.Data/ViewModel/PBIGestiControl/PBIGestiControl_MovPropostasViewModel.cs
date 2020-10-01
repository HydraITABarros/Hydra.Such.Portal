using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.PBIGestiControl
{
    public class PBIGestiControl_MovPropostasViewModel : ErrorHandler
    {
        public string ID { get; set; }
        public string CResp { get; set; }
        public string Data { get; set; }
        public string NumPropostas { get; set; }
        public string NumRevistas { get; set; }
        public string NumGanhas { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }
    }
}
