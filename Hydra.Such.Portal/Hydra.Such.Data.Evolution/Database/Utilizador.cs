using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class Utilizador
    {
        public Utilizador()
        {
            ChatIdUserDestinoNavigation = new HashSet<Chat>();
            ChatIdUserEnvioNavigation = new HashSet<Chat>();
            ChatUsersOn = new HashSet<ChatUsersOn>();
            ClientePimpIdUtilizadorAlteracaoNavigation = new HashSet<ClientePimp>();
            ClientePimpIdUtilizadorInsercaoNavigation = new HashSet<ClientePimp>();
            ClientePimpTecnicoNavigation = new HashSet<ClientePimp>();
            EquipPimpIdUtilizadorAlteracaoNavigation = new HashSet<EquipPimp>();
            EquipPimpTecnicoNavigation = new HashSet<EquipPimp>();
            OrdemManutencao = new HashSet<OrdemManutencao>();
            UtilizadorCompetencias = new HashSet<UtilizadorCompetencias>();
            UtilizadorFormacao = new HashSet<UtilizadorFormacao>();
            UtilizadorHabilitacoes = new HashSet<UtilizadorHabilitacoes>();
            UtilizadorPermissao = new HashSet<UtilizadorPermissao>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string TelefoneGeral { get; set; }
        public string TelefoneExtensao { get; set; }
        public string Telemovel { get; set; }
        public int? NivelAcesso { get; set; }
        public bool? Activo { get; set; }
        public string Code1 { get; set; }
        public string Code2 { get; set; }
        public string Code3 { get; set; }
        public string NumMec { get; set; }
        public bool? AcessoAdministracao { get; set; }
        public int? ModuloFolhaObra { get; set; }
        public int? ModuloFichaEquip { get; set; }
        public int? ModuloRequisicoes { get; set; }
        public int? ModuloRegistoDiario { get; set; }
        public int? ModuloContratos { get; set; }
        public int? ModuloPlaneamento { get; set; }
        public bool? ValidadorEquipamento { get; set; }
        public int? ModuloClientes { get; set; }
        public int? ModuloFornecedores { get; set; }
        public int? ModuloInstituicoes { get; set; }
        public int? ModuloServicos { get; set; }
        public int? ModuloDadosEstatisticos { get; set; }
        public int? ModuloHabilitacoes { get; set; }
        public int? ModuloFormacoesCompetencias { get; set; }
        public int? ModuloUtilizadores { get; set; }
        public int? SuperiorHierarquico { get; set; }
        public int? ModuloReplicarPlaneamento { get; set; }
        public int? ModuloEmm { get; set; }
        public int? ModuloSolicitacoes { get; set; }
        public bool? EquipaFixa { get; set; }
        public int? IdEquipa { get; set; }
        public int? IdCliente { get; set; }
        public bool? AutorizarFacturar { get; set; }
        public bool? ChefeProjecto { get; set; }
        public bool? ResponsavelProjecto { get; set; }
        public string UserRespProjecto { get; set; }

        public NivelAcessoTipo NivelAcessoNavigation { get; set; }
        public ICollection<Chat> ChatIdUserDestinoNavigation { get; set; }
        public ICollection<Chat> ChatIdUserEnvioNavigation { get; set; }
        public ICollection<ChatUsersOn> ChatUsersOn { get; set; }
        public ICollection<ClientePimp> ClientePimpIdUtilizadorAlteracaoNavigation { get; set; }
        public ICollection<ClientePimp> ClientePimpIdUtilizadorInsercaoNavigation { get; set; }
        public ICollection<ClientePimp> ClientePimpTecnicoNavigation { get; set; }
        public ICollection<EquipPimp> EquipPimpIdUtilizadorAlteracaoNavigation { get; set; }
        public ICollection<EquipPimp> EquipPimpTecnicoNavigation { get; set; }
        public ICollection<OrdemManutencao> OrdemManutencao { get; set; }
        public ICollection<UtilizadorCompetencias> UtilizadorCompetencias { get; set; }
        public ICollection<UtilizadorFormacao> UtilizadorFormacao { get; set; }
        public ICollection<UtilizadorHabilitacoes> UtilizadorHabilitacoes { get; set; }
        public ICollection<UtilizadorPermissao> UtilizadorPermissao { get; set; }
    }
}
