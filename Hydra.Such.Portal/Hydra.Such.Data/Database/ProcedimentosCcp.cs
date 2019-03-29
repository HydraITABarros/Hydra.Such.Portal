using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Database
{
    public partial class ProcedimentosCcp
    {
        public ProcedimentosCcp()
        {
            ElementosJuri = new HashSet<ElementosJuri>();
            EmailsProcedimentosCcp = new HashSet<EmailsProcedimentosCcp>();
            FluxoTrabalhoListaControlo = new HashSet<FluxoTrabalhoListaControlo>();
            LinhasPEncomendaProcedimentosCcp = new HashSet<LinhasPEncomendaProcedimentosCcp>();
            NotasProcedimentosCcp = new HashSet<NotasProcedimentosCcp>();
            RegistoDeAtas = new HashSet<RegistoDeAtas>();
            WorkflowProcedimentosCcp = new HashSet<WorkflowProcedimentosCcp>();

            LotesProcedimento = new HashSet<LoteProcedimentoCcp>();
        }

        public string Nº { get; set; }
        public int? Tipo { get; set; }
        public int? Ano { get; set; }
        public int? Referência { get; set; }
        public string CódigoRegião { get; set; }
        public string CódigoÁreaFuncional { get; set; }
        public string CódigoCentroResponsabilidade { get; set; }
        public int? Estado { get; set; }
        public DateTime? DataCriação { get; set; }
        public bool? Imobilizado { get; set; }
        public string ComentárioEstado { get; set; }
        public bool? Anexos { get; set; }
        public DateTime? DataHoraEstado { get; set; }
        public string UtilizadorEstado { get; set; }
        public string CondiçõesDePagamento { get; set; }
        public string NomeProcesso { get; set; }
        public string GestorProcesso { get; set; }
        public int? TipoProcedimento { get; set; }
        public string InformaçãoTécnica { get; set; }
        public string FundamentaçãoAquisição { get; set; }
        public bool? PreçoBase { get; set; }
        public decimal? ValorPreçoBase { get; set; }
        public bool? Negociação { get; set; }
        public string CritériosAdjudicação { get; set; }
        public string Prazo { get; set; }
        public bool? PreçoMaisBaixo { get; set; }
        public bool? PropostaEconMaisVantajosa { get; set; }
        public bool? PropostaVariante { get; set; }
        public int? AbertoFechadoAoMercado { get; set; }
        public string PrazoEntrega { get; set; }
        public string LocaisEntrega { get; set; }
        public string ObservaçõesAdicionais { get; set; }
        public decimal? EstimativaPreço { get; set; }
        public string FornecedoresSugeridos { get; set; }
        public bool? FornecedorExclusivo { get; set; }
        public string Interlocutor { get; set; }
        public string DescPreçoMaisBaixo { get; set; }
        public string DescPropostaEconMaisVantajosa { get; set; }
        public string DescPropostaVariante { get; set; }
        public string DescAbertoFechadoAoMercado { get; set; }
        public string DescFornecedorExclusivo { get; set; }
        public string CritérioEscolhaProcedimento { get; set; }
        public string DescEscolhaProcedimento { get; set; }
        public bool? Júri { get; set; }
        public int? ObjetoDoContrato { get; set; }
        public bool? PréÁrea { get; set; }
        public bool? SubmeterPréÁrea { get; set; }
        public decimal? ValorDecisãoContratar { get; set; }
        public decimal? ValorAdjudicaçãoAnteriro { get; set; }
        public decimal? ValorAdjudicaçãoAtual { get; set; }
        public decimal? DiferençaEuros { get; set; }
        public decimal? DiferençaPercent { get; set; }
        public bool? WorkflowFinanceiros { get; set; }
        public bool? WorkflowJurídicos { get; set; }
        public bool? WorkflowFinanceirosConfirm { get; set; }
        public bool? WorkflowJurídicosConfirm { get; set; }
        public bool? AutorizaçãoImobCa { get; set; }
        public bool? AutorizaçãoAberturaCa { get; set; }
        public bool? AutorizaçãoAquisiçãoCa { get; set; }
        public bool? RejeiçãoImobCa { get; set; }
        public bool? RejeiçãoAberturaCa { get; set; }
        public bool? RejeiçãoAquisiçãoCa { get; set; }
        public DateTime? DataAutorizaçãoImobCa { get; set; }
        public DateTime? DataAutorizaçãoAbertCa { get; set; }
        public DateTime? DataAutorizaçãoAquisiCa { get; set; }
        public bool? RatificarCaAbertura { get; set; }
        public bool? RatificarCaAdjudicação { get; set; }
        public bool? CaRatificar { get; set; }
        public DateTime? CaDataRatificaçãoAbert { get; set; }
        public DateTime? CaDataRatificaçãoAdjudic { get; set; }
        public string NºAta { get; set; }
        public DateTime? DataAta { get; set; }
        public string ComentárioPublicação { get; set; }
        public DateTime? DataPublicação { get; set; }
        public string UtilizadorPublicação { get; set; }
        public DateTime? DataSistemaPublicação { get; set; }
        public string RecolhaComentário { get; set; }
        public DateTime? DataRecolha { get; set; }
        public string UtilizadorRecolha { get; set; }
        public DateTime? DataSistemaRecolha { get; set; }
        public string ComentárioRelatórioPreliminar { get; set; }
        public DateTime? DataValidRelatórioPreliminar { get; set; }
        public string UtilizadorValidRelatórioPreliminar { get; set; }
        public DateTime? DataSistemaValidRelatórioPreliminar { get; set; }
        public string ComentárioAudiênciaPrévia { get; set; }
        public DateTime? DataAudiênciaPrévia { get; set; }
        public string UtilizadorAudiênciaPrévia { get; set; }
        public DateTime? DataSistemaAudiênciaPrévia { get; set; }
        public string ComentárioRelatórioFinal { get; set; }
        public DateTime? DataRelatórioFinal { get; set; }
        public string UtilizadorRelatórioFinal { get; set; }
        public DateTime? DataSistemaRelatórioFinal { get; set; }
        public string ComentárioNotificação { get; set; }
        public DateTime? DataNotificação { get; set; }
        public string UtilizadorNotificação { get; set; }
        public DateTime? DataSistemaNotificação { get; set; }
        public int? PrazoNotificaçãoDias { get; set; }
        public int? PercentExecução { get; set; }
        public bool? Arquivado { get; set; }
        public DateTime? DataFechoInicial { get; set; }
        public DateTime? DataFechoPrevista { get; set; }
        public int? NºDiasProcesso { get; set; }
        public int? NºDiasAtraso { get; set; }
        public bool? Tratado { get; set; }
        public string Fornecedor { get; set; }
        public string Comentário { get; set; }
        public bool? CaSuspenso { get; set; }
        public bool? CríticoAbertura { get; set; }
        public bool? CríticoAdjudicação { get; set; }
        public string ObjetoDecisão { get; set; }
        public string RazãoNecessidade { get; set; }
        public string ProtocoloContrato { get; set; }
        public bool? AutorizaçãoAdjudicação { get; set; }
        public bool? NãoAdjudicaçãoEEncerramento { get; set; }
        public bool? NãoAdjudicaçãoESuspensão { get; set; }
        public DateTime? DataHoraCriação { get; set; }
        public string UtilizadorCriação { get; set; }
        public DateTime? DataHoraModificação { get; set; }
        public string UtilizadorModificação { get; set; }

        #region zpgm.28032019
        public int? FundamentoLegalTipo { get; set; }
        public int? ProcedimentoEmLotes { get; set; }
        public string FundamentacaoPrecoBase { get; set; }
        public int? VistoAberturaPeloAprovisionamento { get; set; }
        public int? VistoAdjudicacaoPeloAprovisionamento { get; set; }

        public TipoProcedimentoCcp TipoContratacaoPublica { get; set; }
        public FundamentoLegalTipoProcedimentoCcp FundamentoLegal { get; set; }
        public ICollection<LoteProcedimentoCcp> LotesProcedimento { get; set; }
        #endregion

        public TemposPaCcp TemposPaCcp { get; set; }
        public ICollection<ElementosJuri> ElementosJuri { get; set; }
        public ICollection<EmailsProcedimentosCcp> EmailsProcedimentosCcp { get; set; }
        public ICollection<FluxoTrabalhoListaControlo> FluxoTrabalhoListaControlo { get; set; }
        public ICollection<LinhasPEncomendaProcedimentosCcp> LinhasPEncomendaProcedimentosCcp { get; set; }
        public ICollection<NotasProcedimentosCcp> NotasProcedimentosCcp { get; set; }
        public ICollection<RegistoDeAtas> RegistoDeAtas { get; set; }
        public ICollection<WorkflowProcedimentosCcp> WorkflowProcedimentosCcp { get; set; }
    }
}
