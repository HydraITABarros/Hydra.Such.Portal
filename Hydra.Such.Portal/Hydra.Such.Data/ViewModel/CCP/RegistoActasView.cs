using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.CCP
{
    public class RegistoActasView
    {
        public string NumProcedimento { get; set; }
        public string NumActa { get; set; }
        public DateTime? DataDaActa { get; set; }
        public string Observacoes { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string UtilizadorModificacao { get; set; }

        public string DataDaActa_Show { get; set; }
    }
}
