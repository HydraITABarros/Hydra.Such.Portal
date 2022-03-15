using System;

namespace Hydra.Such.Data.ViewModel.VisitasVM
{
    public class VisitasViewModel: ErrorHandler
    {
        public int ID { get; set; }
        public string CodVisita { get; set; }
        public string Objetivo { get; set; }
        public string Local { get; set; }
        public string CodCliente { get; set; }
        public string ClienteTexto { get; set; }
        public string CodFornecedor { get; set; }
        public string FornecedorTexto { get; set; }
        public string Entidade { get; set; }
        public string CodRegiao { get; set; }
        public string RegiaoTexto { get; set; }
        public string CodArea { get; set; }
        public string AreaTexto { get; set; }
        public string CodCresp { get; set; }
        public string CrespTexto { get; set; }
        public DateTime? InicioDataHora { get; set; }
        public string InicioDataHoraTexto { get; set; }
        public string InicioDataTexto { get; set; }
        public string InicioHoraTexto { get; set; }
        public DateTime? FimDataHora { get; set; }
        public string FimDataHoraTexto { get; set; }
        public string FimDataTexto { get; set; }
        public string FimHoraTexto { get; set; }
        public int? CodEstado { get; set; }
        public string EstadoTexto { get; set; }
        public string IniciativaCriador { get; set; }
        public string IniciativaCriadorTexto { get; set; }
        public string IniciativaResponsavel { get; set; }
        public string IniciativaResponsavelTexto { get; set; }
        public string IniciativaIntervinientes { get; set; }
        public string RececaoCriador { get; set; }
        public string RececaoResponsavel { get; set; }
        public string RececaoIntervinientes { get; set; }
        public string RelatorioSimplificado { get; set; }
        public string UtilizadorCriacao { get; set; }
        public string UtilizadorCriacaoTexto { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string DataHoraCriacaoTexto { get; set; }
        public string UtilizadorModificacao { get; set; }
        public string UtilizadorModificacaoTexto { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string DataHoraModificacaoTexto { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }
    }
}
