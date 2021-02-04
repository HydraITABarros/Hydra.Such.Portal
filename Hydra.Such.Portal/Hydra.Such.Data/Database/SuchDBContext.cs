using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Hydra.Such.Data.Database
{
    public partial class SuchDBContext : DbContext
    {
        public virtual DbSet<AcessosDimensões> AcessosDimensões { get; set; }
        public virtual DbSet<AcessosLocalizacoes> AcessosLocalizacoes { get; set; }
        public virtual DbSet<AcessosPerfil> AcessosPerfil { get; set; }
        public virtual DbSet<AcessosUtilizador> AcessosUtilizador { get; set; }
        public virtual DbSet<AçõesDeConfeção> AçõesDeConfeção { get; set; }
        public virtual DbSet<AcordoPrecos> AcordoPrecos { get; set; }
        public virtual DbSet<Actividades> Actividades { get; set; }
        public virtual DbSet<ActividadesPorFornecedor> ActividadesPorFornecedor { get; set; }
        public virtual DbSet<ActividadesPorProduto> ActividadesPorProduto { get; set; }
        public virtual DbSet<Anexos> Anexos { get; set; }
        public virtual DbSet<AnexosErros> AnexosErros { get; set; }
        public virtual DbSet<AutorizacaoFhRh> AutorizacaoFhRh { get; set; }
        public virtual DbSet<AutorizarFaturaçãoContratos> AutorizarFaturaçãoContratos { get; set; }
        public virtual DbSet<BarramentosDeVoz> BarramentosDeVoz { get; set; }
        public virtual DbSet<CafetariasRefeitórios> CafetariasRefeitórios { get; set; }
        public virtual DbSet<CartõesEApólices> CartõesEApólices { get; set; }
        public virtual DbSet<CartõesTelemóveis> CartõesTelemóveis { get; set; }
        public virtual DbSet<CatálogoManutenção> CatálogoManutenção { get; set; }
        public virtual DbSet<ClassificaçãoFichasTécnicas> ClassificaçãoFichasTécnicas { get; set; }
        public virtual DbSet<Comprador> Comprador { get; set; }
        public virtual DbSet<Compras> Compras { get; set; }
        public virtual DbSet<CondicoesPropostasFornecedores> CondicoesPropostasFornecedores { get; set; }
        public virtual DbSet<ConfigMercadoLocal> ConfigMercadoLocal { get; set; }
        public virtual DbSet<Configuração> Configuração { get; set; }
        public virtual DbSet<ConfiguracaoAjudaCusto> ConfiguracaoAjudaCusto { get; set; }
        public virtual DbSet<ConfiguraçãoAprovações> ConfiguraçãoAprovações { get; set; }
        public virtual DbSet<ConfiguracaoCcp> ConfiguracaoCcp { get; set; }
        public virtual DbSet<ConfiguraçãoEmailFornecedores> ConfiguraçãoEmailFornecedores { get; set; }
        public virtual DbSet<ConfiguraçãoNumerações> ConfiguraçãoNumerações { get; set; }
        public virtual DbSet<ConfiguraçãoTemposCcp> ConfiguraçãoTemposCcp { get; set; }
        public virtual DbSet<ConfiguraçãoVendasAlertas> ConfiguraçãoVendasAlertas { get; set; }
        public virtual DbSet<ConfigUtilizadores> ConfigUtilizadores { get; set; }
        public virtual DbSet<ConsultaMercado> ConsultaMercado { get; set; }
        public virtual DbSet<ConsultaPendentes> ConsultaPendentes { get; set; }
        public virtual DbSet<Contactos> Contactos { get; set; }
        public virtual DbSet<ContactosServicos> ContactosServicos { get; set; }
        public virtual DbSet<ContactosFuncoes> ContactosFuncoes { get; set; }
        public virtual DbSet<Contratos> Contratos { get; set; }
        public virtual DbSet<DestinosFinaisResíduos> DestinosFinaisResíduos { get; set; }
        public virtual DbSet<DiárioCafetariaRefeitório> DiárioCafetariaRefeitório { get; set; }
        public virtual DbSet<DiárioDeProjeto> DiárioDeProjeto { get; set; }
        public virtual DbSet<DiárioDesperdíciosAlimentares> DiárioDesperdíciosAlimentares { get; set; }
        public virtual DbSet<DiárioMovimentosViaturas> DiárioMovimentosViaturas { get; set; }
        public virtual DbSet<DiárioRequisiçãoUnidProdutiva> DiárioRequisiçãoUnidProdutiva { get; set; }
        public virtual DbSet<DistanciaFh> DistanciaFh { get; set; }
        public virtual DbSet<DistribuiçãoCustoFolhaDeHoras> DistribuiçãoCustoFolhaDeHoras { get; set; }
        public virtual DbSet<ElementosJuri> ElementosJuri { get; set; }
        public virtual DbSet<EmailsAprovações> EmailsAprovações { get; set; }
        public virtual DbSet<EmailsProcedimentosCcp> EmailsProcedimentosCcp { get; set; }
        public virtual DbSet<EnderecoSuch> EnderecoSuch { get; set; }
        public virtual DbSet<FeaturesMenus> FeaturesMenus { get; set; }
        public virtual DbSet<FichaProduto> FichaProduto { get; set; }
        public virtual DbSet<FichasTécnicasPratos> FichasTécnicasPratos { get; set; }
        public virtual DbSet<FluxoTrabalhoListaControlo> FluxoTrabalhoListaControlo { get; set; }
        public virtual DbSet<FolhasDeHoras> FolhasDeHoras { get; set; }
        public virtual DbSet<FornecedoresAcordoPrecos> FornecedoresAcordoPrecos { get; set; }
        public virtual DbSet<GruposAprovação> GruposAprovação { get; set; }
        public virtual DbSet<HistoricoCondicoesPropostasFornecedores> HistoricoCondicoesPropostasFornecedores { get; set; }
        public virtual DbSet<HistoricoConsultaMercado> HistoricoConsultaMercado { get; set; }
        public virtual DbSet<HistoricoLinhasCondicoesPropostasFornecedores> HistoricoLinhasCondicoesPropostasFornecedores { get; set; }
        public virtual DbSet<HistoricoLinhasConsultaMercado> HistoricoLinhasConsultaMercado { get; set; }
        public virtual DbSet<HistoricoSeleccaoEntidades> HistoricoSeleccaoEntidades { get; set; }
        public virtual DbSet<Instrutores> Instrutores { get; set; }
        public virtual DbSet<LinhasAcordoPrecos> LinhasAcordoPrecos { get; set; }
        public virtual DbSet<LinhasCondicoesPropostasFornecedores> LinhasCondicoesPropostasFornecedores { get; set; }
        public virtual DbSet<LinhasConsultaMercado> LinhasConsultaMercado { get; set; }
        public virtual DbSet<LinhasContratos> LinhasContratos { get; set; }
        public virtual DbSet<LinhasFaturaçãoContrato> LinhasFaturaçãoContrato { get; set; }
        public virtual DbSet<LinhasFichasTécnicasPratos> LinhasFichasTécnicasPratos { get; set; }
        public virtual DbSet<LinhasFolhaHoras> LinhasFolhaHoras { get; set; }
        public virtual DbSet<LinhasPEncomendaProcedimentosCcp> LinhasPEncomendaProcedimentosCcp { get; set; }
        public virtual DbSet<LinhasPreEncomenda> LinhasPreEncomenda { get; set; }
        public virtual DbSet<LinhasPréRequisição> LinhasPréRequisição { get; set; }
        public virtual DbSet<LinhasRequisição> LinhasRequisição { get; set; }
        public virtual DbSet<LinhasRequisiçãoHist> LinhasRequisiçãoHist { get; set; }
        public virtual DbSet<LinhasRequisiçõesSimplificadas> LinhasRequisiçõesSimplificadas { get; set; }
        public virtual DbSet<Locais> Locais { get; set; }
        public virtual DbSet<Localizações> Localizações { get; set; }
        public virtual DbSet<MãoDeObraFolhaDeHoras> MãoDeObraFolhaDeHoras { get; set; }
        public virtual DbSet<Marcas> Marcas { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Modelos> Modelos { get; set; }
        public virtual DbSet<MovimentoDeProdutos> MovimentoDeProdutos { get; set; }
        public virtual DbSet<MovimentosCafetariaRefeitório> MovimentosCafetariaRefeitório { get; set; }
        public virtual DbSet<MovimentosDeAprovação> MovimentosDeAprovação { get; set; }
        public virtual DbSet<MovimentosDeProjeto> MovimentosDeProjeto { get; set; }
        public virtual DbSet<MovimentosProjectoAutorizados> MovimentosProjectoAutorizados { get; set; }
        public virtual DbSet<MovimentosTelefones> MovimentosTelefones { get; set; }
        public virtual DbSet<MovimentosTelemóveis> MovimentosTelemóveis { get; set; }
        public virtual DbSet<MovimentosViaturas> MovimentosViaturas { get; set; }
        public virtual DbSet<NotasProcedimentosCcp> NotasProcedimentosCcp { get; set; }
        public virtual DbSet<ObjetosDeServiço> ObjetosDeServiço { get; set; }
        public virtual DbSet<OrigemDestinoFh> OrigemDestinoFh { get; set; }
        public virtual DbSet<PercursosEAjudasCustoDespesasFolhaDeHoras> PercursosEAjudasCustoDespesasFolhaDeHoras { get; set; }
        public virtual DbSet<PerfisModelo> PerfisModelo { get; set; }
        public virtual DbSet<PerfisUtilizador> PerfisUtilizador { get; set; }
        public virtual DbSet<PontosSituaçãoRq> PontosSituaçãoRq { get; set; }
        public virtual DbSet<PrecoCustoRecursoFh> PrecoCustoRecursoFh { get; set; }
        public virtual DbSet<PreçosFornecedor> PreçosFornecedor { get; set; }
        public virtual DbSet<PreçosServiçosCliente> PreçosServiçosCliente { get; set; }
        public virtual DbSet<PrecoVendaRecursoFh> PrecoVendaRecursoFh { get; set; }
        public virtual DbSet<PréMovimentosProjeto> PréMovimentosProjeto { get; set; }
        public virtual DbSet<PréRequisição> PréRequisição { get; set; }
        public virtual DbSet<PresençasFolhaDeHoras> PresençasFolhaDeHoras { get; set; }
        public virtual DbSet<ProcedimentosCcp> ProcedimentosCcp { get; set; }
        public virtual DbSet<ProcedimentosDeConfeção> ProcedimentosDeConfeção { get; set; }
        public virtual DbSet<ProcessosDisciplinaresInquérito> ProcessosDisciplinaresInquérito { get; set; }
        public virtual DbSet<ProjectosAutorizados> ProjectosAutorizados { get; set; }
        public virtual DbSet<Projetos> Projetos { get; set; }
        public virtual DbSet<ProjetosFaturação> ProjetosFaturação { get; set; }
        public virtual DbSet<RececaoFaturacao> RececaoFaturacao { get; set; }
        public virtual DbSet<RececaoFaturacaoWorkflow> RececaoFaturacaoWorkflow { get; set; }
        public virtual DbSet<RececaoFaturacaoWorkflowAnexo> RececaoFaturacaoWorkflowAnexo { get; set; }
        public virtual DbSet<RecFacturasProblemas> RecFacturasProblemas { get; set; }
        public virtual DbSet<RecFaturacaoConfigDestinatarios> RecFaturacaoConfigDestinatarios { get; set; }
        public virtual DbSet<RegistoDeAtas> RegistoDeAtas { get; set; }
        public virtual DbSet<RegistoDePropostas> RegistoDePropostas { get; set; }
        public virtual DbSet<Requisição> Requisição { get; set; }
        public virtual DbSet<RequisiçãoHist> RequisiçãoHist { get; set; }
        public virtual DbSet<RequisiçõesClienteContrato> RequisiçõesClienteContrato { get; set; }
        public virtual DbSet<RequisicoesRegAlteracoes> RequisicoesRegAlteracoes { get; set; }
        public virtual DbSet<RequisiçõesSimplificadas> RequisiçõesSimplificadas { get; set; }
        public virtual DbSet<RhRecursosFh> RhRecursosFh { get; set; }
        public virtual DbSet<SeleccaoEntidades> SeleccaoEntidades { get; set; }
        public virtual DbSet<Serviços> Serviços { get; set; }
        public virtual DbSet<ServiçosCliente> ServiçosCliente { get; set; }
        public virtual DbSet<TabelaConfRecursosFh> TabelaConfRecursosFh { get; set; }
        public virtual DbSet<Tarifários> Tarifários { get; set; }
        public virtual DbSet<TaxaResiduos> TaxaResiduos { get; set; }
        public virtual DbSet<Telefones> Telefones { get; set; }
        public virtual DbSet<Telemóveis> Telemóveis { get; set; }
        public virtual DbSet<TelemoveisCartoes> TelemoveisCartoes { get; set; }
        public virtual DbSet<TelemoveisEquipamentos> TelemoveisEquipamentos { get; set; }
        public virtual DbSet<TelemoveisMovimentos> TelemoveisMovimentos { get; set; }
        public virtual DbSet<TelemoveisOcorrencias> TelemoveisOcorrencias { get; set; }
        public virtual DbSet<TemposPaCcp> TemposPaCcp { get; set; }
        public virtual DbSet<TextoFaturaContrato> TextoFaturaContrato { get; set; }
        public virtual DbSet<TipoDeProjeto> TipoDeProjeto { get; set; }
        public virtual DbSet<TiposGrupoContabOmProjeto> TiposGrupoContabOmProjeto { get; set; }
        public virtual DbSet<TiposGrupoContabProjeto> TiposGrupoContabProjeto { get; set; }
        public virtual DbSet<TiposRefeição> TiposRefeição { get; set; }
        public virtual DbSet<TiposRequisições> TiposRequisições { get; set; }
        public virtual DbSet<TiposViatura> TiposViatura { get; set; }
        public virtual DbSet<TipoTrabalhoFh> TipoTrabalhoFh { get; set; }
        public virtual DbSet<UnidadeDeArmazenamento> UnidadeDeArmazenamento { get; set; }
        public virtual DbSet<UnidadeMedida> UnidadeMedida { get; set; }
        public virtual DbSet<UnidadeMedidaProduto> UnidadeMedidaProduto { get; set; }
        public virtual DbSet<UnidadePrestação> UnidadePrestação { get; set; }
        public virtual DbSet<UnidadesProdutivas> UnidadesProdutivas { get; set; }
        public virtual DbSet<UtilizadoresGruposAprovação> UtilizadoresGruposAprovação { get; set; }
        public virtual DbSet<UtilizadoresMovimentosDeAprovação> UtilizadoresMovimentosDeAprovação { get; set; }
        public virtual DbSet<Viaturas> Viaturas { get; set; }
        public virtual DbSet<ViaturasImagens> ViaturasImagens { get; set; }
        public virtual DbSet<Viaturas2> Viaturas2 { get; set; }
        public virtual DbSet<ConfiguracaoTabelas> ConfiguracaoTabelas { get; set; }
        public virtual DbSet<Viaturas2Marcas> Viaturas2Marcas { get; set; }
        public virtual DbSet<Viaturas2Modelos> Viaturas2Modelos { get; set; }
        public virtual DbSet<Viaturas2Parqueamento> Viaturas2Parqueamento { get; set; }
        public virtual DbSet<Viaturas2ParqueamentoLocal> Viaturas2ParqueamentoLocal { get; set; }
        public virtual DbSet<Viaturas2Inspecoes> Viaturas2Inspecoes { get; set; }
        public virtual DbSet<Viaturas2Gestores> Viaturas2Gestores { get; set; }
        public virtual DbSet<Viaturas2GestoresGestor> Viaturas2GestoresGestor { get; set; }
        public virtual DbSet<Viaturas2Estados> Viaturas2Estados { get; set; }
        public virtual DbSet<Viaturas2Dimensoes> Viaturas2Dimensoes { get; set; }
        public virtual DbSet<Viaturas2Propriedades> Viaturas2Propriedades { get; set; }
        public virtual DbSet<Viaturas2Km> Viaturas2Km { get; set; }
        public virtual DbSet<Viaturas2Manutencao> Viaturas2Manutencao { get; set; }
        public virtual DbSet<Viaturas2CartaVerde> Viaturas2CartaVerde { get; set; }
        public virtual DbSet<Viaturas2CartaVerdeSeguradora> Viaturas2CartaVerdeSeguradora { get; set; }
        public virtual DbSet<Viaturas2Acidentes> Viaturas2Acidentes { get; set; }
        public virtual DbSet<Viaturas2ContraOrdenacoes> Viaturas2ContraOrdenacoes { get; set; }
        public virtual DbSet<Viaturas2Afetacao> Viaturas2Afetacao { get; set; }
        public virtual DbSet<Viaturas2AfetacaoAreaReal> Viaturas2AfetacaoAreaReal { get; set; }
        public virtual DbSet<Viaturas2CartaoCombustivel> Viaturas2CartaoCombustivel { get; set; }
        public virtual DbSet<Viaturas2CarTrack> Viaturas2CarTrack { get; set; }
        public virtual DbSet<Viaturas2ViaVerde> Viaturas2ViaVerde { get; set; }
        public virtual DbSet<Viaturas2Abastecimentos> Viaturas2Abastecimentos { get; set; }
        public virtual DbSet<Viaturas2Abate> Viaturas2Abate { get; set; }
        public virtual DbSet<Viaturas2Imobilizados> Viaturas2Imobilizados { get; set; }
        public virtual DbSet<Viaturas2RentingContrato> Viaturas2RentingContrato { get; set; }
        public virtual DbSet<Viaturas2RentingContratoAlteracoes> Viaturas2RentingContratoAlteracoes { get; set; }
        public virtual DbSet<Viaturas2Substituicao> Viaturas2Substituicao { get; set; }
        public virtual DbSet<WorkflowProcedimentosCcp> WorkflowProcedimentosCcp { get; set; }
        public virtual DbSet<PedidosPagamento> PedidosPagamento { get; set; }
        public virtual DbSet<TabelaLog> TabelaLog { get; set; }
        public virtual DbSet<PedidosDEV> PedidosDEV { get; set; }
        public virtual DbSet<ConfigLinhasEncFornecedor> ConfigLinhasEncFornecedor { get; set; }
        public virtual DbSet<Orcamentos> Orcamentos { get; set; }
        public virtual DbSet<LinhasOrcamentos> LinhasOrcamentos { get; set; }
        public virtual DbSet<OrcamentosContatos> OrcamentosContatos { get; set; }
        public virtual DbSet<ConfiguracaoParametros> ConfiguracaoParametros { get; set; }
        public virtual DbSet<SISLOGReservas> SISLOGReservas { get; set; }
        public virtual DbSet<SISLOGProdutos> SISLOGProdutos { get; set; }
        public virtual DbSet<Ocorrencias> Ocorrencias { get; set; }

        #region zpgm.ALT_CCP_#001.y2019
        public virtual DbSet<TipoProcedimentoCcp> TipoProcedimentoCcp { get; set; }
        public virtual DbSet<FundamentoLegalTipoProcedimentoCcp> FundamentoLegalTipoProcedimentoCcp { get; set; }
        public virtual DbSet<LoteProcedimentoCcp> LoteProcedimentoCcp { get; set; }
        #endregion

        #region SGPPF
        public virtual DbSet<AccaoFormacao> AccaoFormacao { get; set; }
        public virtual DbSet<EntidadeFormadora> EntidadeFormadora { get; set; }
        public virtual DbSet<PedidoParticipacaoFormacao> PedidoParticipacaoFormacao { get; set; }
        public virtual DbSet<RegistoAlteracoesPedidoFormacao> RegistoAlteracoesPedidoFormacao { get; set; }
        public virtual DbSet<SessaoAccaoFormacao> SessaoAccaoFormacao { get; set; }
        public virtual DbSet<TemaFormacao> TemaFormacao { get; set; }
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AcessosDimensões>(entity =>
            {
                entity.HasKey(e => new { e.IdUtilizador, e.Dimensão, e.ValorDimensão });

                entity.ToTable("Acessos Dimensões");

                entity.Property(e => e.IdUtilizador)
                    .HasColumnName("Id Utilizador")
                    .HasMaxLength(50);

                entity.Property(e => e.ValorDimensão)
                    .HasColumnName("Valor Dimensão")
                    .HasMaxLength(20);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdUtilizadorNavigation)
                    .WithMany(p => p.AcessosDimensões)
                    .HasForeignKey(d => d.IdUtilizador)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Acessos Dimensões_Config. Utilizadores");
            });

            modelBuilder.Entity<AcessosLocalizacoes>(entity =>
            {
                entity.HasKey(e => new { e.IdUtilizador, e.Localizacao });

                entity.Property(e => e.IdUtilizador)
                    .HasColumnName("ID_Utilizador")
                    .HasMaxLength(50);

                entity.Property(e => e.Localizacao).HasMaxLength(20);

                entity.Property(e => e.DataHoraCriacao)
                    .HasColumnName("DataHora_Criacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificacao)
                    .HasColumnName("DataHora_Modificacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("Utilizador_Criacao")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("Utilizador_Modificacao")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<AcessosPerfil>(entity =>
            {
                entity.HasKey(e => new { e.IdPerfil, e.Área, e.Funcionalidade });

                entity.ToTable("Acessos Perfil");

                entity.Property(e => e.IdPerfil).HasColumnName("Id Perfil");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdPerfilNavigation)
                    .WithMany(p => p.AcessosPerfil)
                    .HasForeignKey(d => d.IdPerfil)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Acessos Perfil_Perfis Modelo");
            });

            modelBuilder.Entity<AcessosUtilizador>(entity =>
            {
                entity.HasKey(e => new { e.IdUtilizador, e.Funcionalidade });

                entity.ToTable("Acessos Utilizador");

                entity.Property(e => e.IdUtilizador)
                    .HasColumnName("Id Utilizador")
                    .HasMaxLength(50);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<AçõesDeConfeção>(entity =>
            {
                entity.HasKey(e => e.Código);

                entity.ToTable("Ações de Confeção");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(150);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<AcordoPrecos>(entity =>
            {
                entity.HasKey(e => e.NoProcedimento);

                entity.Property(e => e.NoProcedimento)
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.DtFim).HasColumnType("datetime");

                entity.Property(e => e.DtInicio).HasColumnType("datetime");
            });

            modelBuilder.Entity<Actividades>(entity =>
            {
                entity.HasKey(e => e.CodActividade);

                entity.Property(e => e.CodActividade)
                    .HasColumnName("Cod_Actividade")
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.Descricao).HasMaxLength(30);
            });

            modelBuilder.Entity<ActividadesPorFornecedor>(entity =>
            {
                entity.ToTable("Actividades_por_Fornecedor");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CodActividade)
                    .HasColumnName("Cod_Actividade")
                    .HasMaxLength(20);

                entity.Property(e => e.CodFornecedor)
                    .HasColumnName("Cod_Fornecedor")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<ActividadesPorProduto>(entity =>
            {
                entity.ToTable("Actividades_por_Produto");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CodActividade)
                    .HasColumnName("Cod_Actividade")
                    .HasMaxLength(20);

                entity.Property(e => e.CodProduto)
                    .HasColumnName("Cod_Produto")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Anexos>(entity =>
            {
                entity.HasKey(e => new { e.TipoOrigem, e.NºOrigem, e.NºLinha });

                entity.Property(e => e.TipoOrigem).HasColumnName("Tipo Origem");

                entity.Property(e => e.NºOrigem)
                    .HasColumnName("Nº Origem")
                    .HasMaxLength(20);

                entity.Property(e => e.NºLinha)
                    .HasColumnName("Nº Linha")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.UrlAnexo)
                    .HasColumnName("URL Anexo")
                    .HasMaxLength(200);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.NºOrigemNavigation)
                    .WithMany(p => p.Anexos)
                    .HasForeignKey(d => d.NºOrigem)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Anexos_Pré-Requisição");
            });

            modelBuilder.Entity<AnexosErros>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AlteradoPor).HasMaxLength(50);

                entity.Property(e => e.Codigo).HasMaxLength(50);

                entity.Property(e => e.CriadoPor).HasMaxLength(50);

                entity.Property(e => e.DataHoraAlteracao)
                    .HasColumnName("DataHora_Alteracao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraCriacao)
                    .HasColumnName("DataHora_Criacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.NomeAnexo).HasMaxLength(200);
            });

            modelBuilder.Entity<AutorizacaoFhRh>(entity =>
            {
                entity.HasKey(e => e.NoEmpregado);

                entity.ToTable("Autorizacao_FH_RH");

                entity.Property(e => e.NoEmpregado)
                    .HasColumnName("No_Empregado")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.AlteradoPor)
                    .HasColumnName("Alterado Por")
                    .HasMaxLength(50);

                entity.Property(e => e.CriadoPor)
                    .HasColumnName("Criado Por")
                    .HasMaxLength(50);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraÚltimaAlteração)
                    .HasColumnName("Data/Hora Última Alteração")
                    .HasColumnType("datetime");

                entity.Property(e => e.NoResponsavel1)
                    .HasColumnName("No_Responsavel_1")
                    .HasMaxLength(50);

                entity.Property(e => e.NoResponsavel2)
                    .HasColumnName("No_Responsavel_2")
                    .HasMaxLength(50);

                entity.Property(e => e.NoResponsavel3)
                    .HasColumnName("No_Responsavel_3")
                    .HasMaxLength(50);

                entity.Property(e => e.ValidadorRh1)
                    .HasColumnName("Validador_RH1")
                    .HasMaxLength(50);

                entity.Property(e => e.ValidadorRh2)
                    .HasColumnName("Validador_RH2")
                    .HasMaxLength(50);

                entity.Property(e => e.ValidadorRh3)
                    .HasColumnName("Validador_RH3")
                    .HasMaxLength(50);

                entity.Property(e => e.ValidadorRhkm1)
                    .HasColumnName("Validador_RHKM1")
                    .HasMaxLength(50);

                entity.Property(e => e.ValidadorRhkm2)
                    .HasColumnName("Validador_RHKM2")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<AutorizarFaturaçãoContratos>(entity =>
            {
                entity.HasKey(e => new { e.NºContrato, e.GrupoFatura });

                entity.ToTable("Autorizar Faturação Contratos");

                entity.Property(e => e.NºContrato)
                    .HasColumnName("Nº Contrato")
                    .HasMaxLength(20);

                entity.Property(e => e.GrupoFatura).HasColumnName("Grupo Fatura");

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataDeExpiração)
                    .HasColumnName("Data de Expiração")
                    .HasColumnType("date");

                entity.Property(e => e.DataDeRegisto)
                    .HasColumnName("Data de Registo")
                    .HasColumnType("date");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataInicial)
                    .HasColumnName("Data Inicial")
                    .HasColumnType("date");

                entity.Property(e => e.DataPróximaFatura)
                    .HasColumnName("Data Próxima Fatura")
                    .HasColumnType("date");

                entity.Property(e => e.Descrição).HasMaxLength(100);

                entity.Property(e => e.NºCliente)
                    .HasColumnName("Nº Cliente")
                    .HasMaxLength(20);

                entity.Property(e => e.NºDeFaturasAEmitir).HasColumnName("Nº de Faturas a Emitir");

                entity.Property(e => e.NãoFaturar).HasColumnName("Não Faturar");

                entity.Property(e => e.Situação).HasMaxLength(150);

                entity.Property(e => e.TotalAFaturar).HasColumnName("Total a Faturar");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.ValorDoContrato).HasColumnName("Valor do Contrato");

                entity.Property(e => e.ValorFaturado).HasColumnName("Valor Faturado");

                entity.Property(e => e.ValorPorFaturar).HasColumnName("Valor por Faturar");

                entity.Property(e => e.NoRequisicaoDoCliente)
                    .HasColumnName("NoRequisicaoDoCliente")
                    .HasMaxLength(30);

                entity.Property(e => e.DataRececaoRequisicao)
                    .HasColumnName("DataRececaoRequisicao")
                    .HasColumnType("date");

                entity.Property(e => e.NoCompromisso)
                    .HasColumnName("NoCompromisso")
                    .HasMaxLength(20);

                entity.Property(e => e.TextoFatura)
                    .HasColumnName("TextoFatura")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<BarramentosDeVoz>(entity =>
            {
                entity.HasKey(e => e.Código);

                entity.ToTable("Barramentos de Voz");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CafetariasRefeitórios>(entity =>
            {
                entity.HasKey(e => new { e.NºUnidadeProdutiva, e.Tipo, e.Código, e.DataInícioExploração });

                entity.ToTable("Cafetarias/Refeitórios");

                entity.Property(e => e.NºUnidadeProdutiva).HasColumnName("Nº Unidade Produtiva");

                entity.Property(e => e.Código).ValueGeneratedOnAdd();

                entity.Property(e => e.DataInícioExploração)
                    .HasColumnName("Data Início Exploração")
                    .HasColumnType("date");

                entity.Property(e => e.Armazém).HasMaxLength(10);

                entity.Property(e => e.ArmazémLocal)
                    .HasColumnName("Armazém Local")
                    .HasMaxLength(10);

                entity.Property(e => e.CódResponsável)
                    .HasColumnName("Cód. Responsável")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataFimExploração)
                    .HasColumnName("Data Fim Exploração")
                    .HasColumnType("date");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataModificação)
                    .HasColumnName("Data Modificação")
                    .HasColumnType("date");

                entity.Property(e => e.Descrição).HasMaxLength(50);

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.NºRefeições).HasColumnName("Nº Refeições");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.NºUnidadeProdutivaNavigation)
                    .WithMany(p => p.CafetariasRefeitórios)
                    .HasForeignKey(d => d.NºUnidadeProdutiva)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cafetarias/Refeitórios_Unidades Produtivas");
            });

            modelBuilder.Entity<CartõesEApólices>(entity =>
            {
                entity.HasKey(e => new { e.Tipo, e.Número });

                entity.ToTable("Cartões e Apólices");

                entity.Property(e => e.Número).HasMaxLength(25);

                entity.Property(e => e.DataFim)
                    .HasColumnName("Data Fim")
                    .HasColumnType("date");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataInício)
                    .HasColumnName("Data Início")
                    .HasColumnType("date");

                entity.Property(e => e.Descrição).HasMaxLength(50);

                entity.Property(e => e.Fornecedor).HasMaxLength(20);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CartõesTelemóveis>(entity =>
            {
                entity.HasKey(e => e.NúmeroTelemóvel);

                entity.ToTable("Cartões Telemóveis");

                entity.Property(e => e.NúmeroTelemóvel)
                    .HasColumnName("Número Telemóvel")
                    .HasMaxLength(9)
                    .ValueGeneratedNever();

                entity.Property(e => e.BarramentoDeVoz).HasColumnName("Barramento de Voz");

                entity.Property(e => e.ChamadasInternacionais).HasColumnName("Chamadas Internacionais");

                entity.Property(e => e.ContaSuch)
                    .HasColumnName("Conta SUCH")
                    .HasMaxLength(10);

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataAtribuição)
                    .HasColumnName("Data Atribuição")
                    .HasColumnType("date");

                entity.Property(e => e.DataFidelização)
                    .HasColumnName("Data Fidelização")
                    .HasColumnType("date");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.EquipamentoNãoDevolvido).HasColumnName("Equipamento não Devolvido");

                entity.Property(e => e.ExtraPlafond).HasColumnName("Extra Plafond");

                entity.Property(e => e.Gprs).HasColumnName("GPRS");

                entity.Property(e => e.Imei)
                    .HasColumnName("IMEI")
                    .HasMaxLength(16);

                entity.Property(e => e.NúmeroEmpregado)
                    .HasColumnName("Número Empregado")
                    .HasMaxLength(20);

                entity.Property(e => e.Observações).HasColumnType("text");

                entity.Property(e => e.Plafond100Utilizador).HasColumnName("Plafond 100% Utilizador");

                entity.Property(e => e.PlafondFr).HasColumnName("Plafond FR");

                entity.Property(e => e.TarifárioDeDados).HasColumnName("Tarifário de Dados");

                entity.Property(e => e.TipoDeServiço).HasColumnName("Tipo de Serviço");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.ValorMensalidade).HasColumnName("Valor Mensalidade");

                entity.HasOne(d => d.BarramentoDeVozNavigation)
                    .WithMany(p => p.CartõesTelemóveis)
                    .HasForeignKey(d => d.BarramentoDeVoz)
                    .HasConstraintName("FK_Cartões Telemóveis_Barramentos de Voz");

                entity.HasOne(d => d.TarifárioDeDadosNavigation)
                    .WithMany(p => p.CartõesTelemóveis)
                    .HasForeignKey(d => d.TarifárioDeDados)
                    .HasConstraintName("FK_Cartões Telemóveis_Tarifários");
            });

            modelBuilder.Entity<CatálogoManutenção>(entity =>
            {
                entity.HasKey(e => e.Código);

                entity.ToTable("Catálogo Manutenção");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ClassificaçãoFichasTécnicas>(entity =>
            {
                entity.HasKey(e => e.Código);

                entity.ToTable("Classificação Fichas Técnicas");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.GrupoNavigation)
                    .WithMany(p => p.InverseGrupoNavigation)
                    .HasForeignKey(d => d.Grupo)
                    .HasConstraintName("FK_Classificação Fichas Técnicas_Classificação Fichas Técnicas");
            });

            modelBuilder.Entity<Comprador>(entity =>
            {
                entity.HasKey(e => e.CodComprador);

                entity.Property(e => e.CodComprador)
                    .HasColumnName("Cod_Comprador")
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.NomeComprador)
                    .IsRequired()
                    .HasColumnName("Nome_Comprador")
                    .HasMaxLength(60);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Compras>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CodigoProduto).HasMaxLength(20);

                entity.Property(e => e.CodigoUnidadeMedida).HasMaxLength(10);

                entity.Property(e => e.DataConsultaMercado).HasColumnType("datetime");

                entity.Property(e => e.DataCriacao).HasColumnType("datetime");

                entity.Property(e => e.DataEncomenda).HasColumnType("datetime");

                entity.Property(e => e.DataMercadoLocal).HasColumnType("datetime");

                entity.Property(e => e.DataRecusa).HasColumnType("datetime");

                entity.Property(e => e.DataTratado).HasColumnType("datetime");

                entity.Property(e => e.DataValidacao).HasColumnType("datetime");

                entity.Property(e => e.Descricao).HasMaxLength(50);

                entity.Property(e => e.Descricao2).HasMaxLength(50);

                entity.Property(e => e.NoConsultaMercado).HasMaxLength(20);

                entity.Property(e => e.NoEncomenda).HasMaxLength(20);

                entity.Property(e => e.NoFornecedor).HasMaxLength(20);

                entity.Property(e => e.NoProjeto).HasMaxLength(20);

                entity.Property(e => e.NoRequisicao).HasMaxLength(20);

                entity.Property(e => e.RegiaoMercadoLocal).HasMaxLength(20);

                entity.Property(e => e.Responsaveis).HasMaxLength(250);

                entity.Property(e => e.UtilizadorCriacao).HasMaxLength(50);

                entity.Property(e => e.UtilizadorRecusa).HasMaxLength(50);

                entity.Property(e => e.UtilizadorTratado).HasMaxLength(50);

                entity.Property(e => e.UtilizadorValidacao).HasMaxLength(50);

                entity.HasOne(d => d.RegiaoMercadoLocalNavigation)
                    .WithMany(p => p.Compras)
                    .HasForeignKey(d => d.RegiaoMercadoLocal)
                    .HasConstraintName("FK_Compras_Config Mercado Local");
            });

            modelBuilder.Entity<CondicoesPropostasFornecedores>(entity =>
            {
                entity.HasKey(e => e.IdCondicaoPropostaFornecedores);

                entity.ToTable("Condicoes_Propostas_Fornecedores");

                entity.HasIndex(e => new { e.NumConsultaMercado, e.CodFornecedor, e.Alternativa })
                    .HasName("IX_Condicoes_Propostas_Fornecedores")
                    .IsUnique();

                entity.Property(e => e.IdCondicaoPropostaFornecedores).HasColumnName("ID_Condicao_Proposta_Fornecedores");

                entity.Property(e => e.Alternativa).HasMaxLength(20);

                entity.Property(e => e.CodActividade)
                    .HasColumnName("Cod_Actividade")
                    .HasMaxLength(20);

                entity.Property(e => e.CodFormaPagamento)
                    .HasColumnName("Cod_Forma_Pagamento")
                    .HasMaxLength(10);

                entity.Property(e => e.CodFornecedor)
                    .HasColumnName("Cod_Fornecedor")
                    .HasMaxLength(20);

                entity.Property(e => e.CodTermosPagamento)
                    .HasColumnName("Cod_Termos_Pagamento")
                    .HasMaxLength(10);

                entity.Property(e => e.DataProposta)
                    .HasColumnName("Data_Proposta")
                    .HasColumnType("date");

                entity.Property(e => e.EnviarPedidoProposta).HasColumnName("Enviar_Pedido_Proposta");

                entity.Property(e => e.FornecedorSelecionado).HasColumnName("Fornecedor_Selecionado");

                entity.Property(e => e.NomeFornecedor)
                    .HasColumnName("Nome_Fornecedor")
                    .HasMaxLength(100);

                entity.Property(e => e.NumConsultaMercado)
                    .HasColumnName("Num_Consulta_Mercado")
                    .HasMaxLength(20);

                entity.Property(e => e.NumProjecto)
                    .HasColumnName("Num_Projecto")
                    .HasMaxLength(20);

                entity.Property(e => e.ValidadeProposta)
                    .HasColumnName("Validade_Proposta")
                    .HasMaxLength(10);

                entity.HasOne(d => d.NumConsultaMercadoNavigation)
                    .WithMany(p => p.CondicoesPropostasFornecedores)
                    .HasForeignKey(d => d.NumConsultaMercado)
                    .HasConstraintName("FK_Condicoes_Propostas_Fornecedores_Consulta_Mercado");
            });

            modelBuilder.Entity<ConfigMercadoLocal>(entity =>
            {
                entity.HasKey(e => e.RegiaoMercadoLocal);

                entity.ToTable("Config Mercado Local");

                entity.Property(e => e.RegiaoMercadoLocal)
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.Responsavel1).HasMaxLength(50);

                entity.Property(e => e.Responsavel2).HasMaxLength(50);

                entity.Property(e => e.Responsavel3).HasMaxLength(50);

                entity.Property(e => e.Responsavel4).HasMaxLength(50);
            });

            modelBuilder.Entity<Configuração>(entity =>
            {
                entity.Property(e => e.ArmazemCompraDireta).HasMaxLength(50);

                entity.Property(e => e.CodAreaResiduos).HasMaxLength(20);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.NumeraçãoContactos).HasColumnName("Numeração Contactos");

                entity.Property(e => e.NumeraçãoContratos).HasColumnName("Numeração Contratos");

                entity.Property(e => e.NumeraçãoFichasTécnicasDePratos).HasColumnName("Numeração Fichas Técnicas de Pratos");

                entity.Property(e => e.NumeraçãoFolhasDeHoras).HasColumnName("Numeração Folhas de Horas");

                entity.Property(e => e.NumeraçãoOportunidades).HasColumnName("Numeração Oportunidades");

                entity.Property(e => e.NumeraçãoProcedimentoAquisição).HasColumnName("Numeração Procedimento Aquisição");

                entity.Property(e => e.NumeraçãoProcedimentoSimplificado).HasColumnName("Numeração Procedimento Simplificado");

                entity.Property(e => e.NumeraçãoProjetos).HasColumnName("Numeração Projetos");

                entity.Property(e => e.NumeraçãoPropostas).HasColumnName("Numeração Propostas");

                entity.Property(e => e.NumeraçãoPréRequisições).HasColumnName("Numeração Pré Requisições");

                entity.Property(e => e.NumeraçãoRequisiçõesSimplificada).HasColumnName("Numeração Requisições Simplificada");

                entity.Property(e => e.ReportPassword).HasMaxLength(60);

                entity.Property(e => e.ReportUsername).HasMaxLength(60);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                #region zpgm.SGPPF
                entity.Property(e => e.NumeracaoPedidoFormacao).HasColumnName("NumeracaoPedidoFormacao");

                entity.HasOne(d => d.NumeracaoPedidoFormacaoNavigation)
                    .WithMany(p => p.ConfiguracaoNumeracoPedidoFormacao)
                    .HasForeignKey(d => d.NumeracaoPedidoFormacao)
                    .HasConstraintName("FK_Configuração_NumeracaoPedidoFormacao");
                #endregion

                entity.HasOne(d => d.NumeraçãoContratosNavigation)
                    .WithMany(p => p.ConfiguraçãoNumeraçãoContratosNavigation)
                    .HasForeignKey(d => d.NumeraçãoContratos)
                    .HasConstraintName("FK_Configuração_Configuração Numerações1");

                entity.HasOne(d => d.NumeraçãoFolhasDeHorasNavigation)
                    .WithMany(p => p.ConfiguraçãoNumeraçãoFolhasDeHorasNavigation)
                    .HasForeignKey(d => d.NumeraçãoFolhasDeHoras)
                    .HasConstraintName("FK_Configuração_Configuração Numerações2");

                entity.HasOne(d => d.NumeraçãoProcedimentoAquisiçãoNavigation)
                    .WithMany(p => p.ConfiguraçãoNumeraçãoProcedimentoAquisiçãoNavigation)
                    .HasForeignKey(d => d.NumeraçãoProcedimentoAquisição)
                    .HasConstraintName("FK_Configuração_Configuração Numerações3");

                entity.HasOne(d => d.NumeraçãoProcedimentoSimplificadoNavigation)
                    .WithMany(p => p.ConfiguraçãoNumeraçãoProcedimentoSimplificadoNavigation)
                    .HasForeignKey(d => d.NumeraçãoProcedimentoSimplificado)
                    .HasConstraintName("FK_Configuração_Configuração Numerações4");

                entity.HasOne(d => d.NumeraçãoProjetosNavigation)
                    .WithMany(p => p.ConfiguraçãoNumeraçãoProjetosNavigation)
                    .HasForeignKey(d => d.NumeraçãoProjetos)
                    .HasConstraintName("FK_Configuração_Configuração Numerações");
            });

            modelBuilder.Entity<ConfiguracaoAjudaCusto>(entity =>
            {
                entity.HasKey(e => new { e.CodigoTipoCusto, e.CodigoRefCusto, e.DataChegadaDataPartida });

                entity.ToTable("Configuracao Ajuda Custo");

                entity.Property(e => e.CodigoTipoCusto).HasMaxLength(20);

                entity.Property(e => e.DataHoraCriacao).HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificacao).HasColumnType("datetime");

                entity.Property(e => e.UtilizadorCriacao).HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificacao).HasMaxLength(50);
            });

            modelBuilder.Entity<ConfiguraçãoAprovações>(entity =>
            {
                entity.ToTable("Configuração Aprovações");

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataFinal)
                    .HasColumnName("Data Final")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataInicial)
                    .HasColumnName("Data Inicial")
                    .HasColumnType("datetime");

                entity.Property(e => e.GrupoAprovação).HasColumnName("Grupo Aprovação");

                entity.Property(e => e.NívelAprovação).HasColumnName("Nível Aprovação");

                entity.Property(e => e.UtilizadorAprovação)
                    .HasColumnName("Utilizador Aprovação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.ValorAprovação).HasColumnName("Valor Aprovação");

                entity.HasOne(d => d.GrupoAprovaçãoNavigation)
                    .WithMany(p => p.ConfiguraçãoAprovações)
                    .HasForeignKey(d => d.GrupoAprovação)
                    .HasConstraintName("FK_Configuração Aprovações_Grupos Aprovação");
            });

            modelBuilder.Entity<ConfiguracaoCcp>(entity =>
            {
                entity.ToTable("ConfiguracaoCCP");

                entity.Property(e => e.Id).HasDefaultValueSql("((1))");

                entity.Property(e => e.Email10Compras).HasMaxLength(50);

                entity.Property(e => e.Email11Compras).HasMaxLength(50);

                entity.Property(e => e.Email2Compras).HasMaxLength(50);

                entity.Property(e => e.Email2Contabilidade).HasMaxLength(50);

                entity.Property(e => e.Email2Financeiros).HasMaxLength(50);

                entity.Property(e => e.Email2Juridicos).HasMaxLength(50);

                entity.Property(e => e.Email3Compras).HasMaxLength(50);

                entity.Property(e => e.Email3Contabilidade).HasMaxLength(50);

                entity.Property(e => e.Email4Compras).HasMaxLength(50);

                entity.Property(e => e.Email5Compras).HasMaxLength(50);

                entity.Property(e => e.Email6Compras).HasMaxLength(50);

                entity.Property(e => e.Email7Compras).HasMaxLength(50);

                entity.Property(e => e.Email8Compras).HasMaxLength(50);

                entity.Property(e => e.Email9Compras).HasMaxLength(50);

                entity.Property(e => e.EmailCa)
                    .HasColumnName("EmailCA")
                    .HasMaxLength(50);

                entity.Property(e => e.EmailCompras).HasMaxLength(50);

                entity.Property(e => e.EmailContabilidade).HasMaxLength(50);

                entity.Property(e => e.EmailFinanceiros).HasMaxLength(50);

                entity.Property(e => e.EmailJurididos).HasMaxLength(50);
            });

            modelBuilder.Entity<ConfiguraçãoEmailFornecedores>(entity =>
            {
                entity.HasKey(e => new { e.CodFornecedor, e.Cresp });

                entity.ToTable("Configuração Email Fornecedores");

                entity.Property(e => e.CodFornecedor).HasMaxLength(20);

                entity.Property(e => e.Cresp)
                    .HasColumnName("CResp")
                    .HasMaxLength(20);

                entity.Property(e => e.DataHoraCriacao).HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificacao).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(200);

                entity.Property(e => e.Nome).HasMaxLength(200);

                entity.Property(e => e.UtilizadorCriacao).HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificacao).HasMaxLength(50);
            });

            modelBuilder.Entity<ConfiguraçãoNumerações>(entity =>
            {
                entity.ToTable("Configuração Numerações");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(50);

                entity.Property(e => e.NºDígitosIncrementar).HasColumnName("Nº Dígitos Incrementar");

                entity.Property(e => e.Prefixo).HasMaxLength(10);

                entity.Property(e => e.QuantidadeIncrementar).HasColumnName("Quantidade Incrementar");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.ÚltimoNºUsado)
                    .HasColumnName("Último Nº Usado")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<ConfiguraçãoTemposCcp>(entity =>
            {
                entity.HasKey(e => e.Tipo);

                entity.ToTable("Configuração Tempos CCP");

                entity.Property(e => e.Tipo).ValueGeneratedNever();

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Estado0).HasColumnName("Estado 0");

                entity.Property(e => e.Estado1).HasColumnName("Estado 1");

                entity.Property(e => e.Estado10).HasColumnName("Estado 10");

                entity.Property(e => e.Estado11).HasColumnName("Estado 11");

                entity.Property(e => e.Estado12).HasColumnName("Estado 12");

                entity.Property(e => e.Estado13).HasColumnName("Estado 13");

                entity.Property(e => e.Estado14).HasColumnName("Estado 14");

                entity.Property(e => e.Estado15).HasColumnName("Estado 15");

                entity.Property(e => e.Estado16).HasColumnName("Estado 16");

                entity.Property(e => e.Estado17).HasColumnName("Estado 17");

                entity.Property(e => e.Estado18).HasColumnName("Estado 18");

                entity.Property(e => e.Estado19).HasColumnName("Estado 19");

                entity.Property(e => e.Estado2).HasColumnName("Estado 2");

                entity.Property(e => e.Estado20).HasColumnName("Estado 20");

                entity.Property(e => e.Estado3).HasColumnName("Estado 3");

                entity.Property(e => e.Estado4).HasColumnName("Estado 4");

                entity.Property(e => e.Estado5).HasColumnName("Estado 5");

                entity.Property(e => e.Estado6).HasColumnName("Estado 6");

                entity.Property(e => e.Estado7).HasColumnName("Estado 7");

                entity.Property(e => e.Estado8).HasColumnName("Estado 8");

                entity.Property(e => e.Estado9).HasColumnName("Estado 9");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ConfiguraçãoVendasAlertas>(entity =>
            {
                entity.ToTable("Configuração Vendas Alertas");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DataHoraCriacao).HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificacao).HasColumnType("datetime");

                entity.Property(e => e.Email1Regiao12).HasMaxLength(100);

                entity.Property(e => e.Email1Regiao23).HasMaxLength(100);

                entity.Property(e => e.Email1Regiao33).HasMaxLength(100);

                entity.Property(e => e.Email1Regiao43).HasMaxLength(100);

                entity.Property(e => e.Email2Regiao12).HasMaxLength(100);

                entity.Property(e => e.Email2Regiao23).HasMaxLength(100);

                entity.Property(e => e.Email2Regiao33).HasMaxLength(100);

                entity.Property(e => e.Email2Regiao43).HasMaxLength(100);

                entity.Property(e => e.UtilizadorCriacao).HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificacao).HasMaxLength(50);
            });

            modelBuilder.Entity<ConfigUtilizadores>(entity =>
            {
                entity.HasKey(e => e.IdUtilizador);

                entity.ToTable("Config. Utilizadores");

                entity.Property(e => e.IdUtilizador)
                    .HasColumnName("Id Utilizador")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.AreaPorDefeito)
                    .HasColumnName("Area por defeito")
                    .HasMaxLength(20);

                entity.Property(e => e.CentroDeResponsabilidade)
                    .HasColumnName("Centro de Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CentroRespPorDefeito)
                    .HasColumnName("Centro Resp por defeito")
                    .HasMaxLength(20);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.EmployeeNo).HasMaxLength(20);

                entity.Property(e => e.Nome).HasMaxLength(50);

                entity.Property(e => e.NumSerieFaturas)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NumSerieNotasCredito)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NumSerieNotasCreditoCompra)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NumSerieNotasDebito)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NumSeriePreFaturasCompra)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NumSeriePreFaturasCompraCf)
                    .HasColumnName("NumSeriePreFaturasCompraCF")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NumSeriePreFaturasCompraCp)
                    .HasColumnName("NumSeriePreFaturasCompraCP")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ProcedimentosEmailEnvioParaArea).HasMaxLength(50);

                entity.Property(e => e.ProcedimentosEmailEnvioParaArea2).HasMaxLength(50);

                entity.Property(e => e.ProcedimentosEmailEnvioParaCa)
                    .HasColumnName("ProcedimentosEmailEnvioParaCA")
                    .HasMaxLength(50);

                entity.Property(e => e.RegiãoPorDefeito)
                    .HasColumnName("Região por defeito")
                    .HasMaxLength(20);

                entity.Property(e => e.RfalterarDestinatarios).HasColumnName("RFAlterarDestinatarios");

                entity.Property(e => e.RffiltroArea)
                    .HasColumnName("RFFiltroArea")
                    .HasMaxLength(120);

                entity.Property(e => e.RfmailEnvio)
                    .HasColumnName("RFMailEnvio")
                    .HasMaxLength(60);

                entity.Property(e => e.RfnomeAbreviado)
                    .HasColumnName("RFNomeAbreviado")
                    .HasMaxLength(80);

                entity.Property(e => e.Rfperfil).HasColumnName("RFPerfil");

                entity.Property(e => e.RfperfilVisualizacao).HasColumnName("RFPerfilVisualizacao");

                entity.Property(e => e.RfrespostaContabilidade).HasColumnName("RFRespostaContabilidade");

                entity.Property(e => e.SuperiorHierarquico).HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.AprovadorPedidoPag1)
                    .HasColumnName("AprovadorPedidoPag1")
                    .HasMaxLength(50);

                entity.Property(e => e.AprovadorPedidoPag2)
                    .HasColumnName("AprovadorPedidoPag2")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ConsultaMercado>(entity =>
            {
                entity.HasKey(e => e.NumConsultaMercado);

                entity.ToTable("Consulta_Mercado");

                entity.Property(e => e.NumConsultaMercado)
                    .HasColumnName("Num_Consulta_Mercado")
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.AdjudicacaoEm)
                    .HasColumnName("Adjudicacao_Em")
                    .HasColumnType("datetime");

                entity.Property(e => e.AdjudicacaoPor)
                    .HasColumnName("Adjudicacao_Por")
                    .HasMaxLength(50);

                entity.Property(e => e.Amostra).HasDefaultValueSql("((0))");

                entity.Property(e => e.CodActividade)
                    .HasColumnName("Cod_Actividade")
                    .HasMaxLength(20);

                entity.Property(e => e.CodAreaFuncional)
                    .HasColumnName("Cod_Area_Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.CodCentroResponsabilidade)
                    .HasColumnName("Cod_Centro_Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CodComprador).HasMaxLength(10);

                entity.Property(e => e.CodFormaPagamento)
                    .HasColumnName("Cod_Forma_Pagamento")
                    .HasMaxLength(10);

                entity.Property(e => e.CodLocalizacao)
                    .HasColumnName("Cod_Localizacao")
                    .HasMaxLength(20);

                entity.Property(e => e.CodProjecto)
                    .HasColumnName("Cod_Projecto")
                    .HasMaxLength(20);

                entity.Property(e => e.CodRegiao)
                    .HasColumnName("Cod_Regiao")
                    .HasMaxLength(20);

                entity.Property(e => e.ConsultaEm)
                    .HasColumnName("Consulta_Em")
                    .HasColumnType("datetime");

                entity.Property(e => e.ConsultaPor)
                    .HasColumnName("Consulta_Por")
                    .HasMaxLength(50);

                entity.Property(e => e.DataEntregaArmazem).HasColumnType("datetime");

                entity.Property(e => e.DataEntregaFornecedor).HasColumnType("datetime");

                entity.Property(e => e.DataLimite)
                    .HasColumnName("Data_Limite")
                    .HasColumnType("date");

                entity.Property(e => e.DataPedidoCotacao)
                    .HasColumnName("Data_Pedido_Cotacao")
                    .HasColumnType("date");

                entity.Property(e => e.DataRecolha).HasColumnType("datetime");

                entity.Property(e => e.Descricao).HasMaxLength(100);

                entity.Property(e => e.EmailEnviado)
                    .HasColumnName("Email_Enviado")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Equipamento).HasDefaultValueSql("((0))");

                entity.Property(e => e.EspecificacaoTecnica).HasColumnName("Especificacao_Tecnica");

                entity.Property(e => e.FiltroActividade)
                    .HasColumnName("Filtro_Actividade")
                    .HasMaxLength(250);

                entity.Property(e => e.FornecedorSelecionado)
                    .HasColumnName("Fornecedor_Selecionado")
                    .HasMaxLength(20);

                entity.Property(e => e.Historico).HasDefaultValueSql("((0))");

                entity.Property(e => e.NegociacaoContratacaoEm)
                    .HasColumnName("Negociacao_Contratacao_Em")
                    .HasColumnType("datetime");

                entity.Property(e => e.NegociacaoContratacaoPor)
                    .HasColumnName("Negociacao_Contratacao_Por")
                    .HasMaxLength(50);

                entity.Property(e => e.NumDocumentoCompra)
                    .HasColumnName("Num_Documento_Compra")
                    .HasMaxLength(20);

                entity.Property(e => e.NumEncomenda)
                    .HasColumnName("Num_Encomenda")
                    .HasMaxLength(20);

                entity.Property(e => e.NumRequisicao)
                    .HasColumnName("Num_Requisicao")
                    .HasMaxLength(20);

                entity.Property(e => e.Obs).HasColumnType("text");

                entity.Property(e => e.PedidoCotacaoCriadoEm)
                    .HasColumnName("Pedido_Cotacao_Criado_Em")
                    .HasColumnType("datetime");

                entity.Property(e => e.PedidoCotacaoCriadoPor)
                    .HasColumnName("Pedido_Cotacao_Criado_Por")
                    .HasMaxLength(50);

                entity.Property(e => e.PedidoCotacaoOrigem)
                    .HasColumnName("Pedido_Cotacao_Origem")
                    .HasMaxLength(20);

                entity.Property(e => e.RegiaoMercadoLocal).HasMaxLength(20);

                entity.Property(e => e.SeleccaoEfectuada).HasColumnName("Seleccao_Efectuada");

                entity.Property(e => e.Urgente).HasDefaultValueSql("((0))");

                entity.Property(e => e.UtilizadorRequisicao)
                    .HasColumnName("Utilizador_Requisicao")
                    .HasMaxLength(50);

                entity.Property(e => e.ValorAdjudicado).HasColumnName("Valor_Adjudicado");

                entity.Property(e => e.ValorPedidoCotacao).HasColumnName("Valor_Pedido_Cotacao");

                entity.Property(e => e.DataHoraCriacao)
                    .HasColumnName("DataHoraCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraAlteracao)
                    .HasColumnName("DataHoraAlteracao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ConsultaPendentes>(entity =>
            {
                entity.HasKey(e => new { e.IdUser, e.TipoDocumento, e.NoDocumento });

                entity.ToTable("Consulta Pendentes");

                entity.Property(e => e.IdUser).HasMaxLength(50);

                entity.Property(e => e.NoDocumento).HasMaxLength(20);

                entity.Property(e => e.Area).HasMaxLength(20);

                entity.Property(e => e.CentroResponsabilidade).HasMaxLength(20);

                entity.Property(e => e.Cliente).HasMaxLength(20);

                entity.Property(e => e.ConsultaMercado).HasMaxLength(20);

                entity.Property(e => e.DataAprovacao).HasColumnType("date");

                entity.Property(e => e.DataConsultaMercado).HasColumnType("date");

                entity.Property(e => e.DataCriacao).HasColumnType("date");

                entity.Property(e => e.DataDisponibilizado).HasColumnType("date");

                entity.Property(e => e.DataEncomenda).HasColumnType("date");

                entity.Property(e => e.DataEntregaArmazem).HasColumnType("date");

                entity.Property(e => e.DataEnvioArea).HasColumnType("date");

                entity.Property(e => e.DataEnvioMercadoLocal).HasColumnType("date");

                entity.Property(e => e.DataHoraAlteracao).HasColumnType("date");

                entity.Property(e => e.DataHoraCriacao).HasColumnType("date");

                entity.Property(e => e.DataRecebimento).HasColumnType("date");

                entity.Property(e => e.DataRececaoArea).HasColumnType("date");

                entity.Property(e => e.DataRececaoEsperada).HasColumnType("date");

                entity.Property(e => e.DataRespostaFornecedor).HasColumnType("date");

                entity.Property(e => e.DataValidacao).HasColumnType("date");

                entity.Property(e => e.EstadoRequisicao).HasMaxLength(20);

                entity.Property(e => e.Fornecedor).HasMaxLength(20);

                entity.Property(e => e.FornecedorBloqueado).HasDefaultValueSql("((0))");

                entity.Property(e => e.NoEncomenda).HasMaxLength(20);

                entity.Property(e => e.NoProjeto).HasMaxLength(20);

                entity.Property(e => e.NomeCliente).HasMaxLength(100);

                entity.Property(e => e.NomeCliente2).HasMaxLength(100);

                entity.Property(e => e.NomeFornecedor).HasMaxLength(100);

                entity.Property(e => e.NomeFornecedor2).HasMaxLength(100);

                entity.Property(e => e.Regiao).HasMaxLength(20);

                entity.Property(e => e.RegistoMercadoLocal).HasMaxLength(20);

                entity.Property(e => e.Requisicao).HasMaxLength(20);

                entity.Property(e => e.UserAprovacao).HasMaxLength(50);

                entity.Property(e => e.UserCriacao).HasMaxLength(50);

                entity.Property(e => e.UserValidacao).HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriacao).HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificacao).HasMaxLength(50);
            });

            modelBuilder.Entity<Contactos>(entity =>
            {
                entity.HasKey(e => e.No);

                entity.Property(e => e.No)
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.NoCliente).HasMaxLength(20);

                entity.Property(e => e.Nome).HasMaxLength(100);

                entity.Property(e => e.Telefone).HasMaxLength(30);

                entity.Property(e => e.Telemovel).HasMaxLength(30);

                entity.Property(e => e.Fax).HasMaxLength(30);

                entity.Property(e => e.Email).HasMaxLength(80);

                entity.Property(e => e.Pessoa).HasMaxLength(100);

                entity.Property(e => e.Notas).HasColumnType("text");

                entity.Property(e => e.CriadoPor).HasMaxLength(50);

                entity.Property(e => e.DataCriacao).HasColumnType("datetime");

                entity.Property(e => e.AlteradoPor).HasMaxLength(50);

                entity.Property(e => e.DataAlteracao).HasColumnType("datetime");
            });

            modelBuilder.Entity<ContactosServicos>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Contactos Servicos");

                entity.Property(e => e.Servico).HasMaxLength(500);

                entity.Property(e => e.CriadoPor).HasMaxLength(50);

                entity.Property(e => e.DataCriacao).HasColumnType("datetime");

                entity.Property(e => e.AlteradoPor).HasMaxLength(50);

                entity.Property(e => e.DataAlteracao).HasColumnType("datetime");
            });

            modelBuilder.Entity<ContactosFuncoes>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Contactos Funcoes");

                entity.Property(e => e.Funcao).HasMaxLength(500);

                entity.Property(e => e.CriadoPor).HasMaxLength(50);

                entity.Property(e => e.DataCriacao).HasColumnType("datetime");

                entity.Property(e => e.AlteradoPor).HasMaxLength(50);

                entity.Property(e => e.DataAlteracao).HasColumnType("datetime");
            });

            modelBuilder.Entity<Contratos>(entity =>
            {
                entity.HasKey(e => new { e.TipoContrato, e.NºDeContrato, e.NºVersão });

                entity.Property(e => e.TipoContrato).HasColumnName("Tipo Contrato");

                entity.Property(e => e.NºDeContrato)
                    .HasColumnName("Nº de Contrato")
                    .HasMaxLength(20);

                entity.Property(e => e.NºVersão).HasColumnName("Nº Versão");

                entity.Property(e => e.AssinadoPeloCliente).HasColumnName("Assinado pelo Cliente");

                entity.Property(e => e.AudiênciaPrévia)
                    .HasColumnName("Audiência Prévia")
                    .HasColumnType("datetime");

                entity.Property(e => e.CondiçõesPagamento).HasColumnName("Condições Pagamento");

                entity.Property(e => e.CondiçõesPagamentoOutra)
                    .HasColumnName("Condições Pagamento Outra")
                    .HasMaxLength(20);

                entity.Property(e => e.CondiçõesParaRenovação).HasColumnName("Condições para Renovação");

                entity.Property(e => e.CondiçõesRenovaçãoOutra)
                    .HasColumnName("Condições Renovação Outra")
                    .HasMaxLength(20);

                entity.Property(e => e.ContratoAvençaFixa).HasColumnName("Contrato Avença Fixa");

                entity.Property(e => e.ContratoAvençaVariável).HasColumnName("Contrato Avença Variável");

                entity.Property(e => e.CódEndereçoEnvio)
                    .HasColumnName("Cód. Endereço Envio")
                    .HasMaxLength(10);

                entity.Property(e => e.CódTermosPagamento)
                    .HasColumnName("Cód. Termos Pagamento")
                    .HasMaxLength(10);

                entity.Property(e => e.CódFormaPagamento)
                    .HasColumnName("Cód. Forma Pagamento")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataAlteraçãoProposta)
                    .HasColumnName("Data Alteração Proposta")
                    .HasColumnType("date");

                entity.Property(e => e.DataDaAssinatura)
                    .HasColumnName("Data da Assinatura")
                    .HasColumnType("date");

                entity.Property(e => e.DataEnvioCliente)
                    .HasColumnName("Data Envio Cliente")
                    .HasColumnType("date");

                entity.Property(e => e.DataEstadoProposta)
                    .HasColumnName("Data Estado Proposta")
                    .HasColumnType("date");

                entity.Property(e => e.DataExpiração)
                    .HasColumnName("Data Expiração")
                    .HasColumnType("date");

                entity.Property(e => e.DataFimContrato)
                    .HasColumnName("Data Fim Contrato")
                    .HasColumnType("date");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraEntregaProposta)
                    .HasColumnName("Data/Hora Entrega Proposta")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraErrosEOmissões)
                    .HasColumnName("Data/Hora Erros e Omissões")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraHabilitaçãoDocumental)
                    .HasColumnName("Data/Hora Habilitação Documental")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraLimiteEsclarecimentos)
                    .HasColumnName("Data/Hora Limite Esclarecimentos")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraRelatórioFinal)
                    .HasColumnName("Data/Hora Relatório Final")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataInicial)
                    .HasColumnName("Data Inicial")
                    .HasColumnType("date");

                entity.Property(e => e.DataInício1ºContrato)
                    .HasColumnName("Data Início 1º Contrato")
                    .HasColumnType("date");

                entity.Property(e => e.DataInícioContrato)
                    .HasColumnName("Data Início Contrato")
                    .HasColumnType("date");

                entity.Property(e => e.DataReceçãoRequisição)
                    .HasColumnName("Data Receção Requisição")
                    .HasColumnType("date");

                entity.Property(e => e.DescOrigemDoPedido)
                    .HasColumnName("Desc. Origem do Pedido")
                    .HasMaxLength(50);

                entity.Property(e => e.Descrição).HasMaxLength(100);

                entity.Property(e => e.DescriçãoDuraçãoContrato)
                    .HasColumnName("Descrição Duração Contrato")
                    .HasMaxLength(4000);

                entity.Property(e => e.DuraçãoMáxContrato)
                    .HasColumnName("Duração Máx. Contrato")
                    .HasColumnType("date");

                entity.Property(e => e.EnvioACódPostal)
                    .HasColumnName("Envio-a Cód. Postal")
                    .HasMaxLength(20);

                entity.Property(e => e.EnvioAEndereço)
                    .HasColumnName("Envio-a Endereço")
                    .HasMaxLength(50);

                entity.Property(e => e.EnvioAEndereço2)
                    .HasColumnName("Envio-a Endereço2")
                    .HasMaxLength(50);

                entity.Property(e => e.EnvioALocalidade)
                    .HasColumnName("Envio-a Localidade")
                    .HasMaxLength(30);

                entity.Property(e => e.EnvioANome)
                    .HasColumnName("Envio-a Nome")
                    .HasMaxLength(50);

                entity.Property(e => e.EnvioANome2)
                    .HasColumnName("Envio-a Nome2")
                    .HasMaxLength(50);

                entity.Property(e => e.EstadoAlteração).HasColumnName("Estado Alteração");

                entity.Property(e => e.JuntarFaturas).HasColumnName("Juntar Faturas");

                entity.Property(e => e.LinhasContratoEmFact).HasColumnName("Linhas Contrato em Fact.");

                entity.Property(e => e.LocalArquivoFísico)
                    .HasColumnName("Local Arquivo Físico")
                    .HasMaxLength(50);

                entity.Property(e => e.Mc)
                    .HasColumnName("% MC")
                    .HasColumnType("decimal(, 20)");

                entity.Property(e => e.Notas)
                    .HasMaxLength(4000);

                entity.Property(e => e.NumeraçãoInterna)
                    .HasColumnName("Numeração Interna")
                    .HasMaxLength(20);

                entity.Property(e => e.NºCliente)
                    .HasColumnName("Nº Cliente")
                    .HasMaxLength(20);

                entity.Property(e => e.NºComprimissoObrigatório).HasColumnName("Nº Comprimisso Obrigatório");

                entity.Property(e => e.NºCompromisso)
                    .HasColumnName("Nº Compromisso")
                    .HasMaxLength(50);

                entity.Property(e => e.NºContato)
                    .HasColumnName("Nº Contato")
                    .HasMaxLength(20);

                entity.Property(e => e.NºContrato)
                    .HasColumnName("Nº Contrato")
                    .HasMaxLength(20);

                entity.Property(e => e.NºOportunidade)
                    .HasColumnName("Nº Oportunidade")
                    .HasMaxLength(20);

                entity.Property(e => e.NºProposta)
                    .HasColumnName("Nº Proposta")
                    .HasMaxLength(20);

                entity.Property(e => e.NºRequisiçãoDoCliente)
                    .HasColumnName("Nº Requisição do Cliente")
                    .HasMaxLength(30);

                entity.Property(e => e.NºVep)
                    .HasColumnName("Nº VEP")
                    .HasMaxLength(20);

                entity.Property(e => e.ObjetoServiço).HasColumnName("Objeto Serviço");

                entity.Property(e => e.OrigemDoPedido).HasColumnName("Origem do Pedido");

                entity.Property(e => e.PeríodoFatura).HasColumnName("Período Fatura");

                entity.Property(e => e.PróximaDataFatura)
                    .HasColumnName("Próxima Data Fatura")
                    .HasColumnType("date");

                entity.Property(e => e.PróximoPeríodoFact)
                    .HasColumnName("Próximo Período Fact.")
                    .HasMaxLength(30);

                entity.Property(e => e.RazãoArquivo)
                    .HasColumnName("Razão Arquivo")
                    .HasMaxLength(2000);

                entity.Property(e => e.Referência1ºContrato)
                    .HasColumnName("Referência 1º Contrato")
                    .HasMaxLength(30);

                entity.Property(e => e.ReferênciaContrato)
                    .HasColumnName("Referência Contrato")
                    .HasMaxLength(20);

                entity.Property(e => e.RescisãoPrazoAviso).HasColumnName("Rescisão (Prazo Aviso)");

                entity.Property(e => e.TaxaAprovisionamento)
                    .HasColumnName("Taxa Aprovisionamento")
                    .HasColumnType("decimal(, 20)");

                entity.Property(e => e.TaxaDeslocação)
                    .HasColumnName("Taxa Deslocação")
                    .HasColumnType("decimal(, 20)");

                entity.Property(e => e.TipoContratoManut).HasColumnName("Tipo Contrato Manut.");

                entity.Property(e => e.TipoFaturação).HasColumnName("Tipo Faturação");

                entity.Property(e => e.TipoProposta).HasColumnName("Tipo Proposta");

                entity.Property(e => e.UnidadePrestação).HasColumnName("Unidade Prestação");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.ValorBaseProcedimento)
                    .HasColumnName("Valor Base Procedimento")
                    .HasColumnType("decimal(, 20)");

                entity.Property(e => e.ValorTotalProposta)
                    .HasColumnName("Valor Total Proposta")
                    .HasColumnType("decimal(, 20)");

                entity.Property(e => e.ÚltimaDataFatura)
                    .HasColumnName("Última Data Fatura")
                    .HasColumnType("date");

                entity.Property(e => e.TextoFatura)
                    .HasColumnName("TextoFatura")
                    .HasMaxLength(250);

                entity.Property(e => e.FaturaPrecosIvaIncluido).HasColumnName("FaturaPrecosIvaIncluido");


                entity.HasOne(d => d.ObjetoServiçoNavigation)
                    .WithMany(p => p.Contratos)
                    .HasForeignKey(d => d.ObjetoServiço)
                    .HasConstraintName("FK_Contratos_Objetos de Serviço");
            });

            modelBuilder.Entity<DestinosFinaisResíduos>(entity =>
            {
                entity.HasKey(e => e.Código);

                entity.ToTable("Destinos Finais Resíduos");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<DiárioCafetariaRefeitório>(entity =>
            {
                entity.HasKey(e => e.NºLinha);

                entity.ToTable("Diário Cafetaria/Refeitório");

                entity.Property(e => e.NºLinha).HasColumnName("Nº Linha");

                entity.Property(e => e.CódigoCafetariaRefeitório).HasColumnName("Código Cafetaria/Refeitório");

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataRegisto)
                    .HasColumnName("Data Registo")
                    .HasColumnType("date");

                entity.Property(e => e.Descrição).HasMaxLength(50);

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.NºRecurso)
                    .HasColumnName("Nº Recurso")
                    .HasMaxLength(20);

                entity.Property(e => e.NºUnidadeProdutiva).HasColumnName("Nº Unidade Produtiva");

                entity.Property(e => e.TipoMovimento).HasColumnName("Tipo Movimento");

                entity.Property(e => e.TipoRefeição).HasColumnName("Tipo Refeição");

                entity.Property(e => e.Utilizador).HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.NºProjetoNavigation)
                    .WithMany(p => p.DiárioCafetariaRefeitório)
                    .HasForeignKey(d => d.NºProjeto)
                    .HasConstraintName("FK_Diário Cafetaria/Refeitório_Projetos");

                entity.HasOne(d => d.NºUnidadeProdutivaNavigation)
                    .WithMany(p => p.DiárioCafetariaRefeitório)
                    .HasForeignKey(d => d.NºUnidadeProdutiva)
                    .HasConstraintName("FK_Diário Cafetaria/Refeitório_Unidades Produtivas");

                entity.HasOne(d => d.TipoRefeiçãoNavigation)
                    .WithMany(p => p.DiárioCafetariaRefeitório)
                    .HasForeignKey(d => d.TipoRefeição)
                    .HasConstraintName("FK_Diário Cafetaria/Refeitório_Tipos Refeição");
            });

            modelBuilder.Entity<DiárioDeProjeto>(entity =>
            {
                entity.HasKey(e => e.NºLinha);

                entity.ToTable("Diário de Projeto");

                entity.Property(e => e.NºLinha).HasColumnName("Nº Linha");

                entity.Property(e => e.AcertoDePreços).HasColumnName("Acerto de Preços");

                entity.Property(e => e.CustoTotal).HasColumnName("Custo Total");

                entity.Property(e => e.CustoUnitário)
                    .HasColumnName("Custo Unitário")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.CódDestinoFinalResíduos).HasColumnName("Cód. Destino Final Resíduos");

                entity.Property(e => e.CódGrupoServiço)
                    .HasColumnName("Cód. Grupo Serviço")
                    .HasMaxLength(20);

                entity.Property(e => e.CódLocalização)
                    .HasColumnName("Cód. Localização")
                    .HasMaxLength(10);

                entity.Property(e => e.CódServiçoCliente)
                    .HasColumnName("Cód Serviço Cliente")
                    .HasMaxLength(20);

                entity.Property(e => e.CódUnidadeMedida)
                    .HasColumnName("Cód. Unidade Medida")
                    .HasMaxLength(10);

                entity.Property(e => e.Código).HasMaxLength(20);

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.Data).HasColumnType("date");

                entity.Property(e => e.DataAutorizaçãoFaturação)
                    .HasColumnName("Data Autorização Faturação")
                    .HasColumnType("date");

                entity.Property(e => e.DataConsumo)
                    .HasColumnName("Data Consumo")
                    .HasColumnType("date");

                entity.Property(e => e.DataDocumentoCorrigido)
                    .HasColumnName("Data Documento Corrigido")
                    .HasColumnType("date");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(100);

                entity.Property(e => e.DocumentoCorrigido)
                    .HasColumnName("Documento Corrigido")
                    .HasMaxLength(20);

                entity.Property(e => e.DocumentoOriginal)
                    .HasColumnName("Documento Original")
                    .HasMaxLength(20);

                entity.Property(e => e.FaturaANºCliente)
                    .HasColumnName("Fatura-a Nº Cliente")
                    .HasMaxLength(20);

                entity.Property(e => e.FaturaçãoAutorizada).HasColumnName("Faturação Autorizada");

                entity.Property(e => e.GrupoContabProjeto)
                    .HasColumnName("Grupo Contab. Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.Moeda).HasMaxLength(20);

                entity.Property(e => e.Motorista).HasMaxLength(80);

                entity.Property(e => e.NºDocumento)
                    .HasColumnName("Nº Documento")
                    .HasMaxLength(20);

                entity.Property(e => e.NºFolhaHoras)
                    .HasColumnName("Nº Folha Horas")
                    .HasMaxLength(20);

                entity.Property(e => e.NºFuncionário)
                    .HasColumnName("Nº Funcionário")
                    .HasMaxLength(20);

                entity.Property(e => e.NºGuiaExterna)
                    .HasColumnName("Nº Guia Externa")
                    .HasMaxLength(80);

                entity.Property(e => e.NºGuiaResíduos)
                    .HasColumnName("Nº Guia Resíduos")
                    .HasMaxLength(80);

                entity.Property(e => e.NºLinhaRequisição).HasColumnName("Nº Linha Requisição");

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.NºRequisição)
                    .HasColumnName("Nº Requisição")
                    .HasMaxLength(20);

                entity.Property(e => e.PreçoTotal).HasColumnName("Preço Total");

                entity.Property(e => e.PreçoUnitário)
                    .HasColumnName("Preço Unitário")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.PréRegisto).HasColumnName("Pré-Registo");

                entity.Property(e => e.QuantidadeDevolvida).HasColumnName("Quantidade Devolvida");

                entity.Property(e => e.RequisiçãoInterna)
                    .HasColumnName("Requisição Interna")
                    .HasMaxLength(30);

                entity.Property(e => e.TipoMovimento).HasColumnName("Tipo Movimento");

                entity.Property(e => e.TipoRecurso).HasColumnName("Tipo Recurso");

                entity.Property(e => e.TipoRefeição).HasColumnName("Tipo Refeição");

                entity.Property(e => e.Utilizador).HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.ValorUnitárioAFaturar).HasColumnName("Valor Unitário a Faturar");

                entity.HasOne(d => d.CódDestinoFinalResíduosNavigation)
                    .WithMany(p => p.DiárioDeProjeto)
                    .HasForeignKey(d => d.CódDestinoFinalResíduos)
                    .HasConstraintName("FK_Diário de Projeto_Destinos Finais Resíduos");

                entity.HasOne(d => d.NºProjetoNavigation)
                    .WithMany(p => p.DiárioDeProjeto)
                    .HasForeignKey(d => d.NºProjeto)
                    .HasConstraintName("FK_Diário de Projeto_Projetos");

                entity.HasOne(d => d.NºRequisiçãoNavigation)
                    .WithMany(p => p.DiárioDeProjeto)
                    .HasForeignKey(d => d.NºRequisição)
                    .HasConstraintName("FK_Diário de Projeto_Requisição");

                entity.HasOne(d => d.TipoRefeiçãoNavigation)
                    .WithMany(p => p.DiárioDeProjeto)
                    .HasForeignKey(d => d.TipoRefeição)
                    .HasConstraintName("FK_Diário de Projeto_Tipos Refeição");
            });

            modelBuilder.Entity<DiárioDesperdíciosAlimentares>(entity =>
            {
                entity.HasKey(e => e.NºLinha);

                entity.ToTable("Diário Desperdícios Alimentares");

                entity.Property(e => e.NºLinha).HasColumnName("Nº Linha");

                entity.Property(e => e.CódLocalização)
                    .HasColumnName("Cód. Localização")
                    .HasMaxLength(10);

                entity.Property(e => e.CódUnidadeMedida)
                    .HasColumnName("Cód. Unidade Medida")
                    .HasMaxLength(10);

                entity.Property(e => e.Código).HasMaxLength(20);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(100);

                entity.Property(e => e.NºUnidadeProdutiva).HasColumnName("Nº Unidade Produtiva");

                entity.Property(e => e.TipoRefeição).HasColumnName("Tipo Refeição");

                entity.Property(e => e.Utilizador).HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.ValorCusto).HasColumnName("Valor Custo");

                entity.Property(e => e.ValorVenda).HasColumnName("Valor Venda");

                entity.HasOne(d => d.NºUnidadeProdutivaNavigation)
                    .WithMany(p => p.DiárioDesperdíciosAlimentares)
                    .HasForeignKey(d => d.NºUnidadeProdutiva)
                    .HasConstraintName("FK_Diário Desperdícios Alimentares_Unidades Produtivas");

                entity.HasOne(d => d.TipoRefeiçãoNavigation)
                    .WithMany(p => p.DiárioDesperdíciosAlimentares)
                    .HasForeignKey(d => d.TipoRefeição)
                    .HasConstraintName("FK_Diário Desperdícios Alimentares_Tipos Refeição");
            });

            modelBuilder.Entity<DiárioMovimentosViaturas>(entity =>
            {
                entity.HasKey(e => e.NºLinha);

                entity.ToTable("Diário Movimentos Viaturas");

                entity.Property(e => e.NºLinha).HasColumnName("Nº Linha");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataRegisto)
                    .HasColumnName("Data Registo")
                    .HasColumnType("date");

                entity.Property(e => e.Descrição).HasMaxLength(100);

                entity.Property(e => e.Matrícula).HasMaxLength(20);

                entity.Property(e => e.Recurso).HasMaxLength(20);

                entity.Property(e => e.TipoMovimento).HasColumnName("Tipo Movimento");

                entity.Property(e => e.Utilizador).HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.MatrículaNavigation)
                    .WithMany(p => p.DiárioMovimentosViaturas)
                    .HasForeignKey(d => d.Matrícula)
                    .HasConstraintName("FK_Diário Movimentos Viaturas_Viaturas");
            });

            modelBuilder.Entity<DiárioRequisiçãoUnidProdutiva>(entity =>
            {
                entity.HasKey(e => e.NºLinha);

                entity.ToTable("Diário Requisição Unid. Produtiva");

                entity.Property(e => e.NºLinha).HasColumnName("Nº Linha");

                entity.Property(e => e.CodigoLocalização)
                    .HasColumnName("Codigo Localização")
                    .HasMaxLength(10);

                entity.Property(e => e.CodigoProdutoFornecedor)
                    .HasColumnName("Codigo Produto Fornecedor")
                    .HasMaxLength(50);

                entity.Property(e => e.CustoUnitárioDireto).HasColumnName("Custo Unitário Direto");

                entity.Property(e => e.CódUnidadeMedida)
                    .HasColumnName("Cód. Unidade Medida")
                    .HasMaxLength(10);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataPPreçoFornecedor)
                    .HasColumnName("Data p/ Preço Fornecedor")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataReceçãoEsperada)
                    .HasColumnName("Data Receção Esperada")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(100);

                entity.Property(e => e.Descricao2).HasMaxLength(100);

                entity.Property(e => e.GrupoRegistoIvaProduto).HasMaxLength(10);

                entity.Property(e => e.DescriçãoProdutoFornecedor)
                    .HasColumnName("Descrição Produto Fornecedor")
                    .HasMaxLength(80);

                entity.Property(e => e.DescriçãoUnidadeProduto)
                    .HasColumnName("Descrição Unidade Produto")
                    .HasMaxLength(50);

                entity.Property(e => e.NomeFornecedor)
                    .HasColumnName("Nome Fornecedor")
                    .HasMaxLength(100);

                entity.Property(e => e.NomeSubFornecedor)
                    .HasColumnName("NomeSubFornecedor")
                    .HasMaxLength(100);

                entity.Property(e => e.NºDocumento)
                    .HasColumnName("Nº Documento")
                    .HasMaxLength(20);

                entity.Property(e => e.NºEncomendaAberto)
                    .HasColumnName("Nº Encomenda Aberto")
                    .HasMaxLength(20);

                entity.Property(e => e.NºFornecedor)
                    .HasColumnName("Nº Fornecedor")
                    .HasMaxLength(20);

                entity.Property(e => e.NoSubFornecedor)
                    .HasColumnName("NoSubFornecedor")
                    .HasMaxLength(20);

                entity.Property(e => e.NºLinhaEncomendaAberto)
                    .HasColumnName("Nº Linha Encomenda Aberto")
                    .HasMaxLength(20);

                entity.Property(e => e.NºProduto)
                    .HasColumnName("Nº Produto")
                    .HasMaxLength(20);

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.NºUnidadeProdutiva).HasColumnName("Nº Unidade Produtiva");

                entity.Property(e => e.Observações).HasColumnType("text");

                entity.Property(e => e.QuantidadePorUnidMedida).HasColumnName("Quantidade por Unid. Medida");

                entity.Property(e => e.TabelaPreçosFornecedor).HasColumnName("Tabela Preços Fornecedor");

                entity.Property(e => e.TipoRefeição).HasColumnName("Tipo Refeição");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.NºUnidadeProdutivaNavigation)
                    .WithMany(p => p.DiárioRequisiçãoUnidProdutiva)
                    .HasForeignKey(d => d.NºUnidadeProdutiva)
                    .HasConstraintName("FK_Diário Requisição Unid. Produtiva_Unidades Produtivas");

                entity.HasOne(d => d.TipoRefeiçãoNavigation)
                    .WithMany(p => p.DiárioRequisiçãoUnidProdutiva)
                    .HasForeignKey(d => d.TipoRefeição)
                    .HasConstraintName("FK_Diário Requisição Unid. Produtiva_Tipos Refeição");
            });

            modelBuilder.Entity<DistanciaFh>(entity =>
            {
                entity.HasKey(e => new { e.CódigoOrigem, e.CódigoDestino });

                entity.ToTable("Distancia_FH");

                entity.Property(e => e.CódigoOrigem)
                    .HasColumnName("Código_Origem")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoDestino)
                    .HasColumnName("Código_Destino")
                    .HasMaxLength(20);

                entity.Property(e => e.AlteradoPor)
                    .HasColumnName("Alterado Por")
                    .HasMaxLength(50);

                entity.Property(e => e.CriadoPor)
                    .HasColumnName("Criado Por")
                    .HasMaxLength(50);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraÚltimaAlteração)
                    .HasColumnName("Data/Hora Última Alteração")
                    .HasColumnType("datetime");

                entity.Property(e => e.Distância).HasColumnType("decimal(, 20)");
            });

            modelBuilder.Entity<DistribuiçãoCustoFolhaDeHoras>(entity =>
            {
                entity.HasKey(e => new { e.NºFolhasDeHoras, e.NºLinhaPercursosEAjudasCustoDespesas, e.NºLinha });

                entity.ToTable("Distribuição Custo Folha de Horas");

                entity.Property(e => e.NºFolhasDeHoras)
                    .HasColumnName("Nº Folhas de Horas")
                    .HasMaxLength(20);

                entity.Property(e => e.NºLinhaPercursosEAjudasCustoDespesas).HasColumnName("Nº Linha Percursos e Ajudas Custo/Despesas");

                entity.Property(e => e.NºLinha)
                    .HasColumnName("Nº Linha")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.NºObra)
                    .HasColumnName("Nº Obra")
                    .HasMaxLength(20);

                entity.Property(e => e.TipoObra).HasColumnName("Tipo Obra");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.PercentagemValor).HasColumnName("% Valor");

                entity.Property(e => e.Valor).HasColumnName("Valor");

                entity.Property(e => e.TotalValor).HasColumnName("TotalValor");

                entity.Property(e => e.TotalPercentagemValor).HasColumnName("Total%Valor");

                entity.Property(e => e.KmTotais).HasColumnName("KmTotais");

                entity.Property(e => e.KmDistancia).HasColumnName("KmDistancia");

                entity.Property(e => e.Quantidade).HasColumnName("Quantidade");

                entity.HasOne(d => d.NºFolhasDeHorasNavigation)
                    .WithMany(p => p.DistribuiçãoCustoFolhaDeHoras)
                    .HasForeignKey(d => d.NºFolhasDeHoras)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Distribuição Custo Folha de Horas_Folhas de Horas");

                entity.HasOne(d => d.NºObraNavigation)
                    .WithMany(p => p.DistribuiçãoCustoFolhaDeHoras)
                    .HasForeignKey(d => d.NºObra)
                    .HasConstraintName("FK_Distribuição Custo Folha de Horas_Projetos");
            });

            modelBuilder.Entity<ElementosJuri>(entity =>
            {
                entity.HasKey(e => new { e.NºProcedimento, e.NºLinha });

                entity.ToTable("Elementos Juri");

                entity.Property(e => e.NºProcedimento)
                    .HasColumnName("Nº Procedimento")
                    .HasMaxLength(10);

                entity.Property(e => e.NºLinha)
                    .HasColumnName("Nº Linha")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.EnviarEmail).HasColumnName("Enviar Email");

                entity.Property(e => e.NºEmpregado)
                    .HasColumnName("Nº Empregado")
                    .HasMaxLength(20);

                entity.Property(e => e.Utilizador).HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.NºProcedimentoNavigation)
                    .WithMany(p => p.ElementosJuri)
                    .HasForeignKey(d => d.NºProcedimento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Elementos Juri_Procedimentos CCP");
            });

            modelBuilder.Entity<EmailsAprovações>(entity =>
            {
                entity.ToTable("Emails Aprovações");

                entity.Property(e => e.Assunto)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DataHoraEmail)
                    .HasColumnName("Data/Hora Email")
                    .HasColumnType("datetime");

                entity.Property(e => e.EmailDestinatário)
                    .IsRequired()
                    .HasColumnName("Email Destinatário")
                    .HasMaxLength(50);

                entity.Property(e => e.NomeDestinatário)
                    .IsRequired()
                    .HasColumnName("Nome Destinatário")
                    .HasMaxLength(50);

                entity.Property(e => e.NºMovimento).HasColumnName("Nº Movimento");

                entity.Property(e => e.ObservaçõesEnvio)
                    .HasColumnName("Observações Envio")
                    .HasMaxLength(150);

                entity.Property(e => e.TextoEmail)
                    .IsRequired()
                    .HasColumnName("Texto Email")
                    .HasColumnType("text");
            });

            modelBuilder.Entity<EmailsProcedimentosCcp>(entity =>
            {
                entity.HasKey(e => new { e.NºProcedimento, e.NºLinha });

                entity.ToTable("Emails Procedimentos CCP");

                entity.Property(e => e.NºProcedimento)
                    .HasColumnName("Nº Procedimento")
                    .HasMaxLength(10);

                entity.Property(e => e.NºLinha)
                    .HasColumnName("Nº Linha")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Anexo1).HasColumnName("Anexo 1");

                entity.Property(e => e.Assunto).HasMaxLength(120);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraEmail)
                    .HasColumnName("Data/Hora Email")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraPedido)
                    .HasColumnName("Data/Hora Pedido")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraResposta)
                    .HasColumnName("Data/Hora Resposta")
                    .HasColumnType("datetime");

                entity.Property(e => e.Destinatário).HasMaxLength(50);

                entity.Property(e => e.EmailDestinatário)
                    .HasColumnName("Email Destinatário")
                    .HasMaxLength(60);

                entity.Property(e => e.Esclarecimento).HasColumnType("text");

                entity.Property(e => e.ObservacoesEnvio).HasColumnType("text");

                entity.Property(e => e.Resposta).HasColumnType("text");

                entity.Property(e => e.TextoEmail)
                    .HasColumnName("Texto Email")
                    .HasColumnType("text");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorEmail)
                    .HasColumnName("Utilizador Email")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorPedidoEscl)
                    .HasColumnName("Utilizador Pedido Escl.")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorResposta)
                    .HasColumnName("Utilizador Resposta")
                    .HasMaxLength(50);

                entity.HasOne(d => d.NºProcedimentoNavigation)
                    .WithMany(p => p.EmailsProcedimentosCcp)
                    .HasForeignKey(d => d.NºProcedimento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Emails Procedimentos CCP_Procedimentos CCP");
            });

            modelBuilder.Entity<EnderecoSuch>(entity =>
            {
                entity.HasKey(e => e.IdEndereco);

                entity.ToTable("Endereco_SUCH");

                entity.Property(e => e.IdEndereco)
                    .HasColumnName("ID_Endereco")
                    .ValueGeneratedNever();

                entity.Property(e => e.CodPostal1)
                    .IsRequired()
                    .HasColumnName("Cod_Postal_1")
                    .HasMaxLength(50);

                entity.Property(e => e.CodPostal2)
                    .IsRequired()
                    .HasColumnName("Cod_Postal_2")
                    .HasMaxLength(50);

                entity.Property(e => e.CodPostal3)
                    .IsRequired()
                    .HasColumnName("Cod_Postal_3")
                    .HasMaxLength(50);

                entity.Property(e => e.CodPostal4)
                    .IsRequired()
                    .HasColumnName("Cod_Postal_4")
                    .HasMaxLength(50);

                entity.Property(e => e.Email1)
                    .IsRequired()
                    .HasColumnName("Email_1")
                    .HasMaxLength(250);

                entity.Property(e => e.Email2)
                    .IsRequired()
                    .HasColumnName("Email_2")
                    .HasMaxLength(250);

                entity.Property(e => e.Email3)
                    .IsRequired()
                    .HasColumnName("Email_3")
                    .HasMaxLength(250);

                entity.Property(e => e.Email4)
                    .IsRequired()
                    .HasColumnName("Email_4")
                    .HasMaxLength(250);

                entity.Property(e => e.Endereco1)
                    .IsRequired()
                    .HasColumnName("Endereco_1")
                    .HasMaxLength(250);

                entity.Property(e => e.Endereco2)
                    .IsRequired()
                    .HasColumnName("Endereco_2")
                    .HasMaxLength(250);

                entity.Property(e => e.Endereco3)
                    .IsRequired()
                    .HasColumnName("Endereco_3")
                    .HasMaxLength(250);

                entity.Property(e => e.Endereco4)
                    .IsRequired()
                    .HasColumnName("Endereco_4")
                    .HasMaxLength(250);

                entity.Property(e => e.Fax1)
                    .IsRequired()
                    .HasColumnName("Fax_1")
                    .HasMaxLength(50);

                entity.Property(e => e.Fax2)
                    .IsRequired()
                    .HasColumnName("Fax_2")
                    .HasMaxLength(50);

                entity.Property(e => e.Fax3)
                    .IsRequired()
                    .HasColumnName("Fax_3")
                    .HasMaxLength(50);

                entity.Property(e => e.Fax4)
                    .IsRequired()
                    .HasColumnName("Fax_4")
                    .HasMaxLength(50);

                entity.Property(e => e.Nome1)
                    .IsRequired()
                    .HasColumnName("Nome_1")
                    .HasMaxLength(50);

                entity.Property(e => e.Nome2)
                    .IsRequired()
                    .HasColumnName("Nome_2")
                    .HasMaxLength(50);

                entity.Property(e => e.Nome3)
                    .IsRequired()
                    .HasColumnName("Nome_3")
                    .HasMaxLength(50);

                entity.Property(e => e.Nome4)
                    .IsRequired()
                    .HasColumnName("Nome_4")
                    .HasMaxLength(50);

                entity.Property(e => e.NumContribuiente)
                    .IsRequired()
                    .HasColumnName("Num_Contribuiente")
                    .HasMaxLength(20);

                entity.Property(e => e.Telefone1)
                    .IsRequired()
                    .HasColumnName("Telefone_1")
                    .HasMaxLength(50);

                entity.Property(e => e.Telefone2)
                    .IsRequired()
                    .HasColumnName("Telefone_2")
                    .HasMaxLength(50);

                entity.Property(e => e.Telefone3)
                    .IsRequired()
                    .HasColumnName("Telefone_3")
                    .HasMaxLength(50);

                entity.Property(e => e.Telefone4)
                    .IsRequired()
                    .HasColumnName("Telefone_4")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<FeaturesMenus>(entity =>
            {
                entity.HasKey(e => new { e.IdFeature, e.IdMenu });
            });

            modelBuilder.Entity<FichaProduto>(entity =>
            {
                entity.HasKey(e => e.Nº);

                entity.ToTable("Ficha Produto");

                entity.Property(e => e.Nº)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Alcool).HasColumnType("decimal(, 2)");

                entity.Property(e => e.Açucares).HasColumnType("decimal(, 2)");

                entity.Property(e => e.Calcio).HasColumnType("decimal(, 2)");

                entity.Property(e => e.Colesterol).HasColumnType("decimal(, 2)");

                entity.Property(e => e.CustoUnitário)
                    .HasColumnName("Custo Unitário")
                    .HasColumnType("decimal(, 2)");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(200);

                entity.Property(e => e.Descrição2).HasMaxLength(200);

                entity.Property(e => e.DescriçãoRefeição)
                    .HasColumnName("Descrição Refeição")
                    .HasMaxLength(200);

                entity.Property(e => e.DióxidoDeEnxofreESulfitos).HasColumnName("Dióxido De Enxofre E Sulfitos");

                entity.Property(e => e.Edivel).HasColumnType("decimal(, 2)");

                entity.Property(e => e.Ferro).HasColumnType("decimal(, 2)");

                entity.Property(e => e.FibraAlimentar)
                    .HasColumnName("Fibra Alimentar")
                    .HasColumnType("decimal(, 2)");

                entity.Property(e => e.FibraAlimentar100g)
                    .HasColumnName("Fibra Alimentar 100g")
                    .HasColumnType("decimal(, 2)");

                entity.Property(e => e.FrutasDeCascaRija).HasColumnName("Frutas De Casca Rija");

                entity.Property(e => e.Glícidos).HasColumnType("decimal(, 2)");

                entity.Property(e => e.Glícidos100g)
                    .HasColumnName("Glícidos 100g")
                    .HasColumnType("decimal(, 2)");

                entity.Property(e => e.GramasPorQuantUnidMedida)
                    .HasColumnName("Gramas Por Quant Unid Medida")
                    .HasColumnType("decimal(, 2)");

                entity.Property(e => e.Inventário).HasColumnType("decimal(, 2)");

                entity.Property(e => e.ListaDeMateriais).HasColumnName("Lista De Materiais");

                entity.Property(e => e.Lípidos).HasColumnType("decimal(, 2)");

                entity.Property(e => e.Lípidos100g)
                    .HasColumnName("Lípidos 100g")
                    .HasColumnType("decimal(, 2)");

                entity.Property(e => e.NºPrateleira)
                    .HasColumnName("Nº Prateleira")
                    .HasMaxLength(10);

                entity.Property(e => e.Potacio).HasColumnType("decimal(, 2)");

                entity.Property(e => e.PreçoUnitário)
                    .HasColumnName("Preço Unitário")
                    .HasColumnType("decimal(, 2)");

                entity.Property(e => e.Proteínas).HasColumnType("decimal(, 2)");

                entity.Property(e => e.Proteínas100g)
                    .HasColumnName("Proteínas 100g")
                    .HasColumnType("decimal(, 2)");

                entity.Property(e => e.QuantUnidadeMedida)
                    .HasColumnName("Quant Unidade Medida")
                    .HasColumnType("decimal(, 2)");

                entity.Property(e => e.Sal).HasColumnType("decimal(, 2)");

                entity.Property(e => e.SementesDeSésamo).HasColumnName("Sementes De Sésamo");

                entity.Property(e => e.Sodio).HasColumnType("decimal(, 2)");

                entity.Property(e => e.TipoRefeição)
                    .HasColumnName("Tipo Refeição")
                    .HasMaxLength(20);

                entity.Property(e => e.UnidadeMedidaBase)
                    .HasColumnName("Unidade Medida Base")
                    .HasMaxLength(10);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.ValorEnergético)
                    .HasColumnName("Valor Energético")
                    .HasColumnType("decimal(, 2)");

                entity.Property(e => e.ValorEnergético100g)
                    .HasColumnName("Valor Energético 100g")
                    .HasColumnType("decimal(, 2)");

                entity.Property(e => e.VitaminaA)
                    .HasColumnName("Vitamina A")
                    .HasColumnType("decimal(, 2)");

                entity.Property(e => e.VitaminaD)
                    .HasColumnName("Vitamina D")
                    .HasColumnType("decimal(, 2)");

                entity.Property(e => e.ÁcidosGordosSaturados)
                    .HasColumnName("Ácidos Gordos Saturados")
                    .HasColumnType("decimal(, 2)");

                entity.Property(e => e.TipoAlteracaoSISLOG)
                    .HasColumnName("TipoAlteracaoSISLOG");

                entity.Property(e => e.DataAlteracaoSISLOG)
                    .HasColumnName("DataAlteracaoSISLOG")
                    .HasColumnType("datetime");

                entity.Property(e => e.EnviarSISLOG)
                    .HasColumnName("EnviarSISLOG");

                entity.Property(e => e.SISLOG)
                    .HasColumnName("SISLOG");

                entity.Property(e => e.DataEnvioSISLOG)
                    .HasColumnName("DataEnvioSISLOG")
                    .HasColumnType("datetime");

                entity.Property(e => e.NomeCurtoSISLOG)
                    .HasColumnName("NomeCurtoSISLOG")
                    .HasMaxLength(15);

                entity.HasOne(d => d.UnidadeMedidaBaseNavigation)
                    .WithMany(p => p.FichaProduto)
                    .HasForeignKey(d => d.UnidadeMedidaBase)
                    .HasConstraintName("FK_Ficha Produto_Unidade Medida");
            });

            modelBuilder.Entity<FichasTécnicasPratos>(entity =>
            {
                entity.HasKey(e => e.NºPrato);

                entity.ToTable("Fichas Técnicas Pratos");

                entity.Property(e => e.NºPrato)
                    .HasColumnName("Nº Prato")
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.ClassFt1).HasColumnName("Class.FT.1");

                entity.Property(e => e.ClassFt2).HasColumnName("Class.FT.2");

                entity.Property(e => e.ClassFt3)
                    .HasColumnName("Class.FT.3")
                    .HasMaxLength(30);

                entity.Property(e => e.ClassFt4)
                    .HasColumnName("Class.FT.4")
                    .HasMaxLength(30);

                entity.Property(e => e.ClassFt5)
                    .HasColumnName("Class.FT.5")
                    .HasMaxLength(30);

                entity.Property(e => e.ClassFt6)
                    .HasColumnName("Class.FT.6")
                    .HasMaxLength(30);

                entity.Property(e => e.ClassFt7)
                    .HasColumnName("Class.FT.7")
                    .HasMaxLength(30);

                entity.Property(e => e.ClassFt8)
                    .HasColumnName("Class.FT.8")
                    .HasMaxLength(30);

                entity.Property(e => e.ContêmGlúten).HasColumnName("Contêm Glúten");

                entity.Property(e => e.CódLocalização)
                    .HasColumnName("Cód. Localização")
                    .HasMaxLength(10);

                entity.Property(e => e.CódUnidadeMedida)
                    .HasColumnName("Cód. Unidade Medida")
                    .HasMaxLength(10);

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(50);

                entity.Property(e => e.FibraAlimentar).HasColumnName("Fibra Alimentar");

                entity.Property(e => e.Grupo).HasMaxLength(20);

                entity.Property(e => e.HidratosDeCarbono).HasColumnName("Hidratos de Carbono");

                entity.Property(e => e.NomeFichaTécnica)
                    .HasColumnName("Nome Ficha Técnica")
                    .HasMaxLength(60);

                entity.Property(e => e.NºDeDoses).HasColumnName("Nº de Doses");

                entity.Property(e => e.Observações).HasColumnType("text");

                entity.Property(e => e.PreçoCustoActual).HasColumnName("Preço Custo Actual");

                entity.Property(e => e.PreçoCustoEsperado).HasColumnName("Preço Custo Esperado");

                entity.Property(e => e.TemperaturaAServir).HasColumnName("Temperatura a Servir");

                entity.Property(e => e.TemperaturaFinalConfeção).HasColumnName("Temperatura Final Confeção");

                entity.Property(e => e.TemperaturaPreparação).HasColumnName("Temperatura Preparação");

                entity.Property(e => e.TempoPreparação).HasColumnName("Tempo Preparação");

                entity.Property(e => e.TécnicaCulinária).HasColumnName("Técnica Culinária");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.ValorEnergético).HasColumnName("Valor Energético");

                entity.Property(e => e.ValorEnergético2).HasColumnName("Valor Energético 2");

                entity.Property(e => e.VariaçãoPreçoCusto).HasColumnName("Variação Preço Custo");

                entity.Property(e => e.VitaminaA).HasColumnName("Vitamina A");

                entity.Property(e => e.VitaminaD).HasColumnName("Vitamina D");

                entity.Property(e => e.ÁBaseAipo).HasColumnName("Á Base Aipo");

                entity.Property(e => e.ÁBaseAmendoins).HasColumnName("Á Base Amendoins");

                entity.Property(e => e.ÁBaseCrustáceos).HasColumnName("Á Base Crustáceos");

                entity.Property(e => e.ÁBaseEnxofreESulfitos).HasColumnName("Á Base Enxofre e Sulfitos");

                entity.Property(e => e.ÁBaseFrutosCascaRija).HasColumnName("Á Base Frutos Casca Rija");

                entity.Property(e => e.ÁBaseLeite).HasColumnName("Á Base Leite");

                entity.Property(e => e.ÁBaseMoluscos).HasColumnName("Á Base Moluscos");

                entity.Property(e => e.ÁBaseMostarda).HasColumnName("Á Base Mostarda");

                entity.Property(e => e.ÁBaseOvos).HasColumnName("Á Base Ovos");

                entity.Property(e => e.ÁBasePeixes).HasColumnName("Á Base Peixes");

                entity.Property(e => e.ÁBaseSementesDeSésamo).HasColumnName("Á Base Sementes de Sésamo");

                entity.Property(e => e.ÁBaseSoja).HasColumnName("Á Base Soja");

                entity.Property(e => e.ÁBaseTremoço).HasColumnName("Á Base Tremoço");

                entity.Property(e => e.Época).HasMaxLength(20);

                entity.HasOne(d => d.ClassFt1Navigation)
                    .WithMany(p => p.FichasTécnicasPratosClassFt1Navigation)
                    .HasForeignKey(d => d.ClassFt1)
                    .HasConstraintName("FK_Fichas Técnicas Pratos_Classificação Fichas Técnicas");

                entity.HasOne(d => d.ClassFt2Navigation)
                    .WithMany(p => p.FichasTécnicasPratosClassFt2Navigation)
                    .HasForeignKey(d => d.ClassFt2)
                    .HasConstraintName("FK_Fichas Técnicas Pratos_Classificação Fichas Técnicas1");
            });

            modelBuilder.Entity<FluxoTrabalhoListaControlo>(entity =>
            {
                entity.HasKey(e => new { e.No, e.Estado, e.Data, e.Hora });

                entity.ToTable("Fluxo Trabalho Lista Controlo");

                entity.Property(e => e.No).HasMaxLength(10);

                entity.Property(e => e.Data).HasColumnType("date");

                entity.Property(e => e.Comentario).HasColumnType("text");

                entity.Property(e => e.Comentario2).HasColumnType("text");

                entity.Property(e => e.DataHoraCriacao).HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificacao).HasColumnType("datetime");

                entity.Property(e => e.DataResposta).HasColumnType("date");

                entity.Property(e => e.EstadoAnterior).HasColumnName("Estado Anterior");

                entity.Property(e => e.EstadoSeguinte).HasColumnName("Estado Seguinte");

                entity.Property(e => e.NomeUser)
                    .HasColumnName("Nome User")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Resposta).HasColumnType("text");

                entity.Property(e => e.User)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UtilizadorCriacao)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UtilizadorModificacao)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.NoNavigation)
                    .WithMany(p => p.FluxoTrabalhoListaControlo)
                    .HasForeignKey(d => d.No)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Fluxo Trabalho Lista Controlo_Procedimentos CCP");

                // zpgm.ALT_CCP_#001.y2019.b
                entity.HasOne(d => d.LoteProcedimentoCcp)
                   .WithMany(p => p.FluxoTrabalhoListaControlo)
                   .HasForeignKey(d => new { d.No, d.IdLote })
                   //.OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_FluxoTrabalhoListaControlo_LoteProcedimentoCcp");
                // zpgm.ALT_CCP_#001.y2019.e
            });

            modelBuilder.Entity<FolhasDeHoras>(entity =>
            {
                entity.HasKey(e => e.NºFolhaDeHoras);

                entity.ToTable("Folhas de Horas");

                entity.Property(e => e.NºFolhaDeHoras)
                    .HasColumnName("Nº Folha de Horas")
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.CriadoPor)
                    .HasColumnName("Criado Por")
                    .HasMaxLength(50);

                entity.Property(e => e.CustoTotalKm).HasColumnName("CustoTotalKM");

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoTipoKmS)
                    .HasColumnName("Código Tipo Km's")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataHoraChegada)
                    .HasColumnName("Data/Hora Chegada")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraPartida)
                    .HasColumnName("Data/Hora Partida")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraTerminado)
                    .HasColumnName("Data/Hora Terminado")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraValidação)
                    .HasColumnName("Data/Hora Validação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraÚltimoEstado)
                    .HasColumnName("Data/Hora Último Estado")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataIntegraçãoEmRh)
                    .HasColumnName("Data Integração em RH")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataIntegraçãoEmRhKm)
                    .HasColumnName("Data Integração em RH KM")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeslocaçãoForaConcelho).HasColumnName("Deslocação Fora Concelho");

                entity.Property(e => e.DeslocaçãoPlaneada).HasColumnName("Deslocação Planeada");

                entity.Property(e => e.Eliminada).HasDefaultValueSql("((0))");

                entity.Property(e => e.IntegradoEmRh).HasColumnName("IntegradoEmRH");

                entity.Property(e => e.IntegradoEmRhkm).HasColumnName("IntegradoEmRHKM");

                entity.Property(e => e.IntegradorEmRh)
                    .HasColumnName("Integrador em RH")
                    .HasMaxLength(50);

                entity.Property(e => e.IntegradorEmRhKm)
                    .HasColumnName("Integrador em RH KM")
                    .HasMaxLength(50);

                entity.Property(e => e.IntegradoresEmRh)
                    .HasColumnName("IntegradoresEmRH")
                    .HasMaxLength(200);

                entity.Property(e => e.IntegradoresEmRhkm)
                    .HasColumnName("IntegradoresEmRHKM")
                    .HasMaxLength(200);

                entity.Property(e => e.Matrícula).HasMaxLength(20);

                entity.Property(e => e.NomeEmpregado)
                    .HasColumnName("Nome Empregado")
                    .HasMaxLength(200);

                entity.Property(e => e.NumTotalKm).HasColumnName("NumTotalKM");

                entity.Property(e => e.NºEmpregado)
                    .HasColumnName("Nº Empregado")
                    .HasMaxLength(20);

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.NºResponsável1)
                    .HasColumnName("Nº Responsável 1")
                    .HasMaxLength(50);

                entity.Property(e => e.NºResponsável2)
                    .HasColumnName("Nº Responsável 2")
                    .HasMaxLength(50);

                entity.Property(e => e.NºResponsável3)
                    .HasColumnName("Nº Responsável 3")
                    .HasMaxLength(50);

                entity.Property(e => e.Observações).HasMaxLength(500);

                entity.Property(e => e.ProjetoDescricao).HasMaxLength(200);

                entity.Property(e => e.TerminadoPor)
                    .HasColumnName("Terminado Por")
                    .HasMaxLength(50);

                entity.Property(e => e.TipoDeslocação).HasColumnName("Tipo Deslocação");

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.Validador).HasMaxLength(50);

                entity.Property(e => e.Validadores).HasMaxLength(200);

                entity.Property(e => e.ValidadoresRhKm)
                    .HasColumnName("Validadores RH KM")
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<FornecedoresAcordoPrecos>(entity =>
            {
                entity.HasKey(e => new { e.NoProcedimento, e.NoFornecedor });

                entity.Property(e => e.NoProcedimento).HasMaxLength(10);

                entity.Property(e => e.NoFornecedor).HasMaxLength(20);

                entity.Property(e => e.NomeFornecedor).HasMaxLength(50);
            });

            modelBuilder.Entity<GruposAprovação>(entity =>
            {
                entity.HasKey(e => e.Código);

                entity.ToTable("Grupos Aprovação");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<HistoricoCondicoesPropostasFornecedores>(entity =>
            {
                entity.HasKey(e => new { e.IdCondicaoPropostaFornecedores, e.NumConsultaMercado, e.NumVersao, e.CodFornecedor });

                entity.ToTable("Historico_Condicoes_Propostas_Fornecedores");

                entity.Property(e => e.IdCondicaoPropostaFornecedores).HasColumnName("ID_Condicao_Proposta_Fornecedores");

                entity.Property(e => e.NumConsultaMercado)
                    .HasColumnName("Num_Consulta_Mercado")
                    .HasMaxLength(20);

                entity.Property(e => e.NumVersao).HasColumnName("Num_Versao");

                entity.Property(e => e.CodFornecedor)
                    .HasColumnName("Cod_Fornecedor")
                    .HasMaxLength(20);

                entity.Property(e => e.Alternativa).HasMaxLength(20);

                entity.Property(e => e.CodActividade)
                    .HasColumnName("Cod_Actividade")
                    .HasMaxLength(20);

                entity.Property(e => e.CodFormaPagamento)
                    .HasColumnName("Cod_Forma_Pagamento")
                    .HasMaxLength(10);

                entity.Property(e => e.CodTermosPagamento)
                    .HasColumnName("Cod_Termos_Pagamento")
                    .HasMaxLength(10);

                entity.Property(e => e.DataProposta)
                    .HasColumnName("Data_Proposta")
                    .HasColumnType("date");

                entity.Property(e => e.EnviarPedidoProposta).HasColumnName("Enviar_Pedido_Proposta");

                entity.Property(e => e.FornecedorSelecionado).HasColumnName("Fornecedor_Selecionado");

                entity.Property(e => e.NomeFornecedor)
                    .HasColumnName("Nome_Fornecedor")
                    .HasMaxLength(100);

                entity.Property(e => e.NumProjecto)
                    .HasColumnName("Num_Projecto")
                    .HasMaxLength(20);

                entity.Property(e => e.ValidadeProposta)
                    .HasColumnName("Validade_Proposta")
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<HistoricoConsultaMercado>(entity =>
            {
                entity.HasKey(e => new { e.NumConsultaMercado, e.NumVersao });

                entity.ToTable("Historico_Consulta_Mercado");

                entity.Property(e => e.NumConsultaMercado)
                    .HasColumnName("Num_Consulta_Mercado")
                    .HasMaxLength(20);

                entity.Property(e => e.NumVersao).HasColumnName("Num_Versao");

                entity.Property(e => e.AdjudicacaoEm)
                    .HasColumnName("Adjudicacao_Em")
                    .HasColumnType("datetime");

                entity.Property(e => e.AdjudicacaoPor)
                    .HasColumnName("Adjudicacao_Por")
                    .HasMaxLength(50);

                entity.Property(e => e.CodActividade)
                    .HasColumnName("Cod_Actividade")
                    .HasMaxLength(20);

                entity.Property(e => e.CodAreaFuncional)
                    .HasColumnName("Cod_Area_Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.CodCentroResponsabilidade)
                    .HasColumnName("Cod_Centro_Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CodFormaPagamento)
                    .HasColumnName("Cod_Forma_Pagamento")
                    .HasMaxLength(10);

                entity.Property(e => e.CodLocalizacao)
                    .HasColumnName("Cod_Localizacao")
                    .HasMaxLength(20);

                entity.Property(e => e.CodProjecto)
                    .HasColumnName("Cod_Projecto")
                    .HasMaxLength(20);

                entity.Property(e => e.CodRegiao)
                    .HasColumnName("Cod_Regiao")
                    .HasMaxLength(20);

                entity.Property(e => e.ConsultaEm)
                    .HasColumnName("Consulta_Em")
                    .HasColumnType("datetime");

                entity.Property(e => e.ConsultaPor)
                    .HasColumnName("Consulta_Por")
                    .HasMaxLength(50);

                entity.Property(e => e.DataLimite)
                    .HasColumnName("Data_Limite")
                    .HasColumnType("date");

                entity.Property(e => e.DataPedidoCotacao)
                    .HasColumnName("Data_Pedido_Cotacao")
                    .HasColumnType("date");

                entity.Property(e => e.Descricao).HasMaxLength(100);

                entity.Property(e => e.EspecificacaoTecnica).HasColumnName("Especificacao_Tecnica");

                entity.Property(e => e.FiltroActividade)
                    .HasColumnName("Filtro_Actividade")
                    .HasMaxLength(250);

                entity.Property(e => e.FornecedorSelecionado)
                    .HasColumnName("Fornecedor_Selecionado")
                    .HasMaxLength(20);

                entity.Property(e => e.NegociacaoContratacaoEm)
                    .HasColumnName("Negociacao_Contratacao_Em")
                    .HasColumnType("datetime");

                entity.Property(e => e.NegociacaoContratacaoPor)
                    .HasColumnName("Negociacao_Contratacao_Por")
                    .HasMaxLength(50);

                entity.Property(e => e.NumDocumentoCompra)
                    .HasColumnName("Num_Documento_Compra")
                    .HasMaxLength(20);

                entity.Property(e => e.NumRequisicao)
                    .HasColumnName("Num_Requisicao")
                    .HasMaxLength(20);

                entity.Property(e => e.PedidoCotacaoCriadoEm)
                    .HasColumnName("Pedido_Cotacao_Criado_Em")
                    .HasColumnType("datetime");

                entity.Property(e => e.PedidoCotacaoCriadoPor)
                    .HasColumnName("Pedido_Cotacao_Criado_Por")
                    .HasMaxLength(50);

                entity.Property(e => e.PedidoCotacaoOrigem)
                    .HasColumnName("Pedido_Cotacao_Origem")
                    .HasMaxLength(20);

                entity.Property(e => e.SeleccaoEfectuada).HasColumnName("Seleccao_Efectuada");

                entity.Property(e => e.UtilizadorRequisicao)
                    .HasColumnName("Utilizador_Requisicao")
                    .HasMaxLength(50);

                entity.Property(e => e.ValorAdjudicado).HasColumnName("Valor_Adjudicado");

                entity.Property(e => e.ValorPedidoCotacao).HasColumnName("Valor_Pedido_Cotacao");
            });

            modelBuilder.Entity<HistoricoLinhasCondicoesPropostasFornecedores>(entity =>
            {
                entity.HasKey(e => new { e.NumLinha, e.NumConsultaMercado, e.NumVersao, e.CodFornecedor, e.Alternativa });

                entity.ToTable("Historico_Linhas_Condicoes_Propostas_Fornecedores");

                entity.Property(e => e.NumLinha).HasColumnName("Num_Linha");

                entity.Property(e => e.NumConsultaMercado)
                    .HasColumnName("Num_Consulta_Mercado")
                    .HasMaxLength(20);

                entity.Property(e => e.NumVersao).HasColumnName("Num_Versao");

                entity.Property(e => e.CodFornecedor)
                    .HasColumnName("Cod_Fornecedor")
                    .HasMaxLength(20);

                entity.Property(e => e.Alternativa).HasMaxLength(20);

                entity.Property(e => e.CodActividade)
                    .HasColumnName("Cod_Actividade")
                    .HasMaxLength(20);

                entity.Property(e => e.CodLocalizacao)
                    .HasColumnName("Cod_Localizacao")
                    .HasMaxLength(20);

                entity.Property(e => e.CodProduto)
                    .HasColumnName("Cod_Produto")
                    .HasMaxLength(20);

                entity.Property(e => e.CodUnidadeMedida)
                    .HasColumnName("Cod_Unidade_Medida")
                    .HasMaxLength(20);

                entity.Property(e => e.DataEntregaPrevista)
                    .HasColumnName("Data_Entrega_Prevista")
                    .HasColumnType("date");

                entity.Property(e => e.DataEntregaPrometida)
                    .HasColumnName("Data_Entrega_Prometida")
                    .HasColumnType("date");

                entity.Property(e => e.EstadoRespostaFornecedor).HasColumnName("Estado_Resposta_Fornecedor");

                entity.Property(e => e.MotivoRejeicao)
                    .HasColumnName("Motivo_Rejeicao")
                    .HasMaxLength(100);

                entity.Property(e => e.NumProjecto)
                    .HasColumnName("Num_Projecto")
                    .HasMaxLength(20);

                entity.Property(e => e.PercentagemDescontoLinha).HasColumnName("Percentagem_Desconto_Linha");

                entity.Property(e => e.PrazoEntrega)
                    .HasColumnName("Prazo_Entrega")
                    .HasMaxLength(10);

                entity.Property(e => e.PrecoFornecedor)
                    .HasColumnName("Preco_Fornecedor")
                    .HasColumnType("decimal(18, 6)");

                entity.Property(e => e.QuantidadeAAdjudicar).HasColumnName("Quantidade_A_Adjudicar");

                entity.Property(e => e.QuantidadeAEncomendar).HasColumnName("Quantidade_A_Encomendar");

                entity.Property(e => e.QuantidadeAdjudicada).HasColumnName("Quantidade_Adjudicada");

                entity.Property(e => e.QuantidadeEncomendada).HasColumnName("Quantidade_Encomendada");

                entity.Property(e => e.QuantidadeRespondida).HasColumnName("Quantidade_Respondida");

                entity.Property(e => e.RespostaFornecedor).HasColumnName("Resposta_Fornecedor");

                entity.Property(e => e.Validade).HasMaxLength(10);

                entity.Property(e => e.ValorAdjudicadoDl).HasColumnName("Valor_Adjudicado_DL");
            });

            modelBuilder.Entity<HistoricoLinhasConsultaMercado>(entity =>
            {
                entity.HasKey(e => new { e.NumLinha, e.NumConsultaMercado, e.NumVersao });

                entity.ToTable("Historico_Linhas_Consulta_Mercado");

                entity.Property(e => e.NumLinha).HasColumnName("Num_Linha");

                entity.Property(e => e.NumConsultaMercado)
                    .HasColumnName("Num_Consulta_Mercado")
                    .HasMaxLength(20);

                entity.Property(e => e.NumVersao).HasColumnName("Num_Versao");

                entity.Property(e => e.CodActividade)
                    .HasColumnName("Cod_Actividade")
                    .HasMaxLength(20);

                entity.Property(e => e.CodAreaFuncional)
                    .HasColumnName("Cod_Area_Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.CodCentroResponsabilidade)
                    .HasColumnName("Cod_Centro_Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CodLocalizacao)
                    .HasColumnName("Cod_Localizacao")
                    .HasMaxLength(20);

                entity.Property(e => e.CodProduto)
                    .HasColumnName("Cod_Produto")
                    .HasMaxLength(20);

                entity.Property(e => e.CodRegiao)
                    .HasColumnName("Cod_Regiao")
                    .HasMaxLength(20);

                entity.Property(e => e.CodUnidadeMedida)
                    .HasColumnName("Cod_Unidade_Medida")
                    .HasMaxLength(20);

                entity.Property(e => e.CriadoEm)
                    .HasColumnName("Criado_Em")
                    .HasColumnType("datetime");

                entity.Property(e => e.CriadoPor)
                    .HasColumnName("Criado_Por")
                    .HasMaxLength(50);

                entity.Property(e => e.CustoTotalObjectivo).HasColumnName("Custo_Total_Objectivo");

                entity.Property(e => e.CustoTotalPrevisto).HasColumnName("Custo_Total_Previsto");

                entity.Property(e => e.CustoUnitarioObjectivo).HasColumnName("Custo_Unitario_Objectivo");

                entity.Property(e => e.CustoUnitarioPrevisto).HasColumnName("Custo_Unitario_Previsto");

                entity.Property(e => e.DataEntregaPrevista)
                    .HasColumnName("Data_Entrega_Prevista")
                    .HasColumnType("date");

                entity.Property(e => e.Descricao).HasMaxLength(100);

                entity.Property(e => e.Descricao2).HasMaxLength(100);

                entity.Property(e => e.LinhaRequisicao).HasColumnName("Linha_Requisicao");

                entity.Property(e => e.ModificadoEm)
                    .HasColumnName("Modificado_Em")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor)
                    .HasColumnName("Modificado_Por")
                    .HasMaxLength(50);

                entity.Property(e => e.NumProjecto)
                    .HasColumnName("Num_Projecto")
                    .HasMaxLength(20);

                entity.Property(e => e.NumRequisicao)
                    .HasColumnName("Num_Requisicao")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<HistoricoSeleccaoEntidades>(entity =>
            {
                entity.HasKey(e => new { e.IdSeleccaoEntidades, e.NumConsultaMercado, e.NumVersao, e.CodFornecedor });

                entity.ToTable("Historico_Seleccao_Entidades");

                entity.Property(e => e.IdSeleccaoEntidades).HasColumnName("ID_Seleccao_Entidades");

                entity.Property(e => e.NumConsultaMercado)
                    .HasColumnName("Num_Consulta_Mercado")
                    .HasMaxLength(20);

                entity.Property(e => e.NumVersao).HasColumnName("Num_Versao");

                entity.Property(e => e.CodFornecedor)
                    .HasColumnName("Cod_Fornecedor")
                    .HasMaxLength(20);

                entity.Property(e => e.CidadeFornecedor)
                    .HasColumnName("Cidade_Fornecedor")
                    .HasMaxLength(50);

                entity.Property(e => e.CodActividade)
                    .HasColumnName("Cod_Actividade")
                    .HasMaxLength(20);

                entity.Property(e => e.CodFormaPagamento)
                    .HasColumnName("Cod_Forma_Pagamento")
                    .HasMaxLength(10);

                entity.Property(e => e.CodTermosPagamento)
                    .HasColumnName("Cod_Termos_Pagamento")
                    .HasMaxLength(10);

                entity.Property(e => e.NomeFornecedor)
                    .HasColumnName("Nome_Fornecedor")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Instrutores>(entity =>
            {
                entity.HasKey(e => e.Nº);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Nome).HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<LinhasAcordoPrecos>(entity =>
            {
                entity.HasKey(e => new { e.NoProcedimento, e.NoFornecedor, e.CodProduto, e.DtValidadeInicio, e.Cresp, e.Localizacao });

                entity.Property(e => e.NoProcedimento).HasMaxLength(10);

                entity.Property(e => e.GrupoRegistoIvaProduto).HasMaxLength(10);

                entity.Property(e => e.NoFornecedor).HasMaxLength(20);

                entity.Property(e => e.NomeFornecedor).HasMaxLength(50);

                entity.Property(e => e.NoSubFornecedor).HasMaxLength(20);

                entity.Property(e => e.NomeSubFornecedor).HasMaxLength(50);

                entity.Property(e => e.CodProduto).HasMaxLength(20);

                entity.Property(e => e.DtValidadeInicio).HasColumnType("datetime");

                entity.Property(e => e.Cresp).HasMaxLength(20);

                entity.Property(e => e.Localizacao).HasMaxLength(20);

                entity.Property(e => e.Area).HasMaxLength(20);

                entity.Property(e => e.CodCategoriaProduto).HasMaxLength(20);

                entity.Property(e => e.CodProdutoFornecedor).HasMaxLength(20);

                entity.Property(e => e.CustoUnitario).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.DataCriacao).HasColumnType("datetime");

                entity.Property(e => e.DescricaoProdFornecedor).HasMaxLength(80);

                entity.Property(e => e.DescricaoProduto).HasMaxLength(80);

                entity.Property(e => e.DtValidadeFim).HasColumnType("datetime");

                entity.Property(e => e.QtdPorUm).HasColumnName("QtdPorUM");

                entity.Property(e => e.Regiao).HasMaxLength(20);

                entity.Property(e => e.Um)
                    .HasColumnName("UM")
                    .HasMaxLength(10);

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<LinhasCondicoesPropostasFornecedores>(entity =>
            {
                entity.HasKey(e => e.NumLinha);

                entity.ToTable("Linhas_Condicoes_Propostas_Fornecedores");

                entity.HasIndex(e => new { e.NumConsultaMercado, e.CodFornecedor, e.Alternativa })
                    .HasName("IX_Linhas_Condicoes_Propostas_Fornecedores")
                    .IsUnique();

                entity.Property(e => e.NumLinha).HasColumnName("Num_Linha");

                entity.Property(e => e.Alternativa).HasMaxLength(20);

                entity.Property(e => e.CodActividade)
                    .HasColumnName("Cod_Actividade")
                    .HasMaxLength(20);

                entity.Property(e => e.CodFornecedor)
                    .HasColumnName("Cod_Fornecedor")
                    .HasMaxLength(20);

                entity.Property(e => e.CodLocalizacao)
                    .HasColumnName("Cod_Localizacao")
                    .HasMaxLength(20);

                entity.Property(e => e.CodProduto)
                    .HasColumnName("Cod_Produto")
                    .HasMaxLength(20);

                entity.Property(e => e.CodUnidadeMedida)
                    .HasColumnName("Cod_Unidade_Medida")
                    .HasMaxLength(20);

                entity.Property(e => e.DataEntregaPrevista)
                    .HasColumnName("Data_Entrega_Prevista")
                    .HasColumnType("date");

                entity.Property(e => e.DataEntregaPrometida)
                    .HasColumnName("Data_Entrega_Prometida")
                    .HasColumnType("date");

                entity.Property(e => e.EstadoRespostaFornecedor).HasColumnName("Estado_Resposta_Fornecedor");

                entity.Property(e => e.MotivoRejeicao)
                    .HasColumnName("Motivo_Rejeicao")
                    .HasMaxLength(100);

                entity.Property(e => e.NumConsultaMercado)
                    .HasColumnName("Num_Consulta_Mercado")
                    .HasMaxLength(20);

                entity.Property(e => e.NumProjecto)
                    .HasColumnName("Num_Projecto")
                    .HasMaxLength(20);

                entity.Property(e => e.PercentagemDescontoLinha).HasColumnName("Percentagem_Desconto_Linha");

                entity.Property(e => e.PrazoEntrega)
                    .HasColumnName("Prazo_Entrega")
                    .HasMaxLength(10);

                entity.Property(e => e.PrecoFornecedor)
                    .HasColumnName("Preco_Fornecedor")
                    .HasColumnType("decimal(18, 6)");

                entity.Property(e => e.QuantidadeAAdjudicar).HasColumnName("Quantidade_A_Adjudicar");

                entity.Property(e => e.QuantidadeAEncomendar).HasColumnName("Quantidade_A_Encomendar");

                entity.Property(e => e.QuantidadeAdjudicada).HasColumnName("Quantidade_Adjudicada");

                entity.Property(e => e.QuantidadeEncomendada).HasColumnName("Quantidade_Encomendada");

                entity.Property(e => e.QuantidadeRespondida).HasColumnName("Quantidade_Respondida");

                entity.Property(e => e.RespostaFornecedor).HasColumnName("Resposta_Fornecedor");

                entity.Property(e => e.Validade).HasMaxLength(10);

                entity.Property(e => e.ValorAdjudicadoDl).HasColumnName("Valor_Adjudicado_DL");

                entity.HasOne(d => d.NumConsultaMercadoNavigation)
                    .WithMany(p => p.LinhasCondicoesPropostasFornecedores)
                    .HasForeignKey(d => d.NumConsultaMercado)
                    .HasConstraintName("FK_Linhas_Condicoes_Propostas_Fornecedores_Consulta_Mercado");
            });

            modelBuilder.Entity<LinhasConsultaMercado>(entity =>
            {
                entity.HasKey(e => e.NumLinha);

                entity.ToTable("Linhas_Consulta_Mercado");

                entity.Property(e => e.NumLinha).HasColumnName("Num_Linha");

                entity.Property(e => e.CodActividade)
                    .HasColumnName("Cod_Actividade")
                    .HasMaxLength(20);

                entity.Property(e => e.CodAreaFuncional)
                    .HasColumnName("Cod_Area_Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.CodCentroResponsabilidade)
                    .HasColumnName("Cod_Centro_Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CodLocalizacao)
                    .HasColumnName("Cod_Localizacao")
                    .HasMaxLength(20);

                entity.Property(e => e.CodProduto)
                    .HasColumnName("Cod_Produto")
                    .HasMaxLength(20);

                entity.Property(e => e.CodRegiao)
                    .HasColumnName("Cod_Regiao")
                    .HasMaxLength(20);

                entity.Property(e => e.CodUnidadeMedida)
                    .HasColumnName("Cod_Unidade_Medida")
                    .HasMaxLength(20);

                entity.Property(e => e.CriadoEm)
                    .HasColumnName("Criado_Em")
                    .HasColumnType("datetime");

                entity.Property(e => e.CriadoPor)
                    .HasColumnName("Criado_Por")
                    .HasMaxLength(50);

                entity.Property(e => e.CustoTotalObjectivo).HasColumnName("Custo_Total_Objectivo");

                entity.Property(e => e.CustoTotalPrevisto).HasColumnName("Custo_Total_Previsto");

                entity.Property(e => e.CustoUnitarioObjectivo).HasColumnName("Custo_Unitario_Objectivo");

                entity.Property(e => e.CustoUnitarioPrevisto).HasColumnName("Custo_Unitario_Previsto");

                entity.Property(e => e.DataEntregaPrevista)
                    .HasColumnName("Data_Entrega_Prevista")
                    .HasColumnType("date");

                entity.Property(e => e.Descricao).HasMaxLength(100);

                entity.Property(e => e.Descricao2).HasMaxLength(50);

                entity.Property(e => e.LinhaRequisicao).HasColumnName("Linha_Requisicao");

                entity.Property(e => e.ModificadoEm)
                    .HasColumnName("Modificado_Em")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor)
                    .HasColumnName("Modificado_Por")
                    .HasMaxLength(50);

                entity.Property(e => e.NumConsultaMercado)
                    .HasColumnName("Num_Consulta_Mercado")
                    .HasMaxLength(20);

                entity.Property(e => e.NumProjecto)
                    .HasColumnName("Num_Projecto")
                    .HasMaxLength(20);

                entity.Property(e => e.NumRequisicao)
                    .HasColumnName("Num_Requisicao")
                    .HasMaxLength(20);

                entity.HasOne(d => d.NumConsultaMercadoNavigation)
                    .WithMany(p => p.LinhasConsultaMercado)
                    .HasForeignKey(d => d.NumConsultaMercado)
                    .HasConstraintName("FK_Linhas_Consulta_Mercado_Consulta_Mercado");
            });

            modelBuilder.Entity<LinhasContratos>(entity =>
            {
                entity.HasKey(e => new { e.TipoContrato, e.NºContrato, e.NºVersão, e.NºLinha });

                entity.ToTable("Linhas Contratos");

                entity.Property(e => e.TipoContrato).HasColumnName("Tipo Contrato");

                entity.Property(e => e.NºContrato)
                    .HasColumnName("Nº Contrato")
                    .HasMaxLength(20);

                entity.Property(e => e.NºVersão).HasColumnName("Nº Versão");

                entity.Property(e => e.NºLinha)
                    .HasColumnName("Nº Linha")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CriaContrato).HasColumnName("Cria Contrato");

                entity.Property(e => e.CódServiçoCliente)
                    .HasColumnName("Cód. Serviço Cliente")
                    .HasMaxLength(50);

                entity.Property(e => e.CódUnidadeMedida)
                    .HasColumnName("Cód. Unidade Medida")
                    .HasMaxLength(10);

                entity.Property(e => e.Código).HasMaxLength(20);

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataFimVersão)
                    .HasColumnName("Data Fim Versão")
                    .HasColumnType("date");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataInícioVersão)
                    .HasColumnName("Data Início Versão")
                    .HasColumnType("date");

                entity.Property(e => e.DescontoLinha)
                    .HasColumnName("% Desconto Linha")
                    .HasColumnType("decimal(, 20)");

                entity.Property(e => e.Descrição).HasMaxLength(50);

                entity.Property(e => e.Descricao2).HasMaxLength(80);

                entity.Property(e => e.GrupoFatura).HasColumnName("Grupo Fatura");

                entity.Property(e => e.NºHorasIntervenção)
                    .HasColumnName("Nº Horas Intervenção")
                    .HasColumnType("decimal(, 20)");

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.NºResponsável)
                    .HasColumnName("Nº Responsável")
                    .HasMaxLength(20);

                entity.Property(e => e.NºTécnicos)
                    .HasColumnName("Nº Técnicos")
                    .HasColumnType("decimal(, 20)");

                entity.Property(e => e.Periodicidade).HasColumnType("decimal(, 20)");

                entity.Property(e => e.PreçoUnitário)
                    .HasColumnName("Preço Unitário")
                    .HasColumnType("decimal(, 20)");

                entity.Property(e => e.Quantidade).HasColumnType("decimal(, 20)");

                entity.Property(e => e.TipoProposta).HasColumnName("Tipo Proposta");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Contratos)
                    .WithMany(p => p.LinhasContratos)
                    .HasForeignKey(d => new { d.TipoContrato, d.NºContrato, d.NºVersão })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Linhas Contratos_Contratos");
            });

            modelBuilder.Entity<LinhasFaturaçãoContrato>(entity =>
            {
                entity.HasKey(e => new { e.NºContrato, e.GrupoFatura, e.NºLinha });

                entity.ToTable("Linhas Faturação Contrato");

                entity.Property(e => e.NºContrato)
                    .HasColumnName("Nº Contrato")
                    .HasMaxLength(20);

                entity.Property(e => e.GrupoFatura).HasColumnName("Grupo Fatura");

                entity.Property(e => e.NºLinha).HasColumnName("Nº Linha");

                entity.Property(e => e.CódUnidadeMedida)
                    .HasColumnName("Cód. Unidade Medida")
                    .HasMaxLength(10);

                entity.Property(e => e.Código)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Descrição)
                    .HasColumnName("Descrição")
                    .HasMaxLength(50);

                entity.Property(e => e.Descricao2)
                    .HasColumnName("Descricao2")
                    .HasMaxLength(80);

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoServiço)
                    .HasColumnName("Código Serviço")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(100);

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.PreçoUnitário).HasColumnName("Preço Unitário");

                entity.Property(e => e.Tipo).HasColumnType("nchar(10)");

                entity.Property(e => e.TipoRecurso).HasColumnName("Tipo Recurso");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.ValorVenda).HasColumnName("Valor Venda");
            });

            modelBuilder.Entity<LinhasFichasTécnicasPratos>(entity =>
            {
                entity.HasKey(e => new { e.NºPrato, e.NºLinha });

                entity.ToTable("Linhas Fichas Técnicas Pratos");

                entity.Property(e => e.NºPrato)
                    .HasColumnName("Nº Prato")
                    .HasMaxLength(20);

                entity.Property(e => e.NºLinha)
                    .HasColumnName("Nº Linha")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ColesterolPorQuantidade).HasColumnName("Colesterol por Quantidade");

                entity.Property(e => e.CálcioPorQuantidade).HasColumnName("Cálcio por Quantidade");

                entity.Property(e => e.CódLocalização)
                    .HasColumnName("Cód. Localização")
                    .HasMaxLength(10);

                entity.Property(e => e.CódUnidadeMedida)
                    .HasColumnName("Cód. Unidade Medida")
                    .HasMaxLength(10);

                entity.Property(e => e.Código).HasMaxLength(20);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(100);

                entity.Property(e => e.FerroPorQuantidade).HasColumnName("Ferro por Quantidade");

                entity.Property(e => e.FibasPorQuantidade).HasColumnName("Fibas por Quantidade");

                entity.Property(e => e.GlícidosPorQuantidade).HasColumnName("Glícidos por Quantidade");

                entity.Property(e => e.HidratosDeCarbono).HasColumnName("Hidratos de Carbono");

                entity.Property(e => e.LípidosPorQuantidade).HasColumnName("Lípidos por Quantidade");

                entity.Property(e => e.PotássioPorQuantidade).HasColumnName("Potássio por Quantidade");

                entity.Property(e => e.Preparação).HasColumnType("text");

                entity.Property(e => e.PreçoCustoAtual).HasColumnName("Preço Custo Atual");

                entity.Property(e => e.PreçoCustoEsperado).HasColumnName("Preço Custo Esperado");

                entity.Property(e => e.ProteínasPorQuantidade).HasColumnName("Proteínas por Quantidade");

                entity.Property(e => e.QuantidadeDeProdução).HasColumnName("Quantidade de Produção");

                entity.Property(e => e.QuantidadePrato).HasColumnName("Quantidade Prato");

                entity.Property(e => e.SódioPorQuantidade).HasColumnName("Sódio por Quantidade");

                entity.Property(e => e.TpreçoCustoAtual).HasColumnName("TPreço Custo Atual");

                entity.Property(e => e.TpreçoCustoEsperado).HasColumnName("TPreço Custo Esperado");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.ValorEnergético).HasColumnName("Valor Energético");

                entity.Property(e => e.ValorEnergético2).HasColumnName("Valor Energético 2");

                entity.Property(e => e.VitaminaA).HasColumnName("Vitamina A");

                entity.Property(e => e.VitaminaAPorQuantidade).HasColumnName("Vitamina A por Quantidade");

                entity.Property(e => e.VitaminaD).HasColumnName("Vitamina D");

                entity.Property(e => e.VitaminaDPorQuantidade).HasColumnName("Vitamina D por Quantidade");

                entity.Property(e => e.ÁcidosGordosSaturados).HasColumnName("Ácidos Gordos Saturados");

                entity.HasOne(d => d.NºPratoNavigation)
                    .WithMany(p => p.LinhasFichasTécnicasPratos)
                    .HasForeignKey(d => d.NºPrato)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Linhas Fichas Técnicas Pratos_Fichas Técnicas Pratos");
            });

            modelBuilder.Entity<LinhasFolhaHoras>(entity =>
            {
                entity.HasKey(e => new { e.NoFolhaHoras, e.NoLinha });

                entity.ToTable("Linhas Folha Horas");

                entity.Property(e => e.NoFolhaHoras)
                    .HasColumnName("No_Folha_Horas")
                    .HasMaxLength(20);

                entity.Property(e => e.NoLinha).HasColumnName("No_Linha");

                entity.Property(e => e.CalculoAutomatico).HasColumnName("Calculo_Automatico");

                entity.Property(e => e.NoProjeto)
                    .HasColumnName("NoProjeto")
                    .HasMaxLength(20);

                entity.Property(e => e.ProjetoDescricao)
                    .HasColumnName("ProjetoDescricao")
                    .HasMaxLength(200);

                entity.Property(e => e.CodArea)
                    .HasColumnName("Cod_Area")
                    .HasMaxLength(20);

                entity.Property(e => e.CodCresp)
                    .HasColumnName("Cod_Cresp")
                    .HasMaxLength(20);

                entity.Property(e => e.CodDestino)
                    .HasColumnName("Cod_Destino")
                    .HasMaxLength(20);

                entity.Property(e => e.CodOrigem)
                    .HasColumnName("Cod_Origem")
                    .HasMaxLength(20);

                entity.Property(e => e.CodRegiao)
                    .HasColumnName("Cod_Regiao")
                    .HasMaxLength(20);

                entity.Property(e => e.CodTipoCusto)
                    .HasColumnName("Cod_Tipo_Custo")
                    .HasMaxLength(20);

                entity.Property(e => e.CustoTotal).HasColumnName("Custo_Total");

                entity.Property(e => e.CustoUnitario).HasColumnName("Custo_Unitario");

                entity.Property(e => e.DataDespesa)
                    .HasColumnName("Data_Despesa")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraCriacao)
                    .HasColumnName("Data_Hora_Criacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificacao)
                    .HasColumnName("Data_Hora_Modificacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DescricaoDestino)
                    .HasColumnName("Descricao_Destino")
                    .HasMaxLength(200);

                entity.Property(e => e.DescricaoOrigem)
                    .HasColumnName("Descricao_Origem")
                    .HasMaxLength(200);

                entity.Property(e => e.DescricaoTipoCusto)
                    .HasColumnName("Descricao_Tipo_Custo")
                    .HasMaxLength(200);

                entity.Property(e => e.DistanciaPrevista).HasColumnName("Distancia_Prevista");

                entity.Property(e => e.Funcionario).HasMaxLength(20);

                entity.Property(e => e.Matricula).HasMaxLength(20);

                entity.Property(e => e.Observacao).HasMaxLength(200);

                entity.Property(e => e.PrecoUnitario).HasColumnName("Preco_Unitario");

                entity.Property(e => e.PrecoVenda).HasColumnName("Preco_Venda");

                entity.Property(e => e.RegistarSubsidiosPremios).HasColumnName("Registar_Subsidios_Premios");

                entity.Property(e => e.RubricaSalarial)
                    .HasColumnName("Rubrica_Salarial")
                    .HasMaxLength(20);

                entity.Property(e => e.RubricaSalarial2)
                    .HasColumnName("Rubrica_Salarial2")
                    .HasMaxLength(20);

                entity.Property(e => e.TipoCusto).HasColumnName("Tipo_Custo");

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("Utilizador_Criacao")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("Utilizador_Modificacao")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<LinhasPEncomendaProcedimentosCcp>(entity =>
            {
                entity.HasKey(e => new { e.NºProcedimento, e.NºLinha });

                entity.ToTable("Linhas p/ Encomenda Procedimentos CCP");

                entity.Property(e => e.NºProcedimento)
                    .HasColumnName("Nº Procedimento")
                    .HasMaxLength(10);

                entity.Property(e => e.NºLinha)
                    .HasColumnName("Nº Linha")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CustoUnitário).HasColumnName("Custo Unitário");

                entity.Property(e => e.CódLocalização)
                    .HasColumnName("Cód. Localização")
                    .HasMaxLength(10);

                entity.Property(e => e.CódUnidadeMedida)
                    .HasColumnName("Cód. Unidade Medida")
                    .HasMaxLength(10);

                entity.Property(e => e.Código).HasMaxLength(20);

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(100);

                entity.Property(e => e.NºLinhaRequisição).HasColumnName("Nº Linha Requisição");

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.NºRequisição)
                    .HasColumnName("Nº Requisição")
                    .HasMaxLength(20);

                entity.Property(e => e.QuantARequerer).HasColumnName("Quant. a Requerer");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.NºProcedimentoNavigation)
                    .WithMany(p => p.LinhasPEncomendaProcedimentosCcp)
                    .HasForeignKey(d => d.NºProcedimento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Linhas p/ Encomenda Procedimentos CCP_Procedimentos CCP");

                entity.HasOne(d => d.NºRequisiçãoNavigation)
                    .WithMany(p => p.LinhasPEncomendaProcedimentosCcp)
                    .HasForeignKey(d => d.NºRequisição)
                    .HasConstraintName("FK_Linhas p/ Encomenda Procedimentos CCP_Requisição");
            });

            modelBuilder.Entity<LinhasPreEncomenda>(entity =>
            {
                entity.HasKey(e => e.NºLinhaPreEncomenda);

                entity.ToTable("Linhas Pre-Encomenda");

                entity.Property(e => e.NºLinhaPreEncomenda).HasColumnName("Nº Linha Pre-Encomenda");

                entity.Property(e => e.CriarDocumento)
                    .HasColumnName("Criar Documento")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CustoUnitário).HasColumnName("Custo Unitário");

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoLocalização)
                    .HasColumnName("Código Localização")
                    .HasMaxLength(10);

                entity.Property(e => e.CódigoProduto)
                    .HasColumnName("Código Produto")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoUnidadeMedida)
                    .HasColumnName("Código Unidade Medida")
                    .HasMaxLength(10);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DescriçãoProduto)
                    .HasColumnName("Descrição Produto")
                    .HasMaxLength(100);

                entity.Property(e => e.DocumentoACriar).HasColumnName("Documento a Criar");

                entity.Property(e => e.NºEncomendaAberto)
                    .HasColumnName("Nº Encomenda Aberto")
                    .HasMaxLength(20);

                entity.Property(e => e.NºFornecedor)
                    .HasColumnName("Nº Fornecedor")
                    .HasMaxLength(20);

                entity.Property(e => e.NºLinhaEncomendaAberto).HasColumnName("Nº Linha Encomenda Aberto");

                entity.Property(e => e.NºLinhaRequisição).HasColumnName("Nº Linha Requisição");

                entity.Property(e => e.NºPreEncomenda)
                    .HasColumnName("Nº Pre-Encomenda")
                    .HasMaxLength(20);

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.NºRequisição)
                    .HasColumnName("Nº Requisição")
                    .HasMaxLength(20);

                entity.Property(e => e.QuantidadeDisponibilizada).HasColumnName("Quantidade Disponibilizada");

                entity.Property(e => e.Tratada).HasDefaultValueSql("((0))");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<LinhasPréRequisição>(entity =>
            {
                entity.HasKey(e => new { e.NºPréRequisição, e.NºLinha });

                entity.ToTable("Linhas Pré-Requisição");

                entity.Property(e => e.NºPréRequisição)
                    .HasColumnName("Nº Pré-Requisição")
                    .HasMaxLength(20);

                entity.Property(e => e.NºLinha)
                    .HasColumnName("Nº Linha")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CustoUnitário).HasColumnName("Custo Unitário");

                entity.Property(e => e.CustoUnitarioComIVA).HasColumnName("CustoUnitarioComIVA");

                entity.Property(e => e.Código).HasMaxLength(20);

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoLocalização)
                    .HasColumnName("Código Localização")
                    .HasMaxLength(10);

                entity.Property(e => e.CódigoProdutoFornecedor)
                    .HasColumnName("Código Produto Fornecedor")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoUnidadeMedida)
                    .HasColumnName("Código Unidade Medida")
                    .HasMaxLength(10);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataReceçãoEsperada)
                    .HasColumnName("Data Receção Esperada")
                    .HasColumnType("date");

                entity.Property(e => e.Descrição).HasMaxLength(100);

                entity.Property(e => e.Descrição2)
                    .HasColumnName("Descrição 2")
                    .HasMaxLength(50);

                entity.Property(e => e.LocalCompraDireta)
                    .HasColumnName("Local Compra Direta")
                    .HasMaxLength(20);

                entity.Property(e => e.NºCliente)
                    .HasColumnName("Nº Cliente")
                    .HasMaxLength(20);

                entity.Property(e => e.NºEncomendaAberto)
                    .HasColumnName("Nº Encomenda Aberto")
                    .HasMaxLength(20);

                entity.Property(e => e.NºFornecedor)
                    .HasColumnName("Nº Fornecedor")
                    .HasMaxLength(20);

                entity.Property(e => e.NºFuncionário)
                    .HasColumnName("Nº Funcionário")
                    .HasMaxLength(20);

                entity.Property(e => e.NºLinhaEncomendaAberto).HasColumnName("Nº Linha Encomenda Aberto");

                entity.Property(e => e.NºLinhaOrdemManutenção).HasColumnName("Nº Linha Ordem Manutenção");

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.PreçoUnitárioVenda).HasColumnName("Preço Unitário Venda");

                entity.Property(e => e.QtdPorUnidadeMedida).HasColumnName("Qtd Por Unidade Medida");

                entity.Property(e => e.QuantidadeARequerer).HasColumnName("Quantidade a Requerer");

                entity.Property(e => e.QuantidadePendente).HasColumnName("Quantidade Pendente");

                entity.Property(e => e.QuantidadeRequerida).HasColumnName("Quantidade Requerida");

                entity.Property(e => e.UnidadeProdutivaNutrição)
                    .HasColumnName("Unidade Produtiva Nutrição")
                    .HasMaxLength(20);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.ValorOrçamento).HasColumnName("Valor Orçamento");

                entity.Property(e => e.Viatura).HasMaxLength(20);

                entity.Property(e => e.TaxaIVA).HasColumnName("TaxaIVA");

                entity.HasOne(d => d.NºProjetoNavigation)
                    .WithMany(p => p.LinhasPréRequisição)
                    .HasForeignKey(d => d.NºProjeto)
                    .HasConstraintName("FK_Linhas Pré-Requisição_Projetos");

                entity.HasOne(d => d.NºPréRequisiçãoNavigation)
                    .WithMany(p => p.LinhasPréRequisição)
                    .HasForeignKey(d => d.NºPréRequisição)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Linhas Pré-Requisição_Pré-Requisição");
            });

            modelBuilder.Entity<LinhasRequisição>(entity =>
            {
                entity.HasKey(e => new { e.NºRequisição, e.NºLinha });

                entity.ToTable("Linhas Requisição");

                entity.Property(e => e.NºRequisição)
                    .HasColumnName("Nº Requisição")
                    .HasMaxLength(20);

                entity.Property(e => e.NºLinha)
                    .HasColumnName("Nº Linha")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Aprovadores).HasMaxLength(100);

                entity.Property(e => e.CriarConsultaMercado).HasColumnName("Criar Consulta Mercado");

                entity.Property(e => e.CustoUnitário)
                    .HasColumnName("Custo Unitário")
                    .HasColumnType("decimal(18, 5)");

                entity.Property(e => e.Código).HasMaxLength(20);

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoLocalização)
                    .HasColumnName("Código Localização")
                    .HasMaxLength(10);

                entity.Property(e => e.CódigoProdutoFornecedor)
                    .HasColumnName("Código Produto Fornecedor")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoUnidadeMedida)
                    .HasColumnName("Código Unidade Medida")
                    .HasMaxLength(10);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataEnvioParaCompras)
                    .HasColumnName("Data Envio para Compras")
                    .HasColumnType("date");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataMercadoLocal)
                    .HasColumnName("Data Mercado Local")
                    .HasColumnType("date");

                entity.Property(e => e.DataReceçãoEsperada)
                    .HasColumnName("Data Receção Esperada")
                    .HasColumnType("date");

                entity.Property(e => e.DataRecusaMercLocal)
                    .HasColumnName("Data Recusa Merc. Local")
                    .HasColumnType("date");

                entity.Property(e => e.Descrição).HasMaxLength(100);

                entity.Property(e => e.Descrição2).HasMaxLength(50);

                entity.Property(e => e.EnviadoParaCompras).HasColumnName("Enviado para Compras");

                entity.Property(e => e.EnviadoPréCompra).HasColumnName("Enviado Pré Compra");

                entity.Property(e => e.EnviarPréCompra).HasColumnName("Enviar Pré Compra");

                entity.Property(e => e.GrupoRegistoIvanegocio)
                    .HasColumnName("GrupoRegistoIVANegocio")
                    .HasMaxLength(10);

                entity.Property(e => e.GrupoRegistoIvaproduto)
                    .HasColumnName("GrupoRegistoIVAProduto")
                    .HasMaxLength(10);

                entity.Property(e => e.IdCompra).HasColumnName("ID Compra");

                entity.Property(e => e.MercadoLocal).HasColumnName("Mercado Local");

                entity.Property(e => e.MotivoRecusaMercLocal)
                    .HasColumnName("Motivo Recusa Merc. Local")
                    .HasMaxLength(50);

                entity.Property(e => e.NºCliente)
                    .HasColumnName("Nº Cliente")
                    .HasMaxLength(20);

                entity.Property(e => e.NºDeConsultaMercadoCriada)
                    .HasColumnName("Nº de Consulta Mercado Criada")
                    .HasMaxLength(20);

                entity.Property(e => e.NºEncomendaAberto)
                    .HasColumnName("Nº Encomenda Aberto")
                    .HasMaxLength(20);

                entity.Property(e => e.NºEncomendaCriada)
                    .HasColumnName("Nº Encomenda Criada")
                    .HasMaxLength(20);

                entity.Property(e => e.NºFornecedor)
                    .HasColumnName("Nº Fornecedor")
                    .HasMaxLength(20);

                entity.Property(e => e.NoSubFornecedor)
                    .HasColumnName("NoSubFornecedor")
                    .HasMaxLength(20);

                entity.Property(e => e.NºFuncionário)
                    .HasColumnName("Nº Funcionário")
                    .HasMaxLength(20);

                entity.Property(e => e.NºLinhaEncomendaAberto).HasColumnName("Nº Linha Encomenda Aberto");

                entity.Property(e => e.NºLinhaOrdemManutenção).HasColumnName("Nº Linha Ordem Manutenção");

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.PercentagemDesconto).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.PreçoUnitárioVenda)
                    .HasColumnName("Preço Unitário Venda")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.QtdPorUnidadeDeMedida)
                    .HasColumnName("Qtd por Unidade de Medida")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.QuantidadeADisponibilizar)
                    .HasColumnName("Quantidade a Disponibilizar")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.QuantidadeAReceber)
                    .HasColumnName("Quantidade a Receber")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.QuantidadeARequerer)
                    .HasColumnName("Quantidade a Requerer")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.QuantidadeDisponibilizada)
                    .HasColumnName("Quantidade Disponibilizada")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.QuantidadeInicial).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.QuantidadePendente)
                    .HasColumnName("Quantidade Pendente")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.QuantidadeRecebida)
                    .HasColumnName("Quantidade Recebida")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.QuantidadeRequerida)
                    .HasColumnName("Quantidade Requerida")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.QuantidadeDisponivel)
                    .HasColumnName("QuantidadeDisponivel")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.QuantidadeReservada)
                    .HasColumnName("QuantidadeReservada")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.RecusadoCompras).HasColumnName("Recusado Compras");

                entity.Property(e => e.RegiãoMercadoLocal)
                    .HasColumnName("Região Mercado Local")
                    .HasMaxLength(20);

                entity.Property(e => e.UnidadeProdutivaNutrição)
                    .HasColumnName("Unidade Produtiva Nutrição")
                    .HasMaxLength(20);

                entity.Property(e => e.UserMercadoLocal)
                    .HasColumnName("User Mercado Local")
                    .HasMaxLength(20);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.ValidadoCompras).HasColumnName("Validado Compras");

                entity.Property(e => e.ValorOrçamento)
                    .HasColumnName("Valor Orçamento")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.Viatura).HasMaxLength(20);

                entity.Property(e => e.SemEfeito).HasColumnName("SemEfeito");


                entity.HasOne(d => d.NºProjetoNavigation)
                    .WithMany(p => p.LinhasRequisição)
                    .HasForeignKey(d => d.NºProjeto)
                    .HasConstraintName("FK_Linhas Requisição_Projetos");

                entity.HasOne(d => d.NºRequisiçãoNavigation)
                    .WithMany(p => p.LinhasRequisição)
                    .HasForeignKey(d => d.NºRequisição)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Linhas Requisição_Requisição");

                entity.HasOne(d => d.ViaturaNavigation)
                    .WithMany(p => p.LinhasRequisição)
                    .HasForeignKey(d => d.Viatura)
                    .HasConstraintName("FK_Linhas Requisição_Viaturas");
            });

            modelBuilder.Entity<LinhasRequisiçãoHist>(entity =>
            {
                entity.HasKey(e => new { e.NºRequisição, e.NºLinha });

                entity.ToTable("Linhas Requisição Hist");

                entity.Property(e => e.NºRequisição)
                    .HasColumnName("Nº Requisição")
                    .HasMaxLength(20);

                entity.Property(e => e.NºLinha).HasColumnName("Nº Linha");

                entity.Property(e => e.Aprovadores).HasMaxLength(100);

                entity.Property(e => e.CriarConsultaMercado).HasColumnName("Criar Consulta Mercado");

                entity.Property(e => e.CustoUnitário)
                    .HasColumnName("Custo Unitário")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.Código).HasMaxLength(20);

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoLocalização)
                    .HasColumnName("Código Localização")
                    .HasMaxLength(10);

                entity.Property(e => e.CódigoProdutoFornecedor)
                    .HasColumnName("Código Produto Fornecedor")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoUnidadeMedida)
                    .HasColumnName("Código Unidade Medida")
                    .HasMaxLength(10);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataEnvioParaCompras)
                    .HasColumnName("Data Envio para Compras")
                    .HasColumnType("date");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataMercadoLocal)
                    .HasColumnName("Data Mercado Local")
                    .HasColumnType("date");

                entity.Property(e => e.DataReceçãoEsperada)
                    .HasColumnName("Data Receção Esperada")
                    .HasColumnType("date");

                entity.Property(e => e.DataRecusaMercLocal)
                    .HasColumnName("Data Recusa Merc. Local")
                    .HasColumnType("date");

                entity.Property(e => e.Descrição).HasMaxLength(100);

                entity.Property(e => e.Descrição2).HasMaxLength(100);

                entity.Property(e => e.EnviadoParaCompras).HasColumnName("Enviado para Compras");

                entity.Property(e => e.EnviadoPréCompra).HasColumnName("Enviado Pré Compra");

                entity.Property(e => e.EnviarPréCompra).HasColumnName("Enviar Pré Compra");

                entity.Property(e => e.GrupoRegistoIvanegocio)
                    .HasColumnName("GrupoRegistoIVANegocio")
                    .HasMaxLength(10);

                entity.Property(e => e.GrupoRegistoIvaproduto)
                    .HasColumnName("GrupoRegistoIVAProduto")
                    .HasMaxLength(10);

                entity.Property(e => e.IdCompra).HasColumnName("ID Compra");

                entity.Property(e => e.MercadoLocal).HasColumnName("Mercado Local");

                entity.Property(e => e.MotivoRecusaMercLocal)
                    .HasColumnName("Motivo Recusa Merc. Local")
                    .HasMaxLength(50);

                entity.Property(e => e.NºCliente)
                    .HasColumnName("Nº Cliente")
                    .HasMaxLength(20);

                entity.Property(e => e.NºDeConsultaMercadoCriada)
                    .HasColumnName("Nº de Consulta Mercado Criada")
                    .HasMaxLength(20);

                entity.Property(e => e.NºEncomendaAberto)
                    .HasColumnName("Nº Encomenda Aberto")
                    .HasMaxLength(20);

                entity.Property(e => e.NºEncomendaCriada)
                    .HasColumnName("Nº Encomenda Criada")
                    .HasMaxLength(20);

                entity.Property(e => e.NºFornecedor)
                    .HasColumnName("Nº Fornecedor")
                    .HasMaxLength(20);

                entity.Property(e => e.NºFuncionário)
                    .HasColumnName("Nº Funcionário")
                    .HasMaxLength(20);

                entity.Property(e => e.NºLinhaEncomendaAberto).HasColumnName("Nº Linha Encomenda Aberto");

                entity.Property(e => e.NºLinhaOrdemManutenção).HasColumnName("Nº Linha Ordem Manutenção");

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.PercentagemDesconto).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.PreçoUnitárioVenda)
                    .HasColumnName("Preço Unitário Venda")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.QtdPorUnidadeDeMedida)
                    .HasColumnName("Qtd por Unidade de Medida")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.QuantidadeADisponibilizar)
                    .HasColumnName("Quantidade a Disponibilizar")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.QuantidadeAReceber)
                    .HasColumnName("Quantidade a Receber")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.QuantidadeARequerer)
                    .HasColumnName("Quantidade a Requerer")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.QuantidadeDisponibilizada)
                    .HasColumnName("Quantidade Disponibilizada")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.QuantidadePendente)
                    .HasColumnName("Quantidade Pendente")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.QuantidadeRecebida)
                    .HasColumnName("Quantidade Recebida")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.QuantidadeRequerida)
                    .HasColumnName("Quantidade Requerida")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.RecusadoCompras).HasColumnName("Recusado Compras");

                entity.Property(e => e.RegiãoMercadoLocal)
                    .HasColumnName("Região Mercado Local")
                    .HasMaxLength(20);

                entity.Property(e => e.UnidadeProdutivaNutrição)
                    .HasColumnName("Unidade Produtiva Nutrição")
                    .HasMaxLength(20);

                entity.Property(e => e.UserMercadoLocal)
                    .HasColumnName("User Mercado Local")
                    .HasMaxLength(20);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.ValidadoCompras).HasColumnName("Validado Compras");

                entity.Property(e => e.ValorOrçamento)
                    .HasColumnName("Valor Orçamento")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.Viatura).HasMaxLength(20);

                entity.HasOne(d => d.NºRequisiçãoNavigation)
                    .WithMany(p => p.LinhasRequisiçãoHist)
                    .HasForeignKey(d => d.NºRequisição)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Linhas Requisição Hist_Requisição");
            });

            modelBuilder.Entity<LinhasRequisiçõesSimplificadas>(entity =>
            {
                entity.HasKey(e => new { e.NºRequisição, e.NºLinha });

                entity.ToTable("Linhas Requisições Simplificadas");

                entity.Property(e => e.NºRequisição)
                    .HasColumnName("Nº Requisição")
                    .HasMaxLength(20);

                entity.Property(e => e.NºLinha)
                    .HasColumnName("Nº Linha")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CustoTotal).HasColumnName("Custo Total");

                entity.Property(e => e.CustoUnitário).HasColumnName("Custo Unitário");

                entity.Property(e => e.CódLocalização)
                    .HasColumnName("Cód. Localização")
                    .HasMaxLength(50);

                entity.Property(e => e.CódUnidadeMedida)
                    .HasColumnName("Cód. Unidade Medida")
                    .HasMaxLength(10);

                entity.Property(e => e.Código).HasMaxLength(20);

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataRequisição)
                    .HasColumnName("Data Requisição")
                    .HasColumnType("date");

                entity.Property(e => e.Descrição).HasMaxLength(100);

                entity.Property(e => e.Descrição2).HasMaxLength(100);

                entity.Property(e => e.NºFuncionário)
                    .HasColumnName("Nº Funcionário")
                    .HasMaxLength(20);

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.QuantidadeAAprovar).HasColumnName("Quantidade a Aprovar");

                entity.Property(e => e.QuantidadeARequerer).HasColumnName("Quantidade a Requerer");

                entity.Property(e => e.QuantidadeAprovada).HasColumnName("Quantidade Aprovada");

                entity.Property(e => e.QuantidadeRecebida).HasColumnName("Quantidade Recebida");

                entity.Property(e => e.TipoRefeição).HasColumnName("Tipo Refeição");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.NºProjetoNavigation)
                    .WithMany(p => p.LinhasRequisiçõesSimplificadas)
                    .HasForeignKey(d => d.NºProjeto)
                    .HasConstraintName("FK_Linhas Requisições Simplificadas_Projetos");

                entity.HasOne(d => d.NºRequisiçãoNavigation)
                    .WithMany(p => p.LinhasRequisiçõesSimplificadas)
                    .HasForeignKey(d => d.NºRequisição)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Linhas Requisições Simplificadas_Requisição");

                entity.HasOne(d => d.NºRequisição1)
                    .WithMany(p => p.LinhasRequisiçõesSimplificadas)
                    .HasForeignKey(d => d.NºRequisição)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Linhas Requisições Simplificadas_Requisições Simplificadas");

                entity.HasOne(d => d.TipoRefeiçãoNavigation)
                    .WithMany(p => p.LinhasRequisiçõesSimplificadas)
                    .HasForeignKey(d => d.TipoRefeição)
                    .HasConstraintName("FK_Linhas Requisições Simplificadas_Tipos Refeição");
            });

            modelBuilder.Entity<Locais>(entity =>
            {
                entity.HasKey(e => e.Código);

                entity.Property(e => e.Contacto).HasMaxLength(20);

                entity.Property(e => e.CódigoPostal)
                    .HasColumnName("Código Postal")
                    .HasMaxLength(20);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(50);

                entity.Property(e => e.Endereço).HasMaxLength(150);

                entity.Property(e => e.Localidade).HasMaxLength(50);

                entity.Property(e => e.ResponsávelReceção)
                    .HasColumnName("Responsável Receção")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Localizações>(entity =>
            {
                entity.HasKey(e => e.Código);

                entity.Property(e => e.Código)
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.ArmazémAmbiente).HasColumnName("Armazém Ambiente");

                entity.Property(e => e.CentroResponsabilidade)
                    .HasColumnName("Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.Cidade).HasMaxLength(20);

                entity.Property(e => e.Contato).HasMaxLength(50);

                entity.Property(e => e.CódPostal)
                    .HasColumnName("Cód. Postal")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoLocalEntrega).HasColumnName("Código Local Entrega");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Endereço).HasMaxLength(100);

                entity.Property(e => e.LocalFornecedor)
                    .HasColumnName("Local Fornecedor")
                    .HasMaxLength(6);

                entity.Property(e => e.Nome).HasMaxLength(100);

                entity.Property(e => e.NºFax)
                    .HasColumnName("Nº Fax")
                    .HasMaxLength(30);

                entity.Property(e => e.Região).HasMaxLength(20);

                entity.Property(e => e.ResponsávelArmazém)
                    .HasColumnName("Responsável Armazém")
                    .HasMaxLength(20);

                entity.Property(e => e.Telefone).HasMaxLength(30);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.Área).HasMaxLength(20);

                entity.HasOne(d => d.CódigoLocalEntregaNavigation)
                    .WithMany(p => p.Localizações)
                    .HasForeignKey(d => d.CódigoLocalEntrega)
                    .HasConstraintName("FK_Localizações_Locais");
            });

            modelBuilder.Entity<MãoDeObraFolhaDeHoras>(entity =>
            {
                entity.HasKey(e => new { e.NºFolhaDeHoras, e.NºLinha });

                entity.ToTable("Mão de Obra Folha de Horas");

                entity.Property(e => e.NºFolhaDeHoras)
                    .HasColumnName("Nº Folha de Horas")
                    .HasMaxLength(20);

                entity.Property(e => e.NºLinha)
                    .HasColumnName("Nº Linha")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CodigoArea).HasMaxLength(20);

                entity.Property(e => e.CodigoCentroResponsabilidade).HasMaxLength(20);

                entity.Property(e => e.CodigoRegiao).HasMaxLength(20);

                entity.Property(e => e.CustoUnitárioDireto).HasColumnName("Custo Unitário Direto");

                entity.Property(e => e.CódUnidadeMedida)
                    .HasColumnName("Cód. Unidade Medida")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoFamíliaRecurso)
                    .HasColumnName("Código Família Recurso")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoTipoOm).HasColumnName("Código Tipo OM");

                entity.Property(e => e.CódigoTipoTrabalho)
                    .HasColumnName("Código Tipo Trabalho")
                    .HasMaxLength(10);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Descricao).HasMaxLength(2000);

                entity.Property(e => e.HoraFim).HasColumnName("Hora Fim");

                entity.Property(e => e.HoraInício).HasColumnName("Hora Início");

                entity.Property(e => e.HorárioAlmoço).HasColumnName("Horário Almoço");

                entity.Property(e => e.HorárioJantar).HasColumnName("Horário Jantar");

                entity.Property(e => e.NºDeHoras).HasColumnName("Nº de Horas");

                entity.Property(e => e.NºEmpregado)
                    .HasColumnName("Nº Empregado")
                    .HasMaxLength(20);

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.NºRecurso)
                    .HasColumnName("Nº Recurso")
                    .HasMaxLength(20);

                entity.Property(e => e.PreçoDeCusto).HasColumnName("Preço de Custo");

                entity.Property(e => e.PreçoDeVenda).HasColumnName("Preço de Venda");

                entity.Property(e => e.PreçoTotal).HasColumnName("Preço Total");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.CódigoTipoOmNavigation)
                    .WithMany(p => p.MãoDeObraFolhaDeHoras)
                    .HasForeignKey(d => d.CódigoTipoOm)
                    .HasConstraintName("FK_Mão de Obra Folha de Horas_Catálogo Manutenção");

                entity.HasOne(d => d.NºFolhaDeHorasNavigation)
                    .WithMany(p => p.MãoDeObraFolhaDeHoras)
                    .HasForeignKey(d => d.NºFolhaDeHoras)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mão de Obra Folha de Horas_Folhas de Horas");
            });

            modelBuilder.Entity<Marcas>(entity =>
            {
                entity.HasKey(e => e.CódigoMarca);

                entity.Property(e => e.CódigoMarca).HasColumnName("Código Marca");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.Property(e => e.Action)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Controller)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HtmlAttributes)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Icon)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RouteParameters)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Modelos>(entity =>
            {
                entity.HasKey(e => new { e.CódigoMarca, e.CódigoModelo });

                entity.Property(e => e.CódigoMarca).HasColumnName("Código Marca");

                entity.Property(e => e.CódigoModelo)
                    .HasColumnName("Código Modelo")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MovimentoDeProdutos>(entity =>
            {
                entity.HasKey(e => e.NºMovimentos);

                entity.ToTable("Movimento de Produtos");

                entity.Property(e => e.NºMovimentos).HasColumnName("Nº Movimentos");

                entity.Property(e => e.CustoUnitário)
                    .HasColumnName("Custo Unitário")
                    .HasColumnType("decimal(, 2)");

                entity.Property(e => e.CódLocalização)
                    .HasColumnName("Cód. Localização")
                    .HasMaxLength(10);

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(10);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(10);

                entity.Property(e => e.CódigoÁrea)
                    .HasColumnName("Código Área")
                    .HasMaxLength(10);

                entity.Property(e => e.DataRegisto)
                    .HasColumnName("Data Registo")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(30);

                entity.Property(e => e.NºDocumento).HasColumnName("Nº Documento");

                entity.Property(e => e.NºProduto)
                    .HasColumnName("Nº Produto")
                    .HasMaxLength(10);

                entity.Property(e => e.NºProjecto).HasColumnName("Nº Projecto");

                entity.Property(e => e.Quantidade).HasColumnType("decimal(, 2)");

                entity.Property(e => e.TipoMovimento).HasColumnName("Tipo Movimento");

                entity.Property(e => e.Valor).HasColumnType("decimal(, 2)");
            });

            modelBuilder.Entity<MovimentosCafetariaRefeitório>(entity =>
            {
                entity.HasKey(e => e.NºMovimento);

                entity.ToTable("Movimentos Cafetaria/Refeitório");

                entity.Property(e => e.NºMovimento).HasColumnName("Nº Movimento");

                entity.Property(e => e.CódigoCafetariaRefeitório).HasColumnName("Código Cafetaria/Refeitório");

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraSistemaRegisto)
                    .HasColumnName("Data/Hora Sistema Registo")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataRegisto)
                    .HasColumnName("Data Registo")
                    .HasColumnType("date");

                entity.Property(e => e.Descrição).HasMaxLength(50);

                entity.Property(e => e.DescriçãoTipoRefeição)
                    .HasColumnName("Descrição Tipo Refeição")
                    .HasMaxLength(50);

                entity.Property(e => e.NºRecurso)
                    .HasColumnName("Nº Recurso")
                    .HasMaxLength(20);

                entity.Property(e => e.NºUnidadeProdutiva).HasColumnName("Nº Unidade Produtiva");

                entity.Property(e => e.TipoMovimento).HasColumnName("Tipo Movimento");

                entity.Property(e => e.TipoRefeição).HasColumnName("Tipo Refeição");

                entity.Property(e => e.Utilizador).HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.NºUnidadeProdutivaNavigation)
                    .WithMany(p => p.MovimentosCafetariaRefeitório)
                    .HasForeignKey(d => d.NºUnidadeProdutiva)
                    .HasConstraintName("FK_Movimentos Cafetaria/Refeitório_Unidades Produtivas");
            });

            modelBuilder.Entity<MovimentosDeAprovação>(entity =>
            {
                entity.HasKey(e => e.NºMovimento);

                entity.ToTable("Movimentos de Aprovação");

                entity.Property(e => e.NºMovimento).HasColumnName("Nº Movimento");

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataHoraAprovação)
                    .HasColumnName("Data/Hora Aprovação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.MotivoDeRecusa)
                    .HasColumnName("Motivo de Recusa")
                    .HasColumnType("text");

                entity.Property(e => e.Número).HasMaxLength(20);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorSolicitou)
                    .HasColumnName("Utilizador Solicitou")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MovimentosDeProjeto>(entity =>
            {
                entity.HasKey(e => e.NºLinha);

                entity.ToTable("Movimentos De Projeto");

                entity.Property(e => e.NºLinha).HasColumnName("Nº Linha");

                entity.Property(e => e.AcertoDePreços).HasColumnName("Acerto de Preços");

                entity.Property(e => e.AutorizadoPor).HasMaxLength(50);

                entity.Property(e => e.CodCliente).HasMaxLength(20);

                entity.Property(e => e.CodigoLer).HasMaxLength(20);

                entity.Property(e => e.CustoTotal).HasColumnName("Custo Total");

                entity.Property(e => e.CustoUnitário)
                    .HasColumnName("Custo Unitário")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.CódDestinoFinalResíduos).HasColumnName("Cód. Destino Final Resíduos");

                entity.Property(e => e.CódGrupoServiço)
                    .HasColumnName("Cód. Grupo Serviço")
                    .HasMaxLength(20);

                entity.Property(e => e.CódLocalização)
                    .HasColumnName("Cód. Localização")
                    .HasMaxLength(10);

                entity.Property(e => e.CódServiçoCliente)
                    .HasColumnName("Cód Serviço Cliente")
                    .HasMaxLength(20);

                entity.Property(e => e.CódUnidadeMedida)
                    .HasColumnName("Cód. Unidade Medida")
                    .HasMaxLength(10);

                entity.Property(e => e.Código).HasMaxLength(20);

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.Data).HasColumnType("date");

                entity.Property(e => e.DataAutorizaçãoFaturação)
                    .HasColumnName("Data Autorização Faturação")
                    .HasColumnType("date");

                entity.Property(e => e.DataConsumo)
                    .HasColumnName("Data Consumo")
                    .HasColumnType("date");

                entity.Property(e => e.DataDocumentoCorrigido)
                    .HasColumnName("Data Documento Corrigido")
                    .HasColumnType("date");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(100);

                entity.Property(e => e.CodigoTipoTrabalho).HasMaxLength(10);

                entity.Property(e => e.DocumentoCorrigido)
                    .HasColumnName("Documento Corrigido")
                    .HasMaxLength(20);

                entity.Property(e => e.DocumentoOriginal)
                    .HasColumnName("Documento Original")
                    .HasMaxLength(20);

                entity.Property(e => e.FaturaANºCliente)
                    .HasColumnName("Fatura-a Nº Cliente")
                    .HasMaxLength(20);

                entity.Property(e => e.FaturaçãoAutorizada).HasColumnName("Faturação Autorizada");

                entity.Property(e => e.FaturaçãoAutorizada2).HasColumnName("Faturação Autorizada2");

                entity.Property(e => e.Grupo).HasMaxLength(20);

                entity.Property(e => e.GrupoContabProjeto)
                    .HasColumnName("Grupo Contab. Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.GrupoFaturaDescricao).HasMaxLength(30);

                entity.Property(e => e.Matricula).HasMaxLength(20);

                entity.Property(e => e.Moeda).HasMaxLength(20);

                entity.Property(e => e.Motorista).HasMaxLength(80);

                entity.Property(e => e.NºDocumento)
                    .HasColumnName("Nº Documento")
                    .HasMaxLength(20);

                entity.Property(e => e.NºFolhaHoras)
                    .HasColumnName("Nº Folha Horas")
                    .HasMaxLength(20);

                entity.Property(e => e.NºFuncionário)
                    .HasColumnName("Nº Funcionário")
                    .HasMaxLength(20);

                entity.Property(e => e.NºGuiaExterna)
                    .HasColumnName("Nº Guia Externa")
                    .HasMaxLength(80);

                entity.Property(e => e.NºGuiaResíduos)
                    .HasColumnName("Nº Guia Resíduos")
                    .HasMaxLength(80);

                entity.Property(e => e.NºLinhaRequisição).HasColumnName("Nº Linha Requisição");

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.NºRequisição)
                    .HasColumnName("Nº Requisição")
                    .HasMaxLength(20);

                entity.Property(e => e.Operacao).HasMaxLength(20);

                entity.Property(e => e.PreçoTotal).HasColumnName("Preço Total");

                entity.Property(e => e.PreçoUnitário)
                    .HasColumnName("Preço Unitário")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.QuantidadeDevolvida).HasColumnName("Quantidade Devolvida");

                entity.Property(e => e.RequisiçãoInterna)
                    .HasColumnName("Requisição Interna")
                    .HasMaxLength(30);

                entity.Property(e => e.TipoMovimento).HasColumnName("Tipo Movimento");

                entity.Property(e => e.TipoRecurso).HasColumnName("Tipo Recurso");

                entity.Property(e => e.TipoRefeição).HasColumnName("Tipo Refeição");

                entity.Property(e => e.Utilizador).HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.ValorUnitárioAFaturar).HasColumnName("Valor Unitário a Faturar");

                entity.Property(e => e.Selecionada)
                    .HasColumnName("Selecionada")
                    .HasDefaultValueSql("((0))");


                entity.HasOne(d => d.CódDestinoFinalResíduosNavigation)
                    .WithMany(p => p.MovimentosDeProjeto)
                    .HasForeignKey(d => d.CódDestinoFinalResíduos)
                    .HasConstraintName("FK_Movimentos De Projeto_Destinos Finais Resíduos");

                entity.HasOne(d => d.NºProjetoNavigation)
                    .WithMany(p => p.MovimentosDeProjeto)
                    .HasForeignKey(d => d.NºProjeto)
                    .HasConstraintName("FK_Movimentos De Projeto_Projetos");

                entity.HasOne(d => d.NºRequisiçãoNavigation)
                    .WithMany(p => p.MovimentosDeProjeto)
                    .HasForeignKey(d => d.NºRequisição)
                    .HasConstraintName("FK_Movimentos De Projeto_Requisição");

                entity.HasOne(d => d.TipoRefeiçãoNavigation)
                    .WithMany(p => p.MovimentosDeProjeto)
                    .HasForeignKey(d => d.TipoRefeição)
                    .HasConstraintName("FK_Movimentos De Projeto_Tipos Refeição");
            });

            modelBuilder.Entity<MovimentosProjectoAutorizados>(entity =>
            {
                entity.HasKey(e => e.NumMovimento);

                entity.ToTable("Movimentos Projecto Autorizados");

                entity.Property(e => e.NumMovimento).ValueGeneratedNever();

                entity.Property(e => e.CodAreaFuncional)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CodCentroResponsabilidade)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CodCliente).HasMaxLength(20);

                entity.Property(e => e.CodContrato)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CodGrupoServico).HasMaxLength(20);

                entity.Property(e => e.CodProjeto)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CodRegiao)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CodServCliente)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CodUnidadeMedida)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DataConsumo).HasColumnType("date");

                entity.Property(e => e.DataRegisto).HasColumnType("date");

                entity.Property(e => e.DescServCliente).HasMaxLength(80);

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.GrupoFactura).HasColumnName("Grupo Factura");

                entity.Property(e => e.GrupoFaturaDescricao).HasMaxLength(30);

                entity.Property(e => e.NumDocumento)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NumGuiaExterna).HasMaxLength(80);

                entity.Property(e => e.NumGuiaResiduosGar)
                    .HasColumnName("NumGuiaResiduos_GAR")
                    .HasMaxLength(80);
            });

            modelBuilder.Entity<MovimentosTelefones>(entity =>
            {
                entity.ToTable("Movimentos Telefones");

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.Data).HasColumnType("date");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Fornecedor).HasMaxLength(20);

                entity.Property(e => e.MovimentoManual).HasColumnName("Movimento Manual");

                entity.Property(e => e.NºFatura)
                    .HasColumnName("Nº Fatura")
                    .HasMaxLength(20);

                entity.Property(e => e.NºFaturaNav)
                    .HasColumnName("Nº Fatura NAV")
                    .HasMaxLength(20);

                entity.Property(e => e.NºTelefone)
                    .HasColumnName("Nº Telefone")
                    .HasMaxLength(9);

                entity.Property(e => e.Período).HasMaxLength(20);

                entity.Property(e => e.TipoCusto).HasColumnName("Tipo Custo");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.ValorComIva).HasColumnName("Valor com IVA");

                entity.Property(e => e.ValorSemIva).HasColumnName("Valor sem IVA");

                entity.HasOne(d => d.NºTelefoneNavigation)
                    .WithMany(p => p.MovimentosTelefones)
                    .HasForeignKey(d => d.NºTelefone)
                    .HasConstraintName("FK_Movimentos Telefones_Telefones");
            });

            modelBuilder.Entity<MovimentosTelemóveis>(entity =>
            {
                entity.ToTable("Movimentos Telemóveis");

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.Data).HasColumnType("date");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificaçao)
                    .HasColumnName("Data/Hora Modificaçao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.NºFaturaNav)
                    .HasColumnName("Nº Fatura NAV")
                    .HasMaxLength(20);

                entity.Property(e => e.NúmeroFatura)
                    .HasColumnName("Número Fatura")
                    .HasMaxLength(20);

                entity.Property(e => e.NúmeroTelemóvel)
                    .HasColumnName("Número Telemóvel")
                    .HasMaxLength(9);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.ValorComIva).HasColumnName("Valor com IVA");

                entity.Property(e => e.ValorSemIva).HasColumnName("Valor sem IVA");

                entity.HasOne(d => d.NúmeroTelemóvelNavigation)
                    .WithMany(p => p.MovimentosTelemóveis)
                    .HasForeignKey(d => d.NúmeroTelemóvel)
                    .HasConstraintName("FK_Movimentos Telemóveis_Cartões Telemóveis");
            });

            modelBuilder.Entity<MovimentosViaturas>(entity =>
            {
                entity.HasKey(e => e.NºMovimento);

                entity.ToTable("Movimentos Viaturas");

                entity.Property(e => e.NºMovimento).HasColumnName("Nº Movimento");

                entity.Property(e => e.Apólice).HasMaxLength(20);

                entity.Property(e => e.CartãoCombustível)
                    .HasColumnName("Cartão Combustível")
                    .HasMaxLength(20);

                entity.Property(e => e.CustoUnitário).HasColumnName("Custo Unitário");

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraMovimento)
                    .HasColumnName("Data/Hora Movimento")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataRegisto)
                    .HasColumnName("Data Registo")
                    .HasColumnType("date");

                entity.Property(e => e.Descrição).HasMaxLength(100);

                entity.Property(e => e.LocalidadePostoCombustível)
                    .HasColumnName("Localidade Posto Combustível")
                    .HasMaxLength(80);

                entity.Property(e => e.Matrícula).HasMaxLength(20);

                entity.Property(e => e.Nd).HasColumnName("% ND");

                entity.Property(e => e.NomePostoCombustível)
                    .HasColumnName("Nome Posto Combustível")
                    .HasMaxLength(80);

                entity.Property(e => e.NºDocumento)
                    .HasColumnName("Nº Documento")
                    .HasMaxLength(20);

                entity.Property(e => e.NºFornecedor)
                    .HasColumnName("Nº Fornecedor")
                    .HasMaxLength(20);

                entity.Property(e => e.NºFuncionário)
                    .HasColumnName("Nº Funcionário")
                    .HasMaxLength(20);

                entity.Property(e => e.NºRecurso)
                    .HasColumnName("Nº Recurso")
                    .HasMaxLength(20);

                entity.Property(e => e.TipoMovimento).HasColumnName("Tipo Movimento");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.MatrículaNavigation)
                    .WithMany(p => p.MovimentosViaturas)
                    .HasForeignKey(d => d.Matrícula)
                    .HasConstraintName("FK_Movimentos Viaturas_Viaturas");
            });

            modelBuilder.Entity<NotasProcedimentosCcp>(entity =>
            {
                entity.HasKey(e => new { e.NºProcedimento, e.NºLinha });

                entity.ToTable("Notas Procedimentos CCP");

                entity.Property(e => e.NºProcedimento)
                    .HasColumnName("Nº Procedimento")
                    .HasMaxLength(10);

                entity.Property(e => e.NºLinha)
                    .HasColumnName("Nº Linha")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.DataHora)
                    .HasColumnName("Data/Hora")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Nota).HasColumnType("text");

                entity.Property(e => e.Utilizador).HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.NºProcedimentoNavigation)
                    .WithMany(p => p.NotasProcedimentosCcp)
                    .HasForeignKey(d => d.NºProcedimento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notas Procedimentos CCP_Procedimentos CCP");
            });

            modelBuilder.Entity<ObjetosDeServiço>(entity =>
            {
                entity.HasKey(e => e.Código);

                entity.ToTable("Objetos de Serviço");

                entity.Property(e => e.CódÁrea)
                    .HasColumnName("Cód. Área")
                    .HasMaxLength(20);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(250);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<OrigemDestinoFh>(entity =>
            {
                entity.HasKey(e => e.Código);

                entity.ToTable("Origem_Destino_FH");

                entity.Property(e => e.Código)
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.AlteradoPor)
                    .HasColumnName("Alterado Por")
                    .HasMaxLength(50);

                entity.Property(e => e.CriadoPor)
                    .HasColumnName("Criado Por")
                    .HasMaxLength(50);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraÚltimaAlteração)
                    .HasColumnName("Data/Hora Última Alteração")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(200);
            });

            modelBuilder.Entity<PercursosEAjudasCustoDespesasFolhaDeHoras>(entity =>
            {
                entity.HasKey(e => new { e.NºFolhaDeHoras, e.CodPercursoAjuda, e.NºLinha });

                entity.ToTable("Percursos e Ajudas Custo/Despesas Folha de Horas");

                entity.Property(e => e.NºFolhaDeHoras)
                    .HasColumnName("Nº Folha de Horas")
                    .HasMaxLength(20);

                entity.Property(e => e.NºLinha)
                    .HasColumnName("Nº Linha")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CustoTotal).HasColumnName("Custo Total");

                entity.Property(e => e.CustoUnitário).HasColumnName("Custo Unitário");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataViagem)
                    .HasColumnName("Data Viagem")
                    .HasColumnType("date");

                entity.Property(e => e.Descrição).HasMaxLength(50);

                entity.Property(e => e.Destino).HasMaxLength(50);

                entity.Property(e => e.DestinoDescricao).HasMaxLength(200);

                entity.Property(e => e.Justificação).HasColumnType("text");

                entity.Property(e => e.Origem).HasMaxLength(50);

                entity.Property(e => e.OrigemDescricao).HasMaxLength(200);

                entity.Property(e => e.PreçoUnitário).HasColumnName("Preço Unitário");

                entity.Property(e => e.RúbricaSalarial)
                    .HasColumnName("Rúbrica Salarial")
                    .HasMaxLength(20);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.NºFolhaDeHorasNavigation)
                    .WithMany(p => p.PercursosEAjudasCustoDespesasFolhaDeHoras)
                    .HasForeignKey(d => d.NºFolhaDeHoras)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Percursos e Ajudas Custo/Despesas Folha de Horas_Folhas de Horas");
            });

            modelBuilder.Entity<PerfisModelo>(entity =>
            {
                entity.ToTable("Perfis Modelo");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PerfisUtilizador>(entity =>
            {
                entity.HasKey(e => new { e.IdUtilizador, e.IdPerfil });

                entity.ToTable("Perfis Utilizador");

                entity.Property(e => e.IdUtilizador)
                    .HasColumnName("Id Utilizador")
                    .HasMaxLength(50);

                entity.Property(e => e.IdPerfil).HasColumnName("Id Perfil");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdPerfilNavigation)
                    .WithMany(p => p.PerfisUtilizador)
                    .HasForeignKey(d => d.IdPerfil)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Perfis Utilizador_Perfis Modelo");

                entity.HasOne(d => d.IdUtilizadorNavigation)
                    .WithMany(p => p.PerfisUtilizador)
                    .HasForeignKey(d => d.IdUtilizador)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Perfis Utilizador_Config. Utilizadores");
            });

            modelBuilder.Entity<PontosSituaçãoRq>(entity =>
            {
                entity.HasKey(e => new { e.NºRequisição, e.NºPedido });

                entity.ToTable("Pontos Situação RQ");

                entity.Property(e => e.NºRequisição)
                    .HasColumnName("Nº Requisição")
                    .HasMaxLength(20);

                entity.Property(e => e.NºPedido)
                    .HasColumnName("Nº Pedido")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ConfirmaçãoLeitura).HasColumnName("Confirmação Leitura");

                entity.Property(e => e.DataPedido)
                    .HasColumnName("Data Pedido")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataResposta)
                    .HasColumnName("Data Resposta")
                    .HasColumnType("datetime");

                entity.Property(e => e.PedidoDePontoSituação)
                    .IsRequired()
                    .HasColumnName("Pedido de Ponto Situação")
                    .HasMaxLength(2000);

                entity.Property(e => e.Resposta).HasMaxLength(2000);

                entity.Property(e => e.UtilizadorPedido)
                    .HasColumnName("Utilizador Pedido")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorResposta)
                    .HasColumnName("Utilizador Resposta")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PrecoCustoRecursoFh>(entity =>
            {
                entity.HasKey(e => new { e.Code, e.CodTipoTrabalho, e.StartingDate });

                entity.ToTable("Preco_Custo_Recurso_FH");

                entity.Property(e => e.Code).HasMaxLength(20);

                entity.Property(e => e.CodTipoTrabalho).HasMaxLength(10);

                entity.Property(e => e.StartingDate).HasColumnType("date");

                entity.Property(e => e.AlteradoPor).HasMaxLength(50);

                entity.Property(e => e.CriadoPor).HasMaxLength(50);

                entity.Property(e => e.DataHoraCriacao).HasColumnType("datetime");

                entity.Property(e => e.DataHoraUltimaAlteracao).HasColumnType("datetime");

                entity.Property(e => e.Descricao).HasMaxLength(50);

                entity.Property(e => e.EndingDate).HasColumnType("date");

                entity.Property(e => e.FamiliaRecurso).HasMaxLength(20);
            });

            modelBuilder.Entity<PreçosFornecedor>(entity =>
            {
                entity.HasKey(e => new { e.NºFornecedor, e.NºProduto, e.DataValidadeInício, e.CódigoCentroResponsabilidade, e.CódLocalização });

                entity.ToTable("Preços Fornecedor");

                entity.Property(e => e.NºFornecedor)
                    .HasColumnName("Nº Fornecedor")
                    .HasMaxLength(20);

                entity.Property(e => e.NºProduto)
                    .HasColumnName("Nº Produto")
                    .HasMaxLength(20);

                entity.Property(e => e.DataValidadeInício)
                    .HasColumnName("Data Validade Início")
                    .HasColumnType("date");

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódLocalização)
                    .HasColumnName("Cód. Localização")
                    .HasMaxLength(10);

                entity.Property(e => e.CustoUnitário).HasColumnName("Custo Unitário");

                entity.Property(e => e.CódUnidadeMedida)
                    .HasColumnName("Cód. Unidade Medida")
                    .HasMaxLength(10);

                entity.Property(e => e.CódigoProdutoFornecedor)
                    .HasColumnName("Código Produto Fornecedor")
                    .HasMaxLength(30);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraSistema)
                    .HasColumnName("Data/Hora Sistema")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataValidadeFim)
                    .HasColumnName("Data Validade Fim")
                    .HasColumnType("date");

                entity.Property(e => e.DescriçãoProdutoFornecedor)
                    .HasColumnName("Descrição Produto Fornecedor")
                    .HasMaxLength(80);

                entity.Property(e => e.FormaEntrega).HasColumnName("Forma Entrega");

                entity.Property(e => e.Marca).HasMaxLength(60);

                entity.Property(e => e.PesoUnitário).HasColumnName("Peso Unitário");

                entity.Property(e => e.QuartaFeira).HasColumnName("Quarta-Feira");

                entity.Property(e => e.QuintaFeira).HasColumnName("Quinta-Feira");

                entity.Property(e => e.SegundaFeira).HasColumnName("Segunda-Feira");

                entity.Property(e => e.SextaFeira).HasColumnName("Sexta-Feira");

                entity.Property(e => e.SubFornecedor)
                    .HasColumnName("Sub. Fornecedor")
                    .HasMaxLength(30);

                entity.Property(e => e.TerçaFeira).HasColumnName("Terça-Feira");

                entity.Property(e => e.TipoPreço).HasColumnName("Tipo Preço");

                entity.Property(e => e.UnidadeMedidaFornecedor)
                    .HasColumnName("Unidade Medida Fornecedor")
                    .HasMaxLength(20);

                entity.Property(e => e.Utilizador).HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PreçosServiçosCliente>(entity =>
            {
                entity.HasKey(e => new { e.Cliente, e.CodServCliente, e.Recurso });

                entity.ToTable("Preços Serviços Cliente");

                entity.Property(e => e.Cliente).HasMaxLength(50);

                entity.Property(e => e.CodServCliente)
                    .HasColumnName("Cod.Serv.Cliente")
                    .HasMaxLength(50);

                entity.Property(e => e.Recurso).HasMaxLength(20);

                entity.Property(e => e.CodigoArea)
                    .HasColumnName("Codigo Area")
                    .HasMaxLength(20);

                entity.Property(e => e.CodigoCentroResponsabilidade)
                    .HasColumnName("Codigo Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CodigoRegião)
                    .HasColumnName("Codigo Região")
                    .HasMaxLength(20);

                entity.Property(e => e.Data).HasColumnType("date");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DescriçãoDoRecurso)
                    .HasColumnName("Descrição do Recurso")
                    .HasMaxLength(80);

                entity.Property(e => e.DescriçãoServiço)
                    .HasColumnName("Descrição Serviço")
                    .HasMaxLength(80);

                entity.Property(e => e.DescriçãoTipoRefeição)
                    .HasColumnName("Descrição Tipo Refeição")
                    .HasMaxLength(30);

                entity.Property(e => e.Nome).HasMaxLength(80);

                entity.Property(e => e.Nome2)
                    .HasColumnName("Nome 2")
                    .HasMaxLength(50);

                entity.Property(e => e.PreçoDeCusto)
                    .HasColumnName("Preço de Custo")
                    .HasColumnType("decimal(, 20)");

                entity.Property(e => e.PreçoVenda)
                    .HasColumnName("Preço Venda")
                    .HasColumnType("decimal(, 20)");

                entity.Property(e => e.Quantidade)
                    .HasColumnName("Quantidade")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.TipoRefeição)
                    .HasColumnName("Tipo Refeição")
                    .HasMaxLength(20);

                entity.Property(e => e.UnidadeMedida)
                    .HasColumnName("Unidade Medida")
                    .HasMaxLength(10);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.CodServClienteNavigation)
                    .WithMany(p => p.PreçosServiçosCliente)
                    .HasForeignKey(d => d.CodServCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Preços Serviços Cliente_Serviços");
            });

            modelBuilder.Entity<PrecoVendaRecursoFh>(entity =>
            {
                entity.HasKey(e => new { e.Code, e.CodTipoTrabalho, e.StartingDate });

                entity.ToTable("Preco_Venda_Recurso_FH");

                entity.Property(e => e.Code).HasMaxLength(20);

                entity.Property(e => e.CodTipoTrabalho).HasMaxLength(10);

                entity.Property(e => e.StartingDate).HasColumnType("date");

                entity.Property(e => e.AlteradoPor).HasMaxLength(50);

                entity.Property(e => e.CriadoPor).HasMaxLength(50);

                entity.Property(e => e.DataHoraCriacao).HasColumnType("datetime");

                entity.Property(e => e.DataHoraUltimaAlteracao).HasColumnType("datetime");

                entity.Property(e => e.Descricao).HasMaxLength(50);

                entity.Property(e => e.EndingDate).HasColumnType("date");

                entity.Property(e => e.FamiliaRecurso).HasMaxLength(20);
            });

            modelBuilder.Entity<PréMovimentosProjeto>(entity =>
            {
                entity.HasKey(e => e.NºLinha);

                entity.ToTable("Pré-Movimentos Projeto");

                entity.Property(e => e.NºLinha).HasColumnName("Nº Linha");

                entity.Property(e => e.AcertoDePreços).HasColumnName("Acerto de Preços");

                entity.Property(e => e.CustoTotal).HasColumnName("Custo Total");

                entity.Property(e => e.CustoUnitário)
                    .HasColumnName("Custo Unitário")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.CódDestinoFinalResíduos).HasColumnName("Cód. Destino Final Resíduos");

                entity.Property(e => e.CódGrupoServiço)
                    .HasColumnName("Cód. Grupo Serviço")
                    .HasMaxLength(20);

                entity.Property(e => e.CódLocalização)
                    .HasColumnName("Cód. Localização")
                    .HasMaxLength(10);

                entity.Property(e => e.CódServiçoCliente)
                    .HasColumnName("Cód Serviço Cliente")
                    .HasMaxLength(20);

                entity.Property(e => e.CódUnidadeMedida)
                    .HasColumnName("Cód. Unidade Medida")
                    .HasMaxLength(10);

                entity.Property(e => e.Código).HasMaxLength(20);

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.Data).HasColumnType("date");

                entity.Property(e => e.DataAutorizaçãoFaturação)
                    .HasColumnName("Data Autorização Faturação")
                    .HasColumnType("date");

                entity.Property(e => e.DataConsumo)
                    .HasColumnName("Data Consumo")
                    .HasColumnType("date");

                entity.Property(e => e.DataDocumentoCorrigido)
                    .HasColumnName("Data Documento Corrigido")
                    .HasColumnType("date");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(100);

                entity.Property(e => e.DocumentoCorrigido)
                    .HasColumnName("Documento Corrigido")
                    .HasMaxLength(20);

                entity.Property(e => e.DocumentoOriginal)
                    .HasColumnName("Documento Original")
                    .HasMaxLength(20);

                entity.Property(e => e.FaturaANºCliente)
                    .HasColumnName("Fatura-a Nº Cliente")
                    .HasMaxLength(20);

                entity.Property(e => e.FaturaçãoAutorizada).HasColumnName("Faturação Autorizada");

                entity.Property(e => e.GrupoContabProjeto)
                    .HasColumnName("Grupo Contab. Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.Moeda).HasMaxLength(20);

                entity.Property(e => e.Motorista).HasMaxLength(80);

                entity.Property(e => e.NºDocumento)
                    .HasColumnName("Nº Documento")
                    .HasMaxLength(20);

                entity.Property(e => e.NºFolhaHoras)
                    .HasColumnName("Nº Folha Horas")
                    .HasMaxLength(20);

                entity.Property(e => e.NºFuncionário)
                    .HasColumnName("Nº Funcionário")
                    .HasMaxLength(20);

                entity.Property(e => e.NºGuiaExterna)
                    .HasColumnName("Nº Guia Externa")
                    .HasMaxLength(80);

                entity.Property(e => e.NºGuiaResíduos)
                    .HasColumnName("Nº Guia Resíduos")
                    .HasMaxLength(80);

                entity.Property(e => e.NºLinhaRequisição).HasColumnName("Nº Linha Requisição");

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.NºRequisição)
                    .HasColumnName("Nº Requisição")
                    .HasMaxLength(20);

                entity.Property(e => e.PreçoTotal).HasColumnName("Preço Total");

                entity.Property(e => e.PreçoUnitário)
                    .HasColumnName("Preço Unitário")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.QuantidadeDevolvida).HasColumnName("Quantidade Devolvida");

                entity.Property(e => e.RequisiçãoInterna)
                    .HasColumnName("Requisição Interna")
                    .HasMaxLength(30);

                entity.Property(e => e.TipoMovimento).HasColumnName("Tipo Movimento");

                entity.Property(e => e.TipoRecurso).HasColumnName("Tipo Recurso");

                entity.Property(e => e.TipoRefeição).HasColumnName("Tipo Refeição");

                entity.Property(e => e.Utilizador).HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.ValorUnitárioAFaturar).HasColumnName("Valor Unitário a Faturar");

                entity.HasOne(d => d.CódDestinoFinalResíduosNavigation)
                    .WithMany(p => p.PréMovimentosProjeto)
                    .HasForeignKey(d => d.CódDestinoFinalResíduos)
                    .HasConstraintName("FK_Pré-Movimentos Projeto_Destinos Finais Resíduos");

                entity.HasOne(d => d.NºProjetoNavigation)
                    .WithMany(p => p.PréMovimentosProjeto)
                    .HasForeignKey(d => d.NºProjeto)
                    .HasConstraintName("FK_Pré-Movimentos Projeto_Projetos");

                entity.HasOne(d => d.NºRequisiçãoNavigation)
                    .WithMany(p => p.PréMovimentosProjeto)
                    .HasForeignKey(d => d.NºRequisição)
                    .HasConstraintName("FK_Pré-Movimentos Projeto_Requisição");

                entity.HasOne(d => d.TipoRefeiçãoNavigation)
                    .WithMany(p => p.PréMovimentosProjeto)
                    .HasForeignKey(d => d.TipoRefeição)
                    .HasConstraintName("FK_Pré-Movimentos Projeto_Tipos Refeição");
            });

            modelBuilder.Entity<PréRequisição>(entity =>
            {
                entity.HasKey(e => e.NºPréRequisição);

                entity.ToTable("Pré-Requisição");

                entity.Property(e => e.NºPréRequisição)
                    .HasColumnName("Nº Pré-Requisição")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.Aprovadores).HasMaxLength(100);

                entity.Property(e => e.CabimentoOrçamental).HasColumnName("Cabimento Orçamental");

                entity.Property(e => e.CodigoPostalRecolha)
                    .HasColumnName("Codigo Postal Recolha")
                    .HasMaxLength(20);

                entity.Property(e => e.CompraADinheiro).HasColumnName("Compra a Dinheiro");

                entity.Property(e => e.ContatoEntrega)
                    .HasColumnName("Contato Entrega")
                    .HasMaxLength(50);

                entity.Property(e => e.ContatoRecolha)
                    .HasColumnName("Contato Recolha")
                    .HasMaxLength(50);

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoLocalEntrega).HasColumnName("Código Local Entrega");

                entity.Property(e => e.CódigoLocalRecolha).HasColumnName("Código Local Recolha");

                entity.Property(e => e.CódigoLocalização)
                    .HasColumnName("Código Localização")
                    .HasMaxLength(10);

                entity.Property(e => e.CódigoPostalEntrega)
                    .HasColumnName("Código Postal Entrega")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataAprovação)
                    .HasColumnName("Data Aprovação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataEntregaArmazém)
                    .HasColumnName("Data Entrega Armazém")
                    .HasColumnType("date");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataReceção)
                    .HasColumnName("Data Receção")
                    .HasColumnType("date");

                entity.Property(e => e.DataValidação)
                    .HasColumnName("Data Validação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Emm).HasColumnName("EMM");

                entity.Property(e => e.JáExecutado).HasColumnName("Já Executado");

                entity.Property(e => e.LocalDeRecolha).HasColumnName("Local de Recolha");

                entity.Property(e => e.LocalEntrega).HasColumnName("Local Entrega");

                entity.Property(e => e.LocalidadeEntrega)
                    .HasColumnName("Localidade Entrega")
                    .HasMaxLength(50);

                entity.Property(e => e.LocalidadeRecolha)
                    .HasColumnName("Localidade Recolha")
                    .HasMaxLength(50);

                entity.Property(e => e.NoDocumento)
                    .HasColumnName("NoDocumento")
                    .HasMaxLength(50);

                entity.Property(e => e.MercadoLocal).HasColumnName("Mercado Local");

                entity.Property(e => e.ModeloDePréRequisição).HasColumnName("Modelo de Pré-Requisição");

                entity.Property(e => e.Morada2Entrega)
                    .HasColumnName("Morada 2 Entrega")
                    .HasMaxLength(100);

                entity.Property(e => e.Morada2Recolha)
                    .HasColumnName("Morada 2 Recolha")
                    .HasMaxLength(100);

                entity.Property(e => e.MoradaEntrega)
                    .HasColumnName("Morada Entrega")
                    .HasMaxLength(100);

                entity.Property(e => e.MoradaRecolha)
                    .HasColumnName("Morada Recolha")
                    .HasMaxLength(100);

                entity.Property(e => e.NºFatura)
                    .HasColumnName("Nº Fatura")
                    .HasMaxLength(20);

                entity.Property(e => e.NºFuncionário)
                    .HasColumnName("Nº Funcionário")
                    .HasMaxLength(20);

                entity.Property(e => e.NºProcedimentoCcp)
                    .HasColumnName("Nº Procedimento CCP")
                    .HasMaxLength(20);

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.NºRequesiçãoReclamada)
                    .HasColumnName("Nº Requesição Reclamada")
                    .HasMaxLength(20);

                entity.Property(e => e.Observações).HasColumnType("text");

                entity.Property(e => e.RegiãoMercadoLocal)
                    .HasColumnName("Região Mercado Local")
                    .HasMaxLength(20);

                entity.Property(e => e.ReparaçãoComGarantia).HasColumnName("Reparação com Garantia");

                entity.Property(e => e.ReposiçãoDeStock).HasColumnName("Reposição de Stock");

                entity.Property(e => e.RequisiçãoDetergentes).HasColumnName("Requisição Detergentes");

                entity.Property(e => e.RequisiçãoNutrição).HasColumnName("Requisição Nutrição");

                entity.Property(e => e.ResponsávelAprovação)
                    .HasColumnName("Responsável Aprovação")
                    .HasMaxLength(20);

                entity.Property(e => e.ResponsávelCriação)
                    .HasColumnName("Responsável Criação")
                    .HasMaxLength(20);

                entity.Property(e => e.ResponsávelReceção)
                    .HasColumnName("Responsável Receção")
                    .HasMaxLength(20);

                entity.Property(e => e.ResponsávelReceçãoReceção)
                    .HasColumnName("Responsável Receção Receção")
                    .HasMaxLength(50);

                entity.Property(e => e.ResponsávelReceçãoRecolha)
                    .HasColumnName("Responsável Receção Recolha")
                    .HasMaxLength(50);

                entity.Property(e => e.ResponsávelValidação)
                    .HasColumnName("Responsável Validação")
                    .HasMaxLength(20);

                entity.Property(e => e.TipoRequisição)
                    .HasColumnName("Tipo Requisição")
                    .HasMaxLength(20);

                entity.Property(e => e.UnidadeProdutivaAlimentação)
                    .HasColumnName("Unidade Produtiva Alimentação")
                    .HasMaxLength(20);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.Viatura).HasMaxLength(20);

                entity.HasOne(d => d.CódigoLocalEntregaNavigation)
                    .WithMany(p => p.PréRequisiçãoCódigoLocalEntregaNavigation)
                    .HasForeignKey(d => d.CódigoLocalEntrega)
                    .HasConstraintName("FK_Pré-Requisição_Locais1");

                entity.HasOne(d => d.CódigoLocalRecolhaNavigation)
                    .WithMany(p => p.PréRequisiçãoCódigoLocalRecolhaNavigation)
                    .HasForeignKey(d => d.CódigoLocalRecolha)
                    .HasConstraintName("FK_Pré-Requisição_Locais");

                entity.HasOne(d => d.NºProjetoNavigation)
                    .WithMany(p => p.PréRequisição)
                    .HasForeignKey(d => d.NºProjeto)
                    .HasConstraintName("FK_Pré-Requisição_Projetos");

                entity.HasOne(d => d.TipoRequisiçãoNavigation)
                    .WithMany(p => p.PréRequisição)
                    .HasForeignKey(d => d.TipoRequisição)
                    .HasConstraintName("FK_Pré-Requisição_Tipos Requisições");
            });

            modelBuilder.Entity<PresençasFolhaDeHoras>(entity =>
            {
                entity.HasKey(e => new { e.NºFolhaDeHoras, e.Data });

                entity.ToTable("Presenças Folha de Horas");

                entity.Property(e => e.NºFolhaDeHoras)
                    .HasColumnName("Nº Folha de Horas")
                    .HasMaxLength(20);

                entity.Property(e => e.Data).HasColumnType("date");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataIntTr)
                    .HasColumnName("DataIntTR")
                    .HasColumnType("datetime");

                entity.Property(e => e.Hora1ªEntrada).HasColumnName("Hora 1ª Entrada");

                entity.Property(e => e.Hora1ªSaída).HasColumnName("Hora 1ª Saída");

                entity.Property(e => e.Hora2ªEntrada).HasColumnName("Hora 2ª Entrada");

                entity.Property(e => e.Hora2ªSaída).HasColumnName("Hora 2ª Saída");

                entity.Property(e => e.IntegradoTr).HasColumnName("IntegradoTR");

                entity.Property(e => e.NoEmpregado)
                    .HasColumnName("No Empregado")
                    .HasMaxLength(20);

                entity.Property(e => e.Observacoes).HasMaxLength(200);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.NºFolhaDeHorasNavigation)
                    .WithMany(p => p.PresençasFolhaDeHoras)
                    .HasForeignKey(d => d.NºFolhaDeHoras)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Presenças Folha de Horas_Folhas de Horas");
            });

            modelBuilder.Entity<ProcedimentosCcp>(entity =>
            {
                entity.HasKey(e => e.Nº);

                entity.ToTable("Procedimentos CCP");

                entity.Property(e => e.Nº)
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.AbertoFechadoAoMercado).HasColumnName("Aberto/Fechado ao Mercado");

                entity.Property(e => e.AutorizaçãoAberturaCa).HasColumnName("Autorização Abertura CA");

                entity.Property(e => e.AutorizaçãoAdjudicação).HasColumnName("Autorização Adjudicação");

                entity.Property(e => e.AutorizaçãoAquisiçãoCa).HasColumnName("Autorização Aquisição CA");

                entity.Property(e => e.AutorizaçãoImobCa).HasColumnName("Autorização Imob. CA");

                entity.Property(e => e.CaDataRatificaçãoAbert)
                    .HasColumnName("CA Data Ratificação Abert.")
                    .HasColumnType("date");

                entity.Property(e => e.CaDataRatificaçãoAdjudic)
                    .HasColumnName("CA Data Ratificação Adjudic.")
                    .HasColumnType("date");

                entity.Property(e => e.CaRatificar).HasColumnName("CA Ratificar");

                entity.Property(e => e.CaSuspenso).HasColumnName("CA Suspenso");

                entity.Property(e => e.Comentário).HasColumnType("text");

                entity.Property(e => e.ComentárioAudiênciaPrévia)
                    .HasColumnName("Comentário Audiência Prévia")
                    .HasMaxLength(30);

                entity.Property(e => e.ComentárioEstado)
                    .HasColumnName("Comentário Estado")
                    .HasColumnType("text");

                entity.Property(e => e.ComentárioNotificação)
                    .HasColumnName("Comentário Notificação")
                    .HasMaxLength(30);

                entity.Property(e => e.ComentárioPublicação)
                    .HasColumnName("Comentário Publicação")
                    .HasMaxLength(30);

                entity.Property(e => e.ComentárioRelatórioFinal)
                    .HasColumnName("Comentário Relatório Final")
                    .HasMaxLength(100);

                entity.Property(e => e.ComentárioRelatórioPreliminar)
                    .HasColumnName("Comentário Relatório Preliminar")
                    .HasMaxLength(30);

                entity.Property(e => e.CondiçõesDePagamento)
                    .HasColumnName("Condições de Pagamento")
                    .HasMaxLength(30);

                entity.Property(e => e.CritérioEscolhaProcedimento)
                    .HasColumnName("Critério Escolha Procedimento")
                    .HasMaxLength(20);

                entity.Property(e => e.CritériosAdjudicação)
                    .HasColumnName("Critérios Adjudicação")
                    .HasColumnType("text");

                entity.Property(e => e.CríticoAbertura).HasColumnName("Crítico Abertura");

                entity.Property(e => e.CríticoAdjudicação).HasColumnName("Crítico Adjudicação");

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataAta)
                    .HasColumnName("Data Ata")
                    .HasColumnType("date");

                entity.Property(e => e.DataAudiênciaPrévia)
                    .HasColumnName("Data Audiência Prévia")
                    .HasColumnType("date");

                entity.Property(e => e.DataAutorizaçãoAbertCa)
                    .HasColumnName("Data Autorização Abert. CA")
                    .HasColumnType("date");

                entity.Property(e => e.DataAutorizaçãoAquisiCa)
                    .HasColumnName("Data Autorização Aquisi. CA")
                    .HasColumnType("date");

                entity.Property(e => e.DataAutorizaçãoImobCa)
                    .HasColumnName("Data Autorização Imob. CA")
                    .HasColumnType("date");

                entity.Property(e => e.DataCriação)
                    .HasColumnName("Data Criação")
                    .HasColumnType("date");

                entity.Property(e => e.DataFechoInicial)
                    .HasColumnName("Data Fecho Inicial")
                    .HasColumnType("date");

                entity.Property(e => e.DataFechoPrevista)
                    .HasColumnName("Data Fecho Prevista")
                    .HasColumnType("date");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraEstado)
                    .HasColumnName("Data/Hora Estado")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataNotificação)
                    .HasColumnName("Data Notificação")
                    .HasColumnType("date");

                entity.Property(e => e.DataPublicação)
                    .HasColumnName("Data Publicação")
                    .HasColumnType("date");

                entity.Property(e => e.DataRecolha)
                    .HasColumnName("Data Recolha")
                    .HasColumnType("date");

                entity.Property(e => e.DataRelatórioFinal)
                    .HasColumnName("Data Relatório Final")
                    .HasColumnType("date");

                entity.Property(e => e.DataSistemaAudiênciaPrévia)
                    .HasColumnName("Data Sistema Audiência Prévia")
                    .HasColumnType("date");

                entity.Property(e => e.DataSistemaNotificação)
                    .HasColumnName("Data Sistema Notificação")
                    .HasColumnType("date");

                entity.Property(e => e.DataSistemaPublicação)
                    .HasColumnName("Data Sistema Publicação")
                    .HasColumnType("date");

                entity.Property(e => e.DataSistemaRecolha)
                    .HasColumnName("Data Sistema Recolha")
                    .HasColumnType("date");

                entity.Property(e => e.DataSistemaRelatórioFinal)
                    .HasColumnName("Data Sistema Relatório Final")
                    .HasColumnType("date");

                entity.Property(e => e.DataSistemaValidRelatórioPreliminar)
                    .HasColumnName("Data Sistema Valid. Relatório Preliminar")
                    .HasColumnType("date");

                entity.Property(e => e.DataValidRelatórioPreliminar)
                    .HasColumnName("Data Valid. Relatório Preliminar")
                    .HasColumnType("date");

                entity.Property(e => e.DescAbertoFechadoAoMercado)
                    .HasColumnName("Desc. Aberto/Fechado ao Mercado")
                    .HasColumnType("text");

                entity.Property(e => e.DescEscolhaProcedimento)
                    .HasColumnName("Desc. Escolha Procedimento")
                    .HasColumnType("text");

                entity.Property(e => e.DescFornecedorExclusivo)
                    .HasColumnName("Desc. Fornecedor Exclusivo")
                    .HasColumnType("text");

                entity.Property(e => e.DescPreçoMaisBaixo)
                    .HasColumnName("Desc. Preço Mais Baixo")
                    .HasColumnType("text");

                entity.Property(e => e.DescPropostaEconMaisVantajosa)
                    .HasColumnName("Desc. Proposta Econ. Mais Vantajosa")
                    .HasColumnType("text");

                entity.Property(e => e.DescPropostaVariante)
                    .HasColumnName("Desc. Proposta Variante")
                    .HasColumnType("text");

                entity.Property(e => e.DiferençaEuros).HasColumnName("Diferença Euros");

                entity.Property(e => e.DiferençaPercent).HasColumnName("Diferença Percent.");

                entity.Property(e => e.EstimativaPreço).HasColumnName("Estimativa Preço");

                entity.Property(e => e.Fornecedor).HasMaxLength(30);

                entity.Property(e => e.FornecedorExclusivo).HasColumnName("Fornecedor Exclusivo");

                entity.Property(e => e.FornecedoresSugeridos)
                    .HasColumnName("Fornecedores Sugeridos")
                    .HasMaxLength(60);

                entity.Property(e => e.FundamentaçãoAquisição)
                    .HasColumnName("Fundamentação Aquisição")
                    .HasColumnType("text");

                entity.Property(e => e.GestorProcesso)
                    .HasColumnName("Gestor Processo")
                    .HasMaxLength(30);

                entity.Property(e => e.InformaçãoTécnica)
                    .HasColumnName("Informação Técnica")
                    .HasColumnType("text");

                entity.Property(e => e.Interlocutor).HasMaxLength(100);

                entity.Property(e => e.LocaisEntrega)
                    .HasColumnName("Locais Entrega")
                    .HasMaxLength(30);

                entity.Property(e => e.NomeProcesso)
                    .HasColumnName("Nome Processo")
                    .HasMaxLength(60);

                entity.Property(e => e.NºAta)
                    .HasColumnName("Nº Ata")
                    .HasMaxLength(15);

                entity.Property(e => e.NºDiasAtraso).HasColumnName("Nº Dias Atraso");

                entity.Property(e => e.NºDiasProcesso).HasColumnName("Nº Dias Processo");

                entity.Property(e => e.NãoAdjudicaçãoEEncerramento).HasColumnName("Não Adjudicação e Encerramento");

                entity.Property(e => e.NãoAdjudicaçãoESuspensão).HasColumnName("Não Adjudicação e Suspensão");

                entity.Property(e => e.ObjetoDecisão)
                    .HasColumnName("Objeto Decisão")
                    .HasMaxLength(75);

                entity.Property(e => e.ObjetoDoContrato).HasColumnName("Objeto do Contrato");

                entity.Property(e => e.ObservaçõesAdicionais)
                    .HasColumnName("Observações Adicionais")
                    .HasColumnType("text");

                entity.Property(e => e.PercentExecução).HasColumnName("Percent. Execução");

                entity.Property(e => e.Prazo).HasColumnType("text");

                entity.Property(e => e.PrazoEntrega)
                    .HasColumnName("Prazo Entrega")
                    .HasMaxLength(30);

                entity.Property(e => e.PrazoNotificaçãoDias).HasColumnName("Prazo Notificação Dias");

                entity.Property(e => e.PreçoBase).HasColumnName("Preço Base");

                entity.Property(e => e.PreçoMaisBaixo).HasColumnName("Preço Mais Baixo");

                entity.Property(e => e.PropostaEconMaisVantajosa).HasColumnName("Proposta Econ. Mais Vantajosa");

                entity.Property(e => e.PropostaVariante).HasColumnName("Proposta Variante");

                entity.Property(e => e.ProtocoloContrato)
                    .HasColumnName("Protocolo Contrato")
                    .HasMaxLength(75);

                entity.Property(e => e.PréÁrea).HasColumnName("Pré-Área");

                entity.Property(e => e.RatificarCaAbertura).HasColumnName("Ratificar CA Abertura");

                entity.Property(e => e.RatificarCaAdjudicação).HasColumnName("Ratificar CA Adjudicação");

                entity.Property(e => e.RazãoNecessidade)
                    .HasColumnName("Razão Necessidade")
                    .HasMaxLength(75);

                entity.Property(e => e.RecolhaComentário)
                    .HasColumnName("Recolha Comentário")
                    .HasMaxLength(30);

                entity.Property(e => e.RejeiçãoAberturaCa).HasColumnName("Rejeição Abertura CA");

                entity.Property(e => e.RejeiçãoAquisiçãoCa).HasColumnName("Rejeição Aquisição CA");

                entity.Property(e => e.RejeiçãoImobCa).HasColumnName("Rejeição Imob. CA");

                entity.Property(e => e.SubmeterPréÁrea).HasColumnName("Submeter Pré-Área");

                entity.Property(e => e.TipoProcedimento).HasColumnName("Tipo Procedimento");

                entity.Property(e => e.UtilizadorAudiênciaPrévia)
                    .HasColumnName("Utilizador Audiência Prévia")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorEstado)
                    .HasColumnName("Utilizador Estado")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorNotificação)
                    .HasColumnName("Utilizador Notificação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorPublicação)
                    .HasColumnName("Utilizador Publicação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorRecolha)
                    .HasColumnName("Utilizador Recolha")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorRelatórioFinal)
                    .HasColumnName("Utilizador Relatório Final")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorValidRelatórioPreliminar)
                    .HasColumnName("Utilizador Valid. Relatório Preliminar")
                    .HasMaxLength(50);

                entity.Property(e => e.ValorAdjudicaçãoAnteriro).HasColumnName("Valor Adjudicação Anteriro");

                entity.Property(e => e.ValorAdjudicaçãoAtual).HasColumnName("Valor Adjudicação Atual");

                entity.Property(e => e.ValorDecisãoContratar).HasColumnName("Valor Decisão Contratar");

                entity.Property(e => e.ValorPreçoBase).HasColumnName("Valor Preço Base");

                entity.Property(e => e.WorkflowFinanceiros).HasColumnName("Workflow Financeiros");

                entity.Property(e => e.WorkflowFinanceirosConfirm).HasColumnName("Workflow Financeiros Confirm.");

                entity.Property(e => e.WorkflowJurídicos).HasColumnName("Workflow Jurídicos");

                entity.Property(e => e.WorkflowJurídicosConfirm).HasColumnName("Workflow Jurídicos Confirm.");
                // zpgm.ALT_CCP_#001.y2019.b
                entity.HasOne(d => d.TipoNavigation)
                    .WithMany(p => p.ProcedimentosCcp)
                    .HasForeignKey(d => d.Tipo)
                    .HasConstraintName("FK_ProcedimentosCCP_TipoProcedimentoCcp");

                entity.HasOne(d => d.FundamentoLegalTipoProcedimentoCcp)
                    .WithMany(p => p.ProcedimentosCcp)
                    .HasForeignKey(d => new { d.Tipo, d.FundamentoLegalTipo })
                    .HasConstraintName("FK_ProcedimentosCCP_FundamentoLegalTipoProcedimentoCcp");
                // zpgm.ALT_CCP_#001.y2019.e
            });

            #region zpgm.ALT_CCP_#001.y2019
            modelBuilder.Entity<TipoProcedimentoCcp>(entity =>
            {
                entity.HasKey(e => e.IdTipo);

                entity.HasIndex(e => e.Abreviatura)
                    .HasName("IX_Abreviatura")
                    .IsUnique();

                entity.Property(e => e.IdTipo).ValueGeneratedNever();

                entity.Property(e => e.Abreviatura)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.DataCriacao).HasColumnType("datetime");

                entity.Property(e => e.DataModificacao).HasColumnType("datetime");

                entity.Property(e => e.DescricaoTipo)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.UtilizadorCriacao).HasMaxLength(100);

                entity.Property(e => e.UtilizadorModificacao).HasMaxLength(100);
            });

            modelBuilder.Entity<FundamentoLegalTipoProcedimentoCcp>(entity =>
            {
                entity.HasKey(e => new { e.IdTipo, e.IdFundamento });

                entity.Property(e => e.DataCriacao).HasColumnType("datetime");

                entity.Property(e => e.DataModificacao).HasColumnType("datetime");

                entity.Property(e => e.DescricaoFundamento)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.UtilizadorCriacao).HasMaxLength(100);

                entity.Property(e => e.UtilizadorModificacao).HasMaxLength(100);

                entity.HasOne(d => d.IdTipoNavigation)
                    .WithMany(p => p.FundamentoLegalTipoProcedimentoCcp)
                    .HasForeignKey(d => d.IdTipo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FundamentoLegalTipoProcedimentoCcp_TipoProcedimentoCcp");
            });

            modelBuilder.Entity<LoteProcedimentoCcp>(entity =>
            {
                entity.HasKey(e => new { e.NoProcedimento, e.IdLote });

                entity.Property(e => e.NoProcedimento).HasMaxLength(10);

                entity.Property(e => e.DataAdjudicacao).HasColumnType("datetime");

                entity.Property(e => e.DataAutorizacaoAdjudicacao).HasColumnType("datetime");
                
                entity.Property(e => e.DataCriacao).HasColumnType("datetime");

                entity.Property(e => e.DataModificacao).HasColumnType("datetime");

                entity.Property(e => e.DataNotificacao).HasColumnType("datetime");

                entity.Property(e => e.HoraAdjudicacao).HasColumnType("datetime");

                entity.Property(e => e.HoraAutorizacaoAdjudicacao).HasColumnType("datetime");

                entity.Property(e => e.HoraNotificacao).HasColumnType("datetime");

                entity.Property(e => e.DataNaoAdjudicacao).HasColumnType("datetime");

                entity.Property(e => e.HoraNaoAdjudicacao).HasColumnType("datetime");

                entity.Property(e => e.UtilizadorAdjudicacao).HasMaxLength(100);

                entity.Property(e => e.UtilizadorAutorizacaoAdjudicacao).HasMaxLength(100);

                entity.Property(e => e.UtilizadorCriacao).HasMaxLength(100);

                entity.Property(e => e.UtilizadorModificacao).HasMaxLength(100);

                entity.Property(e => e.UtilizadorNotificacao).HasMaxLength(100);

                entity.Property(e => e.UtilizadorNaoAdjudicacao).HasMaxLength(100);

                entity.Property(e => e.ValorAdjudicacao).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ValorEstimado).HasColumnType("decimal(18, 2)");

                entity.HasOne(e => e.NoProcedimentoNavigation)
                    .WithMany(p => p.LoteProcedimentoCcp)
                    .HasForeignKey(d => d.NoProcedimento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LoteProcedimentoCcp_ProcedimentosCCP");
            });
            #endregion

            modelBuilder.Entity<ProcedimentosDeConfeção>(entity =>
            {
                entity.HasKey(e => new { e.NºPrato, e.CódigoAção });

                entity.ToTable("Procedimentos de Confeção");

                entity.Property(e => e.NºPrato)
                    .HasColumnName("Nº Prato")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoAção).HasColumnName("Código Ação");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(150);

                entity.Property(e => e.NºOrdem).HasColumnName("Nº Ordem");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.CódigoAçãoNavigation)
                    .WithMany(p => p.ProcedimentosDeConfeção)
                    .HasForeignKey(d => d.CódigoAção)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Procedimentos de Confeção_Ações de Confeção");

                entity.HasOne(d => d.NºPratoNavigation)
                    .WithMany(p => p.ProcedimentosDeConfeção)
                    .HasForeignKey(d => d.NºPrato)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Procedimentos de Confeção_Fichas Técnicas Pratos");
            });

            modelBuilder.Entity<ProcessosDisciplinaresInquérito>(entity =>
            {
                entity.HasKey(e => new { e.Tipo, e.AnoProcesso, e.NºDoProcesso });

                entity.ToTable("Processos Disciplinares/Inquérito");

                entity.Property(e => e.AnoProcesso).HasColumnName("Ano Processo");

                entity.Property(e => e.NºDoProcesso)
                    .HasColumnName("Nº do Processo")
                    .HasMaxLength(20);

                entity.Property(e => e.DataArquivo)
                    .HasColumnName("Data Arquivo")
                    .HasColumnType("date");

                entity.Property(e => e.DataCriaçãoModificação)
                    .HasColumnName("Data Criação/Modificação")
                    .HasColumnType("date");

                entity.Property(e => e.DataDocumento)
                    .HasColumnName("Data Documento")
                    .HasColumnType("date");

                entity.Property(e => e.DataEntrada)
                    .HasColumnName("Data Entrada")
                    .HasColumnType("date");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DocumentoAssociado)
                    .HasColumnName("Documento Associado")
                    .HasMaxLength(20);

                entity.Property(e => e.EntidadeRemetente)
                    .HasColumnName("Entidade Remetente")
                    .HasMaxLength(40);

                entity.Property(e => e.EstadoDoProcesso).HasColumnName("Estado do Processo");

                entity.Property(e => e.Infração).HasMaxLength(80);

                entity.Property(e => e.Interessado).HasMaxLength(40);

                entity.Property(e => e.Nome).HasMaxLength(50);

                entity.Property(e => e.NºDeProcesso)
                    .HasColumnName("Nº de Processo")
                    .HasMaxLength(20);

                entity.Property(e => e.NºInstrutor).HasColumnName("Nº Instrutor");

                entity.Property(e => e.Observações).HasColumnType("text");

                entity.Property(e => e.Sanção).HasMaxLength(80);

                entity.Property(e => e.Serviço).HasMaxLength(20);

                entity.Property(e => e.TipoDeProcesso).HasColumnName("Tipo de Processo");

                entity.Property(e => e.Utilizador).HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.NºInstrutorNavigation)
                    .WithMany(p => p.ProcessosDisciplinaresInquérito)
                    .HasForeignKey(d => d.NºInstrutor)
                    .HasConstraintName("FK_Processos Disciplinares/Inquérito_Instrutores");
            });

            modelBuilder.Entity<ProjectosAutorizados>(entity =>
            {
                entity.HasKey(e => new { e.CodProjeto, e.GrupoFactura });

                entity.Property(e => e.CodProjeto)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CodAreaFuncional)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CodCentroResponsabilidade)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CodCliente)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NomeCliente)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorAutorizado)
                    .HasColumnName("ValorAutorizado")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.CodContrato)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CodEnderecoEnvio)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CodMetodoPagamento)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CodRegiao)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CodTermosPagamento)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DataAutorizacao).HasColumnType("datetime");

                entity.Property(e => e.DataPedido).HasColumnType("datetime");

                entity.Property(e => e.DataPrestacaoServico).HasColumnType("datetime");

                entity.Property(e => e.DataServPrestado)
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.Descricao)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DescricaoGrupo)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Diversos)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.GrupoContabilisticoObra)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.GrupoContabilisticoProjeto)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NumCompromisso)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumSerie)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Observacoes)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Observacoes1)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.PedidoCliente)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.SituacoesPendentes)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.Utilizador)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Projetos>(entity =>
            {
                entity.HasKey(e => e.NºProjeto);

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.CategoriaProjeto).HasColumnName("Categoria Projeto");

                entity.Property(e => e.ChefeProjeto)
                    .HasColumnName("Chefe Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.CódEndereçoEnvio)
                    .HasColumnName("Cód. Endereço Envio")
                    .HasMaxLength(10);

                entity.Property(e => e.CódObjetoServiço).HasColumnName("Cód. Objeto Serviço");

                entity.Property(e => e.CódTipoProjeto).HasColumnName("Cód. Tipo Projeto");

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.Data).HasColumnType("date");

                entity.Property(e => e.DataDoPedido)
                    .HasColumnName("Data do Pedido")
                    .HasColumnType("date");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(100);

                entity.Property(e => e.DescriçãoDetalhada)
                    .HasColumnName("Descrição Detalhada")
                    .HasColumnType("text");

                entity.Property(e => e.EnvioAContato)
                    .HasColumnName("Envio-a Contato")
                    .HasMaxLength(50);

                entity.Property(e => e.EnvioACódPostal)
                    .HasColumnName("Envio-a Cód. Postal")
                    .HasMaxLength(20);

                entity.Property(e => e.EnvioAEndereço)
                    .HasColumnName("Envio-a Endereço")
                    .HasMaxLength(100);

                entity.Property(e => e.EnvioALocalidade)
                    .HasColumnName("Envio-a Localidade")
                    .HasMaxLength(30);

                entity.Property(e => e.EnvioANome)
                    .HasColumnName("Envio-a Nome")
                    .HasMaxLength(100);

                entity.Property(e => e.GrupoContabObra)
                    .HasColumnName("Grupo Contab. Obra")
                    .HasMaxLength(10);

                entity.Property(e => e.NossaProposta)
                    .HasColumnName("Nossa Proposta")
                    .HasMaxLength(50);

                entity.Property(e => e.NºCliente)
                    .HasColumnName("Nº Cliente")
                    .HasMaxLength(20);

                entity.Property(e => e.NºCompromisso)
                    .HasColumnName("Nº Compromisso")
                    .HasMaxLength(50);

                entity.Property(e => e.NºContrato)
                    .HasColumnName("Nº Contrato")
                    .HasMaxLength(20);

                entity.Property(e => e.NºContratoOrçamento)
                    .HasColumnName("Nº Contrato Orçamento")
                    .HasMaxLength(20);

                entity.Property(e => e.PedidoDoCliente)
                    .HasColumnName("Pedido do Cliente")
                    .HasMaxLength(20);

                entity.Property(e => e.ProjetoInterno).HasColumnName("Projeto Interno");

                entity.Property(e => e.ResponsávelProjeto)
                    .HasColumnName("Responsável Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.TipoGrupoContabOmProjeto).HasColumnName("Tipo Grupo Contab. OM Projeto");

                entity.Property(e => e.TipoGrupoContabProjeto).HasColumnName("Tipo Grupo Contab. Projeto");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.ValidadeDoPedido)
                    .HasColumnName("Validade do Pedido")
                    .HasColumnType("date");

                entity.Property(e => e.FaturaPrecosIvaIncluido).HasColumnName("FaturaPrecosIvaIncluido");

                entity.Property(e => e.FechoAutomatico).HasColumnName("FechoAutomatico");

                entity.HasOne(d => d.CódTipoProjetoNavigation)
                    .WithMany(p => p.Projetos)
                    .HasForeignKey(d => d.CódTipoProjeto)
                    .HasConstraintName("FK_Projetos_Tipo de Projeto");

                entity.HasOne(d => d.TipoGrupoContabOmProjetoNavigation)
                    .WithMany(p => p.Projetos)
                    .HasForeignKey(d => d.TipoGrupoContabOmProjeto)
                    .HasConstraintName("FK_Projetos_Tipos Grupo Contab. OM Projeto");

                entity.HasOne(d => d.TipoGrupoContabProjetoNavigation)
                    .WithMany(p => p.Projetos)
                    .HasForeignKey(d => d.TipoGrupoContabProjeto)
                    .HasConstraintName("FK_Projetos_Tipos Grupo Contab. Projeto");
            });

            modelBuilder.Entity<ProjetosFaturação>(entity =>
            {
                entity.HasKey(e => new { e.NºUnidadeProdutiva, e.NºProjeto });

                entity.ToTable("Projetos Faturação");

                entity.Property(e => e.NºUnidadeProdutiva).HasColumnName("Nº Unidade Produtiva");

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.NºProjetoNavigation)
                    .WithMany(p => p.ProjetosFaturação)
                    .HasForeignKey(d => d.NºProjeto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Projetos Faturação_Projetos");

                entity.HasOne(d => d.NºUnidadeProdutivaNavigation)
                    .WithMany(p => p.ProjetosFaturação)
                    .HasForeignKey(d => d.NºUnidadeProdutiva)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Projetos Faturação_Unidades Produtivas");
            });

            modelBuilder.Entity<RececaoFaturacao>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.AreaPendente).HasMaxLength(50);

                entity.Property(e => e.AreaPendente2).HasMaxLength(50);

                entity.Property(e => e.AreaUltimaInteracao).HasMaxLength(50);

                entity.Property(e => e.CodAreaFuncional).HasMaxLength(20);

                entity.Property(e => e.CodCentroResponsabilidade).HasMaxLength(20);

                entity.Property(e => e.CodFornecedor).HasMaxLength(20);

                entity.Property(e => e.CodLocalizacao).HasColumnType("nchar(10)");

                entity.Property(e => e.CodRegiao).HasMaxLength(20);

                entity.Property(e => e.CriadoPor).HasMaxLength(50);

                entity.Property(e => e.DataCriacao).HasColumnType("datetime");

                entity.Property(e => e.DataDocFornecedor).HasColumnType("datetime");

                entity.Property(e => e.DataModificacao).HasColumnType("datetime");

                entity.Property(e => e.DataPassaPendente).HasColumnType("datetime");

                entity.Property(e => e.DataRececao).HasColumnType("datetime");

                entity.Property(e => e.DataResolucao).HasColumnType("datetime");

                entity.Property(e => e.DataUltimaInteracao).HasColumnType("datetime");

                entity.Property(e => e.Descricao).HasMaxLength(100);

                entity.Property(e => e.DescricaoProblema).HasMaxLength(250);

                entity.Property(e => e.Destinatario).HasMaxLength(50);

                entity.Property(e => e.DocumentoCriadoEm).HasColumnType("datetime");

                entity.Property(e => e.DocumentoCriadoPor).HasMaxLength(50);

                entity.Property(e => e.Local).HasColumnType("nchar(10)");

                entity.Property(e => e.ModificadoPor).HasMaxLength(50);

                entity.Property(e => e.NumAcordoFornecedor).HasMaxLength(20);

                entity.Property(e => e.NumDocFornecedor).HasMaxLength(20);

                entity.Property(e => e.NumDocRegistado).HasMaxLength(20);

                entity.Property(e => e.NumEncomenda).HasMaxLength(20);

                entity.Property(e => e.NumEncomendaManual).HasMaxLength(20);

                entity.Property(e => e.TipoProblema).HasMaxLength(20);

                entity.Property(e => e.UserUltimaInteracao).HasMaxLength(50);
            });

            modelBuilder.Entity<RececaoFaturacaoWorkflow>(entity =>
            {
                entity.Property(e => e.Area).HasMaxLength(20);

                entity.Property(e => e.AreaWorkflow).HasMaxLength(50);

                entity.Property(e => e.CodDestino).HasMaxLength(20);

                entity.Property(e => e.CodProblema).HasMaxLength(10);

                entity.Property(e => e.CodTipoProblema).HasMaxLength(20);

                entity.Property(e => e.Comentario).HasMaxLength(250);

                entity.Property(e => e.CriadoPor).HasMaxLength(50);

                entity.Property(e => e.Data).HasColumnType("datetime");

                entity.Property(e => e.DataCriacao).HasColumnType("datetime");

                entity.Property(e => e.DataModificacao).HasColumnType("datetime");

                entity.Property(e => e.Descricao).HasMaxLength(100);

                entity.Property(e => e.Destinatario).HasMaxLength(50);

                entity.Property(e => e.EnderecoEnvio).HasMaxLength(100);

                entity.Property(e => e.EnderecoFornecedor).HasMaxLength(100);

                entity.Property(e => e.IdRecFaturacao).HasMaxLength(20);

                entity.Property(e => e.ModificadoPor).HasMaxLength(50);

                entity.Property(e => e.Utilizador).HasMaxLength(50);

                entity.HasOne(d => d.IdRecFaturacaoNavigation)
                    .WithMany(p => p.RececaoFaturacaoWorkflow)
                    .HasForeignKey(d => d.IdRecFaturacao)
                    .HasConstraintName("FK_RececaoFaturacaoWorkflow_RececaoFaturacao");

                entity.HasOne(d => d.Cod)
                    .WithMany(p => p.RececaoFaturacaoWorkflow)
                    .HasForeignKey(d => new { d.CodProblema, d.CodTipoProblema })
                    .HasConstraintName("FK_RececaoFaturacaoWorkflow_RecFacturasProblemas");
            });

            modelBuilder.Entity<RececaoFaturacaoWorkflowAnexo>(entity =>
            {
                entity.Property(e => e.Caminho).HasMaxLength(200);

                entity.Property(e => e.Comentario).HasMaxLength(50);
            });

            modelBuilder.Entity<RecFacturasProblemas>(entity =>
            {
                entity.HasKey(e => new { e.Codigo, e.Tipo });

                entity.Property(e => e.Codigo).HasMaxLength(10);

                entity.Property(e => e.Tipo).HasMaxLength(20);

                entity.Property(e => e.Descricao).HasMaxLength(100);

                entity.Property(e => e.EnvioAreas).HasMaxLength(60);
            });

            modelBuilder.Entity<RecFaturacaoConfigDestinatarios>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.Property(e => e.Codigo)
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.CodArea).HasMaxLength(30);

                entity.Property(e => e.CodCentroResponsabilidade).HasMaxLength(20);

                entity.Property(e => e.Destinatario).HasMaxLength(50);

                entity.Property(e => e.Notas).HasMaxLength(250);
            });

            modelBuilder.Entity<RegistoDeAtas>(entity =>
            {
                entity.HasKey(e => new { e.NºProcedimento, e.NºAta });

                entity.ToTable("Registo de Atas");

                entity.Property(e => e.NºProcedimento)
                    .HasColumnName("Nº Procedimento")
                    .HasMaxLength(10);

                entity.Property(e => e.NºAta)
                    .HasColumnName("Nº Ata")
                    .HasMaxLength(30);

                entity.Property(e => e.DataDaAta)
                    .HasColumnName("Data da Ata")
                    .HasColumnType("date");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Observações).HasColumnType("text");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.NºProcedimentoNavigation)
                    .WithMany(p => p.RegistoDeAtas)
                    .HasForeignKey(d => d.NºProcedimento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Registo de Atas_Procedimentos CCP");
            });

            modelBuilder.Entity<RegistoDePropostas>(entity =>
            {
                entity.HasKey(e => e.NumLinha);

                entity.ToTable("Registo_De_Propostas");

                entity.Property(e => e.NumLinha).HasColumnName("Num_Linha");

                entity.Property(e => e.Alternativa).HasMaxLength(20);

                entity.Property(e => e.CodActividade)
                    .HasColumnName("Cod_Actividade")
                    .HasMaxLength(20);

                entity.Property(e => e.CodAreaFuncional)
                    .HasColumnName("Cod_Area_Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.CodCentroResponsabilidade)
                    .HasColumnName("Cod_Centro_Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CodLocalizacao)
                    .HasColumnName("Cod_Localizacao")
                    .HasMaxLength(20);

                entity.Property(e => e.CodProduto)
                    .HasColumnName("Cod_Produto")
                    .HasMaxLength(20);

                entity.Property(e => e.CodRegiao)
                    .HasColumnName("Cod_Regiao")
                    .HasMaxLength(20);

                entity.Property(e => e.Descricao).HasMaxLength(100);

                entity.Property(e => e.Descricao2).HasMaxLength(100);

                entity.Property(e => e.Fornecedor1Code)
                    .HasColumnName("Fornecedor1_Code")
                    .HasMaxLength(20);

                entity.Property(e => e.Fornecedor1Nome)
                    .HasColumnName("Fornecedor1_Nome")
                    .HasMaxLength(50);

                entity.Property(e => e.Fornecedor1Preco).HasColumnName("Fornecedor1_Preco");

                entity.Property(e => e.Fornecedor1Select).HasColumnName("Fornecedor1_Select");

                entity.Property(e => e.Fornecedor2Code)
                    .HasColumnName("Fornecedor2_Code")
                    .HasMaxLength(20);

                entity.Property(e => e.Fornecedor2Nome)
                    .HasColumnName("Fornecedor2_Nome")
                    .HasMaxLength(50);

                entity.Property(e => e.Fornecedor2Preco).HasColumnName("Fornecedor2_Preco");

                entity.Property(e => e.Fornecedor2Select).HasColumnName("Fornecedor2_Select");

                entity.Property(e => e.Fornecedor3Code)
                    .HasColumnName("Fornecedor3_Code")
                    .HasMaxLength(20);

                entity.Property(e => e.Fornecedor3Nome)
                    .HasColumnName("Fornecedor3_Nome")
                    .HasMaxLength(50);

                entity.Property(e => e.Fornecedor3Preco).HasColumnName("Fornecedor3_Preco");

                entity.Property(e => e.Fornecedor3Select).HasColumnName("Fornecedor3_Select");

                entity.Property(e => e.Fornecedor4Code)
                    .HasColumnName("Fornecedor4_Code")
                    .HasMaxLength(20);

                entity.Property(e => e.Fornecedor4Nome)
                    .HasColumnName("Fornecedor4_Nome")
                    .HasMaxLength(50);

                entity.Property(e => e.Fornecedor4Preco).HasColumnName("Fornecedor4_Preco");

                entity.Property(e => e.Fornecedor4Select).HasColumnName("Fornecedor4_Select");

                entity.Property(e => e.Fornecedor5Code)
                    .HasColumnName("Fornecedor5_Code")
                    .HasMaxLength(20);

                entity.Property(e => e.Fornecedor5Nome)
                    .HasColumnName("Fornecedor5_Nome")
                    .HasMaxLength(50);

                entity.Property(e => e.Fornecedor5Preco).HasColumnName("Fornecedor5_Preco");

                entity.Property(e => e.Fornecedor5Select).HasColumnName("Fornecedor5_Select");

                entity.Property(e => e.Fornecedor6Code)
                    .HasColumnName("Fornecedor6_Code")
                    .HasMaxLength(20);

                entity.Property(e => e.Fornecedor6Nome)
                    .HasColumnName("Fornecedor6_Nome")
                    .HasMaxLength(50);

                entity.Property(e => e.Fornecedor6Preco).HasColumnName("Fornecedor6_Preco");

                entity.Property(e => e.Fornecedor6Select).HasColumnName("Fornecedor6_Select");

                entity.Property(e => e.NumConsultaMercado)
                    .HasColumnName("Num_Consulta_Mercado")
                    .HasMaxLength(20);

                entity.Property(e => e.NumLinhaConsultaMercado).HasColumnName("Num_Linha_Consulta_Mercado");

                entity.Property(e => e.NumProjecto)
                    .HasColumnName("Num_Projecto")
                    .HasMaxLength(20);

                entity.Property(e => e.VatbusinessPostingGroup1)
                    .HasColumnName("VATBusinessPostingGroup1")
                    .HasMaxLength(10);

                entity.Property(e => e.VatbusinessPostingGroup2)
                    .HasColumnName("VATBusinessPostingGroup2")
                    .HasMaxLength(10);

                entity.Property(e => e.VatbusinessPostingGroup3)
                    .HasColumnName("VATBusinessPostingGroup3")
                    .HasMaxLength(10);

                entity.Property(e => e.VatbusinessPostingGroup4)
                    .HasColumnName("VATBusinessPostingGroup4")
                    .HasMaxLength(10);

                entity.Property(e => e.VatbusinessPostingGroup5)
                    .HasColumnName("VATBusinessPostingGroup5")
                    .HasMaxLength(10);

                entity.Property(e => e.VatbusinessPostingGroup6)
                    .HasColumnName("VATBusinessPostingGroup6")
                    .HasMaxLength(10);

                entity.Property(e => e.VatproductPostingGroup)
                    .HasColumnName("VATProductPostingGroup")
                    .HasMaxLength(10);

                entity.HasOne(d => d.NumConsultaMercadoNavigation)
                    .WithMany(p => p.RegistoDePropostas)
                    .HasForeignKey(d => d.NumConsultaMercado)
                    .HasConstraintName("FK_Registo_De_Propostas_Consulta_Mercado");
            });

            modelBuilder.Entity<Requisição>(entity =>
            {
                entity.HasKey(e => e.NºRequisição);

                entity.Property(e => e.NºRequisição)
                    .HasColumnName("Nº Requisição")
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.Aprovadores).HasMaxLength(500);

                entity.Property(e => e.CabimentoOrçamental).HasColumnName("Cabimento Orçamental");

                entity.Property(e => e.CompraADinheiro).HasColumnName("Compra a Dinheiro");

                entity.Property(e => e.ContatoEntrega)
                    .HasColumnName("Contato Entrega")
                    .HasMaxLength(50);

                entity.Property(e => e.ContatoRecolha)
                    .HasColumnName("Contato Recolha")
                    .HasMaxLength(50);

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoLocalEntrega).HasColumnName("Código Local Entrega");

                entity.Property(e => e.CódigoLocalRecolha).HasColumnName("Código Local Recolha");

                entity.Property(e => e.CódigoLocalização)
                    .HasColumnName("Código Localização")
                    .HasMaxLength(10);

                entity.Property(e => e.CódigoPostalEntrega)
                    .HasColumnName("Código Postal Entrega")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoPostalRecolha)
                    .HasColumnName("Código Postal Recolha")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataAprovação)
                    .HasColumnName("Data Aprovação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataEntregaArmazém)
                    .HasColumnName("Data Entrega Armazém")
                    .HasColumnType("date");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataMercadoLocal)
                    .HasColumnName("Data Mercado Local")
                    .HasColumnType("date");

                entity.Property(e => e.DataReceção)
                    .HasColumnName("Data Receção")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataRequisição)
                    .HasColumnName("Data Requisição")
                    .HasColumnType("date");

                entity.Property(e => e.DataValidação)
                    .HasColumnName("Data Validação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Emm).HasColumnName("EMM");

                entity.Property(e => e.JáExecutado).HasColumnName("Já Executado");

                entity.Property(e => e.LocalDeRecolha).HasColumnName("Local de Recolha");

                entity.Property(e => e.LocalEntrega).HasColumnName("Local Entrega");

                entity.Property(e => e.LocalidadeEntrega)
                    .HasColumnName("Localidade Entrega")
                    .HasMaxLength(50);

                entity.Property(e => e.LocalidadeRecolha)
                    .HasColumnName("Localidade Recolha")
                    .HasMaxLength(50);

                entity.Property(e => e.MercadoLocal).HasColumnName("Mercado Local");

                entity.Property(e => e.ModeloDeRequisição).HasColumnName("Modelo de Requisição");

                entity.Property(e => e.Morada2Entrega)
                    .HasColumnName("Morada 2 Entrega")
                    .HasMaxLength(100);

                entity.Property(e => e.Morada2Recolha)
                    .HasColumnName("Morada 2 Recolha")
                    .HasMaxLength(100);

                entity.Property(e => e.MoradaEntrega)
                    .HasColumnName("Morada Entrega")
                    .HasMaxLength(100);

                entity.Property(e => e.MoradaRecolha)
                    .HasColumnName("Morada Recolha")
                    .HasMaxLength(100);

                entity.Property(e => e.NºConsultaMercado)
                    .HasColumnName("Nº Consulta Mercado")
                    .HasMaxLength(20);

                entity.Property(e => e.NºEncomenda)
                    .HasColumnName("Nº Encomenda")
                    .HasMaxLength(20);

                entity.Property(e => e.NºFatura)
                    .HasColumnName("Nº Fatura")
                    .HasMaxLength(20);

                entity.Property(e => e.NoDocumento)
                    .HasColumnName("NoDocumento")
                    .HasMaxLength(50);

                entity.Property(e => e.NºFuncionário)
                    .HasColumnName("Nº Funcionário")
                    .HasMaxLength(20);

                entity.Property(e => e.NºProcedimentoCcp)
                    .HasColumnName("Nº Procedimento CCP")
                    .HasMaxLength(20);

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.NºRequisiçãoReclamada)
                    .HasColumnName("Nº Requisição Reclamada")
                    .HasMaxLength(20);

                entity.Property(e => e.Observações).HasColumnType("text");

                entity.Property(e => e.RejeicaoMotivo).HasColumnType("text");

                entity.Property(e => e.PrecoIvaincluido).HasColumnName("PrecoIVAIncluido");

                entity.Property(e => e.RegiãoMercadoLocal)
                    .HasColumnName("Região Mercado Local")
                    .HasMaxLength(20);

                entity.Property(e => e.ReparaçãoComGarantia).HasColumnName("Reparação com Garantia");

                entity.Property(e => e.ReposiçãoDeStock).HasColumnName("Reposição de Stock");

                entity.Property(e => e.RequisiçãoDetergentes).HasColumnName("Requisição Detergentes");

                entity.Property(e => e.RequisiçãoNutrição).HasColumnName("Requisição Nutrição");

                entity.Property(e => e.ResponsávelAprovação)
                    .HasColumnName("Responsável Aprovação")
                    .HasMaxLength(50);

                entity.Property(e => e.ResponsávelCriação)
                    .HasColumnName("Responsável Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.ResponsávelReceção)
                    .HasColumnName("Responsável Receção")
                    .HasMaxLength(50);

                entity.Property(e => e.ResponsávelReceçãoReceção)
                    .HasColumnName("Responsável Receção Receção")
                    .HasMaxLength(50);

                entity.Property(e => e.ResponsávelReceçãoRecolha)
                    .HasColumnName("Responsável Receção Recolha")
                    .HasMaxLength(50);

                entity.Property(e => e.ResponsávelValidação)
                    .HasColumnName("Responsável Validação")
                    .HasMaxLength(50);

                entity.Property(e => e.UnidadeProdutivaAlimentação)
                    .HasColumnName("Unidade Produtiva Alimentação")
                    .HasMaxLength(20);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.ValorEstimado)
                    .HasColumnName("Valor Estimado")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.TipoAlteracaoSISLOG)
                    .HasColumnName("TipoAlteracaoSISLOG");

                entity.Property(e => e.DataAlteracaoSISLOG)
                    .HasColumnName("DataAlteracaoSISLOG")
                    .HasColumnType("datetime");

                entity.Property(e => e.EnviarSISLOG)
                    .HasColumnName("EnviarSISLOG");

                entity.Property(e => e.SISLOG)
                    .HasColumnName("SISLOG");

                entity.Property(e => e.DataEnvioSISLOG)
                    .HasColumnName("DataEnvioSISLOG")
                    .HasColumnType("datetime");

                entity.Property(e => e.Viatura).HasMaxLength(20);

                entity.HasOne(d => d.NºProjetoNavigation)
                    .WithMany(p => p.Requisição)
                    .HasForeignKey(d => d.NºProjeto)
                    .HasConstraintName("FK_Requisição_Projetos");

                entity.HasOne(d => d.ViaturaNavigation)
                    .WithMany(p => p.Requisição)
                    .HasForeignKey(d => d.Viatura)
                    .HasConstraintName("FK_Requisição_Viaturas");
            });

            modelBuilder.Entity<RequisiçãoHist>(entity =>
            {
                entity.HasKey(e => e.NºRequisição);

                entity.ToTable("Requisição Hist");

                entity.Property(e => e.NºRequisição)
                    .HasColumnName("Nº Requisição")
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.Aprovadores).HasMaxLength(500);

                entity.Property(e => e.CabimentoOrçamental).HasColumnName("Cabimento Orçamental");

                entity.Property(e => e.CompraADinheiro).HasColumnName("Compra a Dinheiro");

                entity.Property(e => e.ContatoEntrega)
                    .HasColumnName("Contato Entrega")
                    .HasMaxLength(50);

                entity.Property(e => e.ContatoRecolha)
                    .HasColumnName("Contato Recolha")
                    .HasMaxLength(50);

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoLocalEntrega).HasColumnName("Código Local Entrega");

                entity.Property(e => e.CódigoLocalRecolha).HasColumnName("Código Local Recolha");

                entity.Property(e => e.CódigoLocalização)
                    .HasColumnName("Código Localização")
                    .HasMaxLength(10);

                entity.Property(e => e.CódigoPostalEntrega)
                    .HasColumnName("Código Postal Entrega")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoPostalRecolha)
                    .HasColumnName("Código Postal Recolha")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataAprovação)
                    .HasColumnName("Data Aprovação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataEntregaArmazém)
                    .HasColumnName("Data Entrega Armazém")
                    .HasColumnType("date");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataMercadoLocal)
                    .HasColumnName("Data Mercado Local")
                    .HasColumnType("date");

                entity.Property(e => e.DataReceção)
                    .HasColumnName("Data Receção")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataRequisição)
                    .HasColumnName("Data Requisição")
                    .HasColumnType("date");

                entity.Property(e => e.DataValidação)
                    .HasColumnName("Data Validação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Emm).HasColumnName("EMM");

                entity.Property(e => e.JáExecutado).HasColumnName("Já Executado");

                entity.Property(e => e.LocalDeRecolha).HasColumnName("Local de Recolha");

                entity.Property(e => e.LocalEntrega).HasColumnName("Local Entrega");

                entity.Property(e => e.LocalidadeEntrega)
                    .HasColumnName("Localidade Entrega")
                    .HasMaxLength(50);

                entity.Property(e => e.LocalidadeRecolha)
                    .HasColumnName("Localidade Recolha")
                    .HasMaxLength(50);

                entity.Property(e => e.MercadoLocal).HasColumnName("Mercado Local");

                entity.Property(e => e.ModeloDeRequisição).HasColumnName("Modelo de Requisição");

                entity.Property(e => e.Morada2Entrega)
                    .HasColumnName("Morada 2 Entrega")
                    .HasMaxLength(100);

                entity.Property(e => e.Morada2Recolha)
                    .HasColumnName("Morada 2 Recolha")
                    .HasMaxLength(100);

                entity.Property(e => e.MoradaEntrega)
                    .HasColumnName("Morada Entrega")
                    .HasMaxLength(100);

                entity.Property(e => e.MoradaRecolha)
                    .HasColumnName("Morada Recolha")
                    .HasMaxLength(100);

                entity.Property(e => e.NºConsultaMercado)
                    .HasColumnName("Nº Consulta Mercado")
                    .HasMaxLength(20);

                entity.Property(e => e.NºEncomenda)
                    .HasColumnName("Nº Encomenda")
                    .HasMaxLength(20);

                entity.Property(e => e.NºFatura)
                    .HasColumnName("Nº Fatura")
                    .HasMaxLength(20);

                entity.Property(e => e.NºFuncionário)
                    .HasColumnName("Nº Funcionário")
                    .HasMaxLength(20);

                entity.Property(e => e.NºProcedimentoCcp)
                    .HasColumnName("Nº Procedimento CCP")
                    .HasMaxLength(20);

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.NºRequisiçãoReclamada)
                    .HasColumnName("Nº Requisição Reclamada")
                    .HasMaxLength(20);

                entity.Property(e => e.Observações).HasColumnType("text");

                entity.Property(e => e.RejeicaoMotivo).HasColumnType("text");

                entity.Property(e => e.PrecoIvaincluido).HasColumnName("PrecoIVAIncluido");

                entity.Property(e => e.RegiãoMercadoLocal)
                    .HasColumnName("Região Mercado Local")
                    .HasMaxLength(20);

                entity.Property(e => e.ReparaçãoComGarantia).HasColumnName("Reparação com Garantia");

                entity.Property(e => e.ReposiçãoDeStock).HasColumnName("Reposição de Stock");

                entity.Property(e => e.RequisiçãoDetergentes).HasColumnName("Requisição Detergentes");

                entity.Property(e => e.RequisiçãoNutrição).HasColumnName("Requisição Nutrição");

                entity.Property(e => e.ResponsávelAprovação)
                    .HasColumnName("Responsável Aprovação")
                    .HasMaxLength(50);

                entity.Property(e => e.ResponsávelCriação)
                    .HasColumnName("Responsável Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.ResponsávelReceção)
                    .HasColumnName("Responsável Receção")
                    .HasMaxLength(50);

                entity.Property(e => e.ResponsávelReceçãoReceção)
                    .HasColumnName("Responsável Receção Receção")
                    .HasMaxLength(50);

                entity.Property(e => e.ResponsávelReceçãoRecolha)
                    .HasColumnName("Responsável Receção Recolha")
                    .HasMaxLength(50);

                entity.Property(e => e.ResponsávelValidação)
                    .HasColumnName("Responsável Validação")
                    .HasMaxLength(50);

                entity.Property(e => e.UnidadeProdutivaAlimentação)
                    .HasColumnName("Unidade Produtiva Alimentação")
                    .HasMaxLength(20);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.ValorEstimado)
                    .HasColumnName("Valor Estimado")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.Viatura).HasMaxLength(20);
            });

            modelBuilder.Entity<RequisiçõesClienteContrato>(entity =>
            {
                entity.HasKey(e => new { e.NºContrato, e.GrupoFatura, e.NºProjeto, e.DataInícioCompromisso });

                entity.ToTable("Requisições Cliente Contrato");

                entity.Property(e => e.NºContrato)
                    .HasColumnName("Nº Contrato")
                    .HasMaxLength(20);

                entity.Property(e => e.GrupoFatura).HasColumnName("Grupo Fatura");

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.DataInícioCompromisso)
                    .HasColumnName("Data Início Compromisso")
                    .HasColumnType("date");

                entity.Property(e => e.DataFimCompromisso)
                    .HasColumnName("Data Fim Compromisso")
                    .HasColumnType("date");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataRequisição)
                    .HasColumnName("Data Requisição")
                    .HasColumnType("date");

                entity.Property(e => e.DataÚltimaFatura)
                    .HasColumnName("Data Última Fatura")
                    .HasColumnType("date");

                entity.Property(e => e.NºCompromisso)
                    .HasColumnName("Nº Compromisso")
                    .HasMaxLength(50);

                entity.Property(e => e.NºFatura)
                    .HasColumnName("Nº Fatura")
                    .HasMaxLength(20);

                entity.Property(e => e.NºRequisiçãoCliente)
                    .HasColumnName("Nº Requisição Cliente")
                    .HasMaxLength(30);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.ValorFatura)
                    .HasColumnName("Valor Fatura")
                    .HasColumnType("decimal(, 20)");
            });

            modelBuilder.Entity<RequisicoesRegAlteracoes>(entity =>
            {
                entity.Property(e => e.ModificadoEm).HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NºRequisição)
                    .IsRequired()
                    .HasColumnName("Nº Requisição")
                    .HasMaxLength(20);

                entity.Property(e => e.Observacoes)
                    .HasColumnName("Observacoes")
                    .HasMaxLength(2000);

                entity.HasOne(d => d.NºRequisiçãoNavigation)
                    .WithMany(p => p.RequisicoesRegAlteracoes)
                    .HasForeignKey(d => d.NºRequisição)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequisicoesRegAlteracoes_Requisição");
            });

            modelBuilder.Entity<RequisiçõesSimplificadas>(entity =>
            {
                entity.HasKey(e => e.NºRequisição);

                entity.ToTable("Requisições Simplificadas");

                entity.Property(e => e.NºRequisição)
                    .HasColumnName("Nº Requisição")
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.CódLocalização)
                    .HasColumnName("Cód. Localização")
                    .HasMaxLength(10);

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataHoraAprovação)
                    .HasColumnName("Data/Hora Aprovação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraAutorização)
                    .HasColumnName("Data/Hora Autorização")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraDisponibilização)
                    .HasColumnName("Data/Hora Disponibilização")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraEnvio)
                    .HasColumnName("Data/Hora Envio")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraRequisição)
                    .HasColumnName("Data/Hora Requisição")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraVisar)
                    .HasColumnName("Data/Hora Visar")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataReceçãoEsperada)
                    .HasColumnName("Data Receção Esperada")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataReceçãoLinhas).HasColumnName("Data Receção Linhas");

                entity.Property(e => e.DataRegisto)
                    .HasColumnName("Data Registo")
                    .HasColumnType("date");

                entity.Property(e => e.NºFuncionário)
                    .HasColumnName("Nº Funcionário")
                    .HasMaxLength(20);

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.NºUnidadeProdutiva).HasColumnName("Nº Unidade Produtiva");

                entity.Property(e => e.Observações).HasColumnType("text");

                entity.Property(e => e.RequisiçãoModelo).HasColumnName("Requisição Modelo");

                entity.Property(e => e.RequisiçãoNutrição).HasColumnName("Requisição Nutrição");

                entity.Property(e => e.ResponsávelAprovação)
                    .HasColumnName("Responsável Aprovação")
                    .HasMaxLength(50);

                entity.Property(e => e.ResponsávelAutorização)
                    .HasColumnName("Responsável Autorização")
                    .HasMaxLength(50);

                entity.Property(e => e.ResponsávelCriação)
                    .HasColumnName("Responsável Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.ResponsávelEnvio)
                    .HasColumnName("Responsável Envio")
                    .HasMaxLength(50);

                entity.Property(e => e.ResponsávelReceção)
                    .HasColumnName("Responsável Receção")
                    .HasMaxLength(50);

                entity.Property(e => e.ResponsávelVisar)
                    .HasColumnName("Responsável Visar")
                    .HasMaxLength(50);

                entity.Property(e => e.TipoRefeição).HasColumnName("Tipo Refeição");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.Visadores).HasMaxLength(100);

                entity.HasOne(d => d.NºProjetoNavigation)
                    .WithMany(p => p.RequisiçõesSimplificadas)
                    .HasForeignKey(d => d.NºProjeto)
                    .HasConstraintName("FK_Requisições Simplificadas_Projetos");

                entity.HasOne(d => d.NºUnidadeProdutivaNavigation)
                    .WithMany(p => p.RequisiçõesSimplificadas)
                    .HasForeignKey(d => d.NºUnidadeProdutiva)
                    .HasConstraintName("FK_Requisições Simplificadas_Unidades Produtivas");
            });

            modelBuilder.Entity<RhRecursosFh>(entity =>
            {
                entity.HasKey(e => new { e.NoEmpregado, e.Recurso });

                entity.ToTable("RH_Recursos_FH");

                entity.Property(e => e.NoEmpregado).HasMaxLength(10);

                entity.Property(e => e.Recurso).HasMaxLength(20);

                entity.Property(e => e.AlteradoPor).HasMaxLength(50);

                entity.Property(e => e.CriadoPor).HasMaxLength(50);

                entity.Property(e => e.DataHoraCriacao).HasColumnType("datetime");

                entity.Property(e => e.DataHoraUltimaAlteracao).HasColumnType("datetime");

                entity.Property(e => e.FamiliaRecurso).HasMaxLength(20);

                entity.Property(e => e.NomeEmpregado).HasMaxLength(100);

                entity.Property(e => e.NomeRecurso).HasMaxLength(50);
            });

            modelBuilder.Entity<SeleccaoEntidades>(entity =>
            {
                entity.HasKey(e => e.IdSeleccaoEntidades);

                entity.ToTable("Seleccao_Entidades");

                entity.Property(e => e.IdSeleccaoEntidades).HasColumnName("ID_Seleccao_Entidades");

                entity.Property(e => e.CidadeFornecedor)
                    .HasColumnName("Cidade_Fornecedor")
                    .HasMaxLength(50);

                entity.Property(e => e.CodActividade)
                    .HasColumnName("Cod_Actividade")
                    .HasMaxLength(20);

                entity.Property(e => e.CodFormaPagamento)
                    .HasColumnName("Cod_Forma_Pagamento")
                    .HasMaxLength(10);

                entity.Property(e => e.CodFornecedor)
                    .HasColumnName("Cod_Fornecedor")
                    .HasMaxLength(20);

                entity.Property(e => e.CodTermosPagamento)
                    .HasColumnName("Cod_Termos_Pagamento")
                    .HasMaxLength(10);

                entity.Property(e => e.DataEnvioAoFornecedor).HasColumnType("datetime");

                entity.Property(e => e.DataEnvioPropostaArea).HasColumnType("datetime");

                entity.Property(e => e.DataPedidoEsclarecimento).HasColumnType("datetime");

                entity.Property(e => e.DataRecepcaoProposta).HasColumnType("datetime");

                entity.Property(e => e.DataRespostaArea).HasColumnType("datetime");

                entity.Property(e => e.DataRespostaDoFornecedor).HasColumnType("datetime");

                entity.Property(e => e.DataRespostaEsclarecimento).HasColumnType("datetime");

                entity.Property(e => e.DataRespostaEsperada).HasColumnType("datetime");

                entity.Property(e => e.EmailFornecedor)
                    .HasColumnName("Email_Fornecedor")
                    .HasMaxLength(60);

                entity.Property(e => e.Fase).HasDefaultValueSql("((0))");

                entity.Property(e => e.NaoRespostaDoFornecedor).HasDefaultValueSql("((0))");

                entity.Property(e => e.NomeFornecedor)
                    .HasColumnName("Nome_Fornecedor")
                    .HasMaxLength(50);

                entity.Property(e => e.NumConsultaMercado)
                    .HasColumnName("Num_Consulta_Mercado")
                    .HasMaxLength(20);

                entity.Property(e => e.UtilizadorEnvio).HasMaxLength(60);

                entity.Property(e => e.UtilizadorRecepcaoProposta).HasMaxLength(60);

                entity.HasOne(d => d.NumConsultaMercadoNavigation)
                    .WithMany(p => p.SeleccaoEntidades)
                    .HasForeignKey(d => d.NumConsultaMercado)
                    .HasConstraintName("FK_Seleccao_Entidades_Consulta_Mercado");
            });

            modelBuilder.Entity<Serviços>(entity =>
            {
                entity.HasKey(e => e.Código);

                entity.Property(e => e.Código)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(200);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ServiçosCliente>(entity =>
            {
                entity.HasKey(e => new { e.NºCliente, e.CódServiço });

                entity.ToTable("Serviços Cliente");

                entity.Property(e => e.NºCliente)
                    .HasColumnName("Nº Cliente")
                    .HasMaxLength(50);

                entity.Property(e => e.CódServiço)
                    .HasColumnName("Cód. Serviço")
                    .HasMaxLength(50);

                entity.Property(e => e.CodGrupoServico)
                    .HasColumnName("CodGrupoServico")
                    .HasMaxLength(50);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.GrupoServiços).HasColumnName("Grupo Serviços");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.CódServiçoNavigation)
                    .WithMany(p => p.ServiçosCliente)
                    .HasForeignKey(d => d.CódServiço)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Serviços Cliente_Serviços");
            });

            modelBuilder.Entity<TabelaConfRecursosFh>(entity =>
            {
                entity.HasKey(e => new { e.Tipo, e.CodRecurso });

                entity.ToTable("Tabela_Conf_Recursos_FH");

                entity.Property(e => e.Tipo).HasMaxLength(20);

                entity.Property(e => e.CodRecurso)
                    .HasColumnName("Cod_Recurso")
                    .HasMaxLength(20);

                entity.Property(e => e.Descricao).HasMaxLength(200);

                entity.Property(e => e.PrecoUnitarioCusto)
                    .HasColumnName("Preco_Unitario_Custo")
                    .HasColumnType("decimal(, 20)");

                entity.Property(e => e.PrecoUnitarioVenda)
                    .HasColumnName("Preco_Unitario_Venda")
                    .HasColumnType("decimal(, 20)");

                entity.Property(e => e.RubricaSalarial)
                    .HasColumnName("Rubrica_Salarial")
                    .HasMaxLength(20);

                entity.Property(e => e.UnidMedida)
                    .HasColumnName("Unid_Medida")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Tarifários>(entity =>
            {
                entity.HasKey(e => e.Código);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TaxaResiduos>(entity =>
            {
                entity.HasKey(e => e.Recurso);

                entity.Property(e => e.Recurso)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Data).HasColumnType("datetime");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.FamiliaRecurso)
                    .HasColumnName("Familia Recurso")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Telefones>(entity =>
            {
                entity.HasKey(e => e.NºTelefone);

                entity.Property(e => e.NºTelefone)
                    .HasColumnName("Nº Telefone")
                    .HasMaxLength(9)
                    .ValueGeneratedNever();

                entity.Property(e => e.Cidade).HasMaxLength(30);

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DébitoDireto).HasColumnName("Débito Direto");

                entity.Property(e => e.Fornecedor).HasMaxLength(30);

                entity.Property(e => e.IpRouter)
                    .HasColumnName("IP Router")
                    .HasMaxLength(20);

                entity.Property(e => e.IpServidor)
                    .HasColumnName("IP Servidor")
                    .HasMaxLength(20);

                entity.Property(e => e.LarguraDeBanda)
                    .HasColumnName("Largura de Banda")
                    .HasMaxLength(10);

                entity.Property(e => e.LocalDaInstalação)
                    .HasColumnName("Local da Instalação")
                    .HasMaxLength(120);

                entity.Property(e => e.Morada).HasMaxLength(120);

                entity.Property(e => e.NºExtensões).HasColumnName("Nº Extensões");

                entity.Property(e => e.NºFaxes).HasColumnName("Nº Faxes");

                entity.Property(e => e.NºTelefoneÚltimo)
                    .HasColumnName("Nº Telefone (último)")
                    .HasMaxLength(9);

                entity.Property(e => e.Observações).HasColumnType("text");

                entity.Property(e => e.RedeDeDados).HasColumnName("Rede de Dados");

                entity.Property(e => e.Site).HasMaxLength(50);

                entity.Property(e => e.TipoDeCircuito).HasColumnName("Tipo de Circuito");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.ValorAcrescentado).HasColumnName("Valor Acrescentado");

                entity.Property(e => e.ValorAssinaturaMensal).HasColumnName("Valor Assinatura Mensal");
            });

            modelBuilder.Entity<Telemóveis>(entity =>
            {
                entity.HasKey(e => new { e.ImeiNºSérie, e.Tipo });

                entity.Property(e => e.ImeiNºSérie)
                    .HasColumnName("IMEI/Nº Série")
                    .HasMaxLength(16);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.EstadoEquipamento).HasColumnName("Estado Equipamento");

                entity.Property(e => e.Observações).HasColumnType("text");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.MarcaNavigation)
                    .WithMany(p => p.Telemóveis)
                    .HasForeignKey(d => d.Marca)
                    .HasConstraintName("FK_Telemóveis_Marcas");
            });

            modelBuilder.Entity<TelemoveisCartoes>(entity =>
            {
                entity.HasKey(e => e.NumCartao);

                entity.ToTable("Telemoveis_Cartoes");

                entity.Property(e => e.NumCartao)
                    .HasColumnName("Num_Cartao")
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Barramentos)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ChamadasInternacionais)
                    .HasColumnName("Chamadas_Internacionais")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CodAreaFuncional)
                    .HasColumnName("Cod_Area_Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.CodCentroResponsabilidade)
                    .HasColumnName("Cod_Centro_Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CodRegiao)
                    .HasColumnName("Cod_Regiao")
                    .HasMaxLength(20);

                entity.Property(e => e.ContaSuch)
                    .HasColumnName("Conta_SUCH")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ContaUtilizador)
                    .HasColumnName("Conta_Utilizador")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DataAlteracao)
                    .HasColumnName("Data_Alteracao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataAtribuicao)
                    .HasColumnName("Data_Atribuicao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataEstado)
                    .HasColumnName("Data_Estado")
                    .HasColumnType("datetime");

                entity.Property(e => e.Declaracao).HasDefaultValueSql("((0))");

                entity.Property(e => e.EquipamentoNaoDevolvido)
                    .HasColumnName("Equipamento_Nao_Devolvido")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Estado).HasDefaultValueSql("((0))");

                entity.Property(e => e.ExtensaoVpn)
                    .HasColumnName("Extensao_VPN")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FimFidelizacao)
                    .HasColumnName("Fim_Fidelizacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.Gprs)
                    .HasColumnName("GPRS")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Grupo)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Imei)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Internet).HasDefaultValueSql("((0))");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.NumFuncionario)
                    .HasColumnName("Num_Funcionario")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Observacoes)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Plafond100percUtilizador)
                    .HasColumnName("Plafond_100perc_Utilizador")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PlafondDados)
                    .HasColumnName("Plafond_Dados")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PlafondExtra)
                    .HasColumnName("Plafond_Extra")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PlafondFr)
                    .HasColumnName("Plafond_FR")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Roaming).HasDefaultValueSql("((0))");

                entity.Property(e => e.TarifarioDados)
                    .HasColumnName("Tarifario_Dados")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TarifarioVoz)
                    .HasColumnName("Tarifario_Voz")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoServico).HasColumnName("Tipo_Servico");

                entity.Property(e => e.Utilizador)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ValorMensalidadeDados)
                    .HasColumnName("Valor_Mensalidade_Dados")
                    .HasColumnType("decimal(, 20)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.WhiteList)
                    .HasColumnName("White_List")
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TelemoveisEquipamentos>(entity =>
            {
                entity.HasKey(e => new { e.Tipo, e.Imei });

                entity.ToTable("Telemoveis_Equipamentos");

                entity.Property(e => e.Imei)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Cor)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DataAlteracao)
                    .HasColumnName("Data_Alteracao")
                    .HasColumnType("date");

                entity.Property(e => e.DataHoraCriacao)
                    .HasColumnName("Data_Hora_Criacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificacao)
                    .HasColumnName("Data_Hora_Modificacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataRecepcao)
                    .HasColumnName("Data_Recepcao")
                    .HasColumnType("date");

                entity.Property(e => e.DevolvidoBk).HasColumnName("Devolvido_bk");

                entity.Property(e => e.Documento)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentoRecepcao)
                    .HasColumnName("Documento_Recepcao")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Marca)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Modelo)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.NomeComprador)
                    .HasColumnName("Nome_Comprador")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.NumEmpregadoComprador)
                    .HasColumnName("Num_Empregado_Comprador")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Observacoes)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Utilizador)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("Utilizador_Criacao")
                    .HasMaxLength(60);

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("Utilizador_Modificacao")
                    .HasMaxLength(60);
            });

            modelBuilder.Entity<TelemoveisMovimentos>(entity =>
            {
                entity.HasKey(e => e.NumMovimento);

                entity.ToTable("Telemoveis_Movimentos");

                entity.Property(e => e.NumMovimento).HasColumnName("Num_Movimento");

                entity.Property(e => e.CodAreaFuncional)
                    .IsRequired()
                    .HasColumnName("Cod_Area_Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.CodCentroResponsabilidade)
                    .IsRequired()
                    .HasColumnName("Cod_Centro_Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CodRegiao)
                    .IsRequired()
                    .HasColumnName("Cod_Regiao")
                    .HasMaxLength(20);

                entity.Property(e => e.Data)
                    .IsRequired()
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.DataFactura)
                    .HasColumnName("Data_Factura")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataRegisto)
                    .HasColumnName("Data_Registo")
                    .HasColumnType("datetime");

                entity.Property(e => e.NumCartao)
                    .IsRequired()
                    .HasColumnName("Num_Cartao")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.NumDocumento)
                    .HasColumnName("Num_Documento")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NumFactura)
                    .IsRequired()
                    .HasColumnName("Num_Factura")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Utilizador)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Valor).HasColumnType("decimal(, 20)");

                entity.Property(e => e.ValorComIva)
                    .HasColumnName("Valor_Com_IVA")
                    .HasColumnType("decimal(, 20)");
            });

            modelBuilder.Entity<TelemoveisOcorrencias>(entity =>
            {
                entity.HasKey(e => e.NumOcorrencia);

                entity.ToTable("Telemoveis_Ocorrencias");

                entity.Property(e => e.NumOcorrencia).HasColumnName("Num_Ocorrencia");

                entity.Property(e => e.DataAlteracao)
                    .HasColumnName("Data_Alteracao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataOcorrencia)
                    .HasColumnName("Data_Ocorrencia")
                    .HasColumnType("datetime");

                entity.Property(e => e.NumCartao)
                    .IsRequired()
                    .HasColumnName("Num_Cartao")
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.Ocorrencia)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Utilizador)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TemposPaCcp>(entity =>
            {
                entity.HasKey(e => e.NºProcedimento);

                entity.ToTable("Tempos PA CCP");

                entity.Property(e => e.NºProcedimento)
                    .HasColumnName("Nº Procedimento")
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Estado0).HasColumnName("Estado 0");

                entity.Property(e => e.Estado0Tg).HasColumnName("Estado 0 TG");

                entity.Property(e => e.Estado1).HasColumnName("Estado 1");

                entity.Property(e => e.Estado10).HasColumnName("Estado 10");

                entity.Property(e => e.Estado10Tg).HasColumnName("Estado 10 TG");

                entity.Property(e => e.Estado11).HasColumnName("Estado 11");

                entity.Property(e => e.Estado11Tg).HasColumnName("Estado 11 TG");

                entity.Property(e => e.Estado12).HasColumnName("Estado 12");

                entity.Property(e => e.Estado12Tg).HasColumnName("Estado 12 TG");

                entity.Property(e => e.Estado13).HasColumnName("Estado 13");

                entity.Property(e => e.Estado13Tg).HasColumnName("Estado 13 TG");

                entity.Property(e => e.Estado14).HasColumnName("Estado 14");

                entity.Property(e => e.Estado14Tg).HasColumnName("Estado 14 TG");

                entity.Property(e => e.Estado15).HasColumnName("Estado 15");

                entity.Property(e => e.Estado15Tg).HasColumnName("Estado 15 TG");

                entity.Property(e => e.Estado16).HasColumnName("Estado 16");

                entity.Property(e => e.Estado16Tg).HasColumnName("Estado 16 TG");

                entity.Property(e => e.Estado17).HasColumnName("Estado 17");

                entity.Property(e => e.Estado17Tg).HasColumnName("Estado 17 TG");

                entity.Property(e => e.Estado18).HasColumnName("Estado 18");

                entity.Property(e => e.Estado18Tg).HasColumnName("Estado 18 TG");

                entity.Property(e => e.Estado19).HasColumnName("Estado 19");

                entity.Property(e => e.Estado19Tg).HasColumnName("Estado 19 TG");

                entity.Property(e => e.Estado1Tg).HasColumnName("Estado 1 TG");

                entity.Property(e => e.Estado2).HasColumnName("Estado 2");

                entity.Property(e => e.Estado20).HasColumnName("Estado 20");

                entity.Property(e => e.Estado20Tg).HasColumnName("Estado 20 TG");

                entity.Property(e => e.Estado2Tg).HasColumnName("Estado 2 TG");

                entity.Property(e => e.Estado3).HasColumnName("Estado 3");

                entity.Property(e => e.Estado3Tg).HasColumnName("Estado 3 TG");

                entity.Property(e => e.Estado4).HasColumnName("Estado 4");

                entity.Property(e => e.Estado4Tg).HasColumnName("Estado 4 TG");

                entity.Property(e => e.Estado5).HasColumnName("Estado 5");

                entity.Property(e => e.Estado5Tg).HasColumnName("Estado 5 TG");

                entity.Property(e => e.Estado6).HasColumnName("Estado 6");

                entity.Property(e => e.Estado6Tg).HasColumnName("Estado 6 TG");

                entity.Property(e => e.Estado7).HasColumnName("Estado 7");

                entity.Property(e => e.Estado7Tg).HasColumnName("Estado 7 TG");

                entity.Property(e => e.Estado8).HasColumnName("Estado 8");

                entity.Property(e => e.Estado8Tg).HasColumnName("Estado 8 TG");

                entity.Property(e => e.Estado9).HasColumnName("Estado 9");

                entity.Property(e => e.Estado9Tg).HasColumnName("Estado 9 TG");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.NºProcedimentoNavigation)
                    .WithOne(p => p.TemposPaCcp)
                    .HasForeignKey<TemposPaCcp>(d => d.NºProcedimento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tempos PA CCP_Procedimentos CCP");
            });

            modelBuilder.Entity<TextoFaturaContrato>(entity =>
            {
                entity.HasKey(e => new { e.NºContrato, e.GrupoFatura, e.NºProjeto });

                entity.ToTable("Texto Fatura Contrato");

                entity.Property(e => e.NºContrato)
                    .HasColumnName("Nº Contrato")
                    .HasMaxLength(20);

                entity.Property(e => e.GrupoFatura).HasColumnName("Grupo Fatura");

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.TextoFatura)
                    .HasColumnName("Texto Fatura")
                    .HasColumnType("text");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TipoDeProjeto>(entity =>
            {
                entity.HasKey(e => e.Código);

                entity.ToTable("Tipo de Projeto");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TiposGrupoContabOmProjeto>(entity =>
            {
                entity.HasKey(e => e.Código);

                entity.ToTable("Tipos Grupo Contab. OM Projeto");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(100);

                entity.Property(e => e.IndicIncidênciasAvarias).HasColumnName("Indic. Incidências Avarias");

                entity.Property(e => e.IndicTaxaCumprRotinasMp).HasColumnName("Indic. Taxa Cumpr. Rotinas MP");

                entity.Property(e => e.IndicTaxaCumprimentoCat).HasColumnName("Indic. Taxa Cumprimento CAT");

                entity.Property(e => e.IndicadorOrdensEmCurso).HasColumnName("Indicador Ordens em Curso");

                entity.Property(e => e.IndicadorTaxaCoberturaCat).HasColumnName("Indicador Taxa Cobertura CAT");

                entity.Property(e => e.IndicadorTempoEfetivoReparação).HasColumnName("Indicador Tempo Efetivo Reparação");

                entity.Property(e => e.IndicadorTempoFaturação).HasColumnName("Indicador Tempo Faturação");

                entity.Property(e => e.IndicadorTempoFechoObras).HasColumnName("Indicador Tempo Fecho Obras");

                entity.Property(e => e.IndicadorTempoImobilização).HasColumnName("Indicador Tempo Imobilização");

                entity.Property(e => e.IndicadorTempoOcupColaboradores).HasColumnName("Indicador Tempo Ocup. Colaboradores");

                entity.Property(e => e.IndicadorTempoResposta).HasColumnName("Indicador Tempo Resposta");

                entity.Property(e => e.IndicadorValorCustoVenda).HasColumnName("Indicador Valor Custo/Venda");

                entity.Property(e => e.ManutCorretiva).HasColumnName("Manut. Corretiva");

                entity.Property(e => e.ManutPreventiva).HasColumnName("Manut. Preventiva");

                entity.Property(e => e.TipoRazãoFalha).HasColumnName("Tipo Razão Falha");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TiposGrupoContabProjeto>(entity =>
            {
                entity.HasKey(e => e.Código);

                entity.ToTable("Tipos Grupo Contab. Projeto");

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TiposRefeição>(entity =>
            {
                entity.HasKey(e => e.Código);

                entity.ToTable("Tipos Refeição");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(50);

                entity.Property(e => e.GrupoContabProduto)
                    .HasColumnName("Grupo Contab. Produto")
                    .HasMaxLength(10);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TiposRequisições>(entity =>
            {
                entity.HasKey(e => e.Código);

                entity.ToTable("Tipos Requisições");

                entity.Property(e => e.Código)
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TiposViatura>(entity =>
            {
                entity.HasKey(e => e.CódigoTipo);

                entity.ToTable("Tipos Viatura");

                entity.Property(e => e.CódigoTipo).HasColumnName("Código Tipo");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TipoTrabalhoFh>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("Tipo_Trabalho_FH");

                entity.Property(e => e.Codigo)
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.AlteradoPor).HasMaxLength(50);

                entity.Property(e => e.CodUnidadeMedida).HasMaxLength(10);

                entity.Property(e => e.CriadoPor).HasMaxLength(50);

                entity.Property(e => e.DataHoraCriacao).HasColumnType("datetime");

                entity.Property(e => e.DataHoraUltimaAlteracao).HasColumnType("datetime");

                entity.Property(e => e.Descricao).HasMaxLength(30);
            });

            modelBuilder.Entity<UnidadeDeArmazenamento>(entity =>
            {
                entity.HasKey(e => e.NºProduto);

                entity.ToTable("Unidade de Armazenamento");

                entity.Property(e => e.NºProduto)
                    .HasColumnName("Nº Produto")
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.ArmazémPrincipal).HasColumnName("Armazém Principal");

                entity.Property(e => e.ConsumoMédio)
                    .HasColumnName("Consumo Médio")
                    .HasColumnType("decimal(, 2)");

                entity.Property(e => e.CustoPadrão)
                    .HasColumnName("Custo Padrão")
                    .HasColumnType("decimal(, 2)");

                entity.Property(e => e.CustoUnitário)
                    .HasColumnName("Custo Unitário")
                    .HasColumnType("decimal(, 2)");

                entity.Property(e => e.CódCategoriaProduto)
                    .HasColumnName("Cód. Categoria Produto")
                    .HasMaxLength(20);

                entity.Property(e => e.CódGrupoProduto)
                    .HasColumnName("Cód. Grupo Produto")
                    .HasMaxLength(20);

                entity.Property(e => e.CódLocalização)
                    .HasColumnName("Cód. Localização")
                    .HasMaxLength(10);

                entity.Property(e => e.CódProdForn)
                    .HasColumnName("Cód. Prod. Forn")
                    .HasMaxLength(20);

                entity.Property(e => e.CódUnidadeMedidaProduto)
                    .HasColumnName("Cód. Unidade Medida Produto")
                    .HasMaxLength(10);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(100);

                entity.Property(e => e.Inventário).HasColumnType("decimal(, 2)");

                entity.Property(e => e.QuantidadeReservada).HasColumnType("decimal(38, 2)");

                entity.Property(e => e.QuantidadeEncomendada).HasColumnType("decimal(38, 2)");

                entity.Property(e => e.ListaMateriais).HasColumnName("Lista Materiais");

                entity.Property(e => e.NºFornecedor)
                    .HasColumnName("Nº Fornecedor")
                    .HasMaxLength(20);

                entity.Property(e => e.NºPrateleira)
                    .HasColumnName("Nº Prateleira")
                    .HasMaxLength(12);

                entity.Property(e => e.PreçoDeVenda)
                    .HasColumnName("Preço de Venda")
                    .HasColumnType("decimal(, 2)");

                entity.Property(e => e.TamanhoLote)
                    .HasColumnName("Tamanho Lote")
                    .HasColumnType("decimal(, 2)");

                entity.Property(e => e.UltimoCustoDirecto)
                    .HasColumnName("Ultimo Custo Directo")
                    .HasColumnType("decimal(, 2)");

                entity.Property(e => e.UsarMrp2).HasColumnName("Usar MRP2");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.ValorEmArmazem)
                    .HasColumnName("Valor em Armazem")
                    .HasColumnType("decimal(, 2)");
            });

            modelBuilder.Entity<UnidadeMedida>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("Unidade Medida");

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UnidadeMedidaProduto>(entity =>
            {
                entity.HasKey(e => new { e.NºProduto, e.Código });

                entity.ToTable("Unidade Medida Produto");

                entity.Property(e => e.NºProduto)
                    .HasColumnName("Nº Produto")
                    .HasMaxLength(20);

                entity.Property(e => e.Código).HasMaxLength(10);

                entity.Property(e => e.Altura).HasColumnType("decimal(, 2)");

                entity.Property(e => e.Comprimento).HasColumnType("decimal(, 2)");

                entity.Property(e => e.Cubagem).HasColumnType("decimal(, 2)");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Largura).HasColumnType("decimal(, 2)");

                entity.Property(e => e.Peso).HasColumnType("decimal(, 2)");

                entity.Property(e => e.QtdPorUnidadeMedida)
                    .HasColumnName("Qtd por Unidade Medida")
                    .HasColumnType("decimal(, 2)");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UnidadePrestação>(entity =>
            {
                entity.HasKey(e => e.Código);

                entity.ToTable("Unidade Prestação");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasColumnType("nchar(50)");

                entity.Property(e => e.Email1).HasMaxLength(100);

                entity.Property(e => e.Email2).HasMaxLength(100);

                entity.Property(e => e.Email3).HasMaxLength(100);

                entity.Property(e => e.EmailRegiao12).HasMaxLength(100);

                entity.Property(e => e.EmailRegiao23).HasMaxLength(100);

                entity.Property(e => e.EmailRegiao33).HasMaxLength(100);

                entity.Property(e => e.EmailRegiao43).HasMaxLength(100);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UnidadesProdutivas>(entity =>
            {
                entity.HasKey(e => e.NºUnidadeProdutiva);

                entity.ToTable("Unidades Produtivas");

                entity.Property(e => e.NºUnidadeProdutiva).HasColumnName("Nº Unidade Produtiva");

                entity.Property(e => e.Armazém).HasMaxLength(10);

                entity.Property(e => e.ArmazémFornecedor)
                    .HasColumnName("Armazém Fornecedor")
                    .HasMaxLength(10);

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataFimExploração)
                    .HasColumnName("Data Fim Exploração")
                    .HasColumnType("date");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataInícioExploração)
                    .HasColumnName("Data Início Exploração")
                    .HasColumnType("date");

                entity.Property(e => e.DataModificação)
                    .HasColumnName("Data Modificação")
                    .HasColumnType("date");

                entity.Property(e => e.Descrição).HasMaxLength(50);

                entity.Property(e => e.NºCliente)
                    .HasColumnName("Nº Cliente")
                    .HasMaxLength(20);

                entity.Property(e => e.ProjetoCozinha)
                    .HasColumnName("Projeto Cozinha")
                    .HasMaxLength(20);

                entity.Property(e => e.ProjetoDespMatPrimas)
                    .HasColumnName("Projeto Desp. Mat. Primas")
                    .HasMaxLength(20);

                entity.Property(e => e.ProjetoDesperdícios)
                    .HasColumnName("Projeto Desperdícios")
                    .HasMaxLength(20);

                entity.Property(e => e.ProjetoMatSubsidiárias)
                    .HasColumnName("Projeto Mat. Subsidiárias")
                    .HasMaxLength(20);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.ProjetoCozinhaNavigation)
                    .WithMany(p => p.UnidadesProdutivasProjetoCozinhaNavigation)
                    .HasForeignKey(d => d.ProjetoCozinha)
                    .HasConstraintName("FK_Unidades Produtivas_Projetos");

                entity.HasOne(d => d.ProjetoDespMatPrimasNavigation)
                    .WithMany(p => p.UnidadesProdutivasProjetoDespMatPrimasNavigation)
                    .HasForeignKey(d => d.ProjetoDespMatPrimas)
                    .HasConstraintName("FK_Unidades Produtivas_Projetos2");

                entity.HasOne(d => d.ProjetoDesperdíciosNavigation)
                    .WithMany(p => p.UnidadesProdutivasProjetoDesperdíciosNavigation)
                    .HasForeignKey(d => d.ProjetoDesperdícios)
                    .HasConstraintName("FK_Unidades Produtivas_Projetos1");

                entity.HasOne(d => d.ProjetoMatSubsidiáriasNavigation)
                    .WithMany(p => p.UnidadesProdutivasProjetoMatSubsidiáriasNavigation)
                    .HasForeignKey(d => d.ProjetoMatSubsidiárias)
                    .HasConstraintName("FK_Unidades Produtivas_Projetos3");
            });

            modelBuilder.Entity<UtilizadoresGruposAprovação>(entity =>
            {
                entity.HasKey(e => new { e.GrupoAprovação, e.UtilizadorAprovação });

                entity.ToTable("Utilizadores Grupos Aprovação");

                entity.Property(e => e.GrupoAprovação).HasColumnName("Grupo Aprovação");

                entity.Property(e => e.UtilizadorAprovação)
                    .HasColumnName("Utilizador Aprovação")
                    .HasMaxLength(50);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.EnviarEmailAlerta).HasDefaultValueSql("((0))");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.GrupoAprovaçãoNavigation)
                    .WithMany(p => p.UtilizadoresGruposAprovação)
                    .HasForeignKey(d => d.GrupoAprovação)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Utilizadores Grupos Aprovação_Grupos Aprovação");
            });

            modelBuilder.Entity<UtilizadoresMovimentosDeAprovação>(entity =>
            {
                entity.HasKey(e => new { e.NºMovimento, e.Utilizador });

                entity.ToTable("Utilizadores Movimentos de Aprovação");

                entity.Property(e => e.NºMovimento).HasColumnName("Nº Movimento");

                entity.Property(e => e.Utilizador).HasMaxLength(50);

                entity.HasOne(d => d.NºMovimentoNavigation)
                    .WithMany(p => p.UtilizadoresMovimentosDeAprovação)
                    .HasForeignKey(d => d.NºMovimento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Utilizadores Movimentos de Aprovação_Movimentos de Aprovação");
            });

            modelBuilder.Entity<Viaturas>(entity =>
            {
                entity.HasKey(e => e.Matrícula);

                entity.Property(e => e.Matrícula)
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.Apólice).HasMaxLength(20);

                entity.Property(e => e.AtribuídaA)
                    .HasColumnName("Atribuída a")
                    .HasMaxLength(20);

                entity.Property(e => e.CartaVerde)
                    .HasColumnName("Carta Verde")
                    .HasMaxLength(80);

                entity.Property(e => e.CartãoCombustível)
                    .HasColumnName("Cartão Combustível")
                    .HasMaxLength(20);

                entity.Property(e => e.ConsumoIndicativo).HasColumnName("Consumo Indicativo");

                entity.Property(e => e.Cor).HasMaxLength(20);

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoMarca).HasColumnName("Código Marca");

                entity.Property(e => e.CódigoModelo).HasColumnName("Código Modelo");

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoTipoViatura).HasColumnName("Código Tipo Viatura");

                entity.Property(e => e.CódigoÁreaFuncional)
                    .HasColumnName("Código Área Funcional")
                    .HasMaxLength(20);

                entity.Property(e => e.DataAbate)
                    .HasColumnName("Data Abate")
                    .HasColumnType("date");

                entity.Property(e => e.DataAquisição)
                    .HasColumnName("Data Aquisição")
                    .HasColumnType("date");

                entity.Property(e => e.DataEntradaFuncionamento)
                    .HasColumnName("Data Entrada Funcionamento")
                    .HasColumnType("date");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataMatrícula)
                    .HasColumnName("Data Matrícula")
                    .HasColumnType("date");

                entity.Property(e => e.DataUltimaInspeção)
                    .HasColumnName("Data Ultima Inspeção")
                    .HasColumnType("date");

                entity.Property(e => e.DataUltimaRevisão)
                    .HasColumnName("Data Ultima Revisão")
                    .HasColumnType("date");

                entity.Property(e => e.DistânciaEntreEixos).HasColumnName("Distância Entre Eixos");

                entity.Property(e => e.DuraçãoPneus).HasColumnName("Duração Pneus");

                //entity.Property(e => e.Imagem).HasColumnType("image");

                entity.Property(e => e.IntervaloRevisões).HasColumnName("Intervalo Revisões");

                entity.Property(e => e.KmUltimaRevisão).HasColumnName("Km Ultima Revisão");

                entity.Property(e => e.LocalParqueamento)
                    .HasColumnName("Local Parqueamento")
                    .HasMaxLength(80);

                entity.Property(e => e.NoProjeto).HasMaxLength(20);

                entity.Property(e => e.NºImobilizado)
                    .HasColumnName("Nº Imobilizado")
                    .HasMaxLength(20);

                entity.Property(e => e.NºLugares).HasColumnName("Nº Lugares");

                entity.Property(e => e.NºQuadro)
                    .HasColumnName("Nº Quadro")
                    .HasMaxLength(25);

                entity.Property(e => e.NºViaVerde)
                    .HasColumnName("Nº Via Verde")
                    .HasMaxLength(50);

                entity.Property(e => e.Observações).HasColumnType("text");

                entity.Property(e => e.PesoBruto).HasColumnName("Peso Bruto");

                entity.Property(e => e.PneumáticosFrente)
                    .HasColumnName("Pneumáticos Frente")
                    .HasMaxLength(50);

                entity.Property(e => e.PneumáticosRetaguarda)
                    .HasColumnName("Pneumáticos Retaguarda")
                    .HasMaxLength(50);

                entity.Property(e => e.ProximaInspeçãoAté)
                    .HasColumnName("Proxima Inspeção Até")
                    .HasColumnType("date");

                entity.Property(e => e.TipoCombustível).HasColumnName("Tipo Combustível");

                entity.Property(e => e.TipoPropriedade).HasColumnName("Tipo Propriedade");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.ValidadeApólice)
                    .HasColumnName("Validade Apólice")
                    .HasColumnType("date");

                entity.Property(e => e.ValidadeCartaVerde)
                    .HasColumnName("Validade Carta Verde")
                    .HasColumnType("date");

                entity.Property(e => e.ValidadeCartãoCombustivel)
                    .HasColumnName("Validade Cartão Combustivel")
                    .HasColumnType("date");

                entity.Property(e => e.ValorAquisição).HasColumnName("Valor Aquisição");

                entity.Property(e => e.ValorVenda).HasColumnName("Valor Venda");

                entity.HasOne(d => d.CódigoMarcaNavigation)
                    .WithMany(p => p.Viaturas)
                    .HasForeignKey(d => d.CódigoMarca)
                    .HasConstraintName("FK_Viaturas_Marcas");

                entity.HasOne(d => d.CódigoTipoViaturaNavigation)
                    .WithMany(p => p.Viaturas)
                    .HasForeignKey(d => d.CódigoTipoViatura)
                    .HasConstraintName("FK_Viaturas_Tipos Viatura");

                entity.HasOne(d => d.CódigoM)
                    .WithMany(p => p.Viaturas)
                    .HasForeignKey(d => new { d.CódigoMarca, d.CódigoModelo })
                    .HasConstraintName("FK_Viaturas_Modelos");
            });

            modelBuilder.Entity<ViaturasImagens>(entity =>
            {
                entity.HasKey(e => e.Matricula);

                entity.Property(e => e.Matricula)
                    .HasColumnName("Matrícula")
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.Imagem)
                    .HasColumnType("Imagem");

                entity.Property(e => e.DataHoraCriacao)
                    .HasColumnName("DataHoraCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificacao)
                    .HasColumnName("DataHoraModificacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Viaturas2>(entity =>
            {
                entity.HasKey(e => e.Matricula);

                entity.ToTable("Viaturas2");

                entity.Property(e => e.Matricula)
                    .HasColumnName("Matricula")
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.IDEstado)
                    .HasColumnName("IDEstado");

                entity.Property(e => e.IDMarca)
                    .HasColumnName("IDMarca");

                entity.Property(e => e.IDModelo)
                    .HasColumnName("IDModelo");

                entity.Property(e => e.Data1Matricula)
                    .HasColumnName("Data1Matricula")
                    .HasColumnType("date");

                entity.Property(e => e.Cor)
                    .HasColumnName("Cor")
                    .HasMaxLength(40);

                entity.Property(e => e.DataMatricula)
                    .HasColumnName("DataMatricula")
                    .HasColumnType("date");

                entity.Property(e => e.IDCategoria)
                    .HasColumnName("IDCategoria");

                entity.Property(e => e.IDTipo)
                    .HasColumnName("IDTipo");

                entity.Property(e => e.Classificacao)
                    .HasColumnName("Classificacao")
                    .HasMaxLength(80);

                entity.Property(e => e.Cilindrada)
                    .HasColumnName("Cilindrada");

                entity.Property(e => e.IDCombustivel)
                    .HasColumnName("IDCombustivel");

                entity.Property(e => e.ConsumoReferencia)
                    .HasColumnName("ConsumoReferencia")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CapacidadeDeposito)
                    .HasColumnName("CapacidadeDeposito");

                entity.Property(e => e.Autonomia)
                    .HasColumnName("Autonomia");

                entity.Property(e => e.PesoBruto)
                    .HasColumnName("PesoBruto");

                entity.Property(e => e.CargaMaxima)
                    .HasColumnName("CargaMaxima");

                entity.Property(e => e.Tara)
                    .HasColumnName("Tara");

                entity.Property(e => e.Potencia)
                    .HasColumnName("Potencia");

                entity.Property(e => e.DistanciaEixos)
                    .HasColumnName("DistanciaEixos")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.NoLugares)
                    .HasColumnName("NoLugares");

                entity.Property(e => e.NoAnosGarantia)
                    .HasColumnName("NoAnosGarantia");

                entity.Property(e => e.NoQuadro)
                    .HasColumnName("NoQuadro")
                    .HasMaxLength(50);

                entity.Property(e => e.IDTipoCaixa)
                    .HasColumnName("IDTipoCaixa");

                entity.Property(e => e.PneuFrente)
                    .HasColumnName("PneuFrente")
                    .HasMaxLength(50);

                entity.Property(e => e.PneuRetaguarda)
                    .HasColumnName("PneuRetaguarda")
                    .HasMaxLength(50);

                entity.Property(e => e.Observacoes)
                    .HasColumnName("Observacoes")
                    .HasMaxLength(200);

                entity.Property(e => e.NomeImagem)
                    .HasColumnName("NomeImagem")
                    .HasMaxLength(200);

                entity.Property(e => e.DataEstado)
                    .HasColumnName("DataEstado")
                    .HasColumnType("date");

                entity.Property(e => e.IDTipoPropriedade)
                    .HasColumnName("IDTipoPropriedade");

                entity.Property(e => e.IDPropriedade)
                    .HasColumnName("IDPropriedade");

                entity.Property(e => e.IDSegmentacao)
                    .HasColumnName("IDSegmentacao");

                entity.Property(e => e.DataProximaInspecao)
                    .HasColumnName("DataProximaInspecao")
                    .HasColumnType("date");

                entity.Property(e => e.IntervaloRevisoes)
                    .HasColumnName("IntervaloRevisoes");

                entity.Property(e => e.IDLocalParqueamento)
                    .HasColumnName("IDLocalParqueamento");

                entity.Property(e => e.AlvaraLicenca)
                    .HasColumnName("AlvaraLicenca");

                entity.Property(e => e.CodRegiao)
                    .HasColumnName("CodRegiao")
                    .HasMaxLength(20);

                entity.Property(e => e.CodAreaFuncional)
                    .HasColumnName("CodAreaFuncional")
                    .HasMaxLength(20);

                entity.Property(e => e.CodCentroResponsabilidade)
                    .HasColumnName("CodCentroResponsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.NoProjeto)
                    .HasColumnName("NoProjeto")
                    .HasMaxLength(20);

                entity.Property(e => e.DataAquisicao)
                    .HasColumnName("DataAquisicao")
                    .HasColumnType("date");

                entity.Property(e => e.IDGestor)
                    .HasColumnName("IDGestor");

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<ConfiguracaoTabelas>(entity =>
            {
                entity.HasKey(e => new { e.Tabela, e.ID });

                entity.ToTable("Configuracao Tabelas");

                entity.Property(e => e.Tabela)
                    .HasColumnName("Tabela")
                    .HasMaxLength(50);

                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                entity.Property(e => e.Descricao)
                    .HasColumnName("Descricao")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Viaturas2Marcas>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Viaturas2_Marcas");

                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                    entity.Property(e => e.Marca)
                    .HasColumnName("Marca")
                    .HasMaxLength(60);

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Viaturas2Modelos>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Viaturas2_Modelos");

                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                entity.Property(e => e.IDMarca)
                    .HasColumnName("IDMarca");

                entity.Property(e => e.Modelo)
                    .HasColumnName("Modelo")
                    .HasMaxLength(60);

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Viaturas2Parqueamento>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Viaturas2_Parqueamento");

                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                entity.Property(e => e.Matricula)
                .HasColumnName("Matricula")
                .HasMaxLength(20);

                entity.Property(e => e.IDLocal)
                .HasColumnName("IDLocal");

                entity.Property(e => e.DataInicio)
                .HasColumnName("DataInicio")
                .HasColumnType("date");

                entity.Property(e => e.DataFim)
                .HasColumnName("DataFim")
                .HasColumnType("date");

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Viaturas2ParqueamentoLocal>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Viaturas2_Parqueamento_Local");

                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                entity.Property(e => e.Local)
                .HasColumnName("Local")
                .HasMaxLength(200);

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Viaturas2Inspecoes>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Viaturas2_Inspecao");

                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                entity.Property(e => e.Matricula)
                    .HasColumnName("Matricula")
                    .HasMaxLength(20);

                entity.Property(e => e.DataInspecao)
                    .HasColumnName("DataInspecao")
                    .HasColumnType("date");

                entity.Property(e => e.KmInspecao)
                    .HasColumnName("KmInspecao")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.IDResultado)
                    .HasColumnName("IDResultado");

                entity.Property(e => e.ProximaInspecao)
                    .HasColumnName("ProximaInspecao")
                    .HasColumnType("date");

                entity.Property(e => e.Observacao)
                    .HasColumnName("Observacao")
                    .HasMaxLength(200);

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Viaturas2Gestores>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Viaturas2_Gestores");

                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                entity.Property(e => e.Matricula)
                    .HasColumnName("Matricula")
                    .HasMaxLength(20);

                entity.Property(e => e.IDTipo)
                    .HasColumnName("IDTipo");

                entity.Property(e => e.IDGestor)
                    .HasColumnName("IDGestor");

                entity.Property(e => e.DataInicio)
                    .HasColumnName("DataInicio")
                    .HasColumnType("date");

                entity.Property(e => e.DataFim)
                    .HasColumnName("DataFim")
                    .HasColumnType("date");

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Viaturas2GestoresGestor>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Viaturas2_Gestores_Gestor");

                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                entity.Property(e => e.Gestor)
                    .HasColumnName("Gestor")
                    .HasMaxLength(200);

                entity.Property(e => e.SearchName)
                    .HasColumnName("SearchName")
                    .HasMaxLength(200);

                entity.Property(e => e.NoMecanografico)
                    .HasColumnName("NoMecanografico")
                    .HasMaxLength(20);

                entity.Property(e => e.Mail)
                    .HasColumnName("Mail")
                    .HasMaxLength(50);

                entity.Property(e => e.IDTipo)
                    .HasColumnName("IDTipo");

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Viaturas2Estados>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Viaturas2_Estados");

                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                entity.Property(e => e.Matricula)
                    .HasColumnName("Matricula")
                    .HasMaxLength(20);

                entity.Property(e => e.IDEstado)
                    .HasColumnName("IDEstado");

                entity.Property(e => e.DataInicio)
                    .HasColumnName("DataInicio")
                    .HasColumnType("date");

                entity.Property(e => e.DataFim)
                    .HasColumnName("DataFim")
                    .HasColumnType("date");

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Viaturas2Dimensoes>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Viaturas2_Dimensoes");

                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                entity.Property(e => e.Matricula)
                    .HasColumnName("Matricula")
                    .HasMaxLength(20);

                entity.Property(e => e.Regiao)
                    .HasColumnName("Regiao")
                    .HasMaxLength(20);

                entity.Property(e => e.Area)
                    .HasColumnName("Area")
                    .HasMaxLength(20);

                entity.Property(e => e.Cresp)
                    .HasColumnName("Cresp")
                    .HasMaxLength(20);

                entity.Property(e => e.DataInicio)
                    .HasColumnName("DataInicio")
                    .HasColumnType("date");

                entity.Property(e => e.DataFim)
                    .HasColumnName("DataFim")
                    .HasColumnType("date");

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Viaturas2Propriedades>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Viaturas2_Propriedades");

                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                entity.Property(e => e.Matricula)
                    .HasColumnName("Matricula")
                    .HasMaxLength(20);

                entity.Property(e => e.IDTipoPropriedade)
                    .HasColumnName("IDTipoPropriedade");

                entity.Property(e => e.IDPropriedade)
                    .HasColumnName("IDPropriedade");

                entity.Property(e => e.DataInicio)
                    .HasColumnName("DataInicio")
                    .HasColumnType("date");

                entity.Property(e => e.DataFim)
                    .HasColumnName("DataFim")
                    .HasColumnType("date");

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Viaturas2Km>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Viaturas2_Km");

                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                entity.Property(e => e.Matricula)
                    .HasColumnName("Matricula")
                    .HasMaxLength(20);

                entity.Property(e => e.Km)
                    .HasColumnName("Km")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Data)
                    .HasColumnName("Data")
                    .HasColumnType("date");

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Viaturas2Manutencao>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Viaturas2_Manutencao");

                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                entity.Property(e => e.Matricula)
                    .HasColumnName("Matricula")
                    .HasMaxLength(20);

                entity.Property(e => e.Local)
                    .HasColumnName("Local");

                entity.Property(e => e.DataInicio)
                    .HasColumnName("DataInicio")
                    .HasColumnType("date");

                entity.Property(e => e.DataFim)
                    .HasColumnName("DataFim")
                    .HasColumnType("date");

                entity.Property(e => e.Observacoes)
                    .HasColumnName("Observacoes")
                    .HasMaxLength(2000);

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Viaturas2CartaVerde>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Viaturas2_CartaVerde");

                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                entity.Property(e => e.Matricula)
                    .HasColumnName("Matricula")
                    .HasMaxLength(20);

                entity.Property(e => e.NoApolice)
                    .HasColumnName("NoApolice")
                    .HasMaxLength(200);

                entity.Property(e => e.IDSeguradora)
                    .HasColumnName("IDSeguradora");

                entity.Property(e => e.DataInicio)
                    .HasColumnName("DataInicio")
                    .HasColumnType("date");

                entity.Property(e => e.DataFim)
                    .HasColumnName("DataFim")
                    .HasColumnType("date");

                entity.Property(e => e.ValorPremioSeguro)
                    .HasColumnName("ValorPremioSeguro")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Viaturas2CartaVerdeSeguradora>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Viaturas2_CartaVerde_Seguradora");

                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                entity.Property(e => e.Seguradora)
                    .HasColumnName("Seguradora")
                    .HasMaxLength(200);

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Viaturas2Acidentes>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Viaturas2_Acidentes");

                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                entity.Property(e => e.Matricula)
                    .HasColumnName("Matricula")
                    .HasMaxLength(20);

                entity.Property(e => e.Local)
                    .HasColumnName("Local")
                    .HasMaxLength(200);

                entity.Property(e => e.Data)
                    .HasColumnName("Data")
                    .HasColumnType("date");

                entity.Property(e => e.IDCondutor)
                    .HasColumnName("IDCondutor");

                entity.Property(e => e.IDResponsabilidade)
                    .HasColumnName("IDResponsabilidade");

                entity.Property(e => e.Observacoes)
                    .HasColumnName("Observacoes")
                    .HasMaxLength(2000);

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Viaturas2ContraOrdenacoes>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Viaturas2_ContraOrdenacoes");

                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                entity.Property(e => e.Matricula)
                    .HasColumnName("Matricula")
                    .HasMaxLength(20);

                entity.Property(e => e.Local)
                    .HasColumnName("Local")
                    .HasMaxLength(200);

                entity.Property(e => e.Data)
                    .HasColumnName("Data")
                    .HasColumnType("date");

                entity.Property(e => e.IDCondutor)
                    .HasColumnName("IDCondutor");

                entity.Property(e => e.IDResponsabilidade)
                    .HasColumnName("IDResponsabilidade");

                entity.Property(e => e.Valor)
                    .HasColumnName("Valor")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Observacoes)
                    .HasColumnName("Observacoes")
                    .HasMaxLength(2000);

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Viaturas2Afetacao>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Viaturas2_Afetacao");

                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                entity.Property(e => e.Matricula)
                    .HasColumnName("Matricula")
                    .HasMaxLength(20);

                entity.Property(e => e.IDAreaReal)
                    .HasColumnName("IDAreaReal");

                entity.Property(e => e.LocalExato)
                    .HasColumnName("LocalExato")
                    .HasMaxLength(200);

                entity.Property(e => e.DataInicio)
                    .HasColumnName("DataInicio")
                    .HasColumnType("date");

                entity.Property(e => e.DataFim)
                    .HasColumnName("DataFim")
                    .HasColumnType("date");

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Viaturas2AfetacaoAreaReal>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Viaturas2_Afetacao_AreaReal");

                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                entity.Property(e => e.AreaReal)
                    .HasColumnName("AreaReal")
                    .HasMaxLength(200);

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Viaturas2CartaoCombustivel>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Viaturas2_CartaoCombustivel");

                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                entity.Property(e => e.Matricula)
                    .HasColumnName("Matricula")
                    .HasMaxLength(20);

                entity.Property(e => e.IDEmpresa)
                    .HasColumnName("IDEmpresa");

                entity.Property(e => e.NoCartao)
                    .HasColumnName("NoCartao")
                    .HasMaxLength(200);

                entity.Property(e => e.DataInicio)
                    .HasColumnName("DataInicio")
                    .HasColumnType("date");

                entity.Property(e => e.DataFim)
                    .HasColumnName("DataFim")
                    .HasColumnType("date");

                entity.Property(e => e.Plafon)
                    .HasColumnName("Plafon")
                    .HasMaxLength(200);

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Viaturas2CarTrack>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Viaturas2_CarTrack");

                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                entity.Property(e => e.Matricula)
                    .HasColumnName("Matricula")
                    .HasMaxLength(20);

                entity.Property(e => e.IDEmpresa)
                    .HasColumnName("IDEmpresa");

                entity.Property(e => e.DataInicio)
                    .HasColumnName("DataInicio")
                    .HasColumnType("date");

                entity.Property(e => e.DataFim)
                    .HasColumnName("DataFim")
                    .HasColumnType("date");

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Viaturas2ViaVerde>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Viaturas2_ViaVerde");

                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                entity.Property(e => e.Matricula)
                    .HasColumnName("Matricula")
                    .HasMaxLength(20);

                entity.Property(e => e.IDEmpresa)
                    .HasColumnName("IDEmpresa");

                entity.Property(e => e.NoIdentificador)
                    .HasColumnName("NoIdentificador")
                    .HasMaxLength(200);

                entity.Property(e => e.NoContrato)
                    .HasColumnName("NoContrato")
                    .HasMaxLength(200);

                entity.Property(e => e.Data)
                    .HasColumnName("Data")
                    .HasColumnType("date");

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Viaturas2Abastecimentos>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Viaturas2_Abastecimentos");

                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                entity.Property(e => e.Matricula)
                    .HasColumnName("Matricula")
                    .HasMaxLength(20);

                entity.Property(e => e.Data)
                    .HasColumnName("Data")
                    .HasColumnType("date");

                entity.Property(e => e.Kms)
                    .HasColumnName("Kms")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Litros)
                    .HasColumnName("Litros")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PrecoUnitario)
                    .HasColumnName("PrecoUnitario")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.PrecoTotal)
                    .HasColumnName("PrecoTotal")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.IDCombustivel)
                    .HasColumnName("IDCombustivel");

                entity.Property(e => e.IDEmpresa)
                    .HasColumnName("IDEmpresa");

                entity.Property(e => e.IDCartao)
                    .HasColumnName("IDCartao");

                entity.Property(e => e.Local)
                    .HasColumnName("Local")
                    .HasMaxLength(200);

                entity.Property(e => e.IDCondutor)
                    .HasColumnName("IDCondutor");

                entity.Property(e => e.NoDocumento)
                    .HasColumnName("NoDocumento")
                    .HasMaxLength(200);

                entity.Property(e => e.Observacoes)
                    .HasColumnName("Observacoes")
                    .HasMaxLength(2000);

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Viaturas2Abate>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Viaturas2_Abate");

                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                entity.Property(e => e.Matricula)
                    .HasColumnName("Matricula")
                    .HasMaxLength(20);

                entity.Property(e => e.IDTipoAtoAdministrativo)
                    .HasColumnName("IDTipoAtoAdministrativo");

                entity.Property(e => e.NoRegisto)
                    .HasColumnName("NoRegisto")
                    .HasMaxLength(200);

                entity.Property(e => e.IDDescricaoAto)
                    .HasColumnName("IDDescricaoAto");

                entity.Property(e => e.Fundamentacao)
                    .HasColumnName("Fundamentacao")
                    .HasMaxLength(200);

                entity.Property(e => e.Autor)
                    .HasColumnName("Autor")
                    .HasMaxLength(200);

                entity.Property(e => e.Data)
                    .HasColumnName("Data")
                    .HasColumnType("date");

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Viaturas2Imobilizados>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Viaturas2_Imobilizados");

                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                entity.Property(e => e.Matricula)
                    .HasColumnName("Matricula")
                    .HasMaxLength(20);

                entity.Property(e => e.NoImobilizado)
                    .HasColumnName("NoImobilizado")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Viaturas2RentingContrato>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Viaturas2_RentingContrato");

                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                entity.Property(e => e.Matricula)
                    .HasColumnName("Matricula")
                    .HasMaxLength(20);

                entity.Property(e => e.IDFornecedor)
                    .HasColumnName("IDFornecedor");

                entity.Property(e => e.NoContrato)
                    .HasColumnName("NoContrato")
                    .HasMaxLength(60);

                entity.Property(e => e.KmInicio)
                    .HasColumnName("KmInicio")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.KmContratados)
                    .HasColumnName("KmContratados")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PrecoKmAdicionalSemIVA)
                    .HasColumnName("PrecoKmAdicionalSemIVA")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.PrecoKmNaoPercorridoSemIVA)
                    .HasColumnName("PrecoKmNaoPercorridoSemIVA")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.KmMaximo)
                    .HasColumnName("KmMaximo")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.DataInicio)
                    .HasColumnName("DataInicio")
                    .HasColumnType("date");

                entity.Property(e => e.DataTermo)
                    .HasColumnName("DataTermo")
                    .HasColumnType("date");

                entity.Property(e => e.DuracaoContratoMensal)
                    .HasColumnName("DuracaoContratoMensal");

                entity.Property(e => e.InicioPagamento)
                    .HasColumnName("InicioPagamento")
                    .HasColumnType("date");

                entity.Property(e => e.NoPagamentos)
                    .HasColumnName("NoPagamentos");

                entity.Property(e => e.IDPeriodicidade)
                    .HasColumnName("IDPeriodicidade");

                entity.Property(e => e.TotalSemIVA)
                    .HasColumnName("TotalSemIVA")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.IVA)
                    .HasColumnName("IVA")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalComIVA)
                    .HasColumnName("TotalComIVA")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FranquiaSemIVA)
                    .HasColumnName("FranquiaSemIVA")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ResponsavelContrato)
                    .HasColumnName("ResponsavelContrato")
                    .HasMaxLength(200);

                entity.Property(e => e.Observacoes)
                    .HasColumnName("Observacoes")
                    .HasMaxLength(200);

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Viaturas2RentingContratoAlteracoes>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Viaturas2_RentingContrato_Alteracoes");

                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                entity.Property(e => e.Matricula)
                    .HasColumnName("Matricula")
                    .HasMaxLength(20);

                entity.Property(e => e.IDContrato)
                    .HasColumnName("IDContrato");

                entity.Property(e => e.DataPedido)
                    .HasColumnName("DataPedido")
                    .HasColumnType("date");

                entity.Property(e => e.AutorizadoFINLOG)
                    .HasColumnName("AutorizadoFINLOG")
                    .HasMaxLength(200);

                entity.Property(e => e.DataAlteracaoKm)
                    .HasColumnName("DataAlteracaoKm")
                    .HasColumnType("date");

                entity.Property(e => e.KmAlteracao)
                    .HasColumnName("KmAlteracao")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.MensalidadeAlteracao)
                    .HasColumnName("MensalidadeAlteracao")
                    .HasMaxLength(200);

                entity.Property(e => e.Observacoes)
                    .HasColumnName("Observacoes")
                    .HasMaxLength(200);

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Viaturas2Substituicao>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Viaturas2_Substituicao");

                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                entity.Property(e => e.Matricula)
                    .HasColumnName("Matricula")
                    .HasMaxLength(20);

                entity.Property(e => e.MatriculaSubstituicao)
                    .HasColumnName("MatriculaSubstituicao")
                    .HasMaxLength(20);

                entity.Property(e => e.DataInicio)
                    .HasColumnName("DataInicio")
                    .HasColumnType("date");

                entity.Property(e => e.KmInicio)
                    .HasColumnName("KmInicio")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.DataFim)
                    .HasColumnName("DataFim")
                    .HasColumnType("date");

                entity.Property(e => e.KmFim)
                    .HasColumnName("KmFim")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Observacoes)
                    .HasColumnName("Observacoes")
                    .HasMaxLength(200);

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<WorkflowProcedimentosCcp>(entity =>
            {
                entity.HasKey(e => new { e.NºProcedimento, e.Estado, e.DataHora });

                entity.ToTable("Workflow Procedimentos CCP");

                entity.Property(e => e.NºProcedimento)
                    .HasColumnName("Nº Procedimento")
                    .HasMaxLength(10);

                entity.Property(e => e.DataHora)
                    .HasColumnName("Data/Hora")
                    .HasColumnType("datetime");

                entity.Property(e => e.Comentário).HasColumnType("text");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataResposta)
                    .HasColumnName("Data Resposta")
                    .HasColumnType("date");

                entity.Property(e => e.EstadoAnterior).HasColumnName("Estado Anterior");

                entity.Property(e => e.EstadoSeguinte).HasColumnName("Estado Seguinte");

                entity.Property(e => e.Resposta).HasColumnType("text");

                entity.Property(e => e.TipoEstado).HasColumnName("Tipo Estado");

                entity.Property(e => e.TipoResposta).HasColumnName("Tipo Resposta");

                entity.Property(e => e.Utilizador).HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.NºProcedimentoNavigation)
                    .WithMany(p => p.WorkflowProcedimentosCcp)
                    .HasForeignKey(d => d.NºProcedimento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Workflow Procedimentos CCP_Procedimentos CCP");
            });

            modelBuilder.Entity<PedidosPagamento>(entity =>
            {
                entity.HasKey(e => new { e.NoPedido });

                entity.ToTable("Pedidos Pagamento");

                entity.Property(e => e.Data)
                    .HasColumnName("Data")
                    .HasColumnType("date");

                entity.Property(e => e.DataPedido)
                    .HasColumnName("DataPedido")
                    .HasColumnType("date");

                entity.Property(e => e.DataAprovacao)
                    .HasColumnName("DataAprovacao")
                    .HasColumnType("date");

                entity.Property(e => e.DataDisponibilizacao)
                    .HasColumnName("DataDisponibilizacao")
                    .HasColumnType("date");

                entity.Property(e => e.DataEnvioAprovacao)
                    .HasColumnName("DataEnvioAprovacao")
                    .HasColumnType("date");

                entity.Property(e => e.DataValidacao)
                    .HasColumnName("DataValidacao")
                    .HasColumnType("date");

                entity.Property(e => e.DataArquivo)
                    .HasColumnName("DataArquivo")
                    .HasColumnType("date");

                entity.Property(e => e.DataPrioridade)
                    .HasColumnName("DataPrioridade")
                    .HasColumnType("date");

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.NoEncomenda)
                    .HasColumnName("NoEncomenda")
                    .HasMaxLength(20);

                entity.Property(e => e.CodigoFornecedor)
                    .HasColumnName("CodigoFornecedor")
                    .HasMaxLength(20);

                entity.Property(e => e.Fornecedor)
                    .HasColumnName("Fornecedor")
                    .HasMaxLength(60);

                entity.Property(e => e.NIB)
                    .HasColumnName("NIB")
                    .HasMaxLength(21);

                entity.Property(e => e.IBAN)
                    .HasColumnName("IBAN")
                    .HasMaxLength(50);

                entity.Property(e => e.UserPedido)
                    .HasColumnName("UserPedido")
                    .HasMaxLength(50);

                entity.Property(e => e.UserAprovacao)
                    .HasColumnName("UserAprovacao")
                    .HasMaxLength(50);

                entity.Property(e => e.UserFinanceiros)
                    .HasColumnName("UserFinanceiros")
                    .HasMaxLength(50);

                entity.Property(e => e.Descricao)
                    .HasColumnName("Descricao")
                    .HasMaxLength(250);

                entity.Property(e => e.UserValidacao)
                    .HasColumnName("UserValidacao")
                    .HasMaxLength(50);

                entity.Property(e => e.Aprovadores)
                    .HasColumnName("Aprovadores")
                    .HasMaxLength(150);

                entity.Property(e => e.UserLiquidado)
                    .HasColumnName("UserLiquidado")
                    .HasMaxLength(50);

                entity.Property(e => e.DataLiquidado)
                    .HasColumnName("DataLiquidado")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserArquivo)
                    .HasColumnName("UserArquivo")
                    .HasMaxLength(50);

                entity.Property(e => e.MotivoAnulacao)
                    .HasColumnName("MotivoAnulacao")
                    .HasMaxLength(100);

                entity.Property(e => e.NumeroTransferencia)
                    .HasColumnName("NumeroTransferencia")
                    .HasMaxLength(20);

                entity.Property(e => e.RegiaoMercadoLocal)
                    .HasColumnName("RegiaoMercadoLocal")
                    .HasMaxLength(20);

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.Valor)
                    .HasColumnName("Valor")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.ValorEncomenda)
                    .HasColumnName("ValorEncomenda")
                    .HasColumnType("decimal(18, 4)");
            });

            modelBuilder.Entity<TabelaLog>(entity =>
            {
                entity.HasKey(e => new { e.ID });

                entity.ToTable("Tabela_Log");

                entity.Property(e => e.Tabela)
                    .HasColumnName("Tabela")
                    .HasMaxLength(100);

                entity.Property(e => e.Descricao)
                    .HasColumnName("Descricao")
                    .HasMaxLength(250);

                entity.Property(e => e.Utilizador)
                    .HasColumnName("Utilizador")
                    .HasMaxLength(50);

                entity.Property(e => e.DataHora)
                    .HasColumnName("DataHora")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<PedidosDEV>(entity =>
            {
                entity.HasKey(e => new { e.ID });

                entity.ToTable("PedidosDEV");

                entity.Property(e => e.Processo)
                    .HasColumnName("Processo")
                    .HasMaxLength(2000);

                entity.Property(e => e.Descricao)
                    .HasColumnName("Descricao")
                    .HasMaxLength(2000);

                entity.Property(e => e.Acao)
                    .HasColumnName("Acao")
                    .HasMaxLength(2000);

                entity.Property(e => e.URL)
                    .HasColumnName("URL")
                    .HasMaxLength(2000);

                entity.Property(e => e.PedidoPor)
                    .HasColumnName("PedidoPor")
                    .HasMaxLength(2000);

                entity.Property(e => e.Intervenientes)
                    .HasColumnName("Intervenientes")
                    .HasMaxLength(2000);

                entity.Property(e => e.CriadoPor)
                    .HasColumnName("CriadoPor")
                    .HasMaxLength(50);

                entity.Property(e => e.AlteradoPor)
                    .HasColumnName("AlteradoPor")
                    .HasMaxLength(50);

                entity.Property(e => e.DataEstado)
                    .HasColumnName("DataEstado")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataPedido)
                    .HasColumnName("DataPedido")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataConclusao)
                    .HasColumnName("DataConclusao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataAlteracao)
                    .HasColumnName("DataAlteracao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<ConfigLinhasEncFornecedor>(entity =>
            {
                entity.HasKey(e => new { e.VendorNo, e.LineNo });

                entity.ToTable("Config Linhas Enc Fornecedor");

                entity.Property(e => e.VendorNo)
                    .HasColumnName("VendorNo")
                    .HasMaxLength(20);

                entity.Property(e => e.LineNo)
                    .HasColumnName("LineNo");

                entity.Property(e => e.Type)
                    .HasColumnName("Type");

                entity.Property(e => e.No)
                    .HasColumnName("No")
                    .HasMaxLength(20);

                entity.Property(e => e.Description)
                    .HasColumnName("Description")
                    .HasMaxLength(50);

                entity.Property(e => e.Description2)
                    .HasColumnName("Description2")
                    .HasMaxLength(50);

                entity.Property(e => e.Quantity)
                    .HasColumnName("Quantity")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.UnitOfMeasure)
                    .HasColumnName("UnitOfMeasure")
                    .HasMaxLength(10);

                entity.Property(e => e.Valor)
                    .HasColumnName("Valor")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.DataHoraCriacao)
                    .HasColumnName("DataHoraCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataHoraModificacao)
                    .HasColumnName("DataHoraModificacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Orcamentos>(entity =>
            {
                entity.HasKey(e => new { e.No });

                entity.ToTable("Orcamentos");

                entity.Property(e => e.No)
                    .HasColumnName("No")
                    .HasMaxLength(20);

                entity.Property(e => e.NoCliente)
                    .HasColumnName("NoCliente")
                    .HasMaxLength(20);

                entity.Property(e => e.NoContacto)
                    .HasColumnName("NoContacto")
                    .HasMaxLength(20);

                entity.Property(e => e.ContactoNome)
                    .HasColumnName("ContactoNome")
                    .HasMaxLength(100);

                entity.Property(e => e.ContactoTelefone)
                    .HasColumnName("ContactoTelefone")
                    .HasMaxLength(50);

                entity.Property(e => e.ContactoEmail)
                    .HasColumnName("ContactoEmail")
                    .HasMaxLength(100);

                entity.Property(e => e.ContactoNotas)
                    .HasColumnName("ContactoNotas")
                    .HasMaxLength(200);

                entity.Property(e => e.DataValidade)
                    .HasColumnName("DataValidade")
                    .HasColumnType("datetime");

                entity.Property(e => e.IDEstado);

                entity.Property(e => e.Descricao)
                    .HasColumnName("Descricao")
                    .HasMaxLength(200);

                entity.Property(e => e.CodRegiao)
                    .HasColumnName("CodRegiao")
                    .HasMaxLength(20);

                entity.Property(e => e.UnidadePrestacao);

                entity.Property(e => e.TipoFaturacao);

                entity.Property(e => e.CondicoesPagamento);

                entity.Property(e => e.TotalSemIVA)
                    .HasColumnName("TotalSemIVA")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TotalComIVA)
                    .HasColumnName("TotalComIVA")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.NoProposta)
                    .HasColumnName("NoProposta")
                    .HasMaxLength(20);

                entity.Property(e => e.ProjetoAssociado)
                    .HasColumnName("ProjetoAssociado")
                    .HasMaxLength(20);

                entity.Property(e => e.Email)
                    .HasColumnName("Email")
                    .HasMaxLength(50);

                entity.Property(e => e.EmailAssunto)
                    .HasColumnName("EmailAssunto")
                    .HasMaxLength(100);

                entity.Property(e => e.EmailCorpo)
                    .HasColumnName("EmailCorpo")
                    .HasMaxLength(200);

                entity.Property(e => e.EmailDataEnvio)
                    .HasColumnName("EmailDataEnvio")
                    .HasColumnType("datetime");

                entity.Property(e => e.EmailUtilizadorEnvio)
                    .HasColumnName("EmailUtilizadorEnvio")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataAceite)
                    .HasColumnName("DataAceite")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorAceite)
                    .HasColumnName("UtilizadorAceite")
                    .HasMaxLength(50);

                entity.Property(e => e.DataNaoAceite)
                    .HasColumnName("DataNaoAceite")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorNaoAceite)
                    .HasColumnName("UtilizadorNaoAceite")
                    .HasMaxLength(50);

                entity.Property(e => e.DataConcluido)
                    .HasColumnName("DataConcluido")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorConcluido)
                    .HasColumnName("UtilizadorConcluido")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<LinhasOrcamentos>(entity =>
            {
                entity.HasKey(e => new { e.NoLinha });

                entity.ToTable("Linhas Orcamentos");

                entity.Property(e => e.NoLinha);

                entity.Property(e => e.OrcamentosNo)
                    .HasColumnName("OrcamentosNo")
                    .HasMaxLength(20);

                entity.Property(e => e.Descricao)
                    .HasColumnName("Descricao")
                    .HasMaxLength(100);

                entity.Property(e => e.Quantidade)
                    .HasColumnName("Quantidade")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.ValorUnitario)
                    .HasColumnName("ValorUnitario")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TaxaIVA)
                    .HasColumnName("TaxaIVA")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TotalLinha)
                    .HasColumnName("TotalLinha")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<OrcamentosContatos>(entity =>
            {
                entity.HasKey(e => new { e.ID });

                entity.ToTable("OrcamentosContatos");

                entity.Property(e => e.ID)
                    .HasColumnName("ID")
                    .HasMaxLength(20);

                entity.Property(e => e.Organizacao)
                    .HasColumnName("Organizacao")
                    .HasMaxLength(100);

                entity.Property(e => e.Nome)
                    .HasColumnName("Nome")
                    .HasMaxLength(100);

                entity.Property(e => e.Telemovel)
                    .HasColumnName("Telemovel")
                    .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .HasColumnName("Email")
                    .HasMaxLength(100);

                entity.Property(e => e.NIF)
                    .HasColumnName("NIF")
                    .HasMaxLength(20);

                entity.Property(e => e.Notas)
                    .HasColumnName("Notas")
                    .HasMaxLength(200);

                entity.Property(e => e.CriadoPor)
                    .HasColumnName("CriadoPor")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.AlteradoPor)
                    .HasColumnName("AlteradoPor")
                    .HasMaxLength(50);

                entity.Property(e => e.DataAlteracao)
                    .HasColumnName("DataAlteracao")
                    .HasColumnType("datetime");

            });

            modelBuilder.Entity<ConfiguracaoParametros>(entity =>
            {
                entity.HasKey(e => new { e.ID });

                entity.ToTable("Configuracao Parametros");

                entity.Property(e => e.ID);

                entity.Property(e => e.Parametro)
                    .HasColumnName("Parametro")
                    .HasMaxLength(20);

                entity.Property(e => e.Descricao)
                    .HasColumnName("Descricao")
                    .HasMaxLength(500);

                entity.Property(e => e.Valor)
                    .HasColumnName("Valor")
                    .HasMaxLength(50);

                entity.Property(e => e.Ordem)
                    .HasColumnName("Ordem");

                entity.Property(e => e.DataHoraCriacao)
                    .HasColumnName("DataHoraCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataHoraModificacao)
                    .HasColumnName("DataHoraModificacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SISLOGReservas>(entity =>
            {
                entity.HasKey(e => new { e.NoRequisicao, e.NoLinha });

                entity.ToTable("SISLOG_Reservas");

                entity.Property(e => e.NoRequisicao)
                    .HasColumnName("NoRequisicao")
                    .HasMaxLength(20);

                entity.Property(e => e.NoLinha)
                    .HasColumnName("NoLinha");

                entity.Property(e => e.NoProduto)
                    .HasColumnName("NoProduto")
                    .HasMaxLength(20);

                entity.Property(e => e.QuantidadeReserva)
                    .HasColumnName("QuantidadeReserva")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.DataInicioReserva)
                    .HasColumnName("DataInicioReserva")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataFimReserva)
                    .HasColumnName("DataFimReserva")
                    .HasColumnType("datetime");

                entity.Property(e => e.Reservado)
                    .HasColumnName("Reservado")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataHoraCriacao)
                    .HasColumnName("DataHoraCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataHoraModificacao)
                    .HasColumnName("DataHoraModificacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<SISLOGProdutos>(entity =>
            {
                entity.HasKey(e => new { e.Codigo });

                entity.ToTable("SISLOG_Produtos");

                entity.Property(e => e.Codigo)
                    .HasColumnName("Codigo")
                    .HasMaxLength(20);

                entity.Property(e => e.Quantidade)
                    .HasColumnName("Quantidade")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.QtdDisponivel)
                    .HasColumnName("QtdDisponivel")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.QtdPendente)
                    .HasColumnName("QtdPendente")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.QtdPendenteRececao)
                    .HasColumnName("QtdPendenteRececao")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.DataAtualizacao)
                    .HasColumnName("DataAtualizacao")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Ocorrencias>(entity =>
            {
                entity.HasKey(e => new { e.CodOcorrencia });

                entity.ToTable("Ocorrencias");

                entity.Property(e => e.CodOcorrencia)
                    .HasColumnName("CodOcorrencia");

                entity.Property(e => e.CodEstado)
                    .HasColumnName("CodEstado");

                entity.Property(e => e.CodFornecedor)
                    .HasColumnName("CodFornecedor")
                    .HasMaxLength(20);

                entity.Property(e => e.NomeFornecedor)
                    .HasColumnName("NomeFornecedor")
                    .HasMaxLength(200);

                entity.Property(e => e.CodEncomenda)
                    .HasColumnName("CodEncomenda")
                    .HasMaxLength(20);

                entity.Property(e => e.CodRegiao)
                    .HasColumnName("CodRegiao")
                    .HasMaxLength(20);

                entity.Property(e => e.CodAreaFuncional)
                    .HasColumnName("CodAreaFuncional")
                    .HasMaxLength(20);

                entity.Property(e => e.CodCentroResponsabilidade)
                    .HasColumnName("CodCentroResponsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.DataOcorrencia)
                    .HasColumnName("DataOcorrencia")
                    .HasColumnType("datetime");

                entity.Property(e => e.LocalEntrega)
                    .HasColumnName("LocalEntrega")
                    .HasMaxLength(200);

                entity.Property(e => e.NoDocExterno)
                    .HasColumnName("NoDocExterno")
                    .HasMaxLength(20);

                entity.Property(e => e.CodArtigo)
                    .HasColumnName("CodArtigo")
                    .HasMaxLength(20);

                entity.Property(e => e.Descricao)
                    .HasColumnName("Descricao")
                    .HasMaxLength(200);

                entity.Property(e => e.UnidMedida)
                    .HasColumnName("UnidMedida")
                    .HasMaxLength(20);

                entity.Property(e => e.Quantidade)
                    .HasColumnName("Quantidade")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.Motivo)
                    .HasColumnName("Motivo")
                    .HasMaxLength(200);

                entity.Property(e => e.GrauGravidade)
                    .HasColumnName("GrauGravidade");

                entity.Property(e => e.Observacao)
                    .HasColumnName("Observacao")
                    .HasMaxLength(200);

                entity.Property(e => e.MedidaCorretiva)
                    .HasColumnName("MedidaCorretiva")
                    .HasMaxLength(2000);

                entity.Property(e => e.UtilizadorMedidaCorretiva)
                    .HasColumnName("UtilizadorMedidaCorretiva")
                    .HasMaxLength(50);

                entity.Property(e => e.DataMedidaCorretiva)
                    .HasColumnName("DataMedidaCorretiva")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DataCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("UtilizadorModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataModificacao)
                    .HasColumnName("DataModificacao")
                    .HasColumnType("datetime");
            });

            #region SGPPF
            modelBuilder.Entity<AccaoFormacao>(entity =>
            {
                entity.HasKey(e => new { e.IdAccao });

                entity.Property(e => e.IdAccao)
                    .HasColumnName("IdAccao")
                    .HasMaxLength(50);

                entity.Property(e => e.CodigoInterno)
                    .HasColumnName("CodigoInterno")
                    .HasMaxLength(50);

                entity.Property(e => e.DesignacaoAccao)
                    .HasColumnName("DesignacaoAccao")
                    .HasMaxLength(2048);

                entity.Property(e => e.IdTema)
                    .HasColumnName("IdTema")
                    .HasMaxLength(50);

                entity.Property(e => e.Activa)
                    .HasColumnName("Activa")
                    .HasColumnType("tinyint");

                entity.Property(e => e.DataInicio)
                    .HasColumnName("DataInicio")
                    .HasColumnType("date");

                entity.Property(e => e.DataFim)
                    .HasColumnName("DataFim")
                    .HasColumnType("date");

                entity.Property(e => e.IdEntidadeFormadora)
                    .HasColumnName("IdEntidadeFormadora")
                    .HasMaxLength(50);

                entity.Property(e => e.NumeroTotalHoras)
                    .HasColumnName("NumeroTotalHoras")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.LocalRealizacao)
                    .HasColumnName("LocalRealizacao")
                    .HasMaxLength(150);

                entity.Property(e => e.UrlImagem)
                    .HasColumnName("UrlImagem")
                    .HasMaxLength(250);

                entity.HasOne(a => a.Tema)
                    .WithMany(t => t.AccoesTema)
                    .HasForeignKey(a => a.IdTema)
                    .HasConstraintName("FK_AccaoFormacao_TemaFormacao");

                entity.HasOne(a => a.Entidade)
                        .WithMany(t => t.AccoesEntidade)
                        .HasForeignKey(a => a.IdEntidadeFormadora)
                        .HasConstraintName("FK_AccaoFormacao_EntidadeFormadora");

            });

            modelBuilder.Entity<EntidadeFormadora>(entity =>
            {
                entity.HasKey(e => new { e.IdEntidade });
                entity.Property(e => e.IdEntidade)
                    .HasColumnName("IdEntidade")
                    .HasMaxLength(50);

                entity.Property(e => e.Referencia)
                    .HasColumnName("Referencia")
                    .HasMaxLength(50);

                entity.Property(e => e.DescricaoEntidade)
                    .HasColumnName("DescricaoEntidade")
                    .HasMaxLength(250);

            });

            modelBuilder.Entity<PedidoParticipacaoFormacao>(entity =>
            {
                entity.HasKey(e => new { e.IdPedido });

                entity.Property(e => e.IdPedido)
                    .HasColumnName("IdPedido")
                    .HasMaxLength(20);

                entity.Property(e => e.Estado)
                    .HasColumnName("Estado")
                    .HasColumnType("int");

                entity.Property(e => e.IdEmpregado)
                    .HasColumnName("IdEmpregado")
                    .HasMaxLength(20);

                entity.Property(e => e.NomeEmpregado)
                    .HasColumnName("NomeEmpregado")
                    .HasMaxLength(500);

                entity.Property(e => e.FuncaoEmpregado)
                    .HasColumnName("FuncaoEmpregado")
                    .HasMaxLength(250);

                entity.Property(e => e.ProjectoEmpregado)
                    .HasColumnName("ProjectoEmpregado")
                    .HasMaxLength(20);

                entity.Property(e => e.IdAreaFuncional)
                    .HasColumnName("IdAreaFuncional")
                    .HasMaxLength(20);

                entity.Property(e => e.AreaFuncionalEmpregado)
                    .HasColumnName("AreaFuncionalEmpregado")
                    .HasMaxLength(250);

                entity.Property(e => e.IdCentroResponsabilidade)
                    .HasColumnName("IdCentroResponsabilidade")
                    .HasMaxLength(50);

                entity.Property(e => e.CentroResponsabilidadeEmpregado)
                    .HasColumnName("CentroResponsabilidadeEmpregado")
                    .HasMaxLength(250);

                entity.Property(e => e.IdEstabelecimento)
                    .HasColumnName("IdEstabelecimento")
                    .HasMaxLength(20);

                entity.Property(e => e.DescricaoEstabelecimento)
                    .HasColumnName("DescricaoEstabelecimento")
                    .HasMaxLength(250);

                entity.Property(e => e.IdAccaoFormacao)
                    .HasColumnName("IdAccaoFormacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DesignacaoAccao)
                    .HasColumnName("DesignacaoAccao")
                    .HasMaxLength(2048);

                entity.Property(e => e.LocalRealizacao)
                    .HasColumnName("LocalRealizacao")
                    .HasMaxLength(150);

                entity.Property(e => e.IdSessao)
                    .HasColumnName("IdSessao")
                    .HasMaxLength(50);

                entity.Property(e => e.Horario)
                    .HasColumnName("Horario")
                    .HasMaxLength(100);

                entity.Property(e => e.DataInicio)
                    .HasColumnName("DataInicio")
                    .HasColumnType("date");

                entity.Property(e => e.DataFim)
                    .HasColumnName("DataFim")
                    .HasColumnType("date");

                entity.Property(e => e.NumeroTotalHoras)
                    .HasColumnName("NumeroTotalHoras")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.IdEntidadeFormadora)
                    .HasColumnName("IdEntidadeFormadora")
                    .HasMaxLength(50);

                entity.Property(e => e.DescricaoEntidadeFormadora)
                    .HasColumnName("DescricaoEntidadeFormadora")
                    .HasMaxLength(250);

                entity.Property(e => e.CustoInscricao)
                    .HasColumnName("CustoInscricao")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ValorIva)
                    .HasColumnName("ValorIva")
                    .HasColumnType("decimal(18, 2");

                entity.Property(e => e.CustoDeslocacoes)
                    .HasColumnName("CustoDeslocacoes")
                    .HasColumnType("decimal(18, 2");

                entity.Property(e => e.CustoEstadia)
                    .HasColumnName("CustoEstadia")
                    .HasColumnType("decimal(18, 2");

                entity.Property(e => e.DescricaoConhecimentos)
                    .HasColumnName("DescricaoConhecimentos")
                    .HasMaxLength(2048);

                entity.Property(e => e.FundamentacaoChefia)
                    .HasColumnName("FundamentacaoChefia")
                    .HasMaxLength(2048);

                entity.Property(e => e.Planeada)
                    .HasColumnName("Planeada")
                    .HasColumnType("tinyint");

                entity.Property(e => e.CursoPlanoFormacao)
                    .HasColumnName("CursoPlanoFormacao")
                    .HasMaxLength(512);

                entity.Property(e => e.TemDotacaoOrcamental)
                    .HasColumnName("TemDotacaoOrcamental")
                    .HasColumnType("tinyint");

                entity.Property(e => e.ParecerDotacaoAcademia)
                    .HasColumnName("ParecerDotacaoAcademia")
                    .HasMaxLength(2048);

                entity.Property(e => e.ParecerAcademia)
                    .HasColumnName("ParecerAcademia")
                    .HasMaxLength(2048);

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("UtilizadorCriacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataHoraCriacao)
                    .HasColumnName("DataHoraCriacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorUltimaModificacao)
                    .HasColumnName("UtilizadorUltimaModificacao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataHoraUltimaModificacao)
                    .HasColumnName("DataHoraUltimaModificacao")
                    .HasColumnType("datetime");

                entity.HasOne(p => p.Accao)
                    .WithMany(a => a.PedidosParticipacao)
                    .HasForeignKey(p => p.IdAccaoFormacao)
                    .HasConstraintName("FK_PedidoParticipacaoFormacao_AccaoFormacao");

            });

            modelBuilder.Entity<RegistoAlteracoesPedidoFormacao>(entity =>
            {
                entity.HasKey(e => new { e.IdPedidoFormacao, e.IdRegisto });

                entity.Property(e => e.IdPedidoFormacao)
                    .HasColumnName("IdPedidoFormacao")
                    .HasMaxLength(20);

                entity.Property(e => e.IdRegisto)
                    .HasColumnName("IdRegisto")
                    .HasColumnType("int");

                entity.Property(e => e.TipoAlteracao)
                    .HasColumnName("TipoAlteracao")
                    .HasColumnType("int");

                entity.Property(e => e.DescricaoAlteracao)
                    .HasColumnName("DescricaoAlteracao")
                    .HasMaxLength(250);

                entity.Property(e => e.UtilizadorAlteracao)
                    .HasColumnName("UtilizadorAlteracao")
                    .HasMaxLength(100);

                entity.Property(e => e.DataHoraAlteracao)
                    .HasColumnName("DataHoraAlteracao")
                    .HasColumnType("datetime");

                entity.HasOne(r => r.PedidoNavigation)
                    .WithMany(t => t.RegistosAlteracoes)
                    .HasForeignKey(r => r.IdPedidoFormacao)
                    .HasConstraintName("FK_RegistoAlteracoesPedido_PedidoParticipacaoFormacao");

            });

            modelBuilder.Entity<SessaoAccaoFormacao>(entity =>
            {
                entity.HasKey(e => new { e.IdSessaoFormacao, e.IdAccao });

                entity.Property(e => e.IdSessaoFormacao)
                    .HasColumnName("IdSessaoFormacao")
                    .HasMaxLength(50);

                entity.Property(e => e.IdAccao)
                    .HasColumnName("IdAccao")
                    .HasMaxLength(50);

                entity.Property(e => e.DataSessao)
                    .HasColumnName("DataSessao")
                    .HasColumnType("date");

                entity.Property(e => e.HoraInicioSessao)
                    .HasColumnName("HoraInicioSessao")
                    .HasMaxLength(10);

                entity.Property(e => e.HoraFimSessao)
                    .HasColumnName("HoraFimSessao")
                    .HasMaxLength(10);

                entity.Property(e => e.DuracaoSessao)
                    .HasColumnName("DuracaoSessao")
                    .HasColumnType("decimal(18, 2)");

                entity.HasOne(s => s.AccaoNavigation)
                    .WithMany(a => a.SessoesFormacao)
                    .HasForeignKey(s => s.IdAccao)
                    .HasConstraintName("FK_SessaoAccaoFormacao_AccaoFormacao");
            });

            modelBuilder.Entity<TemaFormacao>(entity =>
            {
                entity.HasKey(e => new { e.IdTema });

                entity.Property(e => e.IdTema)
                    .HasColumnName("IdTema")
                    .HasMaxLength(50);

                entity.Property(e => e.CodigoInterno)
                    .HasColumnName("CodigoInterno")
                    .HasMaxLength(50);

                entity.Property(e => e.DescricaoTema)
                    .HasColumnName("DescricaoTema")
                    .HasMaxLength(250);

                entity.Property(e => e.UrlImagem)
                    .HasColumnName("UrlImagem")
                    .HasMaxLength(250);

                entity.Property(e => e.Activo)
                    .HasColumnType("tinyint");

            });
            #endregion


        }
    }
}
