using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;

namespace Hydra.Such.Data.ViewModel.CCP
{
    public class LinhasParaEncomendaCCPView
    {
        public string NoProcedimento { get; set; }
        public int NoLinha { get; set; }
        public int? Tipo { get; set; }
        public string Codigo { get; set; }
        public string CodLocalizacao { get; set; }
        public string Descricao { get; set; }
        public string CodUnidadeMedida { get; set; }
        public decimal? CustoUnitário { get; set; }
        public decimal? QuantARequerer { get; set; }
        public string CodigoRegiao { get; set; }
        public string CodigoAreaFuncional { get; set; }
        public string CodigoCentroResponsabilidade { get; set; }
        public string NoProjeto { get; set; }
        public string NoRequisicao { get; set; }
        public int? NoLinhaRequisicao { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string UtilizadorModificacao { get; set; }

        public LinhasRequisição Nº { get; set; }
        public ProcedimentosCcp NºProcedimentoNavigation { get; set; }
        public Requisição NºRequisiçãoNavigation { get; set; }
    }
}
