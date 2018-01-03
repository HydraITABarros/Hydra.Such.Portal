using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;

namespace Hydra.Such.Data.Logic
{
    public class DBNAV2017StockKeepingUnit
    {
        public static List<NAVStockKeepingUnitViewModel> GetByProductsNo(string NAVDatabaseName, string NAVCompanyName, string productNo)
        {
            List<NAVStockKeepingUnitViewModel> result = new List<NAVStockKeepingUnitViewModel>();
            try
            {
                
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoProduto", productNo)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017UnidadeDeArmazenamento @DBName, @CompanyName, @NoProduto", parameters);
                    
                        foreach (dynamic temp in data)
                        {
                            result.Add(new NAVStockKeepingUnitViewModel()
                            {
                                ItemNo_ = temp.itemNo_,
                                LocationCode = temp.LocationCode,
                                UnitCost = temp.UnitCost,
                                VendorNo_ = temp.VendorNo_,
                                VendorItemNo_ = temp.VendorItemNo_,
                                LeadTimeCalculation = temp.LeadTimeCalculation,
                                SafetyStockQuantity = temp.SafetyStockQuantity,
                                SafetyLeadTime = temp.SafetyLeadTime,
                                TimeBucket = temp.TimeBucket
                            });
                         }
                }

                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }
    }
}
