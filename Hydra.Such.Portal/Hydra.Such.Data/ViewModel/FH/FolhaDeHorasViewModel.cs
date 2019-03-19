using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.FH
{
    public class FolhaDeHorasViewModel : ErrorHandler
    {
        public string FolhaDeHorasNo { get; set; }
        public int? Area { get; set; }
        public string AreaTexto { get; set; }
        public string ProjetoNo { get; set; }
        public string ProjetoDescricao { get; set; }
        public string EmpregadoNo { get; set; }
        public string EmpregadoNome { get; set; }
        public DateTime? DataHoraPartida { get; set; }
        public string DataPartidaTexto { get; set; }
        public string HoraPartidaTexto { get; set; }
        public DateTime? DataHoraChegada { get; set; }
        public string DataChegadaTexto { get; set; }
        public string HoraChegadaTexto { get; set; }
        public int? TipoDeslocacao { get; set; }
        public string TipoDeslocacaoTexto { get; set; }
        public string CodigoTipoKms { get; set; }
        public string Matricula { get; set; }
        public bool? DeslocacaoForaConcelho { get; set; }
        public string DeslocacaoForaConcelhoTexto { get; set; }
        public bool? DeslocacaoPlaneada { get; set; }
        public string DeslocacaoPlaneadaTexto { get; set; }
        public bool? Terminada { get; set; }
        public string TerminadaTexto { get; set; }
        public int? Estado { get; set; }
        public string Estadotexto { get; set; }
        public string CriadoPor { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string DataCriacaoTexto { get; set; }
        public string HoraCriacaoTexto { get; set; }
        public string CodigoRegiao { get; set; }
        public string CodigoAreaFuncional { get; set; }
        public string CodigoCentroResponsabilidade { get; set; }
        public string TerminadoPor { get; set; }
        public DateTime? DataHoraTerminado { get; set; }
        public string DataTerminadoTexto { get; set; }
        public string HoraTerminadoTexto { get; set; }
        public bool? Validado { get; set; }
        public string ValidadoTexto { get; set; }
        public string Validadores { get; set; }
        public string Validador { get; set; }
        public DateTime? DataHoraValidacao { get; set; }
        public string DataValidacaoTexto { get; set; }
        public string HoraValidacaoTexto { get; set; }
        public bool? IntegradoEmRh { get; set; }
        public string IntegradoEmRhTexto { get; set; }
        public string IntegradorEmRH { get; set; }
        public string IntegradoresEmRH { get; set; }
        public DateTime? DataIntegracaoEmRH { get; set; }
        public string DataIntegracaoEmRHTexto { get; set; }
        public string HoraIntegracaoEmRHTexto { get; set; }
        public bool? IntegradoEmRhKm { get; set; }
        public string IntegradoEmRhKmTexto { get; set; }
        public string IntegradorEmRHKM { get; set; }
        public string IntegradoresEmRHKM { get; set; }
        public DateTime? DataIntegracaoEmRHKM { get; set; }
        public string DataIntegracaoEmRHKMTexto { get; set; }
        public string HoraIntegracaoEmRHKMTexto { get; set; }
        public decimal CustoTotalAjudaCusto { get; set; }
        public decimal CustoTotalHoras { get; set; }
        public decimal CustoTotalKM { get; set; }
        public decimal NumTotalKM { get; set; }
        public string Observacoes { get; set; }
        public string Responsavel1No { get; set; }
        public string Responsavel2No { get; set; }
        public string Responsavel3No { get; set; }
        public string ValidadoresRHKM { get; set; }
        public DateTime? DataHoraUltimoEstado { get; set; }
        public string DataUltimoEstadoTexto { get; set; }
        public string HoraUltimoEstadoTexto { get; set; }
        public string UtilizadorModificacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string DataModificacaoTexto { get; set; }
        public string HoraModificacaoTexto { get; set; }
        public bool? Eliminada { get; set; }
        public string Intervenientes { get; set; }

        public bool MostrarBotoes { get; set; }
        public bool MostrarReportAjCustosRH { get; set; }
        

        public Object Colunas { get; set; }



        public List<LinhasFolhaHorasViewModel> FolhaDeHorasPercurso { get; set; }
        public List<LinhasFolhaHorasViewModel> FolhaDeHorasAjuda { get; set; }
        public List<MaoDeObraFolhaDeHorasViewModel> FolhaDeHorasMaoDeObra { get; set; }
        public List<PresencasFolhaDeHorasViewModel> FolhaDeHorasPresenca { get; set; }
    }
}
