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
        public static List<NAVOpenOrderLinesViewModels> GetAll(string NAVDatabaseName, string NAVCompanyName, string Date, string PurchaseHeaderNo, string codFuncitonalArea, bool onlyWithAvailableQuantity = false)
        {
            List<NAVOpenOrderLinesViewModels> result = new List<NAVOpenOrderLinesViewModels>();
            try
            {
                DateTime d;
                string formatedDate = string.Empty;
                if (DateTime.TryParse(Date, out d))
                    formatedDate = d.ToString("MM-dd-yyyy");

                using (var ctx = new SuchDBContextExtention())
                {
                    string purhNo = string.IsNullOrEmpty(PurchaseHeaderNo) ? "" : PurchaseHeaderNo;
                    string codFunc = string.IsNullOrEmpty(codFuncitonalArea) ? "" : codFuncitonalArea;
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@DateSupplierPrice", formatedDate),
                        new SqlParameter("@FunctionalAreaCode", codFunc),
                        new SqlParameter("@PurchaseHeaderNo", purhNo),
                        new SqlParameter("@OnlyWithAvailableQuantity", (onlyWithAvailableQuantity ? 1 : 0)),
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017LinhasEncomendaAberto @DBName, @CompanyName, @DateSupplierPrice, @FunctionalAreaCode,@PurchaseHeaderNo,@OnlyWithAvailableQuantity", parameters);

                    foreach (dynamic temp in data)
                    {
                        var item = new NAVOpenOrderLinesViewModels();

                        item.id = temp.DocumentNO + " " + temp.Line_No + " " + temp.Numb;
                        item.DocumentType = (int)temp.DocumentType;
                        item.DocumentNO = (string)temp.DocumentNO;
                        item.Line_No = (int)temp.Line_No;
                        item.BuyFromVendorNo = (string)temp.BuyFromVendorNo;
                        item.Type = (int)temp.Type;
                        item.Numb = (string)temp.Numb;
                        item.LocationCode = (string)temp.LocationCode;
                        item.LocationName = (string)temp.LocationName;
                        item.DirectPurchLocation = ((int)temp.DirectPurchLocation == 1);
                        item.ExpectedReceiptDate = (DateTime)temp.ExpectedReceiptDate;
                        item.Description = (string)temp.Description;
                        item.UnitofMeasure = (string)temp.UnitofMeasure;
                        item.Quantity = (decimal)temp.Quantity;
                        item.OutstandingQuantity = (decimal)temp.OutstandingQuantity;
                        item.QtytoInvoice = (decimal)temp.QtytoInvoice;
                        item.QtytoReceive = (decimal)temp.QtytoReceive;
                        item.DirectUnitCost = (decimal)temp.DirectUnitCost;
                        item.UnitCostLCY = (decimal)temp.UnitCostLCY;
                        item.Amount = (decimal)temp.Amount;
                        item.UnitPriceLCY = (decimal)temp.UnitPriceLCY;
                        item.OutstandingAmount = (decimal)temp.OutstandingAmount;
                        item.QuantityReceived = (decimal)temp.QuantityReceived;
                        item.AvailableQuantity = (decimal)temp.AvailableQuantity;
                        item.QuantityInvoiced = (decimal)temp.QuantityInvoiced;
                        item.PaytoVendorNo = (string)temp.PaytoVendorNo;
                        item.VendorItemNo = (string)temp.VendorItemNo;
                        item.GenBusPostingGroup = (string)temp.GenBusPostingGroup;
                        item.GenProdPostingGroup = (string)temp.GenProdPostingGroup;
                        item.EntryPoint = (string)temp.EntryPoint;
                        item.CurrencyCode = (string)temp.CurrencyCode;
                        item.VATBaseAmount = (decimal)temp.VATBaseAmount;
                        item.UnitCost = (decimal)temp.UnitCost;
                        item.SystemCreatedEntry = (int)temp.SystemCreatedEntry;
                        item.LineAmount = (decimal)temp.LineAmount;
                        item.ICPartnerRefType = (int)temp.ICPartnerRefType;
                        item.ICPartnerReference = (string)temp.ICPartnerReference;
                        item.ICPartnerCode = (string)temp.ICPartnerCode;
                        item.DimensionSetID = (int)temp.DimensionSetID;
                        item.ReturnsDeferralStartDate = (DateTime)temp.ReturnsDeferralStartDate;
                        item.ProdOrderNo = (string)temp.ProdOrderNo;
                        item.QtyperUnitofMeasure = (decimal)temp.QtyperUnitofMeasure;
                        item.UnitofMeasureCode = (string)temp.UnitofMeasureCode;
                        item.QuantityBase = (decimal)temp.QuantityBase;
                        item.OutstandingQtyBase = (decimal)temp.OutstandingQtyBase;
                        item.QtytoInvoiceBase = (decimal)temp.QtytoInvoiceBase;
                        item.QtytoReceiveBase = (decimal)temp.QtytoReceiveBase;
                        item.QtyRcdNotInvoicedBase = (decimal)temp.QtyRcdNotInvoicedBase;
                        item.QtyInvoicedBase = (decimal)temp.QtyInvoicedBase;
                        item.SalvageValue = (decimal)temp.SalvageValue;
                        item.UnitofMeasureCrossRef = (string)temp.UnitofMeasureCrossRef;
                        item.ItemCategoryCode = (string)temp.ItemCategoryCode;
                        item.ProductGroupCode = (string)temp.ProductGroupCode;
                        item.OrderDate = (DateTime)temp.OrderDate;
                        item.WorkCenterNo = (string)temp.WorkCenterNo;
                        item.Finished = (int)temp.Finished;
                        item.ProdOrderLineN = (int)temp.ProdOrderLineN;
                        item.ContractStartingDate = (DateTime)temp.ContractStartingDate;
                        item.ContractEndingDate = (DateTime)temp.ContractEndingDate;
                        item.FunctionalAreaNo = temp.FunctionalAreaNo.Equals(DBNull.Value) ? "" : (string)temp.FunctionalAreaNo;
                        item.FunctionalAreaDescription = temp.FunctionalAreaDescription.Equals(DBNull.Value) ? "" : (string)temp.FunctionalAreaDescription;
                        item.RegionNo = temp.RegionNo.Equals(DBNull.Value) ? "" : (string)temp.RegionNo;
                        item.RegionDescription = temp.RegionDescription.Equals(DBNull.Value) ? "" : (string)temp.RegionDescription;
                        item.ResponsabilityCenterNo = temp.ResponsabilityCenterNo.Equals(DBNull.Value) ? "" : (string)temp.ResponsabilityCenterNo;
                        item.ResponsabilityCenterDescription = temp.ResponsabilityCenterDescription.Equals(DBNull.Value) ? "" : (string)temp.ResponsabilityCenterDescription;

                        result.Add(item);
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
