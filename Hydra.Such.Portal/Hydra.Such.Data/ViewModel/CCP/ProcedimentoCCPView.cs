using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Such.Data.Database;

namespace Hydra.Such.Data.ViewModel.CCP
{
    public class ProcedimentoCCPView:ErrorHandler
    {
        public string No { get; set; }
        public int? Tipo { get; set; }
        public int? Ano { get; set; }
        public int? Referencia { get; set; }
        public string CodigoRegiao { get; set; }
        public string CodigoAreaFuncional { get; set; }
        public string CodigoCentroResponsabilidade { get; set; }
        public int? Estado { get; set; }
        public DateTime? DataCriacao { get; set; }
        public bool? Imobilizado { get; set; }
        public string ComentarioEstado { get; set; }
        public bool? Anexos { get; set; }
        public DateTime? DataHoraEstado { get; set; }
        public string UtilizadorEstado { get; set; }
        public string CondicoesDePagamento { get; set; }
        public string NomeProcesso { get; set; }
        public string GestorProcesso { get; set; }
        public int? TipoProcedimento { get; set; }
        public string InformacaoTecnica { get; set; }
        public string FundamentacaoAquisicao { get; set; }
        public bool? PrecoBase { get; set; }
        public decimal? ValorPrecoBase { get; set; }
        public bool? Negociacao { get; set; }
        public string CriteriosAdjudicacao { get; set; }
        public string Prazo { get; set; }
        public bool? PrecoMaisBaixo { get; set; }
        public bool? PropostaEconMaisVantajosa { get; set; }
        public bool? PropostaVariante { get; set; }
        public int? AbertoFechadoAoMercado { get; set; }
        public string PrazoEntrega { get; set; }
        public string LocaisEntrega { get; set; }
        public string ObservacoesAdicionais { get; set; }
        public decimal? EstimativaPreco { get; set; }
        public string FornecedoresSugeridos { get; set; }
        public bool? FornecedorExclusivo { get; set; }
        public string Interlocutor { get; set; }
        public string DescPrecoMaisBaixo { get; set; }
        public string DescPropostaEconMaisVantajosa { get; set; }
        public string DescPropostaVariante { get; set; }
        public string DescAbertoFechadoAoMercado { get; set; }
        public string DescFornecedorExclusivo { get; set; }
        public string CriterioEscolhaProcedimento { get; set; }
        public string DescEscolhaProcedimento { get; set; }
        public bool? Juri { get; set; }
        public int? ObjetoDoContrato { get; set; }
        public bool? PreArea { get; set; }
        public bool? SubmeterPreArea { get; set; }
        public decimal? ValorDecisaoContratar { get; set; }
        public decimal? ValorAdjudicacaoAnteriro { get; set; }
        public decimal? ValorAdjudicacaoAtual { get; set; }
        public decimal? DiferencaEuros { get; set; }
        public decimal? DiferencaPercent { get; set; }
        public bool? WorkflowFinanceiros { get; set; }
        public bool? WorkflowJuridicos { get; set; }
        public bool? WorkflowFinanceirosConfirm { get; set; }
        public bool? WorkflowJuridicosConfirm { get; set; }
        public bool? AutorizacaoImobCa { get; set; }
        public bool? AutorizacaoAberturaCa { get; set; }
        public bool? AutorizacaoAquisicaoCa { get; set; }
        public bool? RejeicaoImobCa { get; set; }
        public bool? RejeicaoAberturaCa { get; set; }
        public bool? RejeicaoAquisicaoCa { get; set; }
        public DateTime? DataAutorizacaoImobCa { get; set; }
        public DateTime? DataAutorizacaoAbertCa { get; set; }
        public DateTime? DataAutorizacaoAquisiCa { get; set; }
        public bool? RatificarCaAbertura { get; set; }
        public bool? RatificarCaAdjudicacao { get; set; }
        public bool? CaRatificar { get; set; }
        public DateTime? CaDataRatificacaoAbert { get; set; }
        public DateTime? CaDataRatificacaoAdjudic { get; set; }
        public string No_Ata { get; set; }
        public DateTime? DataAta { get; set; }
        public string ComentarioPublicacao { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public string UtilizadorPublicacao { get; set; }
        public DateTime? DataSistemaPublicacao { get; set; }
        public string RecolhaComentario { get; set; }
        public DateTime? DataRecolha { get; set; }
        public string UtilizadorRecolha { get; set; }
        public DateTime? DataSistemaRecolha { get; set; }
        public string ComentarioRelatorioPreliminar { get; set; }
        public DateTime? DataValidRelatorioPreliminar { get; set; }
        public string UtilizadorValidRelatorioPreliminar { get; set; }
        public DateTime? DataSistemaValidRelatorioPreliminar { get; set; }
        public string ComentarioAudienciaPrevia { get; set; }
        public DateTime? DataAudienciaPrevia { get; set; }
        public string UtilizadorAudienciaPrevia { get; set; }
        public DateTime? DataSistemaAudienciaPrevia { get; set; }
        public string ComentarioRelatorioFinal { get; set; }
        public DateTime? DataRelatorioFinal { get; set; }
        public string UtilizadorRelatorioFinal { get; set; }
        public DateTime? DataSistemaRelatorioFinal { get; set; }
        public string ComentarioNotificacao { get; set; }
        public DateTime? DataNotificacao { get; set; }
        public string UtilizadorNotificacao { get; set; }
        public DateTime? DataSistemaNotificacao { get; set; }
        public int? PrazoNotificacaoDias { get; set; }
        public int? PercentExecucao { get; set; }
        public bool? Arquivado { get; set; }
        public DateTime? DataFechoInicial { get; set; }
        public DateTime? DataFechoPrevista { get; set; }
        public int? No_DiasProcesso { get; set; }
        public int? No_DiasAtraso { get; set; }
        public bool? Tratado { get; set; }
        public string Fornecedor { get; set; }
        public string Comentario { get; set; }
        public bool? CaSuspenso { get; set; }
        public bool? CriticoAbertura { get; set; }
        public bool? CriticoAdjudicacao { get; set; }
        public string ObjetoDecisao { get; set; }
        public string RazaoNecessidade { get; set; }
        public string ProtocoloContrato { get; set; }
        public bool? AutorizacaoAdjudicacao { get; set; }
        public bool? NaoAdjudicacaoEEncerramento { get; set; }
        public bool? NaoAdjudicacaoESuspensao { get; set; }
        public DateTime? DataHoraCriacao { get; set; }
        public string UtilizadorCriacao { get; set; }
        public DateTime? DataHoraModificacao { get; set; }
        public string UtilizadorModificacao { get; set; }

        ///public ElementosChecklist ElementosChecklist { get; set; }
        //public ElementosChecklistArea ChecklistArea { get; set; }

        public string Nome_Utilizador_Logado { get; set; }

        public TemposPACCPView TemposPaCcp { get; set; }
        public ICollection<FluxoTrabalhoListaControlo> FluxoTrabalhoListaControlo { get; set; }
        public ICollection<RegistoActasView> RegistoDeAtas { get; set; }
        public ICollection<ElementosJuriView> ElementosJuri { get; set; }
        public ICollection<EmailsProcedimentoCCPView> EmailsProcedimentosCcp { get; set; }
        public ICollection<LinhasParaEncomendaCCPView> LinhasPEncomendaProcedimentosCcp { get; set; }
        public ICollection<NotasProcedimentoCCPView> NotasProcedimentosCcp { get; set; }
        public ICollection<WorkflowProcedimentosCCPView> WorkflowProcedimentosCcp { get; set; }

        #region used to map flow control checklist
        #region AreaChecklist
        public DateTime? DataAreaChecklist { get; set; }
        public TimeSpan? HoraAreaChecklist { get; set; }
        public string ComentarioArea { get; set; }
        public string ResponsavelArea { get; set; }
        public string NomeResponsavelArea { get; set; }
        public DateTime? DataResponsavel { get; set; }
        #endregion

        #region ImobilizadoContabilidadeChecklist
        public DateTime? ImobContabDataChecklist { get; set; }
        public TimeSpan? ImobContabHoraChecklist { get; set; }
        public string ComentarioImobContabilidade { get; set; }
        public string ComentarioImobContabilidade2 { get; set; }
        public bool? ImobilizadoSimNao { get; set; }
        public string ResponsavelImobContabilidade { get; set; }
        public string NomeResponsavelImobContabilidade { get; set; }
        public DateTime? DataImobContabilidade { get; set; }
        #endregion

        #region ImobilizadoAreaChecklist
        public DateTime? ImobAreaDataChecklist { get; set; }
        public TimeSpan? ImobAreaHoraChecklist { get; set; }
        public string ComentarioImobArea { get; set; }
        public string ResponsavelImobArea { get; set; }
        public string NomeResponsavelImobArea { get; set; }
        public DateTime? DataImobArea { get; set; }
        public string EmailDestinoCA { get; set; }
        #endregion

        #region ImobilizadoCAChecklist
        public DateTime? ImobCADataChecklist { get; set; }
        public TimeSpan? ImobCAHoraChecklist { get; set; }
        public string ComentarioImobCA { get; set; }
        public string ResponsavelImobCA { get; set; }
        public string NomeResponsavelImobCA { get; set; }
        public DateTime? DataImobAprovisionamentoCA { get; set; }
        #endregion

        #region FundamentoComprasChecklist
        public DateTime? FundamentoComprasDataChecklist { get; set; }
        public TimeSpan? FundamentoComprasHoraChecklist { get; set; }
        public string ComentarioFundamentoCompras { get; set; }
        public string ResponsavelFundamentoCompras { get; set; }    // zpgm. NAV label: "Responsável pelo envio para a área"
        public string NomeResponsavelFundamentoCompras { get; set; }
        public DateTime? DataEnvio { get; set; }
        #endregion

        #region FundamentoFinanceirosChecklist
        public DateTime? FundamentoFinDataChecklist { get; set; }
        public TimeSpan? FundamentoFinHoraChecklist { get; set; }
        public string ComentarioFundamentoFinanceiros { get; set; }
        public string ComentarioFundamentoFinanceiros2 { get; set; }
        public string ResponsavelFundamentoFinanceiros { get; set; }    // zpgm. NAV label: "Responsável dos serviços financeiros"
        public string NomeResponsavelFundamentoFinanceiros { get; set; }
        public DateTime? DataFinanceiros { get; set; }
        #endregion

        #region JuridicosChecklist-Estado6
        public DateTime? JuridicosDataChecklist6 { get; set; }
        public TimeSpan? JuridicosHoraChecklist6 { get; set; }
        public string ComentarioJuridico6 { get; set; }
        public string ResponsavelJuridico6 { get; set; }
        public string NomeResponsavelJuridico6 { get; set; }
        public DateTime? DataJuridico6 { get; set; }
        #endregion

        #region JuridicosChecklist-Estado14
        public DateTime? JuridicosDataChecklist14 { get; set; }
        public TimeSpan? JuridicosHoraChecklist14 { get; set; }
        public string ComentarioJuridico14 { get; set; }
        public string ResponsavelJuridico14 { get; set; }
        public string NomeResponsavelJuridico14 { get; set; }
        public DateTime? DataJuridico14 { get; set; }
        #endregion

        #region FundamentacaoAreaChecklist
        public DateTime? FundamentacaoAreaDataChecklist { get; set; }
        public TimeSpan? FundamentacaoAreaHoraChecklist { get; set; }
        public string FundamentacaoAreaComentario { get; set; }
        public string FundamentacaoAreaResponsavel { get; set; }
        public string FundamentacaoAreaNomeResponsavel { get; set; }
        public DateTime? FundamentacaoAreaDataResponsavel { get; set; }
        #endregion

        #region AberturaCAChecklist-Estado8
        public DateTime? AberturaCADataChecklist8 { get; set; }
        public TimeSpan? AberturaCAHoraChecklist8 { get; set; }
        public string ComentarioCA8 { get; set; }
        public string ResponsavelCA8 { get; set; }
        public string NomeResponsavelCA8 { get; set; }
        public DateTime? DataAberturaCA8 { get; set; }
        #endregion

        #region AberturaCAChecklist-Estado17
        public DateTime? AberturaCADataChecklist17 { get; set; }
        public TimeSpan? AberturaCAHoraChecklist17 { get; set; }
        public string ComentarioCA17 { get; set; }
        public string ResponsavelCA17 { get; set; }
        public string NomeResponsavelCA17 { get; set; }
        public DateTime? DataAberturaCA17 { get; set; }
        #endregion

        #region AdjudicacaoComprasChecklist-Estado15
        public DateTime? AdjudicacaoDataChecklist15 { get; set; }
        public TimeSpan? AdjudicacaoHoraChecklist15 { get; set; }
        public string ComentarioAdjudicacao15 { get; set; }
        public string ResponsavelAdjudicacao15 { get; set; }
        public string NomeResponsavelAdjudicacao15 { get; set; }
        public DateTime? DataAdjudicacao15 { get; set; }
        #endregion

        #region AdjudicacaoComprasChecklist-Estado16
        public DateTime? AdjudicacaoDataChecklist16 { get; set; }
        public TimeSpan? AdjudicacaoHoraChecklist16 { get; set; }
        public string ComentarioAdjudicacao16 { get; set; }
        public string ResponsavelAdjudicacao16 { get; set; }
        public string NomeResponsavelAdjudicacao16 { get; set; }
        public DateTime? DataAdjudicacao16 { get; set; }
        #endregion
        #endregion

        #region Used to map Send Email To Juri Aproval
        public string EmailToJuriAproval_From { get; set; }
        public string EmailToJuriAproval_To { get; set; }
        public string EmailToJuriAproval_Subject { get; set; }
        public string EmailToJuriAproval_Comment { get; set; }


        #endregion

        public string DataPublicacao_Show { get; set; }
        public string DataRecolha_Show { get; set; }
        public string DataValidRelatorioPreliminar_Show { get; set; }
        public string DataAudienciaPrevia_Show { get; set; }
        public string DataRelatorioFinal_Show { get; set; }
    }
}
