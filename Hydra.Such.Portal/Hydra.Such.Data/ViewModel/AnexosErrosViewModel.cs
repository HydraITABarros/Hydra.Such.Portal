using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class AnexosErrosViewModel
    {
        public int ID { get; set; }
        public string CodeTexto { get; set; }
        public int Origem { get; set; }
        public string OrigemTexto { get; set; }
        public int Tipo { get; set; }
        public string TipoTexto { get; set; }
        public string Codigo { get; set; }
        public string NomeAnexo { get; set; }
        public byte[] Anexo { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoPorNome { get; set; }
        public DateTime? DataHora_Criacao { get; set; }
        public string DataHora_CriacaoTexto { get; set; }
        public string AlteradoPor { get; set; }
        public string AlteradoPorNome { get; set; }
        public DateTime? DataHora_Alteracao { get; set; }
        public string DataHora_AlteracaoTexto { get; set; }
    }
}
