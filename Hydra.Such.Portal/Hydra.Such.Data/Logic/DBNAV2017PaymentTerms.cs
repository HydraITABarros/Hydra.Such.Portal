using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public class DBNAV2017PaymentTerms
    {
        public static List<NAVPaymentTermsViewModels> GetAll(string NAVDatabaseName, string NAVCompanyName, string TermNo)
        {
            try
            {
                List<NAVPaymentTermsViewModels> result = new List<NAVPaymentTermsViewModels>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoTermo", TermNo)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017TermosPagamento @DBName, @CompanyName, @NoTermo", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVPaymentTermsViewModels()
                        {
                            Code = (string)temp.Code,
                            Description = (string)temp.Description,
                            DueDateCalculation = (string)temp.DueDateCalculation
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
