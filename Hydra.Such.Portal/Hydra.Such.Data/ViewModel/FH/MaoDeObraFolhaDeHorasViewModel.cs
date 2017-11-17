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
        public string DateTexto { get; set; }
        public string ProjetoNo { get; set; }
        public string EmpregadoNo { get; set; }
        public int? CodigoTipoTrabalho { get; set; }
        public string HoraInicio { get; set; }
        public string HoraInicioTexto { get; set; }
        public Boolean? HorarioAlmoco { get; set; }
        public string HoraFim { get; set; }
        public string HoraFimTexto { get; set; }
        public Boolean? HorarioJantar { get; set; }
        public string CodigoFamiliaRecurso { get; set; }
        public int? CodigoTipoOM { get; set; }
        public string HorasNo { get; set; }
        public string HorasNoTexto { get; set; }
        public decimal CustoUnitarioDireto { get; set; }
        public string CodigoCentroResponsabilidade { get; set; }
        public decimal PrecoTotal { get; set; }
        public string Descricao { get; set; }
        public string RecursoNo { get; set; }
        public string CodigoUnidadeMedida { get; set; }
        public decimal PrecoDeCusto { get; set; }
        public decimal PrecoDeVenda { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string DataHoraCriacaoTexto { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string DataHoraModificacaoTexto { get; set; }
    }
}
