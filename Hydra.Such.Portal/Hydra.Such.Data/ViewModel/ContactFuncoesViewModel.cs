using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public partial class ContactFuncoesViewModel
    {
        public int ID { get; set; }
        public string Funcao { get; set; }
        public Boolean? Activo { get; set; }
        public string CriadoPor { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string DataCriacaoText { get; set; }
        public string AlteradoPor { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string DataAlteracaoText { get; set; }
    }
}
