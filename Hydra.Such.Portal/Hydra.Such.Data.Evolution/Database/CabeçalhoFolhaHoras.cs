using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class CabeçalhoFolhaHoras
    {
        public byte[] Timestamp { get; set; }
        public string NºRecurso { get; set; }
        public string NºEmpregado { get; set; }
        public string GlobalDimension1Code { get; set; }
        public string GlobalDimension2Code { get; set; }
        public string ShortcutDimension3Code { get; set; }
        public string ShortcutDimension4Code { get; set; }
        public DateTime DataPartida { get; set; }
        public DateTime HoraPartida { get; set; }
        public DateTime DataChegada { get; set; }
        public DateTime HoraChegada { get; set; }
        public string CódigoTipoKm { get; set; }
        public byte ViaturaPrópria { get; set; }
        public int TipoObra { get; set; }
        public string NºObra { get; set; }
        public string CódFaseProjecto { get; set; }
        public string CódSubfaseProjecto { get; set; }
        public string CódTarefaProjecto { get; set; }
        public int NºLinhaOrdemManut { get; set; }
        public int Estado { get; set; }
        public DateTime DataCriação { get; set; }
        public DateTime DataRegisto { get; set; }
        public string RegistadoPor { get; set; }
        public DateTime DataValidação { get; set; }
        public DateTime DataInvalidação { get; set; }
        public string Validador { get; set; }
        public byte IntegradoEmHr { get; set; }
        public DateTime DataIntegraçãoEmHr { get; set; }
        public string IntegradorEmHr { get; set; }
        public string Perfil { get; set; }
        public int NºLinhaTarefaOrdemManut { get; set; }
        public byte AjudasCalculadas { get; set; }
        public string Responsaveis { get; set; }
        public string NºResponsavel1 { get; set; }
        public string NºResponsavel2 { get; set; }
        public string NºResponsavel3 { get; set; }
        public byte BloqueadoParaSubsidio { get; set; }
        public byte IntegradoSubsidioHr { get; set; }
        public byte DeslocacaoConcelho { get; set; }
        public string ProdPostingGroup { get; set; }
        public string CriadaPor { get; set; }
        public string ValidadoresRh { get; set; }
        public string MotivoInvalidação { get; set; }
        public byte Terminada { get; set; }
        public string Observações { get; set; }
        public byte Validado { get; set; }
        public long NºFolhaHoras { get; set; }
        public DateTime HoraCriação { get; set; }
        public DateTime HoraValidação { get; set; }
        public DateTime HoraIntegraçãoEmHr { get; set; }
        public string ValidadoresRhKm { get; set; }
        public byte IntegradoSubsidioHrkm { get; set; }
        public string IntegradorEmHrKm { get; set; }
        public DateTime DataIntegraçãoEmHrKm { get; set; }
        public DateTime HoraIntegraçãoEmHrKm { get; set; }
        public string Visualizadores { get; set; }
        public string TerminadoPor { get; set; }
        public DateTime? DataTerminado { get; set; }
        public int? TipoDeslocacao { get; set; }
        public string Matricula { get; set; }
        public byte? DeslocaçãoPlaneada { get; set; }
    }
}
