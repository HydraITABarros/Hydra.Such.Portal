using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class RececaoFaturacaoWorkflow
    {
        public int Id { get; set; }
        public int? Estado { get; set; }
        public string AreaWorkflow { get; set; }
        public string Descricao { get; set; }
        public DateTime? Data { get; set; }
        public string Utilizador { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string CriadoPor { get; set; }
        public DateTime? DataModificacao { get; set; }
        public string ModificadoPor { get; set; }
    }
}
