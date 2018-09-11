using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;

namespace Hydra.Such.Data.Logic
{
   public class DBNAV2017OpenOrderLines
    {
        public static List<NAVOpenOrderLinesViewModels> GetAll(string NAVDatabaseName, string NAVCompanyName, string Date, string PurchaseHeaderNo,string codFuncitonalArea, bool onlyWithAvailableQuantity = false)
        {
            List<NAVOpenOrderLinesViewModels> result = new List<NAVOpenOrderLinesViewModels>();
            try
            {
               
                using (var ctx = new SuchDBContextExtention())
                {
                    string purhNo = string.IsNullOrEmpty(PurchaseHeaderNo) ? "" : PurchaseHeaderNo;
                    string codFunc = string.IsNullOrEmpty(codFuncitonalArea) ? "" : codFuncitonalArea;
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@DateSupplierPrice", Date),
                        new SqlParameter("@FunctionalAreaCode", codFunc),
                        new SqlParameter("@PurchaseHeaderNo", purhNo),
                        new SqlParameter("@OnlyWithAvailableQuantity", (onlyWithAvailableQuantity ? 1 : 0)),
            };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017LinhasEncomendaAberto @DBName, @CompanyName, @DateSupplierPrice, @FunctionalAreaCode,@PurchaseHeaderNo,@OnlyWithAvailableQuantity", parameters);

                    foreach (dynamic temp in data)
                    {
                        //var item = new NAVOpenOrderLinesViewModels();

                        //item.id = temp.DocumentNO + " " + temp.Line_No + " " + temp.Numb;
                        //item.DocumentType = (int)temp.DocumentType;
                        //item.DocumentNO = (string)temp.DocumentNO;
                        //item.Line_No = (int)temp.Line_No;
                        //item.BuyFromVendorNo = (string)temp.BuyFromVendorNo;
                        //item.Type = (int)temp.Type;
                        //item.Numb = (string)temp.Numb;
                        //item.LocationCode = (string)temp.LocationCode;
                        //item.ExpectedReceiptDate = (DateTime)temp.ExpectedReceiptDate;
                        //item.Description = (string)temp.Description;
                        //item.UnitofMeasure = (string)temp.UnitofMeasure;
                        //item.Quantity = (decimal)temp.Quantity;
                        //item.OutstandingQuantity = (decimal)temp.OutstandingQuantity;
                        //item.QtytoInvoice = (decimal)temp.QtytoInvoice;
                        //item.QtytoReceive = (decimal)temp.QtytoReceive;
                        //item.DirectUnitCost = (decimal)temp.DirectUnitCost;
                        //item.UnitCostLCY = (decimal)temp.UnitCostLCY;
                        //item.Amount = (decimal)temp.Amount;
                        //item.UnitPriceLCY = (decimal)temp.UnitPriceLCY;
                        //item.OutstandingAmount = (decimal)temp.OutstandingAmount;
                        //item.QuantityReceived = (decimal)temp.QuantityReceived;
                        //item.QuantityInvoiced = (decimal)temp.QuantityInvoiced;
                        //item.PaytoVendorNo = (string)temp.PaytoVendorNo;
                        //item.VendorItemNo = (string)temp.VendorItemNo;
                        //item.GenBusPostingGroup = (string)temp.GenBusPostingGroup;
                        //item.GenProdPostingGroup = (string)temp.GenProdPostingGroup;
                        //item.EntryPoint = (string)temp.EntryPoint;
                        //item.CurrencyCode = (string)temp.CurrencyCode;
                        //item.VATBaseAmount = (decimal)temp.VATBaseAmount;
                        //item.UnitCost = (decimal)temp.UnitCost;
                        //item.SystemCreatedEntry = (int)temp.SystemCreatedEntry;
                        //item.LineAmount = (decimal)temp.LineAmount;
                        //item.ICPartnerRefType = (int)temp.ICPartnerRefType;
                        //item.ICPartnerReference = (string)temp.ICPartnerReference;
                        //item.ICPartnerCode = (string)temp.ICPartnerCode;
                        //item.DimensionSetID = (int)temp.DimensionSetID;
                        //item.ReturnsDeferralStartDate = (DateTime)temp.ReturnsDeferralStartDate;
                        //item.ProdOrderNo = (string)temp.ProdOrderNo;
                        //item.QtyperUnitofMeasure = (decimal)temp.QtyperUnitofMeasure;
                        //item.UnitofMeasureCode = (string)temp.UnitofMeasureCode;
                        //item.QuantityBase = (decimal)temp.QuantityBase;
                        //item.OutstandingQtyBase = (decimal)temp.OutstandingQtyBase;
                        //item.QtytoInvoiceBase = (decimal)temp.QtytoInvoiceBase;
                        //item.QtytoReceiveBase = (decimal)temp.QtytoReceiveBase;
                        //item.QtyRcdNotInvoicedBase = (decimal)temp.QtyRcdNotInvoicedBase;
                        //item.QtyInvoicedBase = (decimal)temp.QtyInvoicedBase;
                        //item.SalvageValue = (decimal)temp.SalvageValue;
                        //item.UnitofMeasureCrossRef = (string)temp.UnitofMeasureCrossRef;
                        //item.ItemCategoryCode = (string)temp.ItemCategoryCode;
                        //item.ProductGroupCode = (string)temp.ProductGroupCode;
                        //item.OrderDate = (DateTime)temp.OrderDate;
                        //item.WorkCenterNo = (string)temp.WorkCenterNo;
                        //item.Finished = (int)temp.Finished;
                        //item.ProdOrderLineN = (int)temp.ProdOrderLineN;
                        //item.ContractStartingDate = (DateTime)temp.ContractStartingDate;
                        //item.ContractEndingDate = (DateTime)temp.ContractEndingDate;

                        result.Add(new NAVOpenOrderLinesViewModels()
                        {
                            id = temp.DocumentNO + " " + temp.Line_No + " " + temp.Numb,
                            DocumentType = (int)temp.DocumentType,
                            DocumentNO = (string)temp.DocumentNO,
                            Line_No = (int)temp.Line_No,
                            BuyFromVendorNo = (string)temp.BuyFromVendorNo,
                            Type = (int)temp.Type,
                            Numb = (string)temp.Numb,
                            LocationCode = (string)temp.LocationCode,
                            ExpectedReceiptDate = (DateTime)temp.ExpectedReceiptDate,
                            Description = (string)temp.Description,
                            UnitofMeasure = (string)temp.UnitofMeasure,
                            Quantity = (decimal)temp.Quantity,
                            OutstandingQuantity = (decimal)temp.OutstandingQuantity,
                            QtytoInvoice = (decimal)temp.QtytoInvoice,
                            QtytoReceive = (decimal)temp.QtytoReceive,
                            DirectUnitCost = (decimal)temp.DirectUnitCost,
                            UnitCostLCY = (decimal)temp.UnitCostLCY,
                            Amount = (decimal)temp.Amount,
                            UnitPriceLCY = (decimal)temp.UnitPriceLCY,
                            OutstandingAmount = (decimal)temp.OutstandingAmount,
                            QuantityReceived = (decimal)temp.QuantityReceived,
                            AvailableQuantity = (decimal)temp.AvailableQuantity,
                            QuantityInvoiced = (decimal)temp.QuantityInvoiced,
                            PaytoVendorNo = (string)temp.PaytoVendorNo,
                            VendorItemNo = (string)temp.VendorItemNo,
                            GenBusPostingGroup = (string)temp.GenBusPostingGroup,
                            GenProdPostingGroup = (string)temp.GenProdPostingGroup,
                            EntryPoint = (string)temp.EntryPoint,
                            CurrencyCode = (string)temp.CurrencyCode,
                            VATBaseAmount = (decimal)temp.VATBaseAmount,
                            UnitCost = (decimal)temp.UnitCost,
                            SystemCreatedEntry = (int)temp.SystemCreatedEntry,
                            LineAmount = (decimal)temp.LineAmount,
                            ICPartnerRefType = (int)temp.ICPartnerRefType,
                            ICPartnerReference = (string)temp.ICPartnerReference,
                            ICPartnerCode = (string)temp.ICPartnerCode,
                            DimensionSetID = (int)temp.DimensionSetID,
                            ReturnsDeferralStartDate = (DateTime)temp.ReturnsDeferralStartDate,
                            ProdOrderNo = (string)temp.ProdOrderNo,
                            QtyperUnitofMeasure = (decimal)temp.QtyperUnitofMeasure,
                            UnitofMeasureCode = (string)temp.UnitofMeasureCode,
                            QuantityBase = (decimal)temp.QuantityBase,
                            OutstandingQtyBase = (decimal)temp.OutstandingQtyBase,
                            QtytoInvoiceBase = (decimal)temp.QtytoInvoiceBase,
                            QtytoReceiveBase = (decimal)temp.QtytoReceiveBase,
                            QtyRcdNotInvoicedBase = (decimal)temp.QtyRcdNotInvoicedBase,
                            QtyInvoicedBase = (decimal)temp.QtyInvoicedBase,
                            SalvageValue = (decimal)temp.SalvageValue,
                            UnitofMeasureCrossRef = (string)temp.UnitofMeasureCrossRef,
                            ItemCategoryCode = (string)temp.ItemCategoryCode,
                            ProductGroupCode = (string)temp.ProductGroupCode,
                            OrderDate = (DateTime)temp.OrderDate,
                            WorkCenterNo = (string)temp.WorkCenterNo,
                            Finished = (int)temp.Finished,
                            ProdOrderLineN = (int)temp.ProdOrderLineN,
                            ContractStartingDate = (DateTime)temp.ContractStartingDate,
                            ContractEndingDate = (DateTime)temp.ContractEndingDate,
                            FunctionalAreaNo = temp.FunctionalAreaNo.Equals(DBNull.Value) ? "" : (string)temp.FunctionalAreaNo,
                            FunctionalAreaDescription = temp.FunctionalAreaDescription.Equals(DBNull.Value) ? "" : (string)temp.FunctionalAreaDescription,
                            RegionNo = temp.RegionNo.Equals(DBNull.Value) ? "" : (string)temp.RegionNo,
                            RegionDescription = temp.RegionDescription.Equals(DBNull.Value) ? "" : (string)temp.RegionDescription,
                            ResponsabilityCenterNo = temp.ResponsabilityCenterNo.Equals(DBNull.Value) ? "" : (string)temp.ResponsabilityCenterNo,
                            ResponsabilityCenterDescription = temp.ResponsabilityCenterDescription.Equals(DBNull.Value) ? "" : (string)temp.ResponsabilityCenterDescription
                        });
                    }
                    return result;
                }

               
            }
            catch (Exception ex)
            {
                return result;
            }
        }

    }
}
