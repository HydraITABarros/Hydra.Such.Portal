using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.FolhasDeHoras
{
    public class FolhaDeHoraDetailsViewModel : ErrorHandler
    {
        public string FolhaDeHoraNo { get; set; }
        public int? Area { get; set; }
        public string ProjectNo { get; set; }
        public string EmployeeNo { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public int? TipoDeslocação { get; set; }
        public string CodeTypeKms { get; set; }
        public bool? DeslocaçãoForaConcelho { get; set; }
        public string Validadores { get; set; }
        public int? Status { get; set; }
        public string CriadoPor { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public DateTime? DataHoraÚltimoEstado { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }
        public string UtilizadorCriação { get; set; }
    }
}
