using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.ProjectView
{
    public class ContabGroupTypesProjectView
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public string Region { get; set; }
        public string FunctionalAreaCode { get; set; }
        public string ResponsabilityCenter { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string UtilizadorModificacao { get; set; }

    }
}
