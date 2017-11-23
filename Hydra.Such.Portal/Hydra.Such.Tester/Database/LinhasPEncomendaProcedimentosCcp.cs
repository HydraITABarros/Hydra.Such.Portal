using System;
using System.Collections.Generic;

namespace Hydra.Such.Tester.Database
{
    public partial class LinhasPEncomendaProcedimentosCcp
    {
        public string NºProcedimento { get; set; }
        public int NºLinha { get; set; }
        public int? Tipo { get; set; }
        public string Código { get; set; }
        public string CódLocalização { get; set; }
        public string Descrição { get; set; }
        public string CódUnidadeMedida { get; set; }
        public decimal? CustoUnitário { get; set; }
        public decimal? QuantARequerer { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public string NºProjeto { get; set; }
        public string NºRequisição { get; set; }
        public int? NºLinhaRequisição { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }

        public LinhasRequisição Nº { get; set; }
        public ProcedimentosCcp NºProcedimentoNavigation { get; set; }
        public Requisição NºRequisiçãoNavigation { get; set; }
    }
}
