using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.ProjectView;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic.Project
{
    public class DBNAV2017DimensionValues
    {
        public static List<NAVDimValueViewModel> GetByDimType(string NAVDatabaseName, string NAVCompanyName, int NAVDimType)
        {
            try
            {
                List<NAVDimValueViewModel> result = new List<NAVDimValueViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@TipoDim", NAVDimType)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017ValoresDimensao @DBName, @CompanyName, @TipoDim", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVDimValueViewModel()
                        {
                            Code = (string)temp.Code,
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

