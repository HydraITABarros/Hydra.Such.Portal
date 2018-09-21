using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public class DBNAV2017CentroResponsabilidade
    {
        public static List<NAVResponsabilityCenterViewModel> GetAll(string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                List<NAVResponsabilityCenterViewModel> result = new List<NAVResponsabilityCenterViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017CentroDeResponsabilidade @DBName, @CompanyName", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVResponsabilityCenterViewModel()
                        {
                            Code = (string)temp.Code,
                            Name = (string)temp.Name,
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
