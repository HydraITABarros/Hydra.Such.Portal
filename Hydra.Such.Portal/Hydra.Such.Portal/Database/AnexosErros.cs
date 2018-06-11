using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
{
    public partial class AnexosErros
    {
        public int Id { get; set; }
        public int? Origem { get; set; }
        public int? Tipo { get; set; }
        public string Codigo { get; set; }
        public string NomeAnexo { get; set; }
        public byte[] Anexo { get; set; }
        public string CriadoPor { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string AlteradoPor { get; set; }
        public DateTime? DataHoraAlteracao { get; set; }
    }
}
