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
        public static List<NAVOpenOrderLinesViewModels> GetAll(string NAVDatabaseName, string NAVCompanyName, DateTime? Date, string PurchaseHeaderNo)
        {
            try
            {
                List<NAVOpenOrderLinesViewModels> result = new List<NAVOpenOrderLinesViewModels>();
                using (var ctx = new SuchDBContextExtention())
                {
                    int index = 1;
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@DateSupplierPrice", Date),
                        new SqlParameter("@PurchaseHeaderNo", PurchaseHeaderNo)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017LinhasEncomendaAberto @DBName, @CompanyName, @DateSupplierPrice, @PurchaseHeaderNo", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVOpenOrderLinesViewModels()
                        {
                              id = index++,
                              DocumentType = (int)temp.DocumentType,
                              DocumentNO =(string)temp.DocumentNO,
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
                              ContractEndingDate = (DateTime)temp.ContractEndingDate
                        });
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
