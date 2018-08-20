using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class RececaoFaturacaoWorkflow
    {
        public int Id { get; set; }
        public string IdRecFaturacao { get; set; }
        public int? Estado { get; set; }
        public string CodTipoProblema { get; set; }
        public string CodProblema { get; set; }
        public string AreaWorkflow { get; set; }
        public string Area { get; set; }
        public string CodDestino { get; set; }
        public string Destinatario { get; set; }
        public string Descricao { get; set; }
        public string Comentario { get; set; }
        public DateTime? Data { get; set; }
        public string Utilizador { get; set; }
        public string EnderecoEnvio { get; set; }
        public string EnderecoFornecedor { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string CriadoPor { get; set; }
        public DateTime? DataModificacao { get; set; }
        public string ModificadoPor { get; set; }
        public bool? Anexo { get; set; }

        public RecFacturasProblemas Cod { get; set; }
        public RececaoFaturacao IdRecFaturacaoNavigation { get; set; }
    }
}
