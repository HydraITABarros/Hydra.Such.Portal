using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public class DBNAV2017PurchaseHeader
    {
        public static List<NAVPurchaseHeaderViewModel> GetPurchaseHeader(string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                List<NAVPurchaseHeaderViewModel> result = new List<NAVPurchaseHeaderViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017CabecalhoEncomendasAberto @DBName, @CompanyName", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVPurchaseHeaderViewModel()
                        {
                            No_ = (string)temp.No_
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
