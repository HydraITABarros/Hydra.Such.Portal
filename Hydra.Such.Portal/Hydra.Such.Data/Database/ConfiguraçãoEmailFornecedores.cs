using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class ConfiguraçãoEmailFornecedores
    {
        public string CodFornecedor { get; set; }
        public string Nome { get; set; }
        public string Cresp { get; set; }
        public string Email { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
    }
}
