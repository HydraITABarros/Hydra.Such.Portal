using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Viaturas
{
    public class TiposViaturaViewModel : ErrorHandler
    {

        public int CodigoTipo { get; set; }
        public string Descricao { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string UtilizadorModificacao { get; set; }

        public ICollection<ViaturasViewModel> Viaturas { get; set; }

    }
}
