using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class MovimentosDeProjeto
    {
        public int NºLinha { get; set; }
        public string NºProjeto { get; set; }
        public DateTime? Data { get; set; }
        public int? TipoMovimento { get; set; }
        public string NºDocumento { get; set; }
        public int? Tipo { get; set; }
        public string Código { get; set; }
        public string Descrição { get; set; }
        public decimal? Quantidade { get; set; }
        public string CódUnidadeMedida { get; set; }
        public string CódLocalização { get; set; }
        public string GrupoContabProjeto { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public string Utilizador { get; set; }
        public decimal? CustoUnitário { get; set; }
        public decimal? CustoTotal { get; set; }
        public decimal? PreçoUnitário { get; set; }
        public decimal? PreçoTotal { get; set; }
        public bool? Faturável { get; set; }
        public string NºGuiaResíduos { get; set; }
        public string NºGuiaExterna { get; set; }
        public string FaturaANºCliente { get; set; }
        public string NºRequisição { get; set; }
        public int? NºLinhaRequisição { get; set; }
        public string Motorista { get; set; }
        public int? TipoRefeição { get; set; }
        public int? CódDestinoFinalResíduos { get; set; }
        public string DocumentoOriginal { get; set; }
        public string DocumentoCorrigido { get; set; }
        public bool? AcertoDePreços { get; set; }
        public DateTime? DataDocumentoCorrigido { get; set; }
        public bool? FaturaçãoAutorizada { get; set; }
        public bool? FaturaçãoAutorizada2 { get; set; }
        public DateTime? DataAutorizaçãoFaturação { get; set; }
        public string CódGrupoServiço { get; set; }
        public int? TipoRecurso { get; set; }
        public string NºFolhaHoras { get; set; }
        public string RequisiçãoInterna { get; set; }
        public string NºFuncionário { get; set; }
        public decimal? QuantidadeDevolvida { get; set; }
        public DateTime? DataConsumo { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorCriação { get; set; }
        public string UtilizadorModificação { get; set; }
        public bool? Registado { get; set; }
        public bool? Faturada { get; set; }
        public string Moeda { get; set; }
        public decimal? ValorUnitárioAFaturar { get; set; }
        public string CódServiçoCliente { get; set; }
        public string CodCliente { get; set; }
        public string Matricula { get; set; }
        public string CodigoLer { get; set; }
        public string Grupo { get; set; }
        public string Operacao { get; set; }
        public int? GrupoFatura { get; set; }
        public string GrupoFaturaDescricao { get; set; }
        public string AutorizadoPor { get; set; }

        public DestinosFinaisResíduos CódDestinoFinalResíduosNavigation { get; set; }
        public LinhasRequisição Nº { get; set; }
        public Projetos NºProjetoNavigation { get; set; }
        public Requisição NºRequisiçãoNavigation { get; set; }
        public TiposRefeição TipoRefeiçãoNavigation { get; set; }
    }
}
