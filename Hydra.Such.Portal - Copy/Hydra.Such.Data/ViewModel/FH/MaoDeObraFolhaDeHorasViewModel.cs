using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.FH
{
    public class MaoDeObraFolhaDeHorasViewModel
    {
        public string FolhaDeHorasNo { get; set; }
        public int? LinhaNo { get; set; }
        public DateTime? Date { get; set; }
        public string EmpregadoNo { get; set; }
        public string ProjetoNo { get; set; }
        public int? CodigoTipoTrabalho { get; set; }
        public DateTime? HoraInicio { get; set; }
        public string HoraInicioTexto { get; set; }
        public DateTime? HoraFim { get; set; }
        public string HoraFimTexto { get; set; }
        public Boolean? HorarioAlmoco { get; set; }
        public Boolean? HorarioJantar { get; set; }
        public string CodigoFamiliaRecurso { get; set; }
        public string RecursoNo { get; set; }
        public string CodigoUnidadeMedida { get; set; }
        public int? CodigoTipoOM { get; set; }
        public DateTime? HorasNo { get; set; }
        public string HorasNoTexto { get; set; }
        public decimal CustoUnitarioDireto { get; set; }
        public decimal PrecoDeCusto { get; set; }
        public decimal PrecoDeVenda { get; set; }
        public decimal PrecoTotal { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string DataHoraCriacaoTexto { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string DataHoraModificacaoTexto { get; set; }
        public string UtilizadorModificacao { get; set; }
    }
}
