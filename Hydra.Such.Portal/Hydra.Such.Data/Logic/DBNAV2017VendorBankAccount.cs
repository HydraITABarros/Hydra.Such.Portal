using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public class DBNAV2017VendorBankAccount
    {
        public static NAV2017VendorBankAccountViewModel GetVendor(string NAVDatabaseName, string NAVCompanyName, string VendorNo)
        {
            try
            {
                NAV2017VendorBankAccountViewModel result = new NAV2017VendorBankAccountViewModel();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@VendorNo", VendorNo)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017VendorBankAccount @DBName, @CompanyName, @VendorNo", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.VendorNo = (string)temp.Vendor;
                        result.VendorName = (string)temp.Name;
                        result.IBAN = (string)temp.IBAN;
                        result.NIB = (string)temp.NIB;
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
