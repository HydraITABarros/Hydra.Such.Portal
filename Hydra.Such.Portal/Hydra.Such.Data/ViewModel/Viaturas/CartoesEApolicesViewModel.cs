using Hydra.Such.Data.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Viaturas
{
    public class CartoesEApolicesViewModel : ErrorHandler
    {
        public int Tipo { get; set; }
        public string Numero { get; set; }
        public string DataInicio { get; set; }
        public string DataFim { get; set; }
        public string Descricao { get; set; }
        public string Fornecedor { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
    }
}
