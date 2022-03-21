using System;

namespace Hydra.Such.Data.ViewModel.VisitasVM
{
    public class VisitasViewModel: ErrorHandler
    {
        public string CodVisita { get; set; }
        public string Objetivo { get; set; }
        public string Local { get; set; }
        public string CodCliente { get; set; }
        public string NomeCliente { get; set; }
        public string CodFornecedor { get; set; }
        public string NomeFornecedor { get; set; }
        public string Entidade { get; set; }
        public string CodRegiao { get; set; }
        public string NomeRegiao { get; set; }
        public string CodArea { get; set; }
        public string NomeArea { get; set; }
        public string CodCresp { get; set; }
        public string NomeCresp { get; set; }
        public DateTime? InicioData { get; set; }
        public string InicioDataTexto { get; set; }
        public TimeSpan? InicioHora { get; set; }
        public string InicioHoraTexto { get; set; }
        public DateTime? FimData { get; set; }
        public string FimDataTexto { get; set; }
        public TimeSpan? FimHora { get; set; }
        public string FimHoraTexto { get; set; }
        public int? CodEstado { get; set; }
        public string NomeEstado { get; set; }
        public string IniciativaCriador { get; set; }
        public string IniciativaCriadorNome { get; set; }
        public string IniciativaResponsavel { get; set; }
        public string IniciativaResponsavelNome { get; set; }
        public string IniciativaIntervinientes { get; set; }
        public string RececaoCriador { get; set; }
        public string RececaoResponsavel { get; set; }
        public string RececaoIntervinientes { get; set; }
        public string RelatorioSimplificado { get; set; }
        public string UtilizadorCriacao { get; set; }
        public string UtilizadorCriacaoNome { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string DataHoraCriacaoTexto { get; set; }
        public string UtilizadorModificacao { get; set; }
        public string UtilizadorModificacaoNome { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string DataHoraModificacaoTexto { get; set; }
        public string UserLogin { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }
    }
}
