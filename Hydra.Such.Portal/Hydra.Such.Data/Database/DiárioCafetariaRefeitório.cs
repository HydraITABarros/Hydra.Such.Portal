using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class DiárioCafetariaRefeitório
    {
        public int NºLinha { get; set; }
        public int? CódigoCafetariaRefeitório { get; set; }
        public DateTime? DataRegisto { get; set; }
        public string NºRecurso { get; set; }
        public string Descrição { get; set; }
        public decimal? Valor { get; set; }
        public string NºProjeto { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public decimal? Quantidade { get; set; }
        public int? NºUnidadeProdutiva { get; set; }
        public int? TipoRefeição { get; set; }
        public int? TipoMovimento { get; set; }
        public string Utilizador { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }

        public Projetos NºProjetoNavigation { get; set; }
        public UnidadesProdutivas NºUnidadeProdutivaNavigation { get; set; }
        public TiposRefeição TipoRefeiçãoNavigation { get; set; }
    }
}
