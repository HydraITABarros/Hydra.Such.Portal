using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.Database
{
    public partial class AnexosErros
    {
        public int ID { get; set; }
        public int Origem { get; set; }
        public int Tipo { get; set; }
        public string Codigo { get; set; }
        public string NomeAnexo { get; set; }
        public byte[] Anexo { get; set; }
        public string CriadoPor { get; set; }
        public DateTime? DataHora_Criacao { get; set; }
        public string AlteradoPor { get; set; }
        public DateTime? DataHora_Alteracao { get; set; }
    }
}
