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

        public Projetos NºProjetoNavigation { get; set; }
        public ICollection<DistribuiçãoCustoFolhaDeHoras> DistribuiçãoCustoFolhaDeHoras { get; set; }
        public ICollection<MãoDeObraFolhaDeHoras> MãoDeObraFolhaDeHoras { get; set; }
        public ICollection<PercursosEAjudasCustoDespesasFolhaDeHoras> PercursosEAjudasCustoDespesasFolhaDeHoras { get; set; }
        public ICollection<PresençasFolhaDeHoras> PresençasFolhaDeHoras { get; set; }
    }
}
