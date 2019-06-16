using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    [ModelMetadataType(typeof(IMaintenanceOrder))]
    public partial class MaintenanceOrder
    {
        [NotMapped]
        public bool? IsPreventive
        {
            get { if (this.OrderType == "DMNALMP" || this.OrderType == "DMNCATE" || this.OrderType == "DMNLVMP" || this.OrderType == "DMNPRVE") {
                    return true;
                } if (this.OrderType == "DMNCREE" || this.OrderType == "DMNDBI" || this.OrderType == "DMNORCE") {
                    return false;
                }
                return null;
            }
        }
        
        [NotMapped]
        public bool IsToExecute
        {
            get { return !(this.DataFecho > new DateTime (1753,1,1)); }
        }

        [NotMapped]
        public List<Utilizador> Technicals;
    }



    public interface IMaintenanceOrder
    {
        byte[] Timestamp { get; set; }
        int DocumentType { get; set; }
        [Key]
        string No { get; set; }
        string Description { get; set; }
        int? ObjectRefType { get; set; }
        string ObjectRefNo { get; set; }
        string ObjectRefDescription { get; set; }
        string ComponentOf { get; set; }
        string OrderType { get; set; }
        string MaintenanceActivity { get; set; }
        int? SourceDocType { get; set; }
        string SourceDocNo { get; set; }
        string ContractNo { get; set; }
        int? Priority { get; set; }
        int? Status { get; set; }
        string SuspendedOrderReason { get; set; }
        string ResponsibilityCenter { get; set; }
        DateTime? LastDateModified { get; set; }
        string CustomerNo { get; set; }
        string CustomerName { get; set; }
        string CustomerName2 { get; set; }
        string CustomerAddress { get; set; }
        string CustomerAddress2 { get; set; }
        string CustomerCity { get; set; }
        string CustomerPostCode { get; set; }
        string CustomerPhoneNo { get; set; }
        string CustomerEMail { get; set; }
        string CustomerShipToCode { get; set; }
        string CustomerFaxNo { get; set; }
        string CustomerReference { get; set; }
        string CustomerContactName { get; set; }
        string CustomerCountryCode { get; set; }
        DateTime? PostingDate { get; set; }
        string CustomerCounty { get; set; }
        string JobNo { get; set; }
        int? ApplicationMethod { get; set; }
        string LanguageCode { get; set; }
        string ShortcutDimension1Code { get; set; }
        string ShortcutDimension2Code { get; set; }
        string RespCenterCountryCode { get; set; }
        decimal? TotalQuantity { get; set; }
        decimal? TotalQtyToInvoice { get; set; }
        string RespCenterName { get; set; }
        string RespCenterName2 { get; set; }
        string RespCenterFaxNo { get; set; }
        string RespCenterCounty { get; set; }
        string RespCenterAddress { get; set; }
        string RespCenterAddress2 { get; set; }
        string RespCenterPostCode { get; set; }
        string RespCenterCity { get; set; }
        string RespCenterContact { get; set; }
        string RespCenterPhoneNo { get; set; }
        string RespCenterReference { get; set; }
        string FaNo { get; set; }
        string FlNo { get; set; }
        string FlDescription { get; set; }
        string ResponsibleEmployee { get; set; }
        string EnteredBy { get; set; }
        string MaintenanceResponsible { get; set; }
        string PlannerGroupNo { get; set; }
        DateTime? OrderDate { get; set; }
        DateTime? OrderTime { get; set; }
        DateTime? DocumentDate { get; set; }
        DateTime? ExpectedFinishingDate { get; set; }
        DateTime? ExpectedFinishingTime { get; set; }
        DateTime? ExpectedStartingDate { get; set; }
        DateTime? ExpectedStartingTime { get; set; }
        DateTime? StartingDate { get; set; }
        DateTime? StartingTime { get; set; }
        decimal? ResponseTimeHours { get; set; }
        decimal? MaintenanceTimeHours { get; set; }
        DateTime? FinishingDate { get; set; }
        DateTime? FinishingTime { get; set; }
        string GenBusPostingGroup { get; set; }
        string CustomerPriceGroup { get; set; }
        string CustomerDiscGroup { get; set; }
        string VatRegistrationNo { get; set; }
        string PurchaserCode { get; set; }
        string PlannedOrderNo { get; set; }
        string NoSeries { get; set; }
        int? Reserve { get; set; }
        string Validade { get; set; }
        byte? Budget { get; set; }
        string FaPostingGroup { get; set; }
        string WorkCenterNo { get; set; }
        string MachineCenterNo { get; set; }
        decimal? FinishingTimeHours { get; set; }
        string TipoContactoCliente { get; set; }
        string CustomerDocNo { get; set; }
        string JobPostingGroup { get; set; }
        string ShipToCode { get; set; }
        string ShipToName { get; set; }
        string ShipToName2 { get; set; }
        string ShipToAddress { get; set; }
        string ShipToAddress2 { get; set; }
        string ShipToPostCode { get; set; }
        string ShipToCity { get; set; }
        string ShipToCounty { get; set; }
        string ShipToContact { get; set; }
        string ShortcutDimension3Code { get; set; }
        string ShortcutDimension4Code { get; set; }
        DateTime? DataFecho { get; set; }
        DateTime? HoraFecho { get; set; }
        string Loc1 { get; set; }
        int? EstadoOrcamento { get; set; }
        string NumOrdem { get; set; }
        string NoDocumentoEnviado { get; set; }
        string FormaDeEnvio { get; set; }
        DateTime? DataDeEnvio { get; set; }
        DateTime? DataEntrada { get; set; }
        string NºGeste { get; set; }
        DateTime? DataEntrega { get; set; }
        DateTime? DataSaída { get; set; }
        int? OrigemOrdem { get; set; }
        string Loc2 { get; set; }
        string Loc3 { get; set; }
        int? Urgência { get; set; }
        int? PrioridadeObra { get; set; }
        byte? FechoTécnicoObra { get; set; }
        string PrazoDeExecuçãoDaOrdem { get; set; }
        string Descrição1 { get; set; }
        string Descrição2 { get; set; }
        string Descrição3 { get; set; }
        decimal? ValorTotalPrev { get; set; }
        decimal? TotalQPrev { get; set; }
        decimal? TotalQReal { get; set; }
        int? NºLinhaContrato { get; set; }
        DateTime? DataReabertura { get; set; }
        DateTime? HoraReabertura { get; set; }
        string NºAntigoAs400 { get; set; }
        decimal? ValorFacturado { get; set; }
        string ObjectoManutençãoAs400 { get; set; }
        decimal? TotalQuantidadeReal { get; set; }
        decimal? ValorCustoRealTotal { get; set; }
        string ClienteContrato { get; set; }
        decimal? TotalQuantidadeFact { get; set; }
        decimal? TotalValorFact { get; set; }
        decimal? PMargem { get; set; }
        decimal? Margem { get; set; }
        string FTextDescDim1 { get; set; }
        string Cc { get; set; }
        string Paginas { get; set; }
        string De { get; set; }
        string Compensa { get; set; }
        string NãoCompensa { get; set; }
        byte? ObraReclamada { get; set; }
        int? NºReclamacao { get; set; }
        string DescricaoReclamacao { get; set; }
        DateTime? DataPedidoReparação { get; set; }
        DateTime? HoraPedidoReparação { get; set; }
        string FechadoPor { get; set; }
        string ReabertoPor { get; set; }
        string Dimension2CodeOld { get; set; }
        byte? MensagemImpressoOrdem { get; set; }
        string NovaReconv { get; set; }
        string ObjectoServiço { get; set; }
        DateTime? DataPedido { get; set; }
        DateTime? DataValidade { get; set; }
        DateTime? ValidadePedido { get; set; }
        decimal? ValorProjecto { get; set; }
        string DeliberaçãoCa { get; set; }
        byte? ServInternosRequisições { get; set; }
        byte? ServInternosFolhasDeObra { get; set; }
        byte? ServInternosDébInternos { get; set; }
        byte? MãoDeObraEDeslocações { get; set; }
        string ConfigResponsavel { get; set; }
        DateTime? DataUltimoMail { get; set; }
        string UserChefeProjecto { get; set; }
        DateTime? DataChefeProjecto { get; set; }
        string UserResponsavel { get; set; }
        DateTime? DataResponsavel { get; set; }
        DateTime? DataFacturação { get; set; }
        string TécnicoExecutante { get; set; }
        string NoCompromisso { get; set; }
        string NoDocumentoContactoInicial { get; set; }
        string TipoContactoClienteInicial { get; set; }
        string LocalAec { get; set; }
        int? Contrato { get; set; }
        int? IdClienteEvolution { get; set; }
        int? IdInstituicaoEvolution { get; set; }
        int? IdServicoEvolution { get; set; }
        int? IdTecnico1 { get; set; }
        int? IdTecnico2 { get; set; }
        int? IdTecnico3 { get; set; }
        int? IdTecnico4 { get; set; }
        int? IdTecnico5 { get; set; }
        int? GeradaAuto { get; set; }
        string ReferenciaEncomenda { get; set; }
        DateTime? DataEncomenda { get; set; }
    }
}
