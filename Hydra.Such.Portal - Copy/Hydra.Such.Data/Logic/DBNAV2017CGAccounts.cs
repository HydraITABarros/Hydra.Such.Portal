using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.ProjectView;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public class DBNAV2017CGAccounts
    {
        public static List<NAVCGAccountViewModel> GetAllCGAccounts(string NAVDatabaseName, string NAVCompanyName, string NAVNoConta)
        {
            try
            {
                List<NAVCGAccountViewModel> result = new List<NAVCGAccountViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoConta", NAVNoConta)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017ContasCG @DBName, @CompanyName, @NoConta", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVCGAccountViewModel()
                        {
                            Code = (string)temp.No_,
                            Name = (string)temp.Name
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
