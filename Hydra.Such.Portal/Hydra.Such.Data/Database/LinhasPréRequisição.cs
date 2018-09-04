using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class LinhasPréRequisição
    {
        public string NºPréRequisição { get; set; }
        public int NºLinha { get; set; }
        public int? Tipo { get; set; }
        public string Código { get; set; }
        public string Descrição { get; set; }
        public string Descrição2 { get; set; }
        public string CódigoLocalização { get; set; }
        public string CódigoUnidadeMedida { get; set; }
        public decimal? QuantidadeARequerer { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public string NºProjeto { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }
        public decimal? QtdPorUnidadeMedida { get; set; }
        public decimal? QuantidadeRequerida { get; set; }
        public decimal? QuantidadePendente { get; set; }
        public decimal? CustoUnitário { get; set; }
        public decimal? PreçoUnitárioVenda { get; set; }
        public decimal? ValorOrçamento { get; set; }
        public DateTime? DataReceçãoEsperada { get; set; }
        public bool? Faturável { get; set; }
        public int? NºLinhaOrdemManutenção { get; set; }
        public string NºFuncionário { get; set; }
        public string Viatura { get; set; }
        public string NºFornecedor { get; set; }
        public string CódigoProdutoFornecedor { get; set; }
        public string UnidadeProdutivaNutrição { get; set; }
        public string NºCliente { get; set; }
        public string NºEncomendaAberto { get; set; }
        public int? NºLinhaEncomendaAberto { get; set; }
        public string LocalCompraDireta { get; set; }

        public Projetos NºProjetoNavigation { get; set; }
        public PréRequisição NºPréRequisiçãoNavigation { get; set; }
    }
}
