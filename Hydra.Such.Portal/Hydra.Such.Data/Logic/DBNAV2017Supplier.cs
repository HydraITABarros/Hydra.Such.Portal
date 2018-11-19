using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;

namespace Hydra.Such.Data.Logic
{
    public class DBNAV2017Supplier
    {

        public static List<NAVSupplierViewModels> GetAll(string NAVDatabaseName, string NAVCompanyName, string TermNo)
        {
            try
            {
                List<NAVSupplierViewModels> result = new List<NAVSupplierViewModels>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoTermo", TermNo)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017Fornecedores @DBName, @CompanyName, @NoTermo", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVSupplierViewModels()
                        {
                            No_ = (string)temp.No_,
                            Name = (string)temp.Name,
                            Address = (string)temp.Address,
                            VATBusinessPostingGroup = (string)temp.VATBusinessPostingGroup,
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
