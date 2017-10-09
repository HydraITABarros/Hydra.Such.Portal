using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class MãoDeObraFolhaDeHoras
    {
        public string NºFolhaDeHoras { get; set; }
        public int NºLinha { get; set; }
        public DateTime? Date { get; set; }
        public string NºEmpregado { get; set; }
        public string NºProjeto { get; set; }
        public int? CódigoTipoTrabalho { get; set; }
        public TimeSpan? HoraInício { get; set; }
        public TimeSpan? HoraFim { get; set; }
        public bool? HorárioAlmoço { get; set; }
        public bool? HorárioJantar { get; set; }
        public string CódigoFamíliaRecurso { get; set; }
        public string NºRecurso { get; set; }
        public string CódUnidadeMedida { get; set; }
        public int? CódigoTipoOm { get; set; }
        public TimeSpan? NºDeHotas { get; set; }
        public decimal? CustoUnitárioDireto { get; set; }
        public decimal? PreçoDeCusto { get; set; }
        public decimal? PreçoDeVenda { get; set; }
        public decimal? PreçoTotal { get; set; }

        public CatálogoManutenção CódigoTipoOmNavigation { get; set; }
        public FolhasDeHoras NºFolhaDeHorasNavigation { get; set; }
        public Projetos NºProjetoNavigation { get; set; }
    }
}
