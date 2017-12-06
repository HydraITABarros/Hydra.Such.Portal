using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public class DBNAV2017Job
    {
        public static List<NAVJobViewModel> GetJob(string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                List<NAVJobViewModel> result = new List<NAVJobViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017Job @DBName, @CompanyName", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVJobViewModel()
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
