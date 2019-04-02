using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.Database
{
    public partial class ContactosServicos
    {
        public int ID { get; set; }
        public string Servico { get; set; }
        public Boolean? Activo { get; set; }
        public string CriadoPor { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string AlteradoPor { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}
