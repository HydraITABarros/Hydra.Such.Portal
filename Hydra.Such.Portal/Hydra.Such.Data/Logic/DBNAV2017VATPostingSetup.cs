using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public static class DBNAV2017VATPostingSetup
    {
        public static decimal GetIVA(string NAVDatabaseName, string NAVCompanyName, string GrupoFornecedor, string GrupoIVA)
        {
            decimal result = new decimal();
            using (var ctx = new SuchDBContextExtention())
            {
                var parameters = new[]{
                    new SqlParameter("@DBName", NAVDatabaseName),
                    new SqlParameter("@CompanyName", NAVCompanyName),
                    new SqlParameter("@GrupoFornecedor", GrupoFornecedor),
                    new SqlParameter("@GrupoIVA", GrupoIVA)
                };

                IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017VATPostingSetup @DBName, @CompanyName, @GrupoFornecedor, @GrupoIVA", parameters);

                foreach (dynamic temp in data)
                {
                    result = (decimal)temp.IVA;
                }
            }

            return result;
        }
    }
}
