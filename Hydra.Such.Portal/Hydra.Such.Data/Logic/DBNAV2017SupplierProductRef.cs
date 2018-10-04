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
    public class DBNAV2017SupplierProductRef
    {
        public class SuppliersProductsRefs
        {
            public string SupplierNo { get; set; }
            public string ProductId { get; set; }
            public string UnitOfMeasureCode { get; set; }
            public string SupplierProductId { get; set; }
        }

        public static List<SuppliersProductsRefs> GetSuplierProductRefsForRequisition(string NAVDatabaseName, string NAVCompanyName, string requisitionNo)
        {
            try
            {
                List<SuppliersProductsRefs> result = new List<SuppliersProductsRefs>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoRequisicao", requisitionNo),
                    };
                    
                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017ObterProdutosFornecedor @DBName, @CompanyName, @NoRequisicao", parameters);

                    foreach (dynamic temp in data)
                    {
                        var item = new SuppliersProductsRefs();

                        item.SupplierNo = temp.SupplierNo;
                        item.ProductId = temp.ProductId;
                        item.UnitOfMeasureCode = temp.UnitOfMeasureCode;
                        item.SupplierProductId = temp.SupplierProductId;
                        
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
