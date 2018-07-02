using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Compras
{
    public class BillingRecWorkflowModel
    {
        public int Id { get; set; }
        public string IdRecFaturacao { get; set; }
        public Enumerations.BillingReceptionStates Estado { get; set; }
        public string AreaWorkflow { get; set; }
        public string Descricao { get; set; }
        public DateTime? Data { get; set; }
        public string Utilizador { get; set; }
        public string CodTipoProblema { get; set; }
        public int? CodProblema { get; set; }
        public string EnderecoEnvio { get; set; }
        public string EnderecoFornecedor { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string CriadoPor { get; set; }
        public DateTime? DataModificacao { get; set; }
        public string ModificadoPor { get; set; }
    }
}
