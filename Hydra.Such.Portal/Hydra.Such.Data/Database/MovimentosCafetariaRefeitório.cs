using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class MovimentosCafetariaRefeitório
    {
        public int NºMovimento { get; set; }
        public int? CódigoCafetariaRefeitório { get; set; }
        public int? NºUnidadeProdutiva { get; set; }
        public int? Tipo { get; set; }
        public DateTime? DataRegisto { get; set; }
        public string NºRecurso { get; set; }
        public string Descrição { get; set; }
        public decimal? Valor { get; set; }
        public int? TipoMovimento { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public string Utilizador { get; set; }
        public DateTime? DataHoraSistemaRegisto { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }
        public decimal? Quantidade { get; set; }
        public int? TipoRefeição { get; set; }
        public string DescriçãoTipoRefeição { get; set; }

        public UnidadesProdutivas NºUnidadeProdutivaNavigation { get; set; }
    }
}
