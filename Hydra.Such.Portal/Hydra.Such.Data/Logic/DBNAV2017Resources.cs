using Hydra.Such.Data.Database;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hydra.Such.Data.Logic
{
    public class DBNAV2017Resources
    {
        public static List<NAVResourcesViewModel> GetAllResources(string NAVDatabaseName, string NAVCompanyName, string resourceNo, string filterArea, int resourceType, string contabGroup)
        {
            try
            {
                List<NAVResourcesViewModel> result = new List<NAVResourcesViewModel>();
                using (var ctx = new SuchDBContextExtention())
                {
                    var parameters = new[]{
                        new SqlParameter("@DBName", NAVDatabaseName),
                        new SqlParameter("@CompanyName", NAVCompanyName),
                        new SqlParameter("@NoProduto", resourceNo),
                        new SqlParameter("@FiltroArea", resourceNo),
                        new SqlParameter("@TipoRecurso", resourceNo),
                        new SqlParameter("@GrupoContabProd", resourceNo)
                    };

                    IEnumerable<dynamic> data = ctx.execStoredProcedure("exec NAV2017Recursos @DBName, @CompanyName, @NoProduto, @FiltroArea, @TipoRecurso, @GrupoContabProd", parameters);

                    foreach (dynamic temp in data)
                    {
                        result.Add(new NAVResourcesViewModel()
                        {
                            Code = (string)temp.No_,
                            Name = (string)temp.Name,
                            MeasureUnit = (string)temp.Base_Unit_of_Measure
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