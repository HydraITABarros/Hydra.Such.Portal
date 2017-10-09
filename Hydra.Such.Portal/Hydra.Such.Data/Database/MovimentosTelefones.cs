using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class MovimentosTelefones
    {
        public int Id { get; set; }
        public string NºTelefone { get; set; }
        public DateTime? Data { get; set; }
        public string NºFatura { get; set; }
        public string Período { get; set; }
        public decimal? ValorSemIva { get; set; }
        public decimal? ValorComIva { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public string Fornecedor { get; set; }
        public bool? MovimentoManual { get; set; }
        public int? TipoCusto { get; set; }
        public string NºFaturaNav { get; set; }
        public string UtilizadorModificação { get; set; }
        public DateTime? DataHoraModificação { get; set; }

        public Telefones NºTelefoneNavigation { get; set; }
    }
}
