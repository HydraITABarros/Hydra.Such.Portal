using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hydra.Such.Data.Evolution.Database
{
    [ModelMetadataType(typeof(IMaintenanceOrder))]
    using System;
    using System.Collections.Generic;

    namespace Hydra.Such.Data.Evolution.Database
    {
        [ModelMetadataType(typeof(IUtilizador))]
        public partial class Utilizador
        {
           
        }
    }


    public interface IUtilizador
    {
        int Id { get; set; }
        string Nome { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string Email { get; set; }
        string TelefoneGeral { get; set; }
        string TelefoneExtensao { get; set; }
        string Telemovel { get; set; }
        int? NivelAcesso { get; set; }
        bool? Activo { get; set; }
        string Code1 { get; set; }
        string Code2 { get; set; }
        string Code3 { get; set; }
        string NumMec { get; set; }
        bool? AcessoAdministracao { get; set; }
        int? ModuloFolhaObra { get; set; }
        int? ModuloFichaEquip { get; set; }
        int? ModuloRequisicoes { get; set; }
        int? ModuloRegistoDiario { get; set; }
        int? ModuloContratos { get; set; }
        int? ModuloPlaneamento { get; set; }
        bool? ValidadorEquipamento { get; set; }
        int? ModuloClientes { get; set; }
        int? ModuloFornecedores { get; set; }
        int? ModuloInstituicoes { get; set; }
        int? ModuloServicos { get; set; }
        int? ModuloDadosEstatisticos { get; set; }
        int? ModuloHabilitacoes { get; set; }
        int? ModuloFormacoesCompetencias { get; set; }
        int? ModuloUtilizadores { get; set; }
        int? SuperiorHierarquico { get; set; }
        int? ModuloReplicarPlaneamento { get; set; }
        int? ModuloEmm { get; set; }
        int? ModuloSolicitacoes { get; set; }
        bool? EquipaFixa { get; set; }
        int? IdEquipa { get; set; }
        int? IdCliente { get; set; }
        bool? AutorizarFacturar { get; set; }
        bool? ChefeProjecto { get; set; }
        bool? ResponsavelProjecto { get; set; }
        string UserRespProjecto { get; set; }
    }
}
