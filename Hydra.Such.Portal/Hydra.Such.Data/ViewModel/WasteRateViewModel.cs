using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class WasteRateViewModel
    {
        public string Recurso { get; set; }
        public string RecursoName { get; set; }
        public string FamiliaRecurso { get; set; }
        public string Data { get; set; }
        public string DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public string DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }
        public bool Selected { get; set; }
    }
}
