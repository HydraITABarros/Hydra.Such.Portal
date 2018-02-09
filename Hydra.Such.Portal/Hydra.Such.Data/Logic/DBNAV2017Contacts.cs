using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public class DBNAV2017Contacts
    {
        public static List<NAVContactsViewModel> GetContacts(string NAVDatabaseName, string NAVCompanyName, string NAVContactNo)
        {
            try
            {
                List<NAVContactsViewModel> result = new List<NAVContactsViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoContato", NAVContactNo)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017Contactos @DBName, @CompanyName, @NoContato", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVContactsViewModel()
                        {
                            No_ = (string)temp.No_,
                            Name = (string)temp.Name,
                            Address = (string)temp.Address,
                            PostCode = (string)temp.PostCode,
                            City = (string)temp.City,
                            Country_RegionCode = (string)temp.Country_RegionCode
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

        public static List<NAVContactsViewModel> GetCustomerContacts(string NAVDatabaseName, string NAVCompanyName, string customerNo)
        {
            try
            {
                List<NAVContactsViewModel> result = new List<NAVContactsViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoCliente", customerNo)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017ContactosDoCliente @DBName, @CompanyName, @NoCliente", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVContactsViewModel()
                        {
                            No_ = (string)temp.No_,
                            Name = (string)temp.Name,
                            Address = (string)temp.Address,
                            PostCode = (string)temp.PostCode,
                            City = (string)temp.City,
                            Country_RegionCode = (string)temp.Country_RegionCode
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
