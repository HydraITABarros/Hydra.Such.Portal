using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class LinhasRequisição
    {
        public LinhasRequisição()
        {
            DiárioDeProjeto = new HashSet<DiárioDeProjeto>();
            LinhasPEncomendaProcedimentosCcp = new HashSet<LinhasPEncomendaProcedimentosCcp>();
            MovimentosDeProjeto = new HashSet<MovimentosDeProjeto>();
        }

        public string NºRequisição { get; set; }
        public int NºLinha { get; set; }
        public int? Tipo { get; set; }
        public string Código { get; set; }
        public string Descrição { get; set; }
        public string CódigoUnidadeMedida { get; set; }
        public string CódigoLocalização { get; set; }
        public bool? MercadoLocal { get; set; }
        public decimal? QuantidadeARequerer { get; set; }
        public decimal? QuantidadeRequerida { get; set; }
        public decimal? QuantidadeADisponibilizar { get; set; }
        public decimal? QuantidadeDisponibilizada { get; set; }
        public decimal? QuantidadeAReceber { get; set; }
        public decimal? QuantidadeRecebida { get; set; }
        public decimal? QuantidadePendente { get; set; }
        public decimal? CustoUnitário { get; set; }
        public DateTime? DataReceçãoEsperada { get; set; }
        public bool? Faturável { get; set; }
        public string NºProjeto { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public string NºFuncionário { get; set; }
        public string Viatura { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }
        public decimal? QtdPorUnidadeDeMedida { get; set; }
        public decimal? PreçoUnitárioVenda { get; set; }
        public decimal? ValorOrçamento { get; set; }
        public int? NºLinhaOrdemManutenção { get; set; }
        public bool? CriarConsultaMercado { get; set; }
        public bool? EnviarPréCompra { get; set; }
        public bool? EnviadoPréCompra { get; set; }
        public DateTime? DataMercadoLocal { get; set; }
        public string UserMercadoLocal { get; set; }
        public bool? EnviadoParaCompras { get; set; }
        public DateTime? DataEnvioParaCompras { get; set; }
        public bool? ValidadoCompras { get; set; }
        public bool? RecusadoCompras { get; set; }
        public string MotivoRecusaMercLocal { get; set; }
        public DateTime? DataRecusaMercLocal { get; set; }
        public int? IdCompra { get; set; }
        public string NºFornecedor { get; set; }
        public string NºEncomendaAberto { get; set; }
        public int? NºLinhaEncomendaAberto { get; set; }
        public string NºDeConsultaMercadoCriada { get; set; }
        public string NºEncomendaCriada { get; set; }
        public string CódigoProdutoFornecedor { get; set; }
        public string UnidadeProdutivaNutrição { get; set; }
        public string RegiãoMercadoLocal { get; set; }
        public string NºCliente { get; set; }
        public string Aprovadores { get; set; }

        public Projetos NºProjetoNavigation { get; set; }
        public Requisição NºRequisiçãoNavigation { get; set; }
        public Viaturas ViaturaNavigation { get; set; }
        public ICollection<DiárioDeProjeto> DiárioDeProjeto { get; set; }
        public ICollection<LinhasPEncomendaProcedimentosCcp> LinhasPEncomendaProcedimentosCcp { get; set; }
        public ICollection<MovimentosDeProjeto> MovimentosDeProjeto { get; set; }
    }
}
