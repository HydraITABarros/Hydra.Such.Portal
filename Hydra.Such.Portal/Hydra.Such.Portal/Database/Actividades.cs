using System;
using System.Collections.Generic;

namespace Hydra.Such.Portal.Database
{
    public partial class Actividades
    {
        public Actividades()
        {
            ActividadesPorFornecedor = new HashSet<ActividadesPorFornecedor>();
            ActividadesPorProduto = new HashSet<ActividadesPorProduto>();
            CondicoesPropostasFornecedores = new HashSet<CondicoesPropostasFornecedores>();
            ConsultaMercado = new HashSet<ConsultaMercado>();
            HistoricoCondicoesPropostasFornecedores = new HashSet<HistoricoCondicoesPropostasFornecedores>();
            HistoricoConsultaMercado = new HashSet<HistoricoConsultaMercado>();
            HistoricoLinhasCondicoesPropostasFornecedores = new HashSet<HistoricoLinhasCondicoesPropostasFornecedores>();
            HistoricoLinhasConsultaMercado = new HashSet<HistoricoLinhasConsultaMercado>();
            HistoricoSeleccaoEntidades = new HashSet<HistoricoSeleccaoEntidades>();
            LinhasCondicoesPropostasFornecedores = new HashSet<LinhasCondicoesPropostasFornecedores>();
            LinhasConsultaMercado = new HashSet<LinhasConsultaMercado>();
            SeleccaoEntidades = new HashSet<SeleccaoEntidades>();
        }

        public string CodActividade { get; set; }
        public string Descricao { get; set; }

        public ICollection<ActividadesPorFornecedor> ActividadesPorFornecedor { get; set; }
        public ICollection<ActividadesPorProduto> ActividadesPorProduto { get; set; }
        public ICollection<CondicoesPropostasFornecedores> CondicoesPropostasFornecedores { get; set; }
        public ICollection<ConsultaMercado> ConsultaMercado { get; set; }
        public ICollection<HistoricoCondicoesPropostasFornecedores> HistoricoCondicoesPropostasFornecedores { get; set; }
        public ICollection<HistoricoConsultaMercado> HistoricoConsultaMercado { get; set; }
        public ICollection<HistoricoLinhasCondicoesPropostasFornecedores> HistoricoLinhasCondicoesPropostasFornecedores { get; set; }
        public ICollection<HistoricoLinhasConsultaMercado> HistoricoLinhasConsultaMercado { get; set; }
        public ICollection<HistoricoSeleccaoEntidades> HistoricoSeleccaoEntidades { get; set; }
        public ICollection<LinhasCondicoesPropostasFornecedores> LinhasCondicoesPropostasFornecedores { get; set; }
        public ICollection<LinhasConsultaMercado> LinhasConsultaMercado { get; set; }
        public ICollection<SeleccaoEntidades> SeleccaoEntidades { get; set; }
    }
}
