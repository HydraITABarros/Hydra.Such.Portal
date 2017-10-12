using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class MovimentosViaturas
    {
        public int NºMovimento { get; set; }
        public string Matrícula { get; set; }
        public int? TipoMovimento { get; set; }
        public DateTime? DataHoraMovimento { get; set; }
        public decimal? Quantidade { get; set; }
        public decimal? Valor { get; set; }
        public string CartãoCombustível { get; set; }
        public string Apólice { get; set; }
        public string NºFuncionário { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public int? Kms { get; set; }
        public decimal? Consumo { get; set; }
        public decimal? CustoUnitário { get; set; }
        public string NºDocumento { get; set; }
        public string NºFornecedor { get; set; }
        public string NºRecurso { get; set; }
        public string Descrição { get; set; }
        public DateTime? DataRegisto { get; set; }
        public string LocalidadePostoCombustível { get; set; }
        public string NomePostoCombustível { get; set; }
        public decimal? Nd { get; set; }
        public bool? Corrigido { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }

        public Viaturas MatrículaNavigation { get; set; }
    }
}
