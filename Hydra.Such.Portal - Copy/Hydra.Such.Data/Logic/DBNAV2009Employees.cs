using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public static class DBNAV2009Employees
    {
        public static List<NAVEmployeeViewModel> GetAll(string resourceNo, string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                List<NAVEmployeeViewModel> result = new List<NAVEmployeeViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoEmpregado", resourceNo),
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2009Empregados @DBName, @CompanyName, @NoEmpregado", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVEmployeeViewModel()
                        {
                            No = (string)temp.No_,
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
