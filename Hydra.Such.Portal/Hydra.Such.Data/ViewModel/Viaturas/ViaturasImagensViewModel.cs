using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Viaturas
{
    public class ViaturasImagensViewModel
    {
        public string Matricula { get; set; }
        public byte[] Imagem { get; set; }
        public string DataHoraCriacao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public string DataHoraModificacao { get; set; }
        public string UtilizadorModificacao { get; set; }
    }
}
