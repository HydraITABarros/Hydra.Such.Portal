﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using static Hydra.Such.Data.Enumerations;

namespace Hydra.Such.Data.ViewModel.Compras
{
    public class RequisitionViewModel : ErrorHandler
    {
        public string RequisitionNo { get; set; }
        public int? TipoReq { get; set; }
        public int? Area { get; set; }
        //public int? State { get; set; }
        public RequisitionStates? State { get; set; }
        public string StateText { get; set; }
        public string ProjectNo { get; set; }
        public string RegionCode { get; set; }
        public string FunctionalAreaCode { get; set; }
        public string CenterResponsibilityCode { get; set; }
        public string LocalCode { get; set; }
        public string EmployeeNo { get; set; }
        public string Vehicle { get; set; }
        public string ReceivedDate { get; set; }
        public string ReceivedDateHour { get; set; }
        public bool? Urgent { get; set; }
        public bool? Sample { get; set; }
        public bool? Attachment { get; set; }
        public bool? Immobilized { get; set; }
        public bool? BuyCash { get; set; }
        public int? LocalCollectionCode { get; set; }
        public int? LocalDeliveryCode { get; set; }
        public string Comments { get; set; }
        public string NoDocumento { get; set; }
        public string RejeicaoMotivo { get; set; }
        public bool? RequestModel { get; set; }
        public string CreateUser { get; set; }
        public string CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? RelatedSearches { get; set; }
        public bool? Exclusive { get; set; }
        public bool? AlreadyPerformed { get; set; }
        public bool? Equipment { get; set; }
        public bool? StockReplacement { get; set; }
        public bool? Reclamation { get; set; }
        public string RequestReclaimNo { get; set; }
        public string ResponsibleCreation { get; set; }
        public string ResponsibleApproval { get; set; }
        public string ResponsibleValidation { get; set; }
        public string ResponsibleReception { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string ApprovalDateText { get; set; }
        public string ApprovalDateHour { get; set; }
        public DateTime? ValidationDate { get; set; }
        public string ValidationDateText { get; set; }
        public string ValidationDateHour { get; set; }
        public string UnitFoodProduction  { get; set; }
        public bool? RequestNutrition { get; set; }
        public string RequestNutritionText { get; set; }
        public bool? RequestforDetergents { get; set; }
        public string ProcedureCcpNo { get; set; }
        public string Approvers { get; set; }
        public bool? LocalMarket { get; set; }
        public string LocalMarketRegion { get; set; }
        public bool? RepairWithWarranty { get; set; }
        public bool? Emm { get; set; }
        public string WarehouseDeliveryDate { get; set; }
        public int? LocalCollection { get; set; }
        public string CollectionAddress{ get; set; }
        public string Collection2Address{ get; set; }
        public string CollectionPostalCode{ get; set; }
        public string CollectionLocality { get; set; }
        public string CollectionContact  { get; set; }
        public string CollectionResponsibleReception { get; set; }
        public int? LocalDelivery { get; set; }
        public string DeliveryAddress { get; set; }
        public string Delivery2Address { get; set; }
        public string DeliveryPostalCode { get; set; }
        public string LocalityDelivery { get; set; }
        public string DeliveryContact { get; set; }
        public string ResponsibleReceptionReception { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? LocalMarketDate { get; set; }
        public decimal? EstimatedValue { get; set; }
        public string MarketInquiryNo { get; set; }
        public string OrderNo { get; set; }
        public string RequisitionDate { get; set; }
        public string dimension { get; set; }
        public bool? Budget { get; set; }
        public bool? SentReqToAprove { get; set; }
        public string SentReqToAproveText { get; set; }
        public bool GoAprove { get; set; }
        public string PreRequisitionNo { get; set; }
        public bool? PricesIncludingVAT { get; set; }
        public bool? InAdvance { get; set; }
        public string ApprovalDateString { get; set; }
        public bool? PedirOrcamento { get; set; }
        public bool? RoupaManutencao { get; set; }
        public string NumeroMecanografico { get; set; }
        public decimal? ValorTotalDocComIVA { get; set; }
        public bool? ShowPontoSituacao { get; set; }
        public bool? ShowBtnArquivarReqPendente { get; set; }
        public string ClientCode { get; set; }
        public string ClientName { get; set; }
        public string SupplierCode { get; set; }

        //SISLOG
        public int? TipoAlteracaoSISLOG { get; set; }
        public DateTime? DataAlteracaoSISLOG { get; set; }
        public bool? EnviarSISLOG { get; set; }
        public bool? SISLOG { get; set; }
        public DateTime? DataEnvioSISLOG { get; set; }

        public int? Interface { get; set; }
        public string NoSubFornecedor { get; set; }
        public string NomeSubFornecedor { get; set; }
        public string NoEncomendaFornecedor { get; set; }
        public DateTime? DataEncomendaSubfornecedor { get; set; }
        public string DataEncomendaSubfornecedorText { get; set; }

        //LISTA REQUISIÇÕES ORDENADAS

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }
        public List<RequisitionViewModel> Lista { get; set; }


        public List<RequisitionLineViewModel> Lines { get; set; }
        public List<RequisitionChangeLog> ChangeLog { get; set; }

        public RequisitionViewModel()
        {
            this.Lines = new List<RequisitionLineViewModel>();
            this.ChangeLog = new List<RequisitionChangeLog>();
        }

        public decimal GetTotalValue()
        {
            decimal value = 0;
            if (this.Lines != null && this.Lines.Count > 0)
            {
                value = this.Lines
                    .Where(x => x.UnitCost.HasValue && x.QuantityToRequire.HasValue)
                    .Sum(x => x.UnitCost.Value * x.QuantityToRequire.Value);
            }
            return value;
        }

        public decimal GetTotalValueWithIVA()
        {
            decimal value = 0;
            if (this.Lines != null && this.Lines.Count > 0)
            {
                value = this.Lines
                    .Where(x => x.UnitCostWithIVA.HasValue && x.QuantityToRequire.HasValue)
                    .Sum(x => x.UnitCostWithIVA.Value * x.QuantityToRequire.Value);
            }
            return value;
        }
    }

    public partial class RequisitionChangeLog
    {
        public int Id { get; set; }
        public string RequisitionNo { get; set; }
        public RequisitionStates State { get; set; }
        public string StateDescription { get; set; }
        public string Observacoes { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string ModifiedAtAsString { get; set; }
        public string ModifiedBy { get; set; }
    }
}
