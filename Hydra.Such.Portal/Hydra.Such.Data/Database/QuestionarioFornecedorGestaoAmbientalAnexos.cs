using System;

namespace Hydra.Such.Data.Database
{
    public partial class QuestionarioFornecedorGestaoAmbientalAnexos
    {
        public int ID { get; set; }
        public string Codigo { get; set; }
        public int Versao { get; set; }
        public string ID_Fornecedor { get; set; }
        public string URL_Anexo { get; set; }
        public bool? Visivel { get; set; }
        public DateTime? DataHora_Criacao { get; set; }
        public string Utilizador_Criacao { get; set; }
        public DateTime? DataHora_Modificacao { get; set; }
        public string Utilizador_Modificacao { get; set; }
    }
}
