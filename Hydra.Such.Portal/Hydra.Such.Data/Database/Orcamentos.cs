using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.Database
{
    public partial class Orcamentos
    {
        public Orcamentos()
        {
            LinhasOrcamentos = new HashSet<LinhasOrcamentos>();
        }

        public string No { get; set; }
        public string NoCliente { get; set; }
        public string NoContacto { get; set; }
        public string ContactoNome { get; set; }
        public string ContactoTelefone { get; set; }
        public string ContactoEmail { get; set; }
        public string ContactoNotas { get; set; }
        public DateTime? DataValidade { get; set; }
        public int? IDEstado { get; set; }
        public string Descricao { get; set; }
        public string CodRegiao { get; set; }
        public int? UnidadePrestacao { get; set; }
        public int? TipoFaturacao { get; set; }
        public int? CondicoesPagamento { get; set; }
        public decimal? TotalSemIVA { get; set; }
        public decimal? TotalComIVA { get; set; }
        public string NoProposta { get; set; }
        public string ProjetoAssociado { get; set; }
        public string Email { get; set; }
        public string EmailAssunto { get; set; }
        public string EmailCorpo { get; set; }
        public DateTime? EmailDataEnvio { get; set; }
        public string EmailUtilizadorEnvio { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataAceite { get; set; }
        public string UtilizadorAceite { get; set; }
        public DateTime? DataNaoAceite { get; set; }
        public string UtilizadorNaoAceite { get; set; }
        public DateTime? DataConcluido { get; set; }
        public string UtilizadorConcluido { get; set; }
        public DateTime? DataModificacao { get; set; }
        public string UtilizadorModificacao { get; set; }

        public ICollection<LinhasOrcamentos> LinhasOrcamentos { get; set; }
    }
}
