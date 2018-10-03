using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Compras
{
    public class CompradorViewModel : ErrorHandler
    {
        public string CodComprador { get; set; }
        public string NomeComprador { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
    }
}
