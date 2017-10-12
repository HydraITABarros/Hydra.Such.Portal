using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class LinhasRequisiçõesSimplificadas
    {
        public string NºRequisição { get; set; }
        public int NºLinha { get; set; }
        public int? Tipo { get; set; }
        public string Código { get; set; }
        public string CódLocalização { get; set; }
        public int? Estado { get; set; }
        public string Descrição { get; set; }
        public string CódUnidadeMedida { get; set; }
        public decimal? QuantidadeARequerer { get; set; }
        public decimal? QuantidadeAprovada { get; set; }
        public decimal? QuantidadeRecebida { get; set; }
        public decimal? QuantidadeAAprovar { get; set; }
        public decimal? CustoTotal { get; set; }
        public string NºProjeto { get; set; }
        public int? TipoRefeição { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public string NºFuncionário { get; set; }
        public decimal? CustoUnitário { get; set; }
        public DateTime? DataRequisição { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }

        public Projetos NºProjetoNavigation { get; set; }
        public RequisiçõesSimplificadas NºRequisiçãoNavigation { get; set; }
        public TiposRefeição TipoRefeiçãoNavigation { get; set; }
    }
}
