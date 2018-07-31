using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
   public class DBNAV2017SalesLine
    {
        public static List<NAVSalesLinesViewModel> FindSalesLine(string NAVDatabaseName, string NAVCompanyName, string NAVContract, string NAVClientNo)
        {
            List<NAVSalesLinesViewModel> result = new List<NAVSalesLinesViewModel>();
            try
            {
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@ContractNo", NAVContract),
                        new SqlParameter("@CustomerNo", NAVClientNo)
                    };
                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017Pre_Registo @DBName, @CompanyName, @ContractNo, @CustomerNo", parameters);
                    if (data != null)
                    {
                        foreach (dynamic item in data)
                        {
                            result.Add(new NAVSalesLinesViewModel()
                            {
                                ContractNo = (string)item.ContractNo_,
                                ClientNo = (string)item.Sell_toCustomerNo_,
                                DocNo = (string)item.DocumentNo,
                            });
                        }
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
