using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public class DBNAV2017PaymentForms
    {
        public static List<NAVPaymentFormsViewModels> GetAll(string NAVDatabaseName, string NAVCompanyName, string NoForma)
        {
            try
            {
                List<NAVPaymentFormsViewModels> result = new List<NAVPaymentFormsViewModels>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoForma", NoForma)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017FormasPagamento @DBName, @CompanyName, @NoForma", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVPaymentFormsViewModels()
                        {
                            Code = (string)temp.Code,
                            Description = (string)temp.Description
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
