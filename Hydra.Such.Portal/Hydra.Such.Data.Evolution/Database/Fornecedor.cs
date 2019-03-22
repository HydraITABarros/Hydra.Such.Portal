using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class Fornecedor
    {
        public Fornecedor()
        {
            Equipamento = new HashSet<Equipamento>();
        }

        public string IdFornecedor { get; set; }
        public string Nome { get; set; }
        public string Morada { get; set; }
        public string CodPostal { get; set; }
        public string Telefone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string NomeContacto { get; set; }
        public string TelefoneContacto { get; set; }
        public string Nif { get; set; }
        public bool? Activo { get; set; }

        public ICollection<Equipamento> Equipamento { get; set; }
    }
}
