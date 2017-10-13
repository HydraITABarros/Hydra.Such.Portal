using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class TiposGrupoContabOmProjeto
    {
        public TiposGrupoContabOmProjeto()
        {
            Projetos = new HashSet<Projetos>();
        }

        public int Código { get; set; }
        public int? Tipo { get; set; }
        public string Descrição { get; set; }
        public bool? ManutCorretiva { get; set; }
        public bool? ManutPreventiva { get; set; }
        public int? TipoRazãoFalha { get; set; }
        public bool? IndicadorTempoResposta { get; set; }
        public bool? IndicadorTempoImobilização { get; set; }
        public bool? IndicadorTempoEfetivoReparação { get; set; }
        public bool? IndicadorTempoFechoObras { get; set; }
        public bool? IndicadorTempoFaturação { get; set; }
        public bool? IndicadorTempoOcupColaboradores { get; set; }
        public bool? IndicadorValorCustoVenda { get; set; }
        public bool? IndicTaxaCumprimentoCat { get; set; }
        public bool? IndicadorTaxaCoberturaCat { get; set; }
        public bool? IndicTaxaCumprRotinasMp { get; set; }
        public bool? IndicIncidênciasAvarias { get; set; }
        public bool? IndicadorOrdensEmCurso { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }

        public ICollection<Projetos> Projetos { get; set; }
    }
}
