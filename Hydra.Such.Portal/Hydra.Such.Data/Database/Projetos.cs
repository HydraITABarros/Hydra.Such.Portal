﻿using System;
using System.Collections.Generic;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Data.Database
{
    public partial class Projetos
    {
        public Projetos()
        {
            DistribuiçãoCustoFolhaDeHoras = new HashSet<DistribuiçãoCustoFolhaDeHoras>();
            DiárioCafetariaRefeitório = new HashSet<DiárioCafetariaRefeitório>();
            DiárioDeProjeto = new HashSet<DiárioDeProjeto>();
            LinhasPréRequisição = new HashSet<LinhasPréRequisição>();
            LinhasRequisição = new HashSet<LinhasRequisição>();
            LinhasRequisiçõesSimplificadas = new HashSet<LinhasRequisiçõesSimplificadas>();
            MovimentosDeProjeto = new HashSet<MovimentosDeProjeto>();
            ProjetosFaturação = new HashSet<ProjetosFaturação>();
            PréMovimentosProjeto = new HashSet<PréMovimentosProjeto>();
            PréRequisição = new HashSet<PréRequisição>();
            Requisição = new HashSet<Requisição>();
            RequisiçõesSimplificadas = new HashSet<RequisiçõesSimplificadas>();
            UnidadesProdutivasProjetoCozinhaNavigation = new HashSet<UnidadesProdutivas>();
            UnidadesProdutivasProjetoDespMatPrimasNavigation = new HashSet<UnidadesProdutivas>();
            UnidadesProdutivasProjetoDesperdíciosNavigation = new HashSet<UnidadesProdutivas>();
            UnidadesProdutivasProjetoMatSubsidiáriasNavigation = new HashSet<UnidadesProdutivas>();
        }

        public string NºProjeto { get; set; }
        public int? Área { get; set; }
        public string Descrição { get; set; }
        public string NºCliente { get; set; }
        public DateTime? Data { get; set; }
        public EstadoProjecto? Estado { get; set; }
        public string MovimentosVenda { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public bool? Faturável { get; set; }
        public string NºContrato { get; set; }
        public string CódEndereçoEnvio { get; set; }
        public string EnvioANome { get; set; }
        public string EnvioAEndereço { get; set; }
        public string EnvioACódPostal { get; set; }
        public string EnvioALocalidade { get; set; }
        public string EnvioAContato { get; set; }
        public int? CódTipoProjeto { get; set; }
        public string NossaProposta { get; set; }
        public int? CódObjetoServiço { get; set; }
        public string NºCompromisso { get; set; }
        public string GrupoContabObra { get; set; }
        public int? TipoGrupoContabProjeto { get; set; }
        public int? TipoGrupoContabOmProjeto { get; set; }
        public string PedidoDoCliente { get; set; }
        public DateTime? DataDoPedido { get; set; }
        public DateTime? ValidadeDoPedido { get; set; }
        public string DescriçãoDetalhada { get; set; }
        public int? CategoriaProjeto { get; set; }
        public string NºContratoOrçamento { get; set; }
        public bool? ProjetoInterno { get; set; }
        public string ChefeProjeto { get; set; }
        public string ResponsávelProjeto { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }
        public bool? FaturaPrecosIvaIncluido { get; set; }
        public bool? FechoAutomatico { get; set; }
        public decimal? KWPotenciaInstalada { get; set; }


        public TipoDeProjeto CódTipoProjetoNavigation { get; set; }
        public TiposGrupoContabOmProjeto TipoGrupoContabOmProjetoNavigation { get; set; }
        public TiposGrupoContabProjeto TipoGrupoContabProjetoNavigation { get; set; }
        public ICollection<DistribuiçãoCustoFolhaDeHoras> DistribuiçãoCustoFolhaDeHoras { get; set; }
        public ICollection<DiárioCafetariaRefeitório> DiárioCafetariaRefeitório { get; set; }
        public ICollection<DiárioDeProjeto> DiárioDeProjeto { get; set; }
        public ICollection<LinhasPréRequisição> LinhasPréRequisição { get; set; }
        public ICollection<LinhasRequisição> LinhasRequisição { get; set; }
        public ICollection<LinhasRequisiçõesSimplificadas> LinhasRequisiçõesSimplificadas { get; set; }
        public ICollection<MovimentosDeProjeto> MovimentosDeProjeto { get; set; }
        public ICollection<ProjetosFaturação> ProjetosFaturação { get; set; }
        public ICollection<PréMovimentosProjeto> PréMovimentosProjeto { get; set; }
        public ICollection<PréRequisição> PréRequisição { get; set; }
        public ICollection<Requisição> Requisição { get; set; }
        public ICollection<RequisiçõesSimplificadas> RequisiçõesSimplificadas { get; set; }
        public ICollection<UnidadesProdutivas> UnidadesProdutivasProjetoCozinhaNavigation { get; set; }
        public ICollection<UnidadesProdutivas> UnidadesProdutivasProjetoDespMatPrimasNavigation { get; set; }
        public ICollection<UnidadesProdutivas> UnidadesProdutivasProjetoDesperdíciosNavigation { get; set; }
        public ICollection<UnidadesProdutivas> UnidadesProdutivasProjetoMatSubsidiáriasNavigation { get; set; }
    }
}
