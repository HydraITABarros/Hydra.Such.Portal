using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public class DBNAV2017ResourcesFamily
    {
        public static List<NAVResourcesFamilyViewModel> GetAllResourcesFamily(string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                List<NAVResourcesFamilyViewModel> result = new List<NAVResourcesFamilyViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017FamiliaRecurso @DBName, @CompanyName", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVResourcesFamilyViewModel()
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
