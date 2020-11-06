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

        public static List<NAVVATPostingSetupViewModelcs> GetAllIVA(string NAVDatabaseName, string NAVCompanyName)
        {
            List<NAVVATPostingSetupViewModelcs> result = new List<NAVVATPostingSetupViewModelcs>();
            using (var ctx = new SuchDBContextExtention())
            {
                var parameters = new[]{
                    new SqlParameter("@DBName", NAVDatabaseName),
                    new SqlParameter("@CompanyName", NAVCompanyName)
                };

                IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017VATPostingSetupGetALL @DBName, @CompanyName", parameters);

                foreach (dynamic temp in data)
                {
                    result.Add(new NAVVATPostingSetupViewModelcs()
                    {
                        VATBusPostingGroup = (string)temp.VATBusPostingGroup,
                        VATProdPostingGroup = (string)temp.VATProdPostingGroup,
                        VAT = (decimal)temp.VAT,
                    });
                }
            }

            return result;
        }
    }
}
