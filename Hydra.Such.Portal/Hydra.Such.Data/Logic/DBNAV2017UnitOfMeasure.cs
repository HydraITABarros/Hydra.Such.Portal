using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;

namespace Hydra.Such.Data.Logic
{
    public class DBNAV2017UnitOfMeasure
    {
        public static List<NAVUnitOfMeasureViewModel> GetAll(string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                List<NAVUnitOfMeasureViewModel> result = new List<NAVUnitOfMeasureViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017UnidadesMedida @DBName, @CompanyName", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVUnitOfMeasureViewModel()
                        {
                            code = (string)temp.Code,
                            description = (string)temp.Description
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

        public static List<NAVUnitOfMeasureViewModel> GetAllByProduct(string NAVDatabaseName, string NAVCompanyName)
        {
            try
            {
                List<NAVUnitOfMeasureViewModel> result = new List<NAVUnitOfMeasureViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017UnidadesMedidaPorProduto @DBName, @CompanyName", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVUnitOfMeasureViewModel()
                        {
                            code = (string)temp.Code,
                            description = (string)temp.Description
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
