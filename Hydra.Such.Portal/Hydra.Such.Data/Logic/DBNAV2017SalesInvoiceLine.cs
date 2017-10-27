using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public static class DBNAV2017SalesInvoiceLine
    {
        public static List<NAVSalesInvoiceLinesViewModel> GetSalesInvoiceLines(string NAVDatabaseName, string NAVCompanyName, string NAVContractNo, string InitialDate, string ExpirationDate)
        {
            try
            {
                List<NAVSalesInvoiceLinesViewModel> result = new List<NAVSalesInvoiceLinesViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@ContractNoPortal", NAVContractNo),
                        new SqlParameter("@InitialDate", InitialDate),
                        new SqlParameter("@ExpirationDate", ExpirationDate)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017LinhaFaturaVendas @DBName, @CompanyName, @ContractNoPortal, ,@InitialDate, @ExpirationDate", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVSalesInvoiceLinesViewModel()
                        {
                            ContractNo = (string)temp.ContractNoPortal,
                            PostingDate = (string)temp.PostingDate,
                            Amount = (decimal)temp.Amount
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
