using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public static class DBNAV2017Projects
    {
        public static List<NAVProjectsViewModel> GetAll(string NAVDatabaseName, string NAVCompanyName, string ProjectNo)
        {
            try
            {
                List<NAVProjectsViewModel> result = new List<NAVProjectsViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@ProjectNo", ProjectNo)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017Projetos @DBName, @CompanyName, @ProjectNo", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVProjectsViewModel()
                        {
                            No = (string)temp.No_,
                            Description = (string)temp.Description,
                            CustomerNo = (string)temp.BillToCustomerNo_,
                            CustomerName = (string)temp.BillToCustomerName,
                            GlobalDimension1 = (string)temp.GlobalDimension1Code,
                            GlobalDimension2 = (string)temp.GlobalDimension2Code,
                            AreaCode = (string)temp.AreaCode,
                            RegionCode = (string)temp.RegionCode,
                            CenterResponsibilityCode = (string)temp.CenterResponsibilityCode
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

        public static decimal? GetTotalInvoiceValue(string NAVDatabaseName, string NAVCompanyName, string projectNo)
        {
            try
            {
                decimal result = 0;
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@ProjectNo", projectNo)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017ValorFaturasDoProjeto @DBName, @CompanyName, @ProjectNo", parameters);
                    
                    foreach (dynamic temp in data)
                    {
                        result += temp.ValorFaturas.Equals(DBNull.Value) ? 0 : (decimal)temp.ValorFaturas;
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
