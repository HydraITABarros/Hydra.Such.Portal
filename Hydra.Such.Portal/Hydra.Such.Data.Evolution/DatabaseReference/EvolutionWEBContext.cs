using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class EvolutionWEBContext : DbContext
    {
        public EvolutionWEBContext()
        {
        }

        public EvolutionWEBContext(DbContextOptions<EvolutionWEBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Accao> Accao { get; set; }
        public virtual DbSet<Acessorio> Acessorio { get; set; }
        public virtual DbSet<Acessos> Acessos { get; set; }
        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<AreaOp> AreaOp { get; set; }
        public virtual DbSet<CabeçalhoFolhaHoras> CabeçalhoFolhaHoras { get; set; }
        public virtual DbSet<CarregamentoInstituicaoFase2> CarregamentoInstituicaoFase2 { get; set; }
        public virtual DbSet<CarregamentoInstituicaoServico> CarregamentoInstituicaoServico { get; set; }
        public virtual DbSet<CarregamentoPimpHtq07out2019> CarregamentoPimpHtq07out2019 { get; set; }
        public virtual DbSet<Chat> Chat { get; set; }
        public virtual DbSet<ChatUsersOn> ChatUsersOn { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<ClientePimp> ClientePimp { get; set; }
        public virtual DbSet<CodigosSuch> CodigosSuch { get; set; }
        public virtual DbSet<Contactos> Contactos { get; set; }
        public virtual DbSet<Contrato> Contrato { get; set; }
        public virtual DbSet<ContratoAssTipoRequisito> ContratoAssTipoRequisito { get; set; }
        public virtual DbSet<ContratoCondicoesPagamento> ContratoCondicoesPagamento { get; set; }
        public virtual DbSet<ContratoEstado> ContratoEstado { get; set; }
        public virtual DbSet<ContratoLinha> ContratoLinha { get; set; }
        public virtual DbSet<ContratoTipo> ContratoTipo { get; set; }
        public virtual DbSet<ContratoTipoRequisito> ContratoTipoRequisito { get; set; }
        public virtual DbSet<Datas> Datas { get; set; }
        public virtual DbSet<DiariosRegAut> DiariosRegAut { get; set; }
        public virtual DbSet<EmmAnexos> EmmAnexos { get; set; }
        public virtual DbSet<EmmAnexosTmp> EmmAnexosTmp { get; set; }
        public virtual DbSet<EmmCalibracao> EmmCalibracao { get; set; }
        public virtual DbSet<EmmCategorias> EmmCategorias { get; set; }
        public virtual DbSet<EmmEntidade> EmmEntidade { get; set; }
        public virtual DbSet<EmmEquipamentoAcessorios> EmmEquipamentoAcessorios { get; set; }
        public virtual DbSet<EmmEquipamentoAcessoriosTmp> EmmEquipamentoAcessoriosTmp { get; set; }
        public virtual DbSet<EmmEquipamentos> EmmEquipamentos { get; set; }
        public virtual DbSet<EmmEquipamentosEstados> EmmEquipamentosEstados { get; set; }
        public virtual DbSet<EmmEquipamentosServicos> EmmEquipamentosServicos { get; set; }
        public virtual DbSet<EmmGamaUtilizacao> EmmGamaUtilizacao { get; set; }
        public virtual DbSet<EmmGamaUtilizacaoTmp> EmmGamaUtilizacaoTmp { get; set; }
        public virtual DbSet<EmmGrupos> EmmGrupos { get; set; }
        public virtual DbSet<EmmHistorico> EmmHistorico { get; set; }
        public virtual DbSet<EmmHistoricoTabelas> EmmHistoricoTabelas { get; set; }
        public virtual DbSet<EmmInspecaoAssinaturas> EmmInspecaoAssinaturas { get; set; }
        public virtual DbSet<EmmInspecaoPadrao> EmmInspecaoPadrao { get; set; }
        public virtual DbSet<EmmInspecaoPadraoTmp> EmmInspecaoPadraoTmp { get; set; }
        public virtual DbSet<EmmInspecaoRotina> EmmInspecaoRotina { get; set; }
        public virtual DbSet<EmmInspecaoRotinaTmp> EmmInspecaoRotinaTmp { get; set; }
        public virtual DbSet<EmmInspeccoes> EmmInspeccoes { get; set; }
        public virtual DbSet<EmmInspeccoesEstados> EmmInspeccoesEstados { get; set; }
        public virtual DbSet<EmmInspeccoesResultados> EmmInspeccoesResultados { get; set; }
        public virtual DbSet<EmmInspeccoesTipos> EmmInspeccoesTipos { get; set; }
        public virtual DbSet<EmmLaboratorio> EmmLaboratorio { get; set; }
        public virtual DbSet<EmmPeriodicidade> EmmPeriodicidade { get; set; }
        public virtual DbSet<EmmPermissao> EmmPermissao { get; set; }
        public virtual DbSet<EmmSetPoints> EmmSetPoints { get; set; }
        public virtual DbSet<EmmTipo> EmmTipo { get; set; }
        public virtual DbSet<EmmUtilizadores> EmmUtilizadores { get; set; }
        public virtual DbSet<EmmUtilizadoresTmp> EmmUtilizadoresTmp { get; set; }
        public virtual DbSet<EnderecoSuch> EnderecoSuch { get; set; }
        public virtual DbSet<EquipCategoria> EquipCategoria { get; set; }
        public virtual DbSet<EquipDadosTecnicos> EquipDadosTecnicos { get; set; }
        public virtual DbSet<EquipDependente> EquipDependente { get; set; }
        public virtual DbSet<EquipEstado> EquipEstado { get; set; }
        public virtual DbSet<EquipFicheiros> EquipFicheiros { get; set; }
        public virtual DbSet<EquipMarca> EquipMarca { get; set; }
        public virtual DbSet<EquipModelo> EquipModelo { get; set; }
        public virtual DbSet<EquipParametro> EquipParametro { get; set; }
        public virtual DbSet<EquipPimp> EquipPimp { get; set; }
        public virtual DbSet<EquipTipo> EquipTipo { get; set; }
        public virtual DbSet<Equipa> Equipa { get; set; }
        public virtual DbSet<Equipamento> Equipamento { get; set; }
        public virtual DbSet<EquipamentoAcessorio> EquipamentoAcessorio { get; set; }
        public virtual DbSet<EstadoObra> EstadoObra { get; set; }
        public virtual DbSet<Familia> Familia { get; set; }
        public virtual DbSet<Feriado> Feriado { get; set; }
        public virtual DbSet<FichaManutencao> FichaManutencao { get; set; }
        public virtual DbSet<FichaManutencaoEquipamentosTeste> FichaManutencaoEquipamentosTeste { get; set; }
        public virtual DbSet<FichaManutencaoManutencao> FichaManutencaoManutencao { get; set; }
        public virtual DbSet<FichaManutencaoRelatorio> FichaManutencaoRelatorio { get; set; }
        public virtual DbSet<FichaManutencaoRelatorioEquipamentosTeste> FichaManutencaoRelatorioEquipamentosTeste { get; set; }
        public virtual DbSet<FichaManutencaoRelatorioManutencao> FichaManutencaoRelatorioManutencao { get; set; }
        public virtual DbSet<FichaManutencaoRelatorioTestesQualitativos> FichaManutencaoRelatorioTestesQualitativos { get; set; }
        public virtual DbSet<FichaManutencaoRelatorioTestesQuantitativos> FichaManutencaoRelatorioTestesQuantitativos { get; set; }
        public virtual DbSet<FichaManutencaoTempoEstimadoRotina> FichaManutencaoTempoEstimadoRotina { get; set; }
        public virtual DbSet<FichaManutencaoTestesQualitativos> FichaManutencaoTestesQualitativos { get; set; }
        public virtual DbSet<FichaManutencaoTestesQuantitativos> FichaManutencaoTestesQuantitativos { get; set; }
        public virtual DbSet<Fornecedor> Fornecedor { get; set; }
        public virtual DbSet<Grupo> Grupo { get; set; }
        public virtual DbSet<Imagens> Imagens { get; set; }
        public virtual DbSet<Instituicao> Instituicao { get; set; }
        public virtual DbSet<InstituicaoPimp> InstituicaoPimp { get; set; }
        public virtual DbSet<Job> Job { get; set; }
        public virtual DbSet<JobJournalBatch> JobJournalBatch { get; set; }
        public virtual DbSet<JobJournalLine> JobJournalLine { get; set; }
        public virtual DbSet<JobJournalTemplate> JobJournalTemplate { get; set; }
        public virtual DbSet<JobLedgerEntry> JobLedgerEntry { get; set; }
        public virtual DbSet<LinhaFolhaHoras> LinhaFolhaHoras { get; set; }
        public virtual DbSet<LinhaFolhaHorasDistribCus> LinhaFolhaHorasDistribCus { get; set; }
        public virtual DbSet<LinhaFolhaHorasMãoObra> LinhaFolhaHorasMãoObra { get; set; }
        public virtual DbSet<LogoClientes> LogoClientes { get; set; }
        public virtual DbSet<Logs> Logs { get; set; }
        public virtual DbSet<MaintenanceCatalog> MaintenanceCatalog { get; set; }
        public virtual DbSet<MaintenanceHeaderComments> MaintenanceHeaderComments { get; set; }
        public virtual DbSet<MaintenanceOrder> MaintenanceOrder { get; set; }
        public virtual DbSet<MaintenanceOrderAnexo> MaintenanceOrderAnexo { get; set; }
        public virtual DbSet<MaintenanceOrderClienteIteracao> MaintenanceOrderClienteIteracao { get; set; }
        public virtual DbSet<MaintenanceOrderLine> MaintenanceOrderLine { get; set; }
        public virtual DbSet<Menus> Menus { get; set; }
        public virtual DbSet<MetCalibracao> MetCalibracao { get; set; }
        public virtual DbSet<MetCertificado> MetCertificado { get; set; }
        public virtual DbSet<MetEquipamento> MetEquipamento { get; set; }
        public virtual DbSet<MoCommentLine> MoCommentLine { get; set; }
        public virtual DbSet<MoComponents> MoComponents { get; set; }
        public virtual DbSet<MoTasks> MoTasks { get; set; }
        public virtual DbSet<Modelos> Modelos { get; set; }
        public virtual DbSet<MovProjectoAutorizadosFact> MovProjectoAutorizadosFact { get; set; }
        public virtual DbSet<NivelAcessoTipo> NivelAcessoTipo { get; set; }
        public virtual DbSet<NoSeriesLine> NoSeriesLine { get; set; }
        public virtual DbSet<Orcamento> Orcamento { get; set; }
        public virtual DbSet<OrcamentoLine> OrcamentoLine { get; set; }
        public virtual DbSet<OrdemManutencao> OrdemManutencao { get; set; }
        public virtual DbSet<OrdemManutencaoDescricaoAvaria> OrdemManutencaoDescricaoAvaria { get; set; }
        public virtual DbSet<OrdemManutencaoEquipamentos> OrdemManutencaoEquipamentos { get; set; }
        public virtual DbSet<OrdemManutencaoEstadoMaterial> OrdemManutencaoEstadoMaterial { get; set; }
        public virtual DbSet<OrdemManutencaoLinha> OrdemManutencaoLinha { get; set; }
        public virtual DbSet<OrdemManutencaoLinhaMateriais> OrdemManutencaoLinhaMateriais { get; set; }
        public virtual DbSet<OrdemManutencaoMateriais> OrdemManutencaoMateriais { get; set; }
        public virtual DbSet<OrdemManutencaoRelatorioTrabalho> OrdemManutencaoRelatorioTrabalho { get; set; }
        public virtual DbSet<OrigemAvaria> OrigemAvaria { get; set; }
        public virtual DbSet<Periodicidade> Periodicidade { get; set; }
        public virtual DbSet<PermissoesDefault> PermissoesDefault { get; set; }
        public virtual DbSet<PlanoExecutado> PlanoExecutado { get; set; }
        public virtual DbSet<PostedMaintenanceOrder> PostedMaintenanceOrder { get; set; }
        public virtual DbSet<PostedMaintenanceOrderLine> PostedMaintenanceOrderLine { get; set; }
        public virtual DbSet<ProjectosAutorizadosFact> ProjectosAutorizadosFact { get; set; }
        public virtual DbSet<Regiao> Regiao { get; set; }
        public virtual DbSet<Rotina> Rotina { get; set; }
        public virtual DbSet<Sequencia> Sequencia { get; set; }
        public virtual DbSet<Servico> Servico { get; set; }
        public virtual DbSet<SistemaConfig> SistemaConfig { get; set; }
        public virtual DbSet<Solicitacoes> Solicitacoes { get; set; }
        public virtual DbSet<SolicitacoesEstado> SolicitacoesEstado { get; set; }
        public virtual DbSet<SolicitacoesLinha> SolicitacoesLinha { get; set; }
        public virtual DbSet<TipoContacto> TipoContacto { get; set; }
        public virtual DbSet<TipoDeContacto> TipoDeContacto { get; set; }
        public virtual DbSet<TipoMaoObra> TipoMaoObra { get; set; }
        public virtual DbSet<TipoObra> TipoObra { get; set; }
        public virtual DbSet<TransfereProdutosProjecto> TransfereProdutosProjecto { get; set; }
        public virtual DbSet<Utilizador> Utilizador { get; set; }
        public virtual DbSet<UtilizadorCompetencias> UtilizadorCompetencias { get; set; }
        public virtual DbSet<UtilizadorFormacao> UtilizadorFormacao { get; set; }
        public virtual DbSet<UtilizadorHabilitacoes> UtilizadorHabilitacoes { get; set; }
        public virtual DbSet<UtilizadorPermissao> UtilizadorPermissao { get; set; }
        public virtual DbSet<Versao> Versao { get; set; }

        // Unable to generate entity type for table 'dbo.Z_Execucao_Job'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo._CARREGAMENTO_SERVICO'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Solicitacoes_Anexos'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Equip_Manutencao'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.TEMP_1_FICHA_MANUTENCAO'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo._Equipamentos_CHULC'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.TEMP_2_FICHA_MANUTENCAO_TEMPO_ESTIMADO_ROTINA'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.TEMP_3_FICHA_MANUTENCAO_EQUIPAMENTOS_TESTE'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.TEMP_4_FICHA_MANUTENCAO_MANUTENCAO'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.TEMP_5_FICHA_MANUTENCAO_TESTES_QUALITATIVOS'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.TEMP_6_FICHA_MANUTENCAO_TESTES_QUANTITATIVOS'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo._Equipamentos_CHULN'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Meus_Menus'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Job Ledger Entry - BACKUP CGs'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Logs_2018'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Job Ledger Entry - BACKUP RQs'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo._View_Requisicoes'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo._View_Linhas_Requisicoes'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo._View_ConsultasMercado'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo._View_Linhas_ConsultasMercado'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo._View_Linhas_Encomendas'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.___Maintenance Order___ Material Entregue 6 e 7'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Job Ledger Entry_BACKUP_20190607'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo._Carregamento_PIMP_CHTMAD'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo._Carregamento_PIMP_HTQ_19Maio'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo._Carregamento_PIMP_VC170057'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<Accao>(entity =>
            {
                entity.HasKey(e => e.IdAccao);

                entity.Property(e => e.IdAccao).HasColumnName("ID_Accao");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Acessorio>(entity =>
            {
                entity.HasKey(e => e.IdAcessorio);

                entity.Property(e => e.IdAcessorio).HasColumnName("ID_Acessorio");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IdArea).HasColumnName("ID_Area");

                entity.Property(e => e.IdAreaOp).HasColumnName("ID_AreaOp");

                entity.Property(e => e.IdEquipa).HasColumnName("ID_Equipa");

                entity.Property(e => e.IdFornecedor).HasColumnName("ID_Fornecedor");

                entity.Property(e => e.IdMarca).HasColumnName("ID_Marca");

                entity.Property(e => e.IdModelo).HasColumnName("ID_Modelo");

                entity.Property(e => e.IdRegiao).HasColumnName("ID_Regiao");

                entity.Property(e => e.Localizacao).HasMaxLength(50);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NumInventario)
                    .HasColumnName("Num_Inventario")
                    .HasMaxLength(50);

                entity.Property(e => e.NumSerie)
                    .HasColumnName("Num_Serie")
                    .HasMaxLength(50);

                entity.Property(e => e.Observacao).HasMaxLength(250);

                entity.Property(e => e.Tipo).HasMaxLength(50);

                entity.HasOne(d => d.IdAreaNavigation)
                    .WithMany(p => p.Acessorio)
                    .HasForeignKey(d => d.IdArea)
                    .HasConstraintName("FK_Acessorio_Area");

                entity.HasOne(d => d.IdAreaOpNavigation)
                    .WithMany(p => p.Acessorio)
                    .HasForeignKey(d => d.IdAreaOp)
                    .HasConstraintName("FK_Acessorio_AreaOp");

                entity.HasOne(d => d.IdEquipaNavigation)
                    .WithMany(p => p.Acessorio)
                    .HasForeignKey(d => d.IdEquipa)
                    .HasConstraintName("FK_Acessorio_Equipa");

                entity.HasOne(d => d.IdMarcaNavigation)
                    .WithMany(p => p.Acessorio)
                    .HasForeignKey(d => d.IdMarca)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Acessorio_Equip_Marca");

                entity.HasOne(d => d.IdModeloNavigation)
                    .WithMany(p => p.Acessorio)
                    .HasForeignKey(d => d.IdModelo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Acessorio_Equip_Modelo");

                entity.HasOne(d => d.IdRegiaoNavigation)
                    .WithMany(p => p.Acessorio)
                    .HasForeignKey(d => d.IdRegiao)
                    .HasConstraintName("FK_Acessorio_Regiao");
            });

            modelBuilder.Entity<Acessos>(entity =>
            {
                entity.HasKey(e => e.IdAcesso);

                entity.Property(e => e.IdAcesso)
                    .HasColumnName("ID_Acesso")
                    .ValueGeneratedNever();

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Area>(entity =>
            {
                entity.HasKey(e => e.IdArea);

                entity.Property(e => e.IdArea).HasColumnName("ID_Area");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AreaOp>(entity =>
            {
                entity.HasKey(e => e.IdAreaOp);

                entity.Property(e => e.IdAreaOp).HasColumnName("ID_AreaOp");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IdEquipa).HasColumnName("ID_Equipa");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdEquipaNavigation)
                    .WithMany(p => p.AreaOp)
                    .HasForeignKey(d => d.IdEquipa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AreaOp_Equipa");
            });

            modelBuilder.Entity<CabeçalhoFolhaHoras>(entity =>
            {
                entity.HasKey(e => e.NºFolhaHoras);

                entity.ToTable("Cabeçalho Folha Horas");

                entity.Property(e => e.NºFolhaHoras)
                    .HasColumnName("Nº Folha Horas")
                    .ValueGeneratedNever();

                entity.Property(e => e.AjudasCalculadas).HasColumnName("Ajudas Calculadas");

                entity.Property(e => e.CriadaPor)
                    .IsRequired()
                    .HasColumnName("Criada por")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CódFaseProjecto)
                    .IsRequired()
                    .HasColumnName("Cód_ Fase Projecto")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CódSubfaseProjecto)
                    .IsRequired()
                    .HasColumnName("Cód_ Subfase Projecto")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CódTarefaProjecto)
                    .IsRequired()
                    .HasColumnName("Cód_ Tarefa Projecto")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CódigoTipoKm)
                    .IsRequired()
                    .HasColumnName("Código Tipo Km")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DataChegada)
                    .HasColumnName("Data Chegada")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataCriação)
                    .HasColumnName("Data Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataIntegraçãoEmHr)
                    .HasColumnName("Data integração em HR")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataIntegraçãoEmHrKm)
                    .HasColumnName("Data integração em HR KM")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataInvalidação)
                    .HasColumnName("Data Invalidação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataPartida)
                    .HasColumnName("Data Partida")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataRegisto)
                    .HasColumnName("Data Registo")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataTerminado)
                    .HasColumnName("Data Terminado")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataValidação)
                    .HasColumnName("Data Validação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeslocaçãoPlaneada).HasColumnName("Deslocação Planeada");

                entity.Property(e => e.GlobalDimension1Code)
                    .IsRequired()
                    .HasColumnName("Global dimension 1 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.GlobalDimension2Code)
                    .IsRequired()
                    .HasColumnName("Global dimension 2 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.HoraChegada)
                    .HasColumnName("Hora Chegada")
                    .HasColumnType("datetime");

                entity.Property(e => e.HoraCriação)
                    .HasColumnName("Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.HoraIntegraçãoEmHr)
                    .HasColumnName("Hora integração em HR")
                    .HasColumnType("datetime");

                entity.Property(e => e.HoraIntegraçãoEmHrKm)
                    .HasColumnName("Hora integração em HR KM")
                    .HasColumnType("datetime");

                entity.Property(e => e.HoraPartida)
                    .HasColumnName("Hora Partida")
                    .HasColumnType("datetime");

                entity.Property(e => e.HoraValidação)
                    .HasColumnName("Hora Validação")
                    .HasColumnType("datetime");

                entity.Property(e => e.IntegradoEmHr).HasColumnName("Integrado em HR");

                entity.Property(e => e.IntegradoSubsidioHr).HasColumnName("IntegradoSubsidioHR");

                entity.Property(e => e.IntegradoSubsidioHrkm).HasColumnName("IntegradoSubsidioHRKm");

                entity.Property(e => e.IntegradorEmHr)
                    .IsRequired()
                    .HasColumnName("Integrador em HR")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IntegradorEmHrKm)
                    .IsRequired()
                    .HasColumnName("Integrador em HR KM")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Matricula)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.MotivoInvalidação)
                    .IsRequired()
                    .HasColumnName("Motivo Invalidação")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NºEmpregado)
                    .IsRequired()
                    .HasColumnName("Nº Empregado")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NºLinhaOrdemManut).HasColumnName("Nº Linha Ordem Manut");

                entity.Property(e => e.NºLinhaTarefaOrdemManut).HasColumnName("Nº Linha Tarefa Ordem Manut_");

                entity.Property(e => e.NºObra)
                    .IsRequired()
                    .HasColumnName("Nº Obra")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NºRecurso)
                    .IsRequired()
                    .HasColumnName("Nº Recurso")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NºResponsavel1)
                    .IsRequired()
                    .HasColumnName("Nº Responsavel 1")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NºResponsavel2)
                    .IsRequired()
                    .HasColumnName("Nº Responsavel 2")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NºResponsavel3)
                    .IsRequired()
                    .HasColumnName("Nº Responsavel 3")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Observações)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Perfil)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ProdPostingGroup)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RegistadoPor)
                    .IsRequired()
                    .HasColumnName("Registado por")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Responsaveis)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension3Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 3 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension4Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 4 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TerminadoPor)
                    .HasColumnName("Terminado por")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.TipoObra).HasColumnName("Tipo Obra");

                entity.Property(e => e.Validador)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ValidadoresRh)
                    .IsRequired()
                    .HasColumnName("Validadores RH")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.ValidadoresRhKm)
                    .IsRequired()
                    .HasColumnName("Validadores RH KM")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.ViaturaPrópria).HasColumnName("Viatura própria");

                entity.Property(e => e.Visualizadores)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CarregamentoInstituicaoFase2>(entity =>
            {
                entity.ToTable("_CARREGAMENTO_INSTITUICAO_FASE_2");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DescricaoEvolution)
                    .HasColumnName("Descricao_Evolution")
                    .HasMaxLength(250);

                entity.Property(e => e.IdCliente).HasColumnName("ID_CLIENTE");

                entity.Property(e => e.IdInstServ).HasColumnName("ID_INST_SERV");

                entity.Property(e => e.IdInstituicaoSuperior).HasColumnName("ID_INSTITUICAO_SUPERIOR");

                entity.Property(e => e.InstituicaoSuperior).HasMaxLength(50);

                entity.Property(e => e.LocalizacaoFuncional).HasMaxLength(50);

                entity.Property(e => e.NomeCliente).HasMaxLength(250);

                entity.Property(e => e.NumClienteNav2009)
                    .HasColumnName("NumCliente_NAV2009")
                    .HasMaxLength(50);

                entity.Property(e => e.NumClienteNav2017)
                    .HasColumnName("NumCliente_NAV2017")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CarregamentoInstituicaoServico>(entity =>
            {
                entity.ToTable("_CARREGAMENTO_INSTITUICAO_SERVICO");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Contador).HasMaxLength(50);

                entity.Property(e => e.DescricaoEvolution)
                    .HasColumnName("Descricao_Evolution")
                    .HasMaxLength(250);

                entity.Property(e => e.IdCliente).HasColumnName("ID_CLIENTE");

                entity.Property(e => e.IdInstServ).HasColumnName("ID_INST_SERV");

                entity.Property(e => e.IdInstituicaoSuperior).HasColumnName("ID_INSTITUICAO_SUPERIOR");

                entity.Property(e => e.InstituicaoOuServico)
                    .HasColumnName("Instituicao_Ou_Servico")
                    .HasMaxLength(50);

                entity.Property(e => e.InstituicaoSuperior).HasMaxLength(50);

                entity.Property(e => e.LocalizacaoFuncional).HasMaxLength(50);

                entity.Property(e => e.NomeCliente).HasMaxLength(250);

                entity.Property(e => e.NomeCliente2).HasMaxLength(250);

                entity.Property(e => e.NumClienteNav2009)
                    .HasColumnName("NumCliente_NAV2009")
                    .HasMaxLength(50);

                entity.Property(e => e.NumClienteNav2017)
                    .HasColumnName("NumCliente_NAV2017")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CarregamentoPimpHtq07out2019>(entity =>
            {
                entity.ToTable("_Carregamento_PIMP_HTQ_07Out2019");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Ano).HasMaxLength(50);

                entity.Property(e => e.Contrato).HasMaxLength(50);

                entity.Property(e => e.DataPlano).HasMaxLength(50);

                entity.Property(e => e.DataValidacao).HasMaxLength(50);

                entity.Property(e => e.Dia).HasMaxLength(50);

                entity.Property(e => e.Estado).HasMaxLength(50);

                entity.Property(e => e.IdCliente).HasColumnName("ID_CLIENTE");

                entity.Property(e => e.IdEq)
                    .HasColumnName("Id_Eq")
                    .HasMaxLength(50);

                entity.Property(e => e.IdEquipa).HasColumnName("ID_EQUIPA");

                entity.Property(e => e.IdEquipamento).HasColumnName("ID_EQUIPAMENTO");

                entity.Property(e => e.IdRotina).HasColumnName("ID_ROTINA");

                entity.Property(e => e.IdTecnico).HasColumnName("ID_TECNICO");

                entity.Property(e => e.IdUserInsercao).HasColumnName("ID_USER_INSERCAO");

                entity.Property(e => e.Mes).HasMaxLength(50);

                entity.Property(e => e.Rotina).HasMaxLength(50);

                entity.Property(e => e.Semana).HasMaxLength(50);

                entity.Property(e => e.Semestre).HasMaxLength(50);

                entity.Property(e => e.Tecnico).HasMaxLength(50);

                entity.Property(e => e.Trimestre).HasMaxLength(50);

                entity.Property(e => e.Utilizador).HasMaxLength(50);
            });

            modelBuilder.Entity<Chat>(entity =>
            {
                entity.HasKey(e => e.IdMensagem);

                entity.Property(e => e.IdMensagem).HasColumnName("ID_Mensagem");

                entity.Property(e => e.DataMensagem)
                    .HasColumnName("Data_Mensagem")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdUserDestino).HasColumnName("ID_User_Destino");

                entity.Property(e => e.IdUserEnvio).HasColumnName("ID_User_Envio");

                entity.Property(e => e.Mensagem)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.IdUserDestinoNavigation)
                    .WithMany(p => p.ChatIdUserDestinoNavigation)
                    .HasForeignKey(d => d.IdUserDestino)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Chat_Utilizador1");

                entity.HasOne(d => d.IdUserEnvioNavigation)
                    .WithMany(p => p.ChatIdUserEnvioNavigation)
                    .HasForeignKey(d => d.IdUserEnvio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Chat_Utilizador");
            });

            modelBuilder.Entity<ChatUsersOn>(entity =>
            {
                entity.HasKey(e => e.IdChatOn);

                entity.ToTable("Chat_Users_ON");

                entity.Property(e => e.IdChatOn).HasColumnName("ID_ChatOn");

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.ChatUsersOn)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Chat_Users_ON_Utilizador");
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.IdCliente);

                entity.HasIndex(e => e.CodNavision)
                    .HasName("_dta_index_Cliente_6_1959014060__K3");

                entity.HasIndex(e => e.IdCliente)
                    .HasName("_dta_index_Cliente_6_1959014060__K1");

                entity.HasIndex(e => e.Nome)
                    .HasName("_dta_index_Cliente_6_1959014060__K2");

                entity.HasIndex(e => new { e.CodNavision, e.Nome })
                    .HasName("_dta_index_Cliente_6_1959014060__K2_3");

                entity.HasIndex(e => new { e.Nome, e.CodNavision })
                    .HasName("IX_Cliente_Cod_Navision");

                entity.HasIndex(e => new { e.IdCliente, e.Nome, e.CodNavision, e.ActivoManut, e.CrespNav, e.Activo })
                    .HasName("IX_Cliente_Activo");

                entity.Property(e => e.IdCliente).HasColumnName("ID_Cliente");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ActivoManut)
                    .HasColumnName("Activo_MANUT")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.AssociadoAN)
                    .HasColumnName("Associado_A_N")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.Blocked).HasDefaultValueSql("((0))");

                entity.Property(e => e.ClienteAssociado)
                    .HasColumnName("Cliente_Associado")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ClienteInterno)
                    .HasColumnName("Cliente_Interno")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ClienteNacional)
                    .HasColumnName("Cliente_Nacional")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CodNavision)
                    .IsRequired()
                    .HasColumnName("Cod_Navision")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Contacto)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CrespNav)
                    .HasColumnName("CRESP_NAV")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdRegiao).HasColumnName("ID_Regiao");

                entity.Property(e => e.Morada)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NaturezaCliente)
                    .HasColumnName("Natureza_Cliente")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Nif)
                    .HasColumnName("NIF")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(101)
                    .IsUnicode(false);

                entity.Property(e => e.RegiaoNav)
                    .HasColumnName("Regiao_NAV")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TipoCliente)
                    .HasColumnName("Tipo_Cliente")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.IdRegiaoNavigation)
                    .WithMany(p => p.Cliente)
                    .HasForeignKey(d => d.IdRegiao)
                    .HasConstraintName("FK_Cliente_Cliente_Regioes");
            });

            modelBuilder.Entity<ClientePimp>(entity =>
            {
                entity.HasKey(e => e.IdClientePimp);

                entity.ToTable("Cliente_PIMP");

                entity.Property(e => e.IdClientePimp).HasColumnName("ID_Cliente_PIMP");

                entity.Property(e => e.DataAlteracao)
                    .HasColumnName("Data_Alteracao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataInsercao)
                    .HasColumnName("Data_Insercao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataPlano)
                    .HasColumnName("Data_Plano")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdArea).HasColumnName("ID_Area");

                entity.Property(e => e.IdAreaOp).HasColumnName("ID_AreaOp");

                entity.Property(e => e.IdCliente).HasColumnName("ID_Cliente");

                entity.Property(e => e.IdContrato)
                    .IsRequired()
                    .HasColumnName("ID_Contrato")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdContratoLinha).HasColumnName("ID_Contrato_Linha");

                entity.Property(e => e.IdEquipa).HasColumnName("ID_Equipa");

                entity.Property(e => e.IdInstituicao).HasColumnName("ID_Instituicao");

                entity.Property(e => e.IdRegiao).HasColumnName("ID_Regiao");

                entity.Property(e => e.IdServico).HasColumnName("ID_Servico");

                entity.Property(e => e.IdUtilizadorAlteracao).HasColumnName("ID_Utilizador_Alteracao");

                entity.Property(e => e.IdUtilizadorInsercao).HasColumnName("ID_Utilizador_Insercao");

                entity.Property(e => e.Observacoes)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.PlanoExecutado).HasColumnName("Plano_Executado");

                entity.Property(e => e.Replicado).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.IdAreaNavigation)
                    .WithMany(p => p.ClientePimp)
                    .HasForeignKey(d => d.IdArea)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cliente_PIMP_Area");

                entity.HasOne(d => d.IdAreaOpNavigation)
                    .WithMany(p => p.ClientePimp)
                    .HasForeignKey(d => d.IdAreaOp)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cliente_PIMP_AreaOp");

                entity.HasOne(d => d.IdContratoNavigation)
                    .WithMany(p => p.ClientePimp)
                    .HasForeignKey(d => d.IdContrato)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cliente_PIMP_Contrato");

                entity.HasOne(d => d.IdContratoLinhaNavigation)
                    .WithMany(p => p.ClientePimp)
                    .HasForeignKey(d => d.IdContratoLinha)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cliente_PIMP_Contrato_Linha");

                entity.HasOne(d => d.IdEquipaNavigation)
                    .WithMany(p => p.ClientePimp)
                    .HasForeignKey(d => d.IdEquipa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cliente_PIMP_Equipa");

                entity.HasOne(d => d.IdRegiaoNavigation)
                    .WithMany(p => p.ClientePimp)
                    .HasForeignKey(d => d.IdRegiao)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cliente_PIMP_Regiao");

                entity.HasOne(d => d.IdUtilizadorAlteracaoNavigation)
                    .WithMany(p => p.ClientePimpIdUtilizadorAlteracaoNavigation)
                    .HasForeignKey(d => d.IdUtilizadorAlteracao)
                    .HasConstraintName("FK_Cliente_PIMP_Utilizador_Alteracao");

                entity.HasOne(d => d.IdUtilizadorInsercaoNavigation)
                    .WithMany(p => p.ClientePimpIdUtilizadorInsercaoNavigation)
                    .HasForeignKey(d => d.IdUtilizadorInsercao)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cliente_PIMP_Utilizador_Insercao");

                entity.HasOne(d => d.TecnicoNavigation)
                    .WithMany(p => p.ClientePimpTecnicoNavigation)
                    .HasForeignKey(d => d.Tecnico);
            });

            modelBuilder.Entity<CodigosSuch>(entity =>
            {
                entity.HasKey(e => new { e.Tipo, e.Codigo, e.Codigo2 })
                    .HasName("Codigos Such$0");

                entity.ToTable("Codigos Such");

                entity.Property(e => e.Codigo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Codigo2)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Descrição)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();
            });

            modelBuilder.Entity<Contactos>(entity =>
            {
                entity.HasKey(e => e.IdContacto);

                entity.Property(e => e.IdContacto).HasColumnName("ID_Contacto");

                entity.Property(e => e.Contacto).HasMaxLength(80);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NomeContacto)
                    .HasColumnName("Nome_Contacto")
                    .HasMaxLength(50);

                entity.Property(e => e.TipoContacto).HasColumnName("Tipo_Contacto");

                entity.HasOne(d => d.ClienteNavigation)
                    .WithMany(p => p.Contactos)
                    .HasForeignKey(d => d.Cliente)
                    .HasConstraintName("FK_Contactos_Cliente");

                entity.HasOne(d => d.InstituicaoNavigation)
                    .WithMany(p => p.Contactos)
                    .HasForeignKey(d => d.Instituicao)
                    .HasConstraintName("FK_Contactos_Instituicao");

                entity.HasOne(d => d.ServicoNavigation)
                    .WithMany(p => p.Contactos)
                    .HasForeignKey(d => d.Servico)
                    .HasConstraintName("FK_Contactos_Servico");

                entity.HasOne(d => d.TipoContactoNavigation)
                    .WithMany(p => p.Contactos)
                    .HasForeignKey(d => d.TipoContacto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contactos_Tipo_de_Contacto");
            });

            modelBuilder.Entity<Contrato>(entity =>
            {
                entity.HasKey(e => e.IdContrato);

                entity.Property(e => e.IdContrato)
                    .HasColumnName("ID_Contrato")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ClienteNav)
                    .HasColumnName("Cliente_NAV")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.ClienteNavNumber)
                    .HasColumnName("Cliente_NAV_Number")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ContratoAvencaFixa)
                    .HasColumnName("Contrato_Avenca_Fixa")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.DataCancelamento)
                    .HasColumnName("Data_Cancelamento")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataFim)
                    .HasColumnName("Data_Fim")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataInicio)
                    .HasColumnName("Data_Inicio")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataUltimaFatura)
                    .HasColumnName("Data_Ultima_Fatura")
                    .HasColumnType("date");

                entity.Property(e => e.EstadoAlteracao).HasColumnName("Estado_Alteracao");

                entity.Property(e => e.EstadoESuch).HasColumnName("Estado_eSUCH");

                entity.Property(e => e.IdCliente).HasColumnName("ID_Cliente");

                entity.Property(e => e.IdEstado).HasColumnName("ID_Estado");

                entity.Property(e => e.IdTipoContrato).HasColumnName("ID_Tipo_Contrato");

                entity.Property(e => e.NomeContrato)
                    .HasColumnName("Nome_Contrato")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.NumCompromisso)
                    .HasColumnName("Num_Compromisso")
                    .HasMaxLength(20);

                entity.Property(e => e.NumProposta)
                    .HasColumnName("Num_Proposta")
                    .HasMaxLength(20);

                entity.Property(e => e.NumRequisicaoCliente)
                    .HasColumnName("Num_Requisicao_Cliente")
                    .HasMaxLength(30);

                entity.Property(e => e.NumVersao).HasColumnName("Num_Versao");

                entity.Property(e => e.Observacoes)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.PercentagemMc)
                    .HasColumnName("Percentagem_MC")
                    .HasColumnType("decimal(38, 20)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PeriodoFatura).HasColumnName("Periodo_Fatura");

                entity.Property(e => e.RegiaoNav)
                    .HasColumnName("Regiao_NAV")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TaxaAprovisionamento)
                    .HasColumnName("Taxa_Aprovisionamento")
                    .HasColumnType("decimal(38, 20)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TipoContratoManut).HasColumnName("Tipo_Contrato_Manut");

                entity.Property(e => e.TipoFaturacao).HasColumnName("Tipo_Faturacao");

                entity.Property(e => e.ToleranciaContrato).HasColumnName("Tolerancia_Contrato");

                entity.Property(e => e.ValorMensal)
                    .HasColumnName("Valor_Mensal")
                    .HasColumnType("decimal(38, 20)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ValorTotal)
                    .HasColumnName("Valor_Total")
                    .HasColumnType("decimal(38, 20)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ValorTotalProposta)
                    .HasColumnName("Valor_Total_Proposta")
                    .HasColumnType("decimal(38, 20)")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Contrato)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Contrato_Cliente");

                entity.HasOne(d => d.IdEstadoNavigation)
                    .WithMany(p => p.Contrato)
                    .HasForeignKey(d => d.IdEstado)
                    .HasConstraintName("FK_Contrato_Contrato_Estado");

                entity.HasOne(d => d.IdTipoContratoNavigation)
                    .WithMany(p => p.Contrato)
                    .HasForeignKey(d => d.IdTipoContrato)
                    .HasConstraintName("FK_Contrato_Contrato_Tipo");
            });

            modelBuilder.Entity<ContratoAssTipoRequisito>(entity =>
            {
                entity.HasKey(e => e.IdContratoAssTipoRequisito);

                entity.ToTable("Contrato_ASS_Tipo_Requisito");

                entity.HasIndex(e => new { e.IdContrato, e.IdTipoRequisito })
                    .HasName("IX_Contrato_ASS_Tipo_Requisito")
                    .IsUnique();

                entity.Property(e => e.IdContratoAssTipoRequisito).HasColumnName("ID_Contrato_Ass_Tipo_Requisito");

                entity.Property(e => e.IdContrato)
                    .IsRequired()
                    .HasColumnName("ID_Contrato")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdTipoRequisito).HasColumnName("ID_Tipo_Requisito");

                entity.HasOne(d => d.IdContratoNavigation)
                    .WithMany(p => p.ContratoAssTipoRequisito)
                    .HasForeignKey(d => d.IdContrato)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contrato_ASS_Tipo_Requisito_Contrato");

                entity.HasOne(d => d.IdTipoRequisitoNavigation)
                    .WithMany(p => p.ContratoAssTipoRequisito)
                    .HasForeignKey(d => d.IdTipoRequisito)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contrato_ASS_Tipo_Requisito_Contrato_Tipo_Requisito");
            });

            modelBuilder.Entity<ContratoCondicoesPagamento>(entity =>
            {
                entity.HasKey(e => e.IdCondicaoPagamento);

                entity.ToTable("Contrato_CondicoesPagamento");

                entity.Property(e => e.IdCondicaoPagamento)
                    .HasColumnName("ID_Condicao_Pagamento")
                    .ValueGeneratedNever();

                entity.Property(e => e.CondicaoPagamento)
                    .IsRequired()
                    .HasColumnName("Condicao_Pagamento")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ContratoEstado>(entity =>
            {
                entity.HasKey(e => e.IdEstado);

                entity.ToTable("Contrato_Estado");

                entity.Property(e => e.IdEstado).HasColumnName("ID_Estado");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ContratoLinha>(entity =>
            {
                entity.HasKey(e => e.IdContratoLinha);

                entity.ToTable("Contrato_Linha");

                entity.Property(e => e.IdContratoLinha).HasColumnName("ID_Contrato_Linha");

                entity.Property(e => e.AreaNav)
                    .HasColumnName("Area_NAV")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Descricao)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EquipaNav)
                    .HasColumnName("Equipa_NAV")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdArea).HasColumnName("ID_Area");

                entity.Property(e => e.IdAreaOp).HasColumnName("ID_AreaOp");

                entity.Property(e => e.IdContrato)
                    .IsRequired()
                    .HasColumnName("ID_Contrato")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdEquipa).HasColumnName("ID_Equipa");

                entity.Property(e => e.IdEstado).HasColumnName("ID_Estado");

                entity.Property(e => e.IdRegiao).HasColumnName("ID_Regiao");

                entity.Property(e => e.LinhaNum).HasColumnName("Linha_Num");

                entity.Property(e => e.NumHorasIntervencao)
                    .HasColumnName("Num_Horas_Intervencao")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.NumTecnicos).HasColumnName("Num_Tecnicos");

                entity.Property(e => e.NumTecnicosNav)
                    .HasColumnName("Num_Tecnicos_NAV")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.NumVersao).HasColumnName("Num_Versao");

                entity.Property(e => e.Obs)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Quantidade).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RegiaoNav)
                    .HasColumnName("Regiao_NAV")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ValorMensal)
                    .HasColumnName("Valor_Mensal")
                    .HasColumnType("money");
            });

            modelBuilder.Entity<ContratoTipo>(entity =>
            {
                entity.HasKey(e => e.IdTipoContrato);

                entity.ToTable("Contrato_Tipo");

                entity.Property(e => e.IdTipoContrato).HasColumnName("ID_Tipo_Contrato");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ContratoTipoRequisito>(entity =>
            {
                entity.HasKey(e => e.IdTipoRequisito);

                entity.ToTable("Contrato_Tipo_Requisito");

                entity.Property(e => e.IdTipoRequisito).HasColumnName("ID_Tipo_Requisito");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Grupo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Datas>(entity =>
            {
                entity.HasKey(e => e.IdSemana);

                entity.Property(e => e.IdSemana).HasColumnName("ID_Semana");

                entity.Property(e => e.Fim).HasColumnType("datetime");

                entity.Property(e => e.Inicio).HasColumnType("datetime");
            });

            modelBuilder.Entity<DiariosRegAut>(entity =>
            {
                entity.HasKey(e => e.Tipo)
                    .HasName("Diarios Reg_ Aut_$0");

                entity.ToTable("Diarios Reg_ Aut_");

                entity.Property(e => e.Tipo).ValueGeneratedNever();

                entity.Property(e => e.LivroDiário)
                    .IsRequired()
                    .HasColumnName("Livro Diário")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Recurso)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SecçãoDoDiário)
                    .IsRequired()
                    .HasColumnName("Secção do Diário")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.TipoDesc)
                    .IsRequired()
                    .HasColumnName("Tipo_Desc")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmmAnexos>(entity =>
            {
                entity.ToTable("EMM_Anexos");

                entity.HasIndex(e => new { e.IdGrupo, e.NumEmm, e.Activo });

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");

                entity.Property(e => e.Data)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Extensao)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdEquipamento).HasColumnName("ID_Equipamento");

                entity.Property(e => e.IdGrupo).HasColumnName("ID_Grupo");

                entity.Property(e => e.IdUtilizador).HasColumnName("ID_Utilizador");

                entity.Property(e => e.Nome)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NumEmm).HasColumnName("Num_EMM");
            });

            modelBuilder.Entity<EmmAnexosTmp>(entity =>
            {
                entity.ToTable("EMM_Anexos_TMP");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Anexo).IsRequired();

                entity.Property(e => e.Data)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Extensao)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdGrupo).HasColumnName("ID_Grupo");

                entity.Property(e => e.IdUtilizador).HasColumnName("ID_Utilizador");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmmCalibracao>(entity =>
            {
                entity.ToTable("EMM_Calibracao");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Calibracao)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmmCategorias>(entity =>
            {
                entity.ToTable("EMM_Categorias");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Categoria)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmmEntidade>(entity =>
            {
                entity.ToTable("EMM_Entidade");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Entidade)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmmEquipamentoAcessorios>(entity =>
            {
                entity.ToTable("EMM_Equipamento_Acessorios");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AcessorioNumEmm).HasColumnName("Acessorio_Num_EMM");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.EquipamentoNumEmm).HasColumnName("Equipamento_Num_EMM");
            });

            modelBuilder.Entity<EmmEquipamentoAcessoriosTmp>(entity =>
            {
                entity.ToTable("EMM_Equipamento_Acessorios_TMP");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AcessorioNumEmm).HasColumnName("Acessorio_Num_EMM");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.EquipamentoNumEmm).HasColumnName("Equipamento_Num_EMM");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmmEquipamentos>(entity =>
            {
                entity.HasKey(e => e.NumEmm)
                    .HasName("PK_EMM_Equipamentos_1");

                entity.ToTable("EMM_Equipamentos");

                entity.HasIndex(e => new { e.NumEmm, e.IdTipo, e.TipoDescricao, e.IdGrupo, e.Activo, e.IdEstado })
                    .HasName("IX_EMM_Equipamentos_ID_Grupo_Activo_ID_Estado");

                entity.HasIndex(e => new { e.NumEmm, e.IdTipo, e.TipoDescricao, e.IdGrupo, e.IdEquipamento, e.IdEstado })
                    .HasName("IX_EMM_Equipamentos_ID_Grupo_ID_Equipamento_ID_Estado");

                entity.Property(e => e.NumEmm)
                    .HasColumnName("Num_EMM")
                    .ValueGeneratedNever();

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DataAbate)
                    .HasColumnName("Data_Abate")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataCompra)
                    .HasColumnName("Data_Compra")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataProximaInspecao)
                    .HasColumnName("Data_Proxima_Inspecao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataUltimaInspecao)
                    .HasColumnName("Data_Ultima_Inspecao")
                    .HasColumnType("datetime");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IdArea)
                    .HasColumnName("ID_Area")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdAreaUp)
                    .HasColumnName("ID_Area_UP")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdCalibracao).HasColumnName("ID_Calibracao");

                entity.Property(e => e.IdCategoria).HasColumnName("ID_Categoria");

                entity.Property(e => e.IdCresp)
                    .HasColumnName("ID_Cresp")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdEquipamento).HasColumnName("ID_Equipamento");

                entity.Property(e => e.IdEstado).HasColumnName("ID_Estado");

                entity.Property(e => e.IdFornecedor)
                    .HasColumnName("ID_Fornecedor")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdGrupo).HasColumnName("ID_Grupo");

                entity.Property(e => e.IdMarca).HasColumnName("ID_Marca");

                entity.Property(e => e.IdModelo).HasColumnName("ID_Modelo");

                entity.Property(e => e.IdPeriocidade).HasColumnName("ID_Periocidade");

                entity.Property(e => e.IdRegiao)
                    .HasColumnName("ID_Regiao")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdResponsavel)
                    .HasColumnName("ID_Responsavel")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdServico).HasColumnName("ID_Servico");

                entity.Property(e => e.IdSupervisor)
                    .HasColumnName("ID_Supervisor")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdTipo).HasColumnName("ID_Tipo");

                entity.Property(e => e.Informacao)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.NumPatrimonio)
                    .HasColumnName("Num_Patrimonio")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NumSerie)
                    .HasColumnName("Num_Serie")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TipoDescricao)
                    .HasColumnName("Tipo_Descricao")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsadoProcesso)
                    .HasColumnName("Usado_Processo")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmmEquipamentosEstados>(entity =>
            {
                entity.ToTable("EMM_Equipamentos_Estados");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmmEquipamentosServicos>(entity =>
            {
                entity.ToTable("EMM_Equipamentos_Servicos");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Servico)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmmGamaUtilizacao>(entity =>
            {
                entity.ToTable("EMM_Gama_Utilizacao");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Ema)
                    .HasColumnName("EMA")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdGrupo).HasColumnName("ID_Grupo");

                entity.Property(e => e.NumEmm).HasColumnName("Num_EMM");

                entity.Property(e => e.Parametro)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PontosInspecao)
                    .HasColumnName("Pontos_Inspecao")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Tolerancia)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorMaximo)
                    .HasColumnName("Valor_Maximo")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorMinimo)
                    .HasColumnName("Valor_Minimo")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmmGamaUtilizacaoTmp>(entity =>
            {
                entity.ToTable("EMM_Gama_Utilizacao_TMP");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Ema)
                    .HasColumnName("EMA")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdGrupo).HasColumnName("ID_Grupo");

                entity.Property(e => e.Parametro)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PontosInspecao)
                    .HasColumnName("Pontos_Inspecao")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Tolerancia)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorMaximo)
                    .HasColumnName("Valor_Maximo")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValorMinimo)
                    .HasColumnName("Valor_Minimo")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmmGrupos>(entity =>
            {
                entity.ToTable("EMM_Grupos");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Grupo)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmmHistorico>(entity =>
            {
                entity.ToTable("EMM_Historico");

                entity.HasIndex(e => e.Activo)
                    .HasName("_dta_index_EMM_Historico_6_1371203985__K8");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Data).HasColumnType("datetime");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.IdAccao).HasColumnName("ID_Accao");

                entity.Property(e => e.IdTabela).HasColumnName("ID_Tabela");

                entity.Property(e => e.IdUtilizador).HasColumnName("ID_Utilizador");

                entity.Property(e => e.NumEmm).HasColumnName("Num_EMM");
            });

            modelBuilder.Entity<EmmHistoricoTabelas>(entity =>
            {
                entity.ToTable("EMM_Historico_Tabelas");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Tabela)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmmInspecaoAssinaturas>(entity =>
            {
                entity.ToTable("EMM_Inspecao_Assinaturas");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FileName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IdSupervisor)
                    .HasColumnName("ID_Supervisor")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmmInspecaoPadrao>(entity =>
            {
                entity.ToTable("EMM_Inspecao_Padrao");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Erro).HasColumnType("decimal(12, 4)");

                entity.Property(e => e.ErroIncerteza)
                    .HasColumnName("Erro_Incerteza")
                    .HasColumnType("decimal(12, 4)");

                entity.Property(e => e.IdInspecao).HasColumnName("ID_Inspecao");

                entity.Property(e => e.IdSetPoint).HasColumnName("ID_SetPoint");

                entity.Property(e => e.Incerteza).HasColumnType("decimal(12, 4)");

                entity.Property(e => e.PontoCalibracao)
                    .HasColumnName("Ponto_Calibracao")
                    .HasColumnType("decimal(12, 4)");
            });

            modelBuilder.Entity<EmmInspecaoPadraoTmp>(entity =>
            {
                entity.ToTable("EMM_Inspecao_Padrao_TMP");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Erro).HasColumnType("decimal(12, 4)");

                entity.Property(e => e.ErroIncerteza)
                    .HasColumnName("Erro_Incerteza")
                    .HasColumnType("decimal(12, 4)");

                entity.Property(e => e.IdSetPoint).HasColumnName("ID_SetPoint");

                entity.Property(e => e.Incerteza).HasColumnType("decimal(12, 4)");

                entity.Property(e => e.PontoCalibracao)
                    .HasColumnName("Ponto_Calibracao")
                    .HasColumnType("decimal(12, 4)");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmmInspecaoRotina>(entity =>
            {
                entity.ToTable("EMM_Inspecao_Rotina");

                entity.HasIndex(e => e.IdInspecao);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CalculoAceitacao).HasColumnName("Calculo_Aceitacao");

                entity.Property(e => e.Ema)
                    .HasColumnName("EMA")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.EmmPadraoAcessorio).HasColumnName("EMM_Padrao_Acessorio");

                entity.Property(e => e.EmmPadraoEquipamento).HasColumnName("EMM_Padrao_Equipamento");

                entity.Property(e => e.ErroIncerteza)
                    .HasColumnName("Erro_Incerteza")
                    .HasColumnType("decimal(12, 4)");

                entity.Property(e => e.ErroT)
                    .HasColumnName("ERRO_t")
                    .HasColumnType("decimal(12, 4)");

                entity.Property(e => e.IdGrupo).HasColumnName("ID_Grupo");

                entity.Property(e => e.IdInspecao).HasColumnName("ID_Inspecao");

                entity.Property(e => e.IdSetPoint).HasColumnName("ID_SetPoint");

                entity.Property(e => e.LmP)
                    .HasColumnName("LM_p")
                    .HasColumnType("decimal(12, 4)");

                entity.Property(e => e.LmT)
                    .HasColumnName("LM_t")
                    .HasColumnType("decimal(12, 4)");

                entity.Property(e => e.NumEmm).HasColumnName("Num_EMM");

                entity.Property(e => e.NumMedida).HasColumnName("Num_Medida");

                entity.Property(e => e.PontoCalibracao)
                    .HasColumnName("Ponto_Calibracao")
                    .HasColumnType("decimal(12, 4)");
            });

            modelBuilder.Entity<EmmInspecaoRotinaTmp>(entity =>
            {
                entity.ToTable("EMM_Inspecao_Rotina_TMP");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CalculoAceitacao).HasColumnName("Calculo_Aceitacao");

                entity.Property(e => e.Ema)
                    .HasColumnName("EMA")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.EmmPadraoAcessorio).HasColumnName("EMM_Padrao_Acessorio");

                entity.Property(e => e.EmmPadraoEquipamento).HasColumnName("EMM_Padrao_Equipamento");

                entity.Property(e => e.ErroIncerteza)
                    .HasColumnName("Erro_Incerteza")
                    .HasColumnType("decimal(12, 4)");

                entity.Property(e => e.ErroT)
                    .HasColumnName("ERRO_t")
                    .HasColumnType("decimal(12, 4)");

                entity.Property(e => e.IdGrupo).HasColumnName("ID_Grupo");

                entity.Property(e => e.IdSetPoint).HasColumnName("ID_SetPoint");

                entity.Property(e => e.LmP)
                    .HasColumnName("LM_p")
                    .HasColumnType("decimal(12, 4)");

                entity.Property(e => e.LmT)
                    .HasColumnName("LM_t")
                    .HasColumnType("decimal(12, 4)");

                entity.Property(e => e.NumEmm).HasColumnName("Num_EMM");

                entity.Property(e => e.NumMedida).HasColumnName("Num_Medida");

                entity.Property(e => e.PontoCalibracao)
                    .HasColumnName("Ponto_Calibracao")
                    .HasColumnType("decimal(12, 4)");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmmInspeccoes>(entity =>
            {
                entity.HasKey(e => e.IdInspecao);

                entity.ToTable("EMM_Inspeccoes");

                entity.HasIndex(e => new { e.IdInspecao, e.NumEmm, e.IdResultado })
                    .HasName("IX_EMM_Inspeccoes_ID_Resultado");

                entity.HasIndex(e => new { e.NumEmm, e.Activo, e.IdResultado });

                entity.Property(e => e.IdInspecao)
                    .HasColumnName("ID_Inspecao")
                    .ValueGeneratedNever();

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DataEstado)
                    .HasColumnName("Data_Estado")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataInspecao)
                    .HasColumnName("Data_Inspecao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataLimite)
                    .HasColumnName("Data_Limite")
                    .HasColumnType("datetime");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IdEntidade).HasColumnName("ID_Entidade");

                entity.Property(e => e.IdEstado).HasColumnName("ID_Estado");

                entity.Property(e => e.IdGrupo).HasColumnName("ID_Grupo");

                entity.Property(e => e.IdLaboratorio)
                    .HasColumnName("ID_Laboratorio")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdResultado).HasColumnName("ID_Resultado");

                entity.Property(e => e.IdSupervisor)
                    .HasColumnName("ID_Supervisor")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdTipo).HasColumnName("ID_Tipo");

                entity.Property(e => e.Informacao)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.NumCertificado)
                    .HasColumnName("Num_Certificado")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumEmm).HasColumnName("Num_EMM");

                entity.Property(e => e.NumRequesicao)
                    .HasColumnName("Num_Requesicao")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Observacao)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.TempAmbiente)
                    .HasColumnName("Temp_Ambiente")
                    .HasColumnType("decimal(4, 2)");
            });

            modelBuilder.Entity<EmmInspeccoesEstados>(entity =>
            {
                entity.ToTable("EMM_Inspeccoes_Estados");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmmInspeccoesResultados>(entity =>
            {
                entity.ToTable("EMM_Inspeccoes_Resultados");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Resultado)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmmInspeccoesTipos>(entity =>
            {
                entity.ToTable("EMM_Inspeccoes_Tipos");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmmLaboratorio>(entity =>
            {
                entity.ToTable("EMM_Laboratorio");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Laboratorio)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmmPeriodicidade>(entity =>
            {
                entity.ToTable("EMM_Periodicidade");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Abreviatura)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Periodicidade)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TotalDias).HasColumnName("Total_Dias");
            });

            modelBuilder.Entity<EmmPermissao>(entity =>
            {
                entity.ToTable("EMM_Permissao");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CodeArea)
                    .IsRequired()
                    .HasColumnName("Code_Area")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NumColaborador)
                    .IsRequired()
                    .HasColumnName("Num_Colaborador")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmmSetPoints>(entity =>
            {
                entity.ToTable("EMM_SetPoints");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<EmmTipo>(entity =>
            {
                entity.ToTable("EMM_Tipo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmmUtilizadores>(entity =>
            {
                entity.ToTable("EMM_Utilizadores");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IdGrupo).HasColumnName("ID_Grupo");

                entity.Property(e => e.IdUtilizador)
                    .IsRequired()
                    .HasColumnName("ID_Utilizador")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NumEmm).HasColumnName("Num_EMM");
            });

            modelBuilder.Entity<EmmUtilizadoresTmp>(entity =>
            {
                entity.ToTable("EMM_Utilizadores_TMP");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IdGrupo).HasColumnName("ID_Grupo");

                entity.Property(e => e.IdUtilizador)
                    .IsRequired()
                    .HasColumnName("ID_Utilizador")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EnderecoSuch>(entity =>
            {
                entity.HasKey(e => e.IdEndereco);

                entity.ToTable("Endereco_SUCH");

                entity.Property(e => e.IdEndereco).HasColumnName("ID_Endereco");

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

            modelBuilder.Entity<EquipCategoria>(entity =>
            {
                entity.HasKey(e => e.IdCategoria)
                    .HasName("PK_Categoria");

                entity.ToTable("Equip_Categoria");

                entity.Property(e => e.IdCategoria).HasColumnName("ID_Categoria");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Emm).HasColumnName("EMM");

                entity.Property(e => e.FichaManutencao)
                    .HasColumnName("Ficha_Manutencao")
                    .HasMaxLength(20);

                entity.Property(e => e.IdFamilia).HasColumnName("ID_Familia");

                entity.Property(e => e.IdGrupo).HasColumnName("ID_Grupo");

                entity.Property(e => e.IdTipo).HasColumnName("ID_Tipo");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.HasOne(d => d.IdFamiliaNavigation)
                    .WithMany(p => p.EquipCategoria)
                    .HasForeignKey(d => d.IdFamilia)
                    .HasConstraintName("FK_Equip_Categoria_Familia");

                entity.HasOne(d => d.IdGrupoNavigation)
                    .WithMany(p => p.EquipCategoria)
                    .HasForeignKey(d => d.IdGrupo)
                    .HasConstraintName("FK_Equip_Categoria_Grupo");

                entity.HasOne(d => d.IdTipoNavigation)
                    .WithMany(p => p.EquipCategoria)
                    .HasForeignKey(d => d.IdTipo)
                    .HasConstraintName("FK_Equip_Categoria_Equip_Tipo");
            });

            modelBuilder.Entity<EquipDadosTecnicos>(entity =>
            {
                entity.HasKey(e => e.IdEquipDadosTecnicos);

                entity.ToTable("Equip_Dados_Tecnicos");

                entity.HasIndex(e => new { e.IdEquipParametro, e.IdEquipamento })
                    .HasName("IX_Equip_Dados_Tecnicos")
                    .IsUnique();

                entity.Property(e => e.IdEquipDadosTecnicos).HasColumnName("ID_Equip_Dados_Tecnicos");

                entity.Property(e => e.IdEquipParametro).HasColumnName("ID_Equip_Parametro");

                entity.Property(e => e.IdEquipamento).HasColumnName("ID_Equipamento");

                entity.Property(e => e.Valor).HasColumnType("decimal(9, 2)");

                entity.HasOne(d => d.IdEquipParametroNavigation)
                    .WithMany(p => p.EquipDadosTecnicos)
                    .HasForeignKey(d => d.IdEquipParametro)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Equip_Dados_Tecnicos_Equip_Parametro");

                entity.HasOne(d => d.IdEquipamentoNavigation)
                    .WithMany(p => p.EquipDadosTecnicos)
                    .HasForeignKey(d => d.IdEquipamento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Equip_Dados_Tecnicos_Equipamento");
            });

            modelBuilder.Entity<EquipDependente>(entity =>
            {
                entity.HasKey(e => e.IdEquipDependente);

                entity.ToTable("Equip_Dependente");

                entity.HasIndex(e => new { e.IdEquipPrincipal, e.IdEquipSecundario })
                    .HasName("IX_Equip_Dependente")
                    .IsUnique();

                entity.Property(e => e.IdEquipDependente).HasColumnName("ID_Equip_Dependente");

                entity.Property(e => e.IdEquipPrincipal).HasColumnName("ID_Equip_Principal");

                entity.Property(e => e.IdEquipSecundario).HasColumnName("ID_Equip_Secundario");

                entity.HasOne(d => d.IdEquipPrincipalNavigation)
                    .WithMany(p => p.EquipDependenteIdEquipPrincipalNavigation)
                    .HasForeignKey(d => d.IdEquipPrincipal)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Equip_Dependente_Equipamento");

                entity.HasOne(d => d.IdEquipSecundarioNavigation)
                    .WithMany(p => p.EquipDependenteIdEquipSecundarioNavigation)
                    .HasForeignKey(d => d.IdEquipSecundario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Equip_Dependente_Equipamento1");
            });

            modelBuilder.Entity<EquipEstado>(entity =>
            {
                entity.HasKey(e => e.IdEquipEstado);

                entity.ToTable("Equip_Estado");

                entity.Property(e => e.IdEquipEstado).HasColumnName("ID_Equip_Estado");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<EquipFicheiros>(entity =>
            {
                entity.HasKey(e => e.IdEquipFicheiro);

                entity.ToTable("Equip_Ficheiros");

                entity.Property(e => e.IdEquipFicheiro).HasColumnName("ID_Equip_Ficheiro");

                entity.Property(e => e.Data).HasColumnType("datetime");

                entity.Property(e => e.Extensao).HasMaxLength(50);

                entity.Property(e => e.IdEquipamento).HasColumnName("ID_Equipamento");

                entity.Property(e => e.Nome).HasMaxLength(50);
            });

            modelBuilder.Entity<EquipMarca>(entity =>
            {
                entity.HasKey(e => e.IdMarca)
                    .HasName("PK_Marca");

                entity.ToTable("Equip_Marca");

                entity.Property(e => e.IdMarca).HasColumnName("ID_Marca");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<EquipModelo>(entity =>
            {
                entity.HasKey(e => e.IdModelo)
                    .HasName("PK_Modelo");

                entity.ToTable("Equip_Modelo");

                entity.HasIndex(e => e.Activo)
                    .HasName("_dta_index_Equip_Modelo_6_377768403__K6");

                entity.HasIndex(e => e.IdCategoria);

                entity.HasIndex(e => e.IdMarca)
                    .HasName("_dta_index_Equip_Modelo_6_377768403__K4_f1")
                    .HasFilter("([Equip_Modelo].[Activo]=(1))");

                entity.HasIndex(e => new { e.IdMarca, e.Activo })
                    .HasName("_dta_index_Equip_Modelo_6_377768403__K4_K6_f2")
                    .HasFilter("([Equip_Modelo].[Activo]=(1))");

                entity.HasIndex(e => new { e.IdMarca, e.Activo, e.IdModelos })
                    .HasName("_dta_index_Equip_Modelo_6_377768403__K4_K6_K5_f6")
                    .HasFilter("([Equip_Modelo].[Activo]=(1))");

                entity.HasIndex(e => new { e.IdModelos, e.IdMarca, e.Activo })
                    .HasName("_dta_stat_377768403_5_4_6")
                    .HasFilter("([Equip_Modelo].[Activo]=(1))");

                entity.HasIndex(e => new { e.IdModelo, e.Activo, e.IdMarca, e.IdModelos })
                    .HasName("_dta_index_Equip_Modelo_6_377768403__K6_K4_K5_1");

                entity.HasIndex(e => new { e.IdModelo, e.IdMarca, e.Activo, e.IdModelos })
                    .HasName("_dta_index_Equip_Modelo_6_377768403__K4_K6_K5_1_f4")
                    .HasFilter("([Equip_Modelo].[Activo]=(1))");

                entity.HasIndex(e => new { e.IdModelo, e.IdModelos, e.IdMarca, e.Activo })
                    .HasName("_dta_index_Equip_Modelo_6_377768403__K5_K4_K6_1_f3")
                    .HasFilter("([Equip_Modelo].[Activo]=(1))");

                entity.HasIndex(e => new { e.IdModelo, e.IdCategoria, e.IdMarca, e.IdModelos, e.Activo })
                    .HasName("_dta_index_Equip_Modelo_6_377768403__col__");

                entity.Property(e => e.IdModelo).HasColumnName("ID_Modelo");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IdCategoria).HasColumnName("ID_Categoria");

                entity.Property(e => e.IdMarca).HasColumnName("ID_Marca");

                entity.Property(e => e.IdModelos).HasColumnName("ID_Modelos");

                entity.HasOne(d => d.IdCategoriaNavigation)
                    .WithMany(p => p.EquipModelo)
                    .HasForeignKey(d => d.IdCategoria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Equip_Modelo_Equip_Designacao");
            });

            modelBuilder.Entity<EquipParametro>(entity =>
            {
                entity.HasKey(e => e.IdEquipParametro);

                entity.ToTable("Equip_Parametro");

                entity.Property(e => e.IdEquipParametro).HasColumnName("ID_Equip_Parametro");

                entity.Property(e => e.Parametro)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Unidades)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<EquipPimp>(entity =>
            {
                entity.HasKey(e => e.IdEquipPimp);

                entity.ToTable("Equip_PIMP");

                entity.HasIndex(e => new { e.IdEquipamento, e.Ano });

                entity.Property(e => e.IdEquipPimp).HasColumnName("ID_Equip_PIMP");

                entity.Property(e => e.DataAlteracao)
                    .HasColumnName("Data_Alteracao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataExecucao)
                    .HasColumnName("Data_Execucao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataPlano)
                    .HasColumnName("Data_Plano")
                    .HasColumnType("datetime");

                entity.Property(e => e.FolhaAssociada)
                    .HasColumnName("Folha_Associada")
                    .HasMaxLength(50);

                entity.Property(e => e.IdCliente).HasColumnName("ID_Cliente");

                entity.Property(e => e.IdContrato)
                    .HasColumnName("ID_Contrato")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdEquipa).HasColumnName("ID_Equipa");

                entity.Property(e => e.IdEquipamento).HasColumnName("ID_Equipamento");

                entity.Property(e => e.IdRotina).HasColumnName("ID_Rotina");

                entity.Property(e => e.IdUtilizadorAlteracao).HasColumnName("ID_Utilizador_Alteracao");

                entity.Property(e => e.PlanoExecutado).HasColumnName("Plano_Executado");

                entity.Property(e => e.Replicado).HasDefaultValueSql("((0))");

                entity.Property(e => e.ResultadoPimp).HasColumnName("Resultado_PIMP");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.EquipPimp)
                    .HasForeignKey(d => d.IdCliente)
                    .HasConstraintName("FK_Equip_PIMP_Cliente");

                entity.HasOne(d => d.IdContratoNavigation)
                    .WithMany(p => p.EquipPimp)
                    .HasForeignKey(d => d.IdContrato)
                    .HasConstraintName("FK_Equip_PIMP_Contrato");

                entity.HasOne(d => d.IdEquipaNavigation)
                    .WithMany(p => p.EquipPimp)
                    .HasForeignKey(d => d.IdEquipa)
                    .HasConstraintName("FK_Equip_PIMP_Equipa");

                entity.HasOne(d => d.IdEquipamentoNavigation)
                    .WithMany(p => p.EquipPimp)
                    .HasForeignKey(d => d.IdEquipamento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Equip_PIMP_Equipamento");

                entity.HasOne(d => d.IdRotinaNavigation)
                    .WithMany(p => p.EquipPimp)
                    .HasForeignKey(d => d.IdRotina)
                    .HasConstraintName("FK_Equip_PIMP_Rotina");

                entity.HasOne(d => d.IdUtilizadorAlteracaoNavigation)
                    .WithMany(p => p.EquipPimpIdUtilizadorAlteracaoNavigation)
                    .HasForeignKey(d => d.IdUtilizadorAlteracao)
                    .HasConstraintName("FK_Equip_PIMP_Utilizador1");

                entity.HasOne(d => d.PlanoExecutadoNavigation)
                    .WithMany(p => p.EquipPimp)
                    .HasForeignKey(d => d.PlanoExecutado)
                    .HasConstraintName("FK_Equip_PIMP_Plano_Executado");

                entity.HasOne(d => d.ResultadoPimpNavigation)
                    .WithMany(p => p.EquipPimp)
                    .HasForeignKey(d => d.ResultadoPimp)
                    .HasConstraintName("FK_Equip_PIMP_Equip_Estado");

                entity.HasOne(d => d.TecnicoNavigation)
                    .WithMany(p => p.EquipPimpTecnicoNavigation)
                    .HasForeignKey(d => d.Tecnico)
                    .HasConstraintName("FK_Equip_PIMP_Utilizador");
            });

            modelBuilder.Entity<EquipTipo>(entity =>
            {
                entity.HasKey(e => e.IdTipo);

                entity.ToTable("Equip_Tipo");

                entity.Property(e => e.IdTipo).HasColumnName("ID_Tipo");

                entity.Property(e => e.Designacao)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Equipa>(entity =>
            {
                entity.HasKey(e => e.IdEquipa);

                entity.Property(e => e.IdEquipa).HasColumnName("ID_Equipa");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdArea).HasColumnName("ID_Area");

                entity.Property(e => e.IdRegiao).HasColumnName("ID_Regiao");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdAreaNavigation)
                    .WithMany(p => p.Equipa)
                    .HasForeignKey(d => d.IdArea)
                    .HasConstraintName("FK_Equipa_Area");

                entity.HasOne(d => d.IdRegiaoNavigation)
                    .WithMany(p => p.Equipa)
                    .HasForeignKey(d => d.IdRegiao)
                    .HasConstraintName("FK_Equipa_Regiao");
            });

            modelBuilder.Entity<Equipamento>(entity =>
            {
                entity.HasKey(e => e.IdEquipamento);

                entity.HasIndex(e => e.NumEquipamento);

                entity.HasIndex(e => new { e.IdServico, e.Activo, e.PorValidar });

                entity.HasIndex(e => new { e.Nome, e.NumSerie, e.NumInventario, e.NumEquipamento, e.IdEquipamento })
                    .HasName("_dta_index_Equipamento_6_1118679083__K1_2_6_7_8");

                entity.HasIndex(e => new { e.NumSerie, e.NumInventario, e.NumEquipamento, e.IdEquipamento, e.Categoria })
                    .HasName("_dta_index_Equipamento_6_1118679083__K1_K5_6_7_8");

                entity.HasIndex(e => new { e.Nome, e.NumSerie, e.NumInventario, e.NumEquipamento, e.IdEquipamento, e.Categoria })
                    .HasName("_dta_index_Equipamento_6_1118679083__K1_K5_2_6_7_8");

                entity.HasIndex(e => new { e.IdEquipamento, e.Nome, e.Marca, e.Modelo, e.Categoria, e.NumSerie, e.NumInventario, e.NumEquipamento, e.IdCliente, e.PorValidar })
                    .HasName("IX_Equipamento_ID_Cliente_Por_Validar");

                entity.Property(e => e.IdEquipamento).HasColumnName("ID_Equipamento");

                entity.Property(e => e.Abatido).HasDefaultValueSql("((0))");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.AlteradoPor).HasColumnName("Alterado_Por");

                entity.Property(e => e.AnoFabrico).HasColumnName("Ano_Fabrico");

                entity.Property(e => e.AssociadoContrato).HasColumnName("Associado_Contrato");

                entity.Property(e => e.CodigoBarras).HasColumnType("image");

                entity.Property(e => e.CodigoBarrasCliente).HasColumnType("image");

                entity.Property(e => e.Criticidade).HasDefaultValueSql("((0))");

                entity.Property(e => e.DataAlteracao)
                    .HasColumnName("Data_Alteracao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataAquisicao)
                    .HasColumnName("Data_Aquisicao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataEntradaContrato)
                    .HasColumnName("Data_Entrada_Contrato")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataFimGarantia)
                    .HasColumnName("Data_Fim_Garantia")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataInactivacao)
                    .HasColumnName("Data_Inactivacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataInsercao)
                    .HasColumnName("Data_Insercao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataInstalacao)
                    .HasColumnName("Data_Instalacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataSaidaContrato)
                    .HasColumnName("Data_Saida_Contrato")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataValidacao)
                    .HasColumnName("Data_Validacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DesignacaoComplementar).HasMaxLength(250);

                entity.Property(e => e.DesignacaoComplementar2).HasMaxLength(250);

                entity.Property(e => e.Foto).HasColumnType("image");

                entity.Property(e => e.Garantia).HasDefaultValueSql("((0))");

                entity.Property(e => e.IdArea).HasColumnName("ID_Area");

                entity.Property(e => e.IdAreaOp).HasColumnName("ID_AreaOp");

                entity.Property(e => e.IdCliente).HasColumnName("ID_Cliente");

                entity.Property(e => e.IdContrato)
                    .HasColumnName("ID_Contrato")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdEquipa).HasColumnName("ID_Equipa");

                entity.Property(e => e.IdFornecedor)
                    .HasColumnName("ID_Fornecedor")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdPeriodicidade).HasColumnName("ID_Periodicidade");

                entity.Property(e => e.IdRegiao).HasColumnName("ID_Regiao");

                entity.Property(e => e.IdServico).HasColumnName("ID_Servico");

                entity.Property(e => e.IncluiMc)
                    .HasColumnName("Inclui_MC")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.InseridoPor).HasColumnName("Inserido_Por");

                entity.Property(e => e.Localizacao).HasMaxLength(50);

                entity.Property(e => e.MarcaText).HasMaxLength(50);

                entity.Property(e => e.ModeloText).HasMaxLength(100);

                entity.Property(e => e.MpPlaneadas).HasColumnName("MP_Planeadas");

                entity.Property(e => e.Nome).HasMaxLength(60);

                entity.Property(e => e.NomeFornecedor)
                    .HasColumnName("Nome_Fornecedor")
                    .HasMaxLength(50);

                entity.Property(e => e.NumEquipamento)
                    .HasColumnName("Num_Equipamento")
                    .HasMaxLength(50);

                entity.Property(e => e.NumInventario)
                    .HasColumnName("Num_Inventario")
                    .HasMaxLength(50);

                entity.Property(e => e.NumSerie)
                    .HasColumnName("Num_Serie")
                    .HasMaxLength(50);

                entity.Property(e => e.Observacao).HasMaxLength(250);

                entity.Property(e => e.PorValidar)
                    .IsRequired()
                    .HasColumnName("Por_Validar")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.PrecoAquisicao)
                    .HasColumnName("Preco_Aquisicao")
                    .HasColumnType("money");

                entity.Property(e => e.Sala).HasMaxLength(100);

                entity.Property(e => e.SubContratar)
                    .HasColumnName("Sub_Contratar")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ToleranciaEquipamento).HasColumnName("Tolerancia_Equipamento");

                entity.Property(e => e.ValidadoPor).HasColumnName("Validado_Por");

                entity.HasOne(d => d.CategoriaNavigation)
                    .WithMany(p => p.Equipamento)
                    .HasForeignKey(d => d.Categoria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Equipamento_Equip_Categoria");

                entity.HasOne(d => d.IdAreaNavigation)
                    .WithMany(p => p.Equipamento)
                    .HasForeignKey(d => d.IdArea)
                    .HasConstraintName("FK_Equipamento_Area");

                entity.HasOne(d => d.IdAreaOpNavigation)
                    .WithMany(p => p.Equipamento)
                    .HasForeignKey(d => d.IdAreaOp)
                    .HasConstraintName("FK_Equipamento_AreaOp");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Equipamento)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Equipamento_Cliente");

                entity.HasOne(d => d.IdContratoNavigation)
                    .WithMany(p => p.Equipamento)
                    .HasForeignKey(d => d.IdContrato)
                    .HasConstraintName("FK_Equipamento_Contrato");

                entity.HasOne(d => d.IdEquipaNavigation)
                    .WithMany(p => p.Equipamento)
                    .HasForeignKey(d => d.IdEquipa)
                    .HasConstraintName("FK_Equipamento_Equipa");

                entity.HasOne(d => d.IdFornecedorNavigation)
                    .WithMany(p => p.Equipamento)
                    .HasForeignKey(d => d.IdFornecedor)
                    .HasConstraintName("FK_Equipamento_Fornecedor");

                entity.HasOne(d => d.IdRegiaoNavigation)
                    .WithMany(p => p.Equipamento)
                    .HasForeignKey(d => d.IdRegiao)
                    .HasConstraintName("FK_Equipamento_Regiao");

                entity.HasOne(d => d.IdServicoNavigation)
                    .WithMany(p => p.Equipamento)
                    .HasForeignKey(d => d.IdServico)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Equipamento_Servico");

                entity.HasOne(d => d.MarcaNavigation)
                    .WithMany(p => p.Equipamento)
                    .HasForeignKey(d => d.Marca)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Equipamento_Marca");

                entity.HasOne(d => d.ModeloNavigation)
                    .WithMany(p => p.Equipamento)
                    .HasForeignKey(d => d.Modelo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Equipamento_Modelo");
            });

            modelBuilder.Entity<EquipamentoAcessorio>(entity =>
            {
                entity.HasKey(e => e.IdEquipamentoAcessorio);

                entity.ToTable("Equipamento_Acessorio");

                entity.Property(e => e.IdEquipamentoAcessorio).HasColumnName("ID_Equipamento_Acessorio");

                entity.Property(e => e.IdAcessorio).HasColumnName("ID_Acessorio");

                entity.Property(e => e.IdEquipamento).HasColumnName("ID_Equipamento");

                entity.HasOne(d => d.IdAcessorioNavigation)
                    .WithMany(p => p.EquipamentoAcessorio)
                    .HasForeignKey(d => d.IdAcessorio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Equipamento_Acessorio_Acessorio");

                entity.HasOne(d => d.IdEquipamentoNavigation)
                    .WithMany(p => p.EquipamentoAcessorio)
                    .HasForeignKey(d => d.IdEquipamento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Equipamento_Acessorio_Equipamento");
            });

            modelBuilder.Entity<EstadoObra>(entity =>
            {
                entity.HasKey(e => e.IdEstadoObra);

                entity.ToTable("Estado_Obra");

                entity.Property(e => e.IdEstadoObra)
                    .HasColumnName("ID_Estado_Obra")
                    .ValueGeneratedNever();

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Familia>(entity =>
            {
                entity.HasKey(e => e.IdFamilia);

                entity.Property(e => e.IdFamilia).HasColumnName("ID_Familia");

                entity.Property(e => e.Familia1)
                    .IsRequired()
                    .HasColumnName("Familia")
                    .HasMaxLength(50);

                entity.Property(e => e.SubFamilia)
                    .HasColumnName("Sub_Familia")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Feriado>(entity =>
            {
                entity.HasKey(e => e.IdFeriado);

                entity.Property(e => e.IdFeriado).HasColumnName("ID_Feriado");

                entity.Property(e => e.DataFeriado)
                    .HasColumnName("Data_Feriado")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<FichaManutencao>(entity =>
            {
                entity.HasKey(e => new { e.Codigo, e.Versao })
                    .HasName("PK_Ficha_Inventario");

                entity.ToTable("Ficha_Manutencao");

                entity.Property(e => e.Codigo).HasMaxLength(20);

                entity.Property(e => e.Versao)
                    .HasMaxLength(10)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.AprovadaEm)
                    .HasColumnName("Aprovada_em")
                    .HasColumnType("datetime");

                entity.Property(e => e.AprovadaPor).HasColumnName("Aprovada_por");

                entity.Property(e => e.AreaOperacional)
                    .HasColumnName("Area_Operacional")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Data).HasColumnType("datetime");

                entity.Property(e => e.Designacao)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.IdCategoria).HasColumnName("ID_Categoria");

                entity.Property(e => e.IdCliente).HasColumnName("ID_Cliente");

                entity.Property(e => e.IdImagem)
                    .HasColumnName("ID_Imagem")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IdTipo).HasColumnName("ID_Tipo");

                entity.Property(e => e.ParaAprovacao).HasColumnName("Para_Aprovacao");

                entity.Property(e => e.PeriodoFim)
                    .HasColumnName("Periodo_Fim")
                    .HasColumnType("datetime");

                entity.Property(e => e.PeriodoInicio)
                    .HasColumnName("Periodo_Inicio")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<FichaManutencaoEquipamentosTeste>(entity =>
            {
                entity.HasKey(e => e.IdEquipTeste)
                    .HasName("PK_Ficha_Manutencao_Equipamentos_Teste_1");

                entity.ToTable("Ficha_Manutencao_Equipamentos_Teste");

                entity.Property(e => e.IdEquipTeste).HasColumnName("ID_Equip_Teste");

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.EquipamentoTeste).HasColumnName("Equipamento_Teste");

                entity.Property(e => e.Rotinas).IsRequired();

                entity.Property(e => e.Versao)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<FichaManutencaoManutencao>(entity =>
            {
                entity.HasKey(e => e.IdManutencao)
                    .HasName("PK_Ficha_Manutencao_Manutencao_1");

                entity.ToTable("Ficha_Manutencao_Manutencao");

                entity.Property(e => e.IdManutencao).HasColumnName("ID_Manutencao");

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.DescricaoCampo1)
                    .HasColumnName("Descricao_Campo_1")
                    .HasMaxLength(50);

                entity.Property(e => e.DescricaoCampo2)
                    .HasColumnName("Descricao_Campo_2")
                    .HasMaxLength(50);

                entity.Property(e => e.DescricaoCampo3)
                    .HasColumnName("Descricao_Campo_3")
                    .HasMaxLength(50);

                entity.Property(e => e.DescricaoCampo4)
                    .HasColumnName("Descricao_Campo_4")
                    .HasMaxLength(50);

                entity.Property(e => e.DescricaoCampo5)
                    .HasColumnName("Descricao_Campo_5")
                    .HasMaxLength(50);

                entity.Property(e => e.DescricaoCampo6)
                    .HasColumnName("Descricao_Campo_6")
                    .HasMaxLength(50);

                entity.Property(e => e.DescricaoCampo7)
                    .HasColumnName("Descricao_Campo_7")
                    .HasMaxLength(50);

                entity.Property(e => e.Ignoravel).HasDefaultValueSql("((0))");

                entity.Property(e => e.Na)
                    .HasColumnName("NA")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Rotinas).IsRequired();

                entity.Property(e => e.Subcontratacao).HasDefaultValueSql("((0))");

                entity.Property(e => e.TipoCampo1)
                    .HasColumnName("Tipo_Campo_1")
                    .HasMaxLength(20);

                entity.Property(e => e.TipoCampo2)
                    .HasColumnName("Tipo_Campo_2")
                    .HasMaxLength(20);

                entity.Property(e => e.TipoCampo3)
                    .HasColumnName("Tipo_Campo_3")
                    .HasMaxLength(20);

                entity.Property(e => e.TipoCampo4)
                    .HasColumnName("Tipo_Campo_4")
                    .HasMaxLength(20);

                entity.Property(e => e.TipoCampo5)
                    .HasColumnName("Tipo_Campo_5")
                    .HasMaxLength(20);

                entity.Property(e => e.TipoCampo6)
                    .HasColumnName("Tipo_Campo_6")
                    .HasMaxLength(20);

                entity.Property(e => e.TipoCampo7)
                    .HasColumnName("Tipo_Campo_7")
                    .HasMaxLength(20);

                entity.Property(e => e.UnidadeCampo1)
                    .HasColumnName("Unidade_Campo_1")
                    .HasMaxLength(10);

                entity.Property(e => e.UnidadeCampo2)
                    .HasColumnName("Unidade_Campo_2")
                    .HasMaxLength(10);

                entity.Property(e => e.UnidadeCampo3)
                    .HasColumnName("Unidade_Campo_3")
                    .HasMaxLength(10);

                entity.Property(e => e.UnidadeCampo4)
                    .HasColumnName("Unidade_Campo_4")
                    .HasMaxLength(10);

                entity.Property(e => e.UnidadeCampo5)
                    .HasColumnName("Unidade_Campo_5")
                    .HasMaxLength(10);

                entity.Property(e => e.UnidadeCampo6)
                    .HasColumnName("Unidade_Campo_6")
                    .HasMaxLength(10);

                entity.Property(e => e.UnidadeCampo7)
                    .HasColumnName("Unidade_Campo_7")
                    .HasMaxLength(10);

                entity.Property(e => e.UsarCampo1).HasColumnName("Usar_Campo_1");

                entity.Property(e => e.UsarCampo2).HasColumnName("Usar_Campo_2");

                entity.Property(e => e.UsarCampo3).HasColumnName("Usar_Campo_3");

                entity.Property(e => e.UsarCampo4).HasColumnName("Usar_Campo_4");

                entity.Property(e => e.UsarCampo5).HasColumnName("Usar_Campo_5");

                entity.Property(e => e.UsarCampo6).HasColumnName("Usar_Campo_6");

                entity.Property(e => e.UsarCampo7).HasColumnName("Usar_Campo_7");

                entity.Property(e => e.Versao)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<FichaManutencaoRelatorio>(entity =>
            {
                entity.ToTable("Ficha_Manutencao_Relatorio");

                entity.Property(e => e.ActualizadoEm)
                    .HasColumnName("Actualizado_Em")
                    .HasColumnType("datetime");

                entity.Property(e => e.ActualizadoPor).HasColumnName("Actualizado_Por");

                entity.Property(e => e.AssinaturaCliente).HasColumnName("Assinatura_Cliente");

                entity.Property(e => e.AssinaturaSie).HasColumnName("Assinatura_SIE");

                entity.Property(e => e.AssinaturaTecnico).HasColumnName("Assinatura_Tecnico");

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.CriadoEm)
                    .HasColumnName("Criado_Em")
                    .HasColumnType("datetime");

                entity.Property(e => e.CriadoPor).HasColumnName("Criado_Por");

                entity.Property(e => e.IdAssinaturaTecnico).HasColumnName("Id_Assinatura_Tecnico");

                entity.Property(e => e.IdEquipamento).HasColumnName("Id_Equipamento");

                entity.Property(e => e.Om)
                    .IsRequired()
                    .HasColumnName("OM")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RotinaTipo)
                    .HasColumnName("Rotina_Tipo")
                    .HasMaxLength(3);

                entity.Property(e => e.Versao)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<FichaManutencaoRelatorioEquipamentosTeste>(entity =>
            {
                entity.ToTable("Ficha_Manutencao_Relatorio_Equipamentos_Teste");

                entity.Property(e => e.IdEquipTeste).HasColumnName("Id_Equip_Teste");

                entity.Property(e => e.IdRelatorio).HasColumnName("Id_Relatorio");
            });

            modelBuilder.Entity<FichaManutencaoRelatorioManutencao>(entity =>
            {
                entity.ToTable("Ficha_Manutencao_Relatorio_Manutencao");

                entity.Property(e => e.IdManutencao).HasColumnName("Id_Manutencao");

                entity.Property(e => e.IdRelatorio).HasColumnName("Id_Relatorio");

                entity.Property(e => e.ResultadoRotina).HasColumnName("Resultado_Rotina");
            });

            modelBuilder.Entity<FichaManutencaoRelatorioTestesQualitativos>(entity =>
            {
                entity.ToTable("Ficha_Manutencao_Relatorio_Testes_Qualitativos");

                entity.Property(e => e.IdRelatorio).HasColumnName("Id_Relatorio");

                entity.Property(e => e.IdTesteQualitativos).HasColumnName("Id_Teste_Qualitativos");

                entity.Property(e => e.ResultadoRotina).HasColumnName("Resultado_Rotina");
            });

            modelBuilder.Entity<FichaManutencaoRelatorioTestesQuantitativos>(entity =>
            {
                entity.ToTable("Ficha_Manutencao_Relatorio_Testes_Quantitativos");

                entity.Property(e => e.IdRelatorio).HasColumnName("Id_Relatorio");

                entity.Property(e => e.IdTestesQuantitativos).HasColumnName("Id_Testes_Quantitativos");

                entity.Property(e => e.ResultadoRotina)
                    .IsRequired()
                    .HasColumnName("Resultado_Rotina")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<FichaManutencaoTempoEstimadoRotina>(entity =>
            {
                entity.HasKey(e => e.IdTempoEstimadoRotina);

                entity.ToTable("Ficha_Manutencao_Tempo_Estimado_Rotina");

                entity.Property(e => e.IdTempoEstimadoRotina).HasColumnName("ID_Tempo_Estimado_Rotina");

                entity.Property(e => e.CodigoFicha)
                    .HasColumnName("Codigo_Ficha")
                    .HasMaxLength(20);

                entity.Property(e => e.IdRotina).HasColumnName("ID_Rotina");

                entity.Property(e => e.TempoEstimado)
                    .HasColumnName("Tempo_Estimado")
                    .HasColumnType("decimal(6, 2)");

                entity.Property(e => e.VersaoFicha)
                    .HasColumnName("Versao_Ficha")
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<FichaManutencaoTestesQualitativos>(entity =>
            {
                entity.HasKey(e => e.IdTesteQualitativos)
                    .HasName("PK_Ficha_Manutencao_Testes_Qualitativos_1");

                entity.ToTable("Ficha_Manutencao_Testes_Qualitativos");

                entity.HasIndex(e => new { e.IdTesteQualitativos, e.Descricao, e.Codigo, e.Versao })
                    .HasName("IX_Ficha_Manutencao_Testes_Qualitativos_Codigo_Versao");

                entity.Property(e => e.IdTesteQualitativos).HasColumnName("ID_Teste_Qualitativos");

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.DescricaoCampo1)
                    .HasColumnName("Descricao_Campo_1")
                    .HasMaxLength(50);

                entity.Property(e => e.DescricaoCampo2)
                    .HasColumnName("Descricao_Campo_2")
                    .HasMaxLength(50);

                entity.Property(e => e.DescricaoCampo3)
                    .HasColumnName("Descricao_Campo_3")
                    .HasMaxLength(50);

                entity.Property(e => e.DescricaoCampo4)
                    .HasColumnName("Descricao_Campo_4")
                    .HasMaxLength(50);

                entity.Property(e => e.DescricaoCampo5)
                    .HasColumnName("Descricao_Campo_5")
                    .HasMaxLength(50);

                entity.Property(e => e.DescricaoCampo6)
                    .HasColumnName("Descricao_Campo_6")
                    .HasMaxLength(50);

                entity.Property(e => e.DescricaoCampo7)
                    .HasColumnName("Descricao_Campo_7")
                    .HasMaxLength(50);

                entity.Property(e => e.Ignoravel).HasDefaultValueSql("((0))");

                entity.Property(e => e.Na)
                    .HasColumnName("NA")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Rotinas).IsRequired();

                entity.Property(e => e.Subcontratacao).HasDefaultValueSql("((0))");

                entity.Property(e => e.TipoCampo1)
                    .HasColumnName("Tipo_Campo_1")
                    .HasMaxLength(20);

                entity.Property(e => e.TipoCampo2)
                    .HasColumnName("Tipo_Campo_2")
                    .HasMaxLength(20);

                entity.Property(e => e.TipoCampo3)
                    .HasColumnName("Tipo_Campo_3")
                    .HasMaxLength(20);

                entity.Property(e => e.TipoCampo4)
                    .HasColumnName("Tipo_Campo_4")
                    .HasMaxLength(20);

                entity.Property(e => e.TipoCampo5)
                    .HasColumnName("Tipo_Campo_5")
                    .HasMaxLength(20);

                entity.Property(e => e.TipoCampo6)
                    .HasColumnName("Tipo_Campo_6")
                    .HasMaxLength(20);

                entity.Property(e => e.TipoCampo7)
                    .HasColumnName("Tipo_Campo_7")
                    .HasMaxLength(20);

                entity.Property(e => e.UnidadeCampo1)
                    .HasColumnName("Unidade_Campo_1")
                    .HasMaxLength(10);

                entity.Property(e => e.UnidadeCampo2)
                    .HasColumnName("Unidade_Campo_2")
                    .HasMaxLength(10);

                entity.Property(e => e.UnidadeCampo3)
                    .HasColumnName("Unidade_Campo_3")
                    .HasMaxLength(10);

                entity.Property(e => e.UnidadeCampo4)
                    .HasColumnName("Unidade_Campo_4")
                    .HasMaxLength(10);

                entity.Property(e => e.UnidadeCampo5)
                    .HasColumnName("Unidade_Campo_5")
                    .HasMaxLength(10);

                entity.Property(e => e.UnidadeCampo6)
                    .HasColumnName("Unidade_Campo_6")
                    .HasMaxLength(10);

                entity.Property(e => e.UnidadeCampo7)
                    .HasColumnName("Unidade_Campo_7")
                    .HasMaxLength(10);

                entity.Property(e => e.UsarCampo1).HasColumnName("Usar_Campo_1");

                entity.Property(e => e.UsarCampo2).HasColumnName("Usar_Campo_2");

                entity.Property(e => e.UsarCampo3).HasColumnName("Usar_Campo_3");

                entity.Property(e => e.UsarCampo4).HasColumnName("Usar_Campo_4");

                entity.Property(e => e.UsarCampo5).HasColumnName("Usar_Campo_5");

                entity.Property(e => e.UsarCampo6).HasColumnName("Usar_Campo_6");

                entity.Property(e => e.UsarCampo7).HasColumnName("Usar_Campo_7");

                entity.Property(e => e.Versao)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<FichaManutencaoTestesQuantitativos>(entity =>
            {
                entity.HasKey(e => e.IdTestesQuantitativos);

                entity.ToTable("Ficha_Manutencao_Testes_Quantitativos");

                entity.Property(e => e.IdTestesQuantitativos).HasColumnName("ID_Testes_Quantitativos");

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.DescricaoCampo1)
                    .HasColumnName("Descricao_Campo_1")
                    .HasMaxLength(50);

                entity.Property(e => e.DescricaoCampo2)
                    .HasColumnName("Descricao_Campo_2")
                    .HasMaxLength(50);

                entity.Property(e => e.DescricaoCampo3)
                    .HasColumnName("Descricao_Campo_3")
                    .HasMaxLength(50);

                entity.Property(e => e.DescricaoCampo4)
                    .HasColumnName("Descricao_Campo_4")
                    .HasMaxLength(50);

                entity.Property(e => e.DescricaoCampo5)
                    .HasColumnName("Descricao_Campo_5")
                    .HasMaxLength(50);

                entity.Property(e => e.DescricaoCampo6)
                    .HasColumnName("Descricao_Campo_6")
                    .HasMaxLength(50);

                entity.Property(e => e.DescricaoCampo7)
                    .HasColumnName("Descricao_Campo_7")
                    .HasMaxLength(50);

                entity.Property(e => e.Ignoravel).HasDefaultValueSql("((0))");

                entity.Property(e => e.Na)
                    .HasColumnName("NA")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Rotinas).IsRequired();

                entity.Property(e => e.Subcontratacao).HasDefaultValueSql("((0))");

                entity.Property(e => e.TipoCampo1)
                    .HasColumnName("Tipo_Campo_1")
                    .HasMaxLength(20);

                entity.Property(e => e.TipoCampo2)
                    .HasColumnName("Tipo_Campo_2")
                    .HasMaxLength(20);

                entity.Property(e => e.TipoCampo3)
                    .HasColumnName("Tipo_Campo_3")
                    .HasMaxLength(20);

                entity.Property(e => e.TipoCampo4)
                    .HasColumnName("Tipo_Campo_4")
                    .HasMaxLength(20);

                entity.Property(e => e.TipoCampo5)
                    .HasColumnName("Tipo_Campo_5")
                    .HasMaxLength(20);

                entity.Property(e => e.TipoCampo6)
                    .HasColumnName("Tipo_Campo_6")
                    .HasMaxLength(20);

                entity.Property(e => e.TipoCampo7)
                    .HasColumnName("Tipo_Campo_7")
                    .HasMaxLength(20);

                entity.Property(e => e.UnidadeCampo1)
                    .HasColumnName("Unidade_Campo_1")
                    .HasMaxLength(10);

                entity.Property(e => e.UnidadeCampo2)
                    .HasColumnName("Unidade_Campo_2")
                    .HasMaxLength(10);

                entity.Property(e => e.UnidadeCampo3)
                    .HasColumnName("Unidade_Campo_3")
                    .HasMaxLength(10);

                entity.Property(e => e.UnidadeCampo4)
                    .HasColumnName("Unidade_Campo_4")
                    .HasMaxLength(10);

                entity.Property(e => e.UnidadeCampo5)
                    .HasColumnName("Unidade_Campo_5")
                    .HasMaxLength(10);

                entity.Property(e => e.UnidadeCampo6)
                    .HasColumnName("Unidade_Campo_6")
                    .HasMaxLength(10);

                entity.Property(e => e.UnidadeCampo7)
                    .HasColumnName("Unidade_Campo_7")
                    .HasMaxLength(10);

                entity.Property(e => e.UsarCampo1).HasColumnName("Usar_Campo_1");

                entity.Property(e => e.UsarCampo2).HasColumnName("Usar_Campo_2");

                entity.Property(e => e.UsarCampo3).HasColumnName("Usar_Campo_3");

                entity.Property(e => e.UsarCampo4).HasColumnName("Usar_Campo_4");

                entity.Property(e => e.UsarCampo5).HasColumnName("Usar_Campo_5");

                entity.Property(e => e.UsarCampo6).HasColumnName("Usar_Campo_6");

                entity.Property(e => e.UsarCampo7).HasColumnName("Usar_Campo_7");

                entity.Property(e => e.Versao)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<Fornecedor>(entity =>
            {
                entity.HasKey(e => e.IdFornecedor);

                entity.HasIndex(e => new { e.IdFornecedor, e.Nome, e.Activo })
                    .HasName("IX_Fornecedor_Activo");

                entity.Property(e => e.IdFornecedor)
                    .HasColumnName("ID_Fornecedor")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CodPostal)
                    .HasColumnName("Cod_Postal")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.Fax)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Morada)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nif)
                    .HasColumnName("NIF")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NomeContacto)
                    .HasColumnName("Nome_Contacto")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Telefone)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.TelefoneContacto)
                    .HasColumnName("Telefone_Contacto")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Website)
                    .HasMaxLength(80)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Grupo>(entity =>
            {
                entity.HasKey(e => e.IdGrupo);

                entity.Property(e => e.IdGrupo).HasColumnName("ID_Grupo");

                entity.Property(e => e.Designacao)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Imagens>(entity =>
            {
                entity.HasKey(e => e.IdImagem);

                entity.Property(e => e.IdImagem).HasColumnName("ID_Imagem");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ContentType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Ficheiro).IsRequired();

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Instituicao>(entity =>
            {
                entity.HasKey(e => e.IdInstituicao);

                entity.HasIndex(e => e.Cliente);

                entity.HasIndex(e => e.IdInstituicao)
                    .HasName("_dta_index_Instituicao_6_814625945__K1_6497");

                entity.HasIndex(e => new { e.DescricaoTreePath, e.Activo })
                    .HasName("_dta_index_Instituicao_6_814625945__K6_7_8576");

                entity.HasIndex(e => new { e.DescricaoTreePath, e.IdInstituicao })
                    .HasName("_dta_index_Instituicao_6_814625945__K1_7_6497");

                entity.HasIndex(e => new { e.DescricaoTreePath, e.Activo, e.IdInstituicao })
                    .HasName("_dta_index_Instituicao_6_814625945__K6_K1_7_8066");

                entity.HasIndex(e => new { e.Nome, e.Mae, e.Cliente, e.Activo });

                entity.HasIndex(e => new { e.IdInstituicao, e.Nome, e.Cliente, e.DescricaoTreePath, e.NoNavision, e.Mae })
                    .HasName("_dta_index_Instituicao_6_814625945__K3_1_2_4_7_9");

                entity.HasIndex(e => new { e.Nome, e.Cliente, e.DescricaoTreePath, e.NoNavision, e.Activo, e.Mae, e.IdInstituicao })
                    .HasName("_dta_index_Instituicao_6_814625945__K6_K3_K1_2_4_7_9");

                entity.HasIndex(e => new { e.IdInstituicao, e.Nome, e.Mae, e.Cliente, e.TreePath, e.Activo, e.Morada, e.NoNavision })
                    .HasName("_dta_index_Instituicao_6_814625945__col___8066");

                entity.Property(e => e.IdInstituicao).HasColumnName("ID_Instituicao");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DescricaoTreePath)
                    .HasColumnName("Descricao_TreePath")
                    .HasColumnType("nvarchar(max)");

                entity.Property(e => e.Morada).HasMaxLength(250);

                entity.Property(e => e.NoNavision)
                    .HasColumnName("No_Navision")
                    .HasMaxLength(20);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TreePath).HasMaxLength(100);

                entity.HasOne(d => d.ClienteNavigation)
                    .WithMany(p => p.Instituicao)
                    .HasForeignKey(d => d.Cliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Instituicao_Cliente");
            });

            modelBuilder.Entity<InstituicaoPimp>(entity =>
            {
                entity.HasKey(e => e.IdInstituicaoPimp);

                entity.ToTable("Instituicao_PIMP");

                entity.Property(e => e.IdInstituicaoPimp).HasColumnName("ID_Instituicao_PIMP");

                entity.Property(e => e.DataExecucao)
                    .HasColumnName("Data_Execucao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataPlano)
                    .HasColumnName("Data_Plano")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdEquipa).HasColumnName("ID_Equipa");

                entity.Property(e => e.IdInstituicao).HasColumnName("ID_Instituicao");

                entity.Property(e => e.IdRotina).HasColumnName("ID_Rotina");

                entity.Property(e => e.PlanoExecutado).HasColumnName("Plano_Executado");

                entity.Property(e => e.Replicado).HasDefaultValueSql("((0))");

                entity.Property(e => e.ResultadoPimp).HasColumnName("Resultado_PIMP");

                entity.HasOne(d => d.IdEquipaNavigation)
                    .WithMany(p => p.InstituicaoPimp)
                    .HasForeignKey(d => d.IdEquipa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Instituicao_PIMP_Equipa");

                entity.HasOne(d => d.IdInstituicaoNavigation)
                    .WithMany(p => p.InstituicaoPimp)
                    .HasForeignKey(d => d.IdInstituicao)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Instituicao_PIMP_Instituicao");

                entity.HasOne(d => d.IdRotinaNavigation)
                    .WithMany(p => p.InstituicaoPimp)
                    .HasForeignKey(d => d.IdRotina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Instituicao_PIMP_Rotina");

                entity.HasOne(d => d.PlanoExecutadoNavigation)
                    .WithMany(p => p.InstituicaoPimp)
                    .HasForeignKey(d => d.PlanoExecutado)
                    .HasConstraintName("FK_Instituicao_PIMP_Plano_Executado");
            });

            modelBuilder.Entity<Job>(entity =>
            {
                entity.HasKey(e => e.No)
                    .HasName("Job$0");

                entity.HasIndex(e => e.ProjetoDimensoesFixas)
                    .HasName("_dta_index_Job_6_827918071__K90");

                entity.Property(e => e.No)
                    .HasColumnName("No_")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AllowScheduleContractLines).HasColumnName("Allow Schedule_Contract Lines");

                entity.Property(e => e.AreaFilter)
                    .IsRequired()
                    .HasColumnName("Area Filter")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.AreaIntervencao)
                    .IsRequired()
                    .HasColumnName("Area Intervencao")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.BillToAddress)
                    .IsRequired()
                    .HasColumnName("Bill-to Address")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BillToAddress2)
                    .IsRequired()
                    .HasColumnName("Bill-to Address 2")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BillToCity)
                    .IsRequired()
                    .HasColumnName("Bill-to City")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BillToContact)
                    .IsRequired()
                    .HasColumnName("Bill-to Contact")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BillToContactNo)
                    .IsRequired()
                    .HasColumnName("Bill-to Contact No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BillToCountryRegionCode)
                    .IsRequired()
                    .HasColumnName("Bill-to Country_Region Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.BillToCustomerNo)
                    .HasColumnName("Bill-to Customer No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BillToName)
                    .IsRequired()
                    .HasColumnName("Bill-to Name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BillToPostCode)
                    .IsRequired()
                    .HasColumnName("Bill-to Post Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CalcWipMethodUsed).HasColumnName("Calc_ WIP Method Used");

                entity.Property(e => e.CategoriaProjeto).HasColumnName("Categoria Projeto");

                entity.Property(e => e.ConfigResponsavel)
                    .IsRequired()
                    .HasColumnName("Config Responsavel")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ContractNo)
                    .IsRequired()
                    .HasColumnName("Contract No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("Creation Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.CurrencyCode)
                    .IsRequired()
                    .HasColumnName("Currency Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerDiscGroup)
                    .IsRequired()
                    .HasColumnName("Customer Disc_ Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerPriceGroup)
                    .IsRequired()
                    .HasColumnName("Customer Price Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DataChefeProjecto)
                    .HasColumnName("Data Chefe Projecto")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataIntegracao)
                    .HasColumnName("Data Integracao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataPedido)
                    .HasColumnName("Data Pedido")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataResponsavel)
                    .HasColumnName("Data Responsavel")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataUltimoMail)
                    .HasColumnName("Data Ultimo Mail")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataValidade)
                    .HasColumnName("Data Validade")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeliberaçãoCa)
                    .IsRequired()
                    .HasColumnName("Deliberação CA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description2)
                    .IsRequired()
                    .HasColumnName("Description 2")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DescriçãoDetalhada)
                    .IsRequired()
                    .HasColumnName("Descrição Detalhada")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EndingDate)
                    .HasColumnName("Ending Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ExchCalculationCost).HasColumnName("Exch_ Calculation (Cost)");

                entity.Property(e => e.ExchCalculationPrice).HasColumnName("Exch_ Calculation (Price)");

                entity.Property(e => e.GlobalDimension1Code)
                    .IsRequired()
                    .HasColumnName("Global Dimension 1 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.GlobalDimension2Code)
                    .IsRequired()
                    .HasColumnName("Global Dimension 2 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.InvoiceCurrencyCode)
                    .IsRequired()
                    .HasColumnName("Invoice Currency Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.JobPostingGroup)
                    .IsRequired()
                    .HasColumnName("Job Posting Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LanguageCode)
                    .IsRequired()
                    .HasColumnName("Language Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LastDateModified)
                    .HasColumnName("Last Date Modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.Local)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Motorista)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.MãoDeObraEDeslocações).HasColumnName("Mão de Obra e Deslocações");

                entity.Property(e => e.NoCompromisso)
                    .IsRequired()
                    .HasColumnName("No_ Compromisso")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NoSeries)
                    .IsRequired()
                    .HasColumnName("No_ Series")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NossaProposta)
                    .IsRequired()
                    .HasColumnName("Nossa Proposta")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.NºAntigoAs400)
                    .IsRequired()
                    .HasColumnName("Nº Antigo AS400")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.NºContratoOrçamento)
                    .IsRequired()
                    .HasColumnName("Nº Contrato Orçamento")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectoServiço)
                    .IsRequired()
                    .HasColumnName("Objecto Serviço")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.OnlyForMaintInvoicing).HasColumnName("Only for Maint_ Invoicing");

                entity.Property(e => e.PedidoDoCliente)
                    .IsRequired()
                    .HasColumnName("Pedido do Cliente")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PersonResponsible)
                    .IsRequired()
                    .HasColumnName("Person Responsible")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Picture).HasColumnType("image");

                entity.Property(e => e.PostedWipMethodUsed).HasColumnName("Posted WIP Method Used");

                entity.Property(e => e.Projecto)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.ProjetoDimensoesFixas).HasColumnName("Projeto Dimensoes Fixas");

                entity.Property(e => e.ProjetoInterno).HasColumnName("Projeto Interno");

                entity.Property(e => e.ProjetoMigrado).HasColumnName("Projeto Migrado");

                entity.Property(e => e.ProjetoNaoVisivel).HasColumnName("Projeto Nao Visivel");

                entity.Property(e => e.Responsavel)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SearchDescription)
                    .IsRequired()
                    .HasColumnName("Search Description")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServInternosDébInternos).HasColumnName("Serv_ Internos -Déb Internos");

                entity.Property(e => e.ServInternosFolhasDeObra).HasColumnName("Serv_ Internos -Folhas de Obra");

                entity.Property(e => e.ServInternosRequisições).HasColumnName("Serv_ Internos -Requisições");

                entity.Property(e => e.ShipToAddress)
                    .IsRequired()
                    .HasColumnName("Ship-to Address")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToAddress2)
                    .IsRequired()
                    .HasColumnName("Ship-to Address 2")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToCity)
                    .IsRequired()
                    .HasColumnName("Ship-to City")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToCode)
                    .IsRequired()
                    .HasColumnName("Ship-to Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToContact)
                    .IsRequired()
                    .HasColumnName("Ship-to Contact")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToCounty)
                    .IsRequired()
                    .HasColumnName("Ship-to County")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToName)
                    .IsRequired()
                    .HasColumnName("Ship-to Name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToName2)
                    .IsRequired()
                    .HasColumnName("Ship-to Name 2")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToPostCode)
                    .IsRequired()
                    .HasColumnName("Ship-to Post Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension3Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 3 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension4Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 4 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.StartingDate)
                    .HasColumnName("Starting Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.TipoGrupoContabOmProjecto)
                    .IsRequired()
                    .HasColumnName("Tipo Grupo Contab _OM Projecto")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TipoGrupoContabProjecto)
                    .IsRequired()
                    .HasColumnName("Tipo Grupo Contab _ Projecto")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TipoProjecto).HasColumnName("Tipo Projecto");

                entity.Property(e => e.TipoProjetoAec)
                    .IsRequired()
                    .HasColumnName("Tipo Projeto AEC")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TipoRequisicao)
                    .IsRequired()
                    .HasColumnName("Tipo Requisicao")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserChefeProjecto)
                    .IsRequired()
                    .HasColumnName("User Chefe Projecto")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserResponsavel)
                    .IsRequired()
                    .HasColumnName("User Responsavel")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Utilizador)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ValidadePedido)
                    .HasColumnName("Validade Pedido")
                    .HasColumnType("datetime");

                entity.Property(e => e.ValorProjecto)
                    .HasColumnName("Valor Projecto")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.WipMethod).HasColumnName("WIP Method");

                entity.Property(e => e.WipPostedToGL).HasColumnName("WIP Posted To G_L");

                entity.Property(e => e.WipPostingDate)
                    .HasColumnName("WIP Posting Date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<JobJournalBatch>(entity =>
            {
                entity.HasKey(e => new { e.JournalTemplateName, e.Name })
                    .HasName("Job Journal Batch$0");

                entity.ToTable("Job Journal Batch");

                entity.Property(e => e.JournalTemplateName)
                    .HasColumnName("Journal Template Name")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FolhaHoras).HasColumnName("Folha Horas");

                entity.Property(e => e.Gtroupas).HasColumnName("GTRoupas");

                entity.Property(e => e.NoSeries)
                    .IsRequired()
                    .HasColumnName("No_ Series")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PostingNoSeries)
                    .IsRequired()
                    .HasColumnName("Posting No_ Series")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ReasonCode)
                    .IsRequired()
                    .HasColumnName("Reason Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RequisitionBatch).HasColumnName("Requisition Batch");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();
            });

            modelBuilder.Entity<JobJournalLine>(entity =>
            {
                entity.HasKey(e => new { e.NumMec, e.LineNo })
                    .HasName("Job Journal Line$0");

                entity.ToTable("Job Journal Line");

                entity.Property(e => e.NumMec)
                    .HasColumnName("Num_Mec")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LineNo).HasColumnName("Line No_");

                entity.Property(e => e.AcertoPrecos).HasColumnName("Acerto Precos");

                entity.Property(e => e.AppliesFromEntry).HasColumnName("Applies-from Entry");

                entity.Property(e => e.AppliesToEntry).HasColumnName("Applies-to Entry");

                entity.Property(e => e.Area)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.BAjmo).HasColumnName("B_Ajmo");

                entity.Property(e => e.BOrçamento).HasColumnName("B_orçamento");

                entity.Property(e => e.BinCode)
                    .IsRequired()
                    .HasColumnName("Bin Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CentralIncineração)
                    .IsRequired()
                    .HasColumnName("Central Incineração")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ChaveOrcamento).HasColumnName("Chave Orcamento");

                entity.Property(e => e.Classe)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CodServCliente)
                    .IsRequired()
                    .HasColumnName("Cod_Serv_Cliente")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoOrcamento)
                    .IsRequired()
                    .HasColumnName("Codigo Orcamento")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CostFactor)
                    .HasColumnName("Cost Factor")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.CountryRegionCode)
                    .IsRequired()
                    .HasColumnName("Country_Region Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedFromSync).HasColumnName("Created from Sync_");

                entity.Property(e => e.CurrencyCode)
                    .IsRequired()
                    .HasColumnName("Currency Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CurrencyFactor)
                    .HasColumnName("Currency Factor")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.CustomerPriceGroup)
                    .IsRequired()
                    .HasColumnName("Customer Price Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CódCategoriaProd)
                    .IsRequired()
                    .HasColumnName("Cód_Categoria Prod_")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CódGrupoProd)
                    .IsRequired()
                    .HasColumnName("Cód_Grupo Prod_")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DataConsumo)
                    .HasColumnName("Data Consumo")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataDespesa)
                    .HasColumnName("Data Despesa")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataDocumentoCorrigido)
                    .HasColumnName("Data Documento Corrigido")
                    .HasColumnType("datetime");

                entity.Property(e => e.DesServCliente)
                    .IsRequired()
                    .HasColumnName("Des_Serv_Cliente")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.DescontoVenda)
                    .HasColumnName("% Desconto Venda")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description2)
                    .IsRequired()
                    .HasColumnName("Description 2")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description22)
                    .IsRequired()
                    .HasColumnName("Description 2_2")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DescriçãoMarca)
                    .IsRequired()
                    .HasColumnName("Descrição Marca")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.DescriçãoModelo)
                    .IsRequired()
                    .HasColumnName("Descrição Modelo")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.DescriçãoObjectoRef)
                    .IsRequired()
                    .HasColumnName("Descrição Objecto Ref")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.DescriçãoTipo)
                    .IsRequired()
                    .HasColumnName("Descrição Tipo")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.DestinoFinalResiduos)
                    .IsRequired()
                    .HasColumnName("Destino Final Residuos")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DevoluçãoMovProjecto).HasColumnName("Devolução Mov_ Projecto");

                entity.Property(e => e.DiaDaSemana).HasColumnName("Dia da semana");

                entity.Property(e => e.DirectUnitCostLcy)
                    .HasColumnName("Direct Unit Cost (LCY)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.DocumentDate)
                    .HasColumnName("Document Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DocumentNo)
                    .IsRequired()
                    .HasColumnName("Document No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentoCorrigido)
                    .IsRequired()
                    .HasColumnName("Documento Corrigido")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentoOriginal)
                    .IsRequired()
                    .HasColumnName("Documento Original")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.EntryExitPoint)
                    .IsRequired()
                    .HasColumnName("Entry_Exit Point")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.EntryType).HasColumnName("Entry Type");

                entity.Property(e => e.ErrorMessage)
                    .IsRequired()
                    .HasColumnName("Error Message")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ErrorOnPosting).HasColumnName("Error on Posting");

                entity.Property(e => e.ExpirationDate)
                    .HasColumnName("Expiration Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ExternalDocumentNo)
                    .IsRequired()
                    .HasColumnName("External Document No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FacturaANºCliente)
                    .IsRequired()
                    .HasColumnName("Factura-a Nº Cliente")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FacturacaoAutorizada).HasColumnName("Facturacao Autorizada");

                entity.Property(e => e.FromPlanningLineNo).HasColumnName("From Planning Line No_");

                entity.Property(e => e.GenBusPostingGroup)
                    .IsRequired()
                    .HasColumnName("Gen_ Bus_ Posting Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.GenProdPostingGroup)
                    .IsRequired()
                    .HasColumnName("Gen_ Prod_ Posting Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.GrupoServiço)
                    .IsRequired()
                    .HasColumnName("Grupo Serviço")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.HoraDeRegisto)
                    .HasColumnName("Hora de Registo")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdCliente).HasColumnName("ID_Cliente");

                entity.Property(e => e.IdInstituicao).HasColumnName("ID_Instituicao");

                entity.Property(e => e.IdServico).HasColumnName("ID_Servico");

                entity.Property(e => e.ImportadoManage).HasColumnName("Importado Manage");

                entity.Property(e => e.JleOrigem).HasColumnName("JLE_Origem");

                entity.Property(e => e.JobNo)
                    .IsRequired()
                    .HasColumnName("Job No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.JobPostingOnly).HasColumnName("Job Posting Only");

                entity.Property(e => e.JobTaskNo)
                    .IsRequired()
                    .HasColumnName("Job Task No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.JournalDocument).HasColumnName("Journal  Document");

                entity.Property(e => e.LedgerEntryNo).HasColumnName("Ledger Entry No_");

                entity.Property(e => e.LedgerEntryType).HasColumnName("Ledger Entry Type");

                entity.Property(e => e.LineAmount)
                    .HasColumnName("Line Amount")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.LineAmountLcy)
                    .HasColumnName("Line Amount (LCY)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.LineDiscount)
                    .HasColumnName("Line Discount %")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.LineDiscountAmount)
                    .HasColumnName("Line Discount Amount")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.LineDiscountAmountLcy)
                    .HasColumnName("Line Discount Amount (LCY)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.LineType).HasColumnName("Line Type");

                entity.Property(e => e.LocalRecolha)
                    .IsRequired()
                    .HasColumnName("Local recolha")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LocationCode)
                    .IsRequired()
                    .HasColumnName("Location Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LotNo)
                    .IsRequired()
                    .HasColumnName("Lot No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Matricula)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Mc).HasColumnName("%MC");

                entity.Property(e => e.Motorista)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.No)
                    .IsRequired()
                    .HasColumnName("No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NomeSeccaoProjecto)
                    .IsRequired()
                    .HasColumnName("Nome Seccao Projecto")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NºCartãoUtente)
                    .IsRequired()
                    .HasColumnName("Nº Cartão Utente")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.NºFolhaDeHoras).HasColumnName("Nº Folha de Horas");

                entity.Property(e => e.NºFuncionario)
                    .IsRequired()
                    .HasColumnName("Nº Funcionario")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NºGuiaExterna)
                    .IsRequired()
                    .HasColumnName("Nº Guia Externa")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.NºGuiaRemessa)
                    .IsRequired()
                    .HasColumnName("Nº Guia Remessa")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NºGuiaResíduosGar)
                    .IsRequired()
                    .HasColumnName("Nº Guia Resíduos (GAR)")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.NºLinhaFolha).HasColumnName("Nº Linha Folha");

                entity.Property(e => e.NºLinhaOm).HasColumnName("Nº Linha OM");

                entity.Property(e => e.NºPreRegisto).HasColumnName("Nº Pre Registo");

                entity.Property(e => e.ObjectNo)
                    .IsRequired()
                    .HasColumnName("Object No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectRefNo)
                    .IsRequired()
                    .HasColumnName("Object Ref_ No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectRefType).HasColumnName("Object Ref_ Type");

                entity.Property(e => e.ObjectType).HasColumnName("Object Type");

                entity.Property(e => e.PesagemCliente)
                    .HasColumnName("Pesagem Cliente")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.PesoTotal)
                    .HasColumnName("Peso Total")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.PostedServiceShipmentNo)
                    .IsRequired()
                    .HasColumnName("Posted Service Shipment No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PostingDate)
                    .HasColumnName("Posting Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.PostingGroup)
                    .IsRequired()
                    .HasColumnName("Posting Group")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PostingNoSeries)
                    .IsRequired()
                    .HasColumnName("Posting No_ Series")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PurchaseDocument).HasColumnName("Purchase Document");

                entity.Property(e => e.QtyPerUnitOfMeasure)
                    .HasColumnName("Qty_ per Unit of Measure")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.Quantity).HasColumnType("decimal(38, 20)");

                entity.Property(e => e.QuantityBase)
                    .HasColumnName("Quantity (Base)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.ReasonCode)
                    .IsRequired()
                    .HasColumnName("Reason Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RecurringFrequency)
                    .IsRequired()
                    .HasColumnName("Recurring Frequency")
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.RecurringMethod).HasColumnName("Recurring Method");

                entity.Property(e => e.RequisitionLineNo).HasColumnName("Requisition Line No_");

                entity.Property(e => e.RequisitionNo)
                    .IsRequired()
                    .HasColumnName("Requisition No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RequisiçãoInterna)
                    .IsRequired()
                    .HasColumnName("Requisição Interna")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ResourceGroupNo)
                    .IsRequired()
                    .HasColumnName("Resource Group No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SalespersPurchCode)
                    .IsRequired()
                    .HasColumnName("Salespers__Purch_ Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SerialNo)
                    .IsRequired()
                    .HasColumnName("Serial No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceOrderNo)
                    .IsRequired()
                    .HasColumnName("Service Order No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShipmentMethodCode)
                    .IsRequired()
                    .HasColumnName("Shipment Method Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension1Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 1 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension2Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 2 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension3Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 3 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SkipMessage).HasColumnName("Skip Message");

                entity.Property(e => e.SourceCode)
                    .IsRequired()
                    .HasColumnName("Source Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SourceCurrencyCode)
                    .IsRequired()
                    .HasColumnName("Source Currency Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SourceCurrencyLineAmount)
                    .HasColumnName("Source Currency Line Amount")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.SourceCurrencyTotalCost)
                    .HasColumnName("Source Currency Total Cost")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.SourceCurrencyTotalPrice)
                    .HasColumnName("Source Currency Total Price")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.Subcontratação)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.TipoManutencao).HasColumnName("Tipo Manutencao");

                entity.Property(e => e.TipoMovRefeitorio).HasColumnName("Tipo Mov Refeitorio");

                entity.Property(e => e.TipoRecurso).HasColumnName("Tipo Recurso");

                entity.Property(e => e.TipoRefeição)
                    .IsRequired()
                    .HasColumnName("Tipo Refeição")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Tipologia)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TotalCost)
                    .HasColumnName("Total Cost")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TotalCostLcy)
                    .HasColumnName("Total Cost (LCY)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TotalPrice)
                    .HasColumnName("Total Price")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TotalPriceLcy)
                    .HasColumnName("Total Price (LCY)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TransactionSpecification)
                    .IsRequired()
                    .HasColumnName("Transaction Specification")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionType)
                    .IsRequired()
                    .HasColumnName("Transaction Type")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TransfParaProj)
                    .IsRequired()
                    .HasColumnName("Transf para Proj")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TransportMethod)
                    .IsRequired()
                    .HasColumnName("Transport Method")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UnitCost)
                    .HasColumnName("Unit Cost")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.UnitCostLcy)
                    .HasColumnName("Unit Cost (LCY)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.UnitOfMeasureCode)
                    .IsRequired()
                    .HasColumnName("Unit of Measure Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UnitPrice)
                    .HasColumnName("Unit Price")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.UnitPriceLcy)
                    .HasColumnName("Unit Price (LCY)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.Utilizador)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.VariantCode)
                    .IsRequired()
                    .HasColumnName("Variant Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Vat)
                    .HasColumnName("VAT %")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.WorkTypeCode)
                    .IsRequired()
                    .HasColumnName("Work Type Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<JobJournalTemplate>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("Job Journal Template$0");

                entity.ToTable("Job Journal Template");

                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.FolhaHoras).HasColumnName("Folha Horas");

                entity.Property(e => e.ForcePostingReport).HasColumnName("Force Posting Report");

                entity.Property(e => e.FormId).HasColumnName("Form ID");

                entity.Property(e => e.FormIdAlimentação).HasColumnName("Form ID - Alimentação");

                entity.Property(e => e.FormIdProjectosEObras).HasColumnName("Form ID - Projectos e Obras");

                entity.Property(e => e.FormIdRejeicoes).HasColumnName("Form ID - Rejeicoes");

                entity.Property(e => e.FormIdRequisition).HasColumnName("Form ID - Requisition");

                entity.Property(e => e.FormIdTratamento).HasColumnName("Form ID - Tratamento");

                entity.Property(e => e.FormIdTratamentoAmbiente).HasColumnName("Form ID - Tratamento Ambiente");

                entity.Property(e => e.FormIdTratamentoRoupa).HasColumnName("Form ID - Tratamento Roupa");

                entity.Property(e => e.NoSeries)
                    .IsRequired()
                    .HasColumnName("No_ Series")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PostingNoSeries)
                    .IsRequired()
                    .HasColumnName("Posting No_ Series")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PostingReportId).HasColumnName("Posting Report ID");

                entity.Property(e => e.ReasonCode)
                    .IsRequired()
                    .HasColumnName("Reason Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SourceCode)
                    .IsRequired()
                    .HasColumnName("Source Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TestReportId).HasColumnName("Test Report ID");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();
            });

            modelBuilder.Entity<JobLedgerEntry>(entity =>
            {
                entity.HasKey(e => e.EntryNo)
                    .HasName("Job Ledger Entry$0");

                entity.ToTable("Job Ledger Entry");

                entity.HasIndex(e => e.Chargeable)
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K167_f97")
                    .HasFilter("([Job Ledger Entry].[Facturacao Autorizada]=(1))");

                entity.HasIndex(e => e.DataAutorizacaoFacturacao)
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K125");

                entity.HasIndex(e => e.DocumentNo)
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K5_f50")
                    .HasFilter("([Job Ledger Entry].[Facturacao Autorizada]=(1) AND [Job Ledger Entry].[Chargeable]=(1))");

                entity.HasIndex(e => e.EntryNo)
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K2_f68")
                    .HasFilter("([Job Ledger Entry].[Source Code]='VENDAS' AND [Job Ledger Entry].[Document No_]>='2' AND [Job Ledger Entry].[Document No_]<'3' AND [Job Ledger Entry].[Document No_]>='3' AND [Job Ledger Entry].[Document No_]<'4' AND [Job Ledger Entry].[Document No_]>='4' AND [Job Ledger Entry].[Document No_]<'5')");

                entity.HasIndex(e => e.FacturacaoAutorizada)
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K124_f83")
                    .HasFilter("([Job Ledger Entry].[Chargeable]=(1))");

                entity.HasIndex(e => e.JobNo)
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_f21")
                    .HasFilter("([Job Ledger Entry].[Source Code]='VENDAS')");

                entity.HasIndex(e => e.No)
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K7_f86")
                    .HasFilter("([Job Ledger Entry].[Source Code]='VENDAS')");

                entity.HasIndex(e => e.SourceCode)
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K24_f56")
                    .HasFilter("([Job Ledger Entry].[Facturacao Autorizada]=(1) AND [Job Ledger Entry].[Chargeable]=(1))");

                entity.HasIndex(e => e.WsCommunicatedNav2009)
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K176");

                entity.HasIndex(e => new { e.DataAutorizacaoFacturacao, e.EntryNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K2_125");

                entity.HasIndex(e => new { e.DocumentNo, e.JobNo })
                    .HasName("_dta_stat_139915620_5_3");

                entity.HasIndex(e => new { e.DocumentNo, e.SourceCode })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K5_K24_f124")
                    .HasFilter("([Job Ledger Entry].[Source Code]='VENDAS')");

                entity.HasIndex(e => new { e.EntryNo, e.No })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K7_2");

                entity.HasIndex(e => new { e.FacturacaoAutorizada, e.Chargeable })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K124_K167_f95")
                    .HasFilter("([Job Ledger Entry].[Source Code]='VENDAS')");

                entity.HasIndex(e => new { e.JobNo, e.FacturacaoAutorizada })
                    .HasName("_dta_stat_139915620_3_124");

                entity.HasIndex(e => new { e.No, e.DocumentNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K7_K5");

                entity.HasIndex(e => new { e.No, e.EntryNo })
                    .HasName("_dta_stat_139915620_7_2")
                    .HasFilter("([Job Ledger Entry].[Facturacao Autorizada]=(1) AND [Job Ledger Entry].[Chargeable]=(1))");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DocumentNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K5_14");

                entity.HasIndex(e => new { e.UnitPriceLcy, e.No });

                entity.HasIndex(e => new { e.DataAutorizacaoFacturacao, e.FacturacaoAutorizada, e.Chargeable })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K124_K167_125");

                entity.HasIndex(e => new { e.DocumentNo, e.Description, e.No })
                    .HasName("IX_Job Ledger Entry_No_");

                entity.HasIndex(e => new { e.DocumentNo, e.SourceCode, e.EntryNo })
                    .HasName("_dta_stat_139915620_5_24_2")
                    .HasFilter("([Job Ledger Entry].[Source Code]='VENDAS')");

                entity.HasIndex(e => new { e.DocumentNo, e.SourceCode, e.JobNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K5_K24_K3_f25")
                    .HasFilter("([Job Ledger Entry].[Source Code]='VENDAS')");

                entity.HasIndex(e => new { e.DocumentNo, e.TotalPriceLcy, e.DataAutorizacaoFacturacao })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K125_5_14");

                entity.HasIndex(e => new { e.DocumentNo, e.TotalPriceLcy, e.SourceCode })
                    .HasName("_dta_stat_139915620_5_14_24");

                entity.HasIndex(e => new { e.FacturacaoAutorizada, e.Chargeable, e.EntryNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K124_K167_K2_f46")
                    .HasFilter("([Job Ledger Entry].[Chargeable]=(1))");

                entity.HasIndex(e => new { e.FacturacaoAutorizada, e.Chargeable, e.JobNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K124_K167_K3_f96")
                    .HasFilter("([Job Ledger Entry].[Chargeable]=(1))");

                entity.HasIndex(e => new { e.JobNo, e.DocumentNo, e.SourceCode })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K5_K24_f120")
                    .HasFilter("([Job Ledger Entry].[Source Code]='VENDAS' AND [Job Ledger Entry].[Document No_]>='2' AND [Job Ledger Entry].[Document No_]<'3' AND [Job Ledger Entry].[Document No_]>='3' AND [Job Ledger Entry].[Document No_]<'4' AND [Job Ledger Entry].[Document No_]>='4' AND [Job Ledger Entry].[Document No_]<'5')");

                entity.HasIndex(e => new { e.JobNo, e.EntryNo, e.FacturacaoAutorizada })
                    .HasName("_dta_stat_139915620_3_2_124");

                entity.HasIndex(e => new { e.JobNo, e.EntryNo, e.SourceCode })
                    .HasName("_dta_stat_139915620_3_2_24");

                entity.HasIndex(e => new { e.JobNo, e.FacturacaoAutorizada, e.Chargeable })
                    .HasName("IX_Job Ledger Entry_Facturacao Autorizada_Chargeable")
                    .HasFilter("([Job Ledger Entry].[Facturacao Autorizada]=(1) AND [Job Ledger Entry].[Chargeable]=(1))");

                entity.HasIndex(e => new { e.JobNo, e.No, e.DocumentNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K7_K5");

                entity.HasIndex(e => new { e.No, e.DocumentNo, e.JobNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K7_K5_K3");

                entity.HasIndex(e => new { e.No, e.FacturacaoAutorizada, e.Chargeable })
                    .HasName("_dta_stat_139915620_7_124_167");

                entity.HasIndex(e => new { e.No, e.SourceCode, e.DocumentNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K7_K24_K5_f90")
                    .HasFilter("([Job Ledger Entry].[Source Code]='VENDAS')");

                entity.HasIndex(e => new { e.SourceCode, e.JobNo, e.Chargeable })
                    .HasName("_dta_stat_139915620_24_3_167");

                entity.HasIndex(e => new { e.SourceCode, e.JobNo, e.No })
                    .HasName("_dta_stat_139915620_24_3_7");

                entity.HasIndex(e => new { e.Timestamp, e.EntryNo, e.GlobalDimension3Code })
                    .HasName("IX_Job Ledger Entry_Global Dimension 3 Code");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.DocumentNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K5_14_125");

                entity.HasIndex(e => new { e.Chargeable, e.No, e.JobNo, e.FacturacaoAutorizada })
                    .HasName("_dta_stat_139915620_167_7_3_124");

                entity.HasIndex(e => new { e.DataAutorizacaoFacturacao, e.Chargeable, e.FacturacaoAutorizada, e.JobNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K167_K124_K3_125");

                entity.HasIndex(e => new { e.DataAutorizacaoFacturacao, e.JobNo, e.Chargeable, e.FacturacaoAutorizada })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K167_K124_125");

                entity.HasIndex(e => new { e.DocumentNo, e.JobNo, e.FacturacaoAutorizada, e.Chargeable })
                    .HasName("_dta_stat_139915620_5_3_124_167");

                entity.HasIndex(e => new { e.DocumentNo, e.JobNo, e.SourceCode, e.TotalPriceLcy })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K5_K3_K24_K14");

                entity.HasIndex(e => new { e.DocumentNo, e.SourceCode, e.EntryNo, e.JobNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K5_K24_K2_K3_f67")
                    .HasFilter("([Job Ledger Entry].[Source Code]='VENDAS')");

                entity.HasIndex(e => new { e.DocumentNo, e.SourceCode, e.JobNo, e.TotalPriceLcy })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K5_K24_K3_K14");

                entity.HasIndex(e => new { e.DocumentNo, e.TotalPriceLcy, e.No, e.SourceCode })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K7_K24_5_14");

                entity.HasIndex(e => new { e.DocumentNo, e.TotalPriceLcy, e.SourceCode, e.JobNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K5_K14_K24_K3");

                entity.HasIndex(e => new { e.EntryNo, e.JobNo, e.FacturacaoAutorizada, e.Chargeable })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K124_K167_2_3");

                entity.HasIndex(e => new { e.EntryNo, e.JobNo, e.No, e.SourceCode })
                    .HasName("_dta_stat_139915620_2_3_7_24");

                entity.HasIndex(e => new { e.EntryNo, e.No, e.JobNo, e.FacturacaoAutorizada })
                    .HasName("_dta_stat_139915620_2_7_3_124");

                entity.HasIndex(e => new { e.FacturacaoAutorizada, e.Chargeable, e.EntryNo, e.JobNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K124_K167_K2_K3_f65")
                    .HasFilter("([Job Ledger Entry].[Chargeable]=(1))");

                entity.HasIndex(e => new { e.FacturacaoAutorizada, e.Chargeable, e.JobNo, e.EntryNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K124_K167_K3_K2");

                entity.HasIndex(e => new { e.JobNo, e.DocumentNo, e.SourceCode, e.EntryNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K5_K24_K2_f72")
                    .HasFilter("([Job Ledger Entry].[Source Code]='VENDAS' AND [Job Ledger Entry].[Document No_]>='2' AND [Job Ledger Entry].[Document No_]<'3' AND [Job Ledger Entry].[Document No_]>='3' AND [Job Ledger Entry].[Document No_]<'4' AND [Job Ledger Entry].[Document No_]>='4' AND [Job Ledger Entry].[Document No_]<'5')");

                entity.HasIndex(e => new { e.JobNo, e.EntryNo, e.FacturacaoAutorizada, e.Chargeable })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K2_K124_K167");

                entity.HasIndex(e => new { e.JobNo, e.FacturacaoAutorizada, e.Chargeable, e.EntryNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K124_K167_K2_f55")
                    .HasFilter("([Job Ledger Entry].[Facturacao Autorizada]=(1) AND [Job Ledger Entry].[Chargeable]=(1))");

                entity.HasIndex(e => new { e.JobNo, e.No, e.SourceCode, e.DocumentNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K7_K24_K5_f89")
                    .HasFilter("([Job Ledger Entry].[Source Code]='VENDAS')");

                entity.HasIndex(e => new { e.JobNo, e.TotalPriceLcy, e.DocumentNo, e.SourceCode })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K5_K24_3_14");

                entity.HasIndex(e => new { e.JobNo, e.TotalPriceLcy, e.SourceCode, e.DocumentNo })
                    .HasName("IX_Job Ledger Entry_Source Code_Document No_");

                entity.HasIndex(e => new { e.No, e.DocumentNo, e.JobNo, e.FacturacaoAutorizada })
                    .HasName("_dta_stat_139915620_7_5_3_124");

                entity.HasIndex(e => new { e.No, e.FacturacaoAutorizada, e.SourceCode, e.Chargeable })
                    .HasName("_dta_stat_139915620_7_124_24_167");

                entity.HasIndex(e => new { e.No, e.SourceCode, e.DocumentNo, e.JobNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K7_K24_K5_K3_f77")
                    .HasFilter("([Job Ledger Entry].[Source Code]='VENDAS')");

                entity.HasIndex(e => new { e.Timestamp, e.EntryNo, e.DataAutorizacaoFacturacao, e.DocumentNo })
                    .HasName("IX_Job Ledger Entry_Document No_");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.JobNo, e.SourceCode, e.DocumentNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K24_K5_14");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.SourceCode, e.DocumentNo, e.JobNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K24_K5_K3_14");

                entity.HasIndex(e => new { e.DataAutorizacaoFacturacao, e.DocumentNo, e.JobNo, e.SourceCode, e.TotalPriceLcy })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K5_K3_K24_K14_125");

                entity.HasIndex(e => new { e.DataAutorizacaoFacturacao, e.DocumentNo, e.SourceCode, e.JobNo, e.TotalPriceLcy })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K5_K24_K3_K14_125_f2")
                    .HasFilter("([Job Ledger Entry].[Source Code]='VENDAS')");

                entity.HasIndex(e => new { e.DataAutorizacaoFacturacao, e.DocumentNo, e.TotalPriceLcy, e.SourceCode, e.JobNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K5_K14_K24_K3_125");

                entity.HasIndex(e => new { e.DataAutorizacaoFacturacao, e.FacturacaoAutorizada, e.Chargeable, e.JobNo, e.EntryNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K124_K167_K3_K2_125");

                entity.HasIndex(e => new { e.DataAutorizacaoFacturacao, e.JobNo, e.DocumentNo, e.SourceCode, e.TotalPriceLcy })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K5_K24_K14_125_f1")
                    .HasFilter("([Job Ledger Entry].[Source Code]='VENDAS')");

                entity.HasIndex(e => new { e.DataAutorizacaoFacturacao, e.JobNo, e.EntryNo, e.FacturacaoAutorizada, e.Chargeable })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K2_K124_K167_125");

                entity.HasIndex(e => new { e.DataAutorizacaoFacturacao, e.SourceCode, e.DocumentNo, e.JobNo, e.TotalPriceLcy })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K24_K5_K3_K14_125");

                entity.HasIndex(e => new { e.EntryNo, e.JobNo, e.TotalPriceLcy, e.DocumentNo, e.SourceCode })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K5_K24_2_3_14");

                entity.HasIndex(e => new { e.EntryNo, e.No, e.JobNo, e.FacturacaoAutorizada, e.Chargeable })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K2_K7_K3_K124_K167");

                entity.HasIndex(e => new { e.FacturacaoAutorizada, e.Chargeable, e.DocumentNo, e.No, e.JobNo })
                    .HasName("_dta_stat_139915620_124_167_5_7_3");

                entity.HasIndex(e => new { e.FacturacaoAutorizada, e.Chargeable, e.JobNo, e.SourceCode, e.No })
                    .HasName("_dta_stat_139915620_124_167_3_24_7");

                entity.HasIndex(e => new { e.JobNo, e.DocumentNo, e.TotalPriceLcy, e.SourceCode, e.No })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K7_3_5_14_24");

                entity.HasIndex(e => new { e.JobNo, e.No, e.SourceCode, e.DocumentNo, e.Chargeable })
                    .HasName("_dta_stat_139915620_3_7_24_5_167");

                entity.HasIndex(e => new { e.No, e.FacturacaoAutorizada, e.DocumentNo, e.SourceCode, e.Chargeable })
                    .HasName("_dta_stat_139915620_7_124_5_24_167");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.JobNo, e.EntryNo, e.SourceCode, e.DocumentNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K2_K24_K5_14");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.JobNo, e.SourceCode, e.No, e.DocumentNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K24_K7_K5_14");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.SourceCode, e.DocumentNo, e.JobNo, e.EntryNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K24_K5_K3_K2_14");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.SourceCode, e.DocumentNo, e.No, e.JobNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K5_K7_K3_14_24");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.SourceCode, e.JobNo, e.DocumentNo, e.No })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K5_K7_14_24");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.SourceCode, e.No, e.DocumentNo, e.JobNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K24_K7_K5_K3_14");

                entity.HasIndex(e => new { e.Chargeable, e.JobNo, e.No, e.EntryNo, e.SourceCode, e.DocumentNo })
                    .HasName("_dta_stat_139915620_167_3_7_2_24_5");

                entity.HasIndex(e => new { e.DataAutorizacaoFacturacao, e.EntryNo, e.No, e.JobNo, e.FacturacaoAutorizada, e.Chargeable })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K2_K7_K3_K124_K167_125");

                entity.HasIndex(e => new { e.EntryNo, e.JobNo, e.Description, e.Quantity, e.Description2, e.TipoRecurso })
                    .HasName("IX_Job Ledger Entry_Tipo Recurso");

                entity.HasIndex(e => new { e.EntryNo, e.JobNo, e.FacturacaoAutorizada, e.DataAutorizacaoFacturacao, e.Chargeable, e.DocumentNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K5_2_3_124_125_167");

                entity.HasIndex(e => new { e.FacturacaoAutorizada, e.Chargeable, e.EntryNo, e.JobNo, e.No, e.SourceCode })
                    .HasName("_dta_stat_139915620_124_167_2_3_7_24");

                entity.HasIndex(e => new { e.JobNo, e.EntryNo, e.FacturacaoAutorizada, e.Chargeable, e.DocumentNo, e.No })
                    .HasName("_dta_stat_139915620_3_2_124_167_5_7");

                entity.HasIndex(e => new { e.JobNo, e.EntryNo, e.FacturacaoAutorizada, e.Chargeable, e.SourceCode, e.DocumentNo })
                    .HasName("_dta_stat_139915620_3_2_124_167_24_5");

                entity.HasIndex(e => new { e.No, e.SourceCode, e.DocumentNo, e.FacturacaoAutorizada, e.Chargeable, e.JobNo })
                    .HasName("_dta_stat_139915620_7_24_5_124_167_3");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.JobNo, e.EntryNo, e.SourceCode, e.DocumentNo, e.No })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K2_K24_K5_K7_14");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.SourceCode, e.DocumentNo, e.JobNo, e.EntryNo, e.No })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K24_K5_K3_K2_K7_14");

                entity.HasIndex(e => new { e.DocumentNo, e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.No, e.FacturacaoAutorizada, e.SourceCode, e.Chargeable })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K7_K124_K24_K167_5_14_125");

                entity.HasIndex(e => new { e.SourceCode, e.DocumentNo, e.JobNo, e.EntryNo, e.No, e.FacturacaoAutorizada, e.Chargeable })
                    .HasName("_dta_stat_139915620_24_5_3_2_7_124_167");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.JobNo, e.SourceCode, e.DocumentNo, e.FacturacaoAutorizada, e.Chargeable, e.EntryNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K24_K5_K124_K167_K2_14");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.SourceCode, e.DocumentNo, e.JobNo, e.FacturacaoAutorizada, e.Chargeable, e.EntryNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K24_K5_K3_K124_K167_K2_14");

                entity.HasIndex(e => new { e.DocumentNo, e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.No, e.FacturacaoAutorizada, e.SourceCode, e.Chargeable, e.JobNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K7_K124_K24_K167_K3_5_14_125");

                entity.HasIndex(e => new { e.EntryNo, e.JobNo, e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.FacturacaoAutorizada, e.DocumentNo, e.SourceCode, e.Chargeable })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K124_K5_K24_K167_2_3_14_125");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.Chargeable, e.FacturacaoAutorizada, e.JobNo, e.SourceCode, e.No, e.DocumentNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K167_K124_K3_K24_K7_K5_14_125");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.Chargeable, e.FacturacaoAutorizada, e.SourceCode, e.No, e.DocumentNo, e.JobNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K167_K124_K24_K7_K5_K3_14_125");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.Chargeable, e.JobNo, e.FacturacaoAutorizada, e.SourceCode, e.No, e.DocumentNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K167_K3_K124_K24_K7_K5_14_125_f94")
                    .HasFilter("([Job Ledger Entry].[Facturacao Autorizada]=(1))");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.Chargeable, e.JobNo, e.SourceCode, e.DocumentNo, e.FacturacaoAutorizada, e.EntryNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K167_K3_K24_K5_K124_K2_14_125_f117")
                    .HasFilter("([Job Ledger Entry].[Facturacao Autorizada]=(1))");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.DocumentNo, e.JobNo, e.Chargeable, e.FacturacaoAutorizada, e.SourceCode, e.No })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K5_K3_K167_K124_K24_K7_14_125_f76")
                    .HasFilter("([Job Ledger Entry].[Source Code]='VENDAS')");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.DocumentNo, e.SourceCode, e.JobNo, e.FacturacaoAutorizada, e.Chargeable, e.EntryNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K5_K24_K3_K124_K167_K2_14_125_f122")
                    .HasFilter("([Job Ledger Entry].[Source Code]='VENDAS')");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.EntryNo, e.JobNo, e.SourceCode, e.DocumentNo, e.FacturacaoAutorizada, e.Chargeable })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K2_K3_K24_K5_K124_K167_14_125_f121")
                    .HasFilter("([Job Ledger Entry].[Source Code]='VENDAS' AND [Job Ledger Entry].[Document No_]>='2' AND [Job Ledger Entry].[Document No_]<'3' AND [Job Ledger Entry].[Document No_]>='3' AND [Job Ledger Entry].[Document No_]<'4' AND [Job Ledger Entry].[Document No_]>='4' AND [Job Ledger Entry].[Document No_]<'5')");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.FacturacaoAutorizada, e.Chargeable, e.EntryNo, e.JobNo, e.SourceCode, e.DocumentNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K124_K167_K2_K3_K24_K5_14_125_f113")
                    .HasFilter("([Job Ledger Entry].[Chargeable]=(1))");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.FacturacaoAutorizada, e.Chargeable, e.JobNo, e.EntryNo, e.SourceCode, e.DocumentNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K124_K167_K3_K2_K24_K5_14_125");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.FacturacaoAutorizada, e.Chargeable, e.JobNo, e.SourceCode, e.No, e.DocumentNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K124_K167_K3_K24_K7_K5_14_125_f87")
                    .HasFilter("([Job Ledger Entry].[Chargeable]=(1))");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.JobNo, e.Chargeable, e.FacturacaoAutorizada, e.SourceCode, e.No, e.DocumentNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K167_K124_K24_K7_K5_14_125");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.JobNo, e.DocumentNo, e.SourceCode, e.FacturacaoAutorizada, e.Chargeable, e.EntryNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K5_K24_K124_K167_K2_14_125_f123")
                    .HasFilter("([Job Ledger Entry].[Source Code]='VENDAS' AND [Job Ledger Entry].[Document No_]>='2' AND [Job Ledger Entry].[Document No_]<'3' AND [Job Ledger Entry].[Document No_]>='3' AND [Job Ledger Entry].[Document No_]<'4' AND [Job Ledger Entry].[Document No_]>='4' AND [Job Ledger Entry].[Document No_]<'5')");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.JobNo, e.EntryNo, e.FacturacaoAutorizada, e.Chargeable, e.SourceCode, e.DocumentNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K2_K124_K167_K24_K5_14_125");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.JobNo, e.EntryNo, e.SourceCode, e.DocumentNo, e.FacturacaoAutorizada, e.Chargeable })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K2_K24_K5_K124_K167_14_125");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.JobNo, e.FacturacaoAutorizada, e.Chargeable, e.EntryNo, e.SourceCode, e.DocumentNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K124_K167_K2_K24_K5_14_125_f114")
                    .HasFilter("([Job Ledger Entry].[Facturacao Autorizada]=(1) AND [Job Ledger Entry].[Chargeable]=(1))");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.JobNo, e.FacturacaoAutorizada, e.Chargeable, e.SourceCode, e.No, e.DocumentNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K124_K167_K24_K7_K5_14_125_f91")
                    .HasFilter("([Job Ledger Entry].[Facturacao Autorizada]=(1) AND [Job Ledger Entry].[Chargeable]=(1))");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.JobNo, e.No, e.SourceCode, e.DocumentNo, e.Chargeable, e.FacturacaoAutorizada })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K7_K24_K5_K167_K124_14_125_f93")
                    .HasFilter("([Job Ledger Entry].[Source Code]='VENDAS')");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.JobNo, e.SourceCode, e.DocumentNo, e.FacturacaoAutorizada, e.Chargeable, e.EntryNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K24_K5_K124_K167_K2_14_125");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.No, e.SourceCode, e.DocumentNo, e.JobNo, e.Chargeable, e.FacturacaoAutorizada })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K7_K24_K5_K3_K167_K124_14_125_f88")
                    .HasFilter("([Job Ledger Entry].[Source Code]='VENDAS')");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.SourceCode, e.DocumentNo, e.FacturacaoAutorizada, e.Chargeable, e.JobNo, e.EntryNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K24_K5_K124_K167_K3_K2_14_125");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.SourceCode, e.DocumentNo, e.JobNo, e.FacturacaoAutorizada, e.Chargeable, e.EntryNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K24_K5_K3_K124_K167_K2_14_125");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.SourceCode, e.JobNo, e.Chargeable, e.FacturacaoAutorizada, e.No, e.DocumentNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K24_K3_K167_K124_K7_K5_14_125");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.SourceCode, e.JobNo, e.DocumentNo, e.FacturacaoAutorizada, e.Chargeable, e.EntryNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K24_K3_K5_K124_K167_K2_14_125_f119")
                    .HasFilter("([Job Ledger Entry].[Facturacao Autorizada]=(1) AND [Job Ledger Entry].[Chargeable]=(1))");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.JobNo, e.EntryNo, e.SourceCode, e.DocumentNo, e.No, e.FacturacaoAutorizada, e.Chargeable })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K2_K24_K5_K7_K124_K167_14");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.SourceCode, e.DocumentNo, e.JobNo, e.EntryNo, e.No, e.FacturacaoAutorizada, e.Chargeable })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K24_K5_K3_K2_K7_K124_K167_14");

                entity.HasIndex(e => new { e.EntryNo, e.JobNo, e.DocumentNo, e.TotalPriceLcy, e.SourceCode, e.DataAutorizacaoFacturacao, e.No, e.FacturacaoAutorizada, e.Chargeable })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K7_K124_K167_2_3_5_14_24_125");

                entity.HasIndex(e => new { e.EntryNo, e.JobNo, e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.No, e.FacturacaoAutorizada, e.DocumentNo, e.SourceCode, e.Chargeable })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K7_K124_K5_K24_K167_2_3_14_125");

                entity.HasIndex(e => new { e.EntryNo, e.No, e.FacturacaoAutorizada, e.DataAutorizacaoFacturacao, e.Chargeable, e.JobNo, e.DocumentNo, e.SourceCode, e.TotalPriceLcy })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K5_K24_K14_2_7_124_125_167");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.Chargeable, e.JobNo, e.No, e.EntryNo, e.SourceCode, e.DocumentNo, e.FacturacaoAutorizada })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K167_K3_K7_K2_K24_K5_K124_14_125_f48")
                    .HasFilter("([Job Ledger Entry].[Facturacao Autorizada]=(1))");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.Chargeable, e.No, e.JobNo, e.FacturacaoAutorizada, e.EntryNo, e.SourceCode, e.DocumentNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K167_K7_K3_K124_K2_K24_K5_14_125_f105")
                    .HasFilter("([Job Ledger Entry].[Facturacao Autorizada]=(1))");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.DocumentNo, e.SourceCode, e.EntryNo, e.JobNo, e.No, e.FacturacaoAutorizada, e.Chargeable })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K5_K24_K2_K3_K7_K124_K167_14_125_f53")
                    .HasFilter("([Job Ledger Entry].[Source Code]='VENDAS')");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.EntryNo, e.JobNo, e.No, e.SourceCode, e.DocumentNo, e.FacturacaoAutorizada, e.Chargeable })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K2_K3_K7_K24_K5_K124_K167_14_125_f71")
                    .HasFilter("([Job Ledger Entry].[Source Code]='VENDAS' AND [Job Ledger Entry].[Document No_]>='2' AND [Job Ledger Entry].[Document No_]<'3' AND [Job Ledger Entry].[Document No_]>='3' AND [Job Ledger Entry].[Document No_]<'4' AND [Job Ledger Entry].[Document No_]>='4' AND [Job Ledger Entry].[Document No_]<'5')");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.EntryNo, e.No, e.JobNo, e.FacturacaoAutorizada, e.Chargeable, e.SourceCode, e.DocumentNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K2_K7_K3_K124_K167_K24_K5_14_125_f98")
                    .HasFilter("([Job Ledger Entry].[Source Code]='VENDAS' AND [Job Ledger Entry].[Document No_]>='2' AND [Job Ledger Entry].[Document No_]<'3' AND [Job Ledger Entry].[Document No_]>='3' AND [Job Ledger Entry].[Document No_]<'4' AND [Job Ledger Entry].[Document No_]>='4' AND [Job Ledger Entry].[Document No_]<'5')");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.FacturacaoAutorizada, e.Chargeable, e.EntryNo, e.JobNo, e.No, e.SourceCode, e.DocumentNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K124_K167_K2_K3_K7_K24_K5_14_125_f70")
                    .HasFilter("([Job Ledger Entry].[Chargeable]=(1))");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.FacturacaoAutorizada, e.Chargeable, e.JobNo, e.EntryNo, e.SourceCode, e.DocumentNo, e.No })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K124_K167_K3_K2_K24_K5_K7_14_125");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.JobNo, e.DocumentNo, e.SourceCode, e.EntryNo, e.No, e.FacturacaoAutorizada, e.Chargeable })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K5_K24_K2_K7_K124_K167_14_125_f62")
                    .HasFilter("([Job Ledger Entry].[Source Code]='VENDAS' AND [Job Ledger Entry].[Document No_]>='2' AND [Job Ledger Entry].[Document No_]<'3' AND [Job Ledger Entry].[Document No_]>='3' AND [Job Ledger Entry].[Document No_]<'4' AND [Job Ledger Entry].[Document No_]>='4' AND [Job Ledger Entry].[Document No_]<'5')");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.JobNo, e.EntryNo, e.FacturacaoAutorizada, e.Chargeable, e.SourceCode, e.DocumentNo, e.No })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K2_K124_K167_K24_K5_K7_14_125");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.JobNo, e.EntryNo, e.No, e.FacturacaoAutorizada, e.Chargeable, e.SourceCode, e.DocumentNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K2_K7_K124_K167_K24_K5_14_125");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.JobNo, e.EntryNo, e.No, e.SourceCode, e.DocumentNo, e.FacturacaoAutorizada, e.Chargeable })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K2_K7_K24_K5_K124_K167_14_125");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.JobNo, e.EntryNo, e.SourceCode, e.DocumentNo, e.No, e.FacturacaoAutorizada, e.Chargeable })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K2_K24_K5_K7_K124_K167_14_125");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.JobNo, e.FacturacaoAutorizada, e.Chargeable, e.EntryNo, e.No, e.SourceCode, e.DocumentNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K124_K167_K2_K7_K24_K5_14_125_f99")
                    .HasFilter("([Job Ledger Entry].[Source Code]='VENDAS' AND [Job Ledger Entry].[Document No_]>='2' AND [Job Ledger Entry].[Document No_]<'3' AND [Job Ledger Entry].[Document No_]>='3' AND [Job Ledger Entry].[Document No_]<'4' AND [Job Ledger Entry].[Document No_]>='4' AND [Job Ledger Entry].[Document No_]<'5')");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.No, e.EntryNo, e.JobNo, e.FacturacaoAutorizada, e.Chargeable, e.SourceCode, e.DocumentNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K7_K2_K3_K124_K167_K24_K5_14_125");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.No, e.EntryNo, e.JobNo, e.SourceCode, e.DocumentNo, e.FacturacaoAutorizada, e.Chargeable })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K7_K2_K3_K24_K5_K124_K167_14_125_f44")
                    .HasFilter("([Job Ledger Entry].[Facturacao Autorizada]=(1) AND [Job Ledger Entry].[Chargeable]=(1))");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.No, e.FacturacaoAutorizada, e.Chargeable, e.SourceCode, e.DocumentNo, e.JobNo, e.EntryNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K7_K124_K167_K24_K5_K3_K2_14_125");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.No, e.SourceCode, e.DocumentNo, e.FacturacaoAutorizada, e.Chargeable, e.JobNo, e.EntryNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K7_K24_K5_K124_K167_K3_K2_14_125");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.SourceCode, e.DocumentNo, e.JobNo, e.EntryNo, e.No, e.FacturacaoAutorizada, e.Chargeable })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K24_K5_K3_K2_K7_K124_K167_14_125");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.SourceCode, e.JobNo, e.No, e.EntryNo, e.DocumentNo, e.FacturacaoAutorizada, e.Chargeable })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K24_K3_K7_K2_K5_K124_K167_14_125_f58")
                    .HasFilter("([Job Ledger Entry].[Facturacao Autorizada]=(1) AND [Job Ledger Entry].[Chargeable]=(1))");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.DataAutorizacaoFacturacao, e.SourceCode, e.No, e.JobNo, e.FacturacaoAutorizada, e.Chargeable, e.EntryNo, e.DocumentNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K24_K7_K3_K124_K167_K2_K5_14_125_f101")
                    .HasFilter("([Job Ledger Entry].[Document No_]>='2' AND [Job Ledger Entry].[Document No_]<'3' AND [Job Ledger Entry].[Document No_]>='3' AND [Job Ledger Entry].[Document No_]<'4' AND [Job Ledger Entry].[Document No_]>='4' AND [Job Ledger Entry].[Document No_]<'5')");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.SourceCode, e.DataAutorizacaoFacturacao, e.Chargeable, e.JobNo, e.FacturacaoAutorizada, e.EntryNo, e.DocumentNo, e.No })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K167_K3_K124_K2_K5_K7_14_24_125_f125")
                    .HasFilter("([Job Ledger Entry].[Facturacao Autorizada]=(1))");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.SourceCode, e.DataAutorizacaoFacturacao, e.DocumentNo, e.JobNo, e.FacturacaoAutorizada, e.Chargeable, e.EntryNo, e.No })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K5_K3_K124_K167_K2_K7_14_24_125");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.SourceCode, e.DataAutorizacaoFacturacao, e.EntryNo, e.JobNo, e.FacturacaoAutorizada, e.Chargeable, e.DocumentNo, e.No })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K2_K3_K124_K167_K5_K7_14_24_125_f128")
                    .HasFilter("([Job Ledger Entry].[Source Code]='VENDAS' AND [Job Ledger Entry].[Document No_]>='2' AND [Job Ledger Entry].[Document No_]<'3' AND [Job Ledger Entry].[Document No_]>='3' AND [Job Ledger Entry].[Document No_]<'4' AND [Job Ledger Entry].[Document No_]>='4' AND [Job Ledger Entry].[Document No_]<'5')");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.SourceCode, e.DataAutorizacaoFacturacao, e.FacturacaoAutorizada, e.Chargeable, e.DocumentNo, e.No, e.JobNo, e.EntryNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K124_K167_K5_K7_K3_K2_14_24_125");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.SourceCode, e.DataAutorizacaoFacturacao, e.FacturacaoAutorizada, e.Chargeable, e.EntryNo, e.JobNo, e.DocumentNo, e.No })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K124_K167_K2_K3_K5_K7_14_24_125_f127")
                    .HasFilter("([Job Ledger Entry].[Chargeable]=(1))");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.SourceCode, e.DataAutorizacaoFacturacao, e.FacturacaoAutorizada, e.Chargeable, e.JobNo, e.EntryNo, e.DocumentNo, e.No })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K124_K167_K3_K2_K5_K7_14_24_125");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.SourceCode, e.DataAutorizacaoFacturacao, e.JobNo, e.EntryNo, e.FacturacaoAutorizada, e.Chargeable, e.DocumentNo, e.No })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K2_K124_K167_K5_K7_14_24_125");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.SourceCode, e.DataAutorizacaoFacturacao, e.JobNo, e.FacturacaoAutorizada, e.Chargeable, e.EntryNo, e.DocumentNo, e.No })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K124_K167_K2_K5_K7_14_24_125_f126")
                    .HasFilter("([Job Ledger Entry].[Facturacao Autorizada]=(1) AND [Job Ledger Entry].[Chargeable]=(1))");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.SourceCode, e.DataAutorizacaoFacturacao, e.JobNo, e.No, e.DocumentNo, e.FacturacaoAutorizada, e.Chargeable, e.EntryNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K3_K7_K5_K124_K167_K2_14_24_125");

                entity.HasIndex(e => new { e.TotalPriceLcy, e.SourceCode, e.DataAutorizacaoFacturacao, e.No, e.DocumentNo, e.JobNo, e.FacturacaoAutorizada, e.Chargeable, e.EntryNo })
                    .HasName("_dta_index_Job Ledger Entry_6_139915620__K7_K5_K3_K124_K167_K2_14_24_125");

                entity.HasIndex(e => new { e.EntryNo, e.JobNo, e.PostingDate, e.Description, e.Quantity, e.TotalPriceLcy, e.GlobalDimension1Code, e.GlobalDimension2Code, e.Description2, e.RequisitionNo, e.ObjectDescription, e.DataAutorizacaoFacturacao, e.ContractNo, e.GlobalDimension3Code, e.RequisiçãoInterna, e.Chargeable })
                    .HasName("IX_Job Ledger Entry_Chargeable");

                entity.HasIndex(e => new { e.EntryNo, e.JobNo, e.PostingDate, e.Description, e.Quantity, e.UnitPriceLcy, e.TotalPriceLcy, e.UnitOfMeasureCode, e.GlobalDimension1Code, e.GlobalDimension2Code, e.Description2, e.RequisitionNo, e.ObjectDescription, e.DataAutorizacaoFacturacao, e.ContractNo, e.GlobalDimension3Code, e.RequisiçãoInterna, e.Chargeable, e.No, e.TipoRecurso })
                    .HasName("IX_Job Ledger Entry_Chargeable_No__Tipo Recurso");

                entity.HasIndex(e => new { e.EntryNo, e.PostingDate, e.DocumentNo, e.Type, e.No, e.Description, e.Quantity, e.DirectUnitCostLcy, e.UnitCostLcy, e.TotalCostLcy, e.UnitPriceLcy, e.TotalPriceLcy, e.ResourceGroupNo, e.UnitOfMeasureCode, e.LocationCode, e.JobPostingGroup, e.GlobalDimension1Code, e.GlobalDimension2Code, e.WorkTypeCode, e.CustomerPriceGroup, e.UserId, e.SourceCode, e.AmtToPostToGL, e.AmtPostedToGL, e.AmtToRecognize, e.AmtRecognized, e.EntryType, e.JournalBatchName, e.ReasonCode, e.TransactionType, e.TransportMethod, e.CountryRegionCode, e.GenBusPostingGroup, e.GenProdPostingGroup, e.EntryExitPoint, e.DocumentDate, e.ExternalDocumentNo, e.Area, e.TransactionSpecification, e.NoSeries, e.AdditionalCurrencyTotalCost, e.AddCurrencyTotalPrice, e.AddCurrencyLineAmount, e.JobTaskNo, e.LineAmountLcy, e.UnitCost, e.TotalCost, e.UnitPrice, e.TotalPrice, e.LineAmount, e.LineDiscountAmount, e.LineDiscountAmountLcy, e.CurrencyCode, e.CurrencyFactor, e.Description2, e.LedgerEntryType, e.LedgerEntryNo, e.SerialNo, e.LotNo, e.LineDiscount, e.LineType, e.OriginalUnitCostLcy, e.OriginalTotalCostLcy, e.OriginalUnitCost, e.OriginalTotalCost, e.OriginalTotalCostAcy, e.Adjusted, e.DateTimeAdjusted, e.VariantCode, e.BinCode, e.QtyPerUnitOfMeasure, e.QuantityBase, e.ServiceOrderNo, e.PostedServiceShipmentNo, e.ShipmentMethodCode, e.Subcontratação, e.CentralIncineração, e.NºGuiaResíduosGar, e.NºGuiaExterna, e.DiaDaSemana, e.LocalRecolha, e.TipoProjecto, e.HoraDeRegisto, e.FacturaANºCliente, e.Classe, e.RequisitionNo, e.RequisitionLineNo, e.CódCategoriaProd, e.CódGrupoProd, e.NºCartãoUtente, e.CodigoOrcamento, e.ChaveOrcamento, e.FacturaCriada, e.UtilizadorDiario, e.Motorista, e.ValorDescQuantidade, e.NºLinhaFolha, e.Estado, e.NºGuiaRemessa, e.TipoRefeição, e.DestinoFinalResiduos, e.TipoMovRefeitorio, e.DataDeSistema, e.HoraDeSistema, e.Tipologia, e.ObjectRefNo, e.ObjectRefType, e.MoLineNo, e.MoNo, e.MoTaskLineNo, e.MoToolLineNo, e.MoComponentLineNo, e.MoCostLineNo, e.ObjectType, e.ObjectNo, e.ObjectDescription, e.FlNo, e.FlDescription, e.MaintEntryNo, e.Utilizador, e.TipoManutencao, e.FacturacaoAutorizada, e.DataAutorizacaoFacturacao, e.UtilizadorIdAutorizacao, e.ContractNo, e.GrupoServiço, e.PesagemCliente, e.EnviadoTr, e.OldGLAccountNo, e.CodServCliente, e.DesServCliente, e.LinhaAutFac, e.LinhaCopiada, e.LinhaCopiada2, e.AjudasdeCusto, e.TipoRecurso, e.LinhaOrdemManutenção, e.AjudaCusto, e.RegistadoMc, e.GlobalDimension3Code, e.FacturaçãoAutorizada2, e.Matricula, e.NºOrdemAs4001, e.NºLinhaOm, e.Total1, e.TotalEquipamento, e.DescriçãoTipo, e.DescriçãoMarca, e.DescriçãoModelo, e.NºFolhaDeHoras, e.BAjmo, e.BOrçamento, e.DescontoVenda, e.RequisiçãoInterna, e.NºFuncionario, e.Description22, e.QuantidadeDevolvida, e.NºMovOriginalDaDevolução, e.QtdTransferida, e.TransfParaProj, e.DataDespesa, e.NºPreRegisto, e.NeOriginal, e.NºGrFornecedor, e.Chargeable, e.FromPlanningLineNo, e.DataConsumo, e.Vat, e.ImportadoManage, e.DocumentoOriginal, e.DocumentoCorrigido, e.AcertoPrecos, e.DataDocumentoCorrigido, e.JobNo })
                    .HasName("IX_Job Ledger Entry_Job No_");

                entity.HasIndex(e => new { e.Timestamp, e.EntryNo, e.PostingDate, e.DocumentNo, e.Type, e.No, e.Description, e.Quantity, e.DirectUnitCostLcy, e.UnitCostLcy, e.TotalCostLcy, e.UnitPriceLcy, e.TotalPriceLcy, e.ResourceGroupNo, e.UnitOfMeasureCode, e.LocationCode, e.JobPostingGroup, e.GlobalDimension1Code, e.GlobalDimension2Code, e.WorkTypeCode, e.CustomerPriceGroup, e.UserId, e.SourceCode, e.AmtToPostToGL, e.AmtPostedToGL, e.AmtToRecognize, e.AmtRecognized, e.EntryType, e.JournalBatchName, e.ReasonCode, e.TransactionType, e.TransportMethod, e.CountryRegionCode, e.GenBusPostingGroup, e.GenProdPostingGroup, e.EntryExitPoint, e.DocumentDate, e.ExternalDocumentNo, e.Area, e.TransactionSpecification, e.NoSeries, e.AdditionalCurrencyTotalCost, e.AddCurrencyTotalPrice, e.AddCurrencyLineAmount, e.JobTaskNo, e.LineAmountLcy, e.UnitCost, e.TotalCost, e.UnitPrice, e.TotalPrice, e.LineAmount, e.LineDiscountAmount, e.LineDiscountAmountLcy, e.CurrencyCode, e.CurrencyFactor, e.Description2, e.LedgerEntryType, e.LedgerEntryNo, e.SerialNo, e.LotNo, e.LineDiscount, e.LineType, e.OriginalUnitCostLcy, e.OriginalTotalCostLcy, e.OriginalUnitCost, e.OriginalTotalCost, e.OriginalTotalCostAcy, e.Adjusted, e.DateTimeAdjusted, e.VariantCode, e.BinCode, e.QtyPerUnitOfMeasure, e.QuantityBase, e.ServiceOrderNo, e.PostedServiceShipmentNo, e.ShipmentMethodCode, e.Subcontratação, e.CentralIncineração, e.NºGuiaResíduosGar, e.NºGuiaExterna, e.DiaDaSemana, e.LocalRecolha, e.TipoProjecto, e.HoraDeRegisto, e.FacturaANºCliente, e.Classe, e.RequisitionNo, e.RequisitionLineNo, e.CódCategoriaProd, e.CódGrupoProd, e.NºCartãoUtente, e.CodigoOrcamento, e.ChaveOrcamento, e.FacturaCriada, e.UtilizadorDiario, e.Motorista, e.ValorDescQuantidade, e.NºLinhaFolha, e.Estado, e.NºGuiaRemessa, e.TipoRefeição, e.DestinoFinalResiduos, e.TipoMovRefeitorio, e.DataDeSistema, e.HoraDeSistema, e.Tipologia, e.ObjectRefNo, e.ObjectRefType, e.MoLineNo, e.MoNo, e.MoTaskLineNo, e.MoToolLineNo, e.MoComponentLineNo, e.MoCostLineNo, e.ObjectType, e.ObjectDescription, e.FlNo, e.FlDescription, e.MaintEntryNo, e.Utilizador, e.TipoManutencao, e.FacturacaoAutorizada, e.DataAutorizacaoFacturacao, e.UtilizadorIdAutorizacao, e.ContractNo, e.GrupoServiço, e.PesagemCliente, e.EnviadoTr, e.OldGLAccountNo, e.CodServCliente, e.DesServCliente, e.LinhaAutFac, e.LinhaCopiada, e.LinhaCopiada2, e.AjudasdeCusto, e.TipoRecurso, e.LinhaOrdemManutenção, e.AjudaCusto, e.RegistadoMc, e.GlobalDimension3Code, e.FacturaçãoAutorizada2, e.Matricula, e.NºOrdemAs4001, e.NºLinhaOm, e.Total1, e.TotalEquipamento, e.DescriçãoTipo, e.DescriçãoMarca, e.DescriçãoModelo, e.NºFolhaDeHoras, e.BAjmo, e.DescontoVenda, e.RequisiçãoInterna, e.NºFuncionario, e.Description22, e.QuantidadeDevolvida, e.NºMovOriginalDaDevolução, e.QtdTransferida, e.TransfParaProj, e.DataDespesa, e.NºPreRegisto, e.NeOriginal, e.NºGrFornecedor, e.Chargeable, e.FromPlanningLineNo, e.DataConsumo, e.Vat, e.ImportadoManage, e.DocumentoOriginal, e.DocumentoCorrigido, e.AcertoPrecos, e.DataDocumentoCorrigido, e.JobNo, e.BOrçamento, e.ObjectNo })
                    .HasName("IX_Job Ledger Entry_Job No__B_orçamento_Object No_");

                entity.HasIndex(e => new { e.Timestamp, e.EntryNo, e.JobNo, e.PostingDate, e.DocumentNo, e.Type, e.Description, e.Quantity, e.DirectUnitCostLcy, e.UnitCostLcy, e.TotalCostLcy, e.UnitPriceLcy, e.TotalPriceLcy, e.ResourceGroupNo, e.UnitOfMeasureCode, e.LocationCode, e.JobPostingGroup, e.GlobalDimension1Code, e.GlobalDimension2Code, e.CustomerPriceGroup, e.UserId, e.SourceCode, e.AmtToPostToGL, e.AmtPostedToGL, e.AmtToRecognize, e.AmtRecognized, e.EntryType, e.JournalBatchName, e.ReasonCode, e.TransactionType, e.TransportMethod, e.CountryRegionCode, e.GenBusPostingGroup, e.GenProdPostingGroup, e.EntryExitPoint, e.DocumentDate, e.ExternalDocumentNo, e.Area, e.TransactionSpecification, e.NoSeries, e.AdditionalCurrencyTotalCost, e.AddCurrencyTotalPrice, e.AddCurrencyLineAmount, e.JobTaskNo, e.LineAmountLcy, e.UnitCost, e.TotalCost, e.UnitPrice, e.TotalPrice, e.LineAmount, e.LineDiscountAmount, e.LineDiscountAmountLcy, e.CurrencyCode, e.CurrencyFactor, e.Description2, e.LedgerEntryType, e.LedgerEntryNo, e.SerialNo, e.LotNo, e.LineDiscount, e.LineType, e.OriginalUnitCostLcy, e.OriginalTotalCostLcy, e.OriginalUnitCost, e.OriginalTotalCost, e.OriginalTotalCostAcy, e.Adjusted, e.DateTimeAdjusted, e.VariantCode, e.BinCode, e.QtyPerUnitOfMeasure, e.QuantityBase, e.ServiceOrderNo, e.PostedServiceShipmentNo, e.ShipmentMethodCode, e.Subcontratação, e.CentralIncineração, e.NºGuiaResíduosGar, e.NºGuiaExterna, e.DiaDaSemana, e.LocalRecolha, e.TipoProjecto, e.HoraDeRegisto, e.FacturaANºCliente, e.Classe, e.RequisitionNo, e.RequisitionLineNo, e.CódCategoriaProd, e.CódGrupoProd, e.NºCartãoUtente, e.CodigoOrcamento, e.ChaveOrcamento, e.FacturaCriada, e.UtilizadorDiario, e.Motorista, e.ValorDescQuantidade, e.NºLinhaFolha, e.Estado, e.NºGuiaRemessa, e.TipoRefeição, e.DestinoFinalResiduos, e.TipoMovRefeitorio, e.DataDeSistema, e.HoraDeSistema, e.Tipologia, e.ObjectRefNo, e.ObjectRefType, e.MoLineNo, e.MoNo, e.MoTaskLineNo, e.MoToolLineNo, e.MoComponentLineNo, e.MoCostLineNo, e.ObjectType, e.ObjectNo, e.ObjectDescription, e.FlNo, e.FlDescription, e.MaintEntryNo, e.Utilizador, e.TipoManutencao, e.FacturacaoAutorizada, e.DataAutorizacaoFacturacao, e.UtilizadorIdAutorizacao, e.ContractNo, e.GrupoServiço, e.PesagemCliente, e.EnviadoTr, e.OldGLAccountNo, e.CodServCliente, e.DesServCliente, e.LinhaAutFac, e.LinhaCopiada, e.LinhaCopiada2, e.AjudasdeCusto, e.TipoRecurso, e.LinhaOrdemManutenção, e.AjudaCusto, e.RegistadoMc, e.GlobalDimension3Code, e.FacturaçãoAutorizada2, e.Matricula, e.NºOrdemAs4001, e.NºLinhaOm, e.Total1, e.TotalEquipamento, e.DescriçãoTipo, e.DescriçãoMarca, e.DescriçãoModelo, e.NºFolhaDeHoras, e.BAjmo, e.BOrçamento, e.DescontoVenda, e.RequisiçãoInterna, e.NºFuncionario, e.Description22, e.QuantidadeDevolvida, e.NºMovOriginalDaDevolução, e.QtdTransferida, e.TransfParaProj, e.DataDespesa, e.NºPreRegisto, e.NeOriginal, e.NºGrFornecedor, e.Chargeable, e.FromPlanningLineNo, e.DataConsumo, e.Vat, e.ImportadoManage, e.DocumentoOriginal, e.DocumentoCorrigido, e.AcertoPrecos, e.DataDocumentoCorrigido, e.WsCommunicatedNav2009, e.No, e.WorkTypeCode })
                    .HasName("IX_Job Ledger Entry_No__Work Type Code");

                entity.Property(e => e.EntryNo)
                    .HasColumnName("Entry No_")
                    .ValueGeneratedNever();

                entity.Property(e => e.AcertoPrecos).HasColumnName("Acerto Precos");

                entity.Property(e => e.AddCurrencyLineAmount)
                    .HasColumnName("Add_-Currency Line Amount")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.AddCurrencyTotalPrice)
                    .HasColumnName("Add_-Currency Total Price")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.AdditionalCurrencyTotalCost)
                    .HasColumnName("Additional-Currency Total Cost")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.AmtPostedToGL)
                    .HasColumnName("Amt_ Posted to G_L")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.AmtRecognized)
                    .HasColumnName("Amt_ Recognized")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.AmtToPostToGL)
                    .HasColumnName("Amt_ to Post to G_L")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.AmtToRecognize)
                    .HasColumnName("Amt_ to Recognize")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.Area)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.BAjmo).HasColumnName("B_Ajmo");

                entity.Property(e => e.BOrçamento).HasColumnName("B_orçamento");

                entity.Property(e => e.BinCode)
                    .IsRequired()
                    .HasColumnName("Bin Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CentralIncineração)
                    .IsRequired()
                    .HasColumnName("Central Incineração")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ChaveOrcamento).HasColumnName("Chave Orcamento");

                entity.Property(e => e.Classe)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CodServCliente)
                    .IsRequired()
                    .HasColumnName("Cod_Serv_Cliente")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoOrcamento)
                    .IsRequired()
                    .HasColumnName("Codigo Orcamento")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ContractNo)
                    .IsRequired()
                    .HasColumnName("Contract No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CountryRegionCode)
                    .IsRequired()
                    .HasColumnName("Country_Region Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CurrencyCode)
                    .IsRequired()
                    .HasColumnName("Currency Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CurrencyFactor)
                    .HasColumnName("Currency Factor")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.CustomerPriceGroup)
                    .IsRequired()
                    .HasColumnName("Customer Price Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CódCategoriaProd)
                    .IsRequired()
                    .HasColumnName("Cód_Categoria Prod_")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CódGrupoProd)
                    .IsRequired()
                    .HasColumnName("Cód_Grupo Prod_")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DataAutorizacaoFacturacao)
                    .HasColumnName("Data Autorizacao Facturacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataConsumo)
                    .HasColumnName("Data Consumo")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataDeSistema)
                    .HasColumnName("Data de Sistema")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataDespesa)
                    .HasColumnName("Data Despesa")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataDocumentoCorrigido)
                    .HasColumnName("Data Documento Corrigido")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateTimeAdjusted)
                    .HasColumnName("DateTime Adjusted")
                    .HasColumnType("datetime");

                entity.Property(e => e.DesServCliente)
                    .IsRequired()
                    .HasColumnName("Des_Serv_Cliente")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.DescontoVenda)
                    .HasColumnName("% Desconto Venda")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description2)
                    .IsRequired()
                    .HasColumnName("Description 2")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description22)
                    .IsRequired()
                    .HasColumnName("Description 2_2")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DescriçãoMarca)
                    .IsRequired()
                    .HasColumnName("Descrição Marca")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.DescriçãoModelo)
                    .IsRequired()
                    .HasColumnName("Descrição Modelo")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.DescriçãoTipo)
                    .IsRequired()
                    .HasColumnName("Descrição Tipo")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.DestinoFinalResiduos)
                    .IsRequired()
                    .HasColumnName("Destino Final Residuos")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DiaDaSemana).HasColumnName("Dia da semana");

                entity.Property(e => e.DirectUnitCostLcy)
                    .HasColumnName("Direct Unit Cost (LCY)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.DocumentDate)
                    .HasColumnName("Document Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DocumentNo)
                    .IsRequired()
                    .HasColumnName("Document No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentoCorrigido)
                    .IsRequired()
                    .HasColumnName("Documento Corrigido")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentoOriginal)
                    .IsRequired()
                    .HasColumnName("Documento Original")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.EntryExitPoint)
                    .IsRequired()
                    .HasColumnName("Entry_Exit Point")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.EntryType).HasColumnName("Entry Type");

                entity.Property(e => e.EnviadoTr).HasColumnName("Enviado TR");

                entity.Property(e => e.ExternalDocumentNo)
                    .IsRequired()
                    .HasColumnName("External Document No_")
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.Property(e => e.FacturaANºCliente)
                    .IsRequired()
                    .HasColumnName("Factura-a Nº Cliente")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FacturaCriada).HasColumnName("Factura Criada");

                entity.Property(e => e.FacturacaoAutorizada).HasColumnName("Facturacao Autorizada");

                entity.Property(e => e.FacturaçãoAutorizada2).HasColumnName("Facturação Autorizada2");

                entity.Property(e => e.FlDescription)
                    .IsRequired()
                    .HasColumnName("FL Description")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FlNo)
                    .IsRequired()
                    .HasColumnName("FL No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FromPlanningLineNo).HasColumnName("From Planning Line No_");

                entity.Property(e => e.GenBusPostingGroup)
                    .IsRequired()
                    .HasColumnName("Gen_ Bus_ Posting Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.GenProdPostingGroup)
                    .IsRequired()
                    .HasColumnName("Gen_ Prod_ Posting Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.GlobalDimension1Code)
                    .IsRequired()
                    .HasColumnName("Global Dimension 1 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.GlobalDimension2Code)
                    .IsRequired()
                    .HasColumnName("Global Dimension 2 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.GlobalDimension3Code)
                    .IsRequired()
                    .HasColumnName("Global Dimension 3 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.GrupoServiço)
                    .IsRequired()
                    .HasColumnName("Grupo Serviço")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.HoraDeRegisto)
                    .HasColumnName("Hora de Registo")
                    .HasColumnType("datetime");

                entity.Property(e => e.HoraDeSistema)
                    .HasColumnName("Hora de Sistema")
                    .HasColumnType("datetime");

                entity.Property(e => e.ImportadoManage).HasColumnName("Importado Manage");

                entity.Property(e => e.JobNo)
                    .IsRequired()
                    .HasColumnName("Job No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.JobPostingGroup)
                    .IsRequired()
                    .HasColumnName("Job Posting Group")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.JobTaskNo)
                    .IsRequired()
                    .HasColumnName("Job Task No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.JournalBatchName)
                    .IsRequired()
                    .HasColumnName("Journal Batch Name")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LedgerEntryNo).HasColumnName("Ledger Entry No_");

                entity.Property(e => e.LedgerEntryType).HasColumnName("Ledger Entry Type");

                entity.Property(e => e.LineAmount)
                    .HasColumnName("Line Amount")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.LineAmountLcy)
                    .HasColumnName("Line Amount (LCY)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.LineDiscount)
                    .HasColumnName("Line Discount %")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.LineDiscountAmount)
                    .HasColumnName("Line Discount Amount")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.LineDiscountAmountLcy)
                    .HasColumnName("Line Discount Amount (LCY)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.LineType).HasColumnName("Line Type");

                entity.Property(e => e.LinhaAutFac).HasColumnName("Linha Aut_ Fac_");

                entity.Property(e => e.LinhaCopiada2)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LinhaOrdemManutenção)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LocalRecolha)
                    .IsRequired()
                    .HasColumnName("Local recolha")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LocationCode)
                    .IsRequired()
                    .HasColumnName("Location Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LotNo)
                    .IsRequired()
                    .HasColumnName("Lot No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MaintEntryNo).HasColumnName("Maint_ Entry No_");

                entity.Property(e => e.Matricula)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.MoComponentLineNo).HasColumnName("MO Component Line No_");

                entity.Property(e => e.MoCostLineNo).HasColumnName("MO Cost Line No_");

                entity.Property(e => e.MoLineNo).HasColumnName("MO Line No_");

                entity.Property(e => e.MoNo)
                    .IsRequired()
                    .HasColumnName("MO No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MoTaskLineNo).HasColumnName("MO Task Line No_");

                entity.Property(e => e.MoToolLineNo).HasColumnName("MO Tool Line No_");

                entity.Property(e => e.Motorista)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.NeOriginal)
                    .IsRequired()
                    .HasColumnName("NE Original")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.No)
                    .IsRequired()
                    .HasColumnName("No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NoSeries)
                    .IsRequired()
                    .HasColumnName("No_ Series")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NºCartãoUtente)
                    .IsRequired()
                    .HasColumnName("Nº Cartão Utente")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.NºFolhaDeHoras).HasColumnName("Nº Folha de Horas");

                entity.Property(e => e.NºFuncionario)
                    .IsRequired()
                    .HasColumnName("Nº Funcionario")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NºGrFornecedor)
                    .IsRequired()
                    .HasColumnName("Nº GR Fornecedor")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NºGuiaExterna)
                    .IsRequired()
                    .HasColumnName("Nº Guia Externa")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.NºGuiaRemessa)
                    .IsRequired()
                    .HasColumnName("Nº Guia Remessa")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NºGuiaResíduosGar)
                    .IsRequired()
                    .HasColumnName("Nº Guia Resíduos (GAR)")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.NºLinhaFolha).HasColumnName("Nº Linha Folha");

                entity.Property(e => e.NºLinhaOm).HasColumnName("Nº Linha OM");

                entity.Property(e => e.NºMovOriginalDaDevolução).HasColumnName("Nº Mov_ Original da Devolução");

                entity.Property(e => e.NºOrdemAs4001).HasColumnName("Nº Ordem AS 400-1");

                entity.Property(e => e.NºPreRegisto).HasColumnName("Nº Pre Registo");

                entity.Property(e => e.ObjectDescription)
                    .IsRequired()
                    .HasColumnName("Object Description")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectNo)
                    .IsRequired()
                    .HasColumnName("Object No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectRefNo)
                    .IsRequired()
                    .HasColumnName("Object Ref_ No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectRefType).HasColumnName("Object Ref_ Type");

                entity.Property(e => e.ObjectType).HasColumnName("Object Type");

                entity.Property(e => e.OldGLAccountNo)
                    .IsRequired()
                    .HasColumnName("Old G_L Account No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.OriginalTotalCost)
                    .HasColumnName("Original Total Cost")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.OriginalTotalCostAcy)
                    .HasColumnName("Original Total Cost (ACY)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.OriginalTotalCostLcy)
                    .HasColumnName("Original Total Cost (LCY)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.OriginalUnitCost)
                    .HasColumnName("Original Unit Cost")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.OriginalUnitCostLcy)
                    .HasColumnName("Original Unit Cost (LCY)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.PesagemCliente)
                    .HasColumnName("Pesagem Cliente")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.PostedServiceShipmentNo)
                    .IsRequired()
                    .HasColumnName("Posted Service Shipment No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PostingDate)
                    .HasColumnName("Posting Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.QtdTransferida)
                    .HasColumnName("Qtd Transferida")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.QtyPerUnitOfMeasure)
                    .HasColumnName("Qty_ per Unit of Measure")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.QuantidadeDevolvida)
                    .HasColumnName("Quantidade Devolvida")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.Quantity).HasColumnType("decimal(38, 20)");

                entity.Property(e => e.QuantityBase)
                    .HasColumnName("Quantity (Base)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.ReasonCode)
                    .IsRequired()
                    .HasColumnName("Reason Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RegistadoMc).HasColumnName("Registado%MC");

                entity.Property(e => e.RequisitionLineNo).HasColumnName("Requisition Line No_");

                entity.Property(e => e.RequisitionNo)
                    .IsRequired()
                    .HasColumnName("Requisition No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RequisiçãoInterna)
                    .IsRequired()
                    .HasColumnName("Requisição Interna")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ResourceGroupNo)
                    .IsRequired()
                    .HasColumnName("Resource Group No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SerialNo)
                    .IsRequired()
                    .HasColumnName("Serial No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceOrderNo)
                    .IsRequired()
                    .HasColumnName("Service Order No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShipmentMethodCode)
                    .IsRequired()
                    .HasColumnName("Shipment Method Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SourceCode)
                    .IsRequired()
                    .HasColumnName("Source Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Subcontratação)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.TipoManutencao).HasColumnName("Tipo Manutencao");

                entity.Property(e => e.TipoMovRefeitorio).HasColumnName("Tipo Mov Refeitorio");

                entity.Property(e => e.TipoProjecto).HasColumnName("Tipo Projecto");

                entity.Property(e => e.TipoRecurso).HasColumnName("Tipo Recurso");

                entity.Property(e => e.TipoRefeição)
                    .IsRequired()
                    .HasColumnName("Tipo Refeição")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Tipologia)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Total1).HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TotalCost)
                    .HasColumnName("Total Cost")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TotalCostLcy)
                    .HasColumnName("Total Cost (LCY)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TotalEquipamento)
                    .HasColumnName("Total Equipamento")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TotalPrice)
                    .HasColumnName("Total Price")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TotalPriceLcy)
                    .HasColumnName("Total Price (LCY)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TransactionSpecification)
                    .IsRequired()
                    .HasColumnName("Transaction Specification")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionType)
                    .IsRequired()
                    .HasColumnName("Transaction Type")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TransfParaProj)
                    .IsRequired()
                    .HasColumnName("Transf para Proj")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TransportMethod)
                    .IsRequired()
                    .HasColumnName("Transport Method")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UnitCost)
                    .HasColumnName("Unit Cost")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.UnitCostLcy)
                    .HasColumnName("Unit Cost (LCY)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.UnitOfMeasureCode)
                    .IsRequired()
                    .HasColumnName("Unit of Measure Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UnitPrice)
                    .HasColumnName("Unit Price")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.UnitPriceLcy)
                    .HasColumnName("Unit Price (LCY)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("User ID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Utilizador)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.UtilizadorDiario)
                    .IsRequired()
                    .HasColumnName("Utilizador Diario")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UtilizadorIdAutorizacao)
                    .IsRequired()
                    .HasColumnName("Utilizador Id Autorizacao")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ValorDescQuantidade)
                    .HasColumnName("Valor Desc_ Quantidade")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.VariantCode)
                    .IsRequired()
                    .HasColumnName("Variant Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Vat)
                    .HasColumnName("VAT %")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.WorkTypeCode)
                    .IsRequired()
                    .HasColumnName("Work Type Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.WsCommunicatedNav2009).HasColumnName("Ws Communicated Nav 2009");
            });

            modelBuilder.Entity<LinhaFolhaHoras>(entity =>
            {
                entity.HasKey(e => new { e.NºFolhaHoras, e.TipoCusto, e.NºLinha });

                entity.ToTable("Linha Folha Horas");

                entity.Property(e => e.NºFolhaHoras).HasColumnName("Nº Folha Horas");

                entity.Property(e => e.TipoCusto).HasColumnName("Tipo Custo");

                entity.Property(e => e.NºLinha).HasColumnName("Nº Linha");

                entity.Property(e => e.CalculoAutomático).HasColumnName("Calculo Automático");

                entity.Property(e => e.CustoTotal)
                    .HasColumnName("Custo Total")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.CustoUnitário)
                    .HasColumnName("Custo Unitário")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.CódDestino)
                    .IsRequired()
                    .HasColumnName("Cód_ Destino")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CódOrigem)
                    .IsRequired()
                    .HasColumnName("Cód_ Origem")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CódTipoCusto)
                    .IsRequired()
                    .HasColumnName("Cód_Tipo Custo")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DataDespesa)
                    .HasColumnName("Data Despesa")
                    .HasColumnType("datetime");

                entity.Property(e => e.Distância).HasColumnType("decimal(38, 20)");

                entity.Property(e => e.DistânciaPrevista)
                    .HasColumnName("Distância Prevista")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.Matricula)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Observação)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PreçoUnitário)
                    .HasColumnName("Preço Unitário")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.PreçoVenda)
                    .HasColumnName("Preço Venda")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.Quantidade).HasColumnType("decimal(38, 20)");

                entity.Property(e => e.RegistarSubsídiosEPrémios).HasColumnName("Registar Subsídios e Prémios");

                entity.Property(e => e.RubricaSalarial2)
                    .IsRequired()
                    .HasColumnName("Rubrica Salarial2")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();
            });

            modelBuilder.Entity<LinhaFolhaHorasDistribCus>(entity =>
            {
                entity.HasKey(e => new { e.NºFolhaHoras, e.NºLinhaFolhaHoras, e.TipoCusto, e.NºLinha });

                entity.ToTable("Linha Folha Horas - distribCus");

                entity.Property(e => e.NºFolhaHoras).HasColumnName("Nº Folha Horas");

                entity.Property(e => e.NºLinhaFolhaHoras).HasColumnName("Nº Linha Folha Horas");

                entity.Property(e => e.TipoCusto).HasColumnName("Tipo Custo");

                entity.Property(e => e.NºLinha).HasColumnName("Nº Linha");

                entity.Property(e => e.CódFaseProjecto)
                    .IsRequired()
                    .HasColumnName("Cód_ Fase Projecto")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CódSubfaseProjecto)
                    .IsRequired()
                    .HasColumnName("Cód_ Subfase Projecto")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CódTarefaProjecto)
                    .IsRequired()
                    .HasColumnName("Cód_ Tarefa Projecto")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.GlobalDimension1Code)
                    .IsRequired()
                    .HasColumnName("Global dimension 1 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.GlobalDimension2Code)
                    .IsRequired()
                    .HasColumnName("Global dimension 2 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.KmDistancia).HasColumnType("decimal(38, 20)");

                entity.Property(e => e.KmTotais).HasColumnType("decimal(38, 20)");

                entity.Property(e => e.NºLinhaOrdemManut).HasColumnName("Nº Linha Ordem Manut");

                entity.Property(e => e.NºLinhaTarefaOrdemManut).HasColumnName("Nº Linha Tarefa Ordem Manut_");

                entity.Property(e => e.NºObra)
                    .IsRequired()
                    .HasColumnName("Nº obra")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Quantity).HasColumnType("decimal(38, 20)");

                entity.Property(e => e.ShortcutDimension3Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 3 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension4Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 4 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.TipoObra).HasColumnName("Tipo Obra");

                entity.Property(e => e.Valor).HasColumnType("decimal(38, 20)");

                entity.Property(e => e.Valor1)
                    .HasColumnName("% Valor")
                    .HasColumnType("decimal(38, 20)");
            });

            modelBuilder.Entity<LinhaFolhaHorasMãoObra>(entity =>
            {
                entity.HasKey(e => new { e.NºFolhaHoras, e.NºLinha });

                entity.ToTable("Linha Folha Horas - Mão Obra");

                entity.Property(e => e.NºFolhaHoras).HasColumnName("Nº Folha Horas");

                entity.Property(e => e.NºLinha).HasColumnName("Nº Linha");

                entity.Property(e => e.CustoUnitDirecto)
                    .HasColumnName("Custo Unit_ directo")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.CódFaseProjecto)
                    .IsRequired()
                    .HasColumnName("Cód_ Fase Projecto")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CódSubfaseProjecto)
                    .IsRequired()
                    .HasColumnName("Cód_ Subfase Projecto")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CódTarefaProjecto)
                    .IsRequired()
                    .HasColumnName("Cód_ Tarefa Projecto")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CódTipoTrabalho)
                    .IsRequired()
                    .HasColumnName("Cód_ Tipo trabalho")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CódUnidadeMedida)
                    .IsRequired()
                    .HasColumnName("Cód_ Unidade Medida")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Data).HasColumnType("datetime");

                entity.Property(e => e.Descrição1)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Descrição2)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Descrição3)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FimTrabSup1)
                    .HasColumnName("Fim_Trab_Sup1")
                    .HasColumnType("datetime");

                entity.Property(e => e.FimTrabSup2)
                    .HasColumnName("Fim_Trab_Sup2")
                    .HasColumnType("datetime");

                entity.Property(e => e.GlobalDimension1Code)
                    .IsRequired()
                    .HasColumnName("Global dimension 1 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.GlobalDimension2Code)
                    .IsRequired()
                    .HasColumnName("Global dimension 2 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.HoraDeFim)
                    .HasColumnName("Hora de Fim")
                    .HasColumnType("datetime");

                entity.Property(e => e.HoraDeInício)
                    .HasColumnName("Hora de início")
                    .HasColumnType("datetime");

                entity.Property(e => e.InicioTrabSup1)
                    .HasColumnName("Inicio Trab_Sup1")
                    .HasColumnType("datetime");

                entity.Property(e => e.InicioTrabSup2)
                    .HasColumnName("Inicio Trab_Sup2")
                    .HasColumnType("datetime");

                entity.Property(e => e.Margem)
                    .HasColumnName("% Margem")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.Nfh).HasColumnName("NFH");

                entity.Property(e => e.NumHorasTrabSup1Aux).HasColumnType("decimal(38, 20)");

                entity.Property(e => e.NumHorasTrabSup2Aux).HasColumnType("decimal(38, 20)");

                entity.Property(e => e.NºEmpregado)
                    .IsRequired()
                    .HasColumnName("Nº Empregado")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NºFamíliaRecurso)
                    .IsRequired()
                    .HasColumnName("Nº Família Recurso")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NºHoras)
                    .HasColumnName("Nº Horas")
                    .HasColumnType("datetime");

                entity.Property(e => e.NºHorasAux)
                    .HasColumnName("Nº Horas aux")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.NºHorasTrabSup1)
                    .HasColumnName("Nº Horas Trab_ Sup1")
                    .HasColumnType("datetime");

                entity.Property(e => e.NºHorasTrabSup2)
                    .HasColumnName("Nº Horas Trab_ Sup2")
                    .HasColumnType("datetime");

                entity.Property(e => e.NºLinhaOrdemManut).HasColumnName("Nº Linha Ordem Manut");

                entity.Property(e => e.NºLinhaTarefaOrdemManut).HasColumnName("Nº Linha Tarefa Ordem Manut_");

                entity.Property(e => e.NºObra)
                    .IsRequired()
                    .HasColumnName("Nº obra")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NºRecurso)
                    .IsRequired()
                    .HasColumnName("Nº Recurso")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Observação)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OmOrderType)
                    .IsRequired()
                    .HasColumnName("OM Order Type")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PreçoCusto)
                    .HasColumnName("Preço Custo")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.PreçoTotal)
                    .HasColumnName("Preço total")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.PreçoVenda)
                    .HasColumnName("Preço Venda")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.RegistarTrabalhoSuplementar).HasColumnName("Registar Trabalho Suplementar");

                entity.Property(e => e.ShortcutDimension3Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 3 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension4Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 4 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.TipoAusênciaPresença)
                    .IsRequired()
                    .HasColumnName("Tipo Ausência Presença")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.TipoObra).HasColumnName("Tipo Obra");

                entity.Property(e => e.ValorCusto)
                    .HasColumnName("Valor Custo")
                    .HasColumnType("decimal(38, 20)");
            });

            modelBuilder.Entity<LogoClientes>(entity =>
            {
                entity.ToTable("Logo_Clientes");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ContentType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.LogoClientes)
                    .HasForeignKey<LogoClientes>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Logo_Clientes_Cliente");
            });

            modelBuilder.Entity<Logs>(entity =>
            {
                entity.HasKey(e => e.IdLog);

                entity.HasIndex(e => e.IdLog)
                    .HasName("_dta_index_Logs_6_1076914908__K1");

                entity.Property(e => e.IdLog).HasColumnName("ID_Log");

                entity.Property(e => e.Data).HasColumnType("datetime");

                entity.Property(e => e.IdAccao).HasColumnName("ID_Accao");

                entity.Property(e => e.IdUtilizador).HasColumnName("ID_Utilizador");

                entity.Property(e => e.Tabela).HasMaxLength(100);

                entity.HasOne(d => d.IdAccaoNavigation)
                    .WithMany(p => p.Logs)
                    .HasForeignKey(d => d.IdAccao)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Logs_Accao");
            });

            modelBuilder.Entity<MaintenanceCatalog>(entity =>
            {
                entity.HasKey(e => new { e.Type, e.Code })
                    .HasName("Maintenance Catalog$0");

                entity.ToTable("Maintenance Catalog");

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.FaultReasonType).HasColumnName("Fault Reason Type");

                entity.Property(e => e.IncidenciasAvarias).HasColumnName("Incidencias Avarias");

                entity.Property(e => e.ManutCorrectiva).HasColumnName("Manut_ Correctiva");

                entity.Property(e => e.ManutPreventiva).HasColumnName("Manut_ Preventiva");

                entity.Property(e => e.OrdensEmCurso).HasColumnName("Ordens em Curso");

                entity.Property(e => e.TaxaCoberturaCats).HasColumnName("Taxa Cobertura CATs");

                entity.Property(e => e.TaxaCumprimentoCats).HasColumnName("Taxa Cumprimento CATs");

                entity.Property(e => e.TaxaCumprimentoRotinasMp).HasColumnName("Taxa Cumprimento Rotinas MP");

                entity.Property(e => e.TempoEfectivoReparação).HasColumnName("Tempo Efectivo Reparação");

                entity.Property(e => e.TempoFacturação).HasColumnName("Tempo Facturação");

                entity.Property(e => e.TempoFechoObras).HasColumnName("Tempo Fecho Obras");

                entity.Property(e => e.TempoImobilização).HasColumnName("Tempo Imobilização");

                entity.Property(e => e.TempoOcupColaboradores).HasColumnName("Tempo Ocup Colaboradores");

                entity.Property(e => e.TempoResposta).HasColumnName("Tempo Resposta");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.ValorCustoVenda).HasColumnName("Valor Custo Venda");
            });

            modelBuilder.Entity<MaintenanceHeaderComments>(entity =>
            {
                entity.HasKey(e => new { e.TableName, e.No, e.LineNo });

                entity.ToTable("Maintenance Header Comments");

                entity.HasIndex(e => new { e.TableName, e.LineNo, e.Date, e.Code, e.Comment, e.OrcAlternativo, e.No })
                    .HasName("_dta_index_Maintenance Header Comments_6_1692585118__K8_K3_2_4_5_6_7");

                entity.Property(e => e.TableName).HasColumnName("Table Name");

                entity.Property(e => e.No)
                    .HasColumnName("No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LineNo).HasColumnName("Line No_");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.OrcAlternativo).HasColumnName("ORC_Alternativo");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();
            });

            modelBuilder.Entity<MaintenanceOrder>(entity =>
            {
                entity.HasKey(e => new { e.DocumentType, e.No })
                    .HasName("Maintenance Order$0");

                entity.ToTable("Maintenance Order");

                entity.HasIndex(e => e.IdServicoEvolution);

                entity.HasIndex(e => e.No)
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_f20")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => e.Status)
                    .HasName("_dta_index_Maintenance Order_6_859918185__K15_f24")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => e.UserChefeProjecto)
                    .HasName("_dta_index_Maintenance Order_6_859918185__K168_6478");

                entity.HasIndex(e => e.UserResponsavel)
                    .HasName("_dta_index_Maintenance Order_6_859918185__K170_4149");

                entity.HasIndex(e => new { e.No, e.Status })
                    .HasName("_dta_stat_859918185_3_15")
                    .HasFilter("([Maintenance Order].[Status]<(3) AND [Maintenance Order].[Status]>(3))");

                entity.HasIndex(e => new { e.ShortcutDimension3Code, e.No })
                    .HasName("_dta_stat_859918185_100_3");

                entity.HasIndex(e => new { e.Status, e.CustomerNo })
                    .HasName("_dta_stat_859918185_15_19");

                entity.HasIndex(e => new { e.Status, e.No })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K15_K3_f130")
                    .HasFilter("([Maintenance Order].[No_]='OM1805857')");

                entity.HasIndex(e => new { e.CustomerNo, e.No, e.Status })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K19_K3_K15_f23")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.CustomerNo, e.ShortcutDimension1Code, e.No })
                    .HasName("_dta_stat_859918185_19_38_3");

                entity.HasIndex(e => new { e.Description, e.PlannedOrderNo, e.No })
                    .HasName("IX_Maintenance Order_No_");

                entity.HasIndex(e => new { e.No, e.Description, e.Status })
                    .HasName("IX_Maintenance Order_Status");

                entity.HasIndex(e => new { e.No, e.Status, e.CustomerNo })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_K15_K19_f5")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.No, e.Status, e.ShortcutDimension1Code })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_K15_K38_f11")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.No, e.Status, e.ShortcutDimension2Code })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_K15_K39_f4")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.No, e.Status, e.ShortcutDimension3Code })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_K15_K100_f14")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.No, e.UserResponsavel, e.UserChefeProjecto })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_K170_K168_3369");

                entity.HasIndex(e => new { e.ShortcutDimension1Code, e.No, e.Status })
                    .HasName("_dta_stat_859918185_38_3_15")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.ShortcutDimension2Code, e.No, e.Status })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K39_K3_K15_f13")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.ShortcutDimension3Code, e.No, e.Status })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K100_K3_K15_f19")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.CustomerNo, e.No, e.Status, e.ShortcutDimension1Code })
                    .HasName("_dta_stat_859918185_19_3_15_38");

                entity.HasIndex(e => new { e.IdInstituicaoEvolution, e.No, e.UserResponsavel, e.UserChefeProjecto })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K180_K3_K170_K168_5201");

                entity.HasIndex(e => new { e.IdServicoEvolution, e.No, e.UserResponsavel, e.UserChefeProjecto })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K181_K3_K170_K168_3923");

                entity.HasIndex(e => new { e.No, e.UserResponsavel, e.UserChefeProjecto, e.IdInstituicaoEvolution })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_K170_K168_K180_4364");

                entity.HasIndex(e => new { e.No, e.UserResponsavel, e.UserChefeProjecto, e.IdServicoEvolution })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_K170_K168_K181_9987_4364");

                entity.HasIndex(e => new { e.No, e.UserResponsavel, e.UserChefeProjecto, e.TécnicoExecutante })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_K170_K168_K173_440");

                entity.HasIndex(e => new { e.Status, e.No, e.Description, e.PlannedOrderNo })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_K4_K79_15");

                entity.HasIndex(e => new { e.TécnicoExecutante, e.No, e.UserResponsavel, e.UserChefeProjecto })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K173_K3_K170_K168_9850");

                entity.HasIndex(e => new { e.CustomerNo, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.ShortcutDimension3Code, e.No })
                    .HasName("_dta_stat_859918185_19_38_39_100_3");

                entity.HasIndex(e => new { e.No, e.Status, e.ShortcutDimension3Code, e.CustomerNo, e.ShortcutDimension1Code })
                    .HasName("_dta_stat_859918185_3_15_100_19_38");

                entity.HasIndex(e => new { e.No, e.Description, e.ContractNo, e.CustomerNo, e.ShortcutDimension3Code, e.OrderType })
                    .HasName("IX_Maintenance Order_Order Type");

                entity.HasIndex(e => new { e.ShortcutDimension2Code, e.No, e.Status, e.CustomerNo, e.ShortcutDimension1Code, e.ShortcutDimension3Code })
                    .HasName("_dta_stat_859918185_39_3_15_19_38_100");

                entity.HasIndex(e => new { e.FinishingDate, e.DataFecho, e.CustomerNo, e.No, e.Status, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.ShortcutDimension3Code })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K19_K3_K15_K38_K39_K100_72_102_f41")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.FinishingDate, e.DataFecho, e.CustomerNo, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.ShortcutDimension3Code, e.No, e.Status })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K19_K38_K39_K100_K3_K15_72_102");

                entity.HasIndex(e => new { e.FinishingDate, e.DataFecho, e.No, e.Status, e.CustomerNo, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.ShortcutDimension3Code })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_K15_K19_K38_K39_K100_72_102_f36")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.FinishingDate, e.DataFecho, e.No, e.Status, e.ShortcutDimension1Code, e.CustomerNo, e.ShortcutDimension2Code, e.ShortcutDimension3Code })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_K15_K38_K19_K39_K100_72_102_f42")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.FinishingDate, e.DataFecho, e.No, e.Status, e.ShortcutDimension2Code, e.CustomerNo, e.ShortcutDimension1Code, e.ShortcutDimension3Code })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_K15_K39_K19_K38_K100_72_102_f37")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.FinishingDate, e.DataFecho, e.No, e.Status, e.ShortcutDimension3Code, e.CustomerNo, e.ShortcutDimension1Code, e.ShortcutDimension2Code })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_K15_K100_K19_K38_K39_72_102_f35")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.FinishingDate, e.DataFecho, e.ShortcutDimension1Code, e.No, e.Status, e.CustomerNo, e.ShortcutDimension2Code, e.ShortcutDimension3Code })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K38_K3_K15_K19_K39_K100_72_102_f34")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.FinishingDate, e.DataFecho, e.ShortcutDimension2Code, e.No, e.Status, e.CustomerNo, e.ShortcutDimension1Code, e.ShortcutDimension3Code })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K39_K3_K15_K19_K38_K100_72_102_f39")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.FinishingDate, e.DataFecho, e.ShortcutDimension3Code, e.No, e.Status, e.CustomerNo, e.ShortcutDimension1Code, e.ShortcutDimension2Code })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K100_K3_K15_K19_K38_K39_72_102_f38")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.FinishingDate, e.DataFecho, e.Status, e.CustomerNo, e.No, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.ShortcutDimension3Code })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K15_K19_K3_K38_K39_K100_72_102_f40")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.Status, e.CustomerNo, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.FinishingDate, e.ShortcutDimension3Code, e.DataFecho, e.No })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_15_19_38_39_72_100_102");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.CustomerNo, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.FinishingDate, e.ShortcutDimension3Code, e.DataFecho, e.No, e.Status })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_K15_4_9_13_19_38_39_72_100_102_f133")
                    .HasFilter("([Maintenance Order].[Status]<(3) AND [Maintenance Order].[Status]>(3))");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.CustomerNo, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.FinishingDate, e.ShortcutDimension3Code, e.DataFecho, e.Status, e.No })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K15_K3_4_9_13_19_38_39_72_100_102_f129")
                    .HasFilter("([Maintenance Order].[No_]='OM1805857')");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.FinishingDate, e.DataFecho, e.CustomerNo, e.No, e.Status, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.ShortcutDimension3Code })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K19_K3_K15_K38_K39_K100_4_9_13_72_102_f17")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.FinishingDate, e.DataFecho, e.CustomerNo, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.ShortcutDimension3Code, e.No, e.Status })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K19_K38_K39_K100_K3_K15_4_9_13_72_102");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.FinishingDate, e.DataFecho, e.No, e.Status, e.CustomerNo, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.ShortcutDimension3Code })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_K15_K19_K38_K39_K100_4_9_13_72_102_f18")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.FinishingDate, e.DataFecho, e.No, e.Status, e.ShortcutDimension1Code, e.CustomerNo, e.ShortcutDimension2Code, e.ShortcutDimension3Code })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_K15_K38_K19_K39_K100_4_9_13_72_102_f8")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.FinishingDate, e.DataFecho, e.No, e.Status, e.ShortcutDimension2Code, e.CustomerNo, e.ShortcutDimension1Code, e.ShortcutDimension3Code })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_K15_K39_K19_K38_K100_4_9_13_72_102_f3")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.FinishingDate, e.DataFecho, e.No, e.Status, e.ShortcutDimension3Code, e.CustomerNo, e.ShortcutDimension1Code, e.ShortcutDimension2Code })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_K15_K100_K19_K38_K39_4_9_13_72_102_f7")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.FinishingDate, e.DataFecho, e.ShortcutDimension1Code, e.No, e.Status, e.CustomerNo, e.ShortcutDimension2Code, e.ShortcutDimension3Code })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K38_K3_K15_K19_K39_K100_4_9_13_72_102_f6")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.FinishingDate, e.DataFecho, e.ShortcutDimension2Code, e.No, e.Status, e.CustomerNo, e.ShortcutDimension1Code, e.ShortcutDimension3Code })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K39_K3_K15_K19_K38_K100_4_9_13_72_102_f22")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.FinishingDate, e.DataFecho, e.ShortcutDimension3Code, e.No, e.Status, e.CustomerNo, e.ShortcutDimension1Code, e.ShortcutDimension2Code })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K100_K3_K15_K19_K38_K39_4_9_13_72_102_f10")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.FinishingDate, e.DataFecho, e.Status, e.CustomerNo, e.No, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.ShortcutDimension3Code })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K15_K19_K3_K38_K39_K100_4_9_13_72_102_f9")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.FinishingDate, e.ShortcutDimension3Code, e.DataFecho, e.CustomerNo, e.No, e.Status })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K19_K3_K15_4_9_13_38_39_72_100_102_f27")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.FinishingDate, e.ShortcutDimension3Code, e.DataFecho, e.No, e.CustomerNo, e.Status })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_K19_K15_4_9_13_38_39_72_100_102");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.FinishingDate, e.ShortcutDimension3Code, e.DataFecho, e.No, e.Status, e.CustomerNo })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_K15_K19_4_9_13_38_39_72_100_102_f28")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.FinishingDate, e.ShortcutDimension3Code, e.DataFecho, e.Status, e.CustomerNo, e.No })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K15_K19_K3_4_9_13_38_39_72_100_102_f26")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.FinishingDate, e.ShortcutDimension3Code, e.DataFecho, e.Status, e.No, e.CustomerNo })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K15_K3_K19_4_9_13_38_39_72_100_102_f92")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.ShortcutDimension2Code, e.FinishingDate, e.ShortcutDimension3Code, e.DataFecho, e.CustomerNo, e.No, e.Status, e.ShortcutDimension1Code })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K19_K3_K15_K38_4_9_13_39_72_100_102_f32")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.ShortcutDimension2Code, e.FinishingDate, e.ShortcutDimension3Code, e.DataFecho, e.CustomerNo, e.ShortcutDimension1Code, e.No, e.Status })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K19_K38_K3_K15_4_9_13_39_72_100_102");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.ShortcutDimension2Code, e.FinishingDate, e.ShortcutDimension3Code, e.DataFecho, e.No, e.Status, e.CustomerNo, e.ShortcutDimension1Code })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_K15_K19_K38_4_9_13_39_72_100_102_f33")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.ShortcutDimension2Code, e.FinishingDate, e.ShortcutDimension3Code, e.DataFecho, e.No, e.Status, e.ShortcutDimension1Code, e.CustomerNo })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_K15_K38_K19_4_9_13_39_72_100_102_f30")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.ShortcutDimension2Code, e.FinishingDate, e.ShortcutDimension3Code, e.DataFecho, e.ShortcutDimension1Code, e.No, e.Status, e.CustomerNo })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K38_K3_K15_K19_4_9_13_39_72_100_102_f29")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.ShortcutDimension2Code, e.FinishingDate, e.ShortcutDimension3Code, e.DataFecho, e.Status, e.CustomerNo, e.No, e.ShortcutDimension1Code })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K15_K19_K3_K38_4_9_13_39_72_100_102_f31")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.Status, e.CustomerNo, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.FinishingDate, e.ShortcutDimension3Code, e.DataFecho, e.No })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_4_9_13_15_19_38_39_72_100_102");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.FinishingDate, e.DataFecho, e.NoDocumentoContactoInicial, e.ShortcutDimension1Code, e.No, e.Status, e.CustomerNo, e.ShortcutDimension2Code, e.ShortcutDimension3Code })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K38_K3_K15_K19_K39_K100_4_9_13_72_102_175");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.FinishingDate, e.ShortcutDimension3Code, e.DataFecho, e.NoDocumentoContactoInicial, e.CustomerNo, e.No, e.Status })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K19_K3_K15_4_9_13_38_39_72_100_102_175_f75")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.FinishingDate, e.ShortcutDimension3Code, e.DataFecho, e.NoDocumentoContactoInicial, e.No, e.Status, e.CustomerNo })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_K15_K19_4_9_13_38_39_72_100_102_175_f73")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.FinishingDate, e.ShortcutDimension3Code, e.DataFecho, e.NoDocumentoContactoInicial, e.Status, e.CustomerNo, e.No })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K15_K19_K3_4_9_13_38_39_72_100_102_175_f74")
                    .HasFilter("([Maintenance Order].[Status]<>(3))");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.FinishingDate, e.ShortcutDimension3Code, e.DataFecho, e.NoDocumentoContactoInicial, e.Status, e.No, e.CustomerNo })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K15_K3_K19_4_9_13_38_39_72_100_102_175");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.ContractNo, e.Status, e.CustomerNo, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.FinishingDate, e.ShortcutDimension3Code, e.DataFecho, e.NoDocumentoContactoInicial, e.No })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_4_9_13_15_19_38_39_72_100_102_175");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.SourceDocNo, e.ContractNo, e.Status, e.ResponsibilityCenter, e.ShortcutDimension2Code, e.EnteredBy, e.OrderDate, e.OrderTime, e.VatRegistrationNo, e.TipoContactoCliente, e.CustomerDocNo, e.ShortcutDimension3Code, e.NoCompromisso, e.NoDocumentoContactoInicial, e.TipoContactoClienteInicial, e.IdClienteEvolution, e.IdInstituicaoEvolution, e.IdServicoEvolution, e.ReferenciaEncomenda, e.ShortcutDimension1Code, e.No })
                    .HasName("IX_Maintenance Order_Shortcut Dimension 1 Code_No_");

                entity.HasIndex(e => new { e.Timestamp, e.DocumentType, e.Description, e.ObjectRefType, e.ObjectRefNo, e.ObjectRefDescription, e.ComponentOf, e.OrderType, e.MaintenanceActivity, e.SourceDocType, e.SourceDocNo, e.ContractNo, e.Priority, e.Status, e.SuspendedOrderReason, e.ResponsibilityCenter, e.LastDateModified, e.CustomerNo, e.CustomerName, e.CustomerName2, e.CustomerAddress, e.CustomerAddress2, e.CustomerCity, e.CustomerPostCode, e.CustomerPhoneNo, e.CustomerEMail, e.CustomerShipToCode, e.CustomerFaxNo, e.CustomerReference, e.CustomerContactName, e.CustomerCountryCode, e.PostingDate, e.CustomerCounty, e.JobNo, e.ApplicationMethod, e.LanguageCode, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.RespCenterCountryCode, e.TotalQuantity, e.TotalQtyToInvoice, e.RespCenterName, e.RespCenterName2, e.RespCenterFaxNo, e.RespCenterCounty, e.RespCenterAddress, e.RespCenterAddress2, e.RespCenterPostCode, e.RespCenterCity, e.RespCenterContact, e.RespCenterPhoneNo, e.RespCenterReference, e.FaNo, e.FlNo, e.FlDescription, e.ResponsibleEmployee, e.EnteredBy, e.MaintenanceResponsible, e.PlannerGroupNo, e.OrderDate, e.OrderTime, e.DocumentDate, e.ExpectedFinishingDate, e.ExpectedFinishingTime, e.ExpectedStartingDate, e.ExpectedStartingTime, e.StartingDate, e.StartingTime, e.ResponseTimeHours, e.MaintenanceTimeHours, e.FinishingDate, e.FinishingTime, e.GenBusPostingGroup, e.CustomerPriceGroup, e.CustomerDiscGroup, e.VatRegistrationNo, e.PurchaserCode, e.PlannedOrderNo, e.NoSeries, e.Reserve, e.Validade, e.Budget, e.FaPostingGroup, e.WorkCenterNo, e.MachineCenterNo, e.FinishingTimeHours, e.TipoContactoCliente, e.CustomerDocNo, e.JobPostingGroup, e.ShipToCode, e.ShipToName, e.ShipToName2, e.ShipToAddress, e.ShipToAddress2, e.ShipToPostCode, e.ShipToCity, e.ShipToCounty, e.ShipToContact, e.ShortcutDimension3Code, e.ShortcutDimension4Code, e.DataFecho, e.HoraFecho, e.Loc1, e.EstadoOrcamento, e.NoDocumentoEnviado, e.FormaDeEnvio, e.DataDeEnvio, e.DataEntrada, e.NºGeste, e.DataEntrega, e.DataSaída, e.OrigemOrdem, e.Loc2, e.Loc3, e.Urgência, e.PrioridadeObra, e.FechoTécnicoObra, e.PrazoDeExecuçãoDaOrdem, e.Descrição1, e.Descrição2, e.Descrição3, e.ValorTotalPrev, e.TotalQPrev, e.TotalQReal, e.NºLinhaContrato, e.DataReabertura, e.HoraReabertura, e.NºAntigoAs400, e.ValorFacturado, e.ObjectoManutençãoAs400, e.TotalQuantidadeReal, e.ValorCustoRealTotal, e.ClienteContrato, e.TotalQuantidadeFact, e.TotalValorFact, e.PMargem, e.Margem, e.FTextDescDim1, e.Cc, e.Paginas, e.De, e.Compensa, e.NãoCompensa, e.ObraReclamada, e.NºReclamacao, e.DescricaoReclamacao, e.DataPedidoReparação, e.HoraPedidoReparação, e.FechadoPor, e.ReabertoPor, e.MensagemImpressoOrdem, e.NovaReconv, e.ObjectoServiço, e.DataPedido, e.DataValidade, e.ValidadePedido, e.ValorProjecto, e.DeliberaçãoCa, e.ServInternosRequisições, e.ServInternosFolhasDeObra, e.ServInternosDébInternos, e.MãoDeObraEDeslocações, e.ConfigResponsavel, e.DataUltimoMail, e.DataChefeProjecto, e.DataResponsavel, e.DataFacturação, e.NoCompromisso, e.NoDocumentoContactoInicial, e.TipoContactoClienteInicial, e.IdClienteEvolution, e.IdTecnico1, e.IdTecnico2, e.IdTecnico3, e.IdTecnico4, e.IdTecnico5, e.ReferenciaEncomenda, e.IdInstituicaoEvolution, e.IdServicoEvolution, e.TécnicoExecutante, e.No, e.UserChefeProjecto, e.UserResponsavel })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K180_K181_K173_K3_K168_K170_1_2_4_5_6_7_8_9_10_11_12_13_14_15_16_17_18_19_20_21_22_");

                entity.HasIndex(e => new { e.Timestamp, e.DocumentType, e.Description, e.ObjectRefType, e.ObjectRefNo, e.ObjectRefDescription, e.ComponentOf, e.OrderType, e.MaintenanceActivity, e.SourceDocType, e.SourceDocNo, e.ContractNo, e.Priority, e.Status, e.SuspendedOrderReason, e.ResponsibilityCenter, e.LastDateModified, e.CustomerNo, e.CustomerName, e.CustomerName2, e.CustomerAddress, e.CustomerAddress2, e.CustomerCity, e.CustomerPostCode, e.CustomerPhoneNo, e.CustomerEMail, e.CustomerShipToCode, e.CustomerFaxNo, e.CustomerReference, e.CustomerContactName, e.CustomerCountryCode, e.PostingDate, e.CustomerCounty, e.JobNo, e.ApplicationMethod, e.LanguageCode, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.RespCenterCountryCode, e.TotalQuantity, e.TotalQtyToInvoice, e.RespCenterName, e.RespCenterName2, e.RespCenterFaxNo, e.RespCenterCounty, e.RespCenterAddress, e.RespCenterAddress2, e.RespCenterPostCode, e.RespCenterCity, e.RespCenterContact, e.RespCenterPhoneNo, e.RespCenterReference, e.FaNo, e.FlNo, e.FlDescription, e.ResponsibleEmployee, e.EnteredBy, e.MaintenanceResponsible, e.PlannerGroupNo, e.OrderDate, e.OrderTime, e.DocumentDate, e.ExpectedFinishingDate, e.ExpectedFinishingTime, e.ExpectedStartingDate, e.ExpectedStartingTime, e.StartingDate, e.StartingTime, e.ResponseTimeHours, e.MaintenanceTimeHours, e.FinishingDate, e.FinishingTime, e.GenBusPostingGroup, e.CustomerPriceGroup, e.CustomerDiscGroup, e.VatRegistrationNo, e.PurchaserCode, e.PlannedOrderNo, e.NoSeries, e.Reserve, e.Validade, e.Budget, e.FaPostingGroup, e.WorkCenterNo, e.MachineCenterNo, e.FinishingTimeHours, e.TipoContactoCliente, e.CustomerDocNo, e.JobPostingGroup, e.ShipToCode, e.ShipToName, e.ShipToName2, e.ShipToAddress, e.ShipToAddress2, e.ShipToPostCode, e.ShipToCity, e.ShipToCounty, e.ShipToContact, e.ShortcutDimension3Code, e.ShortcutDimension4Code, e.DataFecho, e.HoraFecho, e.Loc1, e.EstadoOrcamento, e.NoDocumentoEnviado, e.FormaDeEnvio, e.DataDeEnvio, e.DataEntrada, e.NºGeste, e.DataEntrega, e.DataSaída, e.OrigemOrdem, e.Loc2, e.Loc3, e.Urgência, e.PrioridadeObra, e.FechoTécnicoObra, e.PrazoDeExecuçãoDaOrdem, e.Descrição1, e.Descrição2, e.Descrição3, e.ValorTotalPrev, e.TotalQPrev, e.TotalQReal, e.NºLinhaContrato, e.DataReabertura, e.HoraReabertura, e.NºAntigoAs400, e.ValorFacturado, e.ObjectoManutençãoAs400, e.TotalQuantidadeReal, e.ValorCustoRealTotal, e.ClienteContrato, e.TotalQuantidadeFact, e.TotalValorFact, e.PMargem, e.Margem, e.FTextDescDim1, e.Cc, e.Paginas, e.De, e.Compensa, e.NãoCompensa, e.ObraReclamada, e.NºReclamacao, e.DescricaoReclamacao, e.DataPedidoReparação, e.HoraPedidoReparação, e.FechadoPor, e.ReabertoPor, e.MensagemImpressoOrdem, e.NovaReconv, e.ObjectoServiço, e.DataPedido, e.DataValidade, e.ValidadePedido, e.ValorProjecto, e.DeliberaçãoCa, e.ServInternosRequisições, e.ServInternosFolhasDeObra, e.ServInternosDébInternos, e.MãoDeObraEDeslocações, e.ConfigResponsavel, e.DataUltimoMail, e.DataChefeProjecto, e.DataResponsavel, e.DataFacturação, e.NoCompromisso, e.NoDocumentoContactoInicial, e.TipoContactoClienteInicial, e.IdClienteEvolution, e.IdTecnico1, e.IdTecnico2, e.IdTecnico3, e.IdTecnico4, e.IdTecnico5, e.ReferenciaEncomenda, e.IdInstituicaoEvolution, e.No, e.UserResponsavel, e.UserChefeProjecto, e.IdServicoEvolution, e.TécnicoExecutante })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K180_K3_K170_K168_K181_K173_1_2_4_5_6_7_8_9_10_11_12_13_14_15_16_17_18_19_20_21_22_");

                entity.HasIndex(e => new { e.Timestamp, e.DocumentType, e.Description, e.ObjectRefType, e.ObjectRefNo, e.ObjectRefDescription, e.ComponentOf, e.OrderType, e.MaintenanceActivity, e.SourceDocType, e.SourceDocNo, e.ContractNo, e.Priority, e.Status, e.SuspendedOrderReason, e.ResponsibilityCenter, e.LastDateModified, e.CustomerNo, e.CustomerName, e.CustomerName2, e.CustomerAddress, e.CustomerAddress2, e.CustomerCity, e.CustomerPostCode, e.CustomerPhoneNo, e.CustomerEMail, e.CustomerShipToCode, e.CustomerFaxNo, e.CustomerReference, e.CustomerContactName, e.CustomerCountryCode, e.PostingDate, e.CustomerCounty, e.JobNo, e.ApplicationMethod, e.LanguageCode, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.RespCenterCountryCode, e.TotalQuantity, e.TotalQtyToInvoice, e.RespCenterName, e.RespCenterName2, e.RespCenterFaxNo, e.RespCenterCounty, e.RespCenterAddress, e.RespCenterAddress2, e.RespCenterPostCode, e.RespCenterCity, e.RespCenterContact, e.RespCenterPhoneNo, e.RespCenterReference, e.FaNo, e.FlNo, e.FlDescription, e.ResponsibleEmployee, e.EnteredBy, e.MaintenanceResponsible, e.PlannerGroupNo, e.OrderDate, e.OrderTime, e.DocumentDate, e.ExpectedFinishingDate, e.ExpectedFinishingTime, e.ExpectedStartingDate, e.ExpectedStartingTime, e.StartingDate, e.StartingTime, e.ResponseTimeHours, e.MaintenanceTimeHours, e.FinishingDate, e.FinishingTime, e.GenBusPostingGroup, e.CustomerPriceGroup, e.CustomerDiscGroup, e.VatRegistrationNo, e.PurchaserCode, e.PlannedOrderNo, e.NoSeries, e.Reserve, e.Validade, e.Budget, e.FaPostingGroup, e.WorkCenterNo, e.MachineCenterNo, e.FinishingTimeHours, e.TipoContactoCliente, e.CustomerDocNo, e.JobPostingGroup, e.ShipToCode, e.ShipToName, e.ShipToName2, e.ShipToAddress, e.ShipToAddress2, e.ShipToPostCode, e.ShipToCity, e.ShipToCounty, e.ShipToContact, e.ShortcutDimension3Code, e.ShortcutDimension4Code, e.DataFecho, e.HoraFecho, e.Loc1, e.EstadoOrcamento, e.NoDocumentoEnviado, e.FormaDeEnvio, e.DataDeEnvio, e.DataEntrada, e.NºGeste, e.DataEntrega, e.DataSaída, e.OrigemOrdem, e.Loc2, e.Loc3, e.Urgência, e.PrioridadeObra, e.FechoTécnicoObra, e.PrazoDeExecuçãoDaOrdem, e.Descrição1, e.Descrição2, e.Descrição3, e.ValorTotalPrev, e.TotalQPrev, e.TotalQReal, e.NºLinhaContrato, e.DataReabertura, e.HoraReabertura, e.NºAntigoAs400, e.ValorFacturado, e.ObjectoManutençãoAs400, e.TotalQuantidadeReal, e.ValorCustoRealTotal, e.ClienteContrato, e.TotalQuantidadeFact, e.TotalValorFact, e.PMargem, e.Margem, e.FTextDescDim1, e.Cc, e.Paginas, e.De, e.Compensa, e.NãoCompensa, e.ObraReclamada, e.NºReclamacao, e.DescricaoReclamacao, e.DataPedidoReparação, e.HoraPedidoReparação, e.FechadoPor, e.ReabertoPor, e.MensagemImpressoOrdem, e.NovaReconv, e.ObjectoServiço, e.DataPedido, e.DataValidade, e.ValidadePedido, e.ValorProjecto, e.DeliberaçãoCa, e.ServInternosRequisições, e.ServInternosFolhasDeObra, e.ServInternosDébInternos, e.MãoDeObraEDeslocações, e.ConfigResponsavel, e.DataUltimoMail, e.DataChefeProjecto, e.DataResponsavel, e.DataFacturação, e.NoCompromisso, e.NoDocumentoContactoInicial, e.TipoContactoClienteInicial, e.IdClienteEvolution, e.IdTecnico1, e.IdTecnico2, e.IdTecnico3, e.IdTecnico4, e.IdTecnico5, e.ReferenciaEncomenda, e.IdServicoEvolution, e.No, e.UserResponsavel, e.UserChefeProjecto, e.IdInstituicaoEvolution, e.TécnicoExecutante })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K181_K3_K170_K168_K180_K173_1_2_4_5_6_7_8_9_10_11_12_13_14_15_16_17_18_19_20_21_22_");

                entity.HasIndex(e => new { e.Timestamp, e.DocumentType, e.Description, e.ObjectRefType, e.ObjectRefNo, e.ObjectRefDescription, e.ComponentOf, e.OrderType, e.MaintenanceActivity, e.SourceDocType, e.SourceDocNo, e.ContractNo, e.Priority, e.Status, e.SuspendedOrderReason, e.ResponsibilityCenter, e.LastDateModified, e.CustomerNo, e.CustomerName, e.CustomerName2, e.CustomerAddress, e.CustomerAddress2, e.CustomerCity, e.CustomerPostCode, e.CustomerPhoneNo, e.CustomerEMail, e.CustomerShipToCode, e.CustomerFaxNo, e.CustomerReference, e.CustomerContactName, e.CustomerCountryCode, e.PostingDate, e.CustomerCounty, e.JobNo, e.ApplicationMethod, e.LanguageCode, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.RespCenterCountryCode, e.TotalQuantity, e.TotalQtyToInvoice, e.RespCenterName, e.RespCenterName2, e.RespCenterFaxNo, e.RespCenterCounty, e.RespCenterAddress, e.RespCenterAddress2, e.RespCenterPostCode, e.RespCenterCity, e.RespCenterContact, e.RespCenterPhoneNo, e.RespCenterReference, e.FaNo, e.FlNo, e.FlDescription, e.ResponsibleEmployee, e.EnteredBy, e.MaintenanceResponsible, e.PlannerGroupNo, e.OrderDate, e.OrderTime, e.DocumentDate, e.ExpectedFinishingDate, e.ExpectedFinishingTime, e.ExpectedStartingDate, e.ExpectedStartingTime, e.StartingDate, e.StartingTime, e.ResponseTimeHours, e.MaintenanceTimeHours, e.FinishingDate, e.FinishingTime, e.GenBusPostingGroup, e.CustomerPriceGroup, e.CustomerDiscGroup, e.VatRegistrationNo, e.PurchaserCode, e.PlannedOrderNo, e.NoSeries, e.Reserve, e.Validade, e.Budget, e.FaPostingGroup, e.WorkCenterNo, e.MachineCenterNo, e.FinishingTimeHours, e.TipoContactoCliente, e.CustomerDocNo, e.JobPostingGroup, e.ShipToCode, e.ShipToName, e.ShipToName2, e.ShipToAddress, e.ShipToAddress2, e.ShipToPostCode, e.ShipToCity, e.ShipToCounty, e.ShipToContact, e.ShortcutDimension3Code, e.ShortcutDimension4Code, e.DataFecho, e.HoraFecho, e.Loc1, e.EstadoOrcamento, e.NoDocumentoEnviado, e.FormaDeEnvio, e.DataDeEnvio, e.DataEntrada, e.NºGeste, e.DataEntrega, e.DataSaída, e.OrigemOrdem, e.Loc2, e.Loc3, e.Urgência, e.PrioridadeObra, e.FechoTécnicoObra, e.PrazoDeExecuçãoDaOrdem, e.Descrição1, e.Descrição2, e.Descrição3, e.ValorTotalPrev, e.TotalQPrev, e.TotalQReal, e.NºLinhaContrato, e.DataReabertura, e.HoraReabertura, e.NºAntigoAs400, e.ValorFacturado, e.ObjectoManutençãoAs400, e.TotalQuantidadeReal, e.ValorCustoRealTotal, e.ClienteContrato, e.TotalQuantidadeFact, e.TotalValorFact, e.PMargem, e.Margem, e.FTextDescDim1, e.Cc, e.Paginas, e.De, e.Compensa, e.NãoCompensa, e.ObraReclamada, e.NºReclamacao, e.DescricaoReclamacao, e.DataPedidoReparação, e.HoraPedidoReparação, e.FechadoPor, e.ReabertoPor, e.MensagemImpressoOrdem, e.NovaReconv, e.ObjectoServiço, e.DataPedido, e.DataValidade, e.ValidadePedido, e.ValorProjecto, e.DeliberaçãoCa, e.ServInternosRequisições, e.ServInternosFolhasDeObra, e.ServInternosDébInternos, e.MãoDeObraEDeslocações, e.ConfigResponsavel, e.DataUltimoMail, e.DataChefeProjecto, e.DataResponsavel, e.DataFacturação, e.NoCompromisso, e.NoDocumentoContactoInicial, e.TipoContactoClienteInicial, e.IdClienteEvolution, e.IdTecnico1, e.IdTecnico2, e.IdTecnico3, e.IdTecnico4, e.IdTecnico5, e.ReferenciaEncomenda, e.No, e.UserResponsavel, e.UserChefeProjecto, e.IdInstituicaoEvolution, e.IdServicoEvolution, e.TécnicoExecutante })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_K170_K168_K180_K181_K173_1_2_4_5_6_7_8_9_10_11_12_13_14_15_16_17_18_19_20_21_22_");

                entity.HasIndex(e => new { e.Timestamp, e.DocumentType, e.Description, e.ObjectRefType, e.ObjectRefNo, e.ObjectRefDescription, e.ComponentOf, e.OrderType, e.MaintenanceActivity, e.SourceDocType, e.SourceDocNo, e.ContractNo, e.Priority, e.Status, e.SuspendedOrderReason, e.ResponsibilityCenter, e.LastDateModified, e.CustomerNo, e.CustomerName, e.CustomerName2, e.CustomerAddress, e.CustomerAddress2, e.CustomerCity, e.CustomerPostCode, e.CustomerPhoneNo, e.CustomerEMail, e.CustomerShipToCode, e.CustomerFaxNo, e.CustomerReference, e.CustomerContactName, e.CustomerCountryCode, e.PostingDate, e.CustomerCounty, e.JobNo, e.ApplicationMethod, e.LanguageCode, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.RespCenterCountryCode, e.TotalQuantity, e.TotalQtyToInvoice, e.RespCenterName, e.RespCenterName2, e.RespCenterFaxNo, e.RespCenterCounty, e.RespCenterAddress, e.RespCenterAddress2, e.RespCenterPostCode, e.RespCenterCity, e.RespCenterContact, e.RespCenterPhoneNo, e.RespCenterReference, e.FaNo, e.FlNo, e.FlDescription, e.ResponsibleEmployee, e.EnteredBy, e.MaintenanceResponsible, e.PlannerGroupNo, e.OrderDate, e.OrderTime, e.DocumentDate, e.ExpectedFinishingDate, e.ExpectedFinishingTime, e.ExpectedStartingDate, e.ExpectedStartingTime, e.StartingDate, e.StartingTime, e.ResponseTimeHours, e.MaintenanceTimeHours, e.FinishingDate, e.FinishingTime, e.GenBusPostingGroup, e.CustomerPriceGroup, e.CustomerDiscGroup, e.VatRegistrationNo, e.PurchaserCode, e.PlannedOrderNo, e.NoSeries, e.Reserve, e.Validade, e.Budget, e.FaPostingGroup, e.WorkCenterNo, e.MachineCenterNo, e.FinishingTimeHours, e.TipoContactoCliente, e.CustomerDocNo, e.JobPostingGroup, e.ShipToCode, e.ShipToName, e.ShipToName2, e.ShipToAddress, e.ShipToAddress2, e.ShipToPostCode, e.ShipToCity, e.ShipToCounty, e.ShipToContact, e.ShortcutDimension3Code, e.ShortcutDimension4Code, e.DataFecho, e.HoraFecho, e.Loc1, e.EstadoOrcamento, e.NoDocumentoEnviado, e.FormaDeEnvio, e.DataDeEnvio, e.DataEntrada, e.NºGeste, e.DataEntrega, e.DataSaída, e.OrigemOrdem, e.Loc2, e.Loc3, e.Urgência, e.PrioridadeObra, e.FechoTécnicoObra, e.PrazoDeExecuçãoDaOrdem, e.Descrição1, e.Descrição2, e.Descrição3, e.ValorTotalPrev, e.TotalQPrev, e.TotalQReal, e.NºLinhaContrato, e.DataReabertura, e.HoraReabertura, e.NºAntigoAs400, e.ValorFacturado, e.ObjectoManutençãoAs400, e.TotalQuantidadeReal, e.ValorCustoRealTotal, e.ClienteContrato, e.TotalQuantidadeFact, e.TotalValorFact, e.PMargem, e.Margem, e.FTextDescDim1, e.Cc, e.Paginas, e.De, e.Compensa, e.NãoCompensa, e.ObraReclamada, e.NºReclamacao, e.DescricaoReclamacao, e.DataPedidoReparação, e.HoraPedidoReparação, e.FechadoPor, e.ReabertoPor, e.MensagemImpressoOrdem, e.NovaReconv, e.ObjectoServiço, e.DataPedido, e.DataValidade, e.ValidadePedido, e.ValorProjecto, e.DeliberaçãoCa, e.ServInternosRequisições, e.ServInternosFolhasDeObra, e.ServInternosDébInternos, e.MãoDeObraEDeslocações, e.ConfigResponsavel, e.DataUltimoMail, e.DataChefeProjecto, e.DataResponsavel, e.DataFacturação, e.NoCompromisso, e.NoDocumentoContactoInicial, e.TipoContactoClienteInicial, e.IdClienteEvolution, e.IdTecnico1, e.IdTecnico2, e.IdTecnico3, e.IdTecnico4, e.IdTecnico5, e.ReferenciaEncomenda, e.No, e.UserResponsavel, e.UserChefeProjecto, e.IdServicoEvolution, e.IdInstituicaoEvolution, e.TécnicoExecutante })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_K170_K168_K181_K180_K173_1_2_4_5_6_7_8_9_10_11_12_13_14_15_16_17_18_19_20_21_22_");

                entity.HasIndex(e => new { e.Timestamp, e.DocumentType, e.Description, e.ObjectRefType, e.ObjectRefNo, e.ObjectRefDescription, e.ComponentOf, e.OrderType, e.MaintenanceActivity, e.SourceDocType, e.SourceDocNo, e.ContractNo, e.Priority, e.Status, e.SuspendedOrderReason, e.ResponsibilityCenter, e.LastDateModified, e.CustomerNo, e.CustomerName, e.CustomerName2, e.CustomerAddress, e.CustomerAddress2, e.CustomerCity, e.CustomerPostCode, e.CustomerPhoneNo, e.CustomerEMail, e.CustomerShipToCode, e.CustomerFaxNo, e.CustomerReference, e.CustomerContactName, e.CustomerCountryCode, e.PostingDate, e.CustomerCounty, e.JobNo, e.ApplicationMethod, e.LanguageCode, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.RespCenterCountryCode, e.TotalQuantity, e.TotalQtyToInvoice, e.RespCenterName, e.RespCenterName2, e.RespCenterFaxNo, e.RespCenterCounty, e.RespCenterAddress, e.RespCenterAddress2, e.RespCenterPostCode, e.RespCenterCity, e.RespCenterContact, e.RespCenterPhoneNo, e.RespCenterReference, e.FaNo, e.FlNo, e.FlDescription, e.ResponsibleEmployee, e.EnteredBy, e.MaintenanceResponsible, e.PlannerGroupNo, e.OrderDate, e.OrderTime, e.DocumentDate, e.ExpectedFinishingDate, e.ExpectedFinishingTime, e.ExpectedStartingDate, e.ExpectedStartingTime, e.StartingDate, e.StartingTime, e.ResponseTimeHours, e.MaintenanceTimeHours, e.FinishingDate, e.FinishingTime, e.GenBusPostingGroup, e.CustomerPriceGroup, e.CustomerDiscGroup, e.VatRegistrationNo, e.PurchaserCode, e.PlannedOrderNo, e.NoSeries, e.Reserve, e.Validade, e.Budget, e.FaPostingGroup, e.WorkCenterNo, e.MachineCenterNo, e.FinishingTimeHours, e.TipoContactoCliente, e.CustomerDocNo, e.JobPostingGroup, e.ShipToCode, e.ShipToName, e.ShipToName2, e.ShipToAddress, e.ShipToAddress2, e.ShipToPostCode, e.ShipToCity, e.ShipToCounty, e.ShipToContact, e.ShortcutDimension3Code, e.ShortcutDimension4Code, e.DataFecho, e.HoraFecho, e.Loc1, e.EstadoOrcamento, e.NoDocumentoEnviado, e.FormaDeEnvio, e.DataDeEnvio, e.DataEntrada, e.NºGeste, e.DataEntrega, e.DataSaída, e.OrigemOrdem, e.Loc2, e.Loc3, e.Urgência, e.PrioridadeObra, e.FechoTécnicoObra, e.PrazoDeExecuçãoDaOrdem, e.Descrição1, e.Descrição2, e.Descrição3, e.ValorTotalPrev, e.TotalQPrev, e.TotalQReal, e.NºLinhaContrato, e.DataReabertura, e.HoraReabertura, e.NºAntigoAs400, e.ValorFacturado, e.ObjectoManutençãoAs400, e.TotalQuantidadeReal, e.ValorCustoRealTotal, e.ClienteContrato, e.TotalQuantidadeFact, e.TotalValorFact, e.PMargem, e.Margem, e.FTextDescDim1, e.Cc, e.Paginas, e.De, e.Compensa, e.NãoCompensa, e.ObraReclamada, e.NºReclamacao, e.DescricaoReclamacao, e.DataPedidoReparação, e.HoraPedidoReparação, e.FechadoPor, e.ReabertoPor, e.MensagemImpressoOrdem, e.NovaReconv, e.ObjectoServiço, e.DataPedido, e.DataValidade, e.ValidadePedido, e.ValorProjecto, e.DeliberaçãoCa, e.ServInternosRequisições, e.ServInternosFolhasDeObra, e.ServInternosDébInternos, e.MãoDeObraEDeslocações, e.ConfigResponsavel, e.DataUltimoMail, e.DataChefeProjecto, e.DataResponsavel, e.DataFacturação, e.NoCompromisso, e.NoDocumentoContactoInicial, e.TipoContactoClienteInicial, e.IdClienteEvolution, e.IdTecnico1, e.IdTecnico2, e.IdTecnico3, e.IdTecnico4, e.IdTecnico5, e.ReferenciaEncomenda, e.No, e.UserResponsavel, e.UserChefeProjecto, e.TécnicoExecutante, e.IdInstituicaoEvolution, e.IdServicoEvolution })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_K170_K168_K173_K180_K181_1_2_4_5_6_7_8_9_10_11_12_13_14_15_16_17_18_19_20_21_22_");

                entity.HasIndex(e => new { e.Timestamp, e.DocumentType, e.Description, e.ObjectRefType, e.ObjectRefNo, e.ObjectRefDescription, e.ComponentOf, e.OrderType, e.MaintenanceActivity, e.SourceDocType, e.SourceDocNo, e.ContractNo, e.Priority, e.Status, e.SuspendedOrderReason, e.ResponsibilityCenter, e.LastDateModified, e.CustomerNo, e.CustomerName, e.CustomerName2, e.CustomerAddress, e.CustomerAddress2, e.CustomerCity, e.CustomerPostCode, e.CustomerPhoneNo, e.CustomerEMail, e.CustomerShipToCode, e.CustomerFaxNo, e.CustomerReference, e.CustomerContactName, e.CustomerCountryCode, e.PostingDate, e.CustomerCounty, e.JobNo, e.ApplicationMethod, e.LanguageCode, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.RespCenterCountryCode, e.TotalQuantity, e.TotalQtyToInvoice, e.RespCenterName, e.RespCenterName2, e.RespCenterFaxNo, e.RespCenterCounty, e.RespCenterAddress, e.RespCenterAddress2, e.RespCenterPostCode, e.RespCenterCity, e.RespCenterContact, e.RespCenterPhoneNo, e.RespCenterReference, e.FaNo, e.FlNo, e.FlDescription, e.ResponsibleEmployee, e.EnteredBy, e.MaintenanceResponsible, e.PlannerGroupNo, e.OrderDate, e.OrderTime, e.DocumentDate, e.ExpectedFinishingDate, e.ExpectedFinishingTime, e.ExpectedStartingDate, e.ExpectedStartingTime, e.StartingDate, e.StartingTime, e.ResponseTimeHours, e.MaintenanceTimeHours, e.FinishingDate, e.FinishingTime, e.GenBusPostingGroup, e.CustomerPriceGroup, e.CustomerDiscGroup, e.VatRegistrationNo, e.PurchaserCode, e.PlannedOrderNo, e.NoSeries, e.Reserve, e.Validade, e.Budget, e.FaPostingGroup, e.WorkCenterNo, e.MachineCenterNo, e.FinishingTimeHours, e.TipoContactoCliente, e.CustomerDocNo, e.JobPostingGroup, e.ShipToCode, e.ShipToName, e.ShipToName2, e.ShipToAddress, e.ShipToAddress2, e.ShipToPostCode, e.ShipToCity, e.ShipToCounty, e.ShipToContact, e.ShortcutDimension3Code, e.ShortcutDimension4Code, e.DataFecho, e.HoraFecho, e.Loc1, e.EstadoOrcamento, e.NoDocumentoEnviado, e.FormaDeEnvio, e.DataDeEnvio, e.DataEntrada, e.NºGeste, e.DataEntrega, e.DataSaída, e.OrigemOrdem, e.Loc2, e.Loc3, e.Urgência, e.PrioridadeObra, e.FechoTécnicoObra, e.PrazoDeExecuçãoDaOrdem, e.Descrição1, e.Descrição2, e.Descrição3, e.ValorTotalPrev, e.TotalQPrev, e.TotalQReal, e.NºLinhaContrato, e.DataReabertura, e.HoraReabertura, e.NºAntigoAs400, e.ValorFacturado, e.ObjectoManutençãoAs400, e.TotalQuantidadeReal, e.ValorCustoRealTotal, e.ClienteContrato, e.TotalQuantidadeFact, e.TotalValorFact, e.PMargem, e.Margem, e.FTextDescDim1, e.Cc, e.Paginas, e.De, e.Compensa, e.NãoCompensa, e.ObraReclamada, e.NºReclamacao, e.DescricaoReclamacao, e.DataPedidoReparação, e.HoraPedidoReparação, e.FechadoPor, e.ReabertoPor, e.MensagemImpressoOrdem, e.NovaReconv, e.ObjectoServiço, e.DataPedido, e.DataValidade, e.ValidadePedido, e.ValorProjecto, e.DeliberaçãoCa, e.ServInternosRequisições, e.ServInternosFolhasDeObra, e.ServInternosDébInternos, e.MãoDeObraEDeslocações, e.ConfigResponsavel, e.DataUltimoMail, e.DataChefeProjecto, e.DataResponsavel, e.DataFacturação, e.NoCompromisso, e.NoDocumentoContactoInicial, e.TipoContactoClienteInicial, e.IdClienteEvolution, e.IdTecnico1, e.IdTecnico2, e.IdTecnico3, e.IdTecnico4, e.IdTecnico5, e.ReferenciaEncomenda, e.TécnicoExecutante, e.No, e.UserResponsavel, e.UserChefeProjecto, e.IdInstituicaoEvolution, e.IdServicoEvolution })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K173_K3_K170_K168_K180_K181_1_2_4_5_6_7_8_9_10_11_12_13_14_15_16_17_18_19_20_21_22_");

                entity.HasIndex(e => new { e.Timestamp, e.DocumentType, e.Description, e.ObjectRefType, e.ObjectRefNo, e.ObjectRefDescription, e.ComponentOf, e.OrderType, e.MaintenanceActivity, e.SourceDocType, e.SourceDocNo, e.ContractNo, e.Priority, e.Status, e.SuspendedOrderReason, e.ResponsibilityCenter, e.LastDateModified, e.CustomerNo, e.CustomerName, e.CustomerName2, e.CustomerAddress, e.CustomerAddress2, e.CustomerCity, e.CustomerPostCode, e.CustomerPhoneNo, e.CustomerEMail, e.CustomerShipToCode, e.CustomerFaxNo, e.CustomerReference, e.CustomerContactName, e.CustomerCountryCode, e.PostingDate, e.CustomerCounty, e.JobNo, e.ApplicationMethod, e.LanguageCode, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.RespCenterCountryCode, e.TotalQuantity, e.TotalQtyToInvoice, e.RespCenterName, e.RespCenterName2, e.RespCenterFaxNo, e.RespCenterCounty, e.RespCenterAddress, e.RespCenterAddress2, e.RespCenterPostCode, e.RespCenterCity, e.RespCenterContact, e.RespCenterPhoneNo, e.RespCenterReference, e.FaNo, e.FlNo, e.FlDescription, e.ResponsibleEmployee, e.EnteredBy, e.MaintenanceResponsible, e.PlannerGroupNo, e.OrderDate, e.OrderTime, e.DocumentDate, e.ExpectedFinishingDate, e.ExpectedFinishingTime, e.ExpectedStartingDate, e.ExpectedStartingTime, e.StartingDate, e.StartingTime, e.ResponseTimeHours, e.MaintenanceTimeHours, e.FinishingDate, e.FinishingTime, e.GenBusPostingGroup, e.CustomerPriceGroup, e.CustomerDiscGroup, e.VatRegistrationNo, e.PurchaserCode, e.PlannedOrderNo, e.NoSeries, e.Reserve, e.Validade, e.Budget, e.FaPostingGroup, e.WorkCenterNo, e.MachineCenterNo, e.FinishingTimeHours, e.TipoContactoCliente, e.CustomerDocNo, e.JobPostingGroup, e.ShipToCode, e.ShipToName, e.ShipToName2, e.ShipToAddress, e.ShipToAddress2, e.ShipToPostCode, e.ShipToCity, e.ShipToCounty, e.ShipToContact, e.ShortcutDimension3Code, e.ShortcutDimension4Code, e.DataFecho, e.HoraFecho, e.Loc1, e.EstadoOrcamento, e.NoDocumentoEnviado, e.FormaDeEnvio, e.DataDeEnvio, e.DataEntrada, e.NºGeste, e.DataEntrega, e.DataSaída, e.OrigemOrdem, e.Loc2, e.Loc3, e.Urgência, e.PrioridadeObra, e.FechoTécnicoObra, e.PrazoDeExecuçãoDaOrdem, e.Descrição1, e.Descrição2, e.Descrição3, e.ValorTotalPrev, e.TotalQPrev, e.TotalQReal, e.NºLinhaContrato, e.DataReabertura, e.HoraReabertura, e.NºAntigoAs400, e.ValorFacturado, e.ObjectoManutençãoAs400, e.TotalQuantidadeReal, e.ValorCustoRealTotal, e.ClienteContrato, e.TotalQuantidadeFact, e.TotalValorFact, e.PMargem, e.Margem, e.FTextDescDim1, e.Cc, e.Paginas, e.De, e.Compensa, e.NãoCompensa, e.ObraReclamada, e.NºReclamacao, e.DescricaoReclamacao, e.DataPedidoReparação, e.HoraPedidoReparação, e.FechadoPor, e.ReabertoPor, e.MensagemImpressoOrdem, e.NovaReconv, e.ObjectoServiço, e.DataPedido, e.DataValidade, e.ValidadePedido, e.ValorProjecto, e.DeliberaçãoCa, e.ServInternosRequisições, e.ServInternosFolhasDeObra, e.ServInternosDébInternos, e.MãoDeObraEDeslocações, e.ConfigResponsavel, e.DataUltimoMail, e.DataChefeProjecto, e.DataResponsavel, e.DataFacturação, e.NoCompromisso, e.NoDocumentoContactoInicial, e.TipoContactoClienteInicial, e.IdClienteEvolution, e.IdTecnico1, e.IdTecnico2, e.IdTecnico3, e.IdTecnico4, e.IdTecnico5, e.ReferenciaEncomenda, e.UserChefeProjecto, e.UserResponsavel, e.No, e.IdInstituicaoEvolution, e.IdServicoEvolution, e.TécnicoExecutante })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K168_K170_K3_K180_K181_K173_1_2_4_5_6_7_8_9_10_11_12_13_14_15_16_17_18_19_20_21_22_");

                entity.HasIndex(e => new { e.Timestamp, e.DocumentType, e.Description, e.ObjectRefType, e.ObjectRefNo, e.ObjectRefDescription, e.ComponentOf, e.OrderType, e.MaintenanceActivity, e.SourceDocType, e.SourceDocNo, e.ContractNo, e.Priority, e.Status, e.SuspendedOrderReason, e.ResponsibilityCenter, e.LastDateModified, e.CustomerNo, e.CustomerName, e.CustomerName2, e.CustomerAddress, e.CustomerAddress2, e.CustomerCity, e.CustomerPostCode, e.CustomerPhoneNo, e.CustomerEMail, e.CustomerShipToCode, e.CustomerFaxNo, e.CustomerReference, e.CustomerContactName, e.CustomerCountryCode, e.PostingDate, e.CustomerCounty, e.JobNo, e.ApplicationMethod, e.LanguageCode, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.RespCenterCountryCode, e.TotalQuantity, e.TotalQtyToInvoice, e.RespCenterName, e.RespCenterName2, e.RespCenterFaxNo, e.RespCenterCounty, e.RespCenterAddress, e.RespCenterAddress2, e.RespCenterPostCode, e.RespCenterCity, e.RespCenterContact, e.RespCenterPhoneNo, e.RespCenterReference, e.FaNo, e.FlNo, e.FlDescription, e.ResponsibleEmployee, e.EnteredBy, e.MaintenanceResponsible, e.PlannerGroupNo, e.OrderDate, e.OrderTime, e.DocumentDate, e.ExpectedFinishingDate, e.ExpectedFinishingTime, e.ExpectedStartingDate, e.ExpectedStartingTime, e.StartingDate, e.StartingTime, e.ResponseTimeHours, e.MaintenanceTimeHours, e.FinishingDate, e.FinishingTime, e.GenBusPostingGroup, e.CustomerPriceGroup, e.CustomerDiscGroup, e.VatRegistrationNo, e.PurchaserCode, e.PlannedOrderNo, e.NoSeries, e.Reserve, e.Validade, e.Budget, e.FaPostingGroup, e.WorkCenterNo, e.MachineCenterNo, e.FinishingTimeHours, e.TipoContactoCliente, e.CustomerDocNo, e.JobPostingGroup, e.ShipToCode, e.ShipToName, e.ShipToName2, e.ShipToAddress, e.ShipToAddress2, e.ShipToPostCode, e.ShipToCity, e.ShipToCounty, e.ShipToContact, e.ShortcutDimension3Code, e.ShortcutDimension4Code, e.DataFecho, e.HoraFecho, e.Loc1, e.EstadoOrcamento, e.NoDocumentoEnviado, e.FormaDeEnvio, e.DataDeEnvio, e.DataEntrada, e.NºGeste, e.DataEntrega, e.DataSaída, e.OrigemOrdem, e.Loc2, e.Loc3, e.Urgência, e.PrioridadeObra, e.FechoTécnicoObra, e.PrazoDeExecuçãoDaOrdem, e.Descrição1, e.Descrição2, e.Descrição3, e.ValorTotalPrev, e.TotalQPrev, e.TotalQReal, e.NºLinhaContrato, e.DataReabertura, e.HoraReabertura, e.NºAntigoAs400, e.ValorFacturado, e.ObjectoManutençãoAs400, e.TotalQuantidadeReal, e.ValorCustoRealTotal, e.ClienteContrato, e.TotalQuantidadeFact, e.TotalValorFact, e.PMargem, e.Margem, e.FTextDescDim1, e.Cc, e.Paginas, e.De, e.Compensa, e.NãoCompensa, e.ObraReclamada, e.NºReclamacao, e.DescricaoReclamacao, e.DataPedidoReparação, e.HoraPedidoReparação, e.FechadoPor, e.ReabertoPor, e.MensagemImpressoOrdem, e.NovaReconv, e.ObjectoServiço, e.DataPedido, e.DataValidade, e.ValidadePedido, e.ValorProjecto, e.DeliberaçãoCa, e.ServInternosRequisições, e.ServInternosFolhasDeObra, e.ServInternosDébInternos, e.MãoDeObraEDeslocações, e.ConfigResponsavel, e.DataUltimoMail, e.DataChefeProjecto, e.DataResponsavel, e.DataFacturação, e.NoCompromisso, e.NoDocumentoContactoInicial, e.TipoContactoClienteInicial, e.IdClienteEvolution, e.IdTecnico1, e.IdTecnico2, e.IdTecnico3, e.IdTecnico4, e.IdTecnico5, e.ReferenciaEncomenda, e.UserResponsavel, e.UserChefeProjecto, e.No, e.IdInstituicaoEvolution, e.IdServicoEvolution, e.TécnicoExecutante })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K170_K168_K3_K180_K181_K173_1_2_4_5_6_7_8_9_10_11_12_13_14_15_16_17_18_19_20_21_22_");

                entity.HasIndex(e => new { e.Timestamp, e.DocumentType, e.Description, e.ObjectRefType, e.ObjectRefNo, e.ObjectRefDescription, e.ComponentOf, e.OrderType, e.MaintenanceActivity, e.SourceDocType, e.SourceDocNo, e.ContractNo, e.Priority, e.Status, e.SuspendedOrderReason, e.ResponsibilityCenter, e.LastDateModified, e.CustomerNo, e.CustomerName, e.CustomerName2, e.CustomerAddress, e.CustomerAddress2, e.CustomerCity, e.CustomerPostCode, e.CustomerPhoneNo, e.CustomerEMail, e.CustomerShipToCode, e.CustomerFaxNo, e.CustomerReference, e.CustomerContactName, e.CustomerCountryCode, e.PostingDate, e.CustomerCounty, e.JobNo, e.ApplicationMethod, e.LanguageCode, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.RespCenterCountryCode, e.TotalQuantity, e.TotalQtyToInvoice, e.RespCenterName, e.RespCenterName2, e.RespCenterFaxNo, e.RespCenterCounty, e.RespCenterAddress, e.RespCenterAddress2, e.RespCenterPostCode, e.RespCenterCity, e.RespCenterContact, e.RespCenterPhoneNo, e.RespCenterReference, e.FaNo, e.FlNo, e.FlDescription, e.ResponsibleEmployee, e.EnteredBy, e.MaintenanceResponsible, e.PlannerGroupNo, e.OrderDate, e.OrderTime, e.DocumentDate, e.ExpectedFinishingDate, e.ExpectedFinishingTime, e.ExpectedStartingDate, e.ExpectedStartingTime, e.StartingDate, e.StartingTime, e.ResponseTimeHours, e.MaintenanceTimeHours, e.FinishingDate, e.FinishingTime, e.GenBusPostingGroup, e.CustomerPriceGroup, e.CustomerDiscGroup, e.VatRegistrationNo, e.PurchaserCode, e.PlannedOrderNo, e.NoSeries, e.Reserve, e.Validade, e.Budget, e.FaPostingGroup, e.WorkCenterNo, e.MachineCenterNo, e.FinishingTimeHours, e.TipoContactoCliente, e.CustomerDocNo, e.JobPostingGroup, e.ShipToCode, e.ShipToName, e.ShipToName2, e.ShipToAddress, e.ShipToAddress2, e.ShipToPostCode, e.ShipToCity, e.ShipToCounty, e.ShipToContact, e.ShortcutDimension3Code, e.ShortcutDimension4Code, e.DataFecho, e.HoraFecho, e.Loc1, e.EstadoOrcamento, e.NoDocumentoEnviado, e.FormaDeEnvio, e.DataDeEnvio, e.DataEntrada, e.NºGeste, e.DataEntrega, e.DataSaída, e.OrigemOrdem, e.Loc2, e.Loc3, e.Urgência, e.PrioridadeObra, e.FechoTécnicoObra, e.PrazoDeExecuçãoDaOrdem, e.Descrição1, e.Descrição2, e.Descrição3, e.ValorTotalPrev, e.TotalQPrev, e.TotalQReal, e.NºLinhaContrato, e.DataReabertura, e.HoraReabertura, e.NºAntigoAs400, e.ValorFacturado, e.ObjectoManutençãoAs400, e.TotalQuantidadeReal, e.ValorCustoRealTotal, e.ClienteContrato, e.TotalQuantidadeFact, e.TotalValorFact, e.PMargem, e.Margem, e.FTextDescDim1, e.Cc, e.Paginas, e.De, e.Compensa, e.NãoCompensa, e.ObraReclamada, e.NºReclamacao, e.DescricaoReclamacao, e.DataPedidoReparação, e.HoraPedidoReparação, e.FechadoPor, e.ReabertoPor, e.MensagemImpressoOrdem, e.NovaReconv, e.ObjectoServiço, e.DataPedido, e.DataValidade, e.ValidadePedido, e.ValorProjecto, e.DeliberaçãoCa, e.ServInternosRequisições, e.ServInternosFolhasDeObra, e.ServInternosDébInternos, e.MãoDeObraEDeslocações, e.ConfigResponsavel, e.DataUltimoMail, e.DataChefeProjecto, e.DataResponsavel, e.DataFacturação, e.TécnicoExecutante, e.NoCompromisso, e.NoDocumentoContactoInicial, e.TipoContactoClienteInicial, e.IdClienteEvolution, e.IdInstituicaoEvolution, e.IdServicoEvolution, e.IdTecnico1, e.IdTecnico2, e.IdTecnico3, e.IdTecnico4, e.IdTecnico5, e.ReferenciaEncomenda, e.No, e.UserResponsavel, e.UserChefeProjecto })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K3_K170_K168_1_2_4_5_6_7_8_9_10_11_12_13_14_15_16_17_18_19_20_21_22_23_24_25_26_27_");

                entity.HasIndex(e => new { e.Timestamp, e.DocumentType, e.No, e.Description, e.ObjectRefType, e.ObjectRefNo, e.ObjectRefDescription, e.ComponentOf, e.OrderType, e.MaintenanceActivity, e.SourceDocType, e.SourceDocNo, e.ContractNo, e.Priority, e.Status, e.SuspendedOrderReason, e.ResponsibilityCenter, e.LastDateModified, e.CustomerNo, e.CustomerName, e.CustomerName2, e.CustomerAddress, e.CustomerAddress2, e.CustomerCity, e.CustomerPostCode, e.CustomerPhoneNo, e.CustomerEMail, e.CustomerShipToCode, e.CustomerFaxNo, e.CustomerReference, e.CustomerContactName, e.CustomerCountryCode, e.PostingDate, e.CustomerCounty, e.JobNo, e.ApplicationMethod, e.LanguageCode, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.RespCenterCountryCode, e.TotalQuantity, e.TotalQtyToInvoice, e.RespCenterName, e.RespCenterName2, e.RespCenterFaxNo, e.RespCenterCounty, e.RespCenterAddress, e.RespCenterAddress2, e.RespCenterPostCode, e.RespCenterCity, e.RespCenterContact, e.RespCenterPhoneNo, e.RespCenterReference, e.FaNo, e.FlNo, e.FlDescription, e.ResponsibleEmployee, e.EnteredBy, e.MaintenanceResponsible, e.PlannerGroupNo, e.OrderDate, e.OrderTime, e.DocumentDate, e.ExpectedFinishingDate, e.ExpectedFinishingTime, e.ExpectedStartingDate, e.ExpectedStartingTime, e.StartingDate, e.StartingTime, e.ResponseTimeHours, e.MaintenanceTimeHours, e.FinishingDate, e.FinishingTime, e.GenBusPostingGroup, e.CustomerPriceGroup, e.CustomerDiscGroup, e.VatRegistrationNo, e.PurchaserCode, e.PlannedOrderNo, e.NoSeries, e.Reserve, e.Validade, e.Budget, e.FaPostingGroup, e.WorkCenterNo, e.MachineCenterNo, e.FinishingTimeHours, e.TipoContactoCliente, e.CustomerDocNo, e.JobPostingGroup, e.ShipToCode, e.ShipToName, e.ShipToName2, e.ShipToAddress, e.ShipToAddress2, e.ShipToPostCode, e.ShipToCity, e.ShipToCounty, e.ShipToContact, e.ShortcutDimension3Code, e.ShortcutDimension4Code, e.DataFecho, e.HoraFecho, e.Loc1, e.NoDocumentoEnviado, e.FormaDeEnvio, e.DataDeEnvio, e.DataEntrada, e.NºGeste, e.DataEntrega, e.DataSaída, e.OrigemOrdem, e.Loc2, e.Loc3, e.Urgência, e.PrioridadeObra, e.FechoTécnicoObra, e.PrazoDeExecuçãoDaOrdem, e.Descrição1, e.Descrição2, e.Descrição3, e.ValorTotalPrev, e.TotalQPrev, e.TotalQReal, e.NºLinhaContrato, e.DataReabertura, e.HoraReabertura, e.NºAntigoAs400, e.ValorFacturado, e.ObjectoManutençãoAs400, e.TotalQuantidadeReal, e.ValorCustoRealTotal, e.ClienteContrato, e.TotalQuantidadeFact, e.TotalValorFact, e.PMargem, e.Margem, e.FTextDescDim1, e.Cc, e.Paginas, e.De, e.Compensa, e.NãoCompensa, e.ObraReclamada, e.NºReclamacao, e.DescricaoReclamacao, e.DataPedidoReparação, e.HoraPedidoReparação, e.FechadoPor, e.ReabertoPor, e.Dimension2CodeOld, e.MensagemImpressoOrdem, e.NovaReconv, e.ObjectoServiço, e.DataPedido, e.DataValidade, e.ValidadePedido, e.ValorProjecto, e.DeliberaçãoCa, e.ServInternosRequisições, e.ServInternosFolhasDeObra, e.ServInternosDébInternos, e.MãoDeObraEDeslocações, e.ConfigResponsavel, e.DataUltimoMail, e.UserChefeProjecto, e.DataChefeProjecto, e.UserResponsavel, e.DataResponsavel, e.DataFacturação, e.TécnicoExecutante, e.NoCompromisso, e.NoDocumentoContactoInicial, e.TipoContactoClienteInicial, e.LocalAec, e.Contrato, e.IdClienteEvolution, e.IdInstituicaoEvolution, e.IdServicoEvolution, e.IdTecnico1, e.IdTecnico2, e.IdTecnico3, e.IdTecnico4, e.IdTecnico5, e.GeradaAuto, e.ReferenciaEncomenda, e.DataEncomenda, e.NumOrdem, e.EstadoOrcamento })
                    .HasName("_dta_index_Maintenance Order_6_859918185__K106_K105_1_2_3_4_5_6_7_8_9_10_11_12_13_14_15_16_17_18_19_20_21_22_23_24_25_26_27_28_");

                entity.HasIndex(e => new { e.DocumentType, e.No, e.Description, e.ObjectRefType, e.ObjectRefNo, e.ObjectRefDescription, e.ComponentOf, e.OrderType, e.MaintenanceActivity, e.SourceDocType, e.SourceDocNo, e.ContractNo, e.Priority, e.Status, e.SuspendedOrderReason, e.ResponsibilityCenter, e.LastDateModified, e.CustomerNo, e.CustomerName, e.CustomerName2, e.CustomerAddress, e.CustomerAddress2, e.CustomerCity, e.CustomerPostCode, e.CustomerPhoneNo, e.CustomerEMail, e.CustomerShipToCode, e.CustomerFaxNo, e.CustomerReference, e.CustomerContactName, e.CustomerCountryCode, e.PostingDate, e.CustomerCounty, e.JobNo, e.ApplicationMethod, e.LanguageCode, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.RespCenterCountryCode, e.TotalQuantity, e.TotalQtyToInvoice, e.RespCenterName, e.RespCenterName2, e.RespCenterFaxNo, e.RespCenterCounty, e.RespCenterAddress, e.RespCenterAddress2, e.RespCenterPostCode, e.RespCenterCity, e.RespCenterContact, e.RespCenterPhoneNo, e.RespCenterReference, e.FaNo, e.FlNo, e.FlDescription, e.ResponsibleEmployee, e.EnteredBy, e.MaintenanceResponsible, e.PlannerGroupNo, e.OrderDate, e.OrderTime, e.DocumentDate, e.ExpectedFinishingDate, e.ExpectedFinishingTime, e.ExpectedStartingDate, e.ExpectedStartingTime, e.StartingDate, e.StartingTime, e.ResponseTimeHours, e.MaintenanceTimeHours, e.FinishingDate, e.FinishingTime, e.GenBusPostingGroup, e.CustomerPriceGroup, e.CustomerDiscGroup, e.VatRegistrationNo, e.PurchaserCode, e.PlannedOrderNo, e.NoSeries, e.Reserve, e.Validade, e.Budget, e.FaPostingGroup, e.WorkCenterNo, e.MachineCenterNo, e.FinishingTimeHours, e.TipoContactoCliente, e.CustomerDocNo, e.JobPostingGroup, e.ShipToCode, e.ShipToName, e.ShipToName2, e.ShipToAddress, e.ShipToAddress2, e.ShipToPostCode, e.ShipToCity, e.ShipToCounty, e.ShipToContact, e.ShortcutDimension3Code, e.ShortcutDimension4Code, e.DataFecho, e.HoraFecho, e.Loc1, e.EstadoOrcamento, e.NumOrdem, e.NoDocumentoEnviado, e.FormaDeEnvio, e.DataDeEnvio, e.DataEntrada, e.NºGeste, e.DataEntrega, e.DataSaída, e.OrigemOrdem, e.Loc2, e.Loc3, e.Urgência, e.PrioridadeObra, e.FechoTécnicoObra, e.PrazoDeExecuçãoDaOrdem, e.Descrição1, e.Descrição2, e.Descrição3, e.ValorTotalPrev, e.TotalQPrev, e.TotalQReal, e.NºLinhaContrato, e.DataReabertura, e.HoraReabertura, e.NºAntigoAs400, e.ValorFacturado, e.ObjectoManutençãoAs400, e.TotalQuantidadeReal, e.ValorCustoRealTotal, e.ClienteContrato, e.TotalQuantidadeFact, e.TotalValorFact, e.PMargem, e.Margem, e.FTextDescDim1, e.Cc, e.Paginas, e.De, e.Compensa, e.NãoCompensa, e.ObraReclamada, e.NºReclamacao, e.DescricaoReclamacao, e.DataPedidoReparação, e.HoraPedidoReparação, e.FechadoPor, e.ReabertoPor, e.Dimension2CodeOld, e.MensagemImpressoOrdem, e.NovaReconv, e.ObjectoServiço, e.DataPedido, e.DataValidade, e.ValidadePedido, e.ValorProjecto, e.DeliberaçãoCa, e.ServInternosRequisições, e.ServInternosFolhasDeObra, e.ServInternosDébInternos, e.MãoDeObraEDeslocações, e.ConfigResponsavel, e.DataUltimoMail, e.UserChefeProjecto, e.DataChefeProjecto, e.UserResponsavel, e.DataResponsavel, e.DataFacturação, e.TécnicoExecutante, e.NoCompromisso, e.NoDocumentoContactoInicial, e.TipoContactoClienteInicial, e.LocalAec, e.Contrato, e.IdClienteEvolution, e.IdInstituicaoEvolution, e.IdServicoEvolution, e.IdTecnico1, e.IdTecnico2, e.IdTecnico3, e.IdTecnico4, e.IdTecnico5, e.GeradaAuto, e.ReferenciaEncomenda, e.DataEncomenda, e.DataConclusao, e.ConcluidoPor, e.NumFolhaAssociada })
                    .HasName("_dta_index_Maintenance Order_6_859918185__col___8341");

                entity.HasIndex(e => new { e.Timestamp, e.DocumentType, e.No, e.Description, e.ObjectRefType, e.ObjectRefNo, e.ObjectRefDescription, e.ComponentOf, e.MaintenanceActivity, e.SourceDocType, e.SourceDocNo, e.ContractNo, e.Priority, e.SuspendedOrderReason, e.ResponsibilityCenter, e.LastDateModified, e.CustomerNo, e.CustomerName, e.CustomerName2, e.CustomerAddress, e.CustomerAddress2, e.CustomerCity, e.CustomerPostCode, e.CustomerPhoneNo, e.CustomerEMail, e.CustomerShipToCode, e.CustomerFaxNo, e.CustomerReference, e.CustomerContactName, e.CustomerCountryCode, e.PostingDate, e.CustomerCounty, e.JobNo, e.ApplicationMethod, e.LanguageCode, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.RespCenterCountryCode, e.TotalQuantity, e.TotalQtyToInvoice, e.RespCenterName, e.RespCenterName2, e.RespCenterFaxNo, e.RespCenterCounty, e.RespCenterAddress, e.RespCenterAddress2, e.RespCenterPostCode, e.RespCenterCity, e.RespCenterContact, e.RespCenterPhoneNo, e.RespCenterReference, e.FaNo, e.FlNo, e.FlDescription, e.ResponsibleEmployee, e.EnteredBy, e.MaintenanceResponsible, e.PlannerGroupNo, e.OrderDate, e.OrderTime, e.DocumentDate, e.ExpectedFinishingDate, e.ExpectedFinishingTime, e.ExpectedStartingDate, e.ExpectedStartingTime, e.StartingDate, e.StartingTime, e.ResponseTimeHours, e.MaintenanceTimeHours, e.FinishingDate, e.FinishingTime, e.GenBusPostingGroup, e.CustomerPriceGroup, e.CustomerDiscGroup, e.VatRegistrationNo, e.PurchaserCode, e.PlannedOrderNo, e.NoSeries, e.Reserve, e.Validade, e.Budget, e.FaPostingGroup, e.WorkCenterNo, e.MachineCenterNo, e.FinishingTimeHours, e.TipoContactoCliente, e.CustomerDocNo, e.JobPostingGroup, e.ShipToCode, e.ShipToName, e.ShipToName2, e.ShipToAddress, e.ShipToAddress2, e.ShipToPostCode, e.ShipToCity, e.ShipToCounty, e.ShipToContact, e.ShortcutDimension3Code, e.ShortcutDimension4Code, e.DataFecho, e.HoraFecho, e.Loc1, e.EstadoOrcamento, e.NumOrdem, e.NoDocumentoEnviado, e.FormaDeEnvio, e.DataDeEnvio, e.DataEntrada, e.NºGeste, e.DataEntrega, e.DataSaída, e.OrigemOrdem, e.Loc2, e.Loc3, e.Urgência, e.PrioridadeObra, e.FechoTécnicoObra, e.PrazoDeExecuçãoDaOrdem, e.Descrição1, e.Descrição2, e.Descrição3, e.ValorTotalPrev, e.TotalQPrev, e.TotalQReal, e.NºLinhaContrato, e.DataReabertura, e.HoraReabertura, e.NºAntigoAs400, e.ValorFacturado, e.ObjectoManutençãoAs400, e.TotalQuantidadeReal, e.ValorCustoRealTotal, e.ClienteContrato, e.TotalQuantidadeFact, e.TotalValorFact, e.PMargem, e.Margem, e.FTextDescDim1, e.Cc, e.Paginas, e.De, e.Compensa, e.NãoCompensa, e.ObraReclamada, e.NºReclamacao, e.DescricaoReclamacao, e.DataPedidoReparação, e.HoraPedidoReparação, e.FechadoPor, e.ReabertoPor, e.Dimension2CodeOld, e.MensagemImpressoOrdem, e.NovaReconv, e.ObjectoServiço, e.DataPedido, e.DataValidade, e.ValidadePedido, e.ValorProjecto, e.DeliberaçãoCa, e.ServInternosRequisições, e.ServInternosFolhasDeObra, e.ServInternosDébInternos, e.MãoDeObraEDeslocações, e.ConfigResponsavel, e.DataUltimoMail, e.UserChefeProjecto, e.DataChefeProjecto, e.UserResponsavel, e.DataResponsavel, e.DataFacturação, e.TécnicoExecutante, e.NoCompromisso, e.NoDocumentoContactoInicial, e.TipoContactoClienteInicial, e.LocalAec, e.Contrato, e.IdServicoEvolution, e.IdTecnico1, e.IdTecnico2, e.IdTecnico3, e.IdTecnico4, e.IdTecnico5, e.GeradaAuto, e.ReferenciaEncomenda, e.DataEncomenda, e.DataConclusao, e.ConcluidoPor, e.NumFolhaAssociada, e.Status, e.OrderType, e.IdClienteEvolution, e.IdInstituicaoEvolution })
                    .HasName("IX_Maintenance Order_Status_Order Type_ID_Cliente_Evolution_ID_Instituicao_Evolution");

                entity.Property(e => e.DocumentType).HasColumnName("Document Type");

                entity.Property(e => e.No)
                    .HasColumnName("No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ApplicationMethod).HasColumnName("Application Method");

                entity.Property(e => e.Cc)
                    .HasColumnName("CC:")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ClienteContrato)
                    .HasColumnName("Cliente Contrato")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Compensa)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ComponentOf)
                    .HasColumnName("Component Of")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ConcluidoPor)
                    .HasColumnName("Concluido_Por")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ConfigResponsavel)
                    .HasColumnName("Config Responsavel")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ContractNo)
                    .HasColumnName("Contract No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerAddress)
                    .HasColumnName("Customer Address")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerAddress2)
                    .HasColumnName("Customer Address 2")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerCity)
                    .HasColumnName("Customer City")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerContactName)
                    .HasColumnName("Customer Contact Name")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerCountryCode)
                    .HasColumnName("Customer Country Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerCounty)
                    .HasColumnName("Customer County")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerDiscGroup)
                    .HasColumnName("Customer Disc_ Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerDocNo)
                    .HasColumnName("Customer Doc_ No_")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerEMail)
                    .HasColumnName("Customer E-Mail")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerFaxNo)
                    .HasColumnName("Customer Fax No_")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerName)
                    .HasColumnName("Customer Name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerName2)
                    .HasColumnName("Customer Name 2")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerNo)
                    .HasColumnName("Customer No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerPhoneNo)
                    .HasColumnName("Customer Phone No_")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerPostCode)
                    .HasColumnName("Customer Post Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerPriceGroup)
                    .HasColumnName("Customer Price Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerReference)
                    .HasColumnName("Customer Reference")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerShipToCode)
                    .HasColumnName("Customer Ship-to Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DataChefeProjecto)
                    .HasColumnName("Data Chefe Projecto")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataConclusao)
                    .HasColumnName("Data_Conclusao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataDeEnvio)
                    .HasColumnName("Data de Envio")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataEncomenda)
                    .HasColumnName("Data_Encomenda")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataEntrada)
                    .HasColumnName("Data Entrada")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataEntrega)
                    .HasColumnName("Data Entrega")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataFacturação)
                    .HasColumnName("Data Facturação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataFecho)
                    .HasColumnName("Data Fecho")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataPedido)
                    .HasColumnName("Data Pedido")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataPedidoReparação)
                    .HasColumnName("Data Pedido Reparação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataReabertura)
                    .HasColumnName("Data Reabertura")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataResponsavel)
                    .HasColumnName("Data Responsavel")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataSaída)
                    .HasColumnName("Data Saída")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataUltimoMail)
                    .HasColumnName("Data Ultimo Mail")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataValidade)
                    .HasColumnName("Data Validade")
                    .HasColumnType("datetime");

                entity.Property(e => e.De)
                    .HasColumnName("De:")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DeliberaçãoCa)
                    .HasColumnName("Deliberação CA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.DescricaoReclamacao)
                    .HasColumnName("Descricao Reclamacao")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Descrição1)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Descrição2)
                    .HasMaxLength(140)
                    .IsUnicode(false);

                entity.Property(e => e.Descrição3)
                    .HasMaxLength(140)
                    .IsUnicode(false);

                entity.Property(e => e.Dimension2CodeOld)
                    .HasColumnName("Dimension 2 Code (old)")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentDate)
                    .HasColumnName("Document Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.EnteredBy)
                    .HasColumnName("Entered By")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ExpectedFinishingDate)
                    .HasColumnName("Expected Finishing Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ExpectedFinishingTime)
                    .HasColumnName("Expected Finishing Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.ExpectedStartingDate)
                    .HasColumnName("Expected Starting Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ExpectedStartingTime)
                    .HasColumnName("Expected Starting Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.FTextDescDim1)
                    .HasColumnName("F_textDescDim1")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FaNo)
                    .HasColumnName("FA No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FaPostingGroup)
                    .HasColumnName("FA Posting Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FechadoPor)
                    .HasColumnName("Fechado Por")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FechoTécnicoObra).HasColumnName("Fecho Técnico Obra");

                entity.Property(e => e.FinishingDate)
                    .HasColumnName("Finishing Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.FinishingTime)
                    .HasColumnName("Finishing Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.FinishingTimeHours)
                    .HasColumnName("Finishing Time (Hours)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.FlDescription)
                    .HasColumnName("FL Description")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FlNo)
                    .HasColumnName("FL No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FormaDeEnvio)
                    .HasColumnName("Forma de Envio")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.GenBusPostingGroup)
                    .HasColumnName("Gen_ Bus_ Posting Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.GeradaAuto)
                    .HasColumnName("Gerada_Auto")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.HoraFecho)
                    .HasColumnName("Hora Fecho")
                    .HasColumnType("datetime");

                entity.Property(e => e.HoraPedidoReparação)
                    .HasColumnName("Hora Pedido Reparação")
                    .HasColumnType("datetime");

                entity.Property(e => e.HoraReabertura)
                    .HasColumnName("Hora Reabertura")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdClienteEvolution).HasColumnName("ID_Cliente_Evolution");

                entity.Property(e => e.IdInstituicaoEvolution).HasColumnName("ID_Instituicao_Evolution");

                entity.Property(e => e.IdServicoEvolution).HasColumnName("ID_Servico_Evolution");

                entity.Property(e => e.IdTecnico1).HasColumnName("ID_Tecnico_1");

                entity.Property(e => e.IdTecnico2).HasColumnName("ID_Tecnico_2");

                entity.Property(e => e.IdTecnico3).HasColumnName("ID_Tecnico_3");

                entity.Property(e => e.IdTecnico4).HasColumnName("ID_Tecnico_4");

                entity.Property(e => e.IdTecnico5).HasColumnName("ID_Tecnico_5");

                entity.Property(e => e.JobNo)
                    .HasColumnName("Job No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.JobPostingGroup)
                    .HasColumnName("Job Posting Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LanguageCode)
                    .HasColumnName("Language Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LastDateModified)
                    .HasColumnName("Last Date Modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.Loc1)
                    .HasColumnName("loc1")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Loc2)
                    .HasColumnName("loc2")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Loc3)
                    .HasColumnName("loc3")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LocalAec)
                    .HasColumnName("Local AEC")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.MachineCenterNo)
                    .HasColumnName("Machine Center No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MaintenanceActivity)
                    .HasColumnName("Maintenance Activity")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.MaintenanceResponsible)
                    .HasColumnName("Maintenance Responsible")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MaintenanceTimeHours)
                    .HasColumnName("Maintenance Time (Hours)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.Margem).HasColumnType("decimal(38, 20)");

                entity.Property(e => e.MensagemImpressoOrdem).HasColumnName("Mensagem Impresso Ordem");

                entity.Property(e => e.MãoDeObraEDeslocações).HasColumnName("Mão de Obra e Deslocações");

                entity.Property(e => e.NoCompromisso)
                    .HasColumnName("No_ Compromisso")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NoDocumentoContactoInicial)
                    .HasColumnName("No Documento Contacto Inicial")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.NoDocumentoEnviado)
                    .HasColumnName("No Documento Enviado")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.NoSeries)
                    .HasColumnName("No_ Series")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NovaReconv)
                    .HasColumnName("Nova Reconv")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.NumFolhaAssociada)
                    .HasColumnName("Num_Folha_Associada")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumOrdem)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NºAntigoAs400)
                    .HasColumnName("Nº Antigo AS400")
                    .HasMaxLength(14)
                    .IsUnicode(false);

                entity.Property(e => e.NºGeste)
                    .HasColumnName("Nº GESTE")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.NºLinhaContrato).HasColumnName("Nº Linha Contrato");

                entity.Property(e => e.NºReclamacao).HasColumnName("Nº Reclamacao");

                entity.Property(e => e.NãoCompensa)
                    .HasColumnName("Não Compensa")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectRefDescription)
                    .HasColumnName("Object Ref_ Description")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectRefNo)
                    .HasColumnName("Object Ref_ No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectRefType).HasColumnName("Object Ref_ Type");

                entity.Property(e => e.ObjectoManutençãoAs400)
                    .HasColumnName("Objecto Manutenção (AS400)")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectoServiço)
                    .HasColumnName("Objecto Serviço")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ObraReclamada).HasColumnName("Obra Reclamada");

                entity.Property(e => e.OrderDate)
                    .HasColumnName("Order Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.OrderTime)
                    .HasColumnName("Order Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.OrderType)
                    .HasColumnName("Order Type")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.OrigemOrdem).HasColumnName("Origem Ordem");

                entity.Property(e => e.PMargem)
                    .HasColumnName("P_Margem")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.Paginas)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.PlannedOrderNo)
                    .HasColumnName("Planned Order No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PlannerGroupNo)
                    .HasColumnName("Planner Group No_")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PostingDate)
                    .HasColumnName("Posting Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.PrazoDeExecuçãoDaOrdem)
                    .HasColumnName("Prazo de Execução da Ordem")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.PrioridadeObra).HasColumnName("Prioridade Obra");

                entity.Property(e => e.PurchaserCode)
                    .HasColumnName("Purchaser Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ReabertoPor)
                    .HasColumnName("Reaberto Por")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ReferenciaEncomenda)
                    .HasColumnName("Referencia_Encomenda")
                    .HasMaxLength(50);

                entity.Property(e => e.RespCenterAddress)
                    .HasColumnName("Resp_ Center Address")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RespCenterAddress2)
                    .HasColumnName("Resp_ Center Address 2")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RespCenterCity)
                    .HasColumnName("Resp_ Center City")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RespCenterContact)
                    .HasColumnName("Resp_ Center Contact")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.RespCenterCountryCode)
                    .HasColumnName("Resp_ Center Country Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RespCenterCounty)
                    .HasColumnName("Resp_ Center County")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RespCenterFaxNo)
                    .HasColumnName("Resp_ Center Fax No_")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RespCenterName)
                    .HasColumnName("Resp_ Center Name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RespCenterName2)
                    .HasColumnName("Resp_ Center Name 2")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RespCenterPhoneNo)
                    .HasColumnName("Resp_ Center Phone No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RespCenterPostCode)
                    .HasColumnName("Resp_ Center Post Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RespCenterReference)
                    .HasColumnName("Resp_ Center Reference")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ResponseTimeHours)
                    .HasColumnName("Response Time (Hours)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.ResponsibilityCenter)
                    .HasColumnName("Responsibility Center")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ResponsibleEmployee)
                    .HasColumnName("Responsible Employee")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ServInternosDébInternos).HasColumnName("Serv_ Internos -Déb Internos");

                entity.Property(e => e.ServInternosFolhasDeObra).HasColumnName("Serv_ Internos -Folhas de Obra");

                entity.Property(e => e.ServInternosRequisições).HasColumnName("Serv_ Internos -Requisições");

                entity.Property(e => e.ShipToAddress)
                    .HasColumnName("Ship-to Address")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToAddress2)
                    .HasColumnName("Ship-to Address 2")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToCity)
                    .HasColumnName("Ship-to City")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToCode)
                    .HasColumnName("Ship-to Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToContact)
                    .HasColumnName("Ship-to Contact")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToCounty)
                    .HasColumnName("Ship-to County")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToName)
                    .HasColumnName("Ship-to Name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToName2)
                    .HasColumnName("Ship-to Name 2")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToPostCode)
                    .HasColumnName("Ship-to Post Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension1Code)
                    .HasColumnName("Shortcut Dimension 1 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension2Code)
                    .HasColumnName("Shortcut Dimension 2 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension3Code)
                    .HasColumnName("Shortcut Dimension 3 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension4Code)
                    .HasColumnName("Shortcut Dimension 4 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SourceDocNo)
                    .HasColumnName("Source Doc_ No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SourceDocType).HasColumnName("Source Doc_ Type");

                entity.Property(e => e.StartingDate)
                    .HasColumnName("Starting Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.StartingTime)
                    .HasColumnName("Starting Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.SuspendedOrderReason)
                    .HasColumnName("Suspended Order Reason")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.TipoContactoCliente)
                    .HasColumnName("Tipo Contacto Cliente")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TipoContactoClienteInicial)
                    .HasColumnName("Tipo Contacto Cliente Inicial")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TotalQPrev)
                    .HasColumnName("Total-Q_-Prev")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TotalQReal)
                    .HasColumnName("Total-Q_-Real")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TotalQtyToInvoice)
                    .HasColumnName("Total Qty_ to Invoice")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TotalQuantidadeFact)
                    .HasColumnName("Total Quantidade Fact_")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TotalQuantidadeReal)
                    .HasColumnName("Total Quantidade Real")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TotalQuantity)
                    .HasColumnName("Total Quantity")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TotalValorFact)
                    .HasColumnName("Total Valor Fact_")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TécnicoExecutante)
                    .HasColumnName("Técnico Executante")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserChefeProjecto)
                    .HasColumnName("User Chefe Projecto")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserResponsavel)
                    .HasColumnName("User Responsavel")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Validade)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.ValidadePedido)
                    .HasColumnName("Validade Pedido")
                    .HasColumnType("datetime");

                entity.Property(e => e.ValorCustoRealTotal)
                    .HasColumnName("Valor Custo Real Total")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.ValorFacturado)
                    .HasColumnName("Valor Facturado")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.ValorProjecto)
                    .HasColumnName("Valor Projecto")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.ValorTotalPrev)
                    .HasColumnName("Valor Total -Prev")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.VatRegistrationNo)
                    .HasColumnName("VAT Registration No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.WorkCenterNo)
                    .HasColumnName("Work Center No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MaintenanceOrderAnexo>(entity =>
            {
                entity.HasKey(e => e.AnexNo);

                entity.ToTable("Maintenance Order Anexo");

                entity.HasIndex(e => e.MoNo);

                entity.Property(e => e.AnexNo).HasColumnName("Anex No_");

                entity.Property(e => e.Data).HasColumnType("datetime");

                entity.Property(e => e.Extensao)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.Property(e => e.MoNo)
                    .IsRequired()
                    .HasColumnName("MO No_")
                    .HasMaxLength(20);

                entity.Property(e => e.Nome)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MaintenanceOrderClienteIteracao>(entity =>
            {
                entity.HasKey(e => e.IdClienteIteracao);

                entity.ToTable("Maintenance Order Cliente Iteracao");

                entity.Property(e => e.IdClienteIteracao).HasColumnName("ID_Cliente_Iteracao");

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.Property(e => e.NumAnexo).HasColumnName("Num_Anexo");

                entity.Property(e => e.NumCompromisso)
                    .HasColumnName("Num_Compromisso")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NumDocumento)
                    .HasColumnName("Num_Documento")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.NumOm)
                    .IsRequired()
                    .HasColumnName("Num_OM")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Observacao)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.TipoContactoCliente)
                    .HasColumnName("Tipo_Contacto_Cliente")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MaintenanceOrderLine>(entity =>
            {
                entity.HasKey(e => new { e.DocumentType, e.MoNo, e.LineNo })
                    .HasName("Maintenance Order Line$0");

                entity.ToTable("Maintenance Order Line");

                entity.HasIndex(e => e.LineNo)
                    .HasName("_dta_index_Maintenance Order Line_6_1791345446__K4");

                entity.HasIndex(e => e.MoNo);

                entity.HasIndex(e => new { e.LineNo, e.MoNo })
                    .HasName("_dta_stat_1791345446_4_3");

                entity.HasIndex(e => new { e.MoNo, e.IdEquipamento })
                    .HasName("IX_Maintenance Order Line_ID_Equipamento");

                entity.HasIndex(e => new { e.MoNo, e.LineNo })
                    .HasName("_dta_index_Maintenance Order Line_6_1791345446__K3_K4");

                entity.HasIndex(e => new { e.DocumentType, e.MoNo, e.LineNo, e.OrderStatus, e.SortField, e.ObjectRefType, e.ObjectRefNo, e.ObjectType, e.ObjectNo, e.ObjectDescription, e.ObjectDescription2, e.FunctionalLocationNo, e.TaskListNo, e.Priority, e.AdditionalData, e.Warranty, e.BomNo, e.WarrantyDate, e.ComponentOf, e.CriticalLevel, e.ResponseTimeHours, e.MaintenanceTimeHours, e.StartingDate, e.StartingTime, e.FinishingDate, e.FinishingTime, e.NotificationType, e.NotificationNo, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.JobNo, e.LineStatus, e.CustomerNo, e.ResponsibilityCenter, e.PlannerGroupNo, e.ResourceNo, e.PostingDate, e.DocumentDate, e.OrderDate, e.OrderTime, e.OrderType, e.ResourceFilterYesNo, e.FinalState, e.UsedDmmFilterYesNo, e.ShortcutDimension3Code, e.ShortcutDimension4Code, e.LinhaOrçamento, e.InventoryNo, e.EstadoLinhasOrçamento, e.FaultReasonCode, e.IdEquipamento, e.IdEquipEstado, e.IdRotina, e.Tbf, e.IdInstituicao, e.IdServico })
                    .HasName("_dta_index_Maintenance Order Line_6_1791345446__col__");

                entity.Property(e => e.DocumentType).HasColumnName("Document Type");

                entity.Property(e => e.MoNo)
                    .HasColumnName("MO No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LineNo).HasColumnName("Line No_");

                entity.Property(e => e.AdditionalData)
                    .HasColumnName("Additional Data")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.BomNo)
                    .HasColumnName("BOM No_")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ComponentOf)
                    .HasColumnName("Component Of")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CriticalLevel).HasColumnName("Critical Level");

                entity.Property(e => e.CustomerNo)
                    .HasColumnName("Customer No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentDate)
                    .HasColumnName("Document Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.EstadoLinhasOrçamento).HasColumnName("Estado Linhas Orçamento");

                entity.Property(e => e.FaultReasonCode).HasColumnName("Fault Reason Code");

                entity.Property(e => e.FinalState).HasColumnName("Final State");

                entity.Property(e => e.FinishingDate)
                    .HasColumnName("Finishing Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.FinishingTime)
                    .HasColumnName("Finishing Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.FunctionalLocationNo)
                    .HasColumnName("Functional Location No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdEquipEstado).HasColumnName("ID_Equip_Estado");

                entity.Property(e => e.IdEquipamento).HasColumnName("ID_Equipamento");

                entity.Property(e => e.IdInstituicao).HasColumnName("ID_Instituicao");

                entity.Property(e => e.IdRotina).HasColumnName("ID_Rotina");

                entity.Property(e => e.IdServico).HasColumnName("ID_Servico");

                entity.Property(e => e.InventoryNo)
                    .HasColumnName("Inventory No_")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.JobNo)
                    .HasColumnName("Job No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LineStatus).HasColumnName("Line Status");

                entity.Property(e => e.LinhaOrçamento).HasColumnName("Linha Orçamento");

                entity.Property(e => e.MaintenanceTimeHours)
                    .HasColumnName("Maintenance Time (Hours)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.NotificationNo)
                    .HasColumnName("Notification No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NotificationType).HasColumnName("Notification Type");

                entity.Property(e => e.ObjectDescription)
                    .HasColumnName("Object Description")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectDescription2)
                    .HasColumnName("Object Description 2")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectNo)
                    .HasColumnName("Object No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectRefNo)
                    .HasColumnName("Object Ref_ No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectRefType).HasColumnName("Object Ref_ Type");

                entity.Property(e => e.ObjectType).HasColumnName("Object Type");

                entity.Property(e => e.OrderDate)
                    .HasColumnName("Order Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.OrderStatus).HasColumnName("Order Status");

                entity.Property(e => e.OrderTime)
                    .HasColumnName("Order Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.OrderType)
                    .HasColumnName("Order Type")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PlannerGroupNo)
                    .HasColumnName("Planner Group No_")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PostingDate)
                    .HasColumnName("Posting Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ResourceFilterYesNo).HasColumnName("Resource Filter Yes_No");

                entity.Property(e => e.ResourceNo)
                    .HasColumnName("Resource No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ResponseTimeHours)
                    .HasColumnName("Response Time (Hours)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.ResponsibilityCenter)
                    .HasColumnName("Responsibility Center")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension1Code)
                    .HasColumnName("Shortcut Dimension 1 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension2Code)
                    .HasColumnName("Shortcut Dimension 2 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension3Code)
                    .HasColumnName("Shortcut Dimension 3 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension4Code)
                    .HasColumnName("Shortcut Dimension 4 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SortField)
                    .HasColumnName("Sort Field")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.StartingDate)
                    .HasColumnName("Starting Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.StartingTime)
                    .HasColumnName("Starting Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.TaskListNo)
                    .HasColumnName("Task List No_")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Tbf).HasColumnName("TBF");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.UsedDmmFilterYesNo).HasColumnName("Used DMM Filter Yes_No");

                entity.Property(e => e.WarrantyDate)
                    .HasColumnName("Warranty Date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Menus>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AcessoAdministracao).HasColumnName("Acesso_Administracao");

                entity.Property(e => e.AcessoTotal).HasColumnName("Acesso_Total");

                entity.Property(e => e.Emms).HasColumnName("EMMs");

                entity.Property(e => e.ModuloClientes).HasColumnName("Modulo_Clientes");

                entity.Property(e => e.ModuloContratos).HasColumnName("Modulo_Contratos");

                entity.Property(e => e.ModuloDadosEstatisticos).HasColumnName("Modulo_DadosEstatisticos");

                entity.Property(e => e.ModuloEmm).HasColumnName("Modulo_EMM");

                entity.Property(e => e.ModuloFichaEquip).HasColumnName("Modulo_FichaEquip");

                entity.Property(e => e.ModuloFolhaObra).HasColumnName("Modulo_FolhaObra");

                entity.Property(e => e.ModuloFormacoesCompetencias).HasColumnName("Modulo_FormacoesCompetencias");

                entity.Property(e => e.ModuloFornecedores).HasColumnName("Modulo_Fornecedores");

                entity.Property(e => e.ModuloHabilitacoes).HasColumnName("Modulo_Habilitacoes");

                entity.Property(e => e.ModuloInstituicoes).HasColumnName("Modulo_Instituicoes");

                entity.Property(e => e.ModuloPlaneamento).HasColumnName("Modulo_Planeamento");

                entity.Property(e => e.ModuloRegistoDiario).HasColumnName("Modulo_RegistoDiario");

                entity.Property(e => e.ModuloReplicarPlaneamento).HasColumnName("Modulo_Replicar_Planeamento");

                entity.Property(e => e.ModuloRequisicoes).HasColumnName("Modulo_Requisicoes");

                entity.Property(e => e.ModuloServicos).HasColumnName("Modulo_Servicos");

                entity.Property(e => e.ModuloSolicitacoes).HasColumnName("Modulo_Solicitacoes");

                entity.Property(e => e.ModuloUtilizadores).HasColumnName("Modulo_Utilizadores");

                entity.Property(e => e.NivelAcesso).HasColumnName("Nivel_Acesso");

                entity.Property(e => e.Text).HasMaxLength(100);

                entity.Property(e => e.ValidadorEquipamento).HasColumnName("Validador_Equipamento");
            });

            modelBuilder.Entity<MetCalibracao>(entity =>
            {
                entity.HasKey(e => e.IdMetCalibracao);

                entity.ToTable("MET_Calibracao");

                entity.Property(e => e.IdMetCalibracao).HasColumnName("ID_MET_Calibracao");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Conformidade).HasMaxLength(50);

                entity.Property(e => e.CriterioAceitacao)
                    .HasColumnName("Criterio_Aceitacao")
                    .HasMaxLength(100);

                entity.Property(e => e.DataCalibracao)
                    .HasColumnName("Data_Calibracao")
                    .HasColumnType("date");

                entity.Property(e => e.DataExecucao)
                    .HasColumnName("Data_Execucao")
                    .HasColumnType("date");

                entity.Property(e => e.IdMetEquipamento).HasColumnName("ID_MET_Equipamento");

                entity.Property(e => e.NumCertificado)
                    .HasColumnName("Num_Certificado")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MetCertificado>(entity =>
            {
                entity.HasKey(e => e.IdMetFicheiro);

                entity.ToTable("MET_Certificado");

                entity.Property(e => e.IdMetFicheiro).HasColumnName("ID_MET_Ficheiro");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ContentType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Data).HasColumnType("datetime");

                entity.Property(e => e.Ficheiro).IsRequired();

                entity.Property(e => e.IdMetCalibracao).HasColumnName("ID_MET_Calibracao");

                entity.Property(e => e.IdMetEquipamento).HasColumnName("ID_MET_Equipamento");

                entity.Property(e => e.Nome).HasMaxLength(255);
            });

            modelBuilder.Entity<MetEquipamento>(entity =>
            {
                entity.HasKey(e => e.IdMetEquipamento);

                entity.ToTable("MET_Equipamento");

                entity.Property(e => e.IdMetEquipamento).HasColumnName("ID_MET_Equipamento");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Conformidade).HasMaxLength(50);

                entity.Property(e => e.CriterioAceitacao)
                    .HasColumnName("Criterio_Aceitacao")
                    .HasMaxLength(100);

                entity.Property(e => e.DataEfectivaCalibracao)
                    .HasColumnName("Data_Efectiva_Calibracao")
                    .HasColumnType("date");

                entity.Property(e => e.DataProximaCalibracao)
                    .HasColumnName("Data_Proxima_Calibracao")
                    .HasColumnType("date");

                entity.Property(e => e.DataUltimaCalibracao)
                    .HasColumnName("Data_Ultima_Calibracao")
                    .HasColumnType("date");

                entity.Property(e => e.IdCategoria).HasColumnName("ID_Categoria");

                entity.Property(e => e.IdCliente).HasColumnName("ID_Cliente");

                entity.Property(e => e.IdInstituicao).HasColumnName("ID_Instituicao");

                entity.Property(e => e.IdMarca).HasColumnName("ID_Marca");

                entity.Property(e => e.IdModelo).HasColumnName("ID_Modelo");

                entity.Property(e => e.IdServico).HasColumnName("ID_Servico");

                entity.Property(e => e.NumCertificado)
                    .HasColumnName("Num_Certificado")
                    .HasMaxLength(50);

                entity.Property(e => e.NumInventario)
                    .IsRequired()
                    .HasColumnName("Num_Inventario")
                    .HasMaxLength(50);

                entity.Property(e => e.NumSerie)
                    .IsRequired()
                    .HasColumnName("Num_Serie")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MoCommentLine>(entity =>
            {
                entity.HasKey(e => new { e.Table, e.Type, e.HeaderNo, e.LineNo, e.CommentNo });

                entity.ToTable("MO Comment Line");

                entity.HasIndex(e => e.HeaderNo);

                entity.HasIndex(e => new { e.Timestamp, e.Date, e.Comment, e.TableSubType, e.Code, e.UserId, e.HeaderNo, e.OrcAlternativo, e.Table, e.Type, e.LineNo, e.CommentNo })
                    .HasName("_dta_index_MO Comment Line_6_1663397045__K4_K12_K2_K3_K5_K6_1_7_8_9_10_11");

                entity.Property(e => e.Table).HasDefaultValueSql("((5))");

                entity.Property(e => e.Type).HasDefaultValueSql("((6))");

                entity.Property(e => e.HeaderNo)
                    .HasColumnName("Header No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LineNo).HasColumnName("Line No_");

                entity.Property(e => e.CommentNo)
                    .HasColumnName("Comment No_")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnType("varchar(max)");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.OrcAlternativo).HasColumnName("ORC_Alternativo");

                entity.Property(e => e.TableSubType).HasColumnName("Table Sub-Type");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("User ID")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MoComponents>(entity =>
            {
                entity.HasKey(e => new { e.DocumentType, e.MoNo, e.MoLineNo, e.LineNo })
                    .HasName("MO Components$0");

                entity.ToTable("MO Components");

                entity.HasIndex(e => e.MoNo)
                    .HasName("_dta_index_MO Components_6_1904777893__K3_3928");

                entity.HasIndex(e => new { e.MoNo, e.OrcAlternativo });

                entity.HasIndex(e => new { e.TotalPrice, e.MoNo })
                    .HasName("_dta_index_MO Components_6_1904777893__K3_24_9429");

                entity.HasIndex(e => new { e.Type, e.Description, e.Description2, e.Quantity, e.TotalPrice, e.MoNo })
                    .HasName("_dta_index_MO Components_6_1904777893__K3_6_8_13_14_24");

                entity.HasIndex(e => new { e.Type, e.No, e.Description, e.Description2, e.Quantity, e.TotalPrice, e.MoNo })
                    .HasName("_dta_index_MO Components_6_1904777893__K3_6_7_8_13_14_24");

                entity.HasIndex(e => new { e.Type, e.Description, e.Description2, e.Quantity, e.TotalPrice, e.ShortcutDimension1Code, e.ShortcutDimension3Code, e.MoNo })
                    .HasName("_dta_index_MO Components_6_1904777893__K3_6_8_13_14_24_42_62");

                entity.HasIndex(e => new { e.Type, e.No, e.Description, e.Description2, e.Quantity, e.TotalPrice, e.ShortcutDimension1Code, e.ShortcutDimension3Code, e.MoNo })
                    .HasName("_dta_index_MO Components_6_1904777893__K3_6_7_8_13_14_24_42_62");

                entity.HasIndex(e => new { e.DocumentType, e.MoNo, e.MoLineNo, e.LineNo, e.Type, e.No, e.Description, e.VariantCode, e.CreatedFromNonstockItem, e.BinCode, e.LocationCode, e.Description2, e.Quantity, e.QuantityBase, e.QtyToInvoice, e.QtyToInvoiceBase, e.UnitOfMeasureCode, e.OutstandingQtyBase, e.UnitCost, e.CostAmount, e.QtyPerUnitOfMeasure, e.UnitPrice, e.TotalPrice, e.Profit, e.CustomerNo, e.CustomerPriceGroup, e.GenProductPostingGroup, e.PostingGroup, e.Chargeable, e.ObjectType, e.ObjectNo, e.ObjectDescription, e.BomNo, e.TaskListNo, e.TaskNo, e.JobNo, e.ContractNo, e.GenBusPostingGroup, e.OrderDate, e.OrderTime, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.Class, e.ProductGroupCode, e.ItemCategoryCode, e.TransactionType, e.TransportMethod, e.CountryCode, e.EntryExitPoint, e.Area, e.TransactionSpecification, e.TaskListLinkCode, e.Date, e.ReservedQtyBase, e.ReservedQuantity, e.Reserve, e.ObjectRefType, e.ObjectRefNo, e.RequestedQty, e.OutstandingQty, e.ShortcutDimension3Code, e.ShortcutDimension4Code, e.NºOrçamentoAs400, e.Estado, e.TaxaAprovisionamento, e.OrcAlternativo })
                    .HasName("_dta_index_MO Components_6_1904777893__col___9910");

                entity.HasIndex(e => new { e.Timestamp, e.Type, e.No, e.Description, e.VariantCode, e.CreatedFromNonstockItem, e.BinCode, e.LocationCode, e.Description2, e.Quantity, e.QuantityBase, e.QtyToInvoice, e.QtyToInvoiceBase, e.UnitOfMeasureCode, e.OutstandingQtyBase, e.UnitCost, e.CostAmount, e.QtyPerUnitOfMeasure, e.UnitPrice, e.TotalPrice, e.Profit, e.CustomerNo, e.CustomerPriceGroup, e.GenProductPostingGroup, e.PostingGroup, e.Chargeable, e.ObjectType, e.ObjectNo, e.ObjectDescription, e.BomNo, e.TaskListNo, e.TaskNo, e.JobNo, e.ContractNo, e.GenBusPostingGroup, e.OrderDate, e.OrderTime, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.Class, e.ProductGroupCode, e.ItemCategoryCode, e.TransactionType, e.TransportMethod, e.CountryCode, e.EntryExitPoint, e.Area, e.TransactionSpecification, e.TaskListLinkCode, e.Date, e.ReservedQtyBase, e.ReservedQuantity, e.Reserve, e.ObjectRefType, e.ObjectRefNo, e.RequestedQty, e.OutstandingQty, e.ShortcutDimension3Code, e.ShortcutDimension4Code, e.NºOrçamentoAs400, e.Estado, e.TaxaAprovisionamento, e.OrcAlternativo, e.MoNo, e.DocumentType, e.MoLineNo, e.LineNo })
                    .HasName("_dta_index_MO Components_6_1904777893__K3_K2_K4_K5_1_6_7_8_9_10_11_12_13_14_15_16_17_18_19_20_21_22_23_24_25_26_27_28_29_3_4364");

                entity.Property(e => e.DocumentType).HasColumnName("Document Type");

                entity.Property(e => e.MoNo)
                    .HasColumnName("MO No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MoLineNo).HasColumnName("MO Line No_");

                entity.Property(e => e.LineNo).HasColumnName("Line No_");

                entity.Property(e => e.Area)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.BinCode)
                    .IsRequired()
                    .HasColumnName("Bin Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BomNo)
                    .IsRequired()
                    .HasColumnName("BOM No_")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Class)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ContractNo)
                    .IsRequired()
                    .HasColumnName("Contract No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CostAmount)
                    .HasColumnName("Cost Amount")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasColumnName("Country Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedFromNonstockItem).HasColumnName("Created From Nonstock Item");

                entity.Property(e => e.CustomerNo)
                    .IsRequired()
                    .HasColumnName("Customer No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerPriceGroup)
                    .IsRequired()
                    .HasColumnName("Customer Price Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description2)
                    .IsRequired()
                    .HasColumnName("Description 2")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EntryExitPoint)
                    .IsRequired()
                    .HasColumnName("Entry_Exit Point")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.GenBusPostingGroup)
                    .IsRequired()
                    .HasColumnName("Gen_ Bus_ Posting Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.GenProductPostingGroup)
                    .IsRequired()
                    .HasColumnName("Gen_ Product Posting Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ItemCategoryCode)
                    .IsRequired()
                    .HasColumnName("Item Category Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.JobNo)
                    .IsRequired()
                    .HasColumnName("Job No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LocationCode)
                    .IsRequired()
                    .HasColumnName("Location Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.No)
                    .IsRequired()
                    .HasColumnName("No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NºOrçamentoAs400).HasColumnName("Nº Orçamento AS400");

                entity.Property(e => e.ObjectDescription)
                    .IsRequired()
                    .HasColumnName("Object Description")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectNo)
                    .IsRequired()
                    .HasColumnName("Object No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectRefNo)
                    .IsRequired()
                    .HasColumnName("Object Ref_ No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectRefType).HasColumnName("Object Ref_ Type");

                entity.Property(e => e.ObjectType).HasColumnName("Object Type");

                entity.Property(e => e.OrcAlternativo)
                    .HasColumnName("ORC_Alternativo")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.OrderDate)
                    .HasColumnName("Order Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.OrderTime)
                    .HasColumnName("Order Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.OutstandingQty)
                    .HasColumnName("Outstanding Qty")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.OutstandingQtyBase)
                    .HasColumnName("Outstanding Qty_ (Base)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.PostingGroup)
                    .IsRequired()
                    .HasColumnName("Posting Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ProductGroupCode)
                    .IsRequired()
                    .HasColumnName("Product Group Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Profit)
                    .HasColumnName("Profit %")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.QtyPerUnitOfMeasure)
                    .HasColumnName("Qty_ per Unit of Measure")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.QtyToInvoice)
                    .HasColumnName("Qty_ to Invoice")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.QtyToInvoiceBase)
                    .HasColumnName("Qty_ to Invoice (Base)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.Quantity).HasColumnType("decimal(38, 20)");

                entity.Property(e => e.QuantityBase)
                    .HasColumnName("Quantity (Base)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.RequestedQty)
                    .HasColumnName("Requested Qty")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.ReservedQtyBase)
                    .HasColumnName("Reserved Qty_ (Base)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.ReservedQuantity)
                    .HasColumnName("Reserved Quantity")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.ShortcutDimension1Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 1 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension2Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 2 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension3Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 3 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension4Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 4 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TaskListLinkCode)
                    .IsRequired()
                    .HasColumnName("Task List Link Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TaskListNo)
                    .IsRequired()
                    .HasColumnName("Task List No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TaskNo).HasColumnName("Task No_");

                entity.Property(e => e.TaxaAprovisionamento)
                    .HasColumnName("Taxa Aprovisionamento")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.TotalPrice)
                    .HasColumnName("Total Price")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TransactionSpecification)
                    .IsRequired()
                    .HasColumnName("Transaction Specification")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionType)
                    .IsRequired()
                    .HasColumnName("Transaction Type")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TransportMethod)
                    .IsRequired()
                    .HasColumnName("Transport Method")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UnitCost)
                    .HasColumnName("Unit Cost")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.UnitOfMeasureCode)
                    .IsRequired()
                    .HasColumnName("Unit of Measure Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UnitPrice)
                    .HasColumnName("Unit Price")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.VariantCode)
                    .IsRequired()
                    .HasColumnName("Variant Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MoTasks>(entity =>
            {
                entity.HasKey(e => new { e.DocumentType, e.MoNo, e.MoLineNo, e.LineNo })
                    .HasName("MO Tasks$0");

                entity.ToTable("MO Tasks");

                entity.HasIndex(e => e.MoNo)
                    .HasName("_dta_index_MO Tasks_6_1872777779__K3_8809");

                entity.HasIndex(e => new { e.MoNo, e.OrcAlternativo });

                entity.HasIndex(e => new { e.TotalPrice, e.MoNo })
                    .HasName("_dta_index_MO Tasks_6_1872777779__K3_21_6980");

                entity.HasIndex(e => new { e.DocumentType, e.MoLineNo, e.LineNo, e.TotalPrice, e.MoNo })
                    .HasName("_dta_index_MO Tasks_6_1872777779__K3_2_4_5_21");

                entity.HasIndex(e => new { e.TotalPrice, e.MoNo, e.DocumentType, e.MoLineNo, e.LineNo })
                    .HasName("_dta_index_MO Tasks_6_1872777779__K3_K2_K4_K5_21");

                entity.HasIndex(e => new { e.DocumentType, e.MoNo, e.MoLineNo, e.LineNo, e.TaskListNo, e.No, e.ResourceNo, e.Description, e.ProcessTime, e.ProcessTimeUnitOfMeasure, e.Description2, e.ConcurrentCapacities, e.QtyPerUnitOfMeasure, e.Duration, e.DurationUnitOfMeasure, e.UnitCostPer, e.CostAmount, e.DirectUnitCost, e.UnitPrice, e.TotalPrice, e.Profit, e.LocationCode, e.OperationCondition, e.MaintenanceActivity, e.SkillCode, e.TaskListLinkCode, e.GenProductPostingGroup, e.GenBusPostingGroup, e.Chargeable, e.InitialDate, e.EndDate, e.ContractNo, e.ObjectType, e.ObjectNo, e.ObjectDescription, e.CustomerNo, e.JobNo, e.OrderDate, e.OrderTime, e.ResourceGroupNo, e.WorkTypeCode, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.Comment, e.ObjectRefType, e.ObjectRefNo, e.QuantityBase, e.QtyToInvoice, e.QtyToInvoiceBase, e.OutstandingQtyBase, e.TransactionType, e.Area, e.TransactionSpecification, e.ShortcutDimension3Code, e.ShortcutDimension4Code, e.TipoRecurso, e.NºOrçamentoAs4000, e.Estado, e.OrcAlternativo })
                    .HasName("_dta_index_MO Tasks_6_1872777779__col___4864");

                entity.Property(e => e.DocumentType).HasColumnName("Document Type");

                entity.Property(e => e.MoNo)
                    .HasColumnName("MO No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MoLineNo).HasColumnName("MO Line No_");

                entity.Property(e => e.LineNo).HasColumnName("Line No_");

                entity.Property(e => e.Area)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ConcurrentCapacities)
                    .HasColumnName("Concurrent Capacities")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.ContractNo)
                    .IsRequired()
                    .HasColumnName("Contract No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CostAmount)
                    .HasColumnName("Cost Amount")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.CustomerNo)
                    .IsRequired()
                    .HasColumnName("Customer No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description2)
                    .IsRequired()
                    .HasColumnName("Description 2")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DirectUnitCost)
                    .HasColumnName("Direct Unit Cost")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.Duration).HasColumnType("decimal(38, 20)");

                entity.Property(e => e.DurationUnitOfMeasure)
                    .IsRequired()
                    .HasColumnName("Duration Unit of Measure")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.EndDate)
                    .HasColumnName("End Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.GenBusPostingGroup)
                    .IsRequired()
                    .HasColumnName("Gen_ Bus_ Posting Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.GenProductPostingGroup)
                    .IsRequired()
                    .HasColumnName("Gen_ Product Posting Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.InitialDate)
                    .HasColumnName("Initial Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.JobNo)
                    .IsRequired()
                    .HasColumnName("Job No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LocationCode)
                    .IsRequired()
                    .HasColumnName("Location Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.MaintenanceActivity)
                    .IsRequired()
                    .HasColumnName("Maintenance Activity")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.No).HasColumnName("No_");

                entity.Property(e => e.NºOrçamentoAs4000).HasColumnName("Nº orçamento AS4000");

                entity.Property(e => e.ObjectDescription)
                    .IsRequired()
                    .HasColumnName("Object Description")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectNo)
                    .IsRequired()
                    .HasColumnName("Object No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectRefNo)
                    .IsRequired()
                    .HasColumnName("Object Ref_ No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectRefType).HasColumnName("Object Ref_ Type");

                entity.Property(e => e.ObjectType).HasColumnName("Object Type");

                entity.Property(e => e.OperationCondition)
                    .IsRequired()
                    .HasColumnName("Operation Condition")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.OrcAlternativo)
                    .HasColumnName("ORC_Alternativo")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.OrderDate)
                    .HasColumnName("Order Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.OrderTime)
                    .HasColumnName("Order Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.OutstandingQtyBase)
                    .HasColumnName("Outstanding Qty_ (Base)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.ProcessTime)
                    .HasColumnName("Process Time")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.ProcessTimeUnitOfMeasure)
                    .IsRequired()
                    .HasColumnName("Process Time Unit of Measure")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Profit)
                    .HasColumnName("Profit %")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.QtyPerUnitOfMeasure)
                    .HasColumnName("Qty_ per Unit of Measure")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.QtyToInvoice)
                    .HasColumnName("Qty_ to Invoice")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.QtyToInvoiceBase)
                    .HasColumnName("Qty_ to Invoice (Base)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.QuantityBase)
                    .HasColumnName("Quantity (Base)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.ResourceGroupNo)
                    .IsRequired()
                    .HasColumnName("Resource Group No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ResourceNo)
                    .IsRequired()
                    .HasColumnName("Resource No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension1Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 1 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension2Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 2 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension3Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 3 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension4Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 4 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SkillCode)
                    .IsRequired()
                    .HasColumnName("Skill Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TaskListLinkCode)
                    .IsRequired()
                    .HasColumnName("Task List Link Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TaskListNo)
                    .IsRequired()
                    .HasColumnName("Task List No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.TipoRecurso).HasColumnName("Tipo Recurso");

                entity.Property(e => e.TotalPrice)
                    .HasColumnName("Total Price")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TransactionSpecification)
                    .IsRequired()
                    .HasColumnName("Transaction Specification")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionType)
                    .IsRequired()
                    .HasColumnName("Transaction Type")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UnitCostPer)
                    .HasColumnName("Unit Cost per")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.UnitPrice)
                    .HasColumnName("Unit Price")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.WorkTypeCode)
                    .IsRequired()
                    .HasColumnName("Work Type Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Modelos>(entity =>
            {
                entity.HasKey(e => e.IdModelos);

                entity.HasIndex(e => new { e.Activo, e.Nome });

                entity.Property(e => e.IdModelos).HasColumnName("ID_Modelos");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MovProjectoAutorizadosFact>(entity =>
            {
                entity.HasKey(e => e.EntryNo)
                    .HasName("Mov Projecto Autorizados Fact$0");

                entity.ToTable("Mov Projecto Autorizados Fact");

                entity.HasIndex(e => e.BAjmo)
                    .HasName("_dta_index_Mov Projecto Autorizados Fact_6_1968778121__K120");

                entity.HasIndex(e => new { e.TotalPrice, e.JobNo, e.GrupoFactura })
                    .HasName("IX_Mov Projecto Autorizados Fact_Job No__Grupo Factura");

                entity.Property(e => e.EntryNo)
                    .HasColumnName("Entry No_")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddCurrAmtPostedToGL)
                    .HasColumnName("Add_-Curr_ Amt_ Posted to G_L")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.AmtPostedToGL)
                    .HasColumnName("Amt_ Posted to G_L")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.AmtRecognized)
                    .HasColumnName("Amt_ Recognized")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.AmtToPostToGL)
                    .HasColumnName("Amt_ to Post to G_L")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.AmtToRecognize)
                    .HasColumnName("Amt_ to Recognize")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.AppliesToId)
                    .IsRequired()
                    .HasColumnName("Applies-to ID")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Area)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.BAjmo).HasColumnName("B_Ajmo");

                entity.Property(e => e.BOrçamento).HasColumnName("B_orçamento");

                entity.Property(e => e.CentralIncineração)
                    .IsRequired()
                    .HasColumnName("Central Incineração")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ChaveOrcamento).HasColumnName("Chave Orcamento");

                entity.Property(e => e.Classe)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ClosedAtDate)
                    .HasColumnName("Closed at Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ClosedByAmount)
                    .HasColumnName("Closed by Amount")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.ClosedByEntryNo).HasColumnName("Closed by Entry No_");

                entity.Property(e => e.CodServCliente)
                    .IsRequired()
                    .HasColumnName("Cod_Serv_Cliente")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoOrcamento)
                    .IsRequired()
                    .HasColumnName("Codigo Orcamento")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ContractNo)
                    .IsRequired()
                    .HasColumnName("Contract No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasColumnName("Country Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerPriceGroup)
                    .IsRequired()
                    .HasColumnName("Customer Price Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CódCategoriaProd)
                    .IsRequired()
                    .HasColumnName("Cód_Categoria Prod_")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CódGrupoProd)
                    .IsRequired()
                    .HasColumnName("Cód_Grupo Prod_")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DataAutorizacaoFacturacao)
                    .HasColumnName("Data Autorizacao Facturacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataConsumo)
                    .HasColumnName("Data Consumo")
                    .HasColumnType("datetime");

                entity.Property(e => e.DesServCliente)
                    .IsRequired()
                    .HasColumnName("Des_Serv_Cliente")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.DescontoVenda)
                    .HasColumnName("% Desconto Venda")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description2)
                    .IsRequired()
                    .HasColumnName("Description 2")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DiaDaSemana).HasColumnName("Dia da semana");

                entity.Property(e => e.DirectUnitCost)
                    .HasColumnName("Direct Unit Cost")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.DocumentDate)
                    .HasColumnName("Document Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DocumentNo)
                    .IsRequired()
                    .HasColumnName("Document No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.EntryType).HasColumnName("Entry Type");

                entity.Property(e => e.EnviadoTr).HasColumnName("Enviado TR");

                entity.Property(e => e.ExternalDocumentNo)
                    .IsRequired()
                    .HasColumnName("External Document No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FacturaANºCliente)
                    .IsRequired()
                    .HasColumnName("Factura-a Nº Cliente")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FacturaCriada).HasColumnName("Factura Criada");

                entity.Property(e => e.FacturacaoAutorizada).HasColumnName("Facturacao Autorizada");

                entity.Property(e => e.FlDescription)
                    .IsRequired()
                    .HasColumnName("FL Description")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FlNo)
                    .IsRequired()
                    .HasColumnName("FL No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.GenBusPostingGroup)
                    .IsRequired()
                    .HasColumnName("Gen_ Bus_ Posting Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.GenProdPostingGroup)
                    .IsRequired()
                    .HasColumnName("Gen_ Prod_ Posting Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.GlobalDimension1Code)
                    .IsRequired()
                    .HasColumnName("Global Dimension 1 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.GlobalDimension2Code)
                    .IsRequired()
                    .HasColumnName("Global Dimension 2 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.GlobalDimension3Code)
                    .IsRequired()
                    .HasColumnName("Global Dimension 3 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.GrupoFactura).HasColumnName("Grupo Factura");

                entity.Property(e => e.GrupoServiço)
                    .IsRequired()
                    .HasColumnName("Grupo Serviço")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.HoraDeRegisto)
                    .HasColumnName("Hora de Registo")
                    .HasColumnType("datetime");

                entity.Property(e => e.JobNo)
                    .IsRequired()
                    .HasColumnName("Job No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.JobPostingGroup)
                    .IsRequired()
                    .HasColumnName("Job Posting Group")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.JobTaskNo)
                    .IsRequired()
                    .HasColumnName("Job Task No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.JournalBatchName)
                    .IsRequired()
                    .HasColumnName("Journal Batch Name")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LedgerEntryNo).HasColumnName("Ledger Entry No_");

                entity.Property(e => e.LedgerEntryType).HasColumnName("Ledger Entry Type");

                entity.Property(e => e.LinhaAutFac).HasColumnName("Linha Aut_ Fac_");

                entity.Property(e => e.LinhaOrdemManutenção)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LocalRecolha)
                    .IsRequired()
                    .HasColumnName("Local recolha")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LocationCode)
                    .IsRequired()
                    .HasColumnName("Location Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.MaintEntryNo).HasColumnName("Maint_ Entry No_");

                entity.Property(e => e.Matricula)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.MoComponentLineNo).HasColumnName("MO Component Line No_");

                entity.Property(e => e.MoCostLineNo).HasColumnName("MO Cost Line No_");

                entity.Property(e => e.MoLineNo).HasColumnName("MO Line No_");

                entity.Property(e => e.MoNo)
                    .IsRequired()
                    .HasColumnName("MO No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MoTaskLineNo).HasColumnName("MO Task Line No_");

                entity.Property(e => e.MoToolLineNo).HasColumnName("MO Tool Line No_");

                entity.Property(e => e.Motorista)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.No)
                    .IsRequired()
                    .HasColumnName("No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NoSeries)
                    .IsRequired()
                    .HasColumnName("No_ Series")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NºCartãoUtente)
                    .IsRequired()
                    .HasColumnName("Nº Cartão Utente")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.NºFuncionario)
                    .IsRequired()
                    .HasColumnName("Nº Funcionario")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NºGuiaExterna)
                    .IsRequired()
                    .HasColumnName("Nº Guia Externa")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.NºGuiaResíduosGar)
                    .IsRequired()
                    .HasColumnName("Nº Guia Resíduos (GAR)")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.NºLinhaFolha).HasColumnName("Nº Linha Folha");

                entity.Property(e => e.NºLinhaOm).HasColumnName("Nº Linha OM");

                entity.Property(e => e.NºOrdemAs4001).HasColumnName("Nº Ordem AS 400-1");

                entity.Property(e => e.ObjectDescription)
                    .IsRequired()
                    .HasColumnName("Object Description")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectNo)
                    .IsRequired()
                    .HasColumnName("Object No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectRefNo)
                    .IsRequired()
                    .HasColumnName("Object Ref_ No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectRefType).HasColumnName("Object Ref_ Type");

                entity.Property(e => e.ObjectType).HasColumnName("Object Type");

                entity.Property(e => e.PesagemCliente)
                    .HasColumnName("Pesagem Cliente")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.PhaseCode)
                    .IsRequired()
                    .HasColumnName("Phase Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PostingDate)
                    .HasColumnName("Posting Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.QtyPerUnitOfMeasure)
                    .HasColumnName("Qty_ per Unit of Measure")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.Quantity).HasColumnType("decimal(38, 20)");

                entity.Property(e => e.ReasonCode)
                    .IsRequired()
                    .HasColumnName("Reason Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RegistadoMc).HasColumnName("Registado%MC");

                entity.Property(e => e.RelatedToBudget).HasColumnName("Related to Budget");

                entity.Property(e => e.RemainingAmount)
                    .HasColumnName("Remaining Amount")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.RequisitionLineNo).HasColumnName("Requisition Line No_");

                entity.Property(e => e.RequisitionNo)
                    .IsRequired()
                    .HasColumnName("Requisition No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RequisiçãoInterna)
                    .IsRequired()
                    .HasColumnName("Requisição Interna")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ResourceGroupNo)
                    .IsRequired()
                    .HasColumnName("Resource Group No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceOrderNo)
                    .IsRequired()
                    .HasColumnName("Service Order No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShipmentMethodCode)
                    .IsRequired()
                    .HasColumnName("Shipment Method Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SourceCode)
                    .IsRequired()
                    .HasColumnName("Source Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.StepCode)
                    .IsRequired()
                    .HasColumnName("Step Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Subcontratação)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.TaskCode)
                    .IsRequired()
                    .HasColumnName("Task Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.TipoManutencao).HasColumnName("Tipo Manutencao");

                entity.Property(e => e.TipoProjecto).HasColumnName("Tipo Projecto");

                entity.Property(e => e.TipoRecurso).HasColumnName("Tipo Recurso");

                entity.Property(e => e.TipoRefeição)
                    .IsRequired()
                    .HasColumnName("Tipo Refeição")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Tipologia)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Total1).HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TotalCost)
                    .HasColumnName("Total Cost")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TotalEquipamento)
                    .HasColumnName("Total Equipamento")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TotalPrice)
                    .HasColumnName("Total Price")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TransactionSpecification)
                    .IsRequired()
                    .HasColumnName("Transaction Specification")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionType)
                    .IsRequired()
                    .HasColumnName("Transaction Type")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TransportMethod)
                    .IsRequired()
                    .HasColumnName("Transport Method")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UnitCost)
                    .HasColumnName("Unit Cost")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.UnitOfMeasureCode)
                    .IsRequired()
                    .HasColumnName("Unit of Measure Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UnitPrice)
                    .HasColumnName("Unit Price")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("User ID")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Utilizador)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.UtilizadorIdAutorizacao)
                    .IsRequired()
                    .HasColumnName("Utilizador Id Autorizacao")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ValorDescQuantidade)
                    .HasColumnName("Valor Desc_ Quantidade")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.WorkTypeCode)
                    .IsRequired()
                    .HasColumnName("Work Type Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<NivelAcessoTipo>(entity =>
            {
                entity.ToTable("Nivel_Acesso_Tipo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.NivelAcesso)
                    .HasColumnName("Nivel_Acesso")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<NoSeriesLine>(entity =>
            {
                entity.HasKey(e => new { e.SeriesCode, e.LineNo })
                    .HasName("SUCH-Produtivo$No_ Series Line$0");

                entity.ToTable("No_ Series Line");

                entity.Property(e => e.SeriesCode)
                    .HasColumnName("Series Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LineNo).HasColumnName("Line No_");

                entity.Property(e => e.ConfigControlo)
                    .IsRequired()
                    .HasColumnName("Config_Controlo")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.DataFacturação)
                    .HasColumnName("Data Facturação")
                    .HasColumnType("datetime");

                entity.Property(e => e.EndingNo)
                    .IsRequired()
                    .HasColumnName("Ending No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IncrementByNo).HasColumnName("Increment-by No_");

                entity.Property(e => e.LastDateUsed)
                    .HasColumnName("Last Date Used")
                    .HasColumnType("datetime");

                entity.Property(e => e.LastHashUsed)
                    .IsRequired()
                    .HasColumnName("Last Hash Used")
                    .HasMaxLength(172)
                    .IsUnicode(false);

                entity.Property(e => e.LastNoPosted)
                    .IsRequired()
                    .HasColumnName("Last No_ Posted")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LastNoUsed)
                    .IsRequired()
                    .HasColumnName("Last No_ Used")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PreviousLastDateUsed)
                    .HasColumnName("Previous Last Date Used")
                    .HasColumnType("datetime");

                entity.Property(e => e.StartingDate)
                    .HasColumnName("Starting Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.StartingNo)
                    .IsRequired()
                    .HasColumnName("Starting No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.WarningNo)
                    .IsRequired()
                    .HasColumnName("Warning No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Orcamento>(entity =>
            {
                entity.HasKey(e => new { e.DocumentType, e.No, e.OrcAlternativo })
                    .HasName("Orcamento$0");

                entity.HasIndex(e => e.No)
                    .HasName("_dta_index_Orcamento_6_1243919553__K3_6355");

                entity.HasIndex(e => e.NumOrdem)
                    .HasName("_dta_index_Orcamento_6_1243919553__K106_1771");

                entity.HasIndex(e => new { e.No, e.NumOrdem })
                    .HasName("_dta_index_Orcamento_6_1243919553__K3_K106_5543");

                entity.HasIndex(e => new { e.NumOrdem, e.No })
                    .HasName("_dta_index_Orcamento_6_1243919553__K106_K3_9429");

                entity.HasIndex(e => new { e.NumOrdem, e.Status });

                entity.HasIndex(e => new { e.Status, e.NumOrdem, e.No })
                    .HasName("_dta_index_Orcamento_6_1243919553__K106_K3_15");

                entity.HasIndex(e => new { e.CustomerNo, e.CustomerName, e.CustomerName2, e.EstadoOrcamento, e.No })
                    .HasName("_dta_index_Orcamento_6_1243919553__K3_19_20_21_105");

                entity.HasIndex(e => new { e.CustomerNo, e.CustomerName, e.CustomerName2, e.ShortcutDimension1Code, e.ShortcutDimension3Code, e.EstadoOrcamento, e.No })
                    .HasName("_dta_index_Orcamento_6_1243919553__K3_19_20_21_38_100_105");

                entity.HasIndex(e => new { e.Description, e.CustomerNo, e.CustomerName, e.CustomerName2, e.ShortcutDimension1Code, e.ShortcutDimension3Code, e.EstadoOrcamento, e.No })
                    .HasName("_dta_index_Orcamento_6_1243919553__K3_4_19_20_21_38_100_105");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.SourceDocNo, e.ContractNo, e.ResponsibilityCenter, e.IdInstituicaoEvolution, e.IdServicoEvolution, e.ReferenciaEncomenda, e.EstadoOrcamento, e.NumOrdem, e.NoCompromisso, e.NoDocumentoContactoInicial, e.TipoContactoClienteInicial, e.IdClienteEvolution, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.VatRegistrationNo, e.TipoContactoCliente, e.CustomerDocNo, e.ShortcutDimension3Code, e.No })
                    .HasName("IX_Orcamento_No_01");

                entity.HasIndex(e => new { e.Description, e.OrderType, e.SourceDocNo, e.ContractNo, e.ResponsibilityCenter, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.OrderDate, e.OrderTime, e.VatRegistrationNo, e.TipoContactoCliente, e.CustomerDocNo, e.ShortcutDimension3Code, e.EstadoOrcamento, e.NumOrdem, e.NoCompromisso, e.NoDocumentoContactoInicial, e.TipoContactoClienteInicial, e.IdClienteEvolution, e.IdInstituicaoEvolution, e.IdServicoEvolution, e.ReferenciaEncomenda, e.No })
                    .HasName("IX_Orcamento_No_");

                entity.HasIndex(e => new { e.DocumentType, e.No, e.Description, e.ObjectRefType, e.ObjectRefNo, e.ObjectRefDescription, e.ComponentOf, e.OrderType, e.MaintenanceActivity, e.SourceDocType, e.SourceDocNo, e.ContractNo, e.Priority, e.Status, e.SuspendedOrderReason, e.ResponsibilityCenter, e.LastDateModified, e.CustomerNo, e.CustomerName, e.CustomerName2, e.CustomerAddress, e.CustomerAddress2, e.CustomerCity, e.CustomerPostCode, e.CustomerPhoneNo, e.CustomerEMail, e.CustomerShipToCode, e.CustomerFaxNo, e.CustomerReference, e.CustomerContactName, e.CustomerCountryCode, e.PostingDate, e.CustomerCounty, e.JobNo, e.ApplicationMethod, e.LanguageCode, e.ShortcutDimension1Code, e.ShortcutDimension2Code, e.RespCenterCountryCode, e.TotalQuantity, e.TotalQtyToInvoice, e.RespCenterName, e.RespCenterName2, e.RespCenterFaxNo, e.RespCenterCounty, e.RespCenterAddress, e.RespCenterAddress2, e.RespCenterPostCode, e.RespCenterCity, e.RespCenterContact, e.RespCenterPhoneNo, e.RespCenterReference, e.FaNo, e.FlNo, e.FlDescription, e.ResponsibleEmployee, e.EnteredBy, e.MaintenanceResponsible, e.PlannerGroupNo, e.OrderDate, e.OrderTime, e.DocumentDate, e.ExpectedFinishingDate, e.ExpectedFinishingTime, e.ExpectedStartingDate, e.ExpectedStartingTime, e.StartingDate, e.StartingTime, e.ResponseTimeHours, e.MaintenanceTimeHours, e.FinishingDate, e.FinishingTime, e.GenBusPostingGroup, e.CustomerPriceGroup, e.CustomerDiscGroup, e.VatRegistrationNo, e.PurchaserCode, e.PlannedOrderNo, e.NoSeries, e.Reserve, e.Validade, e.Budget, e.FaPostingGroup, e.WorkCenterNo, e.MachineCenterNo, e.FinishingTimeHours, e.TipoContactoCliente, e.CustomerDocNo, e.JobPostingGroup, e.ShipToCode, e.ShipToName, e.ShipToName2, e.ShipToAddress, e.ShipToAddress2, e.ShipToPostCode, e.ShipToCity, e.ShipToCounty, e.ShipToContact, e.ShortcutDimension3Code, e.ShortcutDimension4Code, e.DataFecho, e.HoraFecho, e.Loc1, e.EstadoOrcamento, e.NumOrdem, e.NoDocumentoEnviado, e.FormaDeEnvio, e.DataDeEnvio, e.DataEntrada, e.NºGeste, e.DataEntrega, e.DataSaída, e.OrigemOrdem, e.Loc2, e.Loc3, e.Urgência, e.PrioridadeObra, e.FechoTécnicoObra, e.PrazoDeExecuçãoDaOrdem, e.Descrição1, e.Descrição2, e.Descrição3, e.ValorTotalPrev, e.TotalQPrev, e.TotalQReal, e.NºLinhaContrato, e.DataReabertura, e.HoraReabertura, e.NºAntigoAs400, e.ValorFacturado, e.ObjectoManutençãoAs400, e.TotalQuantidadeReal, e.ValorCustoRealTotal, e.ClienteContrato, e.TotalQuantidadeFact, e.TotalValorFact, e.PMargem, e.Margem, e.FTextDescDim1, e.Cc, e.Paginas, e.De, e.Compensa, e.NãoCompensa, e.ObraReclamada, e.NºReclamacao, e.DescricaoReclamacao, e.DataPedidoReparação, e.HoraPedidoReparação, e.FechadoPor, e.ReabertoPor, e.Dimension2CodeOld, e.MensagemImpressoOrdem, e.NovaReconv, e.ObjectoServiço, e.DataPedido, e.DataValidade, e.ValidadePedido, e.ValorProjecto, e.DeliberaçãoCa, e.ServInternosRequisições, e.ServInternosFolhasDeObra, e.ServInternosDébInternos, e.MãoDeObraEDeslocações, e.ConfigResponsavel, e.DataUltimoMail, e.UserChefeProjecto, e.DataChefeProjecto, e.UserResponsavel, e.DataResponsavel, e.DataFacturação, e.TécnicoExecutante, e.NoCompromisso, e.NoDocumentoContactoInicial, e.TipoContactoClienteInicial, e.LocalAec, e.Contrato, e.IdClienteEvolution, e.IdInstituicaoEvolution, e.IdServicoEvolution, e.IdTecnico1, e.IdTecnico2, e.IdTecnico3, e.IdTecnico4, e.IdTecnico5, e.OrcAlternativo, e.ReferenciaEncomenda })
                    .HasName("_dta_index_Orcamento_6_1243919553__col___1912");

                entity.Property(e => e.DocumentType).HasColumnName("Document Type");

                entity.Property(e => e.No)
                    .HasColumnName("No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.OrcAlternativo).HasColumnName("ORC_Alternativo");

                entity.Property(e => e.ApplicationMethod).HasColumnName("Application Method");

                entity.Property(e => e.Cc)
                    .HasColumnName("CC:")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.ClienteContrato)
                    .HasColumnName("Cliente Contrato")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Compensa)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ComponentOf)
                    .HasColumnName("Component Of")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ConfigResponsavel)
                    .HasColumnName("Config Responsavel")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ContractNo)
                    .HasColumnName("Contract No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerAddress)
                    .HasColumnName("Customer Address")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerAddress2)
                    .HasColumnName("Customer Address 2")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerCity)
                    .HasColumnName("Customer City")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerContactName)
                    .HasColumnName("Customer Contact Name")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerCountryCode)
                    .HasColumnName("Customer Country Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerCounty)
                    .HasColumnName("Customer County")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerDiscGroup)
                    .HasColumnName("Customer Disc_ Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerDocNo)
                    .HasColumnName("Customer Doc_ No_")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerEMail)
                    .HasColumnName("Customer E-Mail")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerFaxNo)
                    .HasColumnName("Customer Fax No_")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerName)
                    .HasColumnName("Customer Name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerName2)
                    .HasColumnName("Customer Name 2")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerNo)
                    .HasColumnName("Customer No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerPhoneNo)
                    .HasColumnName("Customer Phone No_")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerPostCode)
                    .HasColumnName("Customer Post Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerPriceGroup)
                    .HasColumnName("Customer Price Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerReference)
                    .HasColumnName("Customer Reference")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerShipToCode)
                    .HasColumnName("Customer Ship-to Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DataChefeProjecto)
                    .HasColumnName("Data Chefe Projecto")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataDeEnvio)
                    .HasColumnName("Data de Envio")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataEntrada)
                    .HasColumnName("Data Entrada")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataEntrega)
                    .HasColumnName("Data Entrega")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataFacturação)
                    .HasColumnName("Data Facturação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataFecho)
                    .HasColumnName("Data Fecho")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataPedido)
                    .HasColumnName("Data Pedido")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataPedidoReparação)
                    .HasColumnName("Data Pedido Reparação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataReabertura)
                    .HasColumnName("Data Reabertura")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataResponsavel)
                    .HasColumnName("Data Responsavel")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataSaída)
                    .HasColumnName("Data Saída")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataUltimoMail)
                    .HasColumnName("Data Ultimo Mail")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataValidade)
                    .HasColumnName("Data Validade")
                    .HasColumnType("datetime");

                entity.Property(e => e.De)
                    .HasColumnName("De:")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.DeliberaçãoCa)
                    .HasColumnName("Deliberação CA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.DescricaoReclamacao)
                    .HasColumnName("Descricao Reclamacao")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Descrição1)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Descrição2)
                    .HasMaxLength(140)
                    .IsUnicode(false);

                entity.Property(e => e.Descrição3)
                    .HasMaxLength(140)
                    .IsUnicode(false);

                entity.Property(e => e.Dimension2CodeOld)
                    .HasColumnName("Dimension 2 Code (old)")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentDate)
                    .HasColumnName("Document Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.EnteredBy)
                    .HasColumnName("Entered By")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ExpectedFinishingDate)
                    .HasColumnName("Expected Finishing Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ExpectedFinishingTime)
                    .HasColumnName("Expected Finishing Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.ExpectedStartingDate)
                    .HasColumnName("Expected Starting Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ExpectedStartingTime)
                    .HasColumnName("Expected Starting Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.FTextDescDim1)
                    .HasColumnName("F_textDescDim1")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FaNo)
                    .HasColumnName("FA No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FaPostingGroup)
                    .HasColumnName("FA Posting Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FechadoPor)
                    .HasColumnName("Fechado Por")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FechoTécnicoObra).HasColumnName("Fecho Técnico Obra");

                entity.Property(e => e.FinishingDate)
                    .HasColumnName("Finishing Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.FinishingTime)
                    .HasColumnName("Finishing Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.FinishingTimeHours)
                    .HasColumnName("Finishing Time (Hours)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.FlDescription)
                    .HasColumnName("FL Description")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FlNo)
                    .HasColumnName("FL No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FormaDeEnvio)
                    .HasColumnName("Forma de Envio")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.GenBusPostingGroup)
                    .HasColumnName("Gen_ Bus_ Posting Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.HoraFecho)
                    .HasColumnName("Hora Fecho")
                    .HasColumnType("datetime");

                entity.Property(e => e.HoraPedidoReparação)
                    .HasColumnName("Hora Pedido Reparação")
                    .HasColumnType("datetime");

                entity.Property(e => e.HoraReabertura)
                    .HasColumnName("Hora Reabertura")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdClienteEvolution).HasColumnName("ID_Cliente_Evolution");

                entity.Property(e => e.IdInstituicaoEvolution).HasColumnName("ID_Instituicao_Evolution");

                entity.Property(e => e.IdServicoEvolution).HasColumnName("ID_Servico_Evolution");

                entity.Property(e => e.IdTecnico1).HasColumnName("ID_Tecnico_1");

                entity.Property(e => e.IdTecnico2).HasColumnName("ID_Tecnico_2");

                entity.Property(e => e.IdTecnico3).HasColumnName("ID_Tecnico_3");

                entity.Property(e => e.IdTecnico4).HasColumnName("ID_Tecnico_4");

                entity.Property(e => e.IdTecnico5).HasColumnName("ID_Tecnico_5");

                entity.Property(e => e.JobNo)
                    .HasColumnName("Job No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.JobPostingGroup)
                    .HasColumnName("Job Posting Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LanguageCode)
                    .HasColumnName("Language Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LastDateModified)
                    .HasColumnName("Last Date Modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.Loc1)
                    .HasColumnName("loc1")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Loc2)
                    .HasColumnName("loc2")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Loc3)
                    .HasColumnName("loc3")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LocalAec)
                    .HasColumnName("Local AEC")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.MachineCenterNo)
                    .HasColumnName("Machine Center No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MaintenanceActivity)
                    .HasColumnName("Maintenance Activity")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.MaintenanceResponsible)
                    .HasColumnName("Maintenance Responsible")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MaintenanceTimeHours)
                    .HasColumnName("Maintenance Time (Hours)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.Margem).HasColumnType("decimal(38, 20)");

                entity.Property(e => e.MensagemImpressoOrdem).HasColumnName("Mensagem Impresso Ordem");

                entity.Property(e => e.MãoDeObraEDeslocações).HasColumnName("Mão de Obra e Deslocações");

                entity.Property(e => e.NoCompromisso)
                    .HasColumnName("No_ Compromisso")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NoDocumentoContactoInicial)
                    .HasColumnName("No Documento Contacto Inicial")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.NoDocumentoEnviado)
                    .HasColumnName("No Documento Enviado")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.NoSeries)
                    .HasColumnName("No_ Series")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NovaReconv)
                    .HasColumnName("Nova Reconv")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.NumOrdem)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NºAntigoAs400)
                    .HasColumnName("Nº Antigo AS400")
                    .HasMaxLength(14)
                    .IsUnicode(false);

                entity.Property(e => e.NºGeste)
                    .HasColumnName("Nº GESTE")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.NºLinhaContrato).HasColumnName("Nº Linha Contrato");

                entity.Property(e => e.NºReclamacao).HasColumnName("Nº Reclamacao");

                entity.Property(e => e.NãoCompensa)
                    .HasColumnName("Não Compensa")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectRefDescription)
                    .HasColumnName("Object Ref_ Description")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectRefNo)
                    .HasColumnName("Object Ref_ No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectRefType).HasColumnName("Object Ref_ Type");

                entity.Property(e => e.ObjectoManutençãoAs400)
                    .HasColumnName("Objecto Manutenção (AS400)")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectoServiço)
                    .HasColumnName("Objecto Serviço")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ObraReclamada).HasColumnName("Obra Reclamada");

                entity.Property(e => e.OrderDate)
                    .HasColumnName("Order Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.OrderTime)
                    .HasColumnName("Order Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.OrderType)
                    .HasColumnName("Order Type")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.OrigemOrdem).HasColumnName("Origem Ordem");

                entity.Property(e => e.PMargem)
                    .HasColumnName("P_Margem")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.Paginas)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.PlannedOrderNo)
                    .HasColumnName("Planned Order No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PlannerGroupNo)
                    .HasColumnName("Planner Group No_")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PostingDate)
                    .HasColumnName("Posting Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.PrazoDeExecuçãoDaOrdem)
                    .HasColumnName("Prazo de Execução da Ordem")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.PrioridadeObra).HasColumnName("Prioridade Obra");

                entity.Property(e => e.PurchaserCode)
                    .HasColumnName("Purchaser Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ReabertoPor)
                    .HasColumnName("Reaberto Por")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ReferenciaEncomenda)
                    .HasColumnName("Referencia_Encomenda")
                    .HasMaxLength(50);

                entity.Property(e => e.RespCenterAddress)
                    .HasColumnName("Resp_ Center Address")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RespCenterAddress2)
                    .HasColumnName("Resp_ Center Address 2")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RespCenterCity)
                    .HasColumnName("Resp_ Center City")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RespCenterContact)
                    .HasColumnName("Resp_ Center Contact")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.RespCenterCountryCode)
                    .HasColumnName("Resp_ Center Country Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RespCenterCounty)
                    .HasColumnName("Resp_ Center County")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RespCenterFaxNo)
                    .HasColumnName("Resp_ Center Fax No_")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RespCenterName)
                    .HasColumnName("Resp_ Center Name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RespCenterName2)
                    .HasColumnName("Resp_ Center Name 2")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RespCenterPhoneNo)
                    .HasColumnName("Resp_ Center Phone No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RespCenterPostCode)
                    .HasColumnName("Resp_ Center Post Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RespCenterReference)
                    .HasColumnName("Resp_ Center Reference")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ResponseTimeHours)
                    .HasColumnName("Response Time (Hours)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.ResponsibilityCenter)
                    .HasColumnName("Responsibility Center")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ResponsibleEmployee)
                    .HasColumnName("Responsible Employee")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ServInternosDébInternos).HasColumnName("Serv_ Internos -Déb Internos");

                entity.Property(e => e.ServInternosFolhasDeObra).HasColumnName("Serv_ Internos -Folhas de Obra");

                entity.Property(e => e.ServInternosRequisições).HasColumnName("Serv_ Internos -Requisições");

                entity.Property(e => e.ShipToAddress)
                    .HasColumnName("Ship-to Address")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToAddress2)
                    .HasColumnName("Ship-to Address 2")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToCity)
                    .HasColumnName("Ship-to City")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToCode)
                    .HasColumnName("Ship-to Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToContact)
                    .HasColumnName("Ship-to Contact")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToCounty)
                    .HasColumnName("Ship-to County")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToName)
                    .HasColumnName("Ship-to Name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToName2)
                    .HasColumnName("Ship-to Name 2")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToPostCode)
                    .HasColumnName("Ship-to Post Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension1Code)
                    .HasColumnName("Shortcut Dimension 1 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension2Code)
                    .HasColumnName("Shortcut Dimension 2 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension3Code)
                    .HasColumnName("Shortcut Dimension 3 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension4Code)
                    .HasColumnName("Shortcut Dimension 4 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SourceDocNo)
                    .HasColumnName("Source Doc_ No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SourceDocType).HasColumnName("Source Doc_ Type");

                entity.Property(e => e.StartingDate)
                    .HasColumnName("Starting Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.StartingTime)
                    .HasColumnName("Starting Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.SuspendedOrderReason)
                    .HasColumnName("Suspended Order Reason")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.TipoContactoCliente)
                    .HasColumnName("Tipo Contacto Cliente")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TipoContactoClienteInicial)
                    .HasColumnName("Tipo Contacto Cliente Inicial")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TotalQPrev)
                    .HasColumnName("Total-Q_-Prev")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TotalQReal)
                    .HasColumnName("Total-Q_-Real")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TotalQtyToInvoice)
                    .HasColumnName("Total Qty_ to Invoice")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TotalQuantidadeFact)
                    .HasColumnName("Total Quantidade Fact_")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TotalQuantidadeReal)
                    .HasColumnName("Total Quantidade Real")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TotalQuantity)
                    .HasColumnName("Total Quantity")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TotalValorFact)
                    .HasColumnName("Total Valor Fact_")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TécnicoExecutante)
                    .HasColumnName("Técnico Executante")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserChefeProjecto)
                    .HasColumnName("User Chefe Projecto")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserResponsavel)
                    .HasColumnName("User Responsavel")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Validade)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.ValidadePedido)
                    .HasColumnName("Validade Pedido")
                    .HasColumnType("datetime");

                entity.Property(e => e.ValorCustoRealTotal)
                    .HasColumnName("Valor Custo Real Total")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.ValorFacturado)
                    .HasColumnName("Valor Facturado")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.ValorProjecto)
                    .HasColumnName("Valor Projecto")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.ValorTotalPrev)
                    .HasColumnName("Valor Total -Prev")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.VatRegistrationNo)
                    .HasColumnName("VAT Registration No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.WorkCenterNo)
                    .HasColumnName("Work Center No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OrcamentoLine>(entity =>
            {
                entity.HasKey(e => new { e.DocumentType, e.MoNo, e.LineNo, e.OrcAlternativo })
                    .HasName("Orcamento Line$0");

                entity.ToTable("Orcamento Line");

                entity.HasIndex(e => e.IdEquipamento)
                    .HasName("_dta_index_Orcamento Line_6_502344904__K55");

                entity.HasIndex(e => new { e.MoNo, e.LineNo, e.OrcAlternativo });

                entity.Property(e => e.DocumentType).HasColumnName("Document Type");

                entity.Property(e => e.MoNo)
                    .HasColumnName("MO No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LineNo).HasColumnName("Line No_");

                entity.Property(e => e.OrcAlternativo).HasColumnName("Orc_Alternativo");

                entity.Property(e => e.AdditionalData)
                    .HasColumnName("Additional Data")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.BomNo)
                    .HasColumnName("BOM No_")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ComponentOf)
                    .HasColumnName("Component Of")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CriticalLevel).HasColumnName("Critical Level");

                entity.Property(e => e.CustomerNo)
                    .HasColumnName("Customer No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentDate)
                    .HasColumnName("Document Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.EstadoLinhasOrçamento).HasColumnName("Estado Linhas Orçamento");

                entity.Property(e => e.FaultReasonCode).HasColumnName("Fault Reason Code");

                entity.Property(e => e.FinalState).HasColumnName("Final State");

                entity.Property(e => e.FinishingDate)
                    .HasColumnName("Finishing Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.FinishingTime)
                    .HasColumnName("Finishing Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.FunctionalLocationNo)
                    .HasColumnName("Functional Location No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdCategoriaEquipamento).HasColumnName("ID_Categoria_Equipamento");

                entity.Property(e => e.IdEquipamento).HasColumnName("ID_Equipamento");

                entity.Property(e => e.IdMarcaEquipamento).HasColumnName("ID_Marca_Equipamento");

                entity.Property(e => e.IdModeloEquipamento).HasColumnName("ID_Modelo_Equipamento");

                entity.Property(e => e.IdTipoEquipamento).HasColumnName("ID_Tipo_Equipamento");

                entity.Property(e => e.InventoryNo)
                    .HasColumnName("Inventory No_")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.JobNo)
                    .HasColumnName("Job No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LineStatus).HasColumnName("Line Status");

                entity.Property(e => e.LinhaOrçamento).HasColumnName("Linha Orçamento");

                entity.Property(e => e.MaintenanceTimeHours)
                    .HasColumnName("Maintenance Time (Hours)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.NotificationNo)
                    .HasColumnName("Notification No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NotificationType).HasColumnName("Notification Type");

                entity.Property(e => e.NumEquipamento)
                    .HasColumnName("Num_Equipamento")
                    .HasMaxLength(50);

                entity.Property(e => e.NumInventarioEquipamento)
                    .HasColumnName("Num_Inventario_Equipamento")
                    .HasMaxLength(50);

                entity.Property(e => e.NumSerieEquipamento)
                    .HasColumnName("Num_Serie_Equipamento")
                    .HasMaxLength(50);

                entity.Property(e => e.ObjectDescription)
                    .HasColumnName("Object Description")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectDescription2)
                    .HasColumnName("Object Description 2")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectNo)
                    .HasColumnName("Object No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectRefNo)
                    .HasColumnName("Object Ref_ No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectRefType).HasColumnName("Object Ref_ Type");

                entity.Property(e => e.ObjectType).HasColumnName("Object Type");

                entity.Property(e => e.OrderDate)
                    .HasColumnName("Order Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.OrderStatus).HasColumnName("Order Status");

                entity.Property(e => e.OrderTime)
                    .HasColumnName("Order Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.OrderType)
                    .HasColumnName("Order Type")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PlannerGroupNo)
                    .HasColumnName("Planner Group No_")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PostingDate)
                    .HasColumnName("Posting Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ResourceFilterYesNo).HasColumnName("Resource Filter Yes_No");

                entity.Property(e => e.ResourceNo)
                    .HasColumnName("Resource No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ResponseTimeHours)
                    .HasColumnName("Response Time (Hours)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.ResponsibilityCenter)
                    .HasColumnName("Responsibility Center")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension1Code)
                    .HasColumnName("Shortcut Dimension 1 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension2Code)
                    .HasColumnName("Shortcut Dimension 2 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension3Code)
                    .HasColumnName("Shortcut Dimension 3 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension4Code)
                    .HasColumnName("Shortcut Dimension 4 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SortField)
                    .HasColumnName("Sort Field")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.StartingDate)
                    .HasColumnName("Starting Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.StartingTime)
                    .HasColumnName("Starting Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.TaskListNo)
                    .HasColumnName("Task List No_")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.UsedDmmFilterYesNo).HasColumnName("Used DMM Filter Yes_No");

                entity.Property(e => e.WarrantyDate)
                    .HasColumnName("Warranty Date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<OrdemManutencao>(entity =>
            {
                entity.HasKey(e => e.IdOm);

                entity.ToTable("Ordem_Manutencao");

                entity.Property(e => e.IdOm).HasColumnName("ID_OM");

                entity.Property(e => e.Contacto)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Contrato)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DataEncerramento)
                    .HasColumnName("Data_Encerramento")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataPedido)
                    .HasColumnName("Data_Pedido")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataRegisto)
                    .HasColumnName("Data_registo")
                    .HasColumnType("datetime");

                entity.Property(e => e.DescAvaria)
                    .HasColumnName("Desc_Avaria")
                    .HasMaxLength(255);

                entity.Property(e => e.HoraAvaria).HasColumnName("Hora_Avaria");

                entity.Property(e => e.IdEstadoObra).HasColumnName("ID_Estado_Obra");

                entity.Property(e => e.IdOrigemAvaria).HasColumnName("ID_Origem_Avaria");

                entity.Property(e => e.IdTipoContacto).HasColumnName("ID_Tipo_Contacto");

                entity.Property(e => e.IdTipoObra).HasColumnName("ID_Tipo_Obra");

                entity.Property(e => e.NumOm).HasColumnName("Num_OM");

                entity.Property(e => e.NumReqCliente)
                    .HasColumnName("Num_Req_Cliente")
                    .HasMaxLength(20);

                entity.Property(e => e.RegistadoPor).HasColumnName("Registado_Por");

                entity.HasOne(d => d.ClienteNavigation)
                    .WithMany(p => p.OrdemManutencao)
                    .HasForeignKey(d => d.Cliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ordem_Manutencao_Cliente");

                entity.HasOne(d => d.ContratoNavigation)
                    .WithMany(p => p.OrdemManutencao)
                    .HasForeignKey(d => d.Contrato)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ordem_Manutencao_Contrato");

                entity.HasOne(d => d.IdEstadoObraNavigation)
                    .WithMany(p => p.OrdemManutencao)
                    .HasForeignKey(d => d.IdEstadoObra)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ordem_Manutencao_Estado_Obra");

                entity.HasOne(d => d.IdOrigemAvariaNavigation)
                    .WithMany(p => p.OrdemManutencao)
                    .HasForeignKey(d => d.IdOrigemAvaria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ordem_Manutencao_Origem_Avaria");

                entity.HasOne(d => d.IdTipoContactoNavigation)
                    .WithMany(p => p.OrdemManutencao)
                    .HasForeignKey(d => d.IdTipoContacto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ordem_Manutencao_Tipo_Contacto");

                entity.HasOne(d => d.IdTipoObraNavigation)
                    .WithMany(p => p.OrdemManutencao)
                    .HasForeignKey(d => d.IdTipoObra)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ordem_Manutencao_Tipo_Obra");

                entity.HasOne(d => d.InstituicaoNavigation)
                    .WithMany(p => p.OrdemManutencao)
                    .HasForeignKey(d => d.Instituicao)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ordem_Manutencao_Instituicao");

                entity.HasOne(d => d.RegistadoPorNavigation)
                    .WithMany(p => p.OrdemManutencao)
                    .HasForeignKey(d => d.RegistadoPor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ordem_Manutencao_Utilizador");

                entity.HasOne(d => d.ServicoNavigation)
                    .WithMany(p => p.OrdemManutencao)
                    .HasForeignKey(d => d.Servico)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ordem_Manutencao_Servico");
            });

            modelBuilder.Entity<OrdemManutencaoDescricaoAvaria>(entity =>
            {
                entity.HasKey(e => e.IdDescricaoAvaria);

                entity.ToTable("Ordem_Manutencao_DescricaoAvaria");

                entity.Property(e => e.IdDescricaoAvaria).HasColumnName("ID_DescricaoAvaria");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.DescricaoAvaria)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Om)
                    .IsRequired()
                    .HasColumnName("OM")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OrdemManutencaoEquipamentos>(entity =>
            {
                entity.HasKey(e => e.IdOmEquipamento);

                entity.ToTable("Ordem_Manutencao_Equipamentos");

                entity.Property(e => e.IdOmEquipamento).HasColumnName("ID_OM_Equipamento");

                entity.Property(e => e.IdEquipEstado).HasColumnName("ID_Equip_Estado");

                entity.Property(e => e.IdEquipamento).HasColumnName("ID_Equipamento");

                entity.Property(e => e.IdOm).HasColumnName("ID_OM");

                entity.Property(e => e.IdRotina).HasColumnName("ID_Rotina");

                entity.Property(e => e.TempoEntreAvarias).HasColumnName("Tempo_Entre_Avarias");

                entity.HasOne(d => d.ClienteNavigation)
                    .WithMany(p => p.OrdemManutencaoEquipamentos)
                    .HasForeignKey(d => d.Cliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ordem_Manutencao_Equipamentos_Cliente");

                entity.HasOne(d => d.IdEquipEstadoNavigation)
                    .WithMany(p => p.OrdemManutencaoEquipamentos)
                    .HasForeignKey(d => d.IdEquipEstado)
                    .HasConstraintName("FK_Ordem_Manutencao_Equipamentos_Equip_Estado");

                entity.HasOne(d => d.IdEquipamentoNavigation)
                    .WithMany(p => p.OrdemManutencaoEquipamentos)
                    .HasForeignKey(d => d.IdEquipamento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ordem_Manutencao_Equipamentos_Equipamento");

                entity.HasOne(d => d.IdOmNavigation)
                    .WithMany(p => p.OrdemManutencaoEquipamentos)
                    .HasForeignKey(d => d.IdOm)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ordem_Manutencao_Equipamentos_Ordem_Manutencao");

                entity.HasOne(d => d.IdRotinaNavigation)
                    .WithMany(p => p.OrdemManutencaoEquipamentos)
                    .HasForeignKey(d => d.IdRotina)
                    .HasConstraintName("FK_Ordem_Manutencao_Equipamentos_Rotina");

                entity.HasOne(d => d.ServicoNavigation)
                    .WithMany(p => p.OrdemManutencaoEquipamentos)
                    .HasForeignKey(d => d.Servico)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ordem_Manutencao_Equipamentos_Servico");
            });

            modelBuilder.Entity<OrdemManutencaoEstadoMaterial>(entity =>
            {
                entity.HasKey(e => e.IdEstadoMaterial);

                entity.ToTable("Ordem_Manutencao_Estado_Material");

                entity.Property(e => e.IdEstadoMaterial).HasColumnName("ID_Estado_Material");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<OrdemManutencaoLinha>(entity =>
            {
                entity.HasKey(e => e.IdOmLinha);

                entity.ToTable("Ordem_Manutencao_Linha");

                entity.HasIndex(e => e.No);

                entity.Property(e => e.IdOmLinha).HasColumnName("ID_OM_Linha");

                entity.Property(e => e.IdEquipEstado).HasColumnName("ID_Equip_Estado");

                entity.Property(e => e.IdEquipamento).HasColumnName("ID_Equipamento");

                entity.Property(e => e.IdRotina).HasColumnName("ID_Rotina");

                entity.Property(e => e.No)
                    .IsRequired()
                    .HasColumnName("No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NumLinha).HasColumnName("Num_Linha");

                entity.Property(e => e.Tbf).HasColumnName("TBF");

                entity.HasOne(d => d.IdEquipEstadoNavigation)
                    .WithMany(p => p.OrdemManutencaoLinha)
                    .HasForeignKey(d => d.IdEquipEstado)
                    .HasConstraintName("FK_Ordem_Manutencao_Linha_Equip_Estado");

                entity.HasOne(d => d.IdEquipamentoNavigation)
                    .WithMany(p => p.OrdemManutencaoLinha)
                    .HasForeignKey(d => d.IdEquipamento)
                    .HasConstraintName("FK_Ordem_Manutencao_Linha_Equipamento");

                entity.HasOne(d => d.IdRotinaNavigation)
                    .WithMany(p => p.OrdemManutencaoLinha)
                    .HasForeignKey(d => d.IdRotina)
                    .HasConstraintName("FK_Ordem_Manutencao_Linha_Rotina");
            });

            modelBuilder.Entity<OrdemManutencaoLinhaMateriais>(entity =>
            {
                entity.HasKey(e => e.IdOmLinhaMateriais);

                entity.ToTable("Ordem_Manutencao_Linha_Materiais");

                entity.HasIndex(e => e.No);

                entity.Property(e => e.IdOmLinhaMateriais).HasColumnName("ID_OM_Linha_Materiais");

                entity.Property(e => e.DataCriacao).HasColumnType("datetime");

                entity.Property(e => e.DescMaterial)
                    .HasColumnName("Desc_Material")
                    .HasMaxLength(250);

                entity.Property(e => e.EntryNo).HasColumnName("Entry_No");

                entity.Property(e => e.HoraFim).HasColumnType("datetime");

                entity.Property(e => e.HoraInicio).HasColumnType("datetime");

                entity.Property(e => e.IdMaterial).HasColumnName("ID_Material");

                entity.Property(e => e.IdOmLinha).HasColumnName("ID_OM_Linha");

                entity.Property(e => e.No)
                    .IsRequired()
                    .HasColumnName("No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.QtdMaterial).HasColumnName("Qtd_Material");
            });

            modelBuilder.Entity<OrdemManutencaoMateriais>(entity =>
            {
                entity.HasKey(e => e.IdOmMaterias);

                entity.ToTable("Ordem_Manutencao_Materiais");

                entity.Property(e => e.IdOmMaterias).HasColumnName("ID_OM_Materias");

                entity.Property(e => e.DataAplicacao)
                    .HasColumnName("Data_Aplicacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdEstadoMaterial).HasColumnName("ID_Estado_Material");

                entity.Property(e => e.IdFornecedor).HasColumnName("ID_Fornecedor");

                entity.Property(e => e.IdMaterial).HasColumnName("ID_Material");

                entity.Property(e => e.IdOm).HasColumnName("ID_OM");

                entity.Property(e => e.PrecoTotal)
                    .HasColumnName("Preco_Total")
                    .HasColumnType("money");

                entity.Property(e => e.PrecoUnitario)
                    .HasColumnName("Preco_Unitario")
                    .HasColumnType("money");

                entity.Property(e => e.QtdMaterial).HasColumnName("Qtd_Material");

                entity.Property(e => e.ReferenciaMaterial)
                    .HasColumnName("Referencia_Material")
                    .HasMaxLength(20);

                entity.HasOne(d => d.IdEstadoMaterialNavigation)
                    .WithMany(p => p.OrdemManutencaoMateriais)
                    .HasForeignKey(d => d.IdEstadoMaterial)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ordem_Manutencao_Materiais_Ordem_Manutencao_Estado_Material");

                entity.HasOne(d => d.IdOmNavigation)
                    .WithMany(p => p.OrdemManutencaoMateriais)
                    .HasForeignKey(d => d.IdOm)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ordem_Manutencao_Materiais_Ordem_Manutencao");
            });

            modelBuilder.Entity<OrdemManutencaoRelatorioTrabalho>(entity =>
            {
                entity.HasKey(e => e.IdRelatorioTrabalho);

                entity.ToTable("Ordem_Manutencao_RelatorioTrabalho");

                entity.Property(e => e.IdRelatorioTrabalho).HasColumnName("ID_RelatorioTrabalho");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Om)
                    .IsRequired()
                    .HasColumnName("OM")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RelatorioTrabalho)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<OrigemAvaria>(entity =>
            {
                entity.HasKey(e => e.IdOrigemAvaria);

                entity.ToTable("Origem_Avaria");

                entity.Property(e => e.IdOrigemAvaria).HasColumnName("ID_Origem_Avaria");

                entity.Property(e => e.OrigemAvaria1)
                    .IsRequired()
                    .HasColumnName("Origem_Avaria")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Periodicidade>(entity =>
            {
                entity.HasKey(e => e.IdPeriodicidade);

                entity.Property(e => e.IdPeriodicidade).HasColumnName("ID_Periodicidade");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumVezesAno).HasColumnName("Num_Vezes_Ano");
            });

            modelBuilder.Entity<PermissoesDefault>(entity =>
            {
                entity.HasKey(e => e.NivelAcesso);

                entity.ToTable("Permissoes_Default");

                entity.HasIndex(e => e.NivelAcesso)
                    .HasName("IX_Permissoes_Default")
                    .IsUnique();

                entity.Property(e => e.NivelAcesso)
                    .HasColumnName("Nivel_Acesso")
                    .ValueGeneratedNever();

                entity.Property(e => e.AcessoAdministracao).HasColumnName("Acesso_Administracao");

                entity.Property(e => e.ModuloClientes).HasColumnName("Modulo_Clientes");

                entity.Property(e => e.ModuloContratos).HasColumnName("Modulo_Contratos");

                entity.Property(e => e.ModuloDadosEstatisticos).HasColumnName("Modulo_DadosEstatisticos");

                entity.Property(e => e.ModuloEmm).HasColumnName("Modulo_EMM");

                entity.Property(e => e.ModuloFichaEquip).HasColumnName("Modulo_FichaEquip");

                entity.Property(e => e.ModuloFolhaObra).HasColumnName("Modulo_FolhaObra");

                entity.Property(e => e.ModuloFormacoesCompetencias).HasColumnName("Modulo_FormacoesCompetencias");

                entity.Property(e => e.ModuloFornecedores).HasColumnName("Modulo_Fornecedores");

                entity.Property(e => e.ModuloHabilitacoes).HasColumnName("Modulo_Habilitacoes");

                entity.Property(e => e.ModuloInstituicoes).HasColumnName("Modulo_Instituicoes");

                entity.Property(e => e.ModuloPlaneamento).HasColumnName("Modulo_Planeamento");

                entity.Property(e => e.ModuloRegistoDiario).HasColumnName("Modulo_RegistoDiario");

                entity.Property(e => e.ModuloReplicarPlaneamento).HasColumnName("Modulo_Replicar_Planeamento");

                entity.Property(e => e.ModuloRequisicoes).HasColumnName("Modulo_Requisicoes");

                entity.Property(e => e.ModuloServicos).HasColumnName("Modulo_Servicos");

                entity.Property(e => e.ModuloSolicitacoes).HasColumnName("Modulo_Solicitacoes");

                entity.Property(e => e.ModuloUtilizadores).HasColumnName("Modulo_Utilizadores");

                entity.Property(e => e.ValidadorEquipamento).HasColumnName("Validador_Equipamento");

                entity.HasOne(d => d.ModuloClientesNavigation)
                    .WithMany(p => p.PermissoesDefaultModuloClientesNavigation)
                    .HasForeignKey(d => d.ModuloClientes)
                    .HasConstraintName("FK_Permissoes_Default_Acessos_Cliente");

                entity.HasOne(d => d.ModuloContratosNavigation)
                    .WithMany(p => p.PermissoesDefaultModuloContratosNavigation)
                    .HasForeignKey(d => d.ModuloContratos)
                    .HasConstraintName("FK_Permissoes_Default_Acessos_Contratos");

                entity.HasOne(d => d.ModuloDadosEstatisticosNavigation)
                    .WithMany(p => p.PermissoesDefaultModuloDadosEstatisticosNavigation)
                    .HasForeignKey(d => d.ModuloDadosEstatisticos)
                    .HasConstraintName("FK_Permissoes_Default_Acessos_DadosEstatisticos");

                entity.HasOne(d => d.ModuloEmmNavigation)
                    .WithMany(p => p.PermissoesDefaultModuloEmmNavigation)
                    .HasForeignKey(d => d.ModuloEmm)
                    .HasConstraintName("FK_Permissoes_Default_Acessos_EMM");

                entity.HasOne(d => d.ModuloFichaEquipNavigation)
                    .WithMany(p => p.PermissoesDefaultModuloFichaEquipNavigation)
                    .HasForeignKey(d => d.ModuloFichaEquip)
                    .HasConstraintName("FK_Permissoes_Default_Acessos_FichaEquip");

                entity.HasOne(d => d.ModuloFolhaObraNavigation)
                    .WithMany(p => p.PermissoesDefaultModuloFolhaObraNavigation)
                    .HasForeignKey(d => d.ModuloFolhaObra)
                    .HasConstraintName("FK_Permissoes_Default_Acessos_FolhaObra");

                entity.HasOne(d => d.ModuloFormacoesCompetenciasNavigation)
                    .WithMany(p => p.PermissoesDefaultModuloFormacoesCompetenciasNavigation)
                    .HasForeignKey(d => d.ModuloFormacoesCompetencias)
                    .HasConstraintName("FK_Permissoes_Default_Acessos_FormacoesCompl");

                entity.HasOne(d => d.ModuloFornecedoresNavigation)
                    .WithMany(p => p.PermissoesDefaultModuloFornecedoresNavigation)
                    .HasForeignKey(d => d.ModuloFornecedores)
                    .HasConstraintName("FK_Permissoes_Default_Acessos_Fornecedores");

                entity.HasOne(d => d.ModuloHabilitacoesNavigation)
                    .WithMany(p => p.PermissoesDefaultModuloHabilitacoesNavigation)
                    .HasForeignKey(d => d.ModuloHabilitacoes)
                    .HasConstraintName("FK_Permissoes_Default_Acessos_Habilitacoes");

                entity.HasOne(d => d.ModuloInstituicoesNavigation)
                    .WithMany(p => p.PermissoesDefaultModuloInstituicoesNavigation)
                    .HasForeignKey(d => d.ModuloInstituicoes)
                    .HasConstraintName("FK_Permissoes_Default_Acessos_Institicoes");

                entity.HasOne(d => d.ModuloPlaneamentoNavigation)
                    .WithMany(p => p.PermissoesDefaultModuloPlaneamentoNavigation)
                    .HasForeignKey(d => d.ModuloPlaneamento)
                    .HasConstraintName("FK_Permissoes_Default_Acessos_Planeamento");

                entity.HasOne(d => d.ModuloRegistoDiarioNavigation)
                    .WithMany(p => p.PermissoesDefaultModuloRegistoDiarioNavigation)
                    .HasForeignKey(d => d.ModuloRegistoDiario)
                    .HasConstraintName("FK_Permissoes_Default_Acessos_RegistoDiario");

                entity.HasOne(d => d.ModuloReplicarPlaneamentoNavigation)
                    .WithMany(p => p.PermissoesDefaultModuloReplicarPlaneamentoNavigation)
                    .HasForeignKey(d => d.ModuloReplicarPlaneamento)
                    .HasConstraintName("FK_Permissoes_Default_Acessos_ReplicarPlaneamento");

                entity.HasOne(d => d.ModuloRequisicoesNavigation)
                    .WithMany(p => p.PermissoesDefaultModuloRequisicoesNavigation)
                    .HasForeignKey(d => d.ModuloRequisicoes)
                    .HasConstraintName("FK_Permissoes_Default_Acessos_Requisicoes");

                entity.HasOne(d => d.ModuloServicosNavigation)
                    .WithMany(p => p.PermissoesDefaultModuloServicosNavigation)
                    .HasForeignKey(d => d.ModuloServicos)
                    .HasConstraintName("FK_Permissoes_Default_Acessos_Servicos");

                entity.HasOne(d => d.ModuloSolicitacoesNavigation)
                    .WithMany(p => p.PermissoesDefaultModuloSolicitacoesNavigation)
                    .HasForeignKey(d => d.ModuloSolicitacoes)
                    .HasConstraintName("FK_Permissoes_Default_Acessos_Solicitacoes");

                entity.HasOne(d => d.ModuloUtilizadoresNavigation)
                    .WithMany(p => p.PermissoesDefaultModuloUtilizadoresNavigation)
                    .HasForeignKey(d => d.ModuloUtilizadores)
                    .HasConstraintName("FK_Permissoes_Default_Acessos_Utilizadores");

                entity.HasOne(d => d.NivelAcessoNavigation)
                    .WithOne(p => p.PermissoesDefault)
                    .HasForeignKey<PermissoesDefault>(d => d.NivelAcesso)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Permissoes_Default_Nivel_Acesso_Tipo");
            });

            modelBuilder.Entity<PlanoExecutado>(entity =>
            {
                entity.HasKey(e => e.IdPlanoExecutado);

                entity.ToTable("Plano_Executado");

                entity.Property(e => e.IdPlanoExecutado).HasColumnName("ID_Plano_Executado");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PostedMaintenanceOrder>(entity =>
            {
                entity.HasKey(e => new { e.DocumentType, e.No })
                    .HasName("Posted Maintenance Order$0");

                entity.ToTable("Posted Maintenance Order");

                entity.Property(e => e.DocumentType).HasColumnName("Document Type");

                entity.Property(e => e.No)
                    .HasColumnName("No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Address2)
                    .IsRequired()
                    .HasColumnName("Address 2")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ApplicationMethod).HasColumnName("Application Method");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ComponentOf)
                    .IsRequired()
                    .HasColumnName("Component Of")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ConcluidoPor)
                    .IsRequired()
                    .HasColumnName("Concluido Por")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ConfigResponsavel)
                    .IsRequired()
                    .HasColumnName("Config Responsavel")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Contact)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ContractNo)
                    .IsRequired()
                    .HasColumnName("Contract No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.County)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerAddress)
                    .IsRequired()
                    .HasColumnName("Customer Address")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerAddress2)
                    .IsRequired()
                    .HasColumnName("Customer Address 2")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerCity)
                    .IsRequired()
                    .HasColumnName("Customer City")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerContactName)
                    .IsRequired()
                    .HasColumnName("Customer Contact Name")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerCountryCode)
                    .IsRequired()
                    .HasColumnName("Customer Country Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerCounty)
                    .IsRequired()
                    .HasColumnName("Customer County")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerDiscGroup)
                    .IsRequired()
                    .HasColumnName("Customer Disc_ Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerDocNo)
                    .IsRequired()
                    .HasColumnName("Customer Doc_ No_")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerEMail)
                    .IsRequired()
                    .HasColumnName("Customer E-Mail")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerFaxNo)
                    .IsRequired()
                    .HasColumnName("Customer Fax No_")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerName)
                    .IsRequired()
                    .HasColumnName("Customer Name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerName2)
                    .IsRequired()
                    .HasColumnName("Customer Name 2")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerNo)
                    .IsRequired()
                    .HasColumnName("Customer No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerPhoneNo)
                    .IsRequired()
                    .HasColumnName("Customer Phone No_")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerPostCode)
                    .IsRequired()
                    .HasColumnName("Customer Post Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerPriceGroup)
                    .IsRequired()
                    .HasColumnName("Customer Price Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerReference)
                    .IsRequired()
                    .HasColumnName("Customer Reference")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerShipToCode)
                    .IsRequired()
                    .HasColumnName("Customer Ship-to Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DataChefeProjecto)
                    .HasColumnName("Data Chefe Projecto")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataEntrada)
                    .HasColumnName("Data Entrada")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataEntrega)
                    .HasColumnName("Data Entrega")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataFacturação)
                    .HasColumnName("Data Facturação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataFecho)
                    .HasColumnName("Data Fecho")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataPedido)
                    .HasColumnName("Data Pedido")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataPedidoReparação)
                    .HasColumnName("Data Pedido Reparação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataReabertura)
                    .HasColumnName("Data Reabertura")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataResponsavel)
                    .HasColumnName("Data Responsavel")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataSaída)
                    .HasColumnName("Data Saída")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataUltimoMail)
                    .HasColumnName("Data Ultimo Mail")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataValidade)
                    .HasColumnName("Data Validade")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeliberaçãoCa)
                    .IsRequired()
                    .HasColumnName("Deliberação CA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Descrição1)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Descrição2)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Descrição3)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentDate)
                    .HasColumnName("Document Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.EnteredBy)
                    .IsRequired()
                    .HasColumnName("Entered By")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ExpServQty)
                    .HasColumnName("Exp Serv Qty")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.ExpectedFinishingDate)
                    .HasColumnName("Expected Finishing Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ExpectedFinishingTime)
                    .HasColumnName("Expected Finishing Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.ExpectedStartingDate)
                    .HasColumnName("Expected Starting Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ExpectedStartingTime)
                    .HasColumnName("Expected Starting Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.FaNo)
                    .IsRequired()
                    .HasColumnName("FA No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FaPostingGroup)
                    .IsRequired()
                    .HasColumnName("FA Posting Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FaxNo)
                    .IsRequired()
                    .HasColumnName("Fax No_")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.FechadoPor)
                    .IsRequired()
                    .HasColumnName("Fechado Por")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FinishingDate)
                    .HasColumnName("Finishing Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.FinishingTime)
                    .HasColumnName("Finishing Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.FinishingTimeHours)
                    .HasColumnName("Finishing Time (Hours)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.FlDescription)
                    .IsRequired()
                    .HasColumnName("FL Description")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FlNo)
                    .IsRequired()
                    .HasColumnName("FL No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.GenBusPostingGroup)
                    .IsRequired()
                    .HasColumnName("Gen_ Bus_ Posting Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.HoraFecho)
                    .HasColumnName("Hora Fecho")
                    .HasColumnType("datetime");

                entity.Property(e => e.HoraPedidoReparação)
                    .HasColumnName("Hora Pedido Reparação")
                    .HasColumnType("datetime");

                entity.Property(e => e.HoraReabertura)
                    .HasColumnName("Hora Reabertura")
                    .HasColumnType("datetime");

                entity.Property(e => e.JobNo)
                    .IsRequired()
                    .HasColumnName("Job No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.JobPostingGroup)
                    .IsRequired()
                    .HasColumnName("Job Posting Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LanguageCode)
                    .IsRequired()
                    .HasColumnName("Language Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LastDateModified)
                    .HasColumnName("Last Date Modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.Loc1)
                    .IsRequired()
                    .HasColumnName("loc1")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Loc2)
                    .IsRequired()
                    .HasColumnName("loc2")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Loc3)
                    .IsRequired()
                    .HasColumnName("loc3")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MachineCenterNo)
                    .IsRequired()
                    .HasColumnName("Machine Center No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MaintenanceActivity)
                    .IsRequired()
                    .HasColumnName("Maintenance Activity")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.MaintenanceTimeHours)
                    .HasColumnName("Maintenance Time (Hours)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.MãoDeObraEDeslocações).HasColumnName("Mão de Obra e Deslocações");

                entity.Property(e => e.NoCompromisso)
                    .IsRequired()
                    .HasColumnName("No_ Compromisso")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NoDocumentoContactoInicial)
                    .IsRequired()
                    .HasColumnName("No Documento Contacto Inicial")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.NoSeries)
                    .IsRequired()
                    .HasColumnName("No_ Series")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NºAntigoAs400)
                    .IsRequired()
                    .HasColumnName("Nº Antigo AS400")
                    .HasMaxLength(14)
                    .IsUnicode(false);

                entity.Property(e => e.NºGeste)
                    .IsRequired()
                    .HasColumnName("Nº GESTE")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.NºLinhaContrato).HasColumnName("Nº Linha Contrato");

                entity.Property(e => e.ObjectRefDescription)
                    .IsRequired()
                    .HasColumnName("Object Ref_ Description")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectRefNo)
                    .IsRequired()
                    .HasColumnName("Object Ref_ No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectRefType).HasColumnName("Object Ref_ Type");

                entity.Property(e => e.OrderDate)
                    .HasColumnName("Order Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.OrderTime)
                    .HasColumnName("Order Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.OrderType)
                    .IsRequired()
                    .HasColumnName("Order Type")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNo)
                    .IsRequired()
                    .HasColumnName("Phone No_")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.PlannedOrderNo)
                    .IsRequired()
                    .HasColumnName("Planned Order No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PlannerGroupNo)
                    .IsRequired()
                    .HasColumnName("Planner Group No_")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PostCode)
                    .IsRequired()
                    .HasColumnName("Post Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PostingDate)
                    .HasColumnName("Posting Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.PurchaserCode)
                    .IsRequired()
                    .HasColumnName("Purchaser Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ReabertoPor)
                    .IsRequired()
                    .HasColumnName("Reaberto Por")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Reference)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ResourceNo)
                    .IsRequired()
                    .HasColumnName("Resource No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RespCenterName)
                    .IsRequired()
                    .HasColumnName("Resp_ Center Name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RespCenterName2)
                    .IsRequired()
                    .HasColumnName("Resp_ Center Name 2")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ResponseTimeHours)
                    .HasColumnName("Response Time (Hours)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.ResponsibilityCenter)
                    .IsRequired()
                    .HasColumnName("Responsibility Center")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ResponsibleEmployee)
                    .IsRequired()
                    .HasColumnName("Responsible Employee")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ServInternosDébInternos).HasColumnName("Serv_ Internos -Déb Internos");

                entity.Property(e => e.ServInternosFolhasDeObra).HasColumnName("Serv_ Internos -Folhas de Obra");

                entity.Property(e => e.ServInternosRequisições).HasColumnName("Serv_ Internos -Requisições");

                entity.Property(e => e.ShipToAddress)
                    .IsRequired()
                    .HasColumnName("Ship-to Address")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToAddress2)
                    .IsRequired()
                    .HasColumnName("Ship-to Address 2")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToCity)
                    .IsRequired()
                    .HasColumnName("Ship-to City")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToCode)
                    .IsRequired()
                    .HasColumnName("Ship-to Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToContact)
                    .IsRequired()
                    .HasColumnName("Ship-to Contact")
                    .HasMaxLength(27)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToCounty)
                    .IsRequired()
                    .HasColumnName("Ship-to County")
                    .HasMaxLength(27)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToName)
                    .IsRequired()
                    .HasColumnName("Ship-to Name")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToName2)
                    .IsRequired()
                    .HasColumnName("Ship-to Name 2")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToPostCode)
                    .IsRequired()
                    .HasColumnName("Ship-to Post Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension1Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 1 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension2Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 2 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension3Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 3 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension4Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 4 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SourceDocNo)
                    .IsRequired()
                    .HasColumnName("Source Doc_ No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SourceDocType).HasColumnName("Source Doc_ Type");

                entity.Property(e => e.StartingDate)
                    .HasColumnName("Starting Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.StartingTime)
                    .HasColumnName("Starting Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.SuspendedOrderReason)
                    .IsRequired()
                    .HasColumnName("Suspended Order Reason")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.TipoContactoCliente)
                    .IsRequired()
                    .HasColumnName("Tipo Contacto Cliente")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TipoContactoClienteInicial)
                    .IsRequired()
                    .HasColumnName("Tipo Contacto Cliente Inicial")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TotalQtyToInvoice)
                    .HasColumnName("Total Qty_ to Invoice")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TotalQuantity)
                    .HasColumnName("Total Quantity")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.TécnicoExecutante)
                    .IsRequired()
                    .HasColumnName("Técnico Executante")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserChefeProjecto)
                    .IsRequired()
                    .HasColumnName("User Chefe Projecto")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserResponsavel)
                    .IsRequired()
                    .HasColumnName("User Responsavel")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Validade)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.ValidadePedido)
                    .HasColumnName("Validade Pedido")
                    .HasColumnType("datetime");

                entity.Property(e => e.ValorProjecto)
                    .HasColumnName("Valor Projecto")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.VatRegistrationNo)
                    .IsRequired()
                    .HasColumnName("VAT Registration No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.WorkCenterNo)
                    .IsRequired()
                    .HasColumnName("Work Center No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PostedMaintenanceOrderLine>(entity =>
            {
                entity.HasKey(e => new { e.DocumentType, e.MoNo, e.LineNo })
                    .HasName("Posted Maintenance Order Line$0");

                entity.ToTable("Posted Maintenance Order Line");

                entity.Property(e => e.DocumentType).HasColumnName("Document Type");

                entity.Property(e => e.MoNo)
                    .HasColumnName("MO No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LineNo).HasColumnName("Line No_");

                entity.Property(e => e.AdditionalData)
                    .IsRequired()
                    .HasColumnName("Additional Data")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.BomNo)
                    .IsRequired()
                    .HasColumnName("BOM No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ComponentOf)
                    .IsRequired()
                    .HasColumnName("Component Of")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ContractNo)
                    .IsRequired()
                    .HasColumnName("Contract No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerNo)
                    .IsRequired()
                    .HasColumnName("Customer No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DataEntrada)
                    .HasColumnName("Data Entrada")
                    .HasColumnType("datetime");

                entity.Property(e => e.FaultReasonCode).HasColumnName("Fault Reason Code");

                entity.Property(e => e.FinalState).HasColumnName("Final State");

                entity.Property(e => e.FinishingDate)
                    .HasColumnName("Finishing Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.FinishingTime)
                    .HasColumnName("Finishing Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.FunctionalLocationNo)
                    .IsRequired()
                    .HasColumnName("Functional Location No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.GuiaDeTransporte).HasColumnName("Guia de Transporte");

                entity.Property(e => e.IdEquipEstado).HasColumnName("ID_Equip_Estado");

                entity.Property(e => e.IdEquipamento).HasColumnName("ID_Equipamento");

                entity.Property(e => e.IdInstituicao).HasColumnName("ID_Instituicao");

                entity.Property(e => e.IdRotina).HasColumnName("ID_Rotina");

                entity.Property(e => e.IdServico).HasColumnName("ID_Servico");

                entity.Property(e => e.InventoryNo)
                    .IsRequired()
                    .HasColumnName("Inventory No_")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.JobNo)
                    .IsRequired()
                    .HasColumnName("Job No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LineStatus).HasColumnName("Line Status");

                entity.Property(e => e.MaintenanceTimeHours)
                    .HasColumnName("Maintenance Time (Hours)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.NotificationNo)
                    .IsRequired()
                    .HasColumnName("Notification No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NotificationType).HasColumnName("Notification Type");

                entity.Property(e => e.ObjectDescription)
                    .IsRequired()
                    .HasColumnName("Object Description")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectDescription2)
                    .IsRequired()
                    .HasColumnName("Object Description 2")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectNo)
                    .IsRequired()
                    .HasColumnName("Object No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectType).HasColumnName("Object Type");

                entity.Property(e => e.OrderDate)
                    .HasColumnName("Order Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.OrderStatus).HasColumnName("Order Status");

                entity.Property(e => e.OrderTime)
                    .HasColumnName("Order Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.PlannerGroupNo)
                    .IsRequired()
                    .HasColumnName("Planner Group No_")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PostingDate)
                    .HasColumnName("Posting Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ResourceFilterYesNo).HasColumnName("Resource Filter Yes_No");

                entity.Property(e => e.ResourceNo)
                    .IsRequired()
                    .HasColumnName("Resource No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ResponseTimeHours)
                    .HasColumnName("Response Time (Hours)")
                    .HasColumnType("decimal(38, 20)");

                entity.Property(e => e.ResponsibilityCenter)
                    .IsRequired()
                    .HasColumnName("Responsibility Center")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension1Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 1 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension2Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 2 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension3Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 3 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension4Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 4 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SortField)
                    .IsRequired()
                    .HasColumnName("Sort Field")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.StartingDate)
                    .HasColumnName("Starting Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.StartingTime)
                    .HasColumnName("Starting Time")
                    .HasColumnType("datetime");

                entity.Property(e => e.TaskListNo)
                    .IsRequired()
                    .HasColumnName("Task List No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Tbf).HasColumnName("TBF");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.WarrantyDate)
                    .HasColumnName("Warranty Date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<ProjectosAutorizadosFact>(entity =>
            {
                entity.HasKey(e => new { e.No, e.GrupoFactura })
                    .HasName("Projectos Autorizados Fact$0");

                entity.ToTable("Projectos Autorizados Fact");

                entity.Property(e => e.No)
                    .HasColumnName("No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.GrupoFactura).HasColumnName("Grupo Factura");

                entity.Property(e => e.AreaFilter)
                    .IsRequired()
                    .HasColumnName("Area Filter")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.BillToCustomerNo)
                    .IsRequired()
                    .HasColumnName("Bill-to Customer No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ContractNo)
                    .IsRequired()
                    .HasColumnName("Contract No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DataAutorização)
                    .HasColumnName("Data Autorização")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataPedido)
                    .HasColumnName("Data Pedido")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataPrestacaoServico)
                    .HasColumnName("Data Prestacao Servico")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataServPrestado)
                    .IsRequired()
                    .HasColumnName("Data Serv_ Prestado")
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description2)
                    .IsRequired()
                    .HasColumnName("Description 2")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DescriçãoGrupo)
                    .IsRequired()
                    .HasColumnName("Descrição Grupo")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Diversos)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.GlobalDimension1Code)
                    .IsRequired()
                    .HasColumnName("Global Dimension 1 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.GlobalDimension2Code)
                    .IsRequired()
                    .HasColumnName("Global Dimension 2 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.JobPostingGroup)
                    .IsRequired()
                    .HasColumnName("Job Posting Group")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Motorista)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.NoCompromisso)
                    .IsRequired()
                    .HasColumnName("No_ Compromisso")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NoSeries)
                    .IsRequired()
                    .HasColumnName("No_ Series")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Observações)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Observações1)
                    .IsRequired()
                    .HasColumnName("Observações 1")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.OnlyForMaintInvoicing).HasColumnName("Only for Maint_ Invoicing");

                entity.Property(e => e.PaymentMethodCode)
                    .IsRequired()
                    .HasColumnName("Payment Method Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentTermsCode)
                    .IsRequired()
                    .HasColumnName("Payment Terms Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PedidoDoCliente)
                    .IsRequired()
                    .HasColumnName("Pedido do Cliente")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.SearchDescription)
                    .IsRequired()
                    .HasColumnName("Search Description")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShipToCode)
                    .IsRequired()
                    .HasColumnName("Ship-to Code")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension3Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 3 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortcutDimension4Code)
                    .IsRequired()
                    .HasColumnName("Shortcut Dimension 4 Code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SituaçõesPendentes)
                    .IsRequired()
                    .HasColumnName("Situações Pendentes")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.TipoGrupoContabOmProjecto)
                    .IsRequired()
                    .HasColumnName("Tipo Grupo Contab _OM Projecto")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TipoGrupoContabProjecto)
                    .IsRequired()
                    .HasColumnName("Tipo Grupo Contab _ Projecto")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TipoProjecto).HasColumnName("Tipo Projecto");

                entity.Property(e => e.Utilizador)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Regiao>(entity =>
            {
                entity.HasKey(e => e.IdRegiao)
                    .HasName("PK_Regioes");

                entity.Property(e => e.IdRegiao).HasColumnName("ID_Regiao");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Regiao1)
                    .IsRequired()
                    .HasColumnName("Regiao")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Rotina>(entity =>
            {
                entity.HasKey(e => e.IdRotina);

                entity.Property(e => e.IdRotina).HasColumnName("ID_Rotina");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasMaxLength(3);
            });

            modelBuilder.Entity<Sequencia>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.NumSequencia).HasColumnName("Num_Sequencia");

                entity.Property(e => e.Tabela)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Servico>(entity =>
            {
                entity.HasKey(e => e.IdServico);

                entity.HasIndex(e => e.IdServico)
                    .HasName("_dta_index_Servico_6_606625204__K1_5492");

                entity.HasIndex(e => e.Nome)
                    .HasName("_dta_index_Servico_6_606625204__K2_8337");

                entity.HasIndex(e => new { e.Nome, e.IdServico })
                    .HasName("_dta_index_Servico_6_606625204__K2_K1_9085");

                entity.HasIndex(e => new { e.IdServico, e.Nome, e.NoNavision, e.Instituicao, e.Activo })
                    .HasName("_dta_index_Servico_6_606625204__K3_K5_1_2_8");

                entity.HasIndex(e => new { e.IdServico, e.Nome, e.Instituicao, e.TreePath, e.Activo, e.CentroCusto, e.Morada, e.NoNavision })
                    .HasName("_dta_index_Servico_6_606625204__col___4606");

                entity.Property(e => e.IdServico).HasColumnName("ID_Servico");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CentroCusto).HasMaxLength(50);

                entity.Property(e => e.Morada).HasMaxLength(250);

                entity.Property(e => e.NoNavision)
                    .HasColumnName("No_Navision")
                    .HasMaxLength(20);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TreePath).HasMaxLength(100);

                entity.HasOne(d => d.InstituicaoNavigation)
                    .WithMany(p => p.Servico)
                    .HasForeignKey(d => d.Instituicao)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Servico_Instituicao");
            });

            modelBuilder.Entity<SistemaConfig>(entity =>
            {
                entity.HasKey(e => e.IdConfig);

                entity.ToTable("Sistema_Config");

                entity.Property(e => e.IdConfig).HasColumnName("ID_Config");

                entity.Property(e => e.IdCliente).HasColumnName("ID_Cliente");

                entity.Property(e => e.NomeInstituicao)
                    .IsRequired()
                    .HasColumnName("Nome_Instituicao")
                    .HasMaxLength(50);

                entity.Property(e => e.NomeInstituicaoMae)
                    .IsRequired()
                    .HasColumnName("Nome_InstituicaoMae")
                    .HasMaxLength(50);

                entity.Property(e => e.NomeServico)
                    .IsRequired()
                    .HasColumnName("Nome_Servico")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Solicitacoes>(entity =>
            {
                entity.HasKey(e => e.IdSolicitacao);

                entity.Property(e => e.IdSolicitacao).HasColumnName("ID_Solicitacao");

                entity.Property(e => e.ContactoEmail)
                    .HasColumnName("Contacto_Email")
                    .HasMaxLength(50);

                entity.Property(e => e.ContactoFax)
                    .HasColumnName("Contacto_Fax")
                    .HasMaxLength(50);

                entity.Property(e => e.ContactoNome)
                    .HasColumnName("Contacto_Nome")
                    .HasMaxLength(50);

                entity.Property(e => e.ContactoTelefone)
                    .HasColumnName("Contacto_Telefone")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("Data_Criacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataFecho)
                    .HasColumnName("Data_Fecho")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataPedido)
                    .HasColumnName("Data_Pedido")
                    .HasColumnType("datetime");

                entity.Property(e => e.DescricaoAnomalia).HasColumnName("Descricao_Anomalia");

                entity.Property(e => e.EstadoAnotacao).HasColumnName("Estado_Anotacao");

                entity.Property(e => e.IdCliente).HasColumnName("ID_Cliente");

                entity.Property(e => e.IdContrato)
                    .HasColumnName("ID_Contrato")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdContratoLinha).HasColumnName("ID_Contrato_Linha");

                entity.Property(e => e.IdEquipamento).HasColumnName("ID_Equipamento");

                entity.Property(e => e.IdInstituicao).HasColumnName("ID_Instituicao");

                entity.Property(e => e.IdResponsavelTriagem).HasColumnName("ID_Responsavel_Triagem");

                entity.Property(e => e.IdServico).HasColumnName("ID_Servico");

                entity.Property(e => e.IdUserCriacao).HasColumnName("ID_User_Criacao");

                entity.Property(e => e.Localizacao).HasMaxLength(250);

                entity.Property(e => e.ManutencaoPreventiva).HasColumnName("Manutencao_Preventiva");

                entity.Property(e => e.Marca).HasMaxLength(50);

                entity.Property(e => e.Modelo).HasMaxLength(50);

                entity.Property(e => e.NumInventario)
                    .HasColumnName("Num_Inventario")
                    .HasMaxLength(50);

                entity.Property(e => e.NumSerie)
                    .HasColumnName("Num_Serie")
                    .HasMaxLength(50);

                entity.Property(e => e.OmGerada)
                    .HasColumnName("OM_Gerada")
                    .HasMaxLength(20);

                entity.Property(e => e.OrcamentoGerado)
                    .HasColumnName("Orcamento_Gerado")
                    .HasMaxLength(20);

                entity.Property(e => e.ReferenciaCliente)
                    .HasColumnName("Referencia_Cliente")
                    .HasMaxLength(50);

                entity.Property(e => e.TipoEquipamento)
                    .HasColumnName("Tipo_Equipamento")
                    .HasMaxLength(60);

                entity.Property(e => e.TipoSolicitacao)
                    .HasColumnName("Tipo_Solicitacao")
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SolicitacoesEstado>(entity =>
            {
                entity.HasKey(e => e.IdEstadoSolicitacao);

                entity.ToTable("Solicitacoes_Estado");

                entity.Property(e => e.IdEstadoSolicitacao).HasColumnName("ID_Estado_Solicitacao");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<SolicitacoesLinha>(entity =>
            {
                entity.HasKey(e => e.IdOmLinha);

                entity.ToTable("Solicitacoes_Linha");

                entity.Property(e => e.IdOmLinha).HasColumnName("ID_OM_Linha");

                entity.Property(e => e.IdEquipEstado).HasColumnName("ID_Equip_Estado");

                entity.Property(e => e.IdEquipamento).HasColumnName("ID_Equipamento");

                entity.Property(e => e.IdRotina).HasColumnName("ID_Rotina");

                entity.Property(e => e.No)
                    .IsRequired()
                    .HasColumnName("No_")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NumLinha).HasColumnName("Num_Linha");

                entity.Property(e => e.Tbf).HasColumnName("TBF");

                entity.HasOne(d => d.IdEquipEstadoNavigation)
                    .WithMany(p => p.SolicitacoesLinha)
                    .HasForeignKey(d => d.IdEquipEstado)
                    .HasConstraintName("FK_Solicitacoes_Linha_Equip_Estado");

                entity.HasOne(d => d.IdEquipamentoNavigation)
                    .WithMany(p => p.SolicitacoesLinha)
                    .HasForeignKey(d => d.IdEquipamento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Solicitacoes_Linha_Equipamento");

                entity.HasOne(d => d.IdRotinaNavigation)
                    .WithMany(p => p.SolicitacoesLinha)
                    .HasForeignKey(d => d.IdRotina)
                    .HasConstraintName("FK_Solicitacoes_Linha_Rotina");
            });

            modelBuilder.Entity<TipoContacto>(entity =>
            {
                entity.HasKey(e => e.IdTipoContacto);

                entity.ToTable("Tipo_Contacto");

                entity.Property(e => e.IdTipoContacto).HasColumnName("ID_Tipo_Contacto");

                entity.Property(e => e.TipoContacto1)
                    .IsRequired()
                    .HasColumnName("Tipo_Contacto")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<TipoDeContacto>(entity =>
            {
                entity.HasKey(e => e.IdTipoDeContacto);

                entity.ToTable("Tipo_de_Contacto");

                entity.Property(e => e.IdTipoDeContacto).HasColumnName("ID_Tipo_de_Contacto");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<TipoMaoObra>(entity =>
            {
                entity.HasKey(e => e.IdTipoMaoObra);

                entity.ToTable("Tipo_Mao_Obra");

                entity.Property(e => e.IdTipoMaoObra).HasColumnName("ID_Tipo_Mao_Obra");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<TipoObra>(entity =>
            {
                entity.HasKey(e => e.IdTipoObra);

                entity.ToTable("Tipo_Obra");

                entity.Property(e => e.IdTipoObra).HasColumnName("ID_Tipo_Obra");

                entity.Property(e => e.Descricao).HasMaxLength(50);

                entity.Property(e => e.TipoObra1)
                    .IsRequired()
                    .HasColumnName("Tipo_Obra")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<TransfereProdutosProjecto>(entity =>
            {
                entity.HasKey(e => e.MovimentoOrigem)
                    .HasName("TransfereProdutosProjecto$0");

                entity.Property(e => e.MovimentoOrigem).ValueGeneratedNever();

                entity.Property(e => e.CodLocaliz)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DescProduto)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.NºDocumento)
                    .IsRequired()
                    .HasColumnName("Nº Documento")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ProdutoDestino)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ProdutoOrigem)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectoDestino)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectoOrigem)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.QtdPossivelTransferir).HasColumnType("decimal(38, 20)");

                entity.Property(e => e.QtdPrevPossivelTransferir).HasColumnType("decimal(38, 20)");

                entity.Property(e => e.QuantidadeDestino).HasColumnType("decimal(38, 20)");

                entity.Property(e => e.QuantidadeJaTransferida).HasColumnType("decimal(38, 20)");

                entity.Property(e => e.QuantidadeOrigem).HasColumnType("decimal(38, 20)");

                entity.Property(e => e.SomaQtdProduto).HasColumnType("decimal(38, 20)");

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasColumnName("timestamp")
                    .IsRowVersion();

                entity.Property(e => e.UnidadeMedida)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Utilizador>(entity =>
            {
                entity.HasIndex(e => e.Nome)
                    .HasName("_dta_index_Utilizador_6_1602872827__K2_8258");

                entity.HasIndex(e => e.NumMec)
                    .HasName("_dta_index_Utilizador_6_1602872827__K14_9987");

                entity.HasIndex(e => new { e.Nome, e.NumMec })
                    .HasName("_dta_index_Utilizador_6_1602872827__K2_K14_8809");

                entity.HasIndex(e => new { e.ResponsavelProjecto, e.UserRespProjecto });

                entity.HasIndex(e => new { e.Username, e.Activo });

                entity.HasIndex(e => new { e.Username, e.NumMec })
                    .HasName("_dta_index_Utilizador_6_1602872827__K14_3");

                entity.HasIndex(e => new { e.Id, e.Nome, e.Activo, e.NivelAcesso })
                    .HasName("IX_Utilizador_Activo_Nivel_Acesso");

                entity.HasIndex(e => new { e.Nome, e.Activo, e.Code2, e.NumMec })
                    .HasName("IX_Utilizador_Activo_Code 2_Num_Mec");

                entity.HasIndex(e => new { e.Nome, e.Username, e.Code1, e.Code2, e.Code3, e.ChefeProjecto, e.ResponsavelProjecto, e.UserRespProjecto, e.NumMec })
                    .HasName("IX_Utilizador_Num_Mec");

                entity.HasIndex(e => new { e.Nome, e.Username, e.NivelAcesso, e.Code1, e.Code2, e.ChefeProjecto, e.ResponsavelProjecto, e.UserRespProjecto, e.Code3, e.NumMec })
                    .HasName("IX_Utilizador_Code 3_Num_Mec");

                entity.HasIndex(e => new { e.Id, e.Nome, e.Username, e.Password, e.Email, e.TelefoneGeral, e.TelefoneExtensao, e.Telemovel, e.NivelAcesso, e.Code1, e.Code3, e.NumMec, e.Code2 })
                    .HasName("IX_Utilizador_Code 2");

                entity.HasIndex(e => new { e.Id, e.Nome, e.Username, e.Password, e.Email, e.TelefoneGeral, e.TelefoneExtensao, e.Telemovel, e.NivelAcesso, e.Activo, e.Code1, e.Code2, e.Code3, e.NumMec, e.AcessoAdministracao, e.ModuloFolhaObra, e.ModuloFichaEquip, e.ModuloRequisicoes, e.ModuloRegistoDiario, e.ModuloContratos, e.ModuloPlaneamento, e.ValidadorEquipamento, e.ModuloClientes, e.ModuloFornecedores, e.ModuloInstituicoes, e.ModuloServicos, e.ModuloDadosEstatisticos, e.ModuloHabilitacoes, e.ModuloFormacoesCompetencias, e.ModuloUtilizadores, e.SuperiorHierarquico, e.ModuloReplicarPlaneamento, e.ModuloEmm, e.ModuloSolicitacoes, e.EquipaFixa, e.IdEquipa, e.IdCliente, e.AutorizarFacturar, e.ChefeProjecto, e.ResponsavelProjecto, e.UserRespProjecto })
                    .HasName("_dta_index_Utilizador_6_1602872827__col___6980");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AcessoAdministracao)
                    .HasColumnName("Acesso_Administracao")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.AutorizarFacturar)
                    .HasColumnName("Autorizar_Facturar")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ChefeProjecto)
                    .HasColumnName("Chefe_Projecto")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Code1)
                    .HasColumnName("Code 1")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Code2)
                    .HasColumnName("Code 2")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Code3)
                    .HasColumnName("Code 3")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.EquipaFixa)
                    .HasColumnName("Equipa_Fixa")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IdCliente).HasColumnName("ID_Cliente");

                entity.Property(e => e.IdEquipa).HasColumnName("ID_Equipa");

                entity.Property(e => e.ModuloClientes)
                    .HasColumnName("Modulo_Clientes")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ModuloContratos)
                    .HasColumnName("Modulo_Contratos")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ModuloDadosEstatisticos)
                    .HasColumnName("Modulo_DadosEstatisticos")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ModuloEmm)
                    .HasColumnName("Modulo_EMM")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ModuloFichaEquip)
                    .HasColumnName("Modulo_FichaEquip")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ModuloFolhaObra)
                    .HasColumnName("Modulo_FolhaObra")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ModuloFormacoesCompetencias)
                    .HasColumnName("Modulo_FormacoesCompetencias")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ModuloFornecedores)
                    .HasColumnName("Modulo_Fornecedores")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ModuloHabilitacoes)
                    .HasColumnName("Modulo_Habilitacoes")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ModuloInstituicoes)
                    .HasColumnName("Modulo_Instituicoes")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ModuloPlaneamento)
                    .HasColumnName("Modulo_Planeamento")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ModuloRegistoDiario)
                    .HasColumnName("Modulo_RegistoDiario")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ModuloReplicarPlaneamento)
                    .HasColumnName("Modulo_Replicar_Planeamento")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ModuloRequisicoes)
                    .HasColumnName("Modulo_Requisicoes")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ModuloServicos)
                    .HasColumnName("Modulo_Servicos")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ModuloSolicitacoes)
                    .HasColumnName("Modulo_Solicitacoes")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ModuloUtilizadores)
                    .HasColumnName("Modulo_Utilizadores")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.NivelAcesso)
                    .HasColumnName("Nivel_Acesso")
                    .HasDefaultValueSql("((7))");

                entity.Property(e => e.Nome)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NumMec)
                    .HasColumnName("Num_Mec")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((1234))");

                entity.Property(e => e.ResponsavelProjecto)
                    .HasColumnName("Responsavel_Projecto")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TelefoneExtensao)
                    .HasColumnName("Telefone_Extensao")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TelefoneGeral)
                    .HasColumnName("Telefone_Geral")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Telemovel)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserRespProjecto)
                    .HasColumnName("User_Resp_Projecto")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Username).HasMaxLength(256);

                entity.Property(e => e.ValidadorEquipamento)
                    .HasColumnName("Validador_Equipamento")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.NivelAcessoNavigation)
                    .WithMany(p => p.Utilizador)
                    .HasForeignKey(d => d.NivelAcesso)
                    .HasConstraintName("FK_Utilizador_Nivel_Acesso_Tipo");
            });

            modelBuilder.Entity<UtilizadorCompetencias>(entity =>
            {
                entity.HasKey(e => e.IdCompetencia);

                entity.ToTable("Utilizador_Competencias");

                entity.Property(e => e.IdCompetencia).HasColumnName("ID_Competencia");

                entity.Property(e => e.DataEmissao)
                    .HasColumnName("Data_Emissao")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.DataValidade)
                    .HasColumnName("Data_Validade")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IdUtilizador).HasColumnName("ID_Utilizador");

                entity.Property(e => e.NumCarteira)
                    .HasColumnName("Num_Carteira")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Observacao)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUtilizadorNavigation)
                    .WithMany(p => p.UtilizadorCompetencias)
                    .HasForeignKey(d => d.IdUtilizador)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Utilizador_Competencias_Utilizador");
            });

            modelBuilder.Entity<UtilizadorFormacao>(entity =>
            {
                entity.HasKey(e => e.IdFormacao);

                entity.ToTable("Utilizador_Formacao");

                entity.Property(e => e.IdFormacao).HasColumnName("ID_Formacao");

                entity.Property(e => e.DataFormacao)
                    .HasColumnName("Data_Formacao")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Entidade)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdUtilizador).HasColumnName("ID_Utilizador");

                entity.Property(e => e.Observacao)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUtilizadorNavigation)
                    .WithMany(p => p.UtilizadorFormacao)
                    .HasForeignKey(d => d.IdUtilizador)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Utilizador_Formacao_Utilizador");
            });

            modelBuilder.Entity<UtilizadorHabilitacoes>(entity =>
            {
                entity.HasKey(e => e.IdHabilitacao);

                entity.ToTable("Utilizador_Habilitacoes");

                entity.Property(e => e.IdHabilitacao).HasColumnName("ID_Habilitacao");

                entity.Property(e => e.DataConclusao)
                    .HasColumnName("Data_Conclusao")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Entidade)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdUtilizador).HasColumnName("ID_Utilizador");

                entity.Property(e => e.Observacao)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUtilizadorNavigation)
                    .WithMany(p => p.UtilizadorHabilitacoes)
                    .HasForeignKey(d => d.IdUtilizador)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Utilizador_Habilitacoes_Utilizador");
            });

            modelBuilder.Entity<UtilizadorPermissao>(entity =>
            {
                entity.HasKey(e => e.IdUtilizadorPermissao)
                    .HasName("PK_Utilizador_Permissoes");

                entity.ToTable("Utilizador_Permissao");

                entity.Property(e => e.IdUtilizadorPermissao).HasColumnName("ID_Utilizador_Permissao");

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.UtilizadorPermissao)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Utilizador_Permissao_Utilizador");
            });

            modelBuilder.Entity<Versao>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Descricao).IsRequired();

                entity.Property(e => e.Versao1)
                    .IsRequired()
                    .HasColumnName("Versao")
                    .HasMaxLength(10);
            });
        }
    }
}
