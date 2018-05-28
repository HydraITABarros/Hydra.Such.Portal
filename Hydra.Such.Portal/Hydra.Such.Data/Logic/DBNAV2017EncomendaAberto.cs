using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public class DBNAV2017EncomendaAberto
    {
        public static List<NAVEncomendaAbertoViewModel> GetByDimValue(string NAVDatabaseName, string NAVCompanyName, int DimValueID)
        {
            try
            {
                List<NAVEncomendaAbertoViewModel> result = new List<NAVEncomendaAbertoViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@SetID", DimValueID)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017EncomendaAberto @DBName, @CompanyName, @SetID", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVEncomendaAbertoViewModel()
                        {
                            Code = (string)temp.DocNo,
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
