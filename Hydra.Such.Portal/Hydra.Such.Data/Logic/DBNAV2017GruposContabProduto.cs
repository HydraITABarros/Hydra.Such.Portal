using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using Hydra.Such.Data.ViewModel.ProjectView;

namespace Hydra.Such.Data.Logic
{
   public class DBNAV2017GruposContabilisticos
    {
        public static List<NAVGroupContViewModel> GetGruposContabProduto(string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                List<NAVGroupContViewModel> result = new List<NAVGroupContViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[] {
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName)
                    };
                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017GruposContabProduto @DBName, @CompanyName", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVGroupContViewModel()
                        {
                            Code = (string) temp.Code,
                            Description = (string)temp.Description
                        });
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<NAVGroupContViewModel> GetVATProductsPostingGroups(string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                List<NAVGroupContViewModel> result = new List<NAVGroupContViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[] {
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName)
                    };
                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017GruposContabIVAProduto @DBName, @CompanyName", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVGroupContViewModel()
                        {
                            Code = (string)temp.Code,
                            Description = (string)temp.Description
                        });
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<NAVGroupContViewModel> GetVATBusinessPostingGroups(string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                List<NAVGroupContViewModel> result = new List<NAVGroupContViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[] {
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName)
                    };
                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017GruposContabIVANegocio @DBName, @CompanyName", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVGroupContViewModel()
                        {
                            Code = (string)temp.Code,
                            Description = (string)temp.Description
                        });
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
