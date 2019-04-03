using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class Contactos
    {
        public string No { get; set; }
        public string NoCliente { get; set; }
        public int? NoServico { get; set; }
        public int? NoFuncao { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Telemovel { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Pessoa { get; set; }
        public string Notas { get; set; }
        public string CriadoPor { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string AlteradoPor { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}
