using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class Contrato
    {
        public Contrato()
        {
            ClientePimp = new HashSet<ClientePimp>();
            ContratoAssTipoRequisito = new HashSet<ContratoAssTipoRequisito>();
            EquipPimp = new HashSet<EquipPimp>();
            Equipamento = new HashSet<Equipamento>();
            OrdemManutencao = new HashSet<OrdemManutencao>();
        }

        public string IdContrato { get; set; }
        public int? IdCliente { get; set; }
        public int? IdTipoContrato { get; set; }
        public int? IdEstado { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public DateTime? DataCancelamento { get; set; }
        public decimal? ValorMensal { get; set; }
        public decimal? ValorTotal { get; set; }
        public string Observacoes { get; set; }
        public string NomeContrato { get; set; }
        public bool? Activo { get; set; }
        public string RegiaoNav { get; set; }
        public string ClienteNav { get; set; }
        public string ClienteNavNumber { get; set; }
        public int ToleranciaContrato { get; set; }
        public int? NumVersao { get; set; }
        public string NumRequisicaoCliente { get; set; }
        public int? EstadoESuch { get; set; }
        public int? TipoFaturacao { get; set; }
        public int? TipoContratoManut { get; set; }
        public decimal? TaxaAprovisionamento { get; set; }
        public decimal? PercentagemMc { get; set; }
        public int? PeriodoFatura { get; set; }
        public DateTime? DataUltimaFatura { get; set; }
        public bool? ContratoAvencaFixa { get; set; }
        public decimal? ValorTotalProposta { get; set; }
        public string NumProposta { get; set; }
        public string NumCompromisso { get; set; }
        public int? EstadoAlteracao { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual ContratoEstado IdEstadoNavigation { get; set; }
        public virtual ContratoTipo IdTipoContratoNavigation { get; set; }
        public virtual ICollection<ClientePimp> ClientePimp { get; set; }
        public virtual ICollection<ContratoAssTipoRequisito> ContratoAssTipoRequisito { get; set; }
        public virtual ICollection<EquipPimp> EquipPimp { get; set; }
        public virtual ICollection<Equipamento> Equipamento { get; set; }
        public virtual ICollection<OrdemManutencao> OrdemManutencao { get; set; }
    }
}
