using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using static Hydra.Such.Data.Enumerations;
using Hydra.Such.Data.ViewModel.Compras;

namespace Hydra.Such.Data.Logic
{
    public class DBNAV2017Purchases
    {
        public static List<PurchaseHeader> GetOrdersBySupplier(string NAVDatabaseName, string NAVCompanyName, string supplierId)
        {
            return GetPurchasesBy(NAVDatabaseName, NAVCompanyName, NAVBaseDocumentTypes.Encomenda, supplierId, string.Empty, string.Empty);
        }

        public static PurchaseHeader GetOrderById(string NAVDatabaseName, string NAVCompanyName, string orderId)
        {
            var items = GetPurchasesBy(NAVDatabaseName, NAVCompanyName, NAVBaseDocumentTypes.Encomenda, string.Empty, orderId, string.Empty);
            if (items != null)
            {
                return items.FirstOrDefault();
            }
            return null;
        }

        public static PurchaseHeader GetByExternalDocNo(string NAVDatabaseName, string NAVCompanyName, NAVBaseDocumentTypes type, string externalDocNo)
        {
            var items = GetPurchasesBy(NAVDatabaseName, NAVCompanyName, type, string.Empty, string.Empty, externalDocNo);
            if (items != null)
            {
                return items.FirstOrDefault();
            }
            return null;
        }

        private static List<PurchaseHeader> GetPurchasesBy(string NAVDatabaseName, string NAVCompanyName, NAVBaseDocumentTypes type, string supplierId, string orderId, string externalDocNo)
        {
            try
            {
                List<PurchaseHeader> result = new List<PurchaseHeader>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@Type", (int)type),
                        new SqlParameter("@SupplierId", supplierId),
                        new SqlParameter("@OrderId", orderId),
                        new SqlParameter("@ExternalDocNo", externalDocNo)
                    };
                    
                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017Encomendas @DBName, @CompanyName, @Type, @SupplierId, @OrderId, @ExternalDocNo", parameters);

                    foreach (dynamic temp in data)
                    {
                        var item = new PurchaseHeader();

                        item.DocumentType = (int)temp.DocumentType;
                        item.No = temp.No;
                        item.OrderDate = temp.OrderDate is DBNull ? string.Empty : ((DateTime)temp.OrderDate).ToString("yyyy-MM-dd");
                        item.DueDate = temp.DueDate is DBNull ? string.Empty : ((DateTime)temp.DueDate).ToString("yyyy-MM-dd");
                        item.LocationCode = temp.LocationCode;
                        item.RegionId = temp.RegionId is DBNull ? string.Empty : temp.RegionId;
                        item.FunctionalAreaId = temp.FunctionalAreaId is DBNull ? string.Empty : temp.FunctionalAreaId;
                        item.RespCenterId = temp.RespCenterId is DBNull ? string.Empty : temp.RespCenterId;
                        item.VendorOrderNo = temp.VendorOrderNo;
                        item.VendorInvoiceNo = temp.VendorInvoiceNo;
                        item.VendorCrMemoNo = temp.VendorCrMemoNo;
                        item.BuyFromVendorNo = temp.BuyFromVendorNo;
                        item.BuyFromVendorName = temp.BuyFromVendorName;
                        item.DocumentDate = temp.DocumentDate is DBNull ? string.Empty : ((DateTime)temp.DocumentDate).ToString("yyyy-MM-dd");
                        item.DimensionSetID = temp.DimensionSetID;
                        item.RelatedDocument = temp.RelatedDocument;
                        item.ValorFactura = temp.ValorFactura;
                        item.SourceDocNo = temp.SourceDocNo;
                        item.Quantity = temp.Quantity;
                        item.QuantityReceived = temp.QuantityReceived;
                        item.AmountRcdNotInvoiced = temp.AmountRcdNotInvoiced;
                        item.Amount = temp.Amount;
                        item.AmountIncludingVAT = temp.AmountIncludingVAT;


                        result.Add(item);
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
