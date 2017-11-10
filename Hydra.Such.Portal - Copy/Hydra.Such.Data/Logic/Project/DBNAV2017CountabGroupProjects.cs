using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel.ProjectView;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic.Project
{
    public static class DBNAV2017CountabGroupProjects
    {
        public static List<String> GetAll(string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                List<String> result = new List<String>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017GrContabilisticosProjetos @DBName, @CompanyName", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add((string)temp.Code);
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
