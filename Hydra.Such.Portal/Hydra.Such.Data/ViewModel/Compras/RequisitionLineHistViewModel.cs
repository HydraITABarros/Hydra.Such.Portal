﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel.Compras
{
    public class RequisitionLineHistViewModel : ErrorHandler
    {
        public string RequestNo { get; set; }
        public int? LineNo { get; set; }
        public int? Type { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public string UnitMeasureCode { get; set; }
        public string LocalCode { get; set; }
        public bool? LocalMarket { get; set; }
        /// <summary>
        /// For filter purposes
        /// </summary>
        public string LocalMarketStringValue
        {
            get
            {
                bool b = this.LocalMarket.HasValue ? this.LocalMarket.Value : false;
                return b ? "Sim" : "Não";
            }
        }
        public decimal? QuantityToRequire { get; set; }
        public decimal? QuantityRequired { get; set; }
        public decimal? QuantityToProvide { get; set; }
        public decimal? QuantityAvailable { get; set; }
        public decimal? QuantityReceivable { get; set; }
        public decimal? QuantityReceived { get; set; }
        public decimal? QuantityPending { get; set; }
        public decimal? UnitCost { get; set; }
        public string ExpectedReceivingDate { get; set; }
        public bool? Billable { get; set; }
        /// <summary>
        /// For filter purposes
        /// </summary>
        public string BillableStringValue
        {
            get
            {
                bool b = this.Billable.HasValue ? this.Billable.Value : false;
                return b ? "Sim" : "Não";
            }
        }
        public string ProjectNo { get; set; }
        public string RegionCode { get; set; }
        public string FunctionalAreaCode { get; set; }
        public string CenterResponsibilityCode { get; set; }
        public string FunctionalNo { get; set; }
        public string Vehicle { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public string UpdateUser { get; set; }
        public decimal? QtyByUnitOfMeasure { get; set; }
        public decimal? UnitCostsould { get; set; }
        public decimal? BudgetValue { get; set; }
        public int? MaintenanceOrderLineNo { get; set; }
        public bool? CriarNotaEncomenda { get; set; }
        /// <summary>
        /// For filter purposes
        /// </summary>
        public string CriarNotaEncomendaStringValue
        {
            get
            {
                bool b = this.CriarNotaEncomenda.HasValue ? this.CriarNotaEncomenda.Value : false;
                return b ? "Sim" : "Não";
            }
        }
        public bool? CreateMarketSearch { get; set; }
        /// <summary>
        /// For filter purposes
        /// </summary>
        public string CreateMarketSearchStringValue
        {
            get
            {
                bool b = this.CreateMarketSearch.HasValue ? this.CreateMarketSearch.Value : false;
                return b ? "Sim" : "Não";
            }
        }

        public bool? SubmitPrePurchase { get; set; }
        /// <summary>
        /// For filter purposes
        /// </summary>
        public string SubmitPrePurchaseStringValue
        {
            get
            {
                bool b = this.SubmitPrePurchase.HasValue ? this.SubmitPrePurchase.Value : false;
                return b ? "Sim" : "Não";
            }
        }
        public bool? SendPrePurchase { get; set; }
        /// <summary>
        /// For filter purposes
        /// </summary>
        public string SendPrePurchaseStringValue
        {
            get
            {
                bool b = this.SendPrePurchase.HasValue ? this.SendPrePurchase.Value : false;
                return b ? "Sim" : "Não";
            }
        }
        public string LocalMarketDate { get; set; }
        public string LocalMarketUser { get; set; }
        public bool? SendForPurchase { get; set; }
        /// <summary>
        /// For filter purposes
        /// </summary>
        public string SendForPurchaseStringValue
        {
            get
            {
                bool b = this.SendForPurchase.HasValue ? this.SendForPurchase.Value : false;
                return b ? "Sim" : "Não";
            }
        }
        public string SendForPurchaseDate { get; set; }
        public bool? PurchaseValidated { get; set; }
        /// <summary>
        /// For filter purposes
        /// </summary>
        public string PurchaseValidatedStringValue
        {
            get
            {
                bool b = this.PurchaseValidated.HasValue ? this.PurchaseValidated.Value : false;
                return b ? "Sim" : "Não";
            }
        }
        public bool? PurchaseRefused { get; set; }
        /// <summary>
        /// For filter purposes
        /// </summary>
        public string PurchaseRefusedStringValue
        {
            get
            {
                bool b = this.PurchaseRefused.HasValue ? this.PurchaseRefused.Value : false;
                return b ? "Sim" : "Não";
            }
        }
        public string ReasonToRejectionLocalMarket { get; set; }
        public string RejectionLocalMarketDate { get; set; }
        public int? PurchaseId { get; set; }
        public string SupplierNo { get; set; }
        public string OpenOrderNo { get; set; }
        public int? OpenOrderLineNo { get; set; }
        public string QueryCreatedMarketNo { get; set; }
        public string CreatedOrderNo { get; set; }
        public string SupplierProductCode { get; set; }
        public string UnitNutritionProduction { get; set; }
        public string MarketLocalRegion { get; set; }
        public string CustomerNo { get; set; }
        public string Approvers { get; set; }

        public bool? Selected { get; set; }
        /// <summary>
        /// For filter purposes
        /// </summary>
        public string SelectedStringValue
        {
            get
            {
                bool b = this.Selected.HasValue ? this.Selected.Value : false;
                return b ? "Sim" : "Não";
            }
        }
        public bool? Urgent { get; set; }
        /// <summary>
        /// For filter purposes
        /// </summary>
        public string UrgentStringValue
        {
            get
            {
                bool b = this.Urgent.HasValue ? this.Urgent.Value : false;
                return b ? "Sim" : "Não";
            }
        }
        public string VATBusinessPostingGroup { get; set; }
        public string VATProductPostingGroup { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public string NoContrato { get; set; }

        //EXPORTAR PARA EXCEL
        public Object ColunasEXCEL { get; set; }

    }
}
