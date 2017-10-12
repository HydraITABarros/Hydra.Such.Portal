using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class UnidadesProdutivas
    {
        public UnidadesProdutivas()
        {
            CafetariasRefeitórios = new HashSet<CafetariasRefeitórios>();
            DiárioCafetariaRefeitório = new HashSet<DiárioCafetariaRefeitório>();
            DiárioDesperdíciosAlimentares = new HashSet<DiárioDesperdíciosAlimentares>();
            DiárioRequisiçãoUnidProdutiva = new HashSet<DiárioRequisiçãoUnidProdutiva>();
            MovimentosCafetariaRefeitório = new HashSet<MovimentosCafetariaRefeitório>();
            ProjetosFaturação = new HashSet<ProjetosFaturação>();
            RequisiçõesSimplificadas = new HashSet<RequisiçõesSimplificadas>();
        }

        public int NºUnidadeProdutiva { get; set; }
        public string Descrição { get; set; }
        public int? Estado { get; set; }
        public bool? Ativa { get; set; }
        public string NºCliente { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public DateTime? DataInícioExploração { get; set; }
        public DateTime? DataFimExploração { get; set; }
        public string Armazém { get; set; }
        public string ArmazémFornecedor { get; set; }
        public string ProjetoCozinha { get; set; }
        public string ProjetoDesperdícios { get; set; }
        public string ProjetoDespMatPrimas { get; set; }
        public string ProjetoMatSubsidiárias { get; set; }
        public DateTime? DataModificação { get; set; }
        public string UtilizadorModificação { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }

        public Projetos ProjetoCozinhaNavigation { get; set; }
        public Projetos ProjetoDespMatPrimasNavigation { get; set; }
        public Projetos ProjetoDesperdíciosNavigation { get; set; }
        public Projetos ProjetoMatSubsidiáriasNavigation { get; set; }
        public ICollection<CafetariasRefeitórios> CafetariasRefeitórios { get; set; }
        public ICollection<DiárioCafetariaRefeitório> DiárioCafetariaRefeitório { get; set; }
        public ICollection<DiárioDesperdíciosAlimentares> DiárioDesperdíciosAlimentares { get; set; }
        public ICollection<DiárioRequisiçãoUnidProdutiva> DiárioRequisiçãoUnidProdutiva { get; set; }
        public ICollection<MovimentosCafetariaRefeitório> MovimentosCafetariaRefeitório { get; set; }
        public ICollection<ProjetosFaturação> ProjetosFaturação { get; set; }
        public ICollection<RequisiçõesSimplificadas> RequisiçõesSimplificadas { get; set; }
    }
}
