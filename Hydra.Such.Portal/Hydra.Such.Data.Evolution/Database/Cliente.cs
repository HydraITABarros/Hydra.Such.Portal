using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class Cliente
    {
        public Cliente()
        {
            Contactos = new HashSet<Contactos>();
            Contrato = new HashSet<Contrato>();
            EquipPimp = new HashSet<EquipPimp>();
            Equipamento = new HashSet<Equipamento>();
            Instituicao = new HashSet<Instituicao>();
            OrdemManutencao = new HashSet<OrdemManutencao>();
            OrdemManutencaoEquipamentos = new HashSet<OrdemManutencaoEquipamentos>();
        }

        public int IdCliente { get; set; }
        public string Nome { get; set; }
        public string CodNavision { get; set; }
        public string Nif { get; set; }
        public string Morada { get; set; }
        public string Contacto { get; set; }
        public int? IdRegiao { get; set; }
        public bool? Activo { get; set; }
        public string RegiaoNav { get; set; }
        public byte? ClienteAssociado { get; set; }
        public byte? ClienteInterno { get; set; }
        public byte? ClienteNacional { get; set; }
        public string AssociadoAN { get; set; }
        public int? TipoCliente { get; set; }
        public int? NaturezaCliente { get; set; }
        public byte? ActivoManut { get; set; }
        public string CrespNav { get; set; }
        public int? Blocked { get; set; }

        public Regiao IdRegiaoNavigation { get; set; }
        public LogoClientes LogoClientes { get; set; }
        public ICollection<Contactos> Contactos { get; set; }
        public ICollection<Contrato> Contrato { get; set; }
        public ICollection<EquipPimp> EquipPimp { get; set; }
        public ICollection<Equipamento> Equipamento { get; set; }
        public ICollection<Instituicao> Instituicao { get; set; }
        public ICollection<OrdemManutencao> OrdemManutencao { get; set; }
        public ICollection<OrdemManutencaoEquipamentos> OrdemManutencaoEquipamentos { get; set; }
    }
}
