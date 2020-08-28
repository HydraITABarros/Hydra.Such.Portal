using System;

namespace Hydra.Such.Data.Database
{
    public partial class OrcamentosContatos
    {
        public string ID { get; set; }
        public string Organizacao { get; set; }
        public string Nome { get; set; }
        public string Telemovel { get; set; }
        public string Email { get; set; }
        public string NIF { get; set; }
        public string Notas { get; set; }
        public string CriadoPor { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string AlteradoPor { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}
