using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public class DBNAV2017Clients
    {
        public static NAVClientsViewModel GetClientById(string NAVDatabaseName, string NAVCompanyName, string NAVClientNo)
        {
            List<NAVClientsViewModel> result = GetClients(NAVDatabaseName, NAVCompanyName, NAVClientNo);
            if (result != null)
                return result.FirstOrDefault();
            return null;
        }

        public static List<NAVClientsViewModel> GetClients(string NAVDatabaseName, string NAVCompanyName, string NAVClientNo)
        {
            try
            {
                List<NAVClientsViewModel> result = new List<NAVClientsViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoCliente", NAVClientNo)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017Clientes @DBName, @CompanyName, @NoCliente", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVClientsViewModel()
                        {
                            No_ = (string)temp.No_,
                            Name = (string)temp.Name,
                            VATRegistrationNo_ = (string)temp.VATRegistrationNo,
                            Address = (string)temp.Address,
                            PostCode = (string)temp.PostalCode,
                            Country_RegionCode = (string)temp.Country_RegionCode,
                            //Country_RegionCode = (string)temp.Country_RegionCode
                            UnderCompromiseLaw = ((int)temp.UnderCompromiseLaw) == 0 ? false : true,
                            National = ((int)temp.NationalCustomer) == 0 ? false : true,
                            PaymentTermsCode = (string)temp.PaymentTermsCode,
                            InternalClient = ((int)temp.InternalClient) == 0 ? false : true,
                            PaymentMethodCode = (string)temp.PaymentMethodCode,
                            RegionCode = (string)temp.RegionCode,
                        });
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static string GetClientNameByNo(string NoClient, string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                string result = "";
                string currency = "";
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoCliente", NoClient)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017Clientes @DBName, @CompanyName, @NoCliente", parameters);

                    foreach (dynamic temp in data)
                    {
                        result = (string)temp.Name;
                        
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
            public static string GetClientVATByNo(string NoClient, string NAVDatabaseName, string NAVCompanyName)
            {
                try
                {
                    string result = "";
                    using (var ctx = new SuchDBContextExtention())
                    {
                        var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoCliente", NoClient)
                    };

                        IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017Clientes @DBName, @CompanyName, @NoCliente", parameters);

                        foreach (dynamic temp in data)
                        {
                            result = (string)temp.VATRegistrationNo;
                        }
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    return "";
                }
            }


        public static string GetClientCurrencyByNo(string NoClient, string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                string result = "";
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoCliente", NoClient)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017Clientes @DBName, @CompanyName, @NoCliente", parameters);

                    foreach (dynamic temp in data)
                    {
                        result = (string)temp.Currency;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
