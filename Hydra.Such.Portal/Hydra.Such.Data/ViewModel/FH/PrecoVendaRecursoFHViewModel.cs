using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.ViewModel.FH
{
    public partial class PrecoVendaRecursoFHViewModel
    {
        public string ORIGINAL_Code { get; set; }
        public string Code { get; set; }
        public string Descricao { get; set; }
        public string ORIGINAL_CodTipoTrabalho { get; set; }
        public string CodTipoTrabalho { get; set; }
        public string CodTipoTrabalhoTexto { get; set; }
        public decimal? PrecoUnitario { get; set; }
        public string PrecoUnitarioTexto { get; set; }
        public decimal? CustoUnitario { get; set; }
        public string CustoUnitarioTexto { get; set; }
        public DateTime? ORIGINAL_StartingDate { get; set; }
        public string ORIGINAL_StartingDateTexto { get; set; }
        public DateTime? StartingDate { get; set; }
        public string StartingDateTexto { get; set; }
        public DateTime? EndingDate { get; set; }
        public string EndingDateTexto { get; set; }
        public string FamiliaRecurso { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
    }
}
