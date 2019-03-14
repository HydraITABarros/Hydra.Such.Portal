using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Hydra.Such.Data.Logic.Project
{
    public class DBNAV2017ShippingAddresses
    {

        public static List<NAVAddressesViewModel> GetAll(string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                List<NAVAddressesViewModel> result = new List<NAVAddressesViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoCliente", "")
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017EnderecosEnvio @DBName, @CompanyName, @NoCliente", parameters);
                    
                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVAddressesViewModel()
                        {
                            Code = (string)temp.Code,
                            Name = (string)temp.Name,
                            Address = (string)temp.Address,
                            ZipCode = (string)temp.ZipCode,
                            City = (string)temp.City,
                            Contact = (string)temp.Contact
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

        public static List<NAVAddressesViewModel> GetByClientNo(string ClientNo, string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                List<NAVAddressesViewModel> result = new List<NAVAddressesViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoCliente", ClientNo)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017EnderecosEnvio @DBName, @CompanyName, @NoCliente", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVAddressesViewModel()
                        {
                            Code = (string)temp.Code,
                            Name = (string)temp.Name,
                            Address = (string)temp.Address,
                            ZipCode = (string)temp.ZipCode,
                            City = (string)temp.City,
                            Contact = (string)temp.Contact
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


        public static NAVAddressesViewModel GetByCode(string Code, string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                if (string.IsNullOrEmpty(Code))
                {
                    return null;
                }
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoCliente", "")
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017EnderecosEnvio @DBName, @CompanyName, @NoCliente", parameters);
                    string customerNo = string.Empty;
                    foreach (dynamic temp in data)
                    {
                        customerNo = (string)temp.Code;
                        if (!string.IsNullOrEmpty(customerNo) && customerNo == Code)
                        //if ((string)temp.Code == Code)
                        {
                            return new NAVAddressesViewModel()
                            {
                                Code = (string)temp.Code,
                                Name = (string)temp.Name,
                                Address = (string)temp.Address,
                                ZipCode = (string)temp.ZipCode,
                                City = (string)temp.City,
                                Contact = (string)temp.Contact
                            };
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static NAVAddressesViewModel GetByClientAndCode(string Client, string Code, string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                if (string.IsNullOrEmpty(Code))
                {
                    return null;
                }
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoCliente", Client)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017EnderecosEnvio @DBName, @CompanyName, @NoCliente", parameters);
                    string customerNo = string.Empty;
                    foreach (dynamic temp in data)
                    {
                        customerNo = (string)temp.Code;
                        if (!string.IsNullOrEmpty(customerNo) && customerNo == Code)
                        //if ((string)temp.Code == Code)
                        {
                            return new NAVAddressesViewModel()
                            {
                                Code = (string)temp.Code,
                                Name = (string)temp.Name,
                                Address = (string)temp.Address,
                                ZipCode = (string)temp.ZipCode,
                                City = (string)temp.City,
                                Contact = (string)temp.Contact
                            };
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
