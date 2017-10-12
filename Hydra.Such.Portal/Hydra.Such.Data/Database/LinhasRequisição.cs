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

        public Projetos NºProjetoNavigation { get; set; }
        public Requisição NºRequisiçãoNavigation { get; set; }
        public Viaturas ViaturaNavigation { get; set; }
        public ICollection<DiárioDeProjeto> DiárioDeProjeto { get; set; }
        public ICollection<LinhasPEncomendaProcedimentosCcp> LinhasPEncomendaProcedimentosCcp { get; set; }
    }
}
