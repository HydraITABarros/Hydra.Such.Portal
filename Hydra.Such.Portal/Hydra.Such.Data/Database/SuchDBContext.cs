using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Hydra.Such.Data.Database
{
    public partial class SuchDBContext : DbContext
    {
        public virtual DbSet<AcessosDimensões> AcessosDimensões { get; set; }
        public virtual DbSet<AcessosPerfil> AcessosPerfil { get; set; }
        public virtual DbSet<AcessosUtilizador> AcessosUtilizador { get; set; }
        public virtual DbSet<AçõesDeConfeção> AçõesDeConfeção { get; set; }
        public virtual DbSet<Anexos> Anexos { get; set; }
        public virtual DbSet<AutorizarFaturaçãoContratos> AutorizarFaturaçãoContratos { get; set; }
        public virtual DbSet<BarramentosDeVoz> BarramentosDeVoz { get; set; }
        public virtual DbSet<CafetariasRefeitórios> CafetariasRefeitórios { get; set; }
        public virtual DbSet<CartõesEApólices> CartõesEApólices { get; set; }
        public virtual DbSet<CartõesTelemóveis> CartõesTelemóveis { get; set; }
        public virtual DbSet<CatálogoManutenção> CatálogoManutenção { get; set; }
        public virtual DbSet<ClassificaçãoFichasTécnicas> ClassificaçãoFichasTécnicas { get; set; }
        public virtual DbSet<Configuração> Configuração { get; set; }
        public virtual DbSet<ConfiguraçãoAprovações> ConfiguraçãoAprovações { get; set; }
        public virtual DbSet<ConfiguraçãoNumerações> ConfiguraçãoNumerações { get; set; }
        public virtual DbSet<ConfiguraçãoTemposCcp> ConfiguraçãoTemposCcp { get; set; }
        public virtual DbSet<ConfigUtilizadores> ConfigUtilizadores { get; set; }
        public virtual DbSet<Contactos> Contactos { get; set; }
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
        public virtual DbSet<EmailsProcedimentosCcp> EmailsProcedimentosCcp { get; set; }
        public virtual DbSet<FichasTécnicasPratos> FichasTécnicasPratos { get; set; }
        public virtual DbSet<FluxoTrabalhoListaControlo> FluxoTrabalhoListaControlo { get; set; }
        public virtual DbSet<FolhasDeHoras> FolhasDeHoras { get; set; }
        public virtual DbSet<GruposAprovação> GruposAprovação { get; set; }
        public virtual DbSet<Instrutores> Instrutores { get; set; }
        public virtual DbSet<LinhasContratos> LinhasContratos { get; set; }
        public virtual DbSet<LinhasFaturaçãoContrato> LinhasFaturaçãoContrato { get; set; }
        public virtual DbSet<LinhasFichasTécnicasPratos> LinhasFichasTécnicasPratos { get; set; }
        public virtual DbSet<LinhasPEncomendaProcedimentosCcp> LinhasPEncomendaProcedimentosCcp { get; set; }
        public virtual DbSet<LinhasPréRequisição> LinhasPréRequisição { get; set; }
        public virtual DbSet<LinhasRequisição> LinhasRequisição { get; set; }
        public virtual DbSet<LinhasRequisiçõesSimplificadas> LinhasRequisiçõesSimplificadas { get; set; }
        public virtual DbSet<Locais> Locais { get; set; }
        public virtual DbSet<MãoDeObraFolhaDeHoras> MãoDeObraFolhaDeHoras { get; set; }
        public virtual DbSet<Marcas> Marcas { get; set; }
        public virtual DbSet<Modelos> Modelos { get; set; }
        public virtual DbSet<MovimentosCafetariaRefeitório> MovimentosCafetariaRefeitório { get; set; }
        public virtual DbSet<MovimentosDeAprovação> MovimentosDeAprovação { get; set; }
        public virtual DbSet<MovimentosTelefones> MovimentosTelefones { get; set; }
        public virtual DbSet<MovimentosTelemóveis> MovimentosTelemóveis { get; set; }
        public virtual DbSet<MovimentosViaturas> MovimentosViaturas { get; set; }
        public virtual DbSet<NotasProcedimentosCcp> NotasProcedimentosCcp { get; set; }
        public virtual DbSet<ObjetosDeServiço> ObjetosDeServiço { get; set; }
        public virtual DbSet<OrigemDestinoFh> OrigemDestinoFh { get; set; }
        public virtual DbSet<LinhasFolhaHoras> LinhasFolhaHoras { get; set; }
        public virtual DbSet<PercursosEAjudasCustoDespesasFolhaDeHoras> PercursosEAjudasCustoDespesasFolhaDeHoras { get; set; }
        public virtual DbSet<PerfisModelo> PerfisModelo { get; set; }
        public virtual DbSet<PerfisUtilizador> PerfisUtilizador { get; set; }
        public virtual DbSet<PreçosFornecedor> PreçosFornecedor { get; set; }
        public virtual DbSet<PréRequisição> PréRequisição { get; set; }
        public virtual DbSet<PresençasFolhaDeHoras> PresençasFolhaDeHoras { get; set; }
        public virtual DbSet<ProcedimentosCcp> ProcedimentosCcp { get; set; }
        public virtual DbSet<ProcedimentosDeConfeção> ProcedimentosDeConfeção { get; set; }
        public virtual DbSet<ProcessosDisciplinaresInquérito> ProcessosDisciplinaresInquérito { get; set; }
        public virtual DbSet<Projetos> Projetos { get; set; }
        public virtual DbSet<ProjetosFaturação> ProjetosFaturação { get; set; }
        public virtual DbSet<RegistoDeAtas> RegistoDeAtas { get; set; }
        public virtual DbSet<Requisição> Requisição { get; set; }
        public virtual DbSet<RequisiçõesClienteContrato> RequisiçõesClienteContrato { get; set; }
        public virtual DbSet<RequisiçõesSimplificadas> RequisiçõesSimplificadas { get; set; }
        public virtual DbSet<Serviços> Serviços { get; set; }
        public virtual DbSet<ServiçosCliente> ServiçosCliente { get; set; }
        public virtual DbSet<Tarifários> Tarifários { get; set; }
        public virtual DbSet<Telefones> Telefones { get; set; }
        public virtual DbSet<Telemóveis> Telemóveis { get; set; }
        public virtual DbSet<TemposPaCcp> TemposPaCcp { get; set; }
        public virtual DbSet<TextoFaturaContrato> TextoFaturaContrato { get; set; }
        public virtual DbSet<TipoDeProjeto> TipoDeProjeto { get; set; }
        public virtual DbSet<TiposGrupoContabOmProjeto> TiposGrupoContabOmProjeto { get; set; }
        public virtual DbSet<TiposGrupoContabProjeto> TiposGrupoContabProjeto { get; set; }
        public virtual DbSet<TiposRefeição> TiposRefeição { get; set; }
        public virtual DbSet<TiposRequisições> TiposRequisições { get; set; }
        public virtual DbSet<TiposViatura> TiposViatura { get; set; }
        public virtual DbSet<UnidadesProdutivas> UnidadesProdutivas { get; set; }
        public virtual DbSet<UtilizadoresGruposAprovação> UtilizadoresGruposAprovação { get; set; }
        public virtual DbSet<Viaturas> Viaturas { get; set; }
        public virtual DbSet<WorkflowProcedimentosCcp> WorkflowProcedimentosCcp { get; set; }

        // Unable to generate entity type for table 'dbo.Autorizacao_FH_RH'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Tabela_Conf_Recursos_FH'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"data source=10.101.1.10\SQLNAVDEV;initial catalog=PlataformaOperacionalSUCH_TST;user id=such_portal_user;password=SuchPW.2K17;");
                
            }
        }

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
                entity.HasKey(e => new { e.IdUtilizador, e.Área, e.Funcionalidade });

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

                entity.HasOne(d => d.IdUtilizadorNavigation)
                    .WithMany(p => p.AcessosUtilizador)
                    .HasForeignKey(d => d.IdUtilizador)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Acessos Utilizador_Config. Utilizadores");
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

                entity.HasOne(d => d.NºOrigem1)
                    .WithMany(p => p.Anexos)
                    .HasForeignKey(d => d.NºOrigem)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Anexos_Requisição");
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

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.HasOne(d => d.NºProjetoNavigation)
                    .WithMany(p => p.CafetariasRefeitórios)
                    .HasForeignKey(d => d.NºProjeto)
                    .HasConstraintName("FK_Cafetarias/Refeitórios_Projetos");

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

            modelBuilder.Entity<Configuração>(entity =>
            {
                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.NumeraçãoContactos).HasColumnName("Numeração Contactos");

                entity.Property(e => e.NumeraçãoContratos).HasColumnName("Numeração Contratos");

                entity.Property(e => e.NumeraçãoFolhasDeHoras).HasColumnName("Numeração Folhas de Horas");

                entity.Property(e => e.NumeraçãoOportunidades).HasColumnName("Numeração Oportunidades");

                entity.Property(e => e.NumeraçãoProcedimentoAquisição).HasColumnName("Numeração Procedimento Aquisição");

                entity.Property(e => e.NumeraçãoProcedimentoSimplificado).HasColumnName("Numeração Procedimento Simplificado");

                entity.Property(e => e.NumeraçãoProjetos).HasColumnName("Numeração Projetos");

                entity.Property(e => e.NumeraçãoPropostas).HasColumnName("Numeração Propostas");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

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

            modelBuilder.Entity<ConfiguraçãoAprovações>(entity =>
            {
                entity.ToTable("Configuração Aprovações");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
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

            modelBuilder.Entity<ConfigUtilizadores>(entity =>
            {
                entity.HasKey(e => e.IdUtilizador);

                entity.ToTable("Config. Utilizadores");

                entity.Property(e => e.IdUtilizador)
                    .HasColumnName("Id Utilizador")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

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

            modelBuilder.Entity<Contactos>(entity =>
            {
                entity.HasKey(e => e.Nº);

                entity.Property(e => e.Nº)
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
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
                    .HasColumnType("text");

                entity.Property(e => e.DuraçãoMáxContrato)
                    .HasColumnName("Duração Máx. Contrato")
                    .HasColumnType("date");

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

                entity.Property(e => e.EstadoAlteração).HasColumnName("Estado Alteração");

                entity.Property(e => e.JuntarFaturas).HasColumnName("Juntar Faturas");

                entity.Property(e => e.LinhasContratoEmFact).HasColumnName("Linhas Contrato em Fact.");

                entity.Property(e => e.LocalArquivoFísico)
                    .HasColumnName("Local Arquivo Físico")
                    .HasMaxLength(50);

                entity.Property(e => e.Mc).HasColumnName("% MC");

                entity.Property(e => e.Notas).HasColumnType("text");

                entity.Property(e => e.NumeraçãoInterna)
                    .HasColumnName("Numeração Interna")
                    .HasMaxLength(20);

                entity.Property(e => e.NºCliente)
                    .HasColumnName("Nº Cliente")
                    .HasMaxLength(20);

                entity.Property(e => e.NºComprimissoObrigatório).HasColumnName("Nº Comprimisso Obrigatório");

                entity.Property(e => e.NºCompromisso)
                    .HasColumnName("Nº Compromisso")
                    .HasMaxLength(20);

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

                entity.Property(e => e.ObjetoServiço).HasColumnName("Objeto Serviço");

                entity.Property(e => e.OrigemDoPedido).HasColumnName("Origem do Pedido");

                entity.Property(e => e.PeríodoFatura).HasColumnName("Período Fatura");

                entity.Property(e => e.PróximaDataFatura)
                    .HasColumnName("Próxima Data Fatura")
                    .HasColumnType("date");

                entity.Property(e => e.PróximoPeríodoFact)
                    .HasColumnName("Próximo Período Fact.")
                    .HasMaxLength(30);

                entity.Property(e => e.Referência1ºContrato)
                    .HasColumnName("Referência 1º Contrato")
                    .HasMaxLength(30);

                entity.Property(e => e.ReferênciaContrato)
                    .HasColumnName("Referência Contrato")
                    .HasMaxLength(20);

                entity.Property(e => e.RescisãoPrazoAviso).HasColumnName("Rescisão (Prazo Aviso)");

                entity.Property(e => e.TaxaAprovisionamento).HasColumnName("Taxa Aprovisionamento");

                entity.Property(e => e.TaxaDeslocação).HasColumnName("Taxa Deslocação");

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

                entity.Property(e => e.ValorTotalProposta).HasColumnName("Valor Total Proposta");

                entity.Property(e => e.ÚltimaDataFatura)
                    .HasColumnName("Última Data Fatura")
                    .HasColumnType("date");

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

                entity.Property(e => e.CustoUnitário).HasColumnName("Custo Unitário");

                entity.Property(e => e.CódDestinoFinalResíduos).HasColumnName("Cód. Destino Final Resíduos");

                entity.Property(e => e.CódGrupoServiço).HasColumnName("Cód. Grupo Serviço");

                entity.Property(e => e.CódLocalização)
                    .HasColumnName("Cód. Localização")
                    .HasMaxLength(10);

                entity.Property(e => e.CódServiçoCliente).HasColumnName("Cód Serviço Cliente");

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

                entity.Property(e => e.PreçoUnitário).HasColumnName("Preço Unitário");

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

                entity.HasOne(d => d.Nº)
                    .WithMany(p => p.DiárioDeProjeto)
                    .HasForeignKey(d => new { d.NºRequisição, d.NºLinhaRequisição })
                    .HasConstraintName("FK_Diário de Projeto_Linhas Requisição");
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

                entity.Property(e => e.Matrícula).HasMaxLength(10);

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

                entity.Property(e => e.Descrição).HasMaxLength(100);

                entity.Property(e => e.NºFornecedor)
                    .HasColumnName("Nº Fornecedor")
                    .HasMaxLength(20);

                entity.Property(e => e.NºProduto)
                    .HasColumnName("Nº Produto")
                    .HasMaxLength(20);

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.NºUnidadeProdutiva).HasColumnName("Nº Unidade Produtiva");

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

                entity.Property(e => e.Valor).HasColumnName("% Valor");

                entity.Property(e => e.Valor1).HasColumnName("Valor");

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

                entity.Property(e => e.Grupo).HasMaxLength(20);

                entity.Property(e => e.Image).HasColumnType("image");

                entity.Property(e => e.NomeFichaTécnica)
                    .HasColumnName("Nome Ficha Técnica")
                    .HasMaxLength(60);

                entity.Property(e => e.NºDeDoses).HasColumnName("Nº de Doses");

                entity.Property(e => e.Observações).HasColumnType("text");

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

                entity.Property(e => e.VariaçãoPreçoCusto).HasColumnName("Variação Preço Custo");

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
                    .HasMaxLength(20);

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

                entity.HasOne(d => d.NºProjetoNavigation)
                    .WithMany(p => p.FolhasDeHoras)
                    .HasForeignKey(d => d.NºProjeto)
                    .HasConstraintName("FK_Folhas de Horas_Projetos");
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

                entity.Property(e => e.CódServiçoCliente).HasColumnName("Cód. Serviço Cliente");

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

                entity.Property(e => e.DescontoLinha).HasColumnName("% Desconto Linha");

                entity.Property(e => e.Descrição).HasMaxLength(100);

                entity.Property(e => e.GrupoFatura).HasColumnName("Grupo Fatura");

                entity.Property(e => e.NºHorasIntervenção).HasColumnName("Nº Horas Intervenção");

                entity.Property(e => e.NºResponsável)
                    .HasColumnName("Nº Responsável")
                    .HasMaxLength(20);

                entity.Property(e => e.NºTécnicos).HasColumnName("Nº Técnicos");

                entity.Property(e => e.PreçoUnitário).HasColumnName("Preço Unitário");

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

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoRegião)
                    .HasColumnName("Código Região")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoServiço).HasColumnName("Código Serviço");

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

                entity.HasOne(d => d.CódigoServiçoNavigation)
                    .WithMany(p => p.LinhasFaturaçãoContrato)
                    .HasForeignKey(d => d.CódigoServiço)
                    .HasConstraintName("FK_Linhas Faturação Contrato_Serviços");
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

                entity.HasOne(d => d.CódigoNavigation)
                    .WithMany(p => p.LinhasFichasTécnicasPratosCódigoNavigation)
                    .HasForeignKey(d => d.Código)
                    .HasConstraintName("FK_Linhas Fichas Técnicas Pratos_Fichas Técnicas Pratos1");

                entity.HasOne(d => d.NºPratoNavigation)
                    .WithMany(p => p.LinhasFichasTécnicasPratosNºPratoNavigation)
                    .HasForeignKey(d => d.NºPrato)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Linhas Fichas Técnicas Pratos_Fichas Técnicas Pratos");
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

                entity.HasOne(d => d.Nº)
                    .WithMany(p => p.LinhasPEncomendaProcedimentosCcp)
                    .HasForeignKey(d => new { d.NºRequisição, d.NºLinhaRequisição })
                    .HasConstraintName("FK_Linhas p/ Encomenda Procedimentos CCP_Linhas Requisição");
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

                entity.Property(e => e.Código).HasMaxLength(20);

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoLocalização)
                    .HasColumnName("Código Localização")
                    .HasMaxLength(10);

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

                entity.Property(e => e.Descrição).HasMaxLength(100);

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.QuantidadeARequerer).HasColumnName("Quantidade a Requerer");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

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

                entity.Property(e => e.CustoUnitário).HasColumnName("Custo Unitário");

                entity.Property(e => e.Código).HasMaxLength(20);

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoLocalização)
                    .HasColumnName("Código Localização")
                    .HasMaxLength(10);

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

                entity.Property(e => e.MercadoLocal).HasColumnName("Mercado Local");

                entity.Property(e => e.NºFuncionário)
                    .HasColumnName("Nº Funcionário")
                    .HasMaxLength(20);

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.QuantidadeADisponibilizar).HasColumnName("Quantidade a Disponibilizar");

                entity.Property(e => e.QuantidadeAReceber).HasColumnName("Quantidade a Receber");

                entity.Property(e => e.QuantidadeARequerer).HasColumnName("Quantidade a Requerer");

                entity.Property(e => e.QuantidadeDisponibilizada).HasColumnName("Quantidade Disponibilizada");

                entity.Property(e => e.QuantidadePendente).HasColumnName("Quantidade Pendente");

                entity.Property(e => e.QuantidadeRecebida).HasColumnName("Quantidade Recebida");

                entity.Property(e => e.QuantidadeRequerida).HasColumnName("Quantidade Requerida");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.Viatura).HasMaxLength(10);

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
                    .HasConstraintName("FK_Linhas Requisições Simplificadas_Requisições Simplificadas");

                entity.HasOne(d => d.TipoRefeiçãoNavigation)
                    .WithMany(p => p.LinhasRequisiçõesSimplificadas)
                    .HasForeignKey(d => d.TipoRefeição)
                    .HasConstraintName("FK_Linhas Requisições Simplificadas_Tipos Refeição");
            });

            modelBuilder.Entity<Locais>(entity =>
            {
                entity.HasKey(e => e.Código);

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descrição).HasMaxLength(50);

                entity.Property(e => e.Endereço).HasMaxLength(150);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);
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

                entity.Property(e => e.CodigoCentroResponsabilidade).HasMaxLength(20);

                entity.Property(e => e.CustoUnitárioDireto).HasColumnName("Custo Unitário Direto");

                entity.Property(e => e.CódUnidadeMedida)
                    .HasColumnName("Cód. Unidade Medida")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoFamíliaRecurso)
                    .HasColumnName("Código Família Recurso")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoTipoOm).HasColumnName("Código Tipo OM");

                entity.Property(e => e.CódigoTipoTrabalho).HasColumnName("Código Tipo Trabalho");

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

                entity.HasOne(d => d.NºProjetoNavigation)
                    .WithMany(p => p.MãoDeObraFolhaDeHoras)
                    .HasForeignKey(d => d.NºProjeto)
                    .HasConstraintName("FK_Mão de Obra Folha de Horas_Projetos");
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

                entity.Property(e => e.NºRecurso)
                    .HasColumnName("Nº Recurso")
                    .HasMaxLength(20);

                entity.Property(e => e.NºUnidadeProdutiva).HasColumnName("Nº Unidade Produtiva");

                entity.Property(e => e.TipoMovimento).HasColumnName("Tipo Movimento");

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

                entity.Property(e => e.DataHoraAprovação)
                    .HasColumnName("Data/Hora Aprovação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraCriação)
                    .HasColumnName("Data/Hora Criação")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataHoraModificação)
                    .HasColumnName("Data/Hora Modificação")
                    .HasColumnType("datetime");

                entity.Property(e => e.GrupoAprovador).HasColumnName("Grupo Aprovador");

                entity.Property(e => e.Número).HasMaxLength(20);

                entity.Property(e => e.UtilizadorAprovador)
                    .HasColumnName("Utilizador Aprovador")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorAprovação)
                    .HasColumnName("Utilizador Aprovação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorSolicitou)
                    .HasColumnName("Utilizador Solicitou")
                    .HasMaxLength(50);

                entity.HasOne(d => d.GrupoAprovadorNavigation)
                    .WithMany(p => p.MovimentosDeAprovação)
                    .HasForeignKey(d => d.GrupoAprovador)
                    .HasConstraintName("FK_Movimentos de Aprovação_Grupos Aprovação");
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

                entity.Property(e => e.Matrícula).HasMaxLength(10);

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

                entity.Property(e => e.Descrição).HasMaxLength(50);

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

            modelBuilder.Entity<LinhasFolhaHoras>(entity =>
            {
                entity.HasKey(e => new { e.NoFolhaHoras, e.NoLinha });

                entity.ToTable("Linhas Folha Horas");

                entity.Property(e => e.NoFolhaHoras)
                    .HasColumnName("No_Folha_Horas")
                    .HasMaxLength(20);

                entity.Property(e => e.NoLinha)
                    .HasColumnName("No_Linha")
                    .HasColumnType("int")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.TipoCusto)
                    .HasColumnName("Tipo_Custo")
                    .HasColumnType("int");

                entity.Property(e => e.CodTipoCusto)
                    .HasColumnName("Cod_Tipo_Custo")
                    .HasMaxLength(20);

                entity.Property(e => e.DescricaoTipoCusto)
                    .HasColumnName("Descricao_Tipo_Custo")
                    .HasMaxLength(200);

                entity.Property(e => e.Quantidade)
                    .HasColumnName("Quantidade")
                    .HasColumnType("decimal(, 20)");

                entity.Property(e => e.CustoUnitario)
                    .HasColumnName("Custo_Unitario")
                    .HasColumnType("decimal(, 20)");

                entity.Property(e => e.CustoTotal)
                    .HasColumnName("Custo_Total")
                    .HasColumnType("decimal(, 20)");

                entity.Property(e => e.PrecoUnitario)
                    .HasColumnName("Preco_Unitario")
                    .HasColumnType("decimal(, 20)");

                entity.Property(e => e.PrecoVenda)
                    .HasColumnName("Preco_Venda")
                    .HasColumnType("decimal(, 20)");

                entity.Property(e => e.CodOrigem)
                    .HasColumnName("Cod_Origem")
                    .HasMaxLength(20);

                entity.Property(e => e.DescricaoOrigem)
                    .HasColumnName("Descricao_Origem")
                    .HasMaxLength(200);

                entity.Property(e => e.CodDestino)
                    .HasColumnName("Cod_Destino")
                    .HasMaxLength(20);

                entity.Property(e => e.DescricaoDestino)
                    .HasColumnName("Descricao_Destino")
                    .HasMaxLength(200);

                entity.Property(e => e.Distancia)
                    .HasColumnName("Distancia")
                    .HasColumnType("decimal(, 20)");

                entity.Property(e => e.DistanciaPrevista)
                    .HasColumnName("Distancia_Prevista")
                    .HasColumnType("decimal(, 20)");

                entity.Property(e => e.RubricaSalarial)
                    .HasColumnName("Rubrica_Salarial")
                    .HasMaxLength(20);

                entity.Property(e => e.RegistarSubsidiosPremios)
                    .HasColumnName("Registar_Subsidios_Premios");

                entity.Property(e => e.Observacao)
                    .HasColumnName("Observacao")
                    .HasMaxLength(200);

                entity.Property(e => e.RubricaSalarial2)
                    .HasColumnName("Rubrica_Salarial2")
                    .HasMaxLength(20);

                entity.Property(e => e.DataDespesa)
                    .HasColumnName("Data_Despesa")
                    .HasColumnType("datetime");

                entity.Property(e => e.Funcionario)
                    .HasColumnName("Funcionario")
                    .HasMaxLength(20);

                entity.Property(e => e.CodRegiao)
                    .HasColumnName("Cod_Regiao")
                    .HasMaxLength(20);

                entity.Property(e => e.CodArea)
                    .HasColumnName("Cod_Area")
                    .HasMaxLength(20);

                entity.Property(e => e.CodCresp)
                    .HasColumnName("Cod_Cresp")
                    .HasMaxLength(20);

                entity.Property(e => e.CalculoAutomatico)
                    .HasColumnName("Calculo_Automatico");

                entity.Property(e => e.Matricula)
                    .HasColumnName("Matricula")
                    .HasMaxLength(20);

                entity.Property(e => e.UtilizadorCriacao)
                    .HasColumnName("Utilizador_Criacao")
                    .HasMaxLength(20);

                entity.Property(e => e.DataHoraCriacao)
                    .HasColumnName("Data_Hora_Criacao")
                    .HasColumnType("datetime");

                entity.Property(e => e.UtilizadorModificacao)
                    .HasColumnName("Utilizador_Modificacao")
                    .HasMaxLength(20);

                entity.Property(e => e.DataHoraModificacao)
                    .HasColumnName("Data_Hora_Modificacao")
                    .HasColumnType("datetime");
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

            modelBuilder.Entity<PréRequisição>(entity =>
            {
                entity.HasKey(e => e.NºPréRequisição);

                entity.ToTable("Pré-Requisição");

                entity.Property(e => e.NºPréRequisição)
                    .HasColumnName("Nº Pré-Requisição")
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.CompraADinheiro).HasColumnName("Compra a Dinheiro");

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoLocalEntrega).HasColumnName("Código Local Entrega");

                entity.Property(e => e.CódigoLocalRecolha).HasColumnName("Código Local Recolha");

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

                entity.Property(e => e.ModeloDePréRequisição).HasColumnName("Modelo de Pré-Requisição");

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.Observações).HasColumnType("text");

                entity.Property(e => e.TipoRequisição).HasColumnName("Tipo Requisição");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

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

                entity.Property(e => e.Hora1ªEntrada).HasColumnName("Hora 1ª Entrada");

                entity.Property(e => e.Hora1ªSaída).HasColumnName("Hora 1ª Saída");

                entity.Property(e => e.Hora2ªEntrada).HasColumnName("Hora 2ª Entrada");

                entity.Property(e => e.Hora2ªSaída).HasColumnName("Hora 2ª Saída");

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
            });

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
                    .HasMaxLength(20);

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

            modelBuilder.Entity<Requisição>(entity =>
            {
                entity.HasKey(e => e.NºRequisição);

                entity.Property(e => e.NºRequisição)
                    .HasColumnName("Nº Requisição")
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.CompraADinheiro).HasColumnName("Compra a Dinheiro");

                entity.Property(e => e.CódigoCentroResponsabilidade)
                    .HasColumnName("Código Centro Responsabilidade")
                    .HasMaxLength(20);

                entity.Property(e => e.CódigoLocalEntrega).HasColumnName("Código Local Entrega");

                entity.Property(e => e.CódigoLocalRecolha).HasColumnName("Código Local Recolha");

                entity.Property(e => e.CódigoLocalização)
                    .HasColumnName("Código Localização")
                    .HasMaxLength(10);

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

                entity.Property(e => e.DataReceção)
                    .HasColumnName("Data Receção")
                    .HasColumnType("date");

                entity.Property(e => e.ModeloDeRequisição).HasColumnName("Modelo de Requisição");

                entity.Property(e => e.NºFuncionário)
                    .HasColumnName("Nº Funcionário")
                    .HasMaxLength(20);

                entity.Property(e => e.NºProjeto)
                    .HasColumnName("Nº Projeto")
                    .HasMaxLength(20);

                entity.Property(e => e.Observações).HasColumnType("text");

                entity.Property(e => e.UtilizadorCriação)
                    .HasColumnName("Utilizador Criação")
                    .HasMaxLength(50);

                entity.Property(e => e.UtilizadorModificação)
                    .HasColumnName("Utilizador Modificação")
                    .HasMaxLength(50);

                entity.Property(e => e.Viatura).HasMaxLength(10);

                entity.HasOne(d => d.NºProjetoNavigation)
                    .WithMany(p => p.Requisição)
                    .HasForeignKey(d => d.NºProjeto)
                    .HasConstraintName("FK_Requisição_Projetos");

                entity.HasOne(d => d.ViaturaNavigation)
                    .WithMany(p => p.Requisição)
                    .HasForeignKey(d => d.Viatura)
                    .HasConstraintName("FK_Requisição_Viaturas");
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
                    .HasMaxLength(20);

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

                entity.Property(e => e.ValorFatura).HasColumnName("Valor Fatura");
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
                    .HasColumnType("date");

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

            modelBuilder.Entity<Serviços>(entity =>
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

            modelBuilder.Entity<ServiçosCliente>(entity =>
            {
                entity.HasKey(e => new { e.NºCliente, e.CódServiço });

                entity.ToTable("Serviços Cliente");

                entity.Property(e => e.NºCliente)
                    .HasColumnName("Nº Cliente")
                    .HasMaxLength(20);

                entity.Property(e => e.CódServiço).HasColumnName("Cód. Serviço");

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

            modelBuilder.Entity<Viaturas>(entity =>
            {
                entity.HasKey(e => e.Matrícula);

                entity.Property(e => e.Matrícula)
                    .HasMaxLength(10)
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

                entity.Property(e => e.Imagem).HasColumnType("image");

                entity.Property(e => e.IntervaloRevisões).HasColumnName("Intervalo Revisões");

                entity.Property(e => e.KmUltimaRevisão).HasColumnName("Km Ultima Revisão");

                entity.Property(e => e.LocalParqueamento)
                    .HasColumnName("Local Parqueamento")
                    .HasMaxLength(80);

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
        }
    }
}
