using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class FolhasDeHoras
    {
        public FolhasDeHoras()
        {
            DistribuiçãoCustoFolhaDeHoras = new HashSet<DistribuiçãoCustoFolhaDeHoras>();
            MãoDeObraFolhaDeHoras = new HashSet<MãoDeObraFolhaDeHoras>();
            PercursosEAjudasCustoDespesasFolhaDeHoras = new HashSet<PercursosEAjudasCustoDespesasFolhaDeHoras>();
            PresençasFolhaDeHoras = new HashSet<PresençasFolhaDeHoras>();
        }

        public string NºFolhaDeHoras { get; set; }
        public int? Área { get; set; }
        public string NºProjeto { get; set; }
        public string NºEmpregado { get; set; }
        public DateTime? DataHoraPartida { get; set; }
        public DateTime? DataHoraChegada { get; set; }
        public int? TipoDeslocação { get; set; }
        public string CódigoTipoKmS { get; set; }
        public bool? DeslocaçãoForaConcelho { get; set; }
        public string Validadores { get; set; }
        public int? Estado { get; set; }
        public string CriadoPor { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public DateTime? DataHoraÚltimoEstado { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }
        public string NomeEmpregado { get; set; }
        public string Matrícula { get; set; }
        public bool? Terminada { get; set; }
        public string TerminadoPor { get; set; }
        public DateTime? DataHoraTerminado { get; set; }
        public bool? Validado { get; set; }
        public bool? DeslocaçãoPlaneada { get; set; }
        public string Observações { get; set; }
        public string NºResponsável1 { get; set; }
        public string NºResponsável2 { get; set; }
        public string NºResponsável3 { get; set; }
        public string ValidadoresRhKm { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public string Validador { get; set; }
        public DateTime? DataHoraValidação { get; set; }
        public string IntegradorEmRh { get; set; }
        public DateTime? DataIntegraçãoEmRh { get; set; }
        public string IntegradorEmRhKm { get; set; }
        public DateTime? DataIntegraçãoEmRhKm { get; set; }
        public string ProjetoDescricao { get; set; }
        public string IntegradoresEmRh { get; set; }
        public string IntegradoresEmRhkm { get; set; }
        public decimal? CustoTotalAjudaCusto { get; set; }
        public decimal? CustoTotalHoras { get; set; }
        public decimal? CustoTotalKm { get; set; }
        public decimal? NumTotalKm { get; set; }

        public Projetos NºProjetoNavigation { get; set; }
        public ICollection<DistribuiçãoCustoFolhaDeHoras> DistribuiçãoCustoFolhaDeHoras { get; set; }
        public ICollection<MãoDeObraFolhaDeHoras> MãoDeObraFolhaDeHoras { get; set; }
        public ICollection<PercursosEAjudasCustoDespesasFolhaDeHoras> PercursosEAjudasCustoDespesasFolhaDeHoras { get; set; }
        public ICollection<PresençasFolhaDeHoras> PresençasFolhaDeHoras { get; set; }
    }
}
