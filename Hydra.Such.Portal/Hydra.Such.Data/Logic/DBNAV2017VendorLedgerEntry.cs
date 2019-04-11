using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public class DBNAV2017VendorLedgerEntry
    {
        public static List<NAV2017VendorLedgerEntryViewModel> GetMovFornecedores(string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                List<NAV2017VendorLedgerEntryViewModel> result = new List<NAV2017VendorLedgerEntryViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017VendorLedgerEntry @DBName, @CompanyName", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAV2017VendorLedgerEntryViewModel()
                        {
                            VendorNo = (string)temp.VendorNo,
                            DocumentType = (int)temp.DocumentType,
                            Open = (int)temp.Open,
                            PostingDate = (DateTime)temp.PostingDate
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
